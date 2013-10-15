// Type: FezEngine.Structure.Scripting.ExpressionOperatorExtensions
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using System;

namespace FezEngine.Structure.Scripting
{
  public static class ExpressionOperatorExtensions
  {
    public static string ToSymbol(this ComparisonOperator op)
    {
      switch (op)
      {
        case ComparisonOperator.None:
          return "";
        case ComparisonOperator.Equal:
          return "=";
        case ComparisonOperator.Greater:
          return ">";
        case ComparisonOperator.GreaterEqual:
          return ">=";
        case ComparisonOperator.Less:
          return "<";
        case ComparisonOperator.LessEqual:
          return "<=";
        case ComparisonOperator.NotEqual:
          return "!=";
        default:
          throw new InvalidOperationException();
      }
    }
  }
}
