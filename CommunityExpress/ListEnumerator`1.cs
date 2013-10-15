// Type: CommunityExpressNS.ListEnumerator`1
// Assembly: CommunityExpress, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3B3F745C-AA2A-4DDF-AA8A-F5898AF84B8D
// Assembly location: F:\Program Files (x86)\FEZ\CommunityExpress.dll

using System;
using System.Collections;
using System.Collections.Generic;

namespace CommunityExpressNS
{
  internal class ListEnumerator<T> : IEnumerator<T>, IDisposable, IEnumerator
  {
    private int _index;
    private IList<T> _list;

    public T Current
    {
      get
      {
        return this._list[this._index];
      }
    }

    object IEnumerator.Current
    {
      get
      {
        return (object) this.Current;
      }
    }

    public ListEnumerator(IList<T> list)
    {
      this._list = list;
      this._index = -1;
    }

    public bool MoveNext()
    {
      ++this._index;
      return this._index < this._list.Count;
    }

    public void Reset()
    {
      this._index = -1;
    }

    public void Dispose()
    {
    }
  }
}
