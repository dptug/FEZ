// Type: FezEngine.Structure.Input.CodeInputComparer
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using System.Collections.Generic;

namespace FezEngine.Structure.Input
{
  public class CodeInputComparer : IEqualityComparer<CodeInput>
  {
    public static readonly CodeInputComparer Default = new CodeInputComparer();

    static CodeInputComparer()
    {
    }

    public bool Equals(CodeInput x, CodeInput y)
    {
      return x == y;
    }

    public int GetHashCode(CodeInput obj)
    {
      return (int) obj;
    }
  }
}
