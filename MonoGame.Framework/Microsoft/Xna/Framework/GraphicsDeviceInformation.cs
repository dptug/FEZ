// Type: Microsoft.Xna.Framework.GraphicsDeviceInformation
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework.Graphics;

namespace Microsoft.Xna.Framework
{
  public class GraphicsDeviceInformation
  {
    public GraphicsAdapter Adapter { get; set; }

    public DeviceType DeviceType { get; set; }

    public PresentationParameters PresentationParameters { get; set; }
  }
}
