// Type: FezGame.Components.SuckBlocksHost
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Components;
using FezEngine.Services;
using FezEngine.Services.Scripting;
using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace FezGame.Components
{
  internal class SuckBlocksHost : GameComponent
  {
    private readonly List<SuckBlocksHost.SuckBlockState> TrackedSuckBlocks = new List<SuckBlocksHost.SuckBlockState>();
    private readonly List<Volume> HostingVolumes = new List<Volume>();
    private readonly Ray[] cornerRays = new Ray[4];
    private SoundEmitter eCratePush;
    private SoundEmitter eSuck;
    private SoundEffect sDenied;
    private SoundEffect sSuck;
    private SoundEffect[] sAccept;
    private List<BackgroundPlane> highlightPlanes;

    [ServiceDependency]
    public ISoundManager SoundManager { private get; set; }

    [ServiceDependency]
    public ISuckBlockService SuckBlockService { private get; set; }

    [ServiceDependency]
    public ILevelMaterializer LevelMaterializer { private get; set; }

    [ServiceDependency]
    public IGameLevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public IPlayerManager PlayerManager { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public IDefaultCameraManager CameraManager { private get; set; }

    [ServiceDependency]
    public IDebuggingBag DebuggingBag { private get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { private get; set; }

    public SuckBlocksHost(Game game)
      : base(game)
    {
    }

    public override void Initialize()
    {
      this.LevelManager.LevelChanged += new Action(this.InitSuckBlocks);
      if (this.LevelManager.Name != null)
        this.InitSuckBlocks();
      this.sAccept = new SoundEffect[4]
      {
        this.CMProvider.Global.Load<SoundEffect>("Sounds/MiscActors/AcceptSuckBlock1"),
        this.CMProvider.Global.Load<SoundEffect>("Sounds/MiscActors/AcceptSuckBlock2"),
        this.CMProvider.Global.Load<SoundEffect>("Sounds/MiscActors/AcceptSuckBlock3"),
        this.CMProvider.Global.Load<SoundEffect>("Sounds/MiscActors/AcceptSuckBlock4")
      };
      this.sDenied = this.CMProvider.Global.Load<SoundEffect>("Sounds/MiscActors/Denied");
      this.sSuck = this.CMProvider.Global.Load<SoundEffect>("Sounds/MiscActors/SuckBlockSuck");
    }

    private void InitSuckBlocks()
    {
      this.HostingVolumes.Clear();
      this.TrackedSuckBlocks.Clear();
      this.highlightPlanes = (List<BackgroundPlane>) null;
      this.eCratePush = this.eSuck = (SoundEmitter) null;
      foreach (TrileGroup group in (IEnumerable<TrileGroup>) this.LevelManager.Groups.Values)
      {
        if (group.ActorType == ActorType.SuckBlock)
        {
          TrileInstance instance = Enumerable.First<TrileInstance>((IEnumerable<TrileInstance>) group.Triles);
          if (instance.ActorSettings.HostVolume.HasValue)
          {
            this.TrackedSuckBlocks.Add(new SuckBlocksHost.SuckBlockState(instance, group));
            SuckBlocksHost.EnableTrile(instance);
            this.HostingVolumes.Add(this.LevelManager.Volumes[instance.ActorSettings.HostVolume.Value]);
          }
        }
      }
      if (this.TrackedSuckBlocks.Count <= 0)
        return;
      this.eCratePush = SoundEffectExtensions.EmitAt(this.CMProvider.Global.Load<SoundEffect>("Sounds/Gomez/PushPickup"), Vector3.Zero, true, true);
      this.highlightPlanes = new List<BackgroundPlane>();
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Paused || this.GameState.InMap || (!this.CameraManager.ActionRunning || !FezMath.IsOrthographic(this.CameraManager.Viewpoint)) || (this.GameState.Loading || this.TrackedSuckBlocks.Count == 0))
        return;
      FaceOrientation visibleOrientation = this.CameraManager.VisibleOrientation;
      Vector3 vector3_1 = FezMath.ForwardVector(this.CameraManager.Viewpoint);
      Vector3 vector3_2 = FezMath.DepthMask(this.CameraManager.Viewpoint);
      Vector3 vector3_3 = FezMath.ScreenSpaceMask(this.CameraManager.Viewpoint);
      Vector3 vector3_4 = vector3_3 / 2f;
      bool flag1 = false;
      foreach (SuckBlocksHost.SuckBlockState suckBlockState in this.TrackedSuckBlocks)
      {
        TrileInstance instance = suckBlockState.Instance;
        if (this.PlayerManager.HeldInstance != instance)
        {
          int num1 = instance.ActorSettings.HostVolume.Value;
          Vector3 vector3_5 = instance.Center * (Vector3.One - vector3_2) + this.CameraManager.Position * vector3_2;
          this.cornerRays[0] = new Ray()
          {
            Position = vector3_5 + vector3_4 * new Vector3(1f, 0.499f, 1f),
            Direction = vector3_1
          };
          this.cornerRays[1] = new Ray()
          {
            Position = vector3_5 + vector3_4 * new Vector3(1f, -1f, 1f),
            Direction = vector3_1
          };
          this.cornerRays[2] = new Ray()
          {
            Position = vector3_5 + vector3_4 * new Vector3(-1f, 0.499f, -1f),
            Direction = vector3_1
          };
          this.cornerRays[3] = new Ray()
          {
            Position = vector3_5 + vector3_4 * new Vector3(-1f, -1f, -1f),
            Direction = vector3_1
          };
          suckBlockState.Update(gameTime.ElapsedGameTime);
          this.eCratePush.Position = instance.Center;
          bool flag2 = false;
          foreach (Volume volume in this.HostingVolumes)
          {
            if (volume.Orientations.Contains(visibleOrientation))
            {
              bool flag3 = false;
              foreach (Ray ray in this.cornerRays)
                flag3 = flag3 | volume.BoundingBox.Intersects(ray).HasValue;
              if (flag3)
              {
                flag2 = true;
                if (suckBlockState.Action == SuckBlocksHost.SuckBlockAction.Sucking && (this.eSuck == null || this.eSuck.Dead))
                  this.eSuck = SoundEffectExtensions.EmitAt(this.sSuck, instance.Center, true);
                flag1 = ((flag1 ? 1 : 0) | (suckBlockState.Action == SuckBlocksHost.SuckBlockAction.Sucking ? 1 : (suckBlockState.Action == SuckBlocksHost.SuckBlockAction.Processing ? 1 : 0))) != 0;
                Vector3 vector3_6 = (volume.BoundingBox.Min + volume.BoundingBox.Max) / 2f;
                Vector3 vector3_7 = (vector3_6 - instance.Center) * vector3_3;
                float num2 = vector3_7.Length();
                if ((double) num2 < 0.00999999977648258)
                {
                  if (suckBlockState.Action == SuckBlocksHost.SuckBlockAction.Sucking)
                  {
                    suckBlockState.Action = SuckBlocksHost.SuckBlockAction.Processing;
                    this.PlayerManager.CanRotate = false;
                    this.eCratePush.VolumeFactor = 0.5f;
                    this.eCratePush.Cue.Pitch = -0.4f;
                  }
                  if (suckBlockState.Action == SuckBlocksHost.SuckBlockAction.Processing)
                  {
                    Vector3 vector3_8 = (volume.BoundingBox.Max - volume.BoundingBox.Min) / 2f;
                    Vector3 vector3_9 = volume.BoundingBox.Min * vector3_3 + vector3_6 * vector3_2 + vector3_8 * vector3_1 - vector3_1 * 0.5f - vector3_2 * 0.5f;
                    Vector3 vector3_10 = vector3_9 - vector3_1;
                    instance.Position = Vector3.Lerp(vector3_10, vector3_9, (float) suckBlockState.SinceActionChanged.Ticks / (float) SuckBlocksHost.SuckBlockState.ProcessingTime.Ticks);
                    this.LevelManager.UpdateInstance(instance);
                    if (suckBlockState.SinceActionChanged > SuckBlocksHost.SuckBlockState.ProcessingTime)
                    {
                      this.PlayerManager.CanRotate = true;
                      if (volume.Id == num1)
                      {
                        // ISSUE: object of a compiler-generated type is created
                        // ISSUE: variable of a compiler-generated type
                        SuckBlocksHost.\u003C\u003Ec__DisplayClassb cDisplayClassb = new SuckBlocksHost.\u003C\u003Ec__DisplayClassb();
                        SuckBlocksHost.DisableTrile(instance);
                        suckBlockState.Action = SuckBlocksHost.SuckBlockAction.Accepted;
                        if (this.eCratePush.Cue.State != SoundState.Paused)
                          this.eCratePush.Cue.Pause();
                        this.SuckBlockService.OnSuck(suckBlockState.Group.Id);
                        SoundEffectExtensions.Emit(this.sAccept[4 - this.TrackedSuckBlocks.Count]);
                        Texture2D texture2D = this.CMProvider.CurrentLevel.Load<Texture2D>("Other Textures/suck_blocks/four_highlight_" + instance.Trile.CubemapPath.Substring(instance.Trile.CubemapPath.Length - 1).ToLower(CultureInfo.InvariantCulture));
                        // ISSUE: reference to a compiler-generated field
                        cDisplayClassb.plane = new BackgroundPlane(this.LevelMaterializer.StaticPlanesMesh, (Texture) texture2D)
                        {
                          Position = instance.Center + FezMath.AsVector(visibleOrientation) * (17.0 / 32.0),
                          Rotation = this.CameraManager.Rotation,
                          Doublesided = true,
                          Fullbright = true,
                          Opacity = 0.0f
                        };
                        // ISSUE: reference to a compiler-generated field
                        this.highlightPlanes.Add(cDisplayClassb.plane);
                        // ISSUE: reference to a compiler-generated field
                        this.LevelManager.AddPlane(cDisplayClassb.plane);
                        // ISSUE: reference to a compiler-generated method
                        Waiters.Interpolate(1.0, new Action<float>(cDisplayClassb.\u003CUpdate\u003Eb__6));
                        if (this.TrackedSuckBlocks.Count == 1)
                        {
                          Waiters.Wait(2.0, (Action) (() => Waiters.Interpolate(1.0, (Action<float>) (s =>
                          {
                            foreach (BackgroundPlane item_3 in this.highlightPlanes)
                              item_3.Opacity = 1f - s;
                          }), (Action) (() => this.eSuck = (SoundEmitter) null))));
                          this.eSuck.FadeOutAndDie(1f);
                        }
                      }
                      else
                        suckBlockState.Action = SuckBlocksHost.SuckBlockAction.Rejected;
                    }
                  }
                  if (suckBlockState.Action == SuckBlocksHost.SuckBlockAction.Rejected && FezMath.XZ(instance.PhysicsState.Velocity) == Vector2.Zero)
                  {
                    int num3 = RandomHelper.Probability(0.5) ? -1 : 1;
                    Vector3 vector3_8 = new Vector3((float) num3, 0.75f, (float) num3) * vector3_3;
                    ServiceHelper.AddComponent((IGameComponent) new CamShake(this.Game)
                    {
                      Distance = 0.1f,
                      Duration = TimeSpan.FromSeconds(0.25)
                    });
                    SoundEffectExtensions.Emit(this.sDenied);
                    if (this.eCratePush.Cue.State != SoundState.Paused)
                      this.eCratePush.Cue.Pause();
                    instance.PhysicsState.Velocity += 6f * vector3_8 * (float) gameTime.ElapsedGameTime.TotalSeconds;
                  }
                }
                else if (suckBlockState.Action != SuckBlocksHost.SuckBlockAction.Rejected)
                {
                  if (instance.PhysicsState.Grounded && this.eCratePush.Cue.State != SoundState.Playing)
                  {
                    this.eCratePush.Cue.Pitch = 0.0f;
                    this.eCratePush.Cue.Resume();
                  }
                  else if (!instance.PhysicsState.Grounded && this.eCratePush.Cue.State != SoundState.Paused)
                    this.eCratePush.Cue.Pause();
                  if (this.eCratePush.Cue.State == SoundState.Playing)
                    this.eCratePush.VolumeFactor = FezMath.Saturate(Math.Abs(FezMath.Dot(instance.PhysicsState.Velocity, FezMath.XZMask) / 0.1f));
                  suckBlockState.Action = SuckBlocksHost.SuckBlockAction.Sucking;
                  instance.PhysicsState.Velocity += 0.25f * vector3_7 / num2 * (float) gameTime.ElapsedGameTime.TotalSeconds;
                }
              }
            }
          }
          if (!flag2)
            suckBlockState.Action = SuckBlocksHost.SuckBlockAction.Idle;
        }
      }
      if (!flag1 && this.eSuck != null && !this.eSuck.Dead)
      {
        this.eSuck.FadeOutAndDie(0.1f);
        this.eSuck = (SoundEmitter) null;
      }
      for (int index = 0; index < this.TrackedSuckBlocks.Count; ++index)
      {
        if (this.TrackedSuckBlocks[index].Action == SuckBlocksHost.SuckBlockAction.Accepted)
        {
          this.TrackedSuckBlocks.RemoveAt(index);
          --index;
        }
      }
    }

    private static void DisableTrile(TrileInstance instance)
    {
      Trile trile = instance.Trile;
      trile.ActorSettings.Type = ActorType.None;
      trile.Faces[FaceOrientation.Left] = trile.Faces[FaceOrientation.Right] = trile.Faces[FaceOrientation.Back] = trile.Faces[FaceOrientation.Front] = CollisionType.None;
    }

    private static void EnableTrile(TrileInstance instance)
    {
      Trile trile = instance.Trile;
      trile.ActorSettings.Type = ActorType.SinkPickup;
      trile.Faces[FaceOrientation.Left] = trile.Faces[FaceOrientation.Right] = trile.Faces[FaceOrientation.Back] = trile.Faces[FaceOrientation.Front] = CollisionType.AllSides;
    }

    private class SuckBlockState
    {
      public static readonly TimeSpan ProcessingTime = TimeSpan.FromSeconds(0.5);
      public readonly TrileInstance Instance;
      public readonly TrileGroup Group;
      private SuckBlocksHost.SuckBlockAction action;

      public TimeSpan SinceActionChanged { get; private set; }

      public SuckBlocksHost.SuckBlockAction Action
      {
        get
        {
          return this.action;
        }
        set
        {
          if (this.action != value)
            this.SinceActionChanged = TimeSpan.Zero;
          this.action = value;
        }
      }

      static SuckBlockState()
      {
      }

      public SuckBlockState(TrileInstance instance, TrileGroup group)
      {
        this.Instance = instance;
        this.Group = group;
      }

      public void Update(TimeSpan elapsed)
      {
        this.SinceActionChanged += elapsed;
      }
    }

    private enum SuckBlockAction
    {
      Idle,
      Processing,
      Sucking,
      Rejected,
      Accepted,
    }
  }
}
