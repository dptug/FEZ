// Type: FezGame.Services.IGameCameraManager
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Services;
using Microsoft.Xna.Framework;

namespace FezGame.Services
{
  public interface IGameCameraManager : IDefaultCameraManager, ICameraProvider
  {
    Viewpoint RequestedViewpoint { get; set; }

    Vector3 OriginalDirection { get; set; }

    void RecordNewCarriedInstancePhi();

    void CancelViewTransition();
  }
}
