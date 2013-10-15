// Type: FezEngine.Effects.ProjectedNodeEffect
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine;
using FezEngine.Effects.Structures;
using FezEngine.Structure;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FezEngine.Effects
{
  public class ProjectedNodeEffect : BaseEffect
  {
    private readonly SemanticMappedTexture texture;
    private readonly SemanticMappedVector2 textureSize;
    private readonly SemanticMappedVector2 viewportSize;
    private readonly SemanticMappedVector3 cubeOffset;
    private readonly SemanticMappedSingle pixPerTrix;
    private readonly SemanticMappedBoolean noTexture;
    private readonly SemanticMappedBoolean complete;
    private Vector3 lastDiffuse;
    private bool lastWasComplete;

    public LightingEffectPass Pass
    {
      set
      {
        this.currentPass = this.currentTechnique.Passes[value == LightingEffectPass.Pre ? 1 : 0];
      }
    }

    public ProjectedNodeEffect()
      : base("ProjectedNodeEffect")
    {
      this.texture = new SemanticMappedTexture(this.effect.Parameters, "BaseTexture");
      this.textureSize = new SemanticMappedVector2(this.effect.Parameters, "TextureSize");
      this.viewportSize = new SemanticMappedVector2(this.effect.Parameters, "ViewportSize");
      this.cubeOffset = new SemanticMappedVector3(this.effect.Parameters, "CubeOffset");
      this.pixPerTrix = new SemanticMappedSingle(this.effect.Parameters, "PixelsPerTrixel");
      this.noTexture = new SemanticMappedBoolean(this.effect.Parameters, "NoTexture");
      this.complete = new SemanticMappedBoolean(this.effect.Parameters, "Complete");
      this.Pass = LightingEffectPass.Main;
    }

    public override BaseEffect Clone()
    {
      return (BaseEffect) new ProjectedNodeEffect();
    }

    public override void Prepare(Mesh mesh)
    {
      base.Prepare(mesh);
      float viewScale = SettingsManager.GetViewScale(this.GraphicsDeviceService.GraphicsDevice);
      Viewport viewport = this.GraphicsDeviceService.GraphicsDevice.Viewport;
      this.matrices.ViewProjection = this.viewProjection;
      this.pixPerTrix.Set((float) ((double) this.CameraProvider.Radius / 45.0 * 18.0) / viewScale);
      this.viewportSize.Set(new Vector2(1280f, 720f));
    }

    public override void Prepare(Group group)
    {
      base.Prepare(group);
      this.cubeOffset.Set(Vector3.Transform(Vector3.Zero, (Matrix) group.WorldMatrix));
      if (this.IgnoreCache || !group.EffectOwner || group.InverseTransposeWorldMatrix.Dirty)
      {
        this.matrices.WorldInverseTranspose = (Matrix) group.InverseTransposeWorldMatrix;
        group.InverseTransposeWorldMatrix.Clean();
      }
      if (group.Material != null)
      {
        if (this.lastDiffuse != group.Material.Diffuse)
        {
          this.material.Diffuse = group.Material.Diffuse;
          this.lastDiffuse = group.Material.Diffuse;
        }
      }
      else
        this.material.Diffuse = group.Mesh.Material.Diffuse;
      if (group.Material != null)
        this.material.Opacity = group.Mesh.Material.Opacity * group.Material.Opacity;
      this.noTexture.Set(group.TexturingType != TexturingType.Texture2D);
      bool flag = (group.CustomData as NodeGroupData).Complete;
      this.complete.Set(flag);
      if (flag)
      {
        GraphicsDeviceExtensions.PrepareStencilWrite(this.GraphicsDeviceService.GraphicsDevice, new StencilMask?(StencilMask.Trails));
        this.lastWasComplete = true;
      }
      else if (this.lastWasComplete)
        GraphicsDeviceExtensions.PrepareStencilRead(this.GraphicsDeviceService.GraphicsDevice, CompareFunction.Always, StencilMask.None);
      if (group.TexturingType != TexturingType.Texture2D)
        return;
      this.texture.Set(group.Texture);
      this.textureSize.Set(new Vector2((float) group.TextureMap.Width, (float) group.TextureMap.Height));
    }
  }
}
