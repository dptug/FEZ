// Type: FezGame.Components.CrumblersHost
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Components;
using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Structure.Input;
using FezEngine.Tools;
using FezGame;
using FezGame.Services;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezGame.Components
{
  public class CrumblersHost : GameComponent
  {
    private readonly List<CrumblersHost.CrumblerState> States = new List<CrumblersHost.CrumblerState>();
    private bool AnyCrumblers;
    private SoundEffect sCrumble;
    private SoundEffect sWarning;

    [ServiceDependency]
    public IDefaultCameraManager CameraManager { get; set; }

    [ServiceDependency]
    public IGamepadsManager Gamepads { get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { get; set; }

    [ServiceDependency]
    public IGameLevelManager LevelManager { get; set; }

    [ServiceDependency]
    public IPlayerManager PlayerManager { get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { get; set; }

    public CrumblersHost(Game game)
      : base(game)
    {
      this.UpdateOrder = -2;
    }

    public override void Initialize()
    {
      base.Initialize();
      this.LevelManager.LevelChanged += new Action(this.TryInitialize);
      this.TryInitialize();
      this.sCrumble = this.CMProvider.Global.Load<SoundEffect>("Sounds/Nature/CrumblerCrumble");
      this.sWarning = this.CMProvider.Global.Load<SoundEffect>("Sounds/Nature/CrumblerWarning");
    }

    private void TryInitialize()
    {
      this.States.Clear();
      this.AnyCrumblers = false;
      foreach (TrileInstance instance in Enumerable.Where<TrileInstance>((IEnumerable<TrileInstance>) this.LevelManager.Triles.Values, (Func<TrileInstance, bool>) (x => x.Trile.ActorSettings.Type == ActorType.Crumbler)))
      {
        this.AnyCrumblers = true;
        instance.PhysicsState = new InstancePhysicsState(instance)
        {
          Sticky = true
        };
      }
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Loading || this.GameState.Paused || (this.GameState.InMap || !this.CameraManager.ActionRunning) || (!FezMath.IsOrthographic(this.CameraManager.Viewpoint) || !this.AnyCrumblers))
        return;
      this.TestForCrumblers();
      for (int index = this.States.Count - 1; index >= 0; --index)
      {
        if (this.States[index].Dead)
          this.States.RemoveAt(index);
        else
          this.States[index].Rumble();
      }
    }

    private void TestForCrumblers()
    {
      TrileInstance instance = (TrileInstance) null;
      bool flag = false;
      if (this.PlayerManager.Grounded)
      {
        TrileInstance nl = this.PlayerManager.Ground.NearLow;
        TrileInstance fh = this.PlayerManager.Ground.FarHigh;
        if (nl != null && (flag = flag | nl.Trile.ActorSettings.Type == ActorType.Crumbler) && !Enumerable.Any<CrumblersHost.CrumblerState>((IEnumerable<CrumblersHost.CrumblerState>) this.States, (Func<CrumblersHost.CrumblerState, bool>) (x => x.Instance == nl)))
          instance = nl;
        else if (fh != null && (flag = flag | fh.Trile.ActorSettings.Type == ActorType.Crumbler) && !Enumerable.Any<CrumblersHost.CrumblerState>((IEnumerable<CrumblersHost.CrumblerState>) this.States, (Func<CrumblersHost.CrumblerState, bool>) (x => x.Instance == fh)))
          instance = fh;
      }
      else if (ActionTypeExtensions.IsOnLedge(this.PlayerManager.Action))
      {
        TrileInstance hi = this.PlayerManager.HeldInstance;
        if (hi != null && (flag = flag | hi.Trile.ActorSettings.Type == ActorType.Crumbler) && !Enumerable.Any<CrumblersHost.CrumblerState>((IEnumerable<CrumblersHost.CrumblerState>) this.States, (Func<CrumblersHost.CrumblerState, bool>) (x => x.Instance == hi)))
          instance = hi;
      }
      if (instance != null)
        this.States.Add(new CrumblersHost.CrumblerState(instance, this));
      if (!flag || Fez.NoGamePad)
        return;
      this.Gamepads[this.GameState.ActivePlayer].Vibrate(VibrationMotor.LeftLow, 0.25, TimeSpan.FromSeconds(0.100000001490116));
      this.Gamepads[this.GameState.ActivePlayer].Vibrate(VibrationMotor.RightHigh, 0.25, TimeSpan.FromSeconds(0.100000001490116));
    }

    private class CrumblerState
    {
      private readonly List<TrileInstance> InstancesToClear = new List<TrileInstance>();
      public readonly TrileInstance Instance;
      private readonly Vector3 OriginalCenter;
      private readonly CrumblersHost Host;
      public TrixelParticleSystem System;
      private Vector3 lastJitter;
      private int c;

      public bool Dead { get; private set; }

      [ServiceDependency]
      public ISoundManager SoundManager { get; set; }

      [ServiceDependency]
      public ILevelMaterializer LevelMaterializer { get; set; }

      [ServiceDependency]
      public IPlayerManager PlayerManager { get; set; }

      [ServiceDependency]
      public IGameLevelManager LevelManager { get; set; }

      [ServiceDependency]
      public ITrixelParticleSystems ParticleSystems { get; set; }

      [ServiceDependency]
      public IGameCameraManager CameraManager { get; set; }

      public CrumblerState(TrileInstance instance, CrumblersHost host)
      {
        ServiceHelper.InjectServices((object) this);
        this.Host = host;
        this.Instance = instance;
        this.OriginalCenter = instance.PhysicsState.Center;
        Waiters.Wait(0.5, new Action(this.StartCrumbling)).AutoPause = true;
        Waiters.Wait(2.5, new Action(this.Respawn)).AutoPause = true;
        SoundEffectExtensions.EmitAt(host.sWarning, this.OriginalCenter, RandomHelper.Centered(0.00999999977648258));
      }

      public void Rumble()
      {
        if (this.c++ != 2)
        {
          this.Instance.PhysicsState.Velocity = Vector3.Zero;
          if (this.System == null)
            return;
          this.System.Offset = Vector3.Zero;
        }
        else
        {
          this.c = 0;
          Vector3 vector3 = new Vector3(RandomHelper.Centered(0.0399999991059303), 0.0f, RandomHelper.Centered(0.0399999991059303));
          Vector3 center = this.Instance.PhysicsState.Center;
          this.Instance.PhysicsState.Center += -this.lastJitter + vector3;
          if (this.System != null)
            this.System.Offset = -this.lastJitter + vector3;
          this.Instance.PhysicsState.Velocity = this.Instance.PhysicsState.Center - center;
          this.Instance.PhysicsState.UpdateInstance();
          this.lastJitter = vector3;
          this.LevelManager.UpdateInstance(this.Instance);
        }
      }

      private void StartCrumbling()
      {
        SoundEffectExtensions.EmitAt(this.Host.sCrumble, this.OriginalCenter, RandomHelper.Centered(0.00999999977648258));
        Vector3 vector3_1 = FezMath.SideMask(this.CameraManager.Viewpoint);
        Vector3 vector3_2 = FezMath.ForwardVector(this.CameraManager.Viewpoint);
        bool flag1 = (double) vector3_1.X != 0.0;
        bool flag2 = flag1;
        int num = flag2 ? (int) vector3_2.Z : (int) vector3_2.X;
        Point key = new Point(flag1 ? this.Instance.Emplacement.X : this.Instance.Emplacement.Z, this.Instance.Emplacement.Y);
        this.LevelManager.WaitForScreenInvalidation();
        Limit limit;
        if (this.LevelManager.ScreenSpaceLimits.TryGetValue(key, out limit))
        {
          limit.End += num;
          TrileEmplacement id = new TrileEmplacement(flag1 ? key.X : limit.Start, key.Y, flag2 ? limit.Start : key.X);
          while ((flag2 ? id.Z : id.X) != limit.End)
          {
            TrileInstance toRemove = this.LevelManager.TrileInstanceAt(ref id);
            if (toRemove != null && !toRemove.Hidden && toRemove.Trile.ActorSettings.Type == ActorType.Crumbler)
            {
              toRemove.Hidden = true;
              this.LevelMaterializer.CullInstanceOut(toRemove);
              this.InstancesToClear.Add(toRemove);
              ITrixelParticleSystems particleSystems = this.ParticleSystems;
              CrumblersHost.CrumblerState crumblerState = this;
              Game game = ServiceHelper.Game;
              TrixelParticleSystem.Settings settings = new TrixelParticleSystem.Settings()
              {
                BaseVelocity = Vector3.Zero,
                Energy = 0.1f,
                ParticleCount = (int) (20.0 / (double) this.InstancesToClear.Count),
                GravityModifier = 0.6f,
                Crumble = true,
                ExplodingInstance = toRemove
              };
              TrixelParticleSystem trixelParticleSystem1;
              TrixelParticleSystem trixelParticleSystem2 = trixelParticleSystem1 = new TrixelParticleSystem(game, settings);
              crumblerState.System = trixelParticleSystem1;
              TrixelParticleSystem system = trixelParticleSystem2;
              particleSystems.Add(system);
            }
            if (flag2)
              id.Z += num;
            else
              id.X += num;
          }
        }
        Waiters.Wait(1.0, new Action(this.ClearTriles)).AutoPause = true;
      }

      private void ClearTriles()
      {
        this.Instance.PhysicsState.Center = this.OriginalCenter;
        this.Instance.PhysicsState.Velocity = Vector3.Zero;
        this.Instance.PhysicsState.UpdateInstance();
        this.LevelManager.UpdateInstance(this.Instance);
        this.lastJitter = Vector3.Zero;
        foreach (TrileInstance instance in this.InstancesToClear)
        {
          if (ActionTypeExtensions.IsOnLedge(this.PlayerManager.Action) && this.PlayerManager.HeldInstance == instance)
          {
            this.PlayerManager.HeldInstance = (TrileInstance) null;
            this.PlayerManager.Action = ActionType.Falling;
          }
          this.LevelManager.ClearTrile(instance, true);
        }
        this.ParticleSystems.UnGroundAll();
        this.Dead = true;
      }

      private void Respawn()
      {
        foreach (TrileInstance instance in this.InstancesToClear)
          ServiceHelper.AddComponent((IGameComponent) new GlitchyRespawner(ServiceHelper.Game, instance));
      }
    }
  }
}
