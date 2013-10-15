// Type: FezGame.Services.Scripting.VolumeService
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using Common;
using FezEngine;
using FezEngine.Components;
using FezEngine.Components.Scripting;
using FezEngine.Services;
using FezEngine.Services.Scripting;
using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Components;
using FezGame.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezGame.Services.Scripting
{
  public class VolumeService : IVolumeService, IScriptingBase
  {
    public bool RegisterNeeded { get; set; }

    [ServiceDependency]
    public IThreadPool ThreadPool { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public IDotManager Dot { private get; set; }

    [ServiceDependency]
    public IGameCameraManager CameraManager { private get; set; }

    [ServiceDependency]
    public IPlayerManager PlayerManager { private get; set; }

    [ServiceDependency]
    public ILevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { private get; set; }

    [ServiceDependency]
    public ISoundManager SoundManager { private get; set; }

    public event Action<int> Enter = new Action<int>(Util.NullAction<int>);

    public event Action<int> Exit = new Action<int>(Util.NullAction<int>);

    public event Action<int> GoLower;

    public event Action<int> GoHigher;

    public event Action<int> CodeAccepted;

    public void OnEnter(int id)
    {
      this.Enter(id);
    }

    public void OnExit(int id)
    {
      this.Exit(id);
    }

    public void OnGoLower(int id)
    {
      if (this.GoLower == null)
        return;
      this.GoLower(id);
    }

    public void OnGoHigher(int id)
    {
      if (this.GoHigher == null)
        return;
      this.GoHigher(id);
    }

    public void OnCodeAccepted(int id)
    {
      if (this.CodeAccepted != null)
        this.CodeAccepted(id);
      this.SetEnabled(id, false, true);
    }

    public bool get_GomezInside(int id)
    {
      return Enumerable.Any<Volume>((IEnumerable<Volume>) this.PlayerManager.CurrentVolumes, (Func<Volume, bool>) (x => x.Id == id));
    }

    public bool get_IsEnabled(int id)
    {
      return this.LevelManager.Volumes[id].Enabled;
    }

    public LongRunningAction MoveDotWithCamera(int id)
    {
      Volume volume = this.LevelManager.Volumes[id];
      Vector3 target = (volume.From + volume.To) / 2f;
      this.PlayerManager.CanControl = false;
      this.Dot.PreventPoI = true;
      this.Dot.MoveWithCamera(target, false);
      return new LongRunningAction((Func<float, float, bool>) ((_, __) => this.Dot.Behaviour == DotHost.BehaviourType.WaitAtTarget), (Action) (() => this.PlayerManager.CanControl = true));
    }

    public LongRunningAction FocusCamera(int id, int pixelsPerTrixel, bool immediate)
    {
      string levelName = this.LevelManager.Name;
      Volume volume = this.LevelManager.Volumes[id];
      BoundingBox boundingBox = volume.BoundingBox;
      bool changedViewpoint = volume.Orientations.Count == 1;
      Viewpoint oldViewpoint = this.CameraManager.Viewpoint;
      float oldRadius = this.CameraManager.Radius;
      this.CameraManager.Constrained = true;
      this.CameraManager.Center = (boundingBox.Min + boundingBox.Max) / 2f;
      if (pixelsPerTrixel > 0)
        this.CameraManager.PixelsPerTrixel = (float) pixelsPerTrixel;
      if (changedViewpoint)
        this.CameraManager.ChangeViewpoint(FezMath.AsViewpoint(Enumerable.First<FaceOrientation>((IEnumerable<FaceOrientation>) volume.Orientations)));
      this.CameraManager.ConstrainedCenter = this.CameraManager.Center;
      if (!immediate)
        return new LongRunningAction((Action) (() =>
        {
          if (this.LevelManager.Name != levelName)
            return;
          this.CameraManager.Constrained = false;
          if (pixelsPerTrixel > 0)
            this.CameraManager.Radius = oldRadius;
          if (!changedViewpoint)
            return;
          this.CameraManager.ChangeViewpoint(oldViewpoint);
        }));
      this.CameraManager.SnapInterpolation();
      return (LongRunningAction) null;
    }

    public void SetEnabled(int id, bool enabled, bool permanent)
    {
      this.LevelManager.Volumes[id].Enabled = enabled;
      if (!permanent)
        return;
      this.GameState.SaveData.ThisLevel.InactiveVolumes.Add(id);
      this.GameState.Save();
    }

    public void SlowFocusOn(int id, float duration, float trixPerPix)
    {
      Volume volume = this.LevelManager.Volumes[id];
      Vector3 center = (volume.From + volume.To) / 2f;
      this.CameraManager.Constrained = true;
      Vector3 c = this.CameraManager.Center;
      float currentTpP = this.CameraManager.PixelsPerTrixel;
      Waiters.Interpolate((double) duration, (Action<float>) (s =>
      {
        float local_0 = Easing.EaseInOut((double) s, EasingType.Sine);
        this.CameraManager.PixelsPerTrixel = MathHelper.Lerp(currentTpP, trixPerPix, local_0);
        this.CameraManager.Center = Vector3.Lerp(c, center - new Vector3(0.0f, 1f, 0.0f), local_0);
      })).AutoPause = true;
    }

    public LongRunningAction LoadHexahedronAt(int id, string toLevel)
    {
      Volume volume = this.LevelManager.Volumes[id];
      Vector3 center = (volume.From + volume.To) / 2f;
      Worker<NowLoadingHexahedron> worker = this.ThreadPool.Take<NowLoadingHexahedron>(new Action<NowLoadingHexahedron>(VolumeService.LoadHex));
      NowLoadingHexahedron context = new NowLoadingHexahedron(ServiceHelper.Game, center, toLevel);
      worker.Start(context);
      worker.Finished += (Action) (() => this.ThreadPool.Return<NowLoadingHexahedron>(worker));
      bool disposed = false;
      context.Disposed += (EventHandler<EventArgs>) ((_, __) => disposed = true);
      return new LongRunningAction((Func<float, float, bool>) ((_, __) => disposed));
    }

    private static void LoadHex(NowLoadingHexahedron nlh)
    {
      ServiceHelper.AddComponent((IGameComponent) nlh);
    }

    public LongRunningAction PlaySoundAt(int id, string soundName, bool loop, float initialDelay, float perLoopDelay, bool directional, float pitchVariation)
    {
      SoundEffect sfx = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/" + soundName);
      float duration = (float) sfx.Duration.TotalSeconds;
      Volume volume = this.LevelManager.Volumes[id];
      Vector3 center = (volume.From + volume.To) / 2f;
      Vector3 direction = Vector3.One;
      if (directional)
        direction = Enumerable.Aggregate<Vector3>(Enumerable.Select<FaceOrientation, Vector3>((IEnumerable<FaceOrientation>) volume.Orientations, (Func<FaceOrientation, Vector3>) (x => FezMath.GetMask(FezMath.AsAxis(x)))), (Func<Vector3, Vector3, Vector3>) ((a, b) => a + b));
      if (!loop && (double) initialDelay <= 0.0)
      {
        SoundEffectExtensions.EmitAt(sfx, center, RandomHelper.Centered((double) pitchVariation)).AxisMask = direction;
        return (LongRunningAction) null;
      }
      else
      {
        float toWait = initialDelay;
        bool perfectLoop = loop && (double) perLoopDelay <= 0.0 && (double) pitchVariation <= 0.0;
        if (perfectLoop)
          return new LongRunningAction((Func<float, float, bool>) ((elapsed, total) =>
          {
            toWait -= elapsed;
            if ((double) toWait > 0.0)
              return false;
            SoundEffectExtensions.EmitAt(sfx, center, perfectLoop, RandomHelper.Centered((double) pitchVariation)).AxisMask = direction;
            return true;
          }));
        else
          return new LongRunningAction((Func<float, float, bool>) ((elapsed, total) =>
          {
            toWait -= elapsed;
            if ((double) toWait <= 0.0)
            {
              SoundEffectExtensions.EmitAt(sfx, center, RandomHelper.Centered((double) pitchVariation)).AxisMask = direction;
              if (!loop)
                return true;
              toWait += perLoopDelay + duration;
            }
            return false;
          }));
      }
    }

    public LongRunningAction FocusWithPan(int id, int pixelsPerTrixel, float verticalPan, float horizontalPan)
    {
      string levelName = this.LevelManager.Name;
      Volume volume = this.LevelManager.Volumes[id];
      BoundingBox boundingBox = volume.BoundingBox;
      bool changedViewpoint = volume.Orientations.Count == 1;
      Viewpoint oldViewpoint = this.CameraManager.Viewpoint;
      float oldRadius = this.CameraManager.Radius;
      this.CameraManager.Constrained = true;
      this.CameraManager.Center = (boundingBox.Min + boundingBox.Max) / 2f;
      this.CameraManager.PanningConstraints = new Vector2?(new Vector2(horizontalPan, verticalPan));
      if (pixelsPerTrixel > 0)
        this.CameraManager.PixelsPerTrixel = (float) pixelsPerTrixel;
      if (changedViewpoint)
        this.CameraManager.ChangeViewpoint(FezMath.AsViewpoint(Enumerable.First<FaceOrientation>((IEnumerable<FaceOrientation>) volume.Orientations)));
      this.CameraManager.ConstrainedCenter = this.CameraManager.Center;
      return new LongRunningAction((Action) (() =>
      {
        if (this.LevelManager.Name != levelName || levelName == "CRYPT" && this.LevelManager.Name == "CRYPT")
          return;
        this.CameraManager.Constrained = false;
        this.CameraManager.PanningConstraints = new Vector2?();
        if (pixelsPerTrixel > 0)
          this.CameraManager.Radius = oldRadius;
        if (!changedViewpoint)
          return;
        this.CameraManager.ChangeViewpoint(oldViewpoint);
      }));
    }

    public void SpawnTrileAt(int id, string actorTypeName)
    {
      BoundingBox boundingBox = this.LevelManager.Volumes[id].BoundingBox;
      Vector3 position1 = (boundingBox.Min + boundingBox.Max) / 2f;
      Trile trile = Enumerable.FirstOrDefault<Trile>(this.LevelManager.ActorTriles((ActorType) Enum.Parse(typeof (ActorType), actorTypeName, true)));
      if (trile == null)
        return;
      Vector3 vector3 = position1 - Vector3.One / 2f;
      NearestTriles nearestTriles = this.LevelManager.NearestTrile(position1);
      TrileInstance trileInstance = nearestTriles.Surface ?? nearestTriles.Deep;
      if (trileInstance != null)
        vector3 = FezMath.ScreenSpaceMask(this.CameraManager.Viewpoint) * vector3 + trileInstance.Center * FezMath.DepthMask(this.CameraManager.Viewpoint) - FezMath.ForwardVector(this.CameraManager.Viewpoint) * 2f;
      Vector3 position2 = Vector3.Clamp(vector3, Vector3.Zero, this.LevelManager.Size - Vector3.One);
      ServiceHelper.AddComponent((IGameComponent) new GlitchyRespawner(ServiceHelper.Game, new TrileInstance(position2, trile.Id)
      {
        OriginalEmplacement = new TrileEmplacement(position2)
      }));
    }

    public void ResetEvents()
    {
      this.Enter = new Action<int>(Util.NullAction<int>);
      this.Exit = new Action<int>(Util.NullAction<int>);
      this.GoHigher = this.GoLower = (Action<int>) null;
      this.CodeAccepted = (Action<int>) null;
    }
  }
}
