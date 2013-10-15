// Type: FezEngine.Services.Scripting.ITimeService
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using FezEngine.Components.Scripting;
using FezEngine.Structure.Scripting;

namespace FezEngine.Services.Scripting
{
  [Entity(Static = true)]
  public interface ITimeService : IScriptingBase
  {
    [Description("The hour of day (0-23)")]
    int Hour { get; }

    [Description("Changes the hour of day (0-23), gradually or immediately")]
    LongRunningAction SetHour(int hour, bool immediate);

    [Description("Sets the speed of time passage (0 = paused)")]
    void SetTimeFactor(int factor);

    [Description("Increments the time factor (specifying how much time before it doubles up)")]
    LongRunningAction IncrementTimeFactor(float secondsUntilDouble);
  }
}
