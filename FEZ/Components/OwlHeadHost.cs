// Type: FezGame.Components.OwlHeadHost
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezGame.Components
{
  internal class OwlHeadHost : GameComponent
  {
    private ArtObjectInstance OwlHeadAo;
    private ArtObjectInstance AttachedCandlesAo;
    private SoundEffect sRumble;
    private SoundEmitter eRumble;
    private Quaternion InterpolatedRotation;
    private Quaternion OriginalRotation;
    private Vector3 OriginalTranslation;
    private bool IsBig;
    private bool IsInverted;
    private float SinceStarted;
    private float lastAngle;

    [ServiceDependency]
    public IGameStateManager GameState { get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { get; set; }

    [ServiceDependency]
    public IGameCameraManager CameraManager { get; set; }

    [ServiceDependency]
    public IGameLevelManager LevelManager { private get; set; }

    public OwlHeadHost(Game game)
      : base(game)
    {
    }

    public override void Initialize()
    {
      base.Initialize();
      this.LevelManager.LevelChanged += new Action(this.TryInitialize);
      this.TryInitialize();
    }

    private void TryInitialize()
    {
      this.sRumble = (SoundEffect) null;
      this.eRumble = (SoundEmitter) null;
      this.OwlHeadAo = Enumerable.SingleOrDefault<ArtObjectInstance>((IEnumerable<ArtObjectInstance>) this.LevelManager.ArtObjects.Values, (Func<ArtObjectInstance, bool>) (x =>
      {
        if (!(x.ArtObjectName == "OWL_STATUE_HEADAO") && !(x.ArtObjectName == "BIG_OWL_HEADAO"))
          return x.ArtObjectName == "OWL_STATUE_DRAPES_BAO";
        else
          return true;
      }));
      this.AttachedCandlesAo = (ArtObjectInstance) null;
      this.IsBig = this.OwlHeadAo != null && this.OwlHeadAo.ArtObjectName == "BIG_OWL_HEADAO";
      this.Enabled = this.OwlHeadAo != null;
      this.SinceStarted = 0.0f;
      if (!this.Enabled)
        return;
      this.sRumble = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/MiscActors/Rumble");
      this.eRumble = SoundEffectExtensions.EmitAt(this.sRumble, this.OwlHeadAo.Position, true, true);
      this.eRumble.VolumeFactor = 0.0f;
      this.IsInverted = false;
      if (this.OwlHeadAo.ArtObjectName == "OWL_STATUE_DRAPES_BAO")
      {
        this.AttachedCandlesAo = this.LevelManager.ArtObjects[14];
        this.OriginalRotation = Quaternion.Identity;
        this.IsInverted = true;
      }
      else
        this.OriginalRotation = this.OwlHeadAo.Rotation * Quaternion.CreateFromAxisAngle(Vector3.Up, FezMath.ToPhi(this.CameraManager.Viewpoint));
      this.OriginalTranslation = this.OwlHeadAo.Position;
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Loading || this.GameState.Paused || (this.GameState.InMap || this.GameState.InFpsMode))
        return;
      this.InterpolatedRotation = Quaternion.Slerp(this.InterpolatedRotation, this.OriginalRotation * this.CameraManager.Rotation, 0.075f);
      if (this.InterpolatedRotation == this.CameraManager.Rotation)
      {
        if (this.eRumble.Dead || this.eRumble.Cue.State == SoundState.Paused)
          return;
        this.eRumble.Cue.Pause();
      }
      else
      {
        Vector3 axis;
        float angle;
        OwlHeadHost.ToAxisAngle(ref this.InterpolatedRotation, out axis, out angle);
        float num = this.lastAngle - angle;
        if (this.eRumble.Cue.State == SoundState.Paused)
          this.eRumble.Cue.Resume();
        this.eRumble.VolumeFactor = Math.Min(Easing.EaseOut((double) FezMath.Saturate(Math.Abs(num) * 10f), EasingType.Quadratic), 0.5f) * FezMath.Saturate(this.SinceStarted);
        this.SinceStarted += (float) gameTime.ElapsedGameTime.TotalSeconds;
        this.lastAngle = angle;
        if (FezMath.AlmostEqual(this.InterpolatedRotation, this.CameraManager.Rotation) || FezMath.AlmostEqual(-this.InterpolatedRotation, this.CameraManager.Rotation))
          this.InterpolatedRotation = this.CameraManager.Rotation;
        Matrix matrix;
        if (this.IsInverted)
        {
          matrix = Matrix.CreateTranslation(0.25f, 0.0f, -0.75f) * Matrix.CreateFromQuaternion(this.InterpolatedRotation) * Matrix.CreateTranslation(this.OriginalTranslation.X - 0.75f, this.OriginalTranslation.Y, this.OriginalTranslation.Z - 0.25f);
          this.AttachedCandlesAo.Rotation = this.InterpolatedRotation;
        }
        else
          matrix = Matrix.CreateTranslation((float) ((this.IsBig ? 8.0 : 4.0) / 16.0), 0.0f, (float) -(this.IsBig ? 24 : 12) / 16f) * Matrix.CreateFromQuaternion(this.InterpolatedRotation) * Matrix.CreateTranslation((float) -(this.IsBig ? 8 : 4) / 16f + this.OriginalTranslation.X, this.OriginalTranslation.Y, (float) ((this.IsBig ? 24.0 : 12.0) / 16.0) + this.OriginalTranslation.Z);
        Vector3 scale;
        Quaternion rotation;
        Vector3 translation;
        matrix.Decompose(out scale, out rotation, out translation);
        this.OwlHeadAo.Position = translation;
        this.OwlHeadAo.Rotation = rotation;
      }
    }

    private static void ToAxisAngle(ref Quaternion q, out Vector3 axis, out float angle)
    {
      angle = (float) Math.Acos((double) MathHelper.Clamp(q.W, -1f, 1f));
      float num1 = (float) Math.Sin((double) angle);
      float num2 = (float) (1.0 / ((double) num1 == 0.0 ? 1.0 : (double) num1));
      angle *= 2f;
      axis = new Vector3(-q.X * num2, -q.Y * num2, -q.Z * num2);
    }
  }
}
