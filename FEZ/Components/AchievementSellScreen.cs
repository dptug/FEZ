// Type: FezGame.Components.AchievementSellScreen
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Components;
using FezEngine.Services;
using FezEngine.Structure.Input;
using FezEngine.Tools;
using FezGame.Services;
using FezGame.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FezGame.Components
{
  public class AchievementSellScreen : DrawableGameComponent
  {
    private static readonly TimeSpan TransitionDuration = TimeSpan.FromSeconds(0.5);
    private readonly Achievement achievement;
    private Texture2D black;
    private SpriteBatch SpriteBatch;
    private TimeSpan sinceTransition;
    private Action nextAction;
    private GlyphTextRenderer tr;
    private bool fadeInComplete;

    [ServiceDependency]
    public ISoundManager SoundManager { private get; set; }

    [ServiceDependency]
    public IInputManager InputManager { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public ITargetRenderingManager TargetRenderer { private get; set; }

    [ServiceDependency]
    public IFontManager Fonts { private get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { private get; set; }

    static AchievementSellScreen()
    {
    }

    public AchievementSellScreen(Game game, Achievement achievement)
      : base(game)
    {
      this.achievement = achievement;
      this.UpdateOrder = -1;
      this.DrawOrder = 1002;
    }

    public override void Initialize()
    {
      this.GameState.InCutscene = true;
      this.GameState.DynamicUpgrade += new Action(this.BackToGame);
      base.Initialize();
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      this.GameState.DynamicUpgrade -= new Action(this.BackToGame);
    }

    protected override void LoadContent()
    {
      base.LoadContent();
      this.SpriteBatch = new SpriteBatch(this.GraphicsDevice);
      this.tr = new GlyphTextRenderer(this.Game);
      this.black = this.CMProvider.Global.Load<Texture2D>("Other Textures/FullBlack");
    }

    public override void Update(GameTime gameTime)
    {
      if (this.InputManager.GrabThrow == FezButtonState.Pressed)
        this.UnlockFullGame();
      if (this.InputManager.Jump == FezButtonState.Pressed)
      {
        this.sinceTransition = TimeSpan.Zero;
        this.nextAction = new Action(this.BackToGame);
      }
      if (!(this.sinceTransition < AchievementSellScreen.TransitionDuration))
        return;
      this.sinceTransition += gameTime.ElapsedGameTime;
      if (!(this.sinceTransition >= AchievementSellScreen.TransitionDuration))
        return;
      if (!this.fadeInComplete)
        this.fadeInComplete = true;
      else
        this.nextAction();
    }

    private void BackToGame()
    {
      this.GameState.InCutscene = false;
      this.SoundManager.Resume();
      ServiceHelper.RemoveComponent<AchievementSellScreen>(this);
    }

    private void UnlockFullGame()
    {
    }

    public override void Draw(GameTime gameTime)
    {
      float num1 = FezMath.Saturate((float) this.sinceTransition.Ticks / (float) AchievementSellScreen.TransitionDuration.Ticks);
      Vector2 vector2 = new Vector2((float) this.GraphicsDevice.Viewport.Width, (float) this.GraphicsDevice.Viewport.Height);
      float alpha = !this.fadeInComplete ? num1 : (this.nextAction != null ? 1f - num1 : 1f);
      this.TargetRenderer.DrawFullscreen(new Color(1f, 1f, 1f, alpha * 0.25f));
      GraphicsDeviceExtensions.BeginPoint(this.SpriteBatch);
      this.SpriteBatch.Draw(this.black, new Rectangle(0, (int) ((double) vector2.Y / 2.0 - (double) vector2.Y / 4.0), (int) vector2.X, (int) ((double) vector2.Y / 2.0)), new Color(0.0f, 0.0f, 0.0f, alpha * 0.9f));
      this.tr.DrawCenteredString(this.SpriteBatch, this.Fonts.Big, StaticText.GetString("AchievementInTrialTitle"), new Color(1f, 1f, 1f, alpha), new Vector2(0.0f, (float) ((double) vector2.Y / 2.0 - 100.0)), this.Fonts.BigFactor);
      this.tr.DrawCenteredString(this.SpriteBatch, this.Fonts.Small, StaticText.GetString("AchievementInTrialText"), new Color(1f, 1f, 1f, alpha), new Vector2(0.0f, vector2.Y / 2f), this.Fonts.SmallFactor);
      this.tr.DrawCenteredString(this.SpriteBatch, this.Fonts.Small, StaticText.GetString("AchievementInTrialSellText"), new Color(1f, 1f, 1f, alpha), new Vector2(0.0f, (float) ((double) vector2.Y / 2.0 + 40.0)), this.Fonts.SmallFactor);
      float num2 = this.Fonts.Small.MeasureString(StaticText.GetString("AchievementInTrialResume")).X * this.Fonts.SmallFactor;
      this.tr.DrawShadowedText(this.SpriteBatch, this.Fonts.Small, StaticText.GetString("AchievementInTrialResume"), new Vector2(vector2.X - this.tr.Margin.X - num2, (float) ((double) vector2.Y / 2.0 + 125.0)), new Color(0.5f, 1f, 0.5f, alpha), this.Fonts.SmallFactor);
      this.SpriteBatch.End();
    }
  }
}
