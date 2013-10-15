// Type: FezEngine.Readers.NpcMetadataReader
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Structure;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace FezEngine.Readers
{
  public class NpcMetadataReader : ContentTypeReader<NpcMetadata>
  {
    protected override NpcMetadata Read(ContentReader input, NpcMetadata existingInstance)
    {
      if (existingInstance == null)
        existingInstance = new NpcMetadata();
      existingInstance.WalkSpeed = input.ReadSingle();
      existingInstance.AvoidsGomez = input.ReadBoolean();
      existingInstance.SoundPath = input.ReadObject<string>();
      existingInstance.SoundActions = input.ReadObject<List<NpcAction>>(existingInstance.SoundActions);
      return existingInstance;
    }
  }
}
