// Type: Microsoft.Xna.Framework.GamerServices.AchievementCollection
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.GamerServices
{
  public class AchievementCollection : IList<Achievement>, ICollection<Achievement>, IEnumerable<Achievement>, IEnumerable, IDisposable
  {
    private List<Achievement> innerlist;
    private bool isReadOnly;

    public int Count
    {
      get
      {
        return this.innerlist.Count;
      }
    }

    public Achievement this[int index]
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

    public AchievementCollection()
    {
      this.innerlist = new List<Achievement>();
    }

    ~AchievementCollection()
    {
      this.Dispose(false);
    }

    public void Add(Achievement item)
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

    public bool Contains(Achievement item)
    {
      return this.innerlist.Contains(item);
    }

    public void CopyTo(Achievement[] array, int arrayIndex)
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

    public int IndexOf(Achievement item)
    {
      return this.innerlist.IndexOf(item);
    }

    public void Insert(int index, Achievement item)
    {
      this.innerlist.Insert(index, item);
    }

    public bool Remove(Achievement item)
    {
      return this.innerlist.Remove(item);
    }

    public void RemoveAt(int index)
    {
      this.innerlist.RemoveAt(index);
    }

    public IEnumerator<Achievement> GetEnumerator()
    {
      return (IEnumerator<Achievement>) this.innerlist.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.innerlist.GetEnumerator();
    }
  }
}
