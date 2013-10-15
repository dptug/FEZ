// Type: FezGame.Components.GeezerLetterSender
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Services;
using FezEngine.Services.Scripting;
using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FezGame.Components
{
  internal class GeezerLetterSender : DrawableGameComponent
  {
    private int? NpcId;
    private NpcInstance Npc;
    private bool Walking;
    private float SinceStarted;
    private BackgroundPlane Plane;
    private float SinceGotThere;
    private Vector3 OldPosition;
    private Vector3 OldDestinationOffset;
    private bool hooked;
    private SoundEffect sLetterInsert;

    [ServiceDependency]
    public IVolumeService VolumeService { get; set; }

    [ServiceDependency]
    public IGomezService GomezService { get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { get; set; }

    [ServiceDependency]
    public IGameCameraManager CameraManager { get; set; }

    [ServiceDependency]
    public IGameLevelManager LevelManager { get; set; }

    [ServiceDependency]
    public ILevelMaterializer LevelMaterializer { get; set; }

    public GeezerLetterSender(Game game, int npcId)
      : base(game)
    {
      this.NpcId = new int?(npcId);
      this.DrawOrder = 99;
    }

    public GeezerLetterSender(Game game)
      : base(game)
    {
      this.NpcId = new int?();
      this.DrawOrder = 99;
    }

    public override void Initialize()
    {
      base.Initialize();
      this.LevelManager.AddPlane(this.Plane = new BackgroundPlane(this.LevelMaterializer.StaticPlanesMesh, (Texture) this.CMProvider.CurrentLevel.Load<Texture2D>("Other Textures/CAPSULE"))
      {
        Rotation = this.CameraManager.Rotation,
        Loop = false
      });
      if (this.NpcId.HasValue)
      {
        this.Npc = this.LevelManager.NonPlayerCharacters[this.NpcId.Value];
        this.OldPosition = this.Npc.Position;
        this.Npc.Position = new Vector3(487.0 / 16.0, 49f, 10f);
        this.OldDestinationOffset = this.Npc.DestinationOffset;
        this.Npc.DestinationOffset = new Vector3(-63.0 / 16.0, 0.0f, 0.0f);
        this.Npc.State.Scripted = true;
        this.Npc.State.LookingDirection = HorizontalDirection.Left;
        this.Npc.State.WalkStep = 0.0f;
        this.Npc.State.CurrentAction = NpcAction.Idle;
        this.Npc.State.UpdateAction();
        this.Npc.State.SyncTextureMatrix();
        this.Npc.Group.Position = this.LevelManager.NonPlayerCharacters[this.NpcId.Value].Position;
        this.CameraManager.Constrained = true;
        this.CameraManager.Center = new Vector3(32.5f, 50.5f, 16.5f);
        this.CameraManager.SnapInterpolation();
        this.Plane.Position = this.Npc.Group.Position + new Vector3((float) (FezMath.Sign(this.Npc.State.LookingDirection) * 4) / 16f, 0.375f, 0.0f);
        this.sLetterInsert = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/MiscActors/LetterTubeInsert");
      }
      else
      {
        this.Plane.Position = new Vector3(20.5f, 20.75f, 23.5f);
        this.Enabled = false;
        this.GomezService.ReadMail += new Action(this.Destroy);
      }
      this.LevelManager.LevelChanged += new Action(this.TryDestroy);
    }

    private void TryDestroy()
    {
      ServiceHelper.RemoveComponent<GeezerLetterSender>(this);
    }

    protected override void Dispose(bool disposing)
    {
      base.Dispose(disposing);
      this.GomezService.ReadMail -= new Action(this.Destroy);
      this.LevelManager.LevelChanged -= new Action(this.TryDestroy);
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.TimePaused || this.GameState.Loading)
        return;
      this.CameraManager.Constrained = false;
      this.SinceStarted += (float) gameTime.ElapsedGameTime.TotalSeconds;
      if ((double) this.SinceStarted > 3.0 && !this.Walking)
      {
        this.Walking = true;
        this.Npc.State.LookingDirection = HorizontalDirection.Right;
        this.Npc.State.CurrentAction = NpcAction.Walk;
        this.Npc.State.UpdateAction();
        this.Npc.State.SyncTextureMatrix();
      }
      if (this.Npc.State.CurrentAction == NpcAction.Walk && (double) this.Npc.State.WalkStep == 1.0)
      {
        this.Npc.State.CurrentAction = NpcAction.Idle;
        this.Npc.State.UpdateAction();
        this.Npc.State.SyncTextureMatrix();
      }
      if ((double) this.Npc.State.WalkStep == 1.0)
      {
        this.SinceGotThere += (float) gameTime.ElapsedGameTime.TotalSeconds;
        if ((double) this.SinceGotThere < 0.5)
        {
          if (this.sLetterInsert != null)
          {
            SoundEffectExtensions.Emit(this.sLetterInsert);
            this.sLetterInsert = (SoundEffect) null;
          }
          this.Plane.Position = this.Npc.Group.Position + new Vector3((float) (FezMath.Sign(this.Npc.State.LookingDirection) * 4) / 16f, 0.375f, 0.0f) + new Vector3(-Easing.EaseIn((double) this.SinceGotThere / 0.5, EasingType.Quadratic), 0.375f, 0.0f);
        }
        if ((double) this.SinceGotThere > 12.5 && (double) this.SinceGotThere < 14.5)
          this.Plane.Position = new Vector3(20.5f, 20.75f + Easing.EaseOut((double) FezMath.Saturate((float) ((13.25 - (double) this.SinceGotThere) * 2.0)), EasingType.Cubic), 23.5f);
        if (this.hooked)
          return;
        this.GomezService.ReadMail += new Action(this.Destroy);
        this.hooked = true;
      }
      else
        this.Plane.Position = this.Npc.Group.Position + new Vector3((float) (FezMath.Sign(this.Npc.State.LookingDirection) * 4) / 16f, 0.375f, 0.0f);
    }

    private void Destroy()
    {
      this.LevelManager.RemovePlane(this.Plane);
      if (this.NpcId.HasValue)
      {
        this.Npc.Position = this.OldPosition;
        this.Npc.DestinationOffset = this.OldDestinationOffset;
        this.Npc.State.WalkStep = 0.25f;
        this.Npc.State.Scripted = false;
      }
      ServiceHelper.RemoveComponent<GeezerLetterSender>(this);
    }
  }
}
