// Type: Microsoft.Xna.Framework.ButtonDefinition
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Microsoft.Xna.Framework
{
  public class ButtonDefinition
  {
    public Texture2D Texture { get; set; }

    public Vector2 Position { get; set; }

    public Buttons Type { get; set; }

    public Rectangle TextureRect { get; set; }
  }
}
