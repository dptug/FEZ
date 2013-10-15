// Type: FezEngine.Structure.ITrixelObject
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine;
using Microsoft.Xna.Framework;

namespace FezEngine.Structure
{
  public interface ITrixelObject
  {
    Vector3 Size { get; }

    TrixelCluster MissingTrixels { get; }

    bool TrixelExists(TrixelEmplacement trixelIdentifier);

    bool CanContain(TrixelEmplacement trixel);

    bool IsBorderTrixelFace(TrixelEmplacement id, FaceOrientation face);

    bool IsBorderTrixelFace(TrixelEmplacement traversed);
  }
}
