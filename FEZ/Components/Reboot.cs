// Type: FezGame.Components.Reboot
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Components;
using FezEngine.Effects;
using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FezGame.Components
{
  public class Reboot : DrawableGameComponent
  {
    private readonly string ToLevel = "GOMEZ_INTERIOR_3D";
    private const float WaitTime = 3f;
    private const float TimeUntilLogo = 1f;
    private const float TimeUntilBootup = 4f;
    private Texture2D BootTexture;
    private Texture2D LaserCheckTexture;
    private RebootPOSTEffect effect;
    private TimeSpan SinceCreated;
    private SoundEffect RebootSound;
    private bool hasPlayedSound;

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public ITargetRenderingManager TargetRenderer { private get; set; }

    [ServiceDependency]
    public IGameLevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public ITimeManager TimeManager { private get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { private get; set; }

    public Reboot(Game game, string toLevel)
      : base(game)
    {
      if (toLevel != null)
        this.ToLevel = toLevel;
      this.DrawOrder = 1005;
    }

    protected override void LoadContent()
    {
      ContentManager contentManager = this.CMProvider.Get(CM.Reboot);
      this.BootTexture = contentManager.Load<Texture2D>("Other Textures/reboot/boot");
      this.LaserCheckTexture = contentManager.Load<Texture2D>("Other Textures/reboot/lasercheck");
      this.RebootSound = contentManager.Load<SoundEffect>("Sounds/Intro/Reboot");
      this.effect = new RebootPOSTEffect();
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      this.effect.Dispose();
      this.CMProvider.Dispose(CM.Reboot);
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Loading || this.GameState.Paused || this.SinceCreated.TotalSeconds <= 7.0)
        return;
      if (this.GameState.InCutscene && Intro.Instance != null)
        ServiceHelper.RemoveComponent<Intro>(Intro.Instance);
      Intro intro = new Intro(this.Game)
      {
        Fake = true,
        FakeLevel = this.ToLevel,
        Glitch = true
      };
      this.TimeManager.TimeFactor = this.TimeManager.DefaultTimeFactor;
      this.TimeManager.CurrentTime = DateTime.Now;
      ServiceHelper.AddComponent((IGameComponent) intro);
      Waiters.Wait(0.100000001490116, (Action) (() => ServiceHelper.RemoveComponent<Reboot>(this)));
      this.Enabled = false;
    }

    public override void Draw(GameTime gameTime)
    {
      this.SinceCreated += gameTime.ElapsedGameTime;
      this.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
      if (this.SinceCreated.TotalSeconds > 3.0)
      {
        if (!this.hasPlayedSound)
        {
          SoundEffectExtensions.Emit(this.RebootSound);
          this.hasPlayedSound = true;
        }
        float num1 = (float) this.SinceCreated.TotalSeconds - 3f;
        float viewScale = SettingsManager.GetViewScale(this.GraphicsDevice);
        float num2 = (float) this.GraphicsDevice.Viewport.Width;
        float num3 = (float) this.GraphicsDevice.Viewport.Height;
        float num4 = (float) this.BootTexture.Width;
        float num5 = (float) this.BootTexture.Height;
        this.TargetRenderer.DrawFullscreen((Texture) this.BootTexture, new Matrix(1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f, 0.0f, 0.0f, -0.5f, -0.5f, 1f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f) * new Matrix((float) ((double) num2 / (double) num4 / 2.0) / viewScale, 0.0f, 0.0f, 0.0f, 0.0f, (float) ((double) num3 / (double) num5 / 2.0) / viewScale, 0.0f, 0.0f, 0.0f, 0.0f, 1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f) * new Matrix(1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f, 0.0f, 0.0f, 0.56f, 0.5f, 1f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f));
        float num6 = num1 / 4f;
        this.effect.PseudoWorld = new Matrix(1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f, 0.0f, 0.0f, (float) (-((double) num6 < 0.200000002980232 ? 0.219999998807907 : ((double) num6 < 0.400000005960464 ? 0.293000012636185 : ((double) num6 < 0.600000023841858 ? 0.361000001430511 : ((double) num6 < 0.699999988079071 ? 0.527999997138977 : ((double) num6 < 0.800000011920929 ? 0.699999988079071 : 1.0))))) * 2.0), 0.0f, 1f);
        this.TargetRenderer.DrawFullscreen((BaseEffect) this.effect, Color.Black);
        if ((double) num1 <= 1.0)
          return;
        float num7 = (float) this.LaserCheckTexture.Width;
        float num8 = (float) this.LaserCheckTexture.Height;
        this.TargetRenderer.DrawFullscreen((Texture) this.LaserCheckTexture, new Matrix(1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f, 0.0f, 0.0f, -0.5f, -0.5f, 1f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f) * new Matrix((float) ((double) num2 / (double) num7 / 2.0) / viewScale, 0.0f, 0.0f, 0.0f, 0.0f, (float) ((double) num3 / (double) num8 / 2.0) / viewScale, 0.0f, 0.0f, 0.0f, 0.0f, 1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f) * new Matrix(1f, 0.0f, 0.0f, 0.0f, 0.0f, 1f, 0.0f, 0.0f, -1.15f, 1f, 1f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f));
      }
      else
        this.TargetRenderer.DrawFullscreen(Color.Black);
    }
  }
}
