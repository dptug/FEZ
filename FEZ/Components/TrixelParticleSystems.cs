// Type: FezGame.Components.TrixelParticleSystems
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Services;
using FezEngine.Tools;
using FezGame.Services;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Threading;

namespace FezGame.Components
{
  public class TrixelParticleSystems : GameComponent, ITrixelParticleSystems
  {
    private readonly List<TrixelParticleSystem> OtherDeadParticleSystems = new List<TrixelParticleSystem>();
    private readonly List<TrixelParticleSystem> ActiveParticleSystems = new List<TrixelParticleSystem>();
    private readonly List<TrixelParticleSystem> DeadParticleSystems = new List<TrixelParticleSystem>();
    private const int LimitBeforeMultithread = 2;
    private Worker<MtUpdateContext<List<TrixelParticleSystem>>> otherThread;

    public int Count
    {
      get
      {
        return this.ActiveParticleSystems.Count;
      }
    }

    [ServiceDependency]
    public IGameCameraManager CameraManager { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public IDebuggingBag DebuggingBag { private get; set; }

    [ServiceDependency]
    public ILevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public IThreadPool ThreadPool { private get; set; }

    public TrixelParticleSystems(Game game)
      : base(game)
    {
    }

    public override void Initialize()
    {
      this.LevelManager.LevelChanged += (Action) (() =>
      {
        foreach (TrixelParticleSystem item_0 in this.ActiveParticleSystems)
          ServiceHelper.RemoveComponent<TrixelParticleSystem>(item_0);
        this.ActiveParticleSystems.Clear();
      });
      this.otherThread = this.ThreadPool.Take<MtUpdateContext<List<TrixelParticleSystem>>>(new Action<MtUpdateContext<List<TrixelParticleSystem>>>(this.UpdateParticleSystems));
      this.otherThread.Priority = ThreadPriority.Normal;
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Loading || this.GameState.Paused || (this.GameState.InMap || !FezMath.IsOrthographic(this.CameraManager.Viewpoint)) || !this.CameraManager.ActionRunning)
        return;
      TimeSpan elapsedGameTime = gameTime.ElapsedGameTime;
      int count = this.ActiveParticleSystems.Count;
      if (count >= 2)
      {
        this.otherThread.Start(new MtUpdateContext<List<TrixelParticleSystem>>()
        {
          Elapsed = elapsedGameTime,
          StartIndex = 0,
          EndIndex = count / 2,
          Result = this.OtherDeadParticleSystems
        });
        this.UpdateParticleSystems(new MtUpdateContext<List<TrixelParticleSystem>>()
        {
          Elapsed = elapsedGameTime,
          StartIndex = count / 2,
          EndIndex = count,
          Result = this.DeadParticleSystems
        });
        this.otherThread.Join();
      }
      else
        this.UpdateParticleSystems(new MtUpdateContext<List<TrixelParticleSystem>>()
        {
          Elapsed = elapsedGameTime,
          StartIndex = 0,
          EndIndex = count,
          Result = this.DeadParticleSystems
        });
      if (this.OtherDeadParticleSystems.Count > 0)
      {
        this.DeadParticleSystems.AddRange((IEnumerable<TrixelParticleSystem>) this.OtherDeadParticleSystems);
        this.OtherDeadParticleSystems.Clear();
      }
      if (this.DeadParticleSystems.Count <= 0)
        return;
      foreach (TrixelParticleSystem component in this.DeadParticleSystems)
      {
        this.ActiveParticleSystems.Remove(component);
        ServiceHelper.RemoveComponent<TrixelParticleSystem>(component);
      }
      this.DeadParticleSystems.Clear();
    }

    private void UpdateParticleSystems(MtUpdateContext<List<TrixelParticleSystem>> context)
    {
      for (int index = context.StartIndex; index < context.EndIndex; ++index)
      {
        if (index < this.ActiveParticleSystems.Count)
        {
          this.ActiveParticleSystems[index].Update(context.Elapsed);
          if (this.ActiveParticleSystems[index].Dead)
            context.Result.Add(this.ActiveParticleSystems[index]);
        }
      }
    }

    public void Add(TrixelParticleSystem system)
    {
      ServiceHelper.AddComponent((IGameComponent) system);
      this.ActiveParticleSystems.Add(system);
    }

    public void PropagateEnergy(Vector3 energySource, float energy)
    {
      foreach (TrixelParticleSystem trixelParticleSystem in this.ActiveParticleSystems)
        trixelParticleSystem.AddImpulse(energySource, energy);
    }

    protected override void Dispose(bool disposing)
    {
      this.ThreadPool.Return<MtUpdateContext<List<TrixelParticleSystem>>>(this.otherThread);
    }

    public void UnGroundAll()
    {
      foreach (TrixelParticleSystem trixelParticleSystem in this.ActiveParticleSystems)
        trixelParticleSystem.UnGround();
    }

    public void ForceDraw()
    {
      foreach (TrixelParticleSystem trixelParticleSystem in this.ActiveParticleSystems)
        trixelParticleSystem.DoDraw();
    }
  }
}
