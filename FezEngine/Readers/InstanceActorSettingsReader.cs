// Type: FezEngine.Readers.InstanceActorSettingsReader
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Structure;
using Microsoft.Xna.Framework.Content;

namespace FezEngine.Readers
{
  public class InstanceActorSettingsReader : ContentTypeReader<InstanceActorSettings>
  {
    protected override InstanceActorSettings Read(ContentReader input, InstanceActorSettings existingInstance)
    {
      if (existingInstance == (InstanceActorSettings) null)
        existingInstance = new InstanceActorSettings();
      existingInstance.ContainedTrile = input.ReadObject<int?>();
      existingInstance.SignText = input.ReadObject<string>(existingInstance.SignText);
      existingInstance.Sequence = input.ReadObject<bool[]>(existingInstance.Sequence);
      existingInstance.SequenceSampleName = input.ReadObject<string>(existingInstance.SequenceSampleName);
      existingInstance.SequenceAlternateSampleName = input.ReadObject<string>(existingInstance.SequenceAlternateSampleName);
      existingInstance.HostVolume = input.ReadObject<int?>();
      return existingInstance;
    }
  }
}
