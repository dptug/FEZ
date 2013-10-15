// Type: Microsoft.Xna.Framework.Media.MediaQueue
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Media
{
  public sealed class MediaQueue
  {
    private List<Song> songs = new List<Song>();
    private Random random = new Random();
    private int _activeSongIndex;

    public Song ActiveSong
    {
      get
      {
        if (this.songs.Count == 0)
          return (Song) null;
        else
          return this.songs[this._activeSongIndex];
      }
    }

    public int ActiveSongIndex
    {
      get
      {
        return this._activeSongIndex;
      }
      set
      {
        this._activeSongIndex = value;
      }
    }

    internal int Count
    {
      get
      {
        return this.songs.Count;
      }
    }

    internal IEnumerable<Song> Songs
    {
      get
      {
        return (IEnumerable<Song>) this.songs;
      }
    }

    internal Song GetNextSong(int direction, bool shuffle)
    {
      this._activeSongIndex = !shuffle ? MathHelper.Clamp(this._activeSongIndex + direction, 0, this.songs.Count - 1) : this.random.Next(this.songs.Count);
      return this.songs[this._activeSongIndex];
    }

    internal void Clear()
    {
      while (this.songs.Count > 0)
      {
        Song song = this.songs[0];
        song.Stop();
        this.songs.Remove(song);
      }
    }

    internal void SetVolume(float volume)
    {
      int count = this.songs.Count;
      for (int index = 0; index < count; ++index)
        this.songs[index].Volume = volume;
    }

    internal void Add(Song song)
    {
      this.songs.Add(song);
    }

    internal void Stop()
    {
      int count = this.songs.Count;
      for (int index = 0; index < count; ++index)
        this.songs[index].Stop();
    }
  }
}
