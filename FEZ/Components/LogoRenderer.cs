// Type: FezGame.Components.LogoRenderer
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Services;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FezGame.Components
{
  internal class LogoRenderer : DrawableGameComponent
  {
    private Texture2D TrapdoorLogo;
    private SpriteBatch SpriteBatch;

    [ServiceDependency]
    public IContentManagerProvider CMProvider { private get; set; }

    public LogoRenderer(Game game)
      : base(game)
    {
    }

    public override void Initialize()
    {
      base.Initialize();
      this.TrapdoorLogo = this.CMProvider.Global.Load<Texture2D>("Other Textures/splash/trapdoor");
      this.SpriteBatch = new SpriteBatch(this.GraphicsDevice);
    }

    public override void Draw(GameTime gameTime)
    {
      base.Draw(gameTime);
      Vector2 vector2 = FezMath.Round(new Vector2((float) this.GraphicsDevice.Viewport.Width, (float) this.GraphicsDevice.Viewport.Height) / 2f);
      this.GraphicsDevice.Clear(Color.White);
      GraphicsDeviceExtensions.BeginPoint(this.SpriteBatch);
      this.SpriteBatch.Draw(this.TrapdoorLogo, vector2 - FezMath.Round(new Vector2((float) this.TrapdoorLogo.Width, (float) this.TrapdoorLogo.Height) / 2f), new Color(1f, 1f, 1f, 1f));
      this.SpriteBatch.End();
    }
  }
}
