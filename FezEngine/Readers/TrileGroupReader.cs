// Type: FezEngine.Readers.TrileGroupReader
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Structure;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace FezEngine.Readers
{
  public class TrileGroupReader : ContentTypeReader<TrileGroup>
  {
    protected override TrileGroup Read(ContentReader input, TrileGroup existingInstance)
    {
      if (existingInstance == null)
        existingInstance = new TrileGroup();
      existingInstance.Triles = input.ReadObject<List<TrileInstance>>(existingInstance.Triles);
      existingInstance.Path = input.ReadObject<MovementPath>(existingInstance.Path);
      existingInstance.Heavy = input.ReadBoolean();
      existingInstance.ActorType = input.ReadObject<ActorType>();
      existingInstance.GeyserOffset = input.ReadSingle();
      existingInstance.GeyserPauseFor = input.ReadSingle();
      existingInstance.GeyserLiftFor = input.ReadSingle();
      existingInstance.GeyserApexHeight = input.ReadSingle();
      existingInstance.SpinCenter = input.ReadVector3();
      existingInstance.SpinClockwise = input.ReadBoolean();
      existingInstance.SpinFrequency = input.ReadSingle();
      existingInstance.SpinNeedsTriggering = input.ReadBoolean();
      existingInstance.Spin180Degrees = input.ReadBoolean();
      existingInstance.FallOnRotate = input.ReadBoolean();
      existingInstance.SpinOffset = input.ReadSingle();
      existingInstance.AssociatedSound = input.ReadObject<string>();
      return existingInstance;
    }
  }
}
