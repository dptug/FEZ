// Type: FezEngine.Readers.ScriptConditionReader
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Structure.Scripting;
using Microsoft.Xna.Framework.Content;

namespace FezEngine.Readers
{
  public class ScriptConditionReader : ContentTypeReader<ScriptCondition>
  {
    protected override ScriptCondition Read(ContentReader input, ScriptCondition existingInstance)
    {
      if (existingInstance == null)
        existingInstance = new ScriptCondition();
      existingInstance.Object = input.ReadObject<Entity>(existingInstance.Object);
      existingInstance.Operator = input.ReadObject<ComparisonOperator>();
      existingInstance.Property = input.ReadString();
      existingInstance.Value = input.ReadString();
      existingInstance.OnDeserialization();
      return existingInstance;
    }
  }
}
