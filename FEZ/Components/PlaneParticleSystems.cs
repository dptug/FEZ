// Type: FezGame.Components.PlaneParticleSystems
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Components;
using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Services;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Threading;

namespace FezGame.Components
{
  public class PlaneParticleSystems : DrawableGameComponent, IPlaneParticleSystems
  {
    private readonly Pool<PlaneParticleSystem> PooledParticleSystems = new Pool<PlaneParticleSystem>();
    private readonly List<PlaneParticleSystem> OtherDeadParticleSystems = new List<PlaneParticleSystem>();
    private readonly List<PlaneParticleSystem> ActiveParticleSystems = new List<PlaneParticleSystem>();
    private readonly List<PlaneParticleSystem> DeadParticleSystems = new List<PlaneParticleSystem>();
    private const int LimitBeforeMultithread = 8;
    private Worker<MtUpdateContext<List<PlaneParticleSystem>>> otherThread;
    private Texture2D WhiteSquare;
    private SoundEffect liquidSplash;

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public ISoundManager SoundManager { private get; set; }

    [ServiceDependency]
    public ILevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public IDebuggingBag DebuggingBag { private get; set; }

    [ServiceDependency]
    public IDefaultCameraManager CameraManager { private get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { get; set; }

    [ServiceDependency]
    public IThreadPool ThreadPool { private get; set; }

    public PlaneParticleSystems(Game game)
      : base(game)
    {
      this.DrawOrder = 20;
    }

    public override void Initialize()
    {
      base.Initialize();
      this.WhiteSquare = this.CMProvider.Global.Load<Texture2D>("Background Planes/white_square");
      this.liquidSplash = this.CMProvider.Global.Load<SoundEffect>("Sounds/Nature/WaterSplash");
      this.PooledParticleSystems.Size = 5;
      this.LevelManager.LevelChanged += (Action) (() =>
      {
        foreach (PlaneParticleSystem item_0 in this.ActiveParticleSystems)
        {
          item_0.Clear();
          this.PooledParticleSystems.Return(item_0);
        }
        this.ActiveParticleSystems.Clear();
        if (this.LevelManager.Rainy)
          return;
        this.PooledParticleSystems.Size = 5;
        this.OtherDeadParticleSystems.Capacity = 50;
        this.DeadParticleSystems.Capacity = 50;
        this.ActiveParticleSystems.Capacity = 100;
        while (this.PooledParticleSystems.Available > 5)
          ServiceHelper.RemoveComponent<PlaneParticleSystem>(this.PooledParticleSystems.Take());
      });
      this.otherThread = this.ThreadPool.Take<MtUpdateContext<List<PlaneParticleSystem>>>(new Action<MtUpdateContext<List<PlaneParticleSystem>>>(this.UpdateParticleSystems));
      this.otherThread.Priority = ThreadPriority.Normal;
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Loading || this.GameState.InMap || this.GameState.InMenuCube)
        return;
      TimeSpan timeSpan = gameTime.ElapsedGameTime;
      if (this.GameState.Paused || this.GameState.InMap || (!FezMath.IsOrthographic(this.CameraManager.Viewpoint) || !this.CameraManager.ActionRunning))
        timeSpan = TimeSpan.Zero;
      int count = this.ActiveParticleSystems.Count;
      if (count >= 8)
      {
        this.otherThread.Start(new MtUpdateContext<List<PlaneParticleSystem>>()
        {
          Elapsed = timeSpan,
          StartIndex = 0,
          EndIndex = count / 2,
          Result = this.OtherDeadParticleSystems
        });
        this.UpdateParticleSystems(new MtUpdateContext<List<PlaneParticleSystem>>()
        {
          Elapsed = timeSpan,
          StartIndex = count / 2,
          EndIndex = count,
          Result = this.DeadParticleSystems
        });
        this.otherThread.Join();
      }
      else
        this.UpdateParticleSystems(new MtUpdateContext<List<PlaneParticleSystem>>()
        {
          Elapsed = timeSpan,
          StartIndex = 0,
          EndIndex = count,
          Result = this.DeadParticleSystems
        });
      if (this.OtherDeadParticleSystems.Count > 0)
      {
        this.DeadParticleSystems.AddRange((IEnumerable<PlaneParticleSystem>) this.OtherDeadParticleSystems);
        this.OtherDeadParticleSystems.Clear();
      }
      if (this.DeadParticleSystems.Count <= 0)
        return;
      foreach (PlaneParticleSystem planeParticleSystem in this.DeadParticleSystems)
      {
        this.ActiveParticleSystems.Remove(planeParticleSystem);
        planeParticleSystem.Clear();
        this.PooledParticleSystems.Return(planeParticleSystem);
      }
      this.DeadParticleSystems.Clear();
    }

