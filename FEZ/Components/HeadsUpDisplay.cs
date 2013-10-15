// Type: FezGame.Components.HeadsUpDisplay
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Components;
using FezEngine.Services;
using FezEngine.Tools;
using FezGame;
using FezGame.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FezGame.Components
{
  public class HeadsUpDisplay : DrawableGameComponent
  {
    private static readonly TimeSpan HudVisibleTime = TimeSpan.FromSeconds(3.0);
    private static readonly TimeSpan HudFadeInTime = TimeSpan.FromSeconds(0.25);
    private static readonly TimeSpan HudFadeOutTime = TimeSpan.FromSeconds(1.0);
    private SpriteBatch spriteBatch;
    private Texture2D keyIcon;
    private Texture2D cubeIcon;
    private Texture2D antiIcon;
    private Texture2D[] smallCubes;
    private TimeSpan sinceHudUpdate;
    private GlyphTextRenderer tr;
    private string cubeShardsText;
    private string antiText;
    private string keysText;
    private TimeSpan sinceSave;

    [ServiceDependency]
    public IGameCameraManager CameraManager { private get; set; }

    [ServiceDependency]
    public ILevelManager LevelManager { private get; set; }

    [ServiceDependency(Optional = true)]
    public IDebuggingBag DebuggingBag { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public IFontManager Fonts { private get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { get; set; }

    static HeadsUpDisplay()
    {
    }

    public HeadsUpDisplay(Game game)
      : base(game)
    {
    }

    public override void Initialize()
    {
      this.DrawOrder = 10000;
      this.GameState.HudElementChanged += new Action(this.RefreshHud);
      this.RefreshHud();
      base.Initialize();
    }

    private void RefreshHud()
    {
      if (this.sinceHudUpdate.Ticks > HeadsUpDisplay.HudFadeInTime.Ticks + HeadsUpDisplay.HudVisibleTime.Ticks + HeadsUpDisplay.HudFadeOutTime.Ticks)
        this.sinceHudUpdate = TimeSpan.Zero;
      else if (this.sinceHudUpdate.Ticks > HeadsUpDisplay.HudFadeInTime.Ticks + HeadsUpDisplay.HudVisibleTime.Ticks * 4L / 5L)
        this.sinceHudUpdate = TimeSpan.FromTicks(HeadsUpDisplay.HudFadeInTime.Ticks + HeadsUpDisplay.HudVisibleTime.Ticks * 4L / 5L);
      this.cubeShardsText = this.GameState.SaveData.CubeShards.ToString();
      this.antiText = this.GameState.SaveData.SecretCubes.ToString();
      this.keysText = this.GameState.SaveData.Keys.ToString();
    }

    protected override void LoadContent()
    {
      this.tr = new GlyphTextRenderer(this.Game);
      this.keyIcon = this.CMProvider.Global.Load<Texture2D>("Other Textures/hud/KEY_CUBE");
      this.cubeIcon = this.CMProvider.Global.Load<Texture2D>("Other Textures/hud/NORMAL_CUBE");
      this.antiIcon = this.CMProvider.Global.Load<Texture2D>("Other Textures/hud/ANTI_CUBE");
      this.smallCubes = new Texture2D[8];
      for (int index = 0; index < 8; ++index)
        this.smallCubes[index] = this.CMProvider.Global.Load<Texture2D>("Other Textures/smallcubes/sc_" + (object) (index + 1));
      this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
    }

    public override void Update(GameTime gameTime)
    {
      this.sinceHudUpdate += gameTime.ElapsedGameTime;
      if (this.GameState.InMenuCube)
        this.RefreshHud();
      if (this.GameState.Saving)
      {
        if (this.sinceSave.Ticks < HeadsUpDisplay.HudFadeInTime.Ticks)
          this.sinceSave += gameTime.ElapsedGameTime;
      }
      else if (this.sinceSave.Ticks > 0L)
        this.sinceSave -= gameTime.ElapsedGameTime;
      base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
      if (this.GameState.Loading || this.GameState.InCutscene || this.GameState.HideHUD && !this.GameState.Paused)
        return;
      GraphicsDeviceExtensions.SetBlendingMode(this.GraphicsDevice, BlendingMode.Alphablending);
      GraphicsDeviceExtensions.BeginPoint(this.spriteBatch);
      if (!Fez.LongScreenshot)
      {
        this.DrawHud();
        this.DrawDebugging();
      }
      this.spriteBatch.End();
    }

    private void DrawHud()
    {
      Vector2 position = new Vector2(50f, 58f);
      float num1 = FezMath.Saturate((float) this.sinceHudUpdate.Ticks / (float) HeadsUpDisplay.HudFadeInTime.Ticks);
      float num2 = FezMath.Saturate((float) (this.sinceHudUpdate.Ticks - HeadsUpDisplay.HudVisibleTime.Ticks) / (float) HeadsUpDisplay.HudFadeOutTime.Ticks);
      if ((double) num2 == 1.0)
        return;
      Color color = new Color(1f, 1f, 1f, Easing.EaseOut((double) num1, EasingType.Quadratic) - Easing.EaseOut((double) num2, EasingType.Quadratic));
      SpriteFont font = Culture.IsCJK ? this.Fonts.Big : this.Fonts.Small;
      float num3 = Culture.IsCJK ? this.Fonts.BigFactor * 0.625f : this.Fonts.SmallFactor;
      int num4 = FezMath.Round((double) SettingsManager.GetViewScale(this.GraphicsDevice));
      float scale = num3 * (float) num4;
      Vector2 vector2_1 = new Vector2(60f, 32f);
      if (this.GameState.SaveData.CollectedParts > 0)
      {
        int num5 = Math.Min(this.GameState.SaveData.CollectedParts, 8);
        this.spriteBatch.Draw(this.smallCubes[num5 - 1], position, new Rectangle?(), color, 0.0f, Vector2.Zero, (float) num4, SpriteEffects.None, 0.0f);
        string text = num5.ToString();
        Vector2 vector2_2 = this.Fonts.Small.MeasureString(text) * scale;
        this.tr.DrawShadowedText(this.spriteBatch, font, text, position + vector2_1 * (float) num4 - vector2_2 * Vector2.UnitY / 2f + this.Fonts.TopSpacing * this.Fonts.SmallFactor * Vector2.UnitY, color, scale, Color.Black, 1f, 1f);
        position.Y += (float) (51 * num4);
      }
      this.spriteBatch.Draw(this.cubeIcon, position, new Rectangle?(), color, 0.0f, Vector2.Zero, (float) num4, SpriteEffects.None, 0.0f);
      Vector2 vector2_3 = this.Fonts.Small.MeasureString(this.cubeShardsText) * scale;
      this.tr.DrawShadowedText(this.spriteBatch, font, this.cubeShardsText, position + vector2_1 * (float) num4 - vector2_3 * Vector2.UnitY / 2f + this.Fonts.TopSpacing * this.Fonts.SmallFactor * Vector2.UnitY, color, scale, Color.Black, 1f, 1f);
      position.Y += (float) (51 * num4);
      if (this.GameState.SaveData.SecretCubes > 0)
      {
        this.spriteBatch.Draw(this.antiIcon, position, new Rectangle?(), color, 0.0f, Vector2.Zero, (float) num4, SpriteEffects.None, 0.0f);
        Vector2 vector2_2 = this.Fonts.Small.MeasureString(this.antiText) * scale;
        this.tr.DrawShadowedText(this.spriteBatch, font, this.antiText, position + vector2_1 * (float) num4 - vector2_2 * Vector2.UnitY / 2f + this.Fonts.TopSpacing * this.Fonts.SmallFactor * Vector2.UnitY, color, scale, Color.Black, 1f, 1f);
        position.Y += (float) (51 * num4);
      }
      this.spriteBatch.Draw(this.keyIcon, position, new Rectangle?(), color, 0.0f, Vector2.Zero, (float) num4, SpriteEffects.None, 0.0f);
      Vector2 vector2_4 = this.Fonts.Small.MeasureString(this.keysText) * scale;
      this.tr.DrawShadowedText(this.spriteBatch, font, this.keysText, position + vector2_1 * (float) num4 - vector2_4 * Vector2.UnitY / 2f + this.Fonts.TopSpacing * this.Fonts.SmallFactor * Vector2.UnitY, color, scale, Color.Black, 1f, 1f);
    }

    private void DrawDebugging()
    {
    }
  }
}
