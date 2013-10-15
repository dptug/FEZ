// Type: Microsoft.Xna.Framework.ThumbStickDefinition
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework.Graphics;

namespace Microsoft.Xna.Framework
{
  public class ThumbStickDefinition
  {
    public Texture2D Texture { get; set; }

    public Vector2 Position { get; set; }

    public Rectangle TextureRect { get; set; }

    internal Vector2 InitialHit { get; set; }

    internal Vector2 Offset { get; set; }
  }
}
