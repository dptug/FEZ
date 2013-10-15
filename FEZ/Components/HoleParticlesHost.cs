// Type: FezGame.Components.HoleParticlesHost
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Services;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using System;

namespace FezGame.Components
{
  public class HoleParticlesHost : GameComponent
  {
    [ServiceDependency]
    public IPhysicsManager PhysicsManager { private get; set; }

    [ServiceDependency]
    public ITrixelParticleSystems ParticleSystems { private get; set; }

    [ServiceDependency]
    public IPlayerManager PlayerManager { private get; set; }

    [ServiceDependency]
    public IGameCameraManager CameraManager { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public IGameLevelManager LevelManager { private get; set; }

    public HoleParticlesHost(Game game)
      : base(game)
    {
    }

    public override void Initialize()
    {
      base.Initialize();
      this.LevelManager.LevelChanged += new Action(this.TryInitialize);
      this.TryInitialize();
      this.Enabled = false;
    }

    private void TryInitialize()
    {
      if (this.LevelManager.Name == null || !this.LevelManager.Name.StartsWith("HOLE"))
        return;
      this.GameState.SkipFadeOut = true;
      this.PlayerManager.CanControl = false;
      this.CameraManager.Constrained = true;
      this.CameraManager.PixelsPerTrixel = 3f;
      this.CameraManager.Center = new Vector3(13f, 20f, 27.875f);
      this.PlayerManager.Position = new Vector3(13.5f, 15.47f, 28.5f);
      this.PlayerManager.Ground = new MultipleHits<TrileInstance>();
      this.PlayerManager.Velocity = 0.007875f * -Vector3.UnitY;
      this.PhysicsManager.Update((IComplexPhysicsEntity) this.PlayerManager);
      this.PlayerManager.Velocity = 0.007875f * -Vector3.UnitY;
      this.PlayerManager.RecordRespawnInformation(true);
      this.PlayerManager.Position = new Vector3(13.5f, 50f, 28.5f);
      this.PlayerManager.Ground = new MultipleHits<TrileInstance>();
      this.PlayerManager.Action = ActionType.FreeFalling;
      for (int index = 0; index < 4; ++index)
      {
        TrileInstance trileInstance = new TrileInstance(new Vector3(13.5f, (float) (30 + index * 5), (float) (27 - index)), 354);
        TrixelParticleSystem system = new TrixelParticleSystem(this.Game, new TrixelParticleSystem.Settings()
        {
          ExplodingInstance = trileInstance,
          EnergySource = new Vector3?(trileInstance.Center),
          ParticleCount = 25,
          MinimumSize = 1,
          MaximumSize = 6,
          GravityModifier = 1.5f,
          Darken = true,
          Energy = (float) (index + 2) / 4f
        });
        this.ParticleSystems.Add(system);
        system.Initialize();
      }
      this.Enabled = true;
    }

    public override void Update(GameTime gameTime)
    {
      if (!ActionTypeExtensions.IsIdle(this.PlayerManager.Action))
        return;
      this.PlayerManager.CanControl = true;
      ServiceHelper.RemoveComponent<HoleParticlesHost>(this);
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      this.LevelManager.LevelChanged -= new Action(this.TryInitialize);
    }
  }
}
