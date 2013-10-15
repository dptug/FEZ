// Type: FezEngine.Readers.DotDialogueLineReader
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Structure;
using Microsoft.Xna.Framework.Content;

namespace FezEngine.Readers
{
  public class DotDialogueLineReader : ContentTypeReader<DotDialogueLine>
  {
    protected override DotDialogueLine Read(ContentReader input, DotDialogueLine existingInstance)
    {
      if (existingInstance == null)
        existingInstance = new DotDialogueLine();
      existingInstance.ResourceText = input.ReadObject<string>();
      existingInstance.Grouped = input.ReadBoolean();
      return existingInstance;
    }
  }
}
