// Type: FezGame.Components.FakeIntro
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Services;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FezGame.Components
{
  internal class FakeIntro : DrawableGameComponent
  {
    private Texture2D PolyLogo;
    private SpriteBatch SpriteBatch;
    private float logoAlpha;
    private float whiteAlpha;
    private float sinceStarted;

    [ServiceDependency]
    public ITargetRenderingManager TRM { get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { protected get; set; }

    public FakeIntro(Game game)
      : base(game)
    {
      this.DrawOrder = 1000;
    }

    protected override void LoadContent()
    {
      this.PolyLogo = this.CMProvider.Global.Load<Texture2D>("Other Textures/splash/Polytron Logo");
      this.SpriteBatch = new SpriteBatch(this.GraphicsDevice);
    }

    public override void Draw(GameTime gameTime)
    {
      this.sinceStarted += (float) gameTime.ElapsedGameTime.TotalSeconds;
      this.logoAlpha = (float) (((double) this.sinceStarted - 1.0) / 1.0);
      this.whiteAlpha = 1f;
      if ((double) this.sinceStarted > 3.0)
        this.logoAlpha = FezMath.Saturate((float) (1.0 - ((double) this.sinceStarted - 3.0) / 0.5));
      if ((double) this.sinceStarted > 3.5)
        this.whiteAlpha = FezMath.Saturate((float) (1.0 - ((double) this.sinceStarted - 4.0) / 0.5));
      this.TRM.DrawFullscreen(new Color(1f, 1f, 1f, this.whiteAlpha));
      Vector2 vector2 = FezMath.Round(new Vector2((float) this.GraphicsDevice.Viewport.Width, (float) this.GraphicsDevice.Viewport.Height) / 2f);
      GraphicsDeviceExtensions.BeginPoint(this.SpriteBatch);
      this.SpriteBatch.Draw(this.PolyLogo, vector2 - FezMath.Round(new Vector2((float) this.PolyLogo.Width, (float) this.PolyLogo.Height) / 2f), new Color(1f, 1f, 1f, this.logoAlpha));
      this.SpriteBatch.End();
      if ((double) this.whiteAlpha != 0.0)
        return;
      ServiceHelper.RemoveComponent<FakeIntro>(this);
    }
  }
}
