// Type: FezEngine.Readers.ScriptActionReader
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Structure.Scripting;
using Microsoft.Xna.Framework.Content;

namespace FezEngine.Readers
{
  public class ScriptActionReader : ContentTypeReader<ScriptAction>
  {
    protected override ScriptAction Read(ContentReader input, ScriptAction existingInstance)
    {
      if (existingInstance == null)
        existingInstance = new ScriptAction();
      existingInstance.Object = input.ReadObject<Entity>(existingInstance.Object);
      existingInstance.Operation = input.ReadString();
      existingInstance.Arguments = input.ReadObject<string[]>(existingInstance.Arguments);
      existingInstance.Killswitch = input.ReadBoolean();
      existingInstance.Blocking = input.ReadBoolean();
      existingInstance.OnDeserialization();
      return existingInstance;
    }
  }
}
