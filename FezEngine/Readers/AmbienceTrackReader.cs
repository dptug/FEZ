// Type: FezEngine.Readers.AmbienceTrackReader
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Structure;
using Microsoft.Xna.Framework.Content;

namespace FezEngine.Readers
{
  public class AmbienceTrackReader : ContentTypeReader<AmbienceTrack>
  {
    protected override AmbienceTrack Read(ContentReader input, AmbienceTrack existingInstance)
    {
      if (existingInstance == null)
        existingInstance = new AmbienceTrack();
      existingInstance.Name = input.ReadObject<string>();
      existingInstance.Dawn = input.ReadBoolean();
      existingInstance.Day = input.ReadBoolean();
      existingInstance.Dusk = input.ReadBoolean();
      existingInstance.Night = input.ReadBoolean();
      return existingInstance;
    }
  }
}
