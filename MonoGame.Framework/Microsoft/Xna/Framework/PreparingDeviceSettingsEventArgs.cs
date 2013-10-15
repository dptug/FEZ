// Type: Microsoft.Xna.Framework.PreparingDeviceSettingsEventArgs
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System;

namespace Microsoft.Xna.Framework
{
  public class PreparingDeviceSettingsEventArgs : EventArgs
  {
    private GraphicsDeviceInformation _graphicsDeviceInformation;

    public GraphicsDeviceInformation GraphicsDeviceInformation
    {
      get
      {
        return this._graphicsDeviceInformation;
      }
    }

    public PreparingDeviceSettingsEventArgs(GraphicsDeviceInformation graphicsDeviceInformation)
    {
      this._graphicsDeviceInformation = graphicsDeviceInformation;
    }
  }
}
