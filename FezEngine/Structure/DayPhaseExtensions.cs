// Type: FezEngine.Structure.DayPhaseExtensions
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using System;

namespace FezEngine.Structure
{
  public static class DayPhaseExtensions
  {
    public static float StartTime(this DayPhase phase)
    {
      switch (phase)
      {
        case DayPhase.Night:
          return 0.8333333f;
        case DayPhase.Dawn:
          return 0.08333334f;
        case DayPhase.Day:
          return 0.2083333f;
        case DayPhase.Dusk:
          return 0.75f;
        default:
          throw new InvalidOperationException();
      }
    }

    public static float EndTime(this DayPhase phase)
    {
      switch (phase)
      {
        case DayPhase.Night:
          return 0.1666667f;
        case DayPhase.Dawn:
          return 0.25f;
        case DayPhase.Day:
          return 0.8333333f;
        case DayPhase.Dusk:
          return 0.9166667f;
        default:
          throw new InvalidOperationException();
      }
    }

    public static float MusicStartTime(this DayPhase phase)
    {
      switch (phase)
      {
        case DayPhase.Night:
          return 0.875f;
        case DayPhase.Dawn:
          return 0.08333334f;
        case DayPhase.Day:
          return 0.2083333f;
        case DayPhase.Dusk:
          return 0.7916667f;
        default:
          throw new InvalidOperationException();
      }
    }

    public static float MusicEndTime(this DayPhase phase)
    {
      switch (phase)
      {
        case DayPhase.Night:
          return 0.08333334f;
        case DayPhase.Dawn:
          return 0.2083333f;
        case DayPhase.Day:
          return 0.7916667f;
        case DayPhase.Dusk:
          return 0.875f;
        default:
          throw new InvalidOperationException();
      }
    }

    public static float Duration(this DayPhase phase)
    {
      float num1 = DayPhaseExtensions.EndTime(phase);
      float num2 = DayPhaseExtensions.StartTime(phase);
      if ((double) num1 < (double) num2)
        ++num1;
      return num1 - num2;
    }
  }
}
