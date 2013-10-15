// Type: Microsoft.Xna.Framework.Audio.SoundEffect
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework.Content;
using OpenTK.Audio.OpenAL;
using System;
using System.IO;

namespace Microsoft.Xna.Framework.Audio
{
  public sealed class SoundEffect : IDisposable
  {
    private static float _masterVolume = 1f;
    private static float _distanceScale = 1f;
    private static float _dopplerScale = 1f;
    private static float speedOfSound = 343.5f;
    private string _name = "";
    private string _filename = "";
    private TimeSpan _duration = TimeSpan.Zero;
    internal byte[] _data;
    private bool isDisposed;

    internal int Rate { get; set; }

    internal ALFormat Format { get; set; }

    internal int Size { get; set; }

    public TimeSpan Duration
    {
      get
      {
        return this._duration;
      }
    }

    public string Name
    {
      get
      {
        return this._name;
      }
      set
      {
        this._name = value;
      }
    }

    public bool IsDisposed
    {
      get
      {
        return this.isDisposed;
      }
    }

    public static float MasterVolume
    {
      get
      {
        return SoundEffect._masterVolume;
      }
      set
      {
        SoundEffect._masterVolume = value;
      }
    }

    public static float DistanceScale
    {
      get
      {
        return SoundEffect._distanceScale;
      }
      set
      {
        if ((double) value <= 0.0)
          throw new ArgumentOutOfRangeException("value of DistanceScale");
        SoundEffect._distanceScale = value;
      }
    }

    public static float DopplerScale
    {
      get
      {
        return SoundEffect._dopplerScale;
      }
      set
      {
        if ((double) value < 0.0)
          throw new ArgumentOutOfRangeException("value of DopplerScale");
        SoundEffect._dopplerScale = value;
      }
    }

    public static float SpeedOfSound
    {
      get
      {
        return SoundEffect.speedOfSound;
      }
      set
      {
        SoundEffect.speedOfSound = value;
      }
    }

    static SoundEffect()
    {
    }

    internal SoundEffect(string fileName)
    {
      this._filename = fileName;
      if (this._filename == string.Empty)
        throw new FileNotFoundException("Supported Sound Effect formats are wav, mp3, acc, aiff");
      this._name = Path.GetFileNameWithoutExtension(fileName);
      Stream s;
      try
      {
        s = (Stream) File.OpenRead(fileName);
      }
      catch (IOException ex)
      {
        throw new ContentLoadException("Could not load audio data", (Exception) ex);
      }
      this._data = this.LoadAudioStream(s, 1f, false);
      s.Close();
    }

    internal SoundEffect(string name, bool isAdpcm, byte[] data)
    {
      this._data = data;
      this._name = name;
      Stream s;
      try
      {
        s = (Stream) new MemoryStream(data);
      }
      catch (IOException ex)
      {
        throw new ContentLoadException("Could not load audio data", (Exception) ex);
      }
      this._data = this.LoadAudioStream(s, 1f, false);
    }

    internal SoundEffect(Stream s)
    {
      this._data = this.LoadAudioStream(s, 1f, false);
    }

    public SoundEffect(byte[] buffer, int sampleRate, AudioChannels channels)
    {
      short num1 = (short) 16;
      using (MemoryStream memoryStream = new MemoryStream(44 + buffer.Length))
      {
        using (BinaryWriter binaryWriter = new BinaryWriter((Stream) memoryStream))
        {
          binaryWriter.Write("RIFF".ToCharArray());
          binaryWriter.Write(36 + buffer.Length);
          binaryWriter.Write("WAVE".ToCharArray());
          binaryWriter.Write("fmt ".ToCharArray());
          binaryWriter.Write(16);
          binaryWriter.Write((short) 1);
          binaryWriter.Write((short) channels);
          binaryWriter.Write(sampleRate);
          short num2 = (short) ((int) num1 / 8 * (int) channels);
          binaryWriter.Write(sampleRate * (int) num2);
          binaryWriter.Write(num2);
          binaryWriter.Write(num1);
          binaryWriter.Write("data".ToCharArray());
          binaryWriter.Write(buffer.Length);
          binaryWriter.Write(buffer);
          memoryStream.Seek(0L, SeekOrigin.Begin);
          this._name = "";
          this._data = this.LoadAudioStream((Stream) memoryStream, 1f, false);
          binaryWriter.Close();
          memoryStream.Close();
        }
      }
    }

    private byte[] LoadAudioStream(Stream s, float volume, bool looping)
    {
      ALFormat format;
      int size;
      int frequency;
      byte[] numArray = AudioLoader.Load(s, out format, out size, out frequency);
      this.Format = format;
      this.Size = size;
      this.Rate = frequency;
      return numArray;
    }

    public bool Play()
    {
      return this.Play(SoundEffect.MasterVolume, 0.0f, 0.0f);
    }

    public bool Play(float volume, float pitch, float pan)
    {
      if ((double) SoundEffect.MasterVolume > 0.0 && this.Size > 0)
      {
        SoundEffectInstance instance = this.CreateInstance(false);
        instance.Volume = volume;
        instance.Pitch = pitch;
        instance.Pan = pan;
        instance.Play();
      }
      return false;
    }

    public SoundEffectInstance CreateInstance(bool forceNoFilter = false)
    {
      return new SoundEffectInstance(this, forceNoFilter);
    }

    public void Dispose()
    {
      this.isDisposed = true;
      if (OpenALSoundController.Instance == null)
        return;
      OpenALSoundController.Instance.DestroySoundEffect(this);
    }

    public static SoundEffect FromStream(Stream stream)
    {
      return new SoundEffect(stream);
    }
  }
}