    public override void Draw(GameTime gameTime)
    {
      if (this.GameState.Loading || this.GameState.StereoMode || this.GameState.InMap)
        return;
      GraphicsDevice graphicsDevice = this.GraphicsDevice;
      GraphicsDeviceExtensions.GetDssCombiner(graphicsDevice).StencilPass = StencilOperation.Keep;
      GraphicsDeviceExtensions.GetDssCombiner(graphicsDevice).StencilFunction = CompareFunction.Always;
      GraphicsDeviceExtensions.GetDssCombiner(graphicsDevice).DepthBufferEnable = true;
      GraphicsDeviceExtensions.GetDssCombiner(graphicsDevice).DepthBufferFunction = CompareFunction.LessEqual;
      GraphicsDeviceExtensions.GetDssCombiner(graphicsDevice).DepthBufferWriteEnable = false;
      GraphicsDeviceExtensions.GetRasterCombiner(graphicsDevice).CullMode = CullMode.CullCounterClockwiseFace;
      this.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
      bool flag = this.LevelManager.Name == "ELDERS";
      GraphicsDeviceExtensions.SetBlendingMode(this.GraphicsDevice, BlendingMode.Alphablending);
      for (int index = 0; index < this.ActiveParticleSystems.Count; ++index)
      {
        PlaneParticleSystem planeParticleSystem = this.ActiveParticleSystems[index];
        planeParticleSystem.InScreen = flag || this.CameraManager.Frustum.Contains(planeParticleSystem.Settings.SpawnVolume) != ContainmentType.Disjoint;
        if (planeParticleSystem.InScreen && planeParticleSystem.DrawOrder == 0)
          planeParticleSystem.Draw();
      }
    }

    public void ForceDraw()
    {
      GraphicsDevice graphicsDevice = this.GraphicsDevice;
      GraphicsDeviceExtensions.GetDssCombiner(graphicsDevice).StencilPass = StencilOperation.Keep;
      GraphicsDeviceExtensions.GetDssCombiner(graphicsDevice).StencilFunction = CompareFunction.Always;
      GraphicsDeviceExtensions.GetDssCombiner(graphicsDevice).DepthBufferWriteEnable = false;
      GraphicsDeviceExtensions.GetDssCombiner(graphicsDevice).DepthBufferFunction = CompareFunction.LessEqual;
      GraphicsDeviceExtensions.GetRasterCombiner(graphicsDevice).CullMode = CullMode.CullCounterClockwiseFace;
      graphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
      for (int index = 0; index < this.ActiveParticleSystems.Count; ++index)
      {
        if (this.ActiveParticleSystems[index].InScreen)
          this.ActiveParticleSystems[index].Draw();
      }
    }

    private void UpdateParticleSystems(MtUpdateContext<List<PlaneParticleSystem>> context)
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

    public PlaneParticleSystem RainSplash(Vector3 center)
    {
      Vector3 vector3 = center;
      PlaneParticleSystem system = this.PooledParticleSystems.Take();
      system.MaximumCount = 3;
      if (system.Settings == null)
        system.Settings = new PlaneParticleSystemSettings();
      system.Settings.NoLightDraw = true;
      system.Settings.SpawnVolume = new BoundingBox()
      {
        Min = vector3 - FezMath.XZMask * 0.15f,
        Max = vector3 + FezMath.XZMask * 0.15f
      };
      system.Settings.Velocity.Function = (Func<Vector3, Vector3, Vector3>) null;
      system.Settings.Velocity.Base = new Vector3(0.0f, 3.5f, 0.0f);
      system.Settings.Velocity.Variation = new Vector3(2f, 1.5f, 2f);
      system.Settings.Gravity = new Vector3(0.0f, -0.4f, 0.0f);
      system.Settings.SpawningSpeed = 60f;
      system.Settings.ParticleLifetime = 0.275f;
      system.Settings.SystemLifetime = 0.275f;
      system.Settings.FadeInDuration = 0.0f;
      system.Settings.FadeOutDuration = 0.5f;
      system.Settings.SpawnBatchSize = 3;
      system.Settings.SizeBirth.Function = (Func<Vector3, Vector3, Vector3>) null;
      system.Settings.SizeBirth.Variation = Vector3.Zero;
      system.Settings.SizeBirth.Base = new Vector3(1.0 / 16.0);
      system.Settings.ColorLife.Base = new Color(145, 182, (int) byte.MaxValue, 96);
      system.Settings.ColorLife.Variation = new Color(0, 0, 0, 32);
      system.Settings.ColorLife.Function = (Func<Color, Color, Color>) null;
      system.Settings.Texture = this.WhiteSquare;
      system.Settings.BlendingMode = BlendingMode.Alphablending;
      system.Settings.Billboarding = true;
      this.Add(system);
      return system;
    }

    public void Splash(IPhysicsEntity entity, bool outwards)
    {
      this.Splash(entity, outwards, 0.0f);
    }

    public void Splash(IPhysicsEntity entity, bool outwards, float velocityBonus)
    {
      // ISSUE: unable to decompile the method.
    }

    public void Add(PlaneParticleSystem system)
    {
      if (!system.Initialized)
        ServiceHelper.AddComponent((IGameComponent) system);
      system.Revive();
      this.ActiveParticleSystems.Add(system);
    }

    public void Remove(PlaneParticleSystem system, bool returnToPool)
    {
      this.ActiveParticleSystems.Remove(system);
      if (!returnToPool)
        return;
      system.Clear();
      this.PooledParticleSystems.Return(system);
    }

    protected override void Dispose(bool disposing)
    {
      this.ThreadPool.Return<MtUpdateContext<List<PlaneParticleSystem>>>(this.otherThread);
      base.Dispose(disposing);
    }
  }
}
