// Type: FezEngine.Components.UnsafeAreaOverlayComponent
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FezEngine.Components
{
  public class UnsafeAreaOverlayComponent : DrawableGameComponent
  {
    private Color noActionAreaColor;
    private Color unsafeAreaColor;
    private SpriteBatch spriteBatch;
    private Texture2D texture;
    private Rectangle[] noActionAreaParts;
    private Rectangle[] unsafeAreaParts;

    public Color NoActionAreaColor
    {
      get
      {
        return this.noActionAreaColor;
      }
      set
      {
        this.noActionAreaColor = value;
      }
    }

    public Color UnsafeAreaColor
    {
      get
      {
        return this.unsafeAreaColor;
      }
      set
      {
        this.unsafeAreaColor = value;
      }
    }

    public UnsafeAreaOverlayComponent(Game game)
      : base(game)
    {
      this.DrawOrder = int.MaxValue;
      this.noActionAreaColor = new Color((int) byte.MaxValue, 0, 0, (int) sbyte.MaxValue);
      this.unsafeAreaColor = new Color((int) byte.MaxValue, (int) byte.MaxValue, 0, (int) sbyte.MaxValue);
    }

    protected override void LoadContent()
    {
      this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
      this.texture = new Texture2D(this.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
      this.texture.SetData<Color>(new Color[1]
      {
        Color.White
      });
      int width = this.GraphicsDevice.Viewport.Width;
      int height = this.GraphicsDevice.Viewport.Height;
      int num1 = (int) ((double) width * 0.05);
      int num2 = (int) ((double) height * 0.05);
      this.noActionAreaParts = new Rectangle[4];
      this.noActionAreaParts[0] = new Rectangle(0, 0, width, num2);
      this.noActionAreaParts[1] = new Rectangle(0, height - num2, width, num2);
      this.noActionAreaParts[2] = new Rectangle(0, num2, num1, height - 2 * num2);
      this.noActionAreaParts[3] = new Rectangle(width - num1, num2, num1, height - 2 * num2);
      this.unsafeAreaParts = new Rectangle[4];
      this.unsafeAreaParts[0] = new Rectangle(num1, num2, width - 2 * num1, num2);
      this.unsafeAreaParts[1] = new Rectangle(num1, height - 2 * num2, width - 2 * num1, num2);
      this.unsafeAreaParts[2] = new Rectangle(num1, 2 * num2, num1, height - 4 * num2);
      this.unsafeAreaParts[3] = new Rectangle(width - 2 * num1, 2 * num2, num1, height - 4 * num2);
    }

    public override void Draw(GameTime gameTime)
    {
      this.spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
      for (int index = 0; index < this.noActionAreaParts.Length; ++index)
        this.spriteBatch.Draw(this.texture, this.noActionAreaParts[index], this.noActionAreaColor);
      for (int index = 0; index < this.unsafeAreaParts.Length; ++index)
        this.spriteBatch.Draw(this.texture, this.unsafeAreaParts[index], this.unsafeAreaColor);
      this.spriteBatch.End();
    }
  }
}
