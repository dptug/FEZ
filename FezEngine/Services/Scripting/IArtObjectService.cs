// Type: FezEngine.Services.Scripting.IArtObjectService
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using FezEngine.Components.Scripting;
using FezEngine.Structure;
using FezEngine.Structure.Scripting;
using System;

namespace FezEngine.Services.Scripting
{
  [Entity(Model = typeof (ArtObjectInstance))]
  public interface IArtObjectService : IScriptingBase
  {
    event Action<int> TreasureOpened;

    void OnTreasureOpened(int id);

    [Description("Replaces the rotation angles (in degrees)")]
    void SetRotation(int id, float x, float y, float z);

    [Description("Rotates over time (in rotations per second)")]
    LongRunningAction Rotate(int id, float dX, float dY, float dZ);

    [Description("Rotates incrementally over time (in time before doubling)")]
    LongRunningAction RotateIncrementally(int id, float initPitch, float initYaw, float initRoll, float secondsUntilDouble);

    [Description("Tilts the art object on its bottom vertex")]
    LongRunningAction TiltOnVertex(int id, float durationSeconds);

    [Description("Moves incrementally over time (in units per second)")]
    LongRunningAction Move(int id, float dX, float dY, float dZ, float easeInFor, float easeOutAfter, float easeOutFor);

    [Description("Makes the object hover vertically")]
    LongRunningAction HoverFloat(int id, float height, float cyclesPerSecond);

    [Description("Does the whole hex-room sequence")]
    LongRunningAction StartEldersSequence(int id);

    [Description("Moves a nut&bolt to its end")]
    void MoveNutToEnd(int id);

    [Description("Moves a nut&bolt to a certain height, and gradually")]
    void MoveNutToHeight(int id, float height);

    [Description("Glitches and removes art object (and associated group if any)")]
    void GlitchOut(int id, bool permanent, string spawnedActor);

    [Description("For the hexahedron")]
    LongRunningAction BeamGomez(int id);

    [Description("For the glow blocks")]
    LongRunningAction Pulse(int id, string textureName);

    [Description("For the zuish speech")]
    LongRunningAction Say(int id, string text, bool zuish);
  }
}
