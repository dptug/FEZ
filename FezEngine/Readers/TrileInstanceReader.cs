// Type: FezEngine.Readers.TrileInstanceReader
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Structure;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace FezEngine.Readers
{
  public class TrileInstanceReader : ContentTypeReader<TrileInstance>
  {
    protected override TrileInstance Read(ContentReader input, TrileInstance existingInstance)
    {
      if (existingInstance == null)
        existingInstance = new TrileInstance();
      existingInstance.Position = input.ReadVector3();
      existingInstance.TrileId = input.ReadInt32();
      byte orientation = input.ReadByte();
      existingInstance.SetPhiLight(orientation);
      if (input.ReadBoolean())
        existingInstance.ActorSettings = input.ReadObject<InstanceActorSettings>(existingInstance.ActorSettings);
      existingInstance.OverlappedTriles = input.ReadObject<List<TrileInstance>>(existingInstance.OverlappedTriles);
      return existingInstance;
    }
  }
}
