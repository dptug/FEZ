// Type: Microsoft.Xna.Framework.GamerServices.MonoLiveGuide
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Security.Principal;

namespace Microsoft.Xna.Framework.GamerServices
{
  internal class MonoLiveGuide : DrawableGameComponent
  {
    private Color alphaColor = new Color(128, 128, 128, 0);
    private TimeSpan gt = TimeSpan.Zero;
    private TimeSpan last = TimeSpan.Zero;
    private SpriteBatch spriteBatch;
    private Texture2D signInProgress;
    private byte startalpha;

    public MonoLiveGuide(Game game)
      : base(game)
    {
      this.Enabled = false;
      this.Visible = false;
      this.DrawOrder = int.MaxValue;
    }

    public override void Initialize()
    {
      base.Initialize();
    }

    private Texture2D Circle(GraphicsDevice graphics, int radius)
    {
      int num1 = radius * 2;
      Vector2 vector2_1 = new Vector2((float) radius, (float) radius);
      Texture2D texture2D = new Texture2D(graphics, num1, num1, false, SurfaceFormat.Color);
      Color[] data = new Color[num1 * num1];
      for (int index = 0; index < data.Length; ++index)
      {
        int num2 = (index + 1) % num1;
        int num3 = (index + 1) / num1;
        Vector2 vector2_2 = new Vector2(Math.Abs(vector2_1.X - (float) num2), Math.Abs(vector2_1.Y - (float) num3));
        data[index] = Math.Sqrt((double) vector2_2.X * (double) vector2_2.X + (double) vector2_2.Y * (double) vector2_2.Y) <= (double) radius ? Color.White : Color.Transparent;
      }
      texture2D.SetData<Color>(data);
      return texture2D;
    }

    protected override void LoadContent()
    {
      this.spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);
      this.signInProgress = this.Circle(this.Game.GraphicsDevice, 10);
      base.LoadContent();
    }

    protected override void UnloadContent()
    {
      base.UnloadContent();
    }

    public override void Draw(GameTime gameTime)
    {
      this.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
      Vector2 vector2 = new Vector2((float) (this.Game.GraphicsDevice.PresentationParameters.BackBufferWidth / 2), (float) (this.Game.GraphicsDevice.PresentationParameters.BackBufferHeight - 100));
      Vector2 position = Vector2.Zero;
      this.alphaColor.A = this.startalpha;
      for (int index = 0; index < 12; ++index)
      {
        float num = (float) ((double) index / 12.0 * Math.PI * 2.0);
        position = new Vector2(vector2.X + (float) Math.Cos((double) num) * 50f, vector2.Y + (float) Math.Sin((double) num) * 50f);
        this.spriteBatch.Draw(this.signInProgress, position, this.alphaColor);
        this.alphaColor.A += (byte) 21;
        if ((int) this.alphaColor.A > (int) byte.MaxValue)
          this.alphaColor.A = (byte) 0;
      }
      this.spriteBatch.End();
      base.Draw(gameTime);
    }

    public override void Update(GameTime gameTime)
    {
      if (this.gt == TimeSpan.Zero)
        this.gt = this.last = gameTime.TotalGameTime;
      if ((gameTime.TotalGameTime - this.last).Milliseconds > 100)
      {
        this.last = gameTime.TotalGameTime;
        this.startalpha += (byte) 21;
      }
      if ((gameTime.TotalGameTime - this.gt).TotalSeconds > 5.0)
      {
        string str = WindowsIdentity.GetCurrent().Name;
        if (str.Contains("\\"))
        {
          int startIndex = str.IndexOf("\\") + 1;
          str = str.Substring(startIndex, str.Length - startIndex);
        }
        SignedInGamer signedInGamer = new SignedInGamer();
        signedInGamer.DisplayName = str;
        signedInGamer.Gamertag = str;
        Gamer.SignedInGamers.Add(signedInGamer);
        this.Visible = false;
        this.Enabled = false;
        this.gt = TimeSpan.Zero;
      }
      base.Update(gameTime);
    }
  }
}
