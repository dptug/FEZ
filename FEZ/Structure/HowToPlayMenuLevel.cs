// Type: FezGame.Structure.HowToPlayMenuLevel
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Components;
using FezEngine.Services;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FezGame.Structure
{
  internal class HowToPlayMenuLevel : MenuLevel
  {
    private Texture2D HowToPlayImage;

    public ITargetRenderingManager TargetRenderer { private get; set; }

    public override void Initialize()
    {
      this.HowToPlayImage = this.CMProvider.Get(CM.Menu).Load<Texture2D>("Other Textures/how_to_play/howtoplay");
      base.Initialize();
    }

    public override void PostDraw(SpriteBatch batch, SpriteFont font, GlyphTextRenderer tr, float alpha)
    {
      float scale = 4f * SettingsManager.GetViewScale(batch.GraphicsDevice);
      Vector2 vector2_1 = new Vector2((float) batch.GraphicsDevice.Viewport.Width, (float) batch.GraphicsDevice.Viewport.Height) / 2f;
      Vector2 vector2_2 = new Vector2((float) this.HowToPlayImage.Width, (float) this.HowToPlayImage.Height) * scale;
      batch.End();
      GraphicsDeviceExtensions.BeginPoint(batch);
      Vector2 position = vector2_1 - vector2_2 / 2f;
      batch.Draw(this.HowToPlayImage, position, new Rectangle?(), new Color(1f, 1f, 1f, alpha), 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
    }
  }
}
