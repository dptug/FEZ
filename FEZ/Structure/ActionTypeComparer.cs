// Type: FezGame.Structure.ActionTypeComparer
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using System.Collections.Generic;

namespace FezGame.Structure
{
  public class ActionTypeComparer : IEqualityComparer<ActionType>
  {
    public static readonly ActionTypeComparer Default = new ActionTypeComparer();

    static ActionTypeComparer()
    {
    }

    public bool Equals(ActionType x, ActionType y)
    {
      return x == y;
    }

    public int GetHashCode(ActionType obj)
    {
      return (int) obj;
    }
  }
}
