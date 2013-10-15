// Type: FezEngine.Services.Scripting.IGroupService
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using FezEngine.Components.Scripting;
using FezEngine.Structure;
using FezEngine.Structure.Scripting;

namespace FezEngine.Services.Scripting
{
  [Entity(Model = typeof (TrileGroup))]
  public interface IGroupService : IScriptingBase
  {
    [Description("Starts the group's moving path (must be set 'Needs Trigger')")]
    void StartPath(int id, bool backwards);

    [Description("Runs the group's moving path, but only once (must be set 'Needs Trigger')")]
    void RunPathOnce(int id, bool backwards);

    [Description("Runs a single segment of the group's moving path (must be set 'Needs Trigger')")]
    void RunSingleSegment(int id, bool backwards);

    [Description("Stops or pauses a moving group")]
    void Stop(int id);

    [Description("Moves a group incrementally over time (units per second)")]
    LongRunningAction Move(int id, float dX, float dY, float dZ);

    [Description("Enables or disables all of a group's triles")]
    void SetEnabled(int id, bool enabled);

    [Description("Moves a moving group to the end of its path")]
    void MovePathToEnd(int i);

    [Description("Moves a moving group to the end of its path")]
    void GlitchyDespawn(int i, bool permanent);
  }
}
