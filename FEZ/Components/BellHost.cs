// Type: FezGame.Components.BellHost
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Components;
using FezEngine.Services;
using FezEngine.Services.Scripting;
using FezEngine.Structure;
using FezEngine.Structure.Input;
using FezEngine.Tools;
using FezGame.Services;
using FezGame.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezGame.Components
{
  internal class BellHost : GameComponent
  {
    private TimeSpan SinceHit = TimeSpan.FromSeconds(100.0);
    private readonly Dictionary<Viewpoint, int> Hits = new Dictionary<Viewpoint, int>(4, (IEqualityComparer<Viewpoint>) ViewpointComparer.Default);
    private readonly Dictionary<Viewpoint, int> ExpectedHits = new Dictionary<Viewpoint, int>(4, (IEqualityComparer<Viewpoint>) ViewpointComparer.Default)
    {
      {
        Viewpoint.Front,
        1
      },
      {
        Viewpoint.Back,
        3
      },
      {
        Viewpoint.Right,
        6
      },
      {
        Viewpoint.Left,
        10
      }
    };
    private ArtObjectInstance BellAo;
    private Vector2 AngularVelocity;
    private Vector2 Angle;
    private Vector3 OriginalPosition;
    private bool Solved;
    private Viewpoint LastHit;
    private SoundEffect[] sBellHit;
    private int stackedHits;
    private IWaiter wutex1;
    private IWaiter wutex2;

    [ServiceDependency]
    public ILevelService LevelService { get; set; }

    [ServiceDependency]
    public ISoundManager SoundManager { get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { get; set; }

    [ServiceDependency]
    public IInputManager InputManager { get; set; }

    [ServiceDependency]
    public IPlayerManager PlayerManager { get; set; }

    [ServiceDependency]
    public IGameCameraManager CameraManager { get; set; }

    [ServiceDependency]
    public ILevelManager LevelManager { get; set; }

    [ServiceDependency]
    public ILevelMaterializer LevelMaterializer { get; set; }

    public BellHost(Game game)
      : base(game)
    {
      this.UpdateOrder = 6;
    }

    public override void Initialize()
    {
      base.Initialize();
      this.TryInitialize();
      this.LevelManager.LevelChanged += new Action(this.TryInitialize);
    }

    private void TryInitialize()
    {
      this.sBellHit = (SoundEffect[]) null;
      this.BellAo = Enumerable.FirstOrDefault<ArtObjectInstance>((IEnumerable<ArtObjectInstance>) this.LevelManager.ArtObjects.Values, (Func<ArtObjectInstance, bool>) (x => x.ArtObject.ActorType == ActorType.Bell));
      this.Enabled = this.BellAo != null;
      if (!this.Enabled)
        return;
      this.OriginalPosition = this.BellAo.Position;
      this.Hits.Clear();
      this.Hits.Add(Viewpoint.Front, 0);
      this.Hits.Add(Viewpoint.Back, 0);
      this.Hits.Add(Viewpoint.Left, 0);
      this.Hits.Add(Viewpoint.Right, 0);
      this.sBellHit = new SoundEffect[4]
      {
        this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/MiscActors/BellHit1"),
        this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/MiscActors/BellHit2"),
        this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/MiscActors/BellHit3"),
        this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/MiscActors/BellHit4")
      };
      this.Solved = this.GameState.SaveData.ThisLevel.InactiveArtObjects.Contains(this.BellAo.Id);
      if (!this.Solved)
        return;
      this.LevelManager.ArtObjects.Remove(this.BellAo.Id);
      this.BellAo.Dispose();
      this.LevelMaterializer.RegisterSatellites();
      if (!this.GameState.SaveData.ThisLevel.DestroyedTriles.Contains(new TrileEmplacement(this.OriginalPosition)))
      {
        Trile trile = Enumerable.FirstOrDefault<Trile>(this.LevelManager.ActorTriles(ActorType.SecretCube));
        if (trile != null)
        {
          Vector3 position = this.OriginalPosition - Vector3.One / 2f;
          this.LevelManager.ClearTrile(new TrileEmplacement(position));
          TrileInstance toAdd;
          this.LevelManager.RestoreTrile(toAdd = new TrileInstance(position, trile.Id)
          {
            OriginalEmplacement = new TrileEmplacement(position)
          });
          toAdd.Foreign = true;
          if (toAdd.InstanceId == -1)
            this.LevelMaterializer.CullInstanceIn(toAdd);
        }
      }
      this.Enabled = false;
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Loading || this.GameState.Paused || (this.GameState.InMap || !this.CameraManager.ActionRunning) || (this.Solved || this.CameraManager.ProjectionTransition || !FezMath.IsOrthographic(this.CameraManager.Viewpoint)))
        return;
      this.SinceHit += gameTime.ElapsedGameTime;
      Vector3 vector = (this.PlayerManager.Position - this.BellAo.Position - new Vector3(0.0f, 1f, 0.0f)) * FezMath.ScreenSpaceMask(this.CameraManager.Viewpoint);
      vector.X += vector.Z;
      Vector3 vector3_1 = FezMath.Abs(vector);
      if (this.InputManager.GrabThrow == FezButtonState.Pressed && ((double) vector3_1.X < 2.0 && (double) vector3_1.Y < 1.5) && (this.PlayerManager.CarriedInstance == null && this.PlayerManager.Grounded) && this.PlayerManager.Action != ActionType.ReadingSign)
      {
        if (this.wutex1 != null || this.wutex2 != null)
        {
          if (this.stackedHits < 10)
            ++this.stackedHits;
        }
        else
        {
          this.PlayerManager.Action = ActionType.TurnToBell;
          this.ScheduleTurnTo();
        }
      }
      if (this.wutex1 == null && this.wutex2 == null && this.stackedHits > 0)
      {
        this.ScheduleTurnTo();
        --this.stackedHits;
      }
      this.AngularVelocity *= MathHelper.Clamp((float) (0.995000004768372 - this.SinceHit.TotalSeconds * (1.0 / 400.0)), 0.0f, 1f);
      this.Angle += this.AngularVelocity * 0.1f;
      this.AngularVelocity += -this.Angle * 0.01f;
      Vector3 scale;
      Quaternion rotation;
      Vector3 translation;
      (Matrix.CreateTranslation(0.0f, -3.5f, 0.0f) * Matrix.CreateFromYawPitchRoll(RandomHelper.Centered(FezMath.Saturate((3.0 - this.SinceHit.TotalSeconds) / 3.0) * 0.0125000001862645), this.Angle.X, this.Angle.Y) * Matrix.CreateTranslation(this.OriginalPosition.X, this.OriginalPosition.Y + 3.5f, this.OriginalPosition.Z)).Decompose(out scale, out rotation, out translation);
      this.BellAo.Position = translation;
      this.BellAo.Rotation = rotation;
      double max = FezMath.Saturate((1.5 - this.SinceHit.TotalSeconds) / 1.5) * 0.0750000029802322;
      IGameCameraManager cameraManager = this.CameraManager;
      Vector3 vector3_2 = cameraManager.InterpolatedCenter + new Vector3(RandomHelper.Between(-max, max), RandomHelper.Between(-max, max), RandomHelper.Between(-max, max));
      cameraManager.InterpolatedCenter = vector3_2;
    }

    private void ScheduleTurnTo()
    {
      this.wutex2 = Waiters.Wait(0.4, (Func<float, bool>) (_ => this.PlayerManager.Action != ActionType.TurnToBell), new Action(this.ScheduleHit));
      this.wutex2.AutoPause = true;
    }

    private void ScheduleHit()
    {
      this.wutex2 = (IWaiter) null;
      this.wutex1 = Waiters.Wait(0.25, (Action) (() =>
      {
        Waiters.Wait(0.25, (Action) (() => this.wutex1 = (IWaiter) null));
        this.PlayerManager.Action = ActionType.HitBell;
        this.PlayerManager.Animation.Timing.Restart();
        this.SinceHit = TimeSpan.Zero;
        SoundEffectExtensions.EmitAt(this.sBellHit[(int) (this.CameraManager.Viewpoint - 1)], this.BellAo.Position);
        this.SoundManager.FadeVolume(0.25f, 1f, 2f);
        this.AngularVelocity += new Vector2(-FezMath.Dot(FezMath.ForwardVector(this.CameraManager.Viewpoint), Vector3.UnitZ), FezMath.Dot(FezMath.ForwardVector(this.CameraManager.Viewpoint), Vector3.UnitX)) * 0.075f;
        if (this.Solved)
          return;
        if (this.LastHit != Viewpoint.None && this.LastHit != this.CameraManager.Viewpoint)
          this.Hits[this.CameraManager.Viewpoint] = 0;
        this.LastHit = this.CameraManager.Viewpoint;
        Dictionary<Viewpoint, int> local_1;
        Viewpoint local_2;
        (local_1 = this.Hits)[local_2 = this.CameraManager.Viewpoint] = local_1[local_2] + 1;
        if (!Enumerable.All<KeyValuePair<Viewpoint, int>>((IEnumerable<KeyValuePair<Viewpoint, int>>) this.Hits, (Func<KeyValuePair<Viewpoint, int>, bool>) (kvp => kvp.Value == this.ExpectedHits[kvp.Key])))
          return;
        this.Solved = true;
        this.GameState.SaveData.ThisLevel.InactiveArtObjects.Add(this.BellAo.Id);
        this.LevelService.ResolvePuzzle();
        ServiceHelper.AddComponent((IGameComponent) new GlitchyDespawner(this.Game, this.BellAo, this.OriginalPosition));
      }));
      this.wutex1.AutoPause = true;
    }
  }
}
