// Type: FezGame.Components.LevelTransition
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FezGame.Components
{
  internal class LevelTransition : DrawableGameComponent
  {
    private const float FadeSeconds = 1.25f;
    private readonly string ToLevel;
    private LevelTransition.Phases Phase;
    private TimeSpan SinceStarted;

    [ServiceDependency]
    public IGameCameraManager CameraManager { private get; set; }

    [ServiceDependency]
    public IGameLevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public IPlayerManager PlayerManager { private get; set; }

    [ServiceDependency]
    public IThreadPool ThreadPool { private get; set; }

    [ServiceDependency]
    public ITargetRenderingManager TargetRenderer { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public ILevelMaterializer LevelMaterializer { private get; set; }

    public LevelTransition(Game game, string toLevel)
      : base(game)
    {
      this.ToLevel = toLevel;
      this.DrawOrder = 101;
    }

    public override void Update(GameTime gameTime)
    {
      if (this.Phase != LevelTransition.Phases.LevelChange)
        this.SinceStarted += gameTime.ElapsedGameTime;
      if (this.SinceStarted.TotalSeconds > 1.25)
      {
        switch (this.Phase)
        {
          case LevelTransition.Phases.FadeOut:
            this.Phase = LevelTransition.Phases.LevelChange;
            this.GameState.Loading = true;
            Worker<bool> worker = this.ThreadPool.Take<bool>(new Action<bool>(this.DoLoad));
            worker.Finished += (Action) (() => this.ThreadPool.Return<bool>(worker));
            worker.Start(false);
            break;
          case LevelTransition.Phases.FadeIn:
            ServiceHelper.RemoveComponent<LevelTransition>(this);
            break;
        }
      }
      base.Update(gameTime);
    }

    private void DoLoad(bool dummy)
    {
      this.LevelManager.ChangeLevel(this.ToLevel);
      this.GameState.ScheduleLoadEnd = true;
      this.Phase = LevelTransition.Phases.FadeIn;
      this.SinceStarted = TimeSpan.Zero;
    }

    public override void Draw(GameTime gameTime)
    {
      double linearStep = this.SinceStarted.TotalSeconds / 1.25;
      if (this.Phase == LevelTransition.Phases.FadeIn)
        linearStep = 1.0 - linearStep;
      float alpha = FezMath.Saturate(Easing.EaseIn(linearStep, EasingType.Cubic));
      GraphicsDeviceExtensions.PrepareStencilRead(this.GraphicsDevice, CompareFunction.Always, StencilMask.None);
      this.TargetRenderer.DrawFullscreen(new Color(0.0f, 0.0f, 0.0f, alpha));
    }

    private enum Phases
    {
      FadeOut,
      LevelChange,
      FadeIn,
    }
  }
}
