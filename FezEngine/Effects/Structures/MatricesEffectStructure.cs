// Type: FezEngine.Effects.Structures.MatricesEffectStructure
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FezEngine.Effects.Structures
{
  internal class MatricesEffectStructure
  {
    private readonly SemanticMappedMatrix worldViewProjection;
    private readonly SemanticMappedMatrix worldInverseTranspose;
    private readonly SemanticMappedMatrix world;
    private readonly SemanticMappedMatrix textureMatrix;
    private readonly SemanticMappedMatrix viewProjection;

    public Matrix WorldViewProjection
    {
      set
      {
        this.worldViewProjection.Set(value);
      }
    }

    public Matrix WorldInverseTranspose
    {
      set
      {
        this.worldInverseTranspose.Set(value);
      }
    }

    public Matrix ViewProjection
    {
      set
      {
        this.viewProjection.Set(value);
      }
    }

    public Matrix World
    {
      set
      {
        this.world.Set(value);
      }
    }

    public Matrix TextureMatrix
    {
      set
      {
        this.textureMatrix.Set(value);
      }
    }

    public MatricesEffectStructure(EffectParameterCollection parameters)
    {
      this.worldViewProjection = new SemanticMappedMatrix(parameters, "Matrices_WorldViewProjection");
      this.worldInverseTranspose = new SemanticMappedMatrix(parameters, "Matrices_WorldInverseTranspose");
      this.world = new SemanticMappedMatrix(parameters, "Matrices_World");
      this.textureMatrix = new SemanticMappedMatrix(parameters, "Matrices_Texture");
      this.viewProjection = new SemanticMappedMatrix(parameters, "Matrices_ViewProjection");
    }
  }
}
