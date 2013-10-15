// Type: Microsoft.Xna.Framework.Media.MediaPlayer
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System;

namespace Microsoft.Xna.Framework.Media
{
  public static class MediaPlayer
  {
    private static int _numSongsInQueuePlayed = 0;
    private static MediaState _state = MediaState.Stopped;
    private static float _volume = 1f;
    private static bool _isMuted = false;
    private static MediaQueue _queue = new MediaQueue();
    private static bool _isRepeating;

    public static MediaQueue Queue
    {
      get
      {
        return MediaPlayer._queue;
      }
    }

    public static bool IsMuted
    {
      get
      {
        return MediaPlayer._isMuted;
      }
      set
      {
        MediaPlayer._isMuted = value;
        if (MediaPlayer._queue.Count == 0)
          return;
        float volume = value ? 0.0f : MediaPlayer._volume;
        MediaPlayer._queue.SetVolume(volume);
      }
    }

    public static bool IsRepeating
    {
      get
      {
        return MediaPlayer._isRepeating;
      }
      set
      {
        MediaPlayer._isRepeating = value;
      }
    }

    public static bool IsShuffled { get; set; }

    public static bool IsVisualizationEnabled
    {
      get
      {
        return false;
      }
    }

    public static TimeSpan PlayPosition
    {
      get
      {
        if (MediaPlayer._queue.ActiveSong == (Song) null)
          return TimeSpan.Zero;
        else
          return MediaPlayer._queue.ActiveSong.Position;
      }
    }

    public static MediaState State
    {
      get
      {
        return MediaPlayer._state;
      }
      private set
      {
        if (MediaPlayer._state == value)
          return;
        MediaPlayer._state = value;
        if (MediaPlayer.MediaStateChanged != null)
          MediaPlayer.MediaStateChanged((object) null, EventArgs.Empty);
      }
    }

    public static bool GameHasControl
    {
      get
      {
        return true;
      }
    }

    public static float Volume
    {
      get
      {
        return MediaPlayer._volume;
      }
      set
      {
        MediaPlayer._volume = value;
        if (MediaPlayer._queue.ActiveSong == (Song) null)
          return;
        MediaPlayer._queue.SetVolume(MediaPlayer._isMuted ? 0.0f : value);
      }
    }

    public static event EventHandler<EventArgs> MediaStateChanged;

    static MediaPlayer()
    {
    }

    public static void Pause()
    {
      if (MediaPlayer._queue.ActiveSong == (Song) null)
        return;
      MediaPlayer._queue.ActiveSong.Pause();
      MediaPlayer.State = MediaState.Paused;
    }

    public static void Play(Song song)
    {
      MediaPlayer._queue.Clear();
      MediaPlayer._numSongsInQueuePlayed = 0;
      MediaPlayer._queue.Add(song);
      MediaPlayer.PlaySong(song);
    }

    public static void Play(SongCollection collection, int index = 0)
    {
      MediaPlayer._queue.Clear();
      MediaPlayer._numSongsInQueuePlayed = 0;
      foreach (Song song in collection)
        MediaPlayer._queue.Add(song);
      MediaPlayer._queue.ActiveSongIndex = index;
      MediaPlayer.PlaySong(MediaPlayer._queue.ActiveSong);
    }

    private static void PlaySong(Song song)
    {
      song.SetEventHandler(new Song.FinishedPlayingHandler(MediaPlayer.OnSongFinishedPlaying));
      song.Volume = MediaPlayer._isMuted ? 0.0f : MediaPlayer._volume;
      song.Play();
      MediaPlayer.State = MediaState.Playing;
    }

    internal static void OnSongFinishedPlaying(object sender, EventArgs args)
    {
      ++MediaPlayer._numSongsInQueuePlayed;
      if (MediaPlayer._numSongsInQueuePlayed >= MediaPlayer._queue.Count)
      {
        MediaPlayer._numSongsInQueuePlayed = 0;
        if (!MediaPlayer.IsRepeating)
        {
          MediaPlayer.State = MediaState.Stopped;
          return;
        }
      }
      MediaPlayer.MoveNext();
    }

    public static void Resume()
    {
      if (MediaPlayer._queue.ActiveSong == (Song) null)
        return;
      MediaPlayer._queue.ActiveSong.Resume();
      MediaPlayer.State = MediaState.Playing;
    }

    public static void Stop()
    {
      if (MediaPlayer._queue.ActiveSong == (Song) null)
        return;
      foreach (Song song in MediaPlayer.Queue.Songs)
        MediaPlayer._queue.ActiveSong.Stop();
      MediaPlayer.State = MediaState.Stopped;
    }

    public static void MoveNext()
    {
      MediaPlayer.NextSong(1);
    }

    public static void MovePrevious()
    {
      MediaPlayer.NextSong(-1);
    }

    private static void NextSong(int direction)
    {
      Song nextSong = MediaPlayer._queue.GetNextSong(direction, MediaPlayer.IsShuffled);
      if (nextSong == (Song) null)
        MediaPlayer.Stop();
      else
        MediaPlayer.Play(nextSong);
    }
  }
}
