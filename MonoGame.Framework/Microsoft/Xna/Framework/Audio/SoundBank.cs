// Type: Microsoft.Xna.Framework.Audio.SoundBank
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Microsoft.Xna.Framework.Audio
{
  public class SoundBank : IDisposable
  {
    private Dictionary<string, Cue> cues = new Dictionary<string, Cue>();
    private string name;
    private string filename;
    private AudioEngine audioengine;
    private WaveBank[] waveBanks;
    private bool loaded;

    public SoundBank(AudioEngine audioEngine, string fileName)
    {
      this.filename = fileName.Replace('\\', Path.DirectorySeparatorChar);
      this.audioengine = audioEngine;
    }

    private void Load()
    {
      using (FileStream fileStream = new FileStream(this.filename, FileMode.Open))
      {
        using (BinaryReader soundReader = new BinaryReader((Stream) fileStream))
        {
          if ((int) soundReader.ReadUInt32() != 1262634067)
            throw new Exception("Bad soundbank format");
          int num1 = (int) soundReader.ReadUInt16();
          int num2 = (int) soundReader.ReadUInt16();
          int num3 = (int) soundReader.ReadUInt16();
          int num4 = (int) soundReader.ReadUInt32();
          int num5 = (int) soundReader.ReadUInt32();
          int num6 = (int) soundReader.ReadByte();
          uint num7 = (uint) soundReader.ReadUInt16();
          uint num8 = (uint) soundReader.ReadUInt16();
          int num9 = (int) soundReader.ReadUInt16();
          int num10 = (int) soundReader.ReadUInt16();
          uint num11 = (uint) soundReader.ReadByte();
          int num12 = (int) soundReader.ReadUInt16();
          uint num13 = (uint) soundReader.ReadUInt16();
          int num14 = (int) soundReader.ReadUInt16();
          uint num15 = soundReader.ReadUInt32();
          uint num16 = soundReader.ReadUInt32();
          uint num17 = soundReader.ReadUInt32();
          int num18 = (int) soundReader.ReadUInt32();
          int num19 = (int) soundReader.ReadUInt32();
          int num20 = (int) soundReader.ReadUInt32();
          uint num21 = soundReader.ReadUInt32();
          int num22 = (int) soundReader.ReadUInt32();
          int num23 = (int) soundReader.ReadUInt32();
          int num24 = (int) soundReader.ReadUInt32();
          this.name = Encoding.UTF8.GetString(soundReader.ReadBytes(64)).Replace("\0", "");
          fileStream.Seek((long) num21, SeekOrigin.Begin);
          this.waveBanks = new WaveBank[(IntPtr) num11];
          for (int index1 = 0; (long) index1 < (long) num11; ++index1)
          {
            string index2 = Encoding.UTF8.GetString(soundReader.ReadBytes(64)).Replace("\0", "");
            this.waveBanks[index1] = this.audioengine.Wavebanks[index2];
          }
          fileStream.Seek((long) num17, SeekOrigin.Begin);
          string[] strArray = Encoding.UTF8.GetString(soundReader.ReadBytes((int) num13)).Split(new char[1]);
          fileStream.Seek((long) num15, SeekOrigin.Begin);
          for (int index = 0; (long) index < (long) num7; ++index)
          {
            int num25 = (int) soundReader.ReadByte();
            uint soundOffset = soundReader.ReadUInt32();
            XactSound sound = new XactSound(this, soundReader, soundOffset);
            Cue cue = new Cue(this.audioengine, strArray[index], sound);
            this.cues.Add(cue.Name, cue);
          }
          fileStream.Seek((long) num16, SeekOrigin.Begin);
          for (int index1 = 0; (long) index1 < (long) num8; ++index1)
          {
            Cue cue;
            if (((int) soundReader.ReadByte() >> 2 & 1) != 0)
            {
              uint soundOffset = soundReader.ReadUInt32();
              int num25 = (int) soundReader.ReadUInt32();
              XactSound sound = new XactSound(this, soundReader, soundOffset);
              cue = new Cue(this.audioengine, strArray[(long) num7 + (long) index1], sound);
            }
            else
            {
              uint num25 = soundReader.ReadUInt32();
              int num26 = (int) soundReader.ReadUInt32();
              long position = fileStream.Position;
              fileStream.Seek((long) num25, SeekOrigin.Begin);
              uint num27 = (uint) soundReader.ReadUInt16();
              uint num28 = (uint) soundReader.ReadUInt16();
              int num29 = (int) soundReader.ReadByte();
              int num30 = (int) soundReader.ReadUInt16();
              int num31 = (int) soundReader.ReadByte();
              XactSound[] _sounds = new XactSound[(IntPtr) num27];
              float[] _probs = new float[(IntPtr) num27];
              uint num32 = num28 >> 3 & 7U;
              for (int index2 = 0; (long) index2 < (long) num27; ++index2)
              {
                switch (num32)
                {
                  case 0U:
                    uint trackIndex1 = (uint) soundReader.ReadUInt16();
                    byte waveBankIndex1 = soundReader.ReadByte();
                    int num33 = (int) soundReader.ReadByte();
                    int num34 = (int) soundReader.ReadByte();
                    _sounds[index2] = new XactSound(this.GetWave(waveBankIndex1, trackIndex1));
                    break;
                  case 1U:
                    uint soundOffset = soundReader.ReadUInt32();
                    int num35 = (int) soundReader.ReadByte();
                    int num36 = (int) soundReader.ReadByte();
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
              cue = new Cue(strArray[(long) num7 + (long) index1], _sounds, _probs);
            }
            int num37 = (int) soundReader.ReadUInt32();
            int num38 = (int) soundReader.ReadByte();
            int num39 = (int) soundReader.ReadByte();
            this.cues.Add(cue.Name, cue);
          }
        }
      }
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
