// Type: Microsoft.Xna.Framework.Media.SongCollection
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Media
{
  public class SongCollection : ICollection<Song>, IEnumerable<Song>, IEnumerable, IDisposable
  {
    private bool isReadOnly = false;
    private List<Song> innerlist = new List<Song>();

    public int Count
    {
      get
      {
        return this.innerlist.Count;
      }
    }

    public bool IsReadOnly
    {
      get
      {
        return this.isReadOnly;
      }
    }

    public Song this[int index]
    {
      get
      {
        return this.innerlist[index];
      }
    }

    public void Dispose()
    {
    }

    public IEnumerator<Song> GetEnumerator()
    {
      return (IEnumerator<Song>) this.innerlist.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.innerlist.GetEnumerator();
    }

    public void Add(Song item)
    {
      if (item == (Song) null)
        throw new ArgumentNullException();
      if (this.innerlist.Count == 0)
      {
        this.innerlist.Add(item);
      }
      else
      {
        for (int index = 0; index < this.innerlist.Count; ++index)
        {
          if (item.TrackNumber < this.innerlist[index].TrackNumber)
          {
            this.innerlist.Insert(index, item);
            return;
          }
        }
        this.innerlist.Add(item);
      }
    }

    public void Clear()
    {
      this.innerlist.Clear();
    }

    public SongCollection Clone()
    {
      SongCollection songCollection = new SongCollection();
      foreach (Song song in this.innerlist)
        songCollection.Add(song);
      return songCollection;
    }

    public bool Contains(Song item)
    {
      return this.innerlist.Contains(item);
    }

    public void CopyTo(Song[] array, int arrayIndex)
    {
      this.innerlist.CopyTo(array, arrayIndex);
    }

    public int IndexOf(Song item)
    {
      return this.innerlist.IndexOf(item);
    }

    public bool Remove(Song item)
    {
      return this.innerlist.Remove(item);
    }
  }
}
