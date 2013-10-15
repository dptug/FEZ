// Type: FezEngine.Effects.InstancedMapEffect
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Effects.Structures;
using FezEngine.Structure;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FezEngine.Effects
{
  public class InstancedMapEffect : BaseEffect, IShaderInstantiatableEffect<Matrix>
  {
    private bool lastWasComplete;
    private readonly SemanticMappedBoolean billboard;
    private readonly SemanticMappedMatrix cameraRotation;
    private readonly SemanticMappedTexture texture;
    private readonly SemanticMappedMatrixArray instanceData;

    public bool Billboard
    {
      set
      {
        this.billboard.Set(value);
      }
    }

    public InstancedMapEffect()
      : base("InstancedMapEffect")
    {
      this.texture = new SemanticMappedTexture(this.effect.Parameters, "BaseTexture");
      this.billboard = new SemanticMappedBoolean(this.effect.Parameters, "Billboard");
      this.cameraRotation = new SemanticMappedMatrix(this.effect.Parameters, "CameraRotation");
      this.instanceData = new SemanticMappedMatrixArray(this.effect.Parameters, "InstanceData");
    }

    public override void Prepare(Mesh mesh)
    {
      base.Prepare(mesh);
      this.texture.Set((Texture) mesh.Texture);
      this.cameraRotation.Set(Matrix.CreateFromQuaternion(this.CameraProvider.Rotation));
    }

    public override void Prepare(Group group)
    {
      base.Prepare(group);
      if (group.CustomData != null && (bool) group.CustomData)
      {
        GraphicsDeviceExtensions.PrepareStencilWrite(this.GraphicsDeviceService.GraphicsDevice, new StencilMask?(StencilMask.Trails));
        this.lastWasComplete = true;
      }
      else
      {
        if (!this.lastWasComplete)
          return;
        GraphicsDeviceExtensions.PrepareStencilRead(this.GraphicsDeviceService.GraphicsDevice, CompareFunction.Always, StencilMask.None);
      }
    }

    public void SetInstanceData(Matrix[] instances)
    {
      this.instanceData.Set(instances);
    }
  }
}
