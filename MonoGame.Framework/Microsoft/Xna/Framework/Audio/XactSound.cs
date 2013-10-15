// Type: Microsoft.Xna.Framework.Audio.XactSound
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System;
using System.IO;

namespace Microsoft.Xna.Framework.Audio
{
  internal class XactSound
  {
    private bool complexSound;
    private XactClip[] soundClips;
    private SoundEffectInstance wave;

    public float Volume
    {
      get
      {
        if (this.complexSound)
          return this.soundClips[0].Volume;
        else
          return this.wave.Volume;
      }
      set
      {
        if (this.complexSound)
        {
          foreach (XactClip xactClip in this.soundClips)
            xactClip.Volume = value;
        }
        else
          this.wave.Volume = value;
      }
    }

    public bool Playing
    {
      get
      {
        if (!this.complexSound)
          return this.wave.State == SoundState.Playing;
        foreach (XactClip xactClip in this.soundClips)
        {
          if (xactClip.Playing)
            return true;
        }
        return false;
      }
    }

    public XactSound(SoundBank soundBank, BinaryReader soundReader, uint soundOffset)
    {
      long position = soundReader.BaseStream.Position;
      soundReader.BaseStream.Seek((long) soundOffset, SeekOrigin.Begin);
      byte num1 = soundReader.ReadByte();
      this.complexSound = ((int) num1 & 1) != 0;
      int num2 = (int) soundReader.ReadUInt16();
      int num3 = (int) soundReader.ReadByte();
      int num4 = (int) soundReader.ReadUInt16();
      int num5 = (int) soundReader.ReadByte();
      int num6 = (int) soundReader.ReadUInt16();
      uint num7 = 0U;
      if (this.complexSound)
      {
        num7 = (uint) soundReader.ReadByte();
      }
      else
      {
        uint trackIndex = (uint) soundReader.ReadUInt16();
        byte waveBankIndex = soundReader.ReadByte();
        this.wave = soundBank.GetWave(waveBankIndex, trackIndex);
      }
      if (((int) num1 & 30) != 0)
      {
        uint num8 = (uint) soundReader.ReadUInt16();
        soundReader.BaseStream.Seek((long) (num8 - 2U), SeekOrigin.Current);
      }
      if (this.complexSound)
      {
        this.soundClips = new XactClip[(IntPtr) num7];
        for (int index = 0; (long) index < (long) num7; ++index)
        {
          int num8 = (int) soundReader.ReadByte();
          uint clipOffset = soundReader.ReadUInt32();
          int num9 = (int) soundReader.ReadUInt32();
          this.soundClips[index] = new XactClip(soundBank, soundReader, clipOffset);
        }
      }
      soundReader.BaseStream.Seek(position, SeekOrigin.Begin);
    }

    public XactSound(SoundEffectInstance sound)
    {
      this.complexSound = false;
      this.wave = sound;
    }

    public void Play()
    {
      if (this.complexSound)
      {
        foreach (XactClip xactClip in this.soundClips)
          xactClip.Play();
      }
      else
      {
        if (this.wave.State == SoundState.Playing)
          this.wave.Stop(false);
        this.wave.Play();
      }
    }

    public void Stop()
    {
      if (this.complexSound)
      {
        foreach (XactClip xactClip in this.soundClips)
          xactClip.Stop();
      }
      else
        this.wave.Stop(false);
    }

    public void Pause()
    {
      if (this.complexSound)
      {
        foreach (XactClip xactClip in this.soundClips)
          xactClip.Pause();
      }
      else
        this.wave.Pause();
    }

    public void Resume()
    {
      if (this.complexSound)
      {
        foreach (XactClip xactClip in this.soundClips)
          xactClip.Play();
      }
      else
        this.wave.Resume();
    }
  }
}
