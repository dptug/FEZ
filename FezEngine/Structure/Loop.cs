// Type: FezEngine.Structure.Loop
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using ContentSerialization.Attributes;

namespace FezEngine.Structure
{
  public class Loop
  {
    public int LoopTimesFrom = 1;
    public int LoopTimesTo = 1;
    public int Duration = 1;
    [Serialization(Optional = true)]
    public bool Day = true;
    [Serialization(Optional = true)]
    public bool Night = true;
    [Serialization(Optional = true)]
    public bool Dawn = true;
    [Serialization(Optional = true)]
    public bool Dusk = true;
    public string Name;
    public int TriggerFrom;
    public int TriggerTo;
    [Serialization(Optional = true)]
    public int Delay;
    [Serialization(Optional = true)]
    public bool OneAtATime;
    [Serialization(Optional = true)]
    public bool CutOffTail;
    [Serialization(Optional = true)]
    public bool FractionalTime;
    [Serialization(Ignore = true)]
    public bool Initialized;
    [Serialization(Ignore = true)]
    public bool OriginalDay;
    [Serialization(Ignore = true)]
    public bool OriginalDusk;
    [Serialization(Ignore = true)]
    public bool OriginalNight;
    [Serialization(Ignore = true)]
    public bool OriginalDawn;

    public Loop Clone()
    {
      return new Loop()
      {
        Name = this.Name,
        TriggerFrom = this.TriggerFrom,
        TriggerTo = this.TriggerTo,
        LoopTimesFrom = this.LoopTimesFrom,
        LoopTimesTo = this.LoopTimesTo,
        Duration = this.Duration,
        Delay = this.Delay,
        Night = this.Night,
        Day = this.Day,
        Dawn = this.Dawn,
        Dusk = this.Dusk,
        OneAtATime = this.OneAtATime,
        CutOffTail = this.CutOffTail,
        FractionalTime = this.FractionalTime
      };
    }

    public void UpdateFromCopy(Loop other)
    {
      this.Name = other.Name;
      this.TriggerFrom = other.TriggerFrom;
      this.TriggerTo = other.TriggerTo;
      this.LoopTimesFrom = other.LoopTimesFrom;
      this.LoopTimesTo = other.LoopTimesTo;
      this.Duration = other.Duration;
      this.Delay = other.Delay;
      this.Night = other.Night;
      this.Day = other.Day;
      this.Dawn = other.Dawn;
      this.Dusk = other.Dusk;
      this.OneAtATime = other.OneAtATime;
      this.CutOffTail = other.CutOffTail;
      this.FractionalTime = other.FractionalTime;
    }
  }
}
