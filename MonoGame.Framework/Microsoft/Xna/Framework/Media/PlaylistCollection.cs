// Type: Microsoft.Xna.Framework.Media.PlaylistCollection
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Media
{
  public sealed class PlaylistCollection : ICollection<Playlist>, IEnumerable<Playlist>, IEnumerable, IDisposable
  {
    private List<Playlist> innerlist = new List<Playlist>();
    private bool isReadOnly;

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

    public Playlist this[int index]
    {
      get
      {
        return this.innerlist[index];
      }
    }

    public void Dispose()
    {
    }

    public IEnumerator<Playlist> GetEnumerator()
    {
      return (IEnumerator<Playlist>) this.innerlist.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.innerlist.GetEnumerator();
    }

    public void Add(Playlist item)
    {
      if (item == null)
        throw new ArgumentNullException();
      if (this.innerlist.Count == 0)
      {
        this.innerlist.Add(item);
      }
      else
      {
        for (int index = 0; index < this.innerlist.Count; ++index)
        {
          if (item.Duration < this.innerlist[index].Duration)
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

    public PlaylistCollection Clone()
    {
      PlaylistCollection playlistCollection = new PlaylistCollection();
      foreach (Playlist playlist in this.innerlist)
        playlistCollection.Add(playlist);
      return playlistCollection;
    }

    public bool Contains(Playlist item)
    {
      return this.innerlist.Contains(item);
    }

    public void CopyTo(Playlist[] array, int arrayIndex)
    {
      this.innerlist.CopyTo(array, arrayIndex);
    }

    public int IndexOf(Playlist item)
    {
      return this.innerlist.IndexOf(item);
    }

    public bool Remove(Playlist item)
    {
      return this.innerlist.Remove(item);
    }
  }
}
