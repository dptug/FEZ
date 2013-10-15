// Type: FezEngine.Structure.ISpatialStructure`1
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using System.Collections.Generic;

namespace FezEngine.Structure
{
  internal interface ISpatialStructure<T>
  {
    bool Empty { get; }

    IEnumerable<T> Cells { get; }

    void Clear();

    void Free(IEnumerable<T> cells);

    void Free(T cell);

    void Fill(IEnumerable<T> cells);

    void Fill(T cell);

    bool IsFilled(T cell);
  }
}
