// Type: FezGame.Components.GameLightingPostProcess
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Components;
using FezEngine.Tools;
using FezGame.Services;
using FezGame.Structure;
using Microsoft.Xna.Framework;

namespace FezGame.Components
{
  public class GameLightingPostProcess : LightingPostProcess
  {
    [ServiceDependency]
    public IPlayerManager PlayerManager { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    public GameLightingPostProcess(Game game)
      : base(game)
    {
    }

    protected override void DrawLightOccluders()
    {
      if (this.PlayerManager.Hidden || this.GameState.InFpsMode)
        return;
      this.PlayerManager.Mesh.Rotation = FezMath.IsOrthographic(this.CameraManager.Viewpoint) || this.CameraManager.LastViewpoint == Viewpoint.None ? this.CameraManager.Rotation : Quaternion.CreateFromAxisAngle(Vector3.UnitY, FezMath.ToPhi(this.CameraManager.LastViewpoint));
      if (this.PlayerManager.LookingDirection == HorizontalDirection.Left)
        this.PlayerManager.Mesh.Rotation *= FezMath.QuaternionFromPhi(3.141593f);
      this.PlayerManager.Mesh.Draw();
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
      if (this.GameState.Loading || !this.LevelManager.BackgroundPlanes.ContainsKey(-1))
        return;
      Vector3 vector3_1 = FezMath.RightVector(this.CameraManager.Viewpoint) * (float) FezMath.Sign(this.PlayerManager.LookingDirection);
      Vector3 vector3_2 = this.PlayerManager.Action == ActionType.PullUpCornerLedge ? this.PlayerManager.Position + this.PlayerManager.Size * (vector3_1 + Vector3.UnitY) * 0.5f * Easing.EaseOut((double) this.PlayerManager.Animation.Timing.NormalizedStep, EasingType.Quadratic) : (this.PlayerManager.Action == ActionType.LowerToCornerLedge ? this.PlayerManager.Position + this.PlayerManager.Size * (-vector3_1 + Vector3.UnitY) * 0.5f * (1f - Easing.EaseOut((double) this.PlayerManager.Animation.Timing.NormalizedStep, EasingType.Quadratic)) : this.PlayerManager.Position);
      if (this.GameState.InFpsMode)
        vector3_2 += this.CameraManager.InverseView.Forward;
      this.LevelManager.BackgroundPlanes[-1].Position = this.LevelManager.HaloFiltering ? vector3_2 : FezMath.Round(vector3_2 * 16f) / 16f;
    }
  }
}
