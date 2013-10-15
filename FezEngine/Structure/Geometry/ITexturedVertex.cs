// Type: FezEngine.Structure.Geometry.ITexturedVertex
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FezEngine.Structure.Geometry
{
  public interface ITexturedVertex : IVertex, IVertexType
  {
    Vector2 TextureCoordinate { get; set; }
  }
}
