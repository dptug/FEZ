// Type: FezGame.Structure.CreditsEntry
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FezGame.Structure
{
  internal class CreditsEntry
  {
    public Color Color = Color.White;
    public bool IsTitle;
    public bool IsSubtitle;
    public Texture2D Image;
    public string Text;
    public Vector2 Size;
  }
}
