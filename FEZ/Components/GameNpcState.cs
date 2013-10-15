// Type: FezGame.Components.GameNpcState
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using Common;
using FezEngine;
using FezEngine.Components;
using FezEngine.Services;
using FezEngine.Services.Scripting;
using FezEngine.Structure;
using FezEngine.Structure.Input;
using FezEngine.Tools;
using FezGame.Services;
using FezGame.Structure;
using FezGame.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezGame.Components
{
  public class GameNpcState : NpcState
  {
    private bool SaidFirstLine;
    private int SequentialLineIndex;
    public bool ForceVisible;
    public bool IsNightForOwl;
    private readonly float originalSpeed;
    private SoundEffect takeoffSound;
    private SoundEffect hideSound;
    private SoundEffect comeOutSound;
    private SoundEffect burrowSound;

    protected override float AnimationSpeed
    {
      get
      {
        if (this.CurrentAction != NpcAction.Walk && this.CurrentAction != NpcAction.Turn)
          return 1f;
        else
          return this.Npc.WalkSpeed / ((double) this.originalSpeed == 0.0 ? 1f : this.originalSpeed);
      }
    }

    [ServiceDependency]
    public IPhysicsManager PhysicsManager { private get; set; }

    [ServiceDependency]
    public new ILevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public ILevelService LevelService { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public ISpeechBubbleManager SpeechManager { private get; set; }

    [ServiceDependency]
    public IPlayerManager PlayerManager { private get; set; }

    [ServiceDependency]
    public IInputManager InputManager { private get; set; }

    [ServiceDependency]
    public IOwlService OwlService { get; set; }

    public GameNpcState(Game game, NpcInstance npc)
      : base(game, npc)
    {
      this.originalSpeed = this.Npc.WalkSpeed;
    }

    public override void Initialize()
    {
      base.Initialize();
      if (this.CanTakeOff)
        this.takeoffSound = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Wildlife/BirdTakeoff");
      if (this.CanHide)
      {
        this.hideSound = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Wildlife/CritterHide");
        this.comeOutSound = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Wildlife/CritterComeOut");
      }
      if (!this.CanBurrow)
        return;
      this.burrowSound = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Wildlife/RabbitBurrow");
    }

    public override void Update(GameTime gameTime)
    {
      if (this.isDisposed)
        return;
      if (this.EngineState.Paused || this.GameState.InMap)
      {
        if (this.Emitter == null || this.Emitter.Dead)
          return;
        this.Emitter.FadeOutAndDie(0.1f);
      }
      else
      {
        if (this.EngineState.Loading || this.EngineState.SkipRendering)
          return;
        if (this.Npc.Visible)
          this.UpdateRotation();
        if (!FezMath.IsOrthographic(this.CameraManager.Viewpoint) || !this.CameraManager.ActionRunning)
          return;
        base.Update(gameTime);
        if (this.Npc.ActorType == ActorType.LightningGhost)
        {
          if (this.CurrentAction == NpcAction.Talk)
            this.Group.Material.Opacity = Math.Min(this.Group.Material.Opacity + 0.01f, 0.5f);
          else if (this.Npc.Talking)
          {
            this.Group.Material.Opacity = Math.Max(this.Group.Material.Opacity - 0.01f, 0.0f);
            if ((double) this.Group.Material.Opacity == 0.0)
            {
              this.Npc.Talking = false;
              this.Group.Material.Opacity = 1f;
            }
          }
        }
        if (this.Npc.ActorType != ActorType.Owl)
          return;
        bool flag = this.TimeManager.IsDayPhase(DayPhase.Night) || this.ForceVisible;
        if (this.ForceVisible)
          this.OwlInvisible = false;
        if (flag)
        {
          if (!this.IsNightForOwl && !this.GameState.SaveData.ThisLevel.InactiveNPCs.Contains(this.Npc.Id))
          {
            this.CurrentAction = NpcAction.Fly;
            this.UpdateAction();
            this.FlyingBack = true;
            this.OwlInvisible = false;
            this.Position = this.Npc.Position + ((float) ((double) Math.Min(this.CameraManager.Radius, 60f * SettingsManager.GetViewScale(this.GraphicsDevice)) / (double) this.CameraManager.AspectRatio / 2.0) - (this.Npc.Position.Y - this.CameraManager.Center.Y)) * (-Vector3.Transform(Vector3.Right, this.Group.Rotation) * 1.502951f + Vector3.Up);
          }
        }
        else if (this.IsNightForOwl)
        {
          this.Hide();
          this.UpdateAction();
          this.MayComeBack = true;
        }
        this.IsNightForOwl = flag;
      }
    }

    protected override void TryFlee()
    {
      if (this.PlayerManager.Action == ActionType.IdleSleep)
        return;
      float num1 = FezMath.Dot(FezMath.Abs(this.Position - this.PlayerManager.Position), FezMath.ScreenSpaceMask(this.CameraManager.Viewpoint));
      this.Npc.WalkSpeed = this.originalSpeed * (float) (1.0 + (1.0 - (double) FezMath.Saturate(num1 / 3f)));
      switch (this.CurrentAction)
      {
        case NpcAction.Turn:
          break;
        case NpcAction.Burrow:
          break;
        case NpcAction.ComeOut:
          break;
        case NpcAction.TakeOff:
          break;
        case NpcAction.Fly:
          break;
        default:
          Vector3 vector3 = Vector3.UnitY + FezMath.SideMask(this.CameraManager.Viewpoint) * 3f + FezMath.DepthMask(this.CameraManager.Viewpoint) * float.MaxValue;
          if (new BoundingBox(this.Position - vector3, this.Position + vector3).Contains(this.PlayerManager.Position) == ContainmentType.Disjoint)
          {
            if (this.CurrentAction != NpcAction.Hide)
              break;
            this.CurrentAction = NpcAction.ComeOut;
            this.UpdateAction();
            break;
          }
          else
          {
            float num2 = FezMath.Dot(this.PlayerManager.Position - this.Position, FezMath.RightVector(this.CameraManager.Viewpoint)) * (float) Math.Sign(FezMath.Dot(this.Npc.DestinationOffset, FezMath.RightVector(this.CameraManager.Viewpoint)));
            NpcAction npcAction = this.CurrentAction;
            if ((double) num1 < 1.0)
              this.Hide();
            else if (this.CurrentAction == NpcAction.Hide)
            {
              this.CurrentAction = NpcAction.ComeOut;
              SoundEffectExtensions.EmitAt(this.comeOutSound, this.Position);
              this.UpdateAction();
              break;
            }
            else
            {
              HorizontalDirection horizontalDirection = FezMath.DirectionFromMovement(-num2);
              if (this.LookingDirection != horizontalDirection)
              {
                if (this.CanTurn)
                {
                  this.CurrentAction = NpcAction.Turn;
                }
                else
                {
                  this.LookingDirection = horizontalDirection;
                  this.CurrentAction = this.CanWalk ? NpcAction.Walk : NpcAction.Idle;
                }
              }
              else
                this.CurrentAction = this.CanWalk ? NpcAction.Walk : NpcAction.Idle;
            }
            if (npcAction == this.CurrentAction)
              break;
            this.UpdateAction();
            break;
          }
      }
    }

    private void Hide()
    {
      if (this.CanBurrow)
      {
        this.CurrentAction = NpcAction.Burrow;
        SoundEffectExtensions.EmitAt(this.burrowSound, this.Position);
      }
      else if (this.CanHide)
      {
        if (this.CurrentAction != NpcAction.Hide)
          SoundEffectExtensions.EmitAt(this.hideSound, this.Position);
        this.CurrentAction = NpcAction.Hide;
      }
      else if (this.CanTakeOff)
      {
        this.CurrentAction = NpcAction.TakeOff;
        SoundEffectExtensions.EmitAt(this.takeoffSound, this.Position);
      }
      else
        this.CurrentAction = this.CanIdle ? NpcAction.Idle : NpcAction.Walk;
    }

    protected override void TryTalk()
    {
      switch (this.PlayerManager.Action)
      {
        case ActionType.Idle:
        case ActionType.Walking:
        case ActionType.Running:
        case ActionType.Sliding:
          if (this.PlayerManager.Background || !this.SpeechManager.Hidden || this.Npc.ActorType == ActorType.Owl && (this.OwlInvisible || this.CurrentAction == NpcAction.TakeOff || this.CurrentAction == NpcAction.Fly))
            break;
          if (this.Npc.CustomSpeechLine == null)
          {
            if (this.InputManager.CancelTalk != FezButtonState.Pressed)
              break;
            Vector3 vector3_1 = Vector3.UnitY + (FezMath.SideMask(this.CameraManager.Viewpoint) + FezMath.DepthMask(this.CameraManager.Viewpoint)) * 1.5f;
            BoundingBox boundingBox = new BoundingBox(this.Position - vector3_1, this.Position + vector3_1);
            Vector3 mask = FezMath.GetMask(FezMath.VisibleAxis(this.CameraManager.Viewpoint));
            Vector3 vector3_2 = FezMath.ForwardVector(this.CameraManager.Viewpoint);
            Ray ray = new Ray()
            {
              Position = this.PlayerManager.Center * (Vector3.One - mask) - vector3_2 * this.LevelManager.Size,
              Direction = vector3_2
            };
            float? nullable = boundingBox.Intersects(ray);
            if (!nullable.HasValue || this.TestObstruction(ray.Position, nullable.Value))
              break;
          }
          this.Talk();
          break;
      }
    }

    private bool TestObstruction(Vector3 hitStart, float hitDistance)
    {
      Vector3 b = FezMath.ForwardVector(this.CameraManager.Viewpoint);
      Vector3 vector3 = FezMath.ScreenSpaceMask(this.CameraManager.Viewpoint);
      foreach (TrileInstance trileInstance in (IEnumerable<TrileInstance>) this.LevelManager.Triles.Values)
      {
        if (trileInstance.InstanceId != -1 && (double) ((trileInstance.Center - hitStart) * vector3).LengthSquared() < 0.5)
        {
          Trile trile = trileInstance.Trile;
          if (trileInstance.Enabled && !trile.Immaterial && !trile.SeeThrough)
            return (double) FezMath.Dot(trileInstance.Position + Vector3.One / 2f + b * -0.5f - hitStart, b) <= (double) hitDistance + 0.25;
        }
      }
      return false;
    }

    private void Talk()
    {
      if (this.Npc.CustomSpeechLine != null)
      {
        this.CurrentLine = this.Npc.CustomSpeechLine;
      }
      else
      {
        SpeechLine speechLine = this.CurrentLine;
        if (this.Npc.Speech.Count <= 1 || this.Npc.SayFirstSpeechLineOnce && !this.SaidFirstLine)
        {
          this.CurrentLine = Enumerable.FirstOrDefault<SpeechLine>((IEnumerable<SpeechLine>) this.Npc.Speech);
        }
        else
        {
          do
          {
            if (this.Npc.RandomizeSpeech)
            {
              this.CurrentLine = RandomHelper.InList<SpeechLine>(this.Npc.Speech);
            }
            else
            {
              this.CurrentLine = this.Npc.Speech[this.SequentialLineIndex];
              ++this.SequentialLineIndex;
              if (this.SequentialLineIndex == this.Npc.Speech.Count)
                this.SequentialLineIndex = 0;
            }
          }
          while (speechLine == this.CurrentLine || this.Npc.SayFirstSpeechLineOnce && this.SaidFirstLine && this.CurrentLine == this.Npc.Speech[0]);
        }
        this.SaidFirstLine = true;
      }
      IPlayerManager playerManager1 = this.PlayerManager;
      Vector3 vector3_1 = playerManager1.Velocity * Vector3.UnitY;
      playerManager1.Velocity = vector3_1;
      this.PlayerManager.Action = ActionType.ReadingSign;
      Vector3 a = this.PlayerManager.Position - this.Position;
      this.SpeechManager.Origin = this.Position + Vector3.UnitY * 0.5f;
      string s;
      if (this.LevelManager.SongName == "Majesty")
      {
        this.SpeechManager.Font = SpeechFont.Zuish;
        string stringRaw = GameText.GetStringRaw(this.CurrentLine.Text);
        this.SpeechManager.Origin = this.Position + Vector3.UnitY * 0.5f + FezMath.RightVector(this.CameraManager.Viewpoint);
        this.SpeechManager.ChangeText(s = stringRaw);
      }
      else
        this.SpeechManager.ChangeText(s = GameText.GetString(this.CurrentLine.Text));
      this.LookingDirection = FezMath.DirectionFromMovement(FezMath.Dot(a * FezMath.Sign(this.Npc.DestinationOffset), FezMath.SideMask(this.CameraManager.Viewpoint)));
      this.PlayerManager.LookingDirection = FezMath.DirectionFromMovement(-FezMath.Dot(a, FezMath.RightVector(this.CameraManager.Viewpoint)));
      float num = FezMath.Dot(a, FezMath.SideMask(this.CameraManager.Viewpoint));
      if ((double) Math.Abs(num) < 1.0)
      {
        Vector3 center = this.PlayerManager.Center;
        Vector3 velocity = this.PlayerManager.Velocity;
        MultipleHits<TrileInstance> ground = this.PlayerManager.Ground;
        IPlayerManager playerManager2 = this.PlayerManager;
        Vector3 vector3_2 = playerManager2.Center + (float) Math.Sign(num) * (1.25f - Math.Abs(num)) * FezMath.SideMask(this.CameraManager.Viewpoint);
        playerManager2.Center = vector3_2;
        this.PhysicsManager.Update((IComplexPhysicsEntity) this.PlayerManager);
        this.PlayerManager.Velocity = velocity;
        if (!this.PlayerManager.Grounded)
        {
          this.PlayerManager.Center = center;
          this.PlayerManager.Ground = ground;
        }
        else
          this.PlayerManager.Center = center + (float) Math.Sign(num) * (1f - Math.Abs(num)) * FezMath.SideMask(this.CameraManager.Viewpoint);
      }
      this.CurrentAction = NpcAction.Talk;
      this.Npc.Talking = true;
      if (this.Npc.ActorType == ActorType.LightningGhost)
        this.Group.Material.Opacity = 0.0f;
      this.talkWaiter = Waiters.Wait(0.100000001490116 + 0.0750000029802322 * (double) Util.StripPunctuation(s).Length * (Culture.IsCJK ? 2.0 : 1.0), (Action) (() =>
      {
        if (this.talkEmitter == null)
          return;
        this.talkEmitter.FadeOutAndPause(0.1f);
      }));
      this.talkWaiter.AutoPause = true;
      this.UpdateAction();
    }

    protected override bool TryStopTalking()
    {
      bool flag = this.SpeechManager.Hidden || !this.PlayerManager.Grounded;
      if (flag)
      {
        if (this.Npc.CustomSpeechLine == null && !this.Npc.RandomizeSpeech && (!this.Npc.SayFirstSpeechLineOnce && this.SequentialLineIndex != 0))
        {
          this.Talk();
          return false;
        }
        else
        {
          if (this.talkWaiter != null && this.talkWaiter.Alive)
            this.talkWaiter.Cancel();
          if (this.talkEmitter != null && !this.talkEmitter.Dead)
            this.talkEmitter.FadeOutAndPause(0.1f);
          if (!this.SpeechManager.Hidden)
            this.SpeechManager.Hide();
          this.Npc.CustomSpeechLine = (SpeechLine) null;
          if (this.Npc.ActorType == ActorType.Owl)
          {
            this.LookingDirection = FezMath.DirectionFromMovement(FezMath.Dot(-(this.PlayerManager.Position - this.Position) * FezMath.Sign(this.Npc.DestinationOffset), FezMath.SideMask(this.CameraManager.Viewpoint)));
            this.CurrentAction = NpcAction.TakeOff;
            this.UpdateAction();
            ++this.GameState.SaveData.CollectedOwls;
            this.OwlService.OnOwlCollected();
            this.GameState.SaveData.ThisLevel.InactiveNPCs.Add(this.Npc.Id);
            this.LevelService.ResolvePuzzle();
          }
        }
      }
      return flag;
    }
  }
}
