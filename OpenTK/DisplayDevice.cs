// Type: OpenTK.DisplayDevice
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK.Graphics;
using OpenTK.Platform;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace OpenTK
{
  public class DisplayDevice
  {
    private DisplayResolution current_resolution = new DisplayResolution();
    private List<DisplayResolution> available_resolutions = new List<DisplayResolution>();
    private static readonly object display_lock = new object();
    private static IDisplayDeviceDriver implementation = Factory.Default.CreateDisplayDeviceDriver();
    private bool primary;
    private Rectangle bounds;
    private DisplayResolution original_resolution;
    private IList<DisplayResolution> available_resolutions_readonly;
    internal object Id;
    private static DisplayDevice primary_display;

    public Rectangle Bounds
    {
      get
      {
        return this.bounds;
      }
      internal set
      {
        this.bounds = value;
        this.current_resolution.Height = this.bounds.Height;
        this.current_resolution.Width = this.bounds.Width;
      }
    }

    public int Width
    {
      get
      {
        return this.current_resolution.Width;
      }
    }

    public int Height
    {
      get
      {
        return this.current_resolution.Height;
      }
    }

    public int BitsPerPixel
    {
      get
      {
        return this.current_resolution.BitsPerPixel;
      }
      internal set
      {
        this.current_resolution.BitsPerPixel = value;
      }
    }

    public float RefreshRate
    {
      get
      {
        return this.current_resolution.RefreshRate;
      }
      internal set
      {
        this.current_resolution.RefreshRate = value;
      }
    }

    public bool IsPrimary
    {
      get
      {
        return this.primary;
      }
      internal set
      {
        if (value && DisplayDevice.primary_display != null && DisplayDevice.primary_display != this)
          DisplayDevice.primary_display.IsPrimary = false;
        lock (DisplayDevice.display_lock)
        {
          this.primary = value;
          if (!value)
            return;
          DisplayDevice.primary_display = this;
        }
      }
    }

    public IList<DisplayResolution> AvailableResolutions
    {
      get
      {
        return this.available_resolutions_readonly;
      }
      internal set
      {
        this.available_resolutions = (List<DisplayResolution>) value;
        this.available_resolutions_readonly = (IList<DisplayResolution>) this.available_resolutions.AsReadOnly();
      }
    }

    [Obsolete("Use GetDisplay(DisplayIndex) instead.")]
    public static IList<DisplayDevice> AvailableDisplays
    {
      get
      {
        List<DisplayDevice> list = new List<DisplayDevice>();
        for (int index = 0; index < 6; ++index)
        {
          DisplayDevice display = DisplayDevice.GetDisplay((DisplayIndex) index);
          if (display != null)
            list.Add(display);
        }
        return (IList<DisplayDevice>) list.AsReadOnly();
      }
    }

    public static DisplayDevice Default
    {
      get
      {
        return DisplayDevice.implementation.GetDisplay(DisplayIndex.Primary);
      }
    }

    static DisplayDevice()
    {
    }

    internal DisplayDevice()
    {
      this.available_resolutions_readonly = (IList<DisplayResolution>) this.available_resolutions.AsReadOnly();
    }

    internal DisplayDevice(DisplayResolution currentResolution, bool primary, IEnumerable<DisplayResolution> availableResolutions, Rectangle bounds, object id)
      : this()
    {
      this.current_resolution = currentResolution;
      this.IsPrimary = primary;
      this.available_resolutions.AddRange(availableResolutions);
      this.bounds = bounds == Rectangle.Empty ? currentResolution.Bounds : bounds;
      this.Id = id;
    }

    public DisplayResolution SelectResolution(int width, int height, int bitsPerPixel, float refreshRate)
    {
      DisplayResolution resolution = this.FindResolution(width, height, bitsPerPixel, refreshRate);
      if (resolution == (DisplayResolution) null)
        resolution = this.FindResolution(width, height, bitsPerPixel, 0.0f);
      if (resolution == (DisplayResolution) null)
        resolution = this.FindResolution(width, height, 0, 0.0f);
      if (resolution == (DisplayResolution) null)
        return this.current_resolution;
      else
        return resolution;
    }

    public void ChangeResolution(DisplayResolution resolution)
    {
      if (resolution == (DisplayResolution) null)
        this.RestoreResolution();
      if (resolution == this.current_resolution)
        return;
      if (!DisplayDevice.implementation.TryChangeResolution(this, resolution))
        throw new GraphicsModeException(string.Format("Device {0}: Failed to change resolution to {1}.", (object) this, (object) resolution));
      if (this.original_resolution == (DisplayResolution) null)
        this.original_resolution = this.current_resolution;
      this.current_resolution = resolution;
    }

    public void ChangeResolution(int width, int height, int bitsPerPixel, float refreshRate)
    {
      this.ChangeResolution(this.SelectResolution(width, height, bitsPerPixel, refreshRate));
    }

    public void RestoreResolution()
    {
      if (!(this.original_resolution != (DisplayResolution) null))
        return;
      if (!DisplayDevice.implementation.TryRestoreResolution(this))
        throw new GraphicsModeException(string.Format("Device {0}: Failed to restore resolution.", (object) this));
      this.current_resolution = this.original_resolution;
      this.original_resolution = (DisplayResolution) null;
    }

    public static DisplayDevice GetDisplay(DisplayIndex index)
    {
      return DisplayDevice.implementation.GetDisplay(index);
    }

    private DisplayResolution FindResolution(int width, int height, int bitsPerPixel, float refreshRate)
    {
      return this.available_resolutions.Find((Predicate<DisplayResolution>) (test =>
      {
        if ((width <= 0 || width != test.Width) && width != 0 || (height <= 0 || height != test.Height) && height != 0 || (bitsPerPixel <= 0 || bitsPerPixel != test.BitsPerPixel) && bitsPerPixel != 0)
          return false;
        if ((double) refreshRate <= 0.0 || (double) Math.Abs(refreshRate - test.RefreshRate) >= 1.0)
          return (double) refreshRate == 0.0;
        else
          return true;
      }));
    }

    public override string ToString()
    {
      return string.Format("{0}: {1} ({2} modes available)", this.IsPrimary ? (object) "Primary" : (object) "Secondary", (object) this.Bounds.ToString(), (object) this.available_resolutions.Count);
    }
  }
}
