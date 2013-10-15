// Type: Microsoft.Xna.Framework.Input.Touch.TouchPanelCapabilities
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

namespace Microsoft.Xna.Framework.Input.Touch
{
  public struct TouchPanelCapabilities
  {
    private bool hasPressure;
    private bool isConnected;
    private int maximumTouchCount;
    private bool initialized;

    public bool HasPressure
    {
      get
      {
        return this.hasPressure;
      }
    }

    public bool IsConnected
    {
      get
      {
        return this.isConnected;
      }
    }

    public int MaximumTouchCount
    {
      get
      {
        return this.maximumTouchCount;
      }
    }

    internal void Initialize()
    {
      if (this.initialized)
        return;
      this.initialized = true;
      this.hasPressure = false;
      this.isConnected = true;
      this.maximumTouchCount = 8;
    }
  }
}
