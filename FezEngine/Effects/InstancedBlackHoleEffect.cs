// Type: FezEngine.Effects.InstancedBlackHoleEffect
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Effects.Structures;
using FezEngine.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FezEngine.Effects
{
  public class InstancedBlackHoleEffect : BaseEffect, IShaderInstantiatableEffect<Matrix>
  {
    private readonly SemanticMappedTexture baseTexture;
    private readonly SemanticMappedBoolean isTextureEnabled;
    private readonly SemanticMappedMatrixArray instanceData;

    public InstancedBlackHoleEffect(bool body)
      : base("InstancedBlackHoleEffect")
    {
      this.baseTexture = new SemanticMappedTexture(this.effect.Parameters, "BaseTexture");
      this.isTextureEnabled = new SemanticMappedBoolean(this.effect.Parameters, "IsTextureEnabled");
      this.instanceData = new SemanticMappedMatrixArray(this.effect.Parameters, "InstanceData");
      this.currentPass = body ? this.currentTechnique.Passes["Body"] : this.currentTechnique.Passes["Fringe"];
      this.SimpleGroupPrepare = this.SimpleMeshPrepare = true;
    }

    public override void Prepare(Mesh mesh)
    {
      this.matrices.WorldViewProjection = this.viewProjection;
      this.isTextureEnabled.Set(mesh.TexturingType == TexturingType.Texture2D);
      this.baseTexture.Set((Texture) mesh.Texture);
      base.Prepare(mesh);
    }

    public void SetInstanceData(Matrix[] instances)
    {
      this.instanceData.Set(instances);
    }
  }
}
