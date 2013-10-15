// Type: FezGame.Components.Actions.Swim
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Components;
using FezGame.Services;
using FezGame.Structure;
using FezGame.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;

namespace FezGame.Components.Actions
{
  internal class Swim : PlayerAction
  {
    private readonly MovementHelper movementHelper = new MovementHelper(4.7f, 4.7f, 0.0f);
    private const float PulseDelay = 0.5f;
    private const float Buoyancy = 0.006f;
    private const float MaxSubmergedPortion = 0.5f;
    private TimeSpan sincePulsed;
    private SoundEmitter treadInstance;
    private SoundEffect swimSound;

    private float SubmergedPortion
    {
      get
      {
        return !LiquidTypeExtensions.IsWater(this.LevelManager.WaterType) ? 0.25f : 0.5f;
      }
    }

    [ServiceDependency]
    public IPlaneParticleSystems PlaneParticleSystems { get; set; }

    public Swim(Game game)
      : base(game)
    {
    }

    protected override void LoadContent()
    {
      base.LoadContent();
      this.swimSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/Swim");
    }

    public override void Initialize()
    {
      base.Initialize();
      this.movementHelper.Entity = (IPhysicsEntity) this.PlayerManager;
      this.LevelManager.LevelChanged += (Action) (() =>
      {
        if (this.LevelManager.WaterType == LiquidType.None)
          return;
        this.treadInstance = SoundEffectExtensions.Emit(this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/Tread"), true, true);
      });
    }

    protected override void TestConditions()
    {
      TrileGroup trileGroup;
      if (this.PlayerManager.Action == ActionType.Swimming || this.PlayerManager.Action == ActionType.Dying || (this.PlayerManager.Action == ActionType.Sinking || this.PlayerManager.Action == ActionType.Treading) || (this.PlayerManager.Action == ActionType.HurtSwim || this.PlayerManager.Action == ActionType.SuckedIn) || this.PlayerManager.Grounded && (this.LevelManager.PickupGroups.TryGetValue(this.PlayerManager.Ground.First, out trileGroup) && trileGroup.ActorType == ActorType.Geyser) || (this.LevelManager.WaterType == LiquidType.None || (double) this.PlayerManager.Position.Y >= (double) this.LevelManager.WaterHeight - (double) this.SubmergedPortion || this.PlayerManager.Action == ActionType.Jumping))
        return;
      this.PlayerManager.RecordRespawnInformation();
      ActionType action = this.PlayerManager.Action;
      this.PlayerManager.Action = ActionType.Treading;
      if (action != ActionType.Flying)
        this.PlaneParticleSystems.Splash((IPhysicsEntity) this.PlayerManager, false);
      IPlayerManager playerManager = this.PlayerManager;
      Vector3 vector3 = playerManager.Velocity * new Vector3(1f, 0.5f, 1f);
      playerManager.Velocity = vector3;
    }

    protected override void Begin()
    {
      this.PlayerManager.CarriedInstance = (TrileInstance) null;
    }

    protected override void End()
    {
      this.sincePulsed = TimeSpan.Zero;
      if (this.PlayerManager.Action == ActionType.Suffering || this.PlayerManager.Action == ActionType.Sinking || (this.LevelManager.WaterType == LiquidType.None || this.PlayerManager.Action == ActionType.Flying))
        return;
      if (this.PlayerManager.Action != ActionType.Jumping)
      {
        IPlayerManager playerManager = this.PlayerManager;
        Vector3 vector3 = playerManager.Velocity * new Vector3(1f, 0.5f, 1f);
        playerManager.Velocity = vector3;
      }
      this.PlaneParticleSystems.Splash((IPhysicsEntity) this.PlayerManager, true);
    }

    protected override bool Act(TimeSpan elapsed)
    {
      // ISSUE: unable to decompile the method.
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
      if (this.GameState.Loading || this.PlayerManager.Action == ActionType.Treading || (this.treadInstance == null || this.treadInstance.Dead) || this.treadInstance.Cue.State != SoundState.Playing)
        return;
      this.treadInstance.Cue.Pause();
    }

    protected override bool IsActionAllowed(ActionType type)
    {
      return ActionTypeExtensions.IsSwimming(type);
    }
  }
}
