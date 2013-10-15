// Type: FezGame.Components.Actions.EnterPipe
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Structure.Input;
using FezEngine.Structure.Scripting;
using FezEngine.Tools;
using FezGame.Services;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezGame.Components.Actions
{
  internal class EnterPipe : PlayerAction
  {
    private const float SuckedSeconds = 0.75f;
    private const float FadeSeconds = 1.25f;
    private SoundEffect EnterSound;
    private SoundEffect ExitSound;
    private Volume PipeVolume;
    private EnterPipe.States State;
    private bool Descending;
    private TimeSpan SinceChanged;
    private string NextLevel;
    private float Depth;

    [ServiceDependency]
    public ITargetRenderingManager TargetRenderer { private get; set; }

    [ServiceDependency]
    public IThreadPool ThreadPool { private get; set; }

    public EnterPipe(Game game)
      : base(game)
    {
      this.DrawOrder = 101;
    }

    protected override void LoadContent()
    {
      base.LoadContent();
      this.EnterSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Sewer/PipeDown");
      this.ExitSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Sewer/PipeUp");
    }

    protected override void TestConditions()
    {
      if (this.PlayerManager.Action == ActionType.WalkingTo || this.IsActionAllowed(this.PlayerManager.Action))
        return;
      if (this.PlayerManager.Grounded && (this.PlayerManager.PipeVolume.HasValue && this.InputManager.Down == FezButtonState.Pressed))
      {
        this.PipeVolume = this.LevelManager.Volumes[this.PlayerManager.PipeVolume.Value];
        this.PlayerManager.Action = ActionType.EnteringPipe;
        this.Descending = true;
      }
      if (this.PlayerManager.Grounded || (!this.PlayerManager.PipeVolume.HasValue || !FezButtonStateExtensions.IsDown(this.InputManager.Up) || !BoxCollisionResultExtensions.AnyCollided(this.PlayerManager.Ceiling)))
        return;
      this.PipeVolume = this.LevelManager.Volumes[this.PlayerManager.PipeVolume.Value];
      this.PlayerManager.Action = ActionType.EnteringPipe;
      this.Descending = false;
      Vector3 vector3_1 = FezMath.ScreenSpaceMask(this.CameraManager.Viewpoint);
      Vector3 vector3_2 = (this.PipeVolume.From + this.PipeVolume.To) / 2f;
      this.PlayerManager.Position = this.PlayerManager.Position * vector3_1 + vector3_2 * (Vector3.One - vector3_1);
    }

    protected override void Begin()
    {
      this.NextLevel = this.PlayerManager.NextLevel;
      this.State = EnterPipe.States.Sucked;
      this.SinceChanged = TimeSpan.Zero;
      this.PlayerManager.Velocity = Vector3.Zero;
      SoundEffectExtensions.EmitAt(this.EnterSound, this.PlayerManager.Position);
    }

    protected override void End()
    {
      this.State = EnterPipe.States.None;
    }

    protected override bool Act(TimeSpan elapsed)
    {
      switch (this.State)
      {
        case EnterPipe.States.Sucked:
          this.PlayerManager.Position += (float) elapsed.TotalSeconds * Vector3.UnitY * (this.Descending ? -1f : 1f) * 0.75f;
          this.SinceChanged += elapsed;
          if (this.SinceChanged.TotalSeconds > 0.75)
          {
            this.State = EnterPipe.States.FadeOut;
            this.SinceChanged = TimeSpan.Zero;
            break;
          }
          else
            break;
        case EnterPipe.States.FadeOut:
          this.PlayerManager.Position += (float) elapsed.TotalSeconds * Vector3.UnitY * (this.Descending ? -1f : 1f) * 0.75f;
          this.SinceChanged += elapsed;
          if (this.SinceChanged.TotalSeconds > 1.25)
          {
            this.State = EnterPipe.States.LevelChange;
            this.SinceChanged = TimeSpan.Zero;
            this.GameState.Loading = true;
            Worker<bool> worker = this.ThreadPool.Take<bool>(new Action<bool>(this.DoLoad));
            worker.Finished += (Action) (() => this.ThreadPool.Return<bool>(worker));
            worker.Start(false);
            break;
          }
          else
            break;
        case EnterPipe.States.FadeIn:
          if (this.SinceChanged == TimeSpan.Zero)
            SoundEffectExtensions.EmitAt(this.ExitSound, this.PlayerManager.Position);
          Vector3 vector3_1 = FezMath.ScreenSpaceMask(this.CameraManager.Viewpoint);
          this.PlayerManager.Position = this.PlayerManager.Position * vector3_1 + this.Depth * (Vector3.One - vector3_1);
          this.PlayerManager.Position += (float) elapsed.TotalSeconds * Vector3.UnitY * (this.Descending ? -1.1f : 1f) * 0.75f;
          this.SinceChanged += elapsed;
          if (this.SinceChanged.TotalSeconds > 1.25)
          {
            this.State = EnterPipe.States.SpitOut;
            this.SinceChanged = TimeSpan.Zero;
            break;
          }
          else
            break;
        case EnterPipe.States.SpitOut:
          this.PlayerManager.Position += (float) elapsed.TotalSeconds * Vector3.UnitY * (this.Descending ? -1.1f : 1f) * 0.75f;
          this.SinceChanged += elapsed;
          bool flag = true;
          foreach (PointCollision pointCollision in this.PlayerManager.CornerCollision)
            flag = flag & pointCollision.Instances.Deep == null;
          if (!this.Descending && flag || this.SinceChanged.TotalSeconds > 0.75)
          {
            this.State = EnterPipe.States.None;
            this.SinceChanged = TimeSpan.Zero;
            if (!this.Descending)
            {
              this.PlayerManager.Position += 0.5f * Vector3.UnitY;
              IPlayerManager playerManager = this.PlayerManager;
              Vector3 vector3_2 = playerManager.Velocity - Vector3.UnitY;
              playerManager.Velocity = vector3_2;
              this.PhysicsManager.Update((IComplexPhysicsEntity) this.PlayerManager);
            }
            this.PlayerManager.Action = ActionType.Idle;
            break;
          }
          else
            break;
      }
      return false;
    }

    private void DoLoad(bool dummy)
    {
      this.LevelManager.ChangeLevel(this.NextLevel);
      this.GameState.ScheduleLoadEnd = true;
      this.State = EnterPipe.States.FadeIn;
      Volume volume = Enumerable.FirstOrDefault<Volume>((IEnumerable<Volume>) this.LevelManager.Volumes.Values, (Func<Volume, bool>) (v =>
      {
        int local_0 = v.Id;
        int? local_1 = Enumerable.First<Script>((IEnumerable<Script>) this.LevelManager.Scripts.Values, (Func<Script, bool>) (s => Enumerable.Any<ScriptAction>((IEnumerable<ScriptAction>) s.Actions, (Func<ScriptAction, bool>) (a => a.Operation == "AllowPipeChangeLevel")))).Triggers[0].Object.Identifier;
        if (local_0 == local_1.GetValueOrDefault())
          return local_1.HasValue;
        else
          return false;
      }));
      if (volume == null)
        throw new InvalidOperationException("Missing pipe volume in destination level!");
      Vector3 a = (volume.From + volume.To) / 2f;
      this.PlayerManager.Action = ActionType.EnteringPipe;
      this.PlayerManager.Position = a + Vector3.UnitY * 1.25f * (this.Descending ? 1f : -1f);
      this.PlayerManager.Velocity = Vector3.Zero;
      this.PlayerManager.RecordRespawnInformation();
      this.Depth = FezMath.Dot(a, FezMath.DepthMask(this.CameraManager.Viewpoint));
    }

    public override void Draw(GameTime gameTime)
    {
      if (this.State != EnterPipe.States.FadeOut && this.State != EnterPipe.States.FadeIn && this.State != EnterPipe.States.LevelChange)
        return;
      double linearStep = this.SinceChanged.TotalSeconds / 1.25;
      if (this.State == EnterPipe.States.FadeIn)
        linearStep = 1.0 - linearStep;
      float alpha = FezMath.Saturate(Easing.EaseIn(linearStep, EasingType.Cubic));
      GraphicsDeviceExtensions.PrepareStencilRead(this.GraphicsDevice, CompareFunction.Always, StencilMask.None);
      this.TargetRenderer.DrawFullscreen(new Color(0.0f, 0.0f, 0.0f, alpha));
    }

    protected override bool IsActionAllowed(ActionType type)
    {
      if (type != ActionType.EnteringPipe)
        return this.State != EnterPipe.States.None;
      else
        return true;
    }

    private enum States
    {
      None,
      Sucked,
      FadeOut,
      LevelChange,
      FadeIn,
      SpitOut,
    }
  }
}
