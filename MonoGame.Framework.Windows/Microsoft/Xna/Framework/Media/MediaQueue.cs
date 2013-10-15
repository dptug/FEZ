// Type: Microsoft.Xna.Framework.Media.MediaQueue
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Media
{
  public sealed class MediaQueue
  {
    private List<Song> songs = new List<Song>();
    private int _activeSongIndex = 0;
    private Random random = new Random();

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
      this._activeSongIndex = !shuffle ? (int) MathHelper.Clamp((float) (this._activeSongIndex + direction), 0.0f, (float) (this.songs.Count - 1)) : this.random.Next(this.songs.Count);
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
