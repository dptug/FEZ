// Type: FezGame.Components.IDotManager
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FezGame.Components
{
  public interface IDotManager
  {
    bool DrawRays { get; set; }

    bool Hidden { get; set; }

    DotHost.BehaviourType Behaviour { get; set; }

    Vector3 Target { get; set; }

    string[] Dialog { get; set; }

    float TimeToWait { get; set; }

    Volume RoamingVolume { get; set; }

    float ScaleFactor { get; set; }

    float Opacity { get; set; }

    float ScalePulsing { get; set; }

    float RotationSpeed { get; set; }

    bool AlwaysShowLines { get; set; }

    float InnerScale { get; set; }

    Vector3 Position { get; }

    bool PreventPoI { get; set; }

    bool Burrowing { get; }

    bool ComingOut { get; }

    object Owner { get; set; }

    DotFaceButton FaceButton { get; set; }

    Texture2D DestinationVignette { get; set; }

    void Reset();

    void Burrow();

    void ComeOut();

    void MoveWithCamera(Vector3 target, bool burrowAfter);

    void SpiralAround(Volume volume, Vector3 center, bool hideDot);

    void ForceDrawOrder(int drawOrder);

    void RevertDrawOrder();

    void Hey();
  }
}
