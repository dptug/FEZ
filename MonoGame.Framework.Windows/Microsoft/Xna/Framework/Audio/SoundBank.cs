// Type: Microsoft.Xna.Framework.Audio.SoundBank
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Microsoft.Xna.Framework.Audio
{
  public class SoundBank : IDisposable
  {
    private Dictionary<string, Cue> cues = new Dictionary<string, Cue>();
    private bool loaded = false;
    private string name;
    private string filename;
    private AudioEngine audioengine;
    private WaveBank[] waveBanks;

    public SoundBank(AudioEngine audioEngine, string fileName)
    {
      this.filename = fileName.Replace('\\', Path.DirectorySeparatorChar);
      this.audioengine = audioEngine;
    }

    private void Load()
    {
      FileStream fileStream = new FileStream(this.filename, FileMode.Open);
      BinaryReader soundReader = new BinaryReader((Stream) fileStream);
      if ((int) soundReader.ReadUInt32() != 1262634067)
        throw new Exception("Bad soundbank format");
      uint num1 = (uint) soundReader.ReadUInt16();
      if ((int) soundReader.ReadUInt16() == 46)
        ;
      uint num2 = (uint) soundReader.ReadUInt16();
      soundReader.ReadUInt32();
      soundReader.ReadUInt32();
      uint num3 = (uint) soundReader.ReadByte();
      uint num4 = (uint) soundReader.ReadUInt16();
      uint num5 = (uint) soundReader.ReadUInt16();
      int num6 = (int) soundReader.ReadUInt16();
      uint num7 = (uint) soundReader.ReadUInt16();
      uint num8 = (uint) soundReader.ReadByte();
      uint num9 = (uint) soundReader.ReadUInt16();
      uint num10 = (uint) soundReader.ReadUInt16();
      int num11 = (int) soundReader.ReadUInt16();
      uint num12 = soundReader.ReadUInt32();
      uint num13 = soundReader.ReadUInt32();
      uint num14 = soundReader.ReadUInt32();
      int num15 = (int) soundReader.ReadUInt32();
      soundReader.ReadUInt32();
      int num16 = (int) soundReader.ReadUInt32();
      uint num17 = soundReader.ReadUInt32();
      soundReader.ReadUInt32();
      soundReader.ReadUInt32();
      soundReader.ReadUInt32();
      this.name = Encoding.UTF8.GetString(soundReader.ReadBytes(64)).Replace("\0", "");
      fileStream.Seek((long) num17, SeekOrigin.Begin);
      this.waveBanks = new WaveBank[(IntPtr) num8];
      for (int index1 = 0; (long) index1 < (long) num8; ++index1)
      {
        string index2 = Encoding.UTF8.GetString(soundReader.ReadBytes(64)).Replace("\0", "");
        this.waveBanks[index1] = this.audioengine.Wavebanks[index2];
      }
      fileStream.Seek((long) num14, SeekOrigin.Begin);
      string[] strArray = Encoding.UTF8.GetString(soundReader.ReadBytes((int) num10)).Split(new char[1]);
      fileStream.Seek((long) num12, SeekOrigin.Begin);
      for (int index = 0; (long) index < (long) num4; ++index)
      {
        soundReader.ReadByte();
        uint soundOffset = soundReader.ReadUInt32();
        XactSound sound = new XactSound(this, soundReader, soundOffset);
        Cue cue = new Cue(this.audioengine, strArray[index], sound);
        this.cues.Add(cue.Name, cue);
      }
      fileStream.Seek((long) num13, SeekOrigin.Begin);
      for (int index1 = 0; (long) index1 < (long) num5; ++index1)
      {
        Cue cue;
        if (((int) soundReader.ReadByte() >> 2 & 1) != 0)
        {
          uint soundOffset = soundReader.ReadUInt32();
          int num18 = (int) soundReader.ReadUInt32();
          XactSound sound = new XactSound(this, soundReader, soundOffset);
          cue = new Cue(this.audioengine, strArray[(long) num4 + (long) index1], sound);
        }
        else
        {
          uint num18 = soundReader.ReadUInt32();
          soundReader.ReadUInt32();
          long position = fileStream.Position;
          fileStream.Seek((long) num18, SeekOrigin.Begin);
          uint num19 = (uint) soundReader.ReadUInt16();
          uint num20 = (uint) soundReader.ReadUInt16();
          int num21 = (int) soundReader.ReadByte();
          int num22 = (int) soundReader.ReadUInt16();
          int num23 = (int) soundReader.ReadByte();
          XactSound[] _sounds = new XactSound[(IntPtr) num19];
          float[] _probs = new float[(IntPtr) num19];
          uint num24 = num20 >> 3 & 7U;
          for (int index2 = 0; (long) index2 < (long) num19; ++index2)
          {
            byte num25;
            byte num26;
            switch (num24)
            {
              case 0U:
                uint trackIndex1 = (uint) soundReader.ReadUInt16();
                byte waveBankIndex1 = soundReader.ReadByte();
                num25 = soundReader.ReadByte();
                num26 = soundReader.ReadByte();
                _sounds[index2] = new XactSound(this.GetWave(waveBankIndex1, trackIndex1));
                break;
              case 1U:
                uint soundOffset = soundReader.ReadUInt32();
                num25 = soundReader.ReadByte();
                num26 = soundReader.ReadByte();
                _sounds[index2] = new XactSound(this, soundReader, soundOffset);
                break;
              case 4U:
                uint trackIndex2 = (uint) soundReader.ReadUInt16();
                byte waveBankIndex2 = soundReader.ReadByte();
                _sounds[index2] = new XactSound(this.GetWave(waveBankIndex2, trackIndex2));
                break;
              default:
                throw new NotImplementedException();
            }
          }
          fileStream.Seek(position, SeekOrigin.Begin);
          cue = new Cue(strArray[(long) num4 + (long) index1], _sounds, _probs);
        }
        int num27 = (int) soundReader.ReadUInt32();
        int num28 = (int) soundReader.ReadByte();
        int num29 = (int) soundReader.ReadByte();
        this.cues.Add(cue.Name, cue);
      }
      soundReader.Close();
      fileStream.Close();
      this.loaded = true;
    }

    internal SoundEffectInstance GetWave(byte waveBankIndex, uint trackIndex)
    {
      return this.waveBanks[(int) waveBankIndex].sounds[(IntPtr) trackIndex];
    }

    public Cue GetCue(string name)
    {
      if (!this.loaded)
        this.Load();
      return this.cues[name];
    }

    public void PlayCue(string name)
    {
      this.GetCue(name).Play();
    }

    public void PlayCue(string name, AudioListener listener, AudioEmitter emitter)
    {
      throw new NotImplementedException();
    }

    public void Dispose()
    {
      throw new NotImplementedException();
    }
  }
}
