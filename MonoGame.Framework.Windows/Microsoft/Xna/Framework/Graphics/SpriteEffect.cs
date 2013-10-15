// Type: Microsoft.Xna.Framework.Graphics.SpriteEffect
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using Microsoft.Xna.Framework;

namespace Microsoft.Xna.Framework.Graphics
{
  public class SpriteEffect : Effect
  {
    internal static readonly byte[] Bytecode = Effect.LoadEffectResource("Microsoft.Xna.Framework.Graphics.Effect.Resources.SpriteEffect.ogl.mgfxo");
    private EffectParameter matrixParam;

    static SpriteEffect()
    {
    }

    public SpriteEffect(GraphicsDevice device)
      : base(device, SpriteEffect.Bytecode)
    {
      this.CacheEffectParameters();
    }

    protected SpriteEffect(SpriteEffect cloneSource)
      : base((Effect) cloneSource)
    {
      this.CacheEffectParameters();
    }

    public override Effect Clone()
    {
      return (Effect) new SpriteEffect(this);
    }

    private void CacheEffectParameters()
    {
      this.matrixParam = this.Parameters["MatrixTransform"];
    }

    protected internal override bool OnApply()
    {
      Viewport viewport = this.GraphicsDevice.Viewport;
      this.matrixParam.SetValue(Matrix.CreateTranslation(-0.5f, -0.5f, 0.0f) * Matrix.CreateOrthographicOffCenter(0.0f, (float) viewport.Width, (float) viewport.Height, 0.0f, 0.0f, 1f));
      return false;
    }
  }
}
