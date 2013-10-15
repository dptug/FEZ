// Type: Microsoft.Xna.Framework.GameWindow
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System;
using System.ComponentModel;

namespace Microsoft.Xna.Framework
{
  public abstract class GameWindow
  {
    private string _title;

    [DefaultValue(false)]
    public abstract bool AllowUserResizing { get; set; }

    public abstract Rectangle ClientBounds { get; }

    public abstract DisplayOrientation CurrentOrientation { get; }

    public abstract IntPtr Handle { get; }

    public abstract string ScreenDeviceName { get; }

    public string Title
    {
      get
      {
        return this._title;
      }
      set
      {
        if (!(this._title != value))
          return;
        this.SetTitle(value);
        this._title = value;
      }
    }

    public event EventHandler<EventArgs> ClientSizeChanged;

    public event EventHandler<EventArgs> OrientationChanged;

    public event EventHandler<EventArgs> ScreenDeviceNameChanged;

    public abstract void BeginScreenDeviceChange(bool willBeFullScreen);

    public abstract void EndScreenDeviceChange(string screenDeviceName, int clientWidth, int clientHeight);

    public void EndScreenDeviceChange(string screenDeviceName)
    {
      this.EndScreenDeviceChange(screenDeviceName, this.ClientBounds.Width, this.ClientBounds.Height);
    }

    protected void OnActivated()
    {
    }

    protected void OnClientSizeChanged()
    {
      if (this.ClientSizeChanged == null)
        return;
      this.ClientSizeChanged((object) this, EventArgs.Empty);
    }

    protected void OnDeactivated()
    {
    }

    protected void OnOrientationChanged()
    {
      if (this.OrientationChanged == null)
        return;
      this.OrientationChanged((object) this, EventArgs.Empty);
    }

    protected void OnPaint()
    {
    }

    protected void OnScreenDeviceNameChanged()
    {
      if (this.ScreenDeviceNameChanged == null)
        return;
      this.ScreenDeviceNameChanged((object) this, EventArgs.Empty);
    }

    protected internal abstract void SetSupportedOrientations(DisplayOrientation orientations);

    protected abstract void SetTitle(string title);
  }
}
