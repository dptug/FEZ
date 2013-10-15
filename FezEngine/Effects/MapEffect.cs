// Type: FezEngine.Effects.MapEffect
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Structure;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FezEngine.Effects
{
  public class MapEffect : BaseEffect
  {
    private bool lastWasComplete;
    private Vector3 lastDiffuse;

    public MapEffect()
      : base("MapEffect")
    {
    }

    public override void Prepare(Group group)
    {
      base.Prepare(group);
      if (group.CustomData != null && (bool) group.CustomData)
      {
        GraphicsDeviceExtensions.PrepareStencilWrite(this.GraphicsDeviceService.GraphicsDevice, new StencilMask?(StencilMask.Trails));
        this.lastWasComplete = true;
      }
      else if (this.lastWasComplete)
        GraphicsDeviceExtensions.PrepareStencilRead(this.GraphicsDeviceService.GraphicsDevice, CompareFunction.Always, StencilMask.None);
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
      if (group.Material == null)
        return;
      this.material.Opacity = group.Mesh.Material.Opacity * group.Material.Opacity;
    }
  }
}
