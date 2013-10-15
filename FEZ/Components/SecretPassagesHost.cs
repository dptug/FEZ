// Type: FezGame.Components.SecretPassagesHost
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Components;
using FezEngine.Services;
using FezEngine.Services.Scripting;
using FezEngine.Structure;
using FezEngine.Structure.Input;
using FezEngine.Structure.Scripting;
using FezEngine.Tools;
using FezGame.Components.Actions;
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
  internal class SecretPassagesHost : GameComponent
  {
    private ArtObjectInstance DoorAo;
    private Volume AssociatedVolume;
    private TrileGroup AttachedGroup;
    private bool Accessible;
    private BackgroundPlane GlowPlane;
    private Viewpoint ExpectedViewpoint;
    private bool MoveUp;
    private TimeSpan SinceStarted;
    private Vector3 AoOrigin;
    private Vector3 PlaneOrigin;
    private SoundEffect sRumble;
    private SoundEffect sLightUp;
    private SoundEffect sFadeOut;
    private SoundEmitter eRumble;
    private bool loop;

    [ServiceDependency]
    public IWalkToService WalkToService { private get; set; }

    [ServiceDependency]
    public IInputManager InputManager { private get; set; }

    [ServiceDependency]
    public IGroupService GroupService { private get; set; }

    [ServiceDependency]
    public IGameCameraManager CameraManager { private get; set; }

    [ServiceDependency]
    public IPlayerManager PlayerManager { private get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { private get; set; }

    [ServiceDependency]
    public IGameLevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public ILevelMaterializer LevelMaterializer { private get; set; }

    public SecretPassagesHost(Game game)
      : base(game)
    {
    }

    public override void Initialize()
    {
      base.Initialize();
      this.LevelManager.LevelChanging += new Action(this.TryInitialize);
      this.TryInitialize();
    }

    private void TryInitialize()
    {
      this.DoorAo = (ArtObjectInstance) null;
      this.AttachedGroup = (TrileGroup) null;
      this.AssociatedVolume = (Volume) null;
      this.Enabled = false;
      this.sRumble = (SoundEffect) null;
      this.sLightUp = (SoundEffect) null;
      this.sFadeOut = (SoundEffect) null;
      if (this.eRumble != null && !this.eRumble.Dead)
        this.eRumble.Cue.Stop(false);
      this.eRumble = (SoundEmitter) null;
      foreach (ArtObjectInstance artObjectInstance in (IEnumerable<ArtObjectInstance>) this.LevelManager.ArtObjects.Values)
      {
        if (artObjectInstance.ArtObject.ActorType == ActorType.SecretPassage)
        {
          this.DoorAo = artObjectInstance;
          this.Enabled = true;
          break;
        }
      }
      if (!this.Enabled)
      {
        if (this.GlowPlane != null)
          this.GlowPlane.Dispose();
        this.GlowPlane = (BackgroundPlane) null;
      }
      if (!this.Enabled)
        return;
      this.AttachedGroup = this.LevelManager.Groups[this.DoorAo.ActorSettings.AttachedGroup.Value];
      this.AssociatedVolume = Enumerable.FirstOrDefault<Volume>((IEnumerable<Volume>) this.LevelManager.Volumes.Values, (Func<Volume, bool>) (x =>
      {
        if (x.ActorSettings != null)
          return x.ActorSettings.IsSecretPassage;
        else
          return false;
      }));
      string key = (string) null;
      foreach (Script script in (IEnumerable<Script>) this.LevelManager.Scripts.Values)
      {
        foreach (ScriptAction scriptAction in script.Actions)
        {
          if (scriptAction.Object.Type == "Level" && scriptAction.Operation.Contains("Level"))
          {
            foreach (ScriptTrigger scriptTrigger in script.Triggers)
            {
              if (scriptTrigger.Object.Type == "Volume" && scriptTrigger.Event == "Enter" && scriptTrigger.Object.Identifier.HasValue)
                key = scriptAction.Arguments[0];
            }
          }
        }
      }
      this.Accessible = this.GameState.SaveData.World.ContainsKey(key);
      this.sRumble = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/MiscActors/Rumble");
      this.sLightUp = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Zu/DoorBitLightUp");
      this.sFadeOut = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Zu/DoorBitFadeOut");
      if (!this.Accessible)
      {
        this.Enabled = false;
      }
      else
      {
        this.GlowPlane = new BackgroundPlane(this.LevelMaterializer.StaticPlanesMesh, (Texture) this.CMProvider.CurrentLevel.Load<Texture2D>("Other Textures/glow/secret_passage"))
        {
          Fullbright = true,
          Opacity = 0.0f,
          Position = this.DoorAo.Position + FezMath.AsVector(Enumerable.First<FaceOrientation>((IEnumerable<FaceOrientation>) this.AssociatedVolume.Orientations)) / (65.0 / 32.0),
          Rotation = FezMath.QuaternionFromPhi(FezMath.ToPhi(Enumerable.First<FaceOrientation>((IEnumerable<FaceOrientation>) this.AssociatedVolume.Orientations))),
          AttachedGroup = new int?(this.AttachedGroup.Id)
        };
        this.LevelManager.AddPlane(this.GlowPlane);
        this.ExpectedViewpoint = FezMath.AsViewpoint(Enumerable.First<FaceOrientation>((IEnumerable<FaceOrientation>) this.AssociatedVolume.Orientations));
        if (!this.LevelManager.WentThroughSecretPassage)
          return;
        this.MoveUp = true;
        this.GlowPlane.Opacity = 1f;
        this.SinceStarted = TimeSpan.Zero;
        this.AoOrigin = this.DoorAo.Position;
        this.PlaneOrigin = this.GlowPlane.Position;
      }
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Loading)
        return;
      if (this.MoveUp)
      {
        if (this.eRumble == null)
        {
          this.eRumble = SoundEffectExtensions.EmitAt(this.sRumble, this.DoorAo.Position, true, 0.0f, 0.625f);
          Waiters.Wait(1.25, (Action) (() => this.eRumble.FadeOutAndDie(0.25f))).AutoPause = true;
        }
        this.DoMoveUp(gameTime.ElapsedGameTime);
      }
      else
      {
        if (!this.DoorAo.Visible || this.DoorAo.ActorSettings.Inactive || (this.CameraManager.Viewpoint != this.ExpectedViewpoint || this.PlayerManager.Background))
          return;
        Vector3 vector3 = FezMath.Abs(this.DoorAo.Position - this.PlayerManager.Position) * FezMath.ScreenSpaceMask(this.CameraManager.Viewpoint);
        bool b = (double) vector3.X + (double) vector3.Z < 0.75 && (double) vector3.Y < 2.0 && (double) FezMath.Dot(this.DoorAo.Position - this.PlayerManager.Position, FezMath.ForwardVector(this.CameraManager.Viewpoint)) >= 0.0;
        float opacity = this.GlowPlane.Opacity;
        this.GlowPlane.Opacity = MathHelper.Lerp(this.GlowPlane.Opacity, (float) FezMath.AsNumeric(b), 0.05f);
        if ((double) this.GlowPlane.Opacity > (double) opacity && (double) opacity > 0.100000001490116 && this.loop)
        {
          SoundEffectExtensions.EmitAt(this.sLightUp, this.DoorAo.Position);
          this.loop = false;
        }
        else if ((double) this.GlowPlane.Opacity < (double) opacity && !this.loop)
        {
          SoundEffectExtensions.EmitAt(this.sFadeOut, this.DoorAo.Position);
          this.loop = true;
        }
        if (!b || !this.PlayerManager.Grounded || this.InputManager.ExactUp != FezButtonState.Pressed)
          return;
        this.Open();
      }
    }

    private void DoMoveUp(TimeSpan elapsed)
    {
      this.SinceStarted += elapsed;
      float amount = Easing.EaseInOut((double) FezMath.Saturate((float) this.SinceStarted.TotalSeconds / 2f), EasingType.Quadratic);
      int num = Math.Sign(this.AttachedGroup.Path.Segments[0].Destination.Y);
      this.GlowPlane.Position = Vector3.Lerp(this.PlaneOrigin + Vector3.UnitY * 2f * (float) num, this.PlaneOrigin, amount);
      this.DoorAo.Position = Vector3.Lerp(this.AoOrigin + Vector3.UnitY * 2f * (float) num, this.AoOrigin, amount);
      if ((double) amount != 1.0)
        return;
      this.MoveUp = false;
    }

    private void Open()
    {
      this.GroupService.RunPathOnce(this.AttachedGroup.Id, false);
      this.PlayerManager.CanControl = false;
      this.Enabled = false;
      this.PlayerManager.Action = ActionType.WalkingTo;
      this.WalkToService.Destination = new Func<Vector3>(this.GetDestination);
      this.WalkToService.NextAction = ActionType.Idle;
      this.eRumble = SoundEffectExtensions.EmitAt(this.sRumble, this.DoorAo.Position, true);
      Waiters.Wait(1.5, (Action) (() => this.eRumble.FadeOutAndDie(0.5f))).AutoPause = true;
      Waiters.Wait(2.0, (Action) (() =>
      {
        this.PlayerManager.CanControl = true;
        this.PlayerManager.Action = ActionType.OpeningDoor;
        this.PlayerManager.Action = ActionType.Idle;
      })).AutoPause = true;
    }

    private Vector3 GetDestination()
    {
      Viewpoint viewpoint = this.CameraManager.Viewpoint;
      return this.PlayerManager.Position * (Vector3.UnitY + FezMath.DepthMask(viewpoint)) + this.DoorAo.Position * FezMath.SideMask(viewpoint);
    }
  }
}
