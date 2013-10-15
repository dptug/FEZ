// Type: FezEngine.Readers.ScriptTriggerReader
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Structure.Scripting;
using Microsoft.Xna.Framework.Content;

namespace FezEngine.Readers
{
  public class ScriptTriggerReader : ContentTypeReader<ScriptTrigger>
  {
    protected override ScriptTrigger Read(ContentReader input, ScriptTrigger existingInstance)
    {
      if (existingInstance == null)
        existingInstance = new ScriptTrigger();
      existingInstance.Object = input.ReadObject<Entity>(existingInstance.Object);
      existingInstance.Event = input.ReadString();
      return existingInstance;
    }
  }
}
