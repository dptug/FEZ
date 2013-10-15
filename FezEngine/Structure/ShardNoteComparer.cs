// Type: FezEngine.Structure.ShardNoteComparer
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using System.Collections.Generic;

namespace FezEngine.Structure
{
  public class ShardNoteComparer : IEqualityComparer<ShardNotes>
  {
    public static readonly ShardNoteComparer Default = new ShardNoteComparer();

    static ShardNoteComparer()
    {
    }

    public bool Equals(ShardNotes x, ShardNotes y)
    {
      return x == y;
    }

    public int GetHashCode(ShardNotes obj)
    {
      return (int) obj;
    }
  }
}
