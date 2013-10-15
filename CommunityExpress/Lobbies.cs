// Type: CommunityExpressNS.Lobbies
// Assembly: CommunityExpress, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3B3F745C-AA2A-4DDF-AA8A-F5898AF84B8D
// Assembly location: F:\Program Files (x86)\FEZ\CommunityExpress.dll

using System;
using System.Collections;
using System.Collections.Generic;

namespace CommunityExpressNS
{
  public class Lobbies : ICollection<Lobby>, IEnumerable<Lobby>, IEnumerable
  {
    private List<Lobby> _lobbyList = new List<Lobby>();

    public int Count
    {
      get
      {
        return this._lobbyList.Count;
      }
    }

    public bool IsReadOnly
    {
      get
      {
        return true;
      }
    }

    public void Add(Lobby item)
    {
      this._lobbyList.Add(item);
    }

    public void Clear()
    {
      this._lobbyList.Clear();
    }

    public bool Contains(Lobby item)
    {
      throw new NotImplementedException();
    }

    public void CopyTo(Lobby[] array, int arrayIndex)
    {
      throw new NotImplementedException();
    }

    public bool Remove(Lobby item)
    {
      throw new NotSupportedException();
    }

    public IEnumerator<Lobby> GetEnumerator()
    {
      return (IEnumerator<Lobby>) new ListEnumerator<Lobby>((IList<Lobby>) this._lobbyList);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.GetEnumerator();
    }
  }
}
