// Type: FezEngine.Readers.MovementPathReader
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Structure;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace FezEngine.Readers
{
  public class MovementPathReader : ContentTypeReader<MovementPath>
  {
    protected override MovementPath Read(ContentReader input, MovementPath existingInstance)
    {
      if (existingInstance == null)
        existingInstance = new MovementPath();
      existingInstance.Segments = input.ReadObject<List<PathSegment>>(existingInstance.Segments);
      existingInstance.NeedsTrigger = input.ReadBoolean();
      existingInstance.EndBehavior = input.ReadObject<PathEndBehavior>();
      existingInstance.SoundName = input.ReadObject<string>();
      existingInstance.IsSpline = input.ReadBoolean();
      existingInstance.OffsetSeconds = input.ReadSingle();
      existingInstance.SaveTrigger = input.ReadBoolean();
      return existingInstance;
    }
  }
}
