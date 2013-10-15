// Type: Microsoft.Xna.Framework.Input.Touch.TouchPanelCapabilities
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

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
