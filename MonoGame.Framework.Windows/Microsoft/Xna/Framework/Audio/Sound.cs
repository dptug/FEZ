// Type: Microsoft.Xna.Framework.Audio.Sound
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using Microsoft.Xna.Framework.Content;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using System;
using System.IO;

namespace Microsoft.Xna.Framework.Audio
{
  internal class Sound : IDisposable
  {
    private static AudioContext context = (AudioContext) null;
    private int bufferID = -1;
    private int sourceID;
    private bool looping;
    private bool loopingCtrl;
    private bool disposed;

    public double Duration
    {
      get
      {
        return this.Seconds;
      }
    }

    public double CurrentPosition
    {
      get
      {
        float num;
        AL.GetSource(this.sourceID, ALSourcef.SecOffset, out num);
        return (double) num;
      }
      set
      {
        AL.Source(this.sourceID, ALSourcef.SecOffset, (float) value);
      }
    }

    public bool Looping
    {
      get
      {
        return this.looping;
      }
      set
      {
        if (this.looping == value)
          return;
        AL.Source(this.sourceID, ALSourceb.Looping, value);
        this.loopingCtrl = this.looping = value;
      }
    }

    public float Pan { get; set; }

    public bool Playing
    {
      get
      {
        return AL.GetSourceState(this.sourceID) == ALSourceState.Playing;
      }
      set
      {
        if (value == this.Playing)
          return;
        if (value)
          this.Play();
        else
          this.Stop();
      }
    }

    public float Volume
    {
      get
      {
        float num;
        AL.GetSource(this.sourceID, ALSourcef.Gain, out num);
        return num;
      }
      set
      {
        if ((double) value < 0.0 || (double) value > 1.0)
          throw new ArgumentException("Volume should be between 0 and 1.0");
        AL.Source(this.sourceID, ALSourcef.Gain, value);
      }
    }

    public float Rate { get; set; }

    private int Channels
    {
      get
      {
        int num;
        AL.GetBuffer(this.bufferID, ALGetBufferi.Channels, out num);
        return num;
      }
    }

    private int Size
    {
      get
      {
        int num;
        AL.GetBuffer(this.bufferID, ALGetBufferi.Size, out num);
        return num;
      }
    }

    private int Bits
    {
      get
      {
        int num;
        AL.GetBuffer(this.bufferID, ALGetBufferi.Bits, out num);
        return num;
      }
    }

    private int Frequency
    {
      get
      {
        int num;
        AL.GetBuffer(this.bufferID, ALGetBufferi.Frequency, out num);
        return num;
      }
    }

    private int Samples
    {
      get
      {
        return this.Size / Sound.GetSampleSize(this.Bits, this.Channels);
      }
    }

    private double Seconds
    {
      get
      {
        return (double) this.Samples / (double) this.Frequency;
      }
    }

    static Sound()
    {
    }

    public Sound(string filename, float volume, bool looping)
    {
      Stream data1;
      try
      {
        data1 = (Stream) File.OpenRead(filename);
      }
      catch (IOException ex)
      {
        throw new ContentLoadException("Could not load audio data", (Exception) ex);
      }
      ALFormat format;
      int size;
      int frequency;
      byte[] data2 = AudioLoader.Load(data1, out format, out size, out frequency);
      data1.Close();
      this.Initialize(data2, format, size, frequency, volume, looping);
    }

    public Sound(byte[] audiodata, float volume, bool looping)
    {
      Stream data1;
      try
      {
        data1 = (Stream) new MemoryStream(audiodata);
      }
      catch (IOException ex)
      {
        throw new ContentLoadException("Could not load audio data", (Exception) ex);
      }
      ALFormat format;
      int size;
      int frequency;
      byte[] data2 = AudioLoader.Load(data1, out format, out size, out frequency);
      data1.Close();
      this.Initialize(data2, format, size, frequency, volume, looping);
    }

    ~Sound()
    {
      this.Dispose(false);
    }

    private static void InitilizeSoundServices()
    {
      if (Sound.context != null)
        return;
      Sound.context = new AudioContext();
    }

    internal static void DisposeSoundServices()
    {
      if (Sound.context == null)
        return;
      Sound.context.Dispose();
      Sound.context = (AudioContext) null;
    }

    private void Initialize(byte[] data, ALFormat format, int size, int frequency, float volume, bool looping)
    {
      Sound.InitilizeSoundServices();
      this.bufferID = AL.GenBuffer();
      this.sourceID = AL.GenSource();
      try
      {
        AL.BufferData<byte>(this.bufferID, format, data, size, frequency);
        AL.Source(this.sourceID, ALSourcei.Buffer, this.bufferID);
      }
      catch (Exception ex)
      {
        throw ex;
      }
      this.Volume = volume;
      this.looping = looping;
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.disposed)
        return;
      if (!disposing)
        ;
      this.Stop();
      AL.DeleteSource(this.sourceID);
      AL.DeleteBuffer(this.bufferID);
      this.bufferID = -1;
      this.disposed = true;
    }

    private static int GetSampleSize(int bits, int channels)
    {
      return bits / 8 * channels;
    }

    public void Pause()
    {
      AL.SourcePause(this.sourceID);
    }

    public void Resume()
    {
      this.Play();
    }

    public void Play()
    {
      if (this.looping && !this.loopingCtrl)
      {
        AL.Source(this.sourceID, ALSourceb.Looping, true);
        this.loopingCtrl = true;
      }
      AL.SourcePlay(this.sourceID);
    }

    public void Stop()
    {
      if (this.loopingCtrl)
      {
        AL.Source(this.sourceID, ALSourceb.Looping, false);
        this.loopingCtrl = false;
      }
      AL.SourceStop(this.sourceID);
    }

    public static Sound CreateAndPlay(string url, float volume, bool looping)
    {
      Sound sound = new Sound(url, volume, looping);
      sound.Play();
      return sound;
    }
  }
}
