// Type: Microsoft.Xna.Framework.Graphics.IGraphicsDeviceService
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System;

namespace Microsoft.Xna.Framework.Graphics
{
  public interface IGraphicsDeviceService
  {
    GraphicsDevice GraphicsDevice { get; }

    event EventHandler<EventArgs> DeviceCreated;

    event EventHandler<EventArgs> DeviceDisposing;

    event EventHandler<EventArgs> DeviceReset;

    event EventHandler<EventArgs> DeviceResetting;
  }
}
