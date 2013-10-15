// Type: Microsoft.Xna.Framework.Graphics.GraphicsAdapter
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using OpenTK;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Microsoft.Xna.Framework.Graphics
{
  public sealed class GraphicsAdapter : IDisposable
  {
    private static ReadOnlyCollection<GraphicsAdapter> adapters;
    private DisplayModeCollection supportedDisplayModes;

    public DisplayMode CurrentDisplayMode
    {
      get
      {
        return new DisplayMode(DisplayDevice.Default.Width, DisplayDevice.Default.Height, (int) DisplayDevice.Default.RefreshRate, SurfaceFormat.Color);
      }
    }

    public static GraphicsAdapter DefaultAdapter
    {
      get
      {
        return GraphicsAdapter.Adapters[0];
      }
    }

    public static ReadOnlyCollection<GraphicsAdapter> Adapters
    {
      get
      {
        if (GraphicsAdapter.adapters == null)
          GraphicsAdapter.adapters = new ReadOnlyCollection<GraphicsAdapter>((IList<GraphicsAdapter>) new GraphicsAdapter[1]
          {
            new GraphicsAdapter()
          });
        return GraphicsAdapter.adapters;
      }
    }

    public string Description
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public int DeviceId
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public Guid DeviceIdentifier
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public string DeviceName
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public string DriverDll
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public Version DriverVersion
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public bool IsDefaultAdapter
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public bool IsWideScreen
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public IntPtr MonitorHandle
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public int Revision
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public int SubSystemId
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public DisplayModeCollection SupportedDisplayModes
    {
      get
      {
        if (this.supportedDisplayModes == null)
        {
          List<DisplayMode> setmodes = new List<DisplayMode>((IEnumerable<DisplayMode>) new DisplayMode[1]
          {
            this.CurrentDisplayMode
          });
          IList<DisplayDevice> availableDisplays = DisplayDevice.AvailableDisplays;
          if (availableDisplays.Count > 0)
          {
            setmodes.Clear();
            foreach (DisplayDevice displayDevice in (IEnumerable<DisplayDevice>) availableDisplays)
            {
              foreach (DisplayResolution displayResolution in (IEnumerable<DisplayResolution>) displayDevice.AvailableResolutions)
              {
                SurfaceFormat format = SurfaceFormat.Color;
                switch (displayResolution.BitsPerPixel)
                {
                  case 8:
                    format = SurfaceFormat.Bgr565;
                    break;
                  case 16:
                    format = SurfaceFormat.Bgr565;
                    break;
                  case 32:
                    format = SurfaceFormat.Color;
                    break;
                }
                if (format == SurfaceFormat.Color)
                  setmodes.Add(new DisplayMode(displayResolution.Width, displayResolution.Height, (int) displayResolution.RefreshRate, format));
              }
            }
          }
          this.supportedDisplayModes = new DisplayModeCollection(setmodes);
        }
        return this.supportedDisplayModes;
      }
    }

    public int VendorId
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    internal GraphicsAdapter()
    {
    }

    public void Dispose()
    {
    }

    public bool QueryRenderTargetFormat(GraphicsProfile graphicsProfile, SurfaceFormat format, DepthFormat depthFormat, int multiSampleCount, out SurfaceFormat selectedFormat, out DepthFormat selectedDepthFormat, out int selectedMultiSampleCount)
    {
      throw new NotImplementedException();
    }
  }
}
