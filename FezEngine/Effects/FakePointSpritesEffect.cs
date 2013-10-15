// Type: FezEngine.Effects.FakePointSpritesEffect
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Effects.Structures;
using FezEngine.Structure;
using FezEngine.Tools;
using Microsoft.Xna.Framework.Graphics;

namespace FezEngine.Effects
{
  public class FakePointSpritesEffect : BaseEffect
  {
    private readonly SemanticMappedTexture texture;
    private readonly SemanticMappedSingle viewScale;
    private bool groupTextureDirty;

    public FakePointSpritesEffect()
      : base("FakePointSpritesEffect")
    {
      this.texture = new SemanticMappedTexture(this.effect.Parameters, "BaseTexture");
      this.viewScale = new SemanticMappedSingle(this.effect.Parameters, "ViewScale");
    }

    public override BaseEffect Clone()
    {
      return (BaseEffect) new FakePointSpritesEffect();
    }

    public override void Prepare(Mesh mesh)
    {
      base.Prepare(mesh);
      this.texture.Set((Texture) mesh.Texture);
      this.viewScale.Set(SettingsManager.GetViewScale(this.GraphicsDeviceService.GraphicsDevice));
      this.groupTextureDirty = false;
    }

    public override void Prepare(Group group)
    {
      base.Prepare(group);
      if (group.TexturingType == TexturingType.Texture2D)
      {
        this.texture.Set(group.Texture);
        this.groupTextureDirty = true;
      }
      else
      {
        if (!this.groupTextureDirty)
          return;
        this.texture.Set((Texture) group.Mesh.Texture);
      }
    }
  }
}
