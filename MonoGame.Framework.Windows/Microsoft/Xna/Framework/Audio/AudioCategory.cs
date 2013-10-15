// Type: Microsoft.Xna.Framework.Audio.AudioCategory
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System;
using System.IO;

namespace Microsoft.Xna.Framework.Audio
{
  public struct AudioCategory : IEquatable<AudioCategory>
  {
    private string name;
    private AudioEngine engine;
    internal float volume;
    internal bool isBackgroundMusic;
    internal bool isPublic;
    internal bool instanceLimit;
    internal int maxInstances;
    internal AudioCategory.MaxInstanceBehaviour instanceBehaviour;
    internal AudioCategory.CrossfadeType fadeType;
    internal float fadeIn;
    internal float fadeOut;

    public string Name
    {
      get
      {
        return this.name;
      }
    }

    internal AudioCategory(AudioEngine audioengine, string name, BinaryReader reader)
    {
      this.name = name;
      this.engine = audioengine;
      this.maxInstances = (int) reader.ReadByte();
      this.instanceLimit = this.maxInstances != (int) byte.MaxValue;
      this.fadeIn = (float) reader.ReadUInt16() / 1000f;
      this.fadeOut = (float) reader.ReadUInt16() / 1000f;
      byte num1 = reader.ReadByte();
      this.fadeType = (AudioCategory.CrossfadeType) ((int) num1 & 7);
      this.instanceBehaviour = (AudioCategory.MaxInstanceBehaviour) ((int) num1 >> 3);
      int num2 = (int) reader.ReadUInt16();
      byte num3 = reader.ReadByte();
      double num4 = -96.0;
      double y = 0.432254984608615;
      double num5 = 80.1748600297963;
      double num6 = 67.7385212334047;
      this.volume = (float) ((num4 - num6) / (1.0 + Math.Pow((double) num3 / num5, y)) + num6);
      byte num7 = reader.ReadByte();
      this.isBackgroundMusic = ((int) num7 & 1) != 0;
      this.isPublic = ((int) num7 & 2) != 0;
    }

    public void Pause()
    {
      throw new NotImplementedException();
    }

    public void Resume()
    {
      throw new NotImplementedException();
    }

    public void Stop()
    {
      throw new NotImplementedException();
    }

    public void SetVolume(float volume)
    {
      throw new NotImplementedException();
    }

    public bool Equals(AudioCategory other)
    {
      throw new NotImplementedException();
    }

    internal enum MaxInstanceBehaviour
    {
      FailToPlay,
      Queue,
      ReplaceOldest,
      ReplaceQuietest,
      ReplaceLowestPriority,
    }

    internal enum CrossfadeType
    {
      Linear,
      Logarithmic,
      EqualPower,
    }
  }
}
