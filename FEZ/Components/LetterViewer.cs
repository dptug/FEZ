// Type: FezGame.Components.LetterViewer
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Components;
using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Structure.Input;
using FezEngine.Tools;
using FezGame.Services;
using FezGame.Structure;
using FezGame.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.Localization;
using System;

namespace FezGame.Components
{
  public class LetterViewer : DrawableGameComponent
  {
    private static readonly Color TextColor = new Color(72, 66, 52, (int) byte.MaxValue);
    private const float FadeSeconds = 0.25f;
    private SoundEffect sLetterAppear;
    private Texture2D letterTexture;
    private GlyphTextRenderer textRenderer;
    private TimeSpan fader;
    private SpriteBatch sb;
    private TimeSpan sinceStarted;
    private readonly string LetterName;
    private string LetterText;
    private int oldLetterCount;
    private LetterViewer.State state;

    public bool IsDisposed { get; private set; }

    [ServiceDependency]
    public ITargetRenderingManager TargetRenderer { private get; set; }

    [ServiceDependency]
    public IInputManager InputManager { private get; set; }

    [ServiceDependency]
    public IPlayerManager PlayerManager { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public IFontManager FontManager { private get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { get; set; }

    static LetterViewer()
    {
    }

    public LetterViewer(Game game, string letter)
      : base(game)
    {
      this.DrawOrder = 100;
      this.LetterName = letter;
    }

    protected override void LoadContent()
    {
      this.sLetterAppear = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/MiscActors/LetterAppear");
      this.letterTexture = this.CMProvider.CurrentLevel.Load<Texture2D>("Other Textures/mail/" + this.LetterName + "_1");
      this.textRenderer = new GlyphTextRenderer(this.Game);
      this.sb = new SpriteBatch(this.GraphicsDevice);
      this.PlayerManager.CanControl = false;
      this.PlayerManager.Action = ActionType.ReadTurnAround;
      this.LetterText = GameText.GetString(this.LetterName);
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      this.sb.Dispose();
      this.PlayerManager.CanControl = true;
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Paused || this.GameState.InMap)
        return;
      this.sinceStarted += gameTime.ElapsedGameTime;
      if (this.state == LetterViewer.State.Wait)
        this.CheckForKeyPress();
      else if (this.state == LetterViewer.State.Out)
      {
        this.fader -= gameTime.ElapsedGameTime;
      }
      else
      {
        this.fader += gameTime.ElapsedGameTime;
        this.CheckForKeyPress();
      }
      if (this.fader.TotalSeconds <= 0.25 && this.fader.TotalSeconds >= 0.0)
        return;
      switch (this.state)
      {
        case LetterViewer.State.In:
          this.state = LetterViewer.State.Wait;
          break;
        case LetterViewer.State.Out:
          this.IsDisposed = true;
          ServiceHelper.RemoveComponent<LetterViewer>(this);
          break;
      }
    }

    private void CheckForKeyPress()
    {
      if (this.InputManager.Jump != FezButtonState.Pressed && this.InputManager.CancelTalk != FezButtonState.Pressed)
        return;
      if (this.sinceStarted.TotalSeconds < 15.0)
        this.sinceStarted = TimeSpan.FromSeconds(15.0);
      else
        this.state = LetterViewer.State.Out;
      this.fader = TimeSpan.FromSeconds(0.25);
    }

    public override void Draw(GameTime gameTime)
    {
      float alpha1 = Easing.EaseOut(FezMath.Saturate(this.fader.TotalSeconds / 0.25), EasingType.Sine);
      float num1 = (float) this.GraphicsDevice.Viewport.Width;
      float num2 = (float) this.GraphicsDevice.Viewport.Height;
      float num3 = (float) this.letterTexture.Width;
      float num4 = (float) this.letterTexture.Height;
      this.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
      float viewScale = SettingsManager.GetViewScale(this.GraphicsDevice);
      Matrix textureMatrix = new Matrix(1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f, 0.0f, 0.0f, -0.5f, -0.5f, 1f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f) * new Matrix((float) ((double) num1 / (double) num3 / 4.0) / viewScale, 0.0f, 0.0f, 0.0f, 0.0f, (float) ((double) num2 / (double) num4 / 4.0) / viewScale, 0.0f, 0.0f, 0.0f, 0.0f, 1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f) * new Matrix(1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f, 0.0f, 0.0f, 0.85f, 0.5f, 1f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f);
      this.TargetRenderer.DrawFullscreen(new Color(0.0f, 0.0f, 0.0f, 0.4f * alpha1));
      this.TargetRenderer.DrawFullscreen((Texture) this.letterTexture, textureMatrix, new Color(1f, 1f, 1f, alpha1));
      GraphicsDeviceExtensions.BeginPoint(this.sb);
      SpriteFont font = Culture.IsCJK ? this.FontManager.Small : this.FontManager.Big;
      float scale = (Culture.IsCJK ? this.FontManager.SmallFactor + 0.05f : this.FontManager.BigFactor) * viewScale;
      string text1 = this.LetterText;
      int num5 = Culture.IsCJK ? 500 : 135;
      string str = WordWrap.Split(text1, font, (float) num5);
      int lineSpacing = font.LineSpacing;
      if (!Culture.IsCJK)
        font.LineSpacing = 14;
      string text2 = str.Substring(0, Math.Min(str.Length, (int) (this.sinceStarted.TotalSeconds * 15.0)));
      if (this.oldLetterCount != this.CountChars(text2))
        SoundEffectExtensions.Emit(this.sLetterAppear);
      this.oldLetterCount = this.CountChars(text2);
      float x = 335f * viewScale;
      float num6 = 176f * viewScale;
      Vector3 vector3 = LetterViewer.TextColor.ToVector3();
      this.textRenderer.DrawString(this.sb, font, text2, new Vector2(x, num6 + scale * this.FontManager.TopSpacing), new Color(vector3.X, vector3.Y, vector3.Z, alpha1), scale);
      if (!Culture.IsCJK)
        font.LineSpacing = lineSpacing;
      float alpha2 = alpha1 * (float) FezMath.Saturate(this.sinceStarted.TotalSeconds - 2.0);
      this.textRenderer.DrawShadowedText(this.sb, this.FontManager.Big, StaticText.GetString("AchievementInTrialResume"), new Vector2(310f * viewScale, (float) (115.0 * (double) viewScale + (double) this.FontManager.TopSpacing * (double) scale)), new Color(0.5f, 1f, 0.5f, alpha2), this.FontManager.BigFactor * viewScale);
      this.sb.End();
    }

    private int CountChars(string text)
    {
      int num = 0;
      for (int index = 0; index < text.Length; ++index)
      {
        if (!char.IsWhiteSpace(text[index]))
          ++num;
      }
      return num;
    }

    private enum State
    {
      In,
      Wait,
      Out,
    }
  }
}
