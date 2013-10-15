// Type: FezEngine.Readers.LoopReader
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Structure;
using Microsoft.Xna.Framework.Content;

namespace FezEngine.Readers
{
  public class LoopReader : ContentTypeReader<Loop>
  {
    protected override Loop Read(ContentReader input, Loop existingInstance)
    {
      if (existingInstance == null)
        existingInstance = new Loop();
      existingInstance.Duration = input.ReadInt32();
      existingInstance.LoopTimesFrom = input.ReadInt32();
      existingInstance.LoopTimesTo = input.ReadInt32();
      existingInstance.Name = input.ReadString();
      existingInstance.TriggerFrom = input.ReadInt32();
      existingInstance.TriggerTo = input.ReadInt32();
      existingInstance.Delay = input.ReadInt32();
      existingInstance.Night = input.ReadBoolean();
      existingInstance.Day = input.ReadBoolean();
      existingInstance.Dusk = input.ReadBoolean();
      existingInstance.Dawn = input.ReadBoolean();
      existingInstance.FractionalTime = input.ReadBoolean();
      existingInstance.OneAtATime = input.ReadBoolean();
      existingInstance.CutOffTail = input.ReadBoolean();
      return existingInstance;
    }
  }
}
