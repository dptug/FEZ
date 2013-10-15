// Type: FezGame.Components.EndCutscene64.ZoomOut
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Services;
using FezEngine.Tools;
using FezGame.Components;
using FezGame.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FezGame.Components.EndCutscene64
{
  internal class ZoomOut : DrawableGameComponent
  {
    private readonly EndCutscene64Host Host;
    private float StepTime;
    private ZoomOut.State ActiveState;
    private float OldSfxVol;

    [ServiceDependency]
    public ISoundManager SoundManager { private get; set; }

    [ServiceDependency(Optional = true)]
    public IKeyboardStateManager KeyboardState { private get; set; }

    [ServiceDependency]
    public ITargetRenderingManager TargetRenderer { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public ILevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public IGameCameraManager CameraManager { private get; set; }

    public ZoomOut(Game game, EndCutscene64Host host)
      : base(game)
    {
      this.Host = host;
      this.DrawOrder = 1000;
      this.UpdateOrder = 1000;
    }

    public override void Initialize()
    {
      base.Initialize();
      this.LevelManager.ActualAmbient = new Color(0.25f, 0.25f, 0.25f);
      this.LevelManager.ActualDiffuse = Color.White;
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Loading || this.GameState.Paused)
        return;
      float num1 = (float) gameTime.ElapsedGameTime.TotalSeconds;
      this.StepTime += num1;
      if (this.ActiveState == ZoomOut.State.Wait)
      {
        if ((double) this.StepTime > 5.0)
        {
          this.OldSfxVol = this.SoundManager.SoundEffectVolume;
          this.ChangeState();
        }
      }
      else if (this.ActiveState == ZoomOut.State.Zooming)
      {
        IGameCameraManager cameraManager = this.CameraManager;
        double num2 = (double) cameraManager.Radius * (double) MathHelper.Lerp(1f, 1.05f, Easing.EaseIn((double) FezMath.Saturate(this.StepTime / 35f), EasingType.Quadratic));
        cameraManager.Radius = (float) num2;
        this.CameraManager.Center = Vector3.Lerp(this.CameraManager.Center, this.LevelManager.Size / 2f, Easing.EaseInOut((double) FezMath.Saturate(this.StepTime / 35f), EasingType.Sine));
        this.SoundManager.SoundEffectVolume = 1f - FezMath.Saturate(this.StepTime / 33f);
        if ((double) this.StepTime > 33.0)
          this.ChangeState();
      }
      if ((double) num1 == 0.0 || !Keyboard.GetState().IsKeyDown(Keys.R))
        return;
      this.ActiveState = ZoomOut.State.Zooming;
      this.ChangeState();
    }

    private void ChangeState()
    {
      if (this.ActiveState == ZoomOut.State.Zooming)
      {
        this.SoundManager.KillSounds();
        this.SoundManager.SoundEffectVolume = this.OldSfxVol;
        this.Host.Cycle();
      }
      else
      {
        this.StepTime = 0.0f;
        ++this.ActiveState;
        this.Update(new GameTime());
      }
    }

    public override void Draw(GameTime gameTime)
    {
      if (this.ActiveState == ZoomOut.State.Wait || this.GameState.Loading || (double) this.StepTime <= 25.0)
        return;
      this.TargetRenderer.DrawFullscreen(new Color(0.2705882f, 0.9764706f, 1f, Easing.EaseInOut((double) FezMath.Saturate((float) (((double) this.StepTime - 25.0) / 7.0)), EasingType.Sine)));
    }

    private enum State
    {
      Wait,
      Zooming,
    }
  }
}
