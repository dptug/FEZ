// Type: Microsoft.Xna.Framework.GamerServices.FriendCollection
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.GamerServices
{
  public class FriendCollection : IList<FriendGamer>, ICollection<FriendGamer>, IEnumerable<FriendGamer>, IEnumerable, IDisposable
  {
    private List<FriendGamer> innerlist;
    private bool isReadOnly;

    public int Count
    {
      get
      {
        return this.innerlist.Count;
      }
    }

    public FriendGamer this[int index]
    {
      get
      {
        return this.innerlist[index];
      }
      set
      {
        if (value == null)
          throw new ArgumentNullException();
        if (index >= this.innerlist.Count)
          throw new IndexOutOfRangeException();
      }
    }

    public bool IsReadOnly
    {
      get
      {
        return this.isReadOnly;
      }
    }

    public FriendCollection()
    {
      this.innerlist = new List<FriendGamer>();
    }

    ~FriendCollection()
    {
      this.Dispose(false);
    }

    public void Add(FriendGamer item)
    {
      if (item == null)
        throw new ArgumentNullException();
      if (this.innerlist.Count == 0)
      {
        this.innerlist.Add(item);
      }
      else
      {
        int num = 0;
        while (num < this.innerlist.Count)
          ++num;
        this.innerlist.Add(item);
      }
    }

    public void Clear()
    {
      this.innerlist.Clear();
    }

    public bool Contains(FriendGamer item)
    {
      return this.innerlist.Contains(item);
    }

    public void CopyTo(FriendGamer[] array, int arrayIndex)
    {
      this.innerlist.CopyTo(array, arrayIndex);
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
    }

    public int IndexOf(FriendGamer item)
    {
      return this.innerlist.IndexOf(item);
    }

    public void Insert(int index, FriendGamer item)
    {
      this.innerlist.Insert(index, item);
    }

    public bool Remove(FriendGamer item)
    {
      return this.innerlist.Remove(item);
    }

    public void RemoveAt(int index)
    {
      this.innerlist.RemoveAt(index);
    }

    public IEnumerator<FriendGamer> GetEnumerator()
    {
      return (IEnumerator<FriendGamer>) this.innerlist.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.innerlist.GetEnumerator();
    }
  }
}
