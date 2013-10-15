// Type: FezEngine.Structure.TrileInstanceData
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FezEngine.Structure
{
  public struct TrileInstanceData
  {
    private static readonly VertexDeclaration declaration = new VertexDeclaration(new VertexElement[1]
    {
      new VertexElement(0, VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 1)
    });
    public Vector4 PositionPhi;

    public static int SizeInBytes
    {
      get
      {
        return 16;
      }
    }

    public VertexDeclaration VertexDeclaration
    {
      get
      {
        return TrileInstanceData.declaration;
      }
    }

    static TrileInstanceData()
    {
    }

    public TrileInstanceData(Vector3 position, float phi)
    {
      this.PositionPhi = new Vector4(position, phi);
    }

    public override string ToString()
    {
      return string.Format("{{PositionPhi:{0}}}", (object) this.PositionPhi);
    }

    public bool Equals(TrileInstanceData other)
    {
      return other.PositionPhi.Equals(this.PositionPhi);
    }
  }
}
