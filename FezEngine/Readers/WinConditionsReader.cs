// Type: FezEngine.Readers.WinConditionsReader
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Structure;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace FezEngine.Readers
{
  public class WinConditionsReader : ContentTypeReader<WinConditions>
  {
    protected override WinConditions Read(ContentReader input, WinConditions existingInstance)
    {
      if (existingInstance == null)
        existingInstance = new WinConditions();
      existingInstance.ChestCount = input.ReadInt32();
      existingInstance.LockedDoorCount = input.ReadInt32();
      existingInstance.UnlockedDoorCount = input.ReadInt32();
      existingInstance.ScriptIds = input.ReadObject<List<int>>(existingInstance.ScriptIds);
      existingInstance.CubeShardCount = input.ReadInt32();
      existingInstance.OtherCollectibleCount = input.ReadInt32();
      existingInstance.SplitUpCount = input.ReadInt32();
      existingInstance.SecretCount = input.ReadInt32();
      return existingInstance;
    }
  }
}
