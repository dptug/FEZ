// Type: FezEngine.Structure.Scripting.ScriptCondition
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using ContentSerialization;
using FezEngine.Services.Scripting;
using System;
using System.Globalization;

namespace FezEngine.Structure.Scripting
{
  public class ScriptCondition : ScriptPart, IDeserializationCallback
  {
    private object[] processedArguments;
    private object processedValue;
    private float processedNumber;
    private DynamicMethodDelegate GetValue;

    public string Property { get; set; }

    public ComparisonOperator Operator { get; set; }

    public string Value { get; set; }

    public ScriptCondition()
    {
      this.Operator = ComparisonOperator.None;
    }

    public override string ToString()
    {
      return (this.Object == null ? "(none)" : this.Object.ToString()) + "." + (this.Property ?? "(none)") + " " + ExpressionOperatorExtensions.ToSymbol(this.Operator) + " " + this.Value;
    }

    public void OnDeserialization()
    {
      this.Process();
    }

    public void Process()
    {
      EntityTypeDescriptor entityTypeDescriptor = EntityTypes.Types[this.Object.Type];
      PropertyDescriptor propertyDescriptor = entityTypeDescriptor.Properties[this.Property];
      this.processedValue = Convert.ChangeType((object) this.Value, propertyDescriptor.Type, (IFormatProvider) CultureInfo.InvariantCulture);
      this.GetValue = propertyDescriptor.GetValue;
      if (this.Operator != ComparisonOperator.Equal && this.Operator != ComparisonOperator.NotEqual)
        this.processedNumber = (float) this.processedValue;
      ScriptCondition scriptCondition = this;
      object[] objArray;
      if (!entityTypeDescriptor.Static)
        objArray = new object[1]
        {
          (object) this.Object.Identifier
        };
      else
        objArray = new object[0];
      scriptCondition.processedArguments = objArray;
    }

    public bool Check(IScriptingBase service)
    {
      object obj = this.GetValue((object) service, this.processedArguments);
      switch (this.Operator)
      {
        case ComparisonOperator.Equal:
          return obj.Equals(this.processedValue);
        case ComparisonOperator.NotEqual:
          return !obj.Equals(this.processedValue);
        default:
          float num = (float) obj;
          switch (this.Operator)
          {
            case ComparisonOperator.Greater:
              return (double) num > (double) this.processedNumber;
            case ComparisonOperator.GreaterEqual:
              return (double) num >= (double) this.processedNumber;
            case ComparisonOperator.Less:
              return (double) num < (double) this.processedNumber;
            case ComparisonOperator.LessEqual:
              return (double) num <= (double) this.processedNumber;
            default:
              throw new InvalidOperationException();
          }
      }
    }

    public ScriptCondition Clone()
    {
      ScriptCondition scriptCondition = new ScriptCondition();
      scriptCondition.Object = this.Object == null ? (Entity) null : this.Object.Clone();
      scriptCondition.Operator = this.Operator;
      scriptCondition.Property = this.Property;
      scriptCondition.Value = this.Value;
      return scriptCondition;
    }
  }
}
