// Type: Microsoft.Xna.Framework.Audio.XactClip
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System;
using System.IO;

namespace Microsoft.Xna.Framework.Audio
{
  internal class XactClip
  {
    private float volume;
    private XactClip.ClipEvent[] events;

    public bool Playing
    {
      get
      {
        return this.events[0].Playing;
      }
    }

    public float Volume
    {
      get
      {
        return this.volume;
      }
      set
      {
        this.volume = value;
        this.events[0].Volume = value;
      }
    }

    public XactClip(SoundBank soundBank, BinaryReader clipReader, uint clipOffset)
    {
      long position = clipReader.BaseStream.Position;
      clipReader.BaseStream.Seek((long) clipOffset, SeekOrigin.Begin);
      byte num1 = clipReader.ReadByte();
      this.events = new XactClip.ClipEvent[(int) num1];
      for (int index = 0; index < (int) num1; ++index)
      {
        if ((int) (clipReader.ReadUInt32() & 31U) != 1)
          throw new NotImplementedException();
        XactClip.EventPlayWave eventPlayWave = new XactClip.EventPlayWave();
        int num2 = (int) clipReader.ReadUInt32();
        uint trackIndex = (uint) clipReader.ReadUInt16();
        byte waveBankIndex = clipReader.ReadByte();
        int num3 = (int) clipReader.ReadByte();
        int num4 = (int) clipReader.ReadUInt16();
        int num5 = (int) clipReader.ReadUInt16();
        eventPlayWave.wave = soundBank.GetWave(waveBankIndex, trackIndex);
        this.events[index] = (XactClip.ClipEvent) eventPlayWave;
        this.events[index].clip = this;
      }
      clipReader.BaseStream.Seek(position, SeekOrigin.Begin);
    }

    public void Play()
    {
      this.events[0].Play();
    }

    public void Stop()
    {
      this.events[0].Stop();
    }

    public void Pause()
    {
      this.events[0].Pause();
    }

    private abstract class ClipEvent
    {
      public XactClip clip;

      public abstract bool Playing { get; }

      public abstract float Volume { get; set; }

      public abstract void Play();

      public abstract void Stop();

      public abstract void Pause();
    }

    private class EventPlayWave : XactClip.ClipEvent
    {
      public SoundEffectInstance wave;

      public override bool Playing
      {
        get
        {
          return this.wave.State == SoundState.Playing;
        }
      }

      public override float Volume
      {
        get
        {
          return this.wave.Volume;
        }
        set
        {
          this.wave.Volume = value;
        }
      }

      public override void Play()
      {
        this.wave.Volume = this.clip.Volume;
        if (this.wave.State == SoundState.Playing)
          this.wave.Stop(false);
        this.wave.Play();
      }

      public override void Stop()
      {
        this.wave.Stop(false);
      }

      public override void Pause()
      {
        this.wave.Pause();
      }
    }
  }
}
