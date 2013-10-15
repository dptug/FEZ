// Type: FezEngine.Services.ITimeManager
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Structure;
using Microsoft.Xna.Framework;
using System;

namespace FezEngine.Services
{
  public interface ITimeManager
  {
    DateTime CurrentTime { get; set; }

    float TimeFactor { get; set; }

    float DayFraction { get; }

    float NightContribution { get; }

    float DawnContribution { get; }

    float DuskContribution { get; }

    float CurrentAmbientFactor { get; set; }

    Color CurrentFogColor { get; set; }

    float DefaultTimeFactor { get; }

    event Action Tick;

    void OnTick();

    bool IsDayPhase(DayPhase phase);

    bool IsDayPhaseForMusic(DayPhase phase);

    float DayPhaseFraction(DayPhase phase);
  }
}
