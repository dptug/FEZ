// Type: FezEngine.Services.IDefaultCameraManager
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine;
using Microsoft.Xna.Framework;
using System;

namespace FezEngine.Services
{
  public interface IDefaultCameraManager : ICameraProvider
  {
    float InterpolationSpeed { get; set; }

    Vector3 Center { get; set; }

    Vector3 InterpolatedCenter { get; set; }

    Vector3 Direction { get; set; }

    float FieldOfView { get; }

    bool ActionRunning { get; }

    bool ViewTransitionReached { get; }

    bool ProjectionTransitionNewlyReached { get; }

    float ViewTransitionStep { get; }

    bool InterpolationReached { get; }

    Viewpoint Viewpoint { get; }

    Viewpoint LastViewpoint { get; }

    FaceOrientation VisibleOrientation { get; }

    Vector3 Position { get; }

    float AspectRatio { get; }

    float DefaultViewableWidth { get; set; }

    float Radius { get; set; }

    bool ProjectionTransition { get; }

    float PixelsPerTrixel { get; set; }

    BoundingFrustum Frustum { get; }

    Quaternion Rotation { get; }

    Matrix InverseView { get; }

    float NearPlane { get; }

    float FarPlane { get; }

    Vector3 ViewOffset { get; set; }

    bool StickyCam { get; set; }

    bool DollyZoomOut { set; }

    bool Constrained { get; set; }

    bool ForceTransition { get; set; }

    bool ViewTransitionCancelled { get; }

    Vector2? PanningConstraints { get; set; }

    Vector3 ConstrainedCenter { get; set; }

    bool ForceInterpolation { get; set; }

    event Action ViewpointChanged;

    event Action PreViewpointChanged;

    void ResetViewpoints();

    void RebuildView();

    bool ChangeViewpoint(Viewpoint view);

    bool ChangeViewpoint(Viewpoint view, float speedFactor);

    void AlterTransition(Viewpoint newTo);

    void AlterTransition(Vector3 newDestinationDirection);

    void SnapInterpolation();
  }
}
