// Type: FezEngine.Services.Scripting.IVolumeService
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
  [Entity(Model = typeof (Volume))]
  public interface IVolumeService : IScriptingBase
  {
    bool RegisterNeeded { get; set; }

    [Description("When the player enters this volume")]
    [EndTrigger("Exit")]
    event Action<int> Enter;

    [Description("When the player exits this volume")]
    event Action<int> Exit;

    [Description("When the player goes higher than the volume/height-marker")]
    event Action<int> GoHigher;

    [Description("When the player goes lower than the volume/height-marker")]
    event Action<int> GoLower;

    [Description("If the input code was accepted")]
    event Action<int> CodeAccepted;

    void OnEnter(int id);

    void OnExit(int id);

    void OnGoHigher(int id);

    void OnGoLower(int id);

    void OnCodeAccepted(int id);

    [Description("Tests if Gomez is inside a certain volume")]
    bool get_GomezInside(int id);

    [Description("Tests if volume is enabled")]
    bool get_IsEnabled(int id);

    [Description("Center the camera view on this volume; set a value <= 0 to pixelsPerTrixels if you don't want to alter it. Immediate doesn't wait for an end-trigger")]
    LongRunningAction FocusCamera(int id, int pixelsPerTrixel, bool immediate);

    [Description("Disables or enables this volume")]
    void SetEnabled(int id, bool enabled, bool permanent);

    [Description("Slowly focuses camera on a volume")]
    void SlowFocusOn(int id, float duration, float trixPerPix);

    [Description("Loads the hex in the middle of a specified volume, and warp to the specified level afterwards")]
    LongRunningAction LoadHexahedronAt(int id, string toLevel);

    [Description("Plays a sound on the specified volume, looped or not, directional or not (follows volume's enabled directions)")]
    LongRunningAction PlaySoundAt(int id, string soundName, bool loop, float initialDelay, float perLoopDelay, bool directional, float pitchVariation);

    [Description("Moves Dot and the camera to the specified volume's center")]
    LongRunningAction MoveDotWithCamera(int id);

    [Description("Focuses the camera with panning support")]
    LongRunningAction FocusWithPan(int id, int pixelsPerTrixel, float verticalPan, float horizontalPan);

    [Description("Spawns a treasure trile at this volume's location")]
    void SpawnTrileAt(int id, string actorTypeName);
  }
}
