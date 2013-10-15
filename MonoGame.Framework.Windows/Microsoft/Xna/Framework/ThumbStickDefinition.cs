// Type: Microsoft.Xna.Framework.ThumbStickDefinition
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

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
