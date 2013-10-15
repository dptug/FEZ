// Type: CommunityExpressNS.Servers
// Assembly: CommunityExpress, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3B3F745C-AA2A-4DDF-AA8A-F5898AF84B8D
// Assembly location: F:\Program Files (x86)\FEZ\CommunityExpress.dll

using System;
using System.Collections;
using System.Collections.Generic;

namespace CommunityExpressNS
{
  public class Servers : ICollection<Server>, IEnumerable<Server>, IEnumerable
  {
    private List<Server> _serverList = new List<Server>();

    public int Count
    {
      get
      {
        return this._serverList.Count;
      }
    }

    public bool IsReadOnly
    {
      get
      {
        return true;
      }
    }

    public void Add(Server item)
    {
      this._serverList.Add(item);
    }

    public void Clear()
    {
      this._serverList.Clear();
    }

    public bool Contains(Server item)
    {
      throw new NotImplementedException();
    }

    public void CopyTo(Server[] array, int arrayIndex)
    {
      throw new NotImplementedException();
    }

    public bool Remove(Server item)
    {
      throw new NotSupportedException();
    }

    public IEnumerator<Server> GetEnumerator()
    {
      return (IEnumerator<Server>) new ListEnumerator<Server>((IList<Server>) this._serverList);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.GetEnumerator();
    }
  }
}
