// Type: FezGame.Components.WatchersHost
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Components;
using FezEngine.Effects;
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
using System.Linq;

namespace FezGame.Components
{
  internal class WatchersHost : DrawableGameComponent
  {
    private readonly List<Vector3> lastCrushDirections = new List<Vector3>();
    private const float WatchRange = 8f;
    private const float SpotDelay = 1f;
    private const float CrushSpeed = 15f;
    private const float WithdrawSpeed = 2f;
    private const float Acceleration = 0.025f;
    private const float CooldownTime = 0.5f;
    private const float CrushWaitTime = 1.5f;
    private Dictionary<TrileInstance, WatchersHost.WatcherState> watchers;
    private SoundEffect seeSound;
    private SoundEffect moveSound;
    private SoundEffect collideSound;
    private SoundEffect withdrawSound;

    [ServiceDependency]
    public ILightingPostProcess LightingPostProcess { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public ILevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public IPlayerManager PlayerManager { private get; set; }

    [ServiceDependency]
    public IPhysicsManager PhysicsManager { private get; set; }

    [ServiceDependency]
    public IGameCameraManager CameraManager { private get; set; }

    [ServiceDependency]
    public ISoundManager SoundManager { private get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { private get; set; }

    public WatchersHost(Game game)
      : base(game)
    {
    }

    public override void Initialize()
    {
      base.Initialize();
      this.UpdateOrder = -2;
      this.DrawOrder = 6;
      this.LevelManager.LevelChanged += new Action(this.InitializeWatchers);
      this.InitializeWatchers();
      this.LightingPostProcess.DrawGeometryLights += new Action(this.PreDraw);
    }

    protected override void LoadContent()
    {
      base.LoadContent();
      this.seeSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Zu/WatcherSee");
      this.moveSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Zu/WatcherMove");
      this.collideSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Zu/WatcherCollide");
      this.withdrawSound = this.CMProvider.Global.Load<SoundEffect>("Sounds/Zu/WatcherWithdraw");
    }

    private void InitializeWatchers()
    {
      this.watchers = new Dictionary<TrileInstance, WatchersHost.WatcherState>();
      if (this.LevelManager.TrileSet == null)
        return;
      foreach (TrileInstance instance in Enumerable.SelectMany<Trile, TrileInstance>(Enumerable.Where<Trile>((IEnumerable<Trile>) this.LevelManager.TrileSet.Triles.Values, (Func<Trile, bool>) (t => t.ActorSettings.Type == ActorType.Watcher)), (Func<Trile, IEnumerable<TrileInstance>>) (t => (IEnumerable<TrileInstance>) t.Instances)))
      {
        instance.Trile.Size = new Vector3(31.0 / 32.0);
        instance.PhysicsState = new InstancePhysicsState(instance);
        Dictionary<TrileInstance, WatchersHost.WatcherState> dictionary = this.watchers;
        TrileInstance key = instance;
        WatchersHost.WatcherState watcherState1 = new WatchersHost.WatcherState();
        WatchersHost.WatcherState watcherState2 = watcherState1;
        Mesh mesh1 = new Mesh();
        Mesh mesh2 = mesh1;
        DefaultEffect.VertexColored vertexColored1 = new DefaultEffect.VertexColored();
        vertexColored1.Fullbright = true;
        DefaultEffect.VertexColored vertexColored2 = vertexColored1;
        mesh2.Effect = (BaseEffect) vertexColored2;
        Mesh mesh3 = mesh1;
        watcherState2.Eyes = mesh3;
        watcherState1.OriginalCenter = instance.Center;
        watcherState1.CrashAttenuation = 1f;
        WatchersHost.WatcherState watcherState3 = watcherState1;
        dictionary.Add(key, watcherState3);
        this.watchers[instance].Eyes.AddColoredBox(Vector3.One / 16f, Vector3.Zero, new Color((int) byte.MaxValue, (int) sbyte.MaxValue, 0), false);
        this.watchers[instance].Eyes.AddColoredBox(Vector3.One / 16f, Vector3.Zero, new Color((int) byte.MaxValue, (int) sbyte.MaxValue, 0), false);
        this.watchers[instance].Eyes.AddColoredBox(Vector3.One / 16f, Vector3.Zero, new Color((int) byte.MaxValue, (int) sbyte.MaxValue, 0), false);
        this.watchers[instance].Eyes.AddColoredBox(Vector3.One / 16f, Vector3.Zero, new Color((int) byte.MaxValue, (int) sbyte.MaxValue, 0), false);
      }
    }

    public override void Update(GameTime gameTime)
    {
      if (this.CameraManager.Viewpoint == Viewpoint.Perspective || this.GameState.InMap || (this.GameState.Paused || this.GameState.Loading) || this.watchers.Count == 0)
        return;
      Vector3 vector3_1 = FezMath.RightVector(this.CameraManager.Viewpoint);
      Vector3 vector3_2 = FezMath.Abs(vector3_1);
      Vector3 vector3_3 = FezMath.ForwardVector(this.CameraManager.Viewpoint);
      foreach (TrileInstance index in this.watchers.Keys)
      {
        WatchersHost.WatcherState watcherState = this.watchers[index];
        Vector3 vector3_4 = index.PhysicsState.Center + vector3_2 * -5f / 16f + Vector3.UnitY * -2f / 16f - 0.5f * vector3_3;
        watcherState.Eyes.Groups[0].Position = vector3_4 + watcherState.EyeOffset;
        watcherState.Eyes.Groups[1].Position = vector3_4 + vector3_2 * 9f / 16f + watcherState.EyeOffset;
        watcherState.Eyes.Groups[0].Enabled = true;
        watcherState.Eyes.Groups[1].Enabled = true;
      }
      if (!this.CameraManager.ActionRunning || !this.CameraManager.ViewTransitionReached)
        return;
      Vector3 center1 = this.PlayerManager.Center;
      BoundingBox box = FezMath.Enclose(center1 - this.PlayerManager.Size / 2f, center1 + this.PlayerManager.Size / 2f);
      Vector3 vector3_5 = vector3_1 * 8f;
      Vector3 vector3_6 = vector3_3 * this.LevelManager.Size;
      Vector3 vector3_7 = Vector3.Up * 8f;
      this.lastCrushDirections.Clear();
      bool flag1 = false;
      foreach (TrileInstance index in this.watchers.Keys)
      {
        WatchersHost.WatcherState watcherState1 = this.watchers[index];
        Vector3 vector1 = FezMath.Sign(center1 - index.Position) * vector3_2;
        Vector3 vector3_4 = FezMath.Sign(center1 - index.Position) * Vector3.UnitY;
        BoundingBox boundingBox1 = (double) Vector3.Dot(vector1, vector3_1) > 0.0 ? FezMath.Enclose(index.Position + Vector3.UnitY * 0.05f - vector3_6, index.Position + vector3_5 + vector3_6 + new Vector3(0.9f)) : FezMath.Enclose(index.Position + Vector3.UnitY * 0.05f - vector3_6 - vector3_5, index.Position + vector3_6 + new Vector3(0.9f));
        BoundingBox boundingBox2 = FezMath.Enclose(index.Position + Vector3.UnitY * 0.05f - vector3_7 - vector3_6, index.Position + vector3_7 + new Vector3(0.9f) + vector3_6);
        switch (watcherState1.Action)
        {
          case WatchersHost.WatcherAction.Idle:
            bool flag2 = boundingBox1.Intersects(box);
            bool flag3 = boundingBox2.Intersects(box);
            watcherState1.EyeOffset = !flag2 ? (!flag3 ? Vector3.Lerp(watcherState1.EyeOffset, Vector3.Zero, 0.1f) : Vector3.Lerp(watcherState1.EyeOffset, vector3_4 * 1f / 16f, 0.25f)) : Vector3.Lerp(watcherState1.EyeOffset, vector1 * 1f / 16f, 0.25f);
            watcherState1.CrushDirection = flag2 ? vector1 : (flag3 ? vector3_4 : Vector3.Zero);
            watcherState1.Eyes.Material.Opacity = 1f;
            WatchersHost.WatcherState watcherState2;
            if (this.LevelManager.NearestTrile(index.Position + new Vector3(0.5f)).Deep == index && (flag2 || flag3) && (!FezMath.In<ActionType>(this.PlayerManager.Action, ActionType.GrabCornerLedge, ActionType.Suffering, ActionType.Dying, (IEqualityComparer<ActionType>) ActionTypeComparer.Default) && (watcherState2 = this.HasPair(index)) != null))
            {
              watcherState1.Action = WatchersHost.WatcherAction.Spotted;
              watcherState2.StartTime = watcherState1.StartTime = gameTime.TotalGameTime;
              if (!watcherState1.SkipNextSound)
              {
                SoundEffectExtensions.EmitAt(this.seeSound, index.Center);
                watcherState2.SkipNextSound = true;
                break;
              }
              else
                break;
            }
            else
              break;
          case WatchersHost.WatcherAction.Spotted:
            watcherState1.EyeOffset = Vector3.Lerp(watcherState1.EyeOffset, watcherState1.CrushDirection * 1f / 16f, 0.25f);
            if ((gameTime.TotalGameTime - watcherState1.StartTime).TotalSeconds > 1.0)
            {
              watcherState1.Action = WatchersHost.WatcherAction.Crushing;
              watcherState1.StartTime = gameTime.TotalGameTime;
              index.PhysicsState.Velocity = watcherState1.OriginalCenter - index.Center;
              this.PhysicsManager.Update((ISimplePhysicsEntity) index.PhysicsState, true, false);
              index.PhysicsState.UpdateInstance();
              this.LevelManager.UpdateInstance(index);
              watcherState1.MoveEmitter = watcherState1.SkipNextSound ? (SoundEmitter) null : SoundEffectExtensions.EmitAt(this.moveSound, index.Center);
              break;
            }
            else
            {
              Vector3 vector3_8 = watcherState1.CrushDirection * RandomHelper.Unit() * 0.5f / 16f;
              index.PhysicsState.Sticky = true;
              index.PhysicsState.Velocity = watcherState1.OriginalCenter + vector3_8 - index.Center;
              this.PhysicsManager.Update((ISimplePhysicsEntity) index.PhysicsState, true, false);
              index.PhysicsState.UpdateInstance();
              this.LevelManager.UpdateInstance(index);
              break;
            }
          case WatchersHost.WatcherAction.Crushing:
            if (index.PhysicsState.Sticky)
            {
              index.PhysicsState.Sticky = false;
              index.PhysicsState.Velocity = Vector3.Zero;
            }
            watcherState1.EyeOffset = watcherState1.CrushDirection * 1f / 16f;
            Vector3 vector3_9 = watcherState1.CrushDirection * (float) gameTime.ElapsedGameTime.TotalSeconds * 15f;
            Vector3 vector3_10 = Vector3.Lerp(index.PhysicsState.Velocity, vector3_9, 0.025f);
            index.PhysicsState.Velocity = vector3_10 * watcherState1.CrashAttenuation;
            if (FezMath.VisibleAxis(this.CameraManager.Viewpoint) != FezMath.AsAxis(FezMath.OrientationFromDirection(watcherState1.CrushDirection)))
              this.PhysicsManager.Update((ISimplePhysicsEntity) index.PhysicsState, false, false);
            Vector3 vector3_11 = vector3_10 * watcherState1.CrashAttenuation - index.PhysicsState.Velocity;
            if (watcherState1.MoveEmitter != null)
              watcherState1.MoveEmitter.Position = index.Center;
            index.PhysicsState.UpdateInstance();
            this.LevelManager.UpdateInstance(index);
            this.PlayerManager.ForceOverlapsDetermination();
            bool flag4 = this.PlayerManager.HeldInstance == index || this.PlayerManager.WallCollision.FarHigh.Destination == index || (this.PlayerManager.WallCollision.NearLow.Destination == index || this.PlayerManager.Ground.NearLow == index) || this.PlayerManager.Ground.FarHigh == index;
            if (!flag4)
            {
              foreach (PointCollision pointCollision in this.PlayerManager.CornerCollision)
              {
                if (pointCollision.Instances.Deep == index)
                {
                  flag4 = true;
                  break;
                }
              }
            }
            if (flag1 && flag4 && this.lastCrushDirections.Contains(-watcherState1.CrushDirection))
            {
              this.PlayerManager.Position = index.Center + Vector3.One / 2f * watcherState1.CrushDirection + -FezMath.SideMask(this.CameraManager.Viewpoint) * FezMath.Abs(watcherState1.CrushDirection) * 1.5f / 16f;
              this.PlayerManager.Velocity = Vector3.Zero;
              this.PlayerManager.Action = (double) watcherState1.CrushDirection.Y == 0.0 ? ActionType.CrushHorizontal : ActionType.CrushVertical;
              watcherState1.CrashAttenuation = this.PlayerManager.Action == ActionType.CrushVertical ? 0.5f : 0.75f;
            }
            flag1 = flag1 | flag4;
            if (flag4 && this.PlayerManager.Action != ActionType.CrushHorizontal && this.PlayerManager.Action != ActionType.CrushVertical)
            {
              this.lastCrushDirections.Add(watcherState1.CrushDirection);
              if ((double) watcherState1.CrushDirection.Y == 0.0)
                this.PlayerManager.Position += index.PhysicsState.Velocity;
            }
            if ((double) vector3_11.LengthSquared() > 4.99999987368938E-05 || (double) Math.Abs(Vector3.Dot(index.Center - watcherState1.OriginalCenter, FezMath.Abs(watcherState1.CrushDirection))) >= 8.0)
            {
              if (watcherState1.MoveEmitter != null && !watcherState1.MoveEmitter.Dead)
                watcherState1.MoveEmitter.Cue.Stop(false);
              watcherState1.MoveEmitter = (SoundEmitter) null;
              if (!watcherState1.SkipNextSound)
                SoundEffectExtensions.EmitAt(this.collideSound, index.Center);
              watcherState1.Action = WatchersHost.WatcherAction.Wait;
              index.PhysicsState.Velocity = Vector3.Zero;
              watcherState1.StartTime = TimeSpan.Zero;
              watcherState1.CrashAttenuation = 1f;
              break;
            }
            else
              break;
          case WatchersHost.WatcherAction.Wait:
            watcherState1.StartTime += gameTime.ElapsedGameTime;
            if (watcherState1.StartTime.TotalSeconds > 1.5)
            {
              watcherState1.Action = WatchersHost.WatcherAction.Withdrawing;
              watcherState1.StartTime = gameTime.TotalGameTime;
              watcherState1.WithdrawEmitter = watcherState1.SkipNextSound ? (SoundEmitter) null : SoundEffectExtensions.EmitAt(this.withdrawSound, index.Center, true);
              break;
            }
            else
              break;
          case WatchersHost.WatcherAction.Withdrawing:
            watcherState1.EyeOffset = Vector3.Lerp(watcherState1.EyeOffset, -watcherState1.CrushDirection * 0.5f / 16f, 0.05f);
            Vector3 vector3_12 = -watcherState1.CrushDirection * (float) gameTime.ElapsedGameTime.TotalSeconds * 2f;
            index.PhysicsState.Velocity = Vector3.Lerp(index.PhysicsState.Velocity, vector3_12, 0.025f);
            if (watcherState1.WithdrawEmitter != null)
              watcherState1.WithdrawEmitter.VolumeFactor = 0.0f;
            bool flag5 = false;
            if (FezMath.DepthMask(this.CameraManager.Viewpoint) == FezMath.GetMask(FezMath.AsAxis(FezMath.OrientationFromDirection(watcherState1.CrushDirection))))
              flag5 = true;
            if (watcherState1.WithdrawEmitter != null)
              watcherState1.WithdrawEmitter.VolumeFactor = 1f;
            Vector3 center2 = index.PhysicsState.Center;
            Vector3 velocity = index.PhysicsState.Velocity;
            this.PhysicsManager.Update((ISimplePhysicsEntity) index.PhysicsState, true, false);
            index.PhysicsState.Center = center2 + velocity;
            if (watcherState1.WithdrawEmitter != null)
              watcherState1.WithdrawEmitter.Position = index.Center;
            if (flag5 ? (double) Math.Abs(Vector3.Dot(index.Center - watcherState1.OriginalCenter, vector3_1 + Vector3.Up)) <= 1.0 / 32.0 : (double) Vector3.Dot(index.Center - watcherState1.OriginalCenter, watcherState1.CrushDirection) <= 1.0 / 1000.0)
            {
              if (watcherState1.WithdrawEmitter != null)
              {
                watcherState1.WithdrawEmitter.FadeOutAndDie(0.1f);
                watcherState1.WithdrawEmitter = (SoundEmitter) null;
              }
              watcherState1.SkipNextSound = false;
              watcherState1.Action = WatchersHost.WatcherAction.Cooldown;
              watcherState1.CrushDirection = Vector3.Zero;
              watcherState1.StartTime = TimeSpan.Zero;
            }
            index.PhysicsState.UpdateInstance();
            this.LevelManager.UpdateInstance(index);
            break;
          case WatchersHost.WatcherAction.Cooldown:
            index.PhysicsState.Velocity = watcherState1.OriginalCenter - index.Center;
            this.PhysicsManager.Update((ISimplePhysicsEntity) index.PhysicsState, true, false);
            index.PhysicsState.UpdateInstance();
            this.LevelManager.UpdateInstance(index);
            watcherState1.EyeOffset = Vector3.Lerp(watcherState1.EyeOffset, Vector3.Zero, 0.05f);
            watcherState1.Eyes.Material.Opacity = 0.5f;
            watcherState1.StartTime += gameTime.ElapsedGameTime;
            if (watcherState1.StartTime.TotalSeconds > 0.5)
            {
              index.PhysicsState.Velocity = Vector3.Zero;
              watcherState1.Action = WatchersHost.WatcherAction.Idle;
              break;
            }
            else
              break;
        }
        Vector3 vector3_13 = index.PhysicsState.Center + vector3_2 * -5f / 16f + Vector3.UnitY * -2f / 16f - 0.5f * vector3_3;
        watcherState1.Eyes.Groups[0].Position = vector3_13 + watcherState1.EyeOffset;
        watcherState1.Eyes.Groups[1].Position = vector3_13 + vector3_2 * 9f / 16f + watcherState1.EyeOffset;
        watcherState1.Eyes.Groups[2].Position = watcherState1.Eyes.Groups[0].Position;
        watcherState1.Eyes.Groups[3].Position = watcherState1.Eyes.Groups[1].Position;
        watcherState1.Eyes.Groups[0].Enabled = false;
        watcherState1.Eyes.Groups[1].Enabled = false;
      }
    }

    private WatchersHost.WatcherState HasPair(TrileInstance watcher)
    {
      WatchersHost.WatcherState watcherState1 = this.watchers[watcher];
      Vector3 b = FezMath.ScreenSpaceMask(this.CameraManager.Viewpoint);
      foreach (TrileInstance index in this.watchers.Keys)
      {
        if (index != watcher)
        {
          WatchersHost.WatcherState watcherState2 = this.watchers[index];
          if (watcherState1.CrushDirection == -watcherState2.CrushDirection && watcherState2.Action != WatchersHost.WatcherAction.Cooldown && (watcherState2.Action != WatchersHost.WatcherAction.Withdrawing && watcherState2.Action != WatchersHost.WatcherAction.Crushing) && ((double) Math.Abs(FezMath.Dot(watcherState1.OriginalCenter - watcherState2.OriginalCenter, b)) > 2.0 && (double) Math.Abs(FezMath.Dot(watcher.Center - index.Center, b)) > 2.0 && this.LevelManager.NearestTrile(index.Position + new Vector3(0.5f)).Deep == index))
            return watcherState2;
        }
      }
      return (WatchersHost.WatcherState) null;
    }

    public override void Draw(GameTime gameTime)
    {
      if (this.GameState.Loading || this.watchers.Count == 0)
        return;
      GraphicsDevice graphicsDevice = this.GraphicsDevice;
      GraphicsDeviceExtensions.PrepareStencilWrite(graphicsDevice, new StencilMask?(StencilMask.Level));
      foreach (WatchersHost.WatcherState watcherState in this.watchers.Values)
        watcherState.Eyes.Draw();
      GraphicsDeviceExtensions.PrepareStencilWrite(graphicsDevice, new StencilMask?(StencilMask.None));
    }

    private void PreDraw()
    {
      if (this.GameState.Loading || this.watchers.Count == 0)
        return;
      GraphicsDevice graphicsDevice = this.GraphicsDevice;
      GraphicsDeviceExtensions.PrepareStencilWrite(graphicsDevice, new StencilMask?(StencilMask.Level));
      foreach (WatchersHost.WatcherState watcherState in this.watchers.Values)
      {
        (watcherState.Eyes.Effect as DefaultEffect).Pass = LightingEffectPass.Pre;
        watcherState.Eyes.Draw();
        (watcherState.Eyes.Effect as DefaultEffect).Pass = LightingEffectPass.Main;
      }
      GraphicsDeviceExtensions.PrepareStencilWrite(graphicsDevice, new StencilMask?(StencilMask.None));
    }

    private class WatcherState
    {
      public WatchersHost.WatcherAction Action { get; set; }

      public TimeSpan StartTime { get; set; }

      public Vector3 OriginalCenter { get; set; }

      public Vector3 CrushDirection { get; set; }

      public Mesh Eyes { get; set; }

      public SoundEmitter MoveEmitter { get; set; }

      public SoundEmitter WithdrawEmitter { get; set; }

      public Vector3 EyeOffset { get; set; }

      public float CrashAttenuation { get; set; }

      public bool SkipNextSound { get; set; }
    }

    private enum WatcherAction
    {
      Idle,
      Spotted,
      Crushing,
      Wait,
      Withdrawing,
      Cooldown,
    }
  }
}
