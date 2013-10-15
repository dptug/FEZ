// Type: FezEngine.Structure.WinConditions
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using ContentSerialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezEngine.Structure
{
  public class WinConditions
  {
    [Serialization(Optional = true)]
    public List<int> ScriptIds = new List<int>();
    [Serialization(DefaultValueOptional = true, Optional = true)]
    public int LockedDoorCount;
    [Serialization(DefaultValueOptional = true, Optional = true)]
    public int UnlockedDoorCount;
    [Serialization(DefaultValueOptional = true, Optional = true)]
    public int ChestCount;
    [Serialization(DefaultValueOptional = true, Optional = true)]
    public int CubeShardCount;
    [Serialization(DefaultValueOptional = true, Optional = true)]
    public int OtherCollectibleCount;
    [Serialization(DefaultValueOptional = true, Optional = true)]
    public int SplitUpCount;
    [Serialization(DefaultValueOptional = true, Optional = true)]
    public int SecretCount;

    public bool Fullfills(WinConditions wonditions)
    {
      if (this.UnlockedDoorCount >= wonditions.UnlockedDoorCount && this.LockedDoorCount >= wonditions.LockedDoorCount && (this.ChestCount >= wonditions.ChestCount && this.CubeShardCount >= wonditions.CubeShardCount) && (this.OtherCollectibleCount >= wonditions.OtherCollectibleCount && this.SplitUpCount >= wonditions.SplitUpCount && this.SecretCount >= wonditions.SecretCount))
        return Enumerable.All<int>((IEnumerable<int>) wonditions.ScriptIds, (Func<int, bool>) (x => this.ScriptIds.Contains(x)));
      else
        return false;
    }

    public WinConditions Clone()
    {
      return new WinConditions()
      {
        LockedDoorCount = this.LockedDoorCount,
        UnlockedDoorCount = this.UnlockedDoorCount,
        ChestCount = this.ChestCount,
        CubeShardCount = this.CubeShardCount,
        OtherCollectibleCount = this.OtherCollectibleCount,
        SplitUpCount = this.SplitUpCount,
        ScriptIds = new List<int>((IEnumerable<int>) this.ScriptIds),
        SecretCount = this.SecretCount
      };
    }

    public void CloneInto(WinConditions w)
    {
      w.LockedDoorCount = this.LockedDoorCount;
      w.UnlockedDoorCount = this.UnlockedDoorCount;
      w.ChestCount = this.ChestCount;
      w.CubeShardCount = this.CubeShardCount;
      w.OtherCollectibleCount = this.OtherCollectibleCount;
      w.SplitUpCount = this.SplitUpCount;
      w.SecretCount = this.SecretCount;
      w.ScriptIds.Clear();
      w.ScriptIds.AddRange((IEnumerable<int>) this.ScriptIds);
    }
  }
}
