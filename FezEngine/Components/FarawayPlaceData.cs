// Type: FezEngine.Components.FarawayPlaceData
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine;
using FezEngine.Structure;
using Microsoft.Xna.Framework;

namespace FezEngine.Components
{
  internal struct FarawayPlaceData
  {
    public Mesh WaterBodyMesh;
    public Vector3 OriginalCenter;
    public Viewpoint Viewpoint;
    public Volume Volume;
    public Vector3 DestinationOffset;
    public float? WaterLevelOffset;
    public string DestinationLevelName;
    public float DestinationWaterLevel;
    public float DestinationLevelSize;
  }
}
