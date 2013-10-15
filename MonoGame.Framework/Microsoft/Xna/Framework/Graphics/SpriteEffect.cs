// Type: Microsoft.Xna.Framework.Graphics.SpriteEffect
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

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
