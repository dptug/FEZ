// Type: FezEngine.Structure.Scripting.ScriptAction
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using ContentSerialization;
using ContentSerialization.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace FezEngine.Structure.Scripting
{
  public class ScriptAction : ScriptPart, IDeserializationCallback
  {
    public string Operation { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public bool Killswitch { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public bool Blocking { get; set; }

    [Serialization(Optional = true)]
    public string[] Arguments { get; set; }

    public object[] ProcessedArguments { get; private set; }

    public DynamicMethodDelegate Invoke { get; private set; }

    public override string ToString()
    {
      if (this.Arguments == null)
        return (this.Object == null ? "(none)" : this.Object.ToString()) + "." + (this.Operation ?? "(none)") + "()";
      return (this.Object == null ? "(none)" : this.Object.ToString()) + "." + (this.Operation ?? "(none)") + "(" + Util.DeepToString<string>((IEnumerable<string>) this.Arguments, true) + ")";
    }

    public void OnDeserialization()
    {
      this.Process();
    }

    public void Process()
    {
      EntityTypeDescriptor entityTypeDescriptor = EntityTypes.Types[this.Object.Type];
      OperationDescriptor operationDescriptor = entityTypeDescriptor.Operations[this.Operation];
      int num = entityTypeDescriptor.Static ? 0 : 1;
      this.ProcessedArguments = new object[num + operationDescriptor.Parameters.Length];
      if (!entityTypeDescriptor.Static)
        this.ProcessedArguments[0] = (object) this.Object.Identifier.Value;
      for (int index = 0; index < operationDescriptor.Parameters.Length; ++index)
      {
        ParameterDescriptor parameterDescriptor = operationDescriptor.Parameters[index];
        this.ProcessedArguments[num + index] = this.Arguments.Length > index ? Convert.ChangeType((object) this.Arguments[index], parameterDescriptor.Type, (IFormatProvider) CultureInfo.InvariantCulture) : (!(parameterDescriptor.Type == typeof (string)) ? Activator.CreateInstance(parameterDescriptor.Type) : (object) string.Empty);
      }
      this.Invoke = operationDescriptor.Call;
    }

    public ScriptAction Clone()
    {
      ScriptAction scriptAction = new ScriptAction();
      scriptAction.Operation = this.Operation;
      scriptAction.Killswitch = this.Killswitch;
      scriptAction.Blocking = this.Blocking;
      scriptAction.Arguments = this.Arguments == null ? (string[]) null : Enumerable.ToArray<string>((IEnumerable<string>) this.Arguments);
      scriptAction.Object = this.Object == null ? (Entity) null : this.Object.Clone();
      return scriptAction;
    }
  }
}
