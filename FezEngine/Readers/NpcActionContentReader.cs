// Type: FezEngine.Readers.NpcActionContentReader
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Structure;
using Microsoft.Xna.Framework.Content;

namespace FezEngine.Readers
{
  public class NpcActionContentReader : ContentTypeReader<NpcActionContent>
  {
    protected override NpcActionContent Read(ContentReader input, NpcActionContent existingInstance)
    {
      if (existingInstance == null)
        existingInstance = new NpcActionContent();
      existingInstance.AnimationName = input.ReadObject<string>();
      existingInstance.SoundName = input.ReadObject<string>();
      return existingInstance;
    }
  }
}
