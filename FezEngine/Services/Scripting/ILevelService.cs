// Type: FezEngine.Services.Scripting.ILevelService
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using FezEngine.Components.Scripting;
using FezEngine.Structure.Scripting;
using System;

namespace FezEngine.Services.Scripting
{
  [Entity(Static = true)]
  public interface ILevelService : IScriptingBase
  {
    bool FirstVisit { get; }

    [Description("When the level starts")]
    event Action Start;

    void OnStart();

    [Description("Changes the level MARIO-STYLE")]
    LongRunningAction AllowPipeChangeLevel(string levelName);

    [Description("Changes the level; if 'asDoor' is true, the level change occurs if gomez enters the door, else it's done immediately")]
    LongRunningAction ChangeLevel(string levelName, bool asDoor, bool spin, bool trialEnding);

    [Description("Returns to the last accessed level; if 'asDoor' is true, the level change occurs if gomez enters the door, else it's done immediately")]
    LongRunningAction ReturnToLastLevel(bool asDoor, bool spin);

    [Description("Changes the faraway level to a specific volume in the destination level; if 'asDoor' is true, the level change occurs if gomez enters the door, else it's done immediately")]
    LongRunningAction ChangeToFarAwayLevel(string levelName, int toVolume, bool trialEnding);

    [Description("Changes the level to a specific volume in the destination level; if 'asDoor' is true, the level change occurs if gomez enters the door, else it's done immediately")]
    LongRunningAction ChangeLevelToVolume(string levelName, int toVolume, bool asDoor, bool spin, bool trialEnding);

    [Description("Produces an epic explosion and changes level when it's done.")]
    LongRunningAction ExploChangeLevel(string levelName);

    [Description("Smoothly changes to a new water height")]
    LongRunningAction SetWaterHeight(float height);

    [Description("Makes water rise to a certain threshold")]
    LongRunningAction RaiseWater(float unitsPerSecond, float stopAtHeight);

    [Description("Makes water stop raising immediately")]
    void StopWater();

    [Description("Marks a puzzle as solved, and plays the chime")]
    void ResolvePuzzle();

    [Description("Silently resolves a puzzle")]
    void ResolvePuzzleSilent();

    void ResolvePuzzleSoundOnly();
  }
}
