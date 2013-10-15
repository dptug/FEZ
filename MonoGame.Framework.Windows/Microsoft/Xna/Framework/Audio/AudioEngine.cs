// Type: Microsoft.Xna.Framework.Audio.AudioEngine
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System;
using System.Collections.Generic;
using System.IO;

namespace Microsoft.Xna.Framework.Audio
{
  public class AudioEngine : IDisposable
  {
    internal Dictionary<string, WaveBank> Wavebanks = new Dictionary<string, WaveBank>();
    private Dictionary<string, int> categoryLookup = new Dictionary<string, int>();
    private Dictionary<string, int> variableLookup = new Dictionary<string, int>();
    public const int ContentVersion = 46;
    private AudioCategory[] categories;
    private AudioEngine.Variable[] variables;
    private AudioEngine.RpcCurve[] rpcCurves;

    public AudioEngine(string settingsFile)
      : this(settingsFile, TimeSpan.Zero, "")
    {
    }

    public AudioEngine(string settingsFile, TimeSpan lookAheadTime, string rendererId)
    {
      using (FileStream fileStream = File.OpenRead(settingsFile))
      {
        using (BinaryReader reader = new BinaryReader((Stream) fileStream))
        {
          if ((int) reader.ReadUInt32() != 1179862872)
            throw new ArgumentException("XGS format not recognized");
          uint num1 = (uint) reader.ReadUInt16();
          uint num2 = (uint) reader.ReadUInt16();
          uint num3 = (uint) reader.ReadUInt16();
          reader.ReadUInt32();
          reader.ReadUInt32();
          int num4 = (int) reader.ReadByte();
          uint count1 = (uint) reader.ReadUInt16();
          uint count2 = (uint) reader.ReadUInt16();
          int num5 = (int) reader.ReadUInt16();
          int num6 = (int) reader.ReadUInt16();
          uint num7 = (uint) reader.ReadUInt16();
          uint num8 = (uint) reader.ReadUInt16();
          uint num9 = (uint) reader.ReadUInt16();
          uint num10 = reader.ReadUInt32();
          uint num11 = reader.ReadUInt32();
          int num12 = (int) reader.ReadUInt32();
          reader.ReadUInt32();
          int num13 = (int) reader.ReadUInt32();
          reader.ReadUInt32();
          uint num14 = reader.ReadUInt32();
          uint num15 = reader.ReadUInt32();
          uint num16 = reader.ReadUInt32();
          reader.ReadUInt32();
          reader.ReadUInt32();
          reader.BaseStream.Seek((long) num14, SeekOrigin.Begin);
          string[] strArray1 = AudioEngine.readNullTerminatedStrings(count1, reader);
          this.categories = new AudioCategory[(IntPtr) count1];
          reader.BaseStream.Seek((long) num10, SeekOrigin.Begin);
          for (int index = 0; (long) index < (long) count1; ++index)
          {
            this.categories[index] = new AudioCategory(this, strArray1[index], reader);
            this.categoryLookup.Add(strArray1[index], index);
          }
          reader.BaseStream.Seek((long) num15, SeekOrigin.Begin);
          string[] strArray2 = AudioEngine.readNullTerminatedStrings(count2, reader);
          this.variables = new AudioEngine.Variable[(IntPtr) count2];
          reader.BaseStream.Seek((long) num11, SeekOrigin.Begin);
          for (int index = 0; (long) index < (long) count2; ++index)
          {
            this.variables[index].name = strArray2[index];
            byte num17 = reader.ReadByte();
            this.variables[index].isPublic = ((int) num17 & 1) != 0;
            this.variables[index].isReadOnly = ((int) num17 & 2) != 0;
            this.variables[index].isGlobal = ((int) num17 & 4) == 0;
            this.variables[index].isReserved = ((int) num17 & 8) != 0;
            this.variables[index].initValue = reader.ReadSingle();
            this.variables[index].minValue = reader.ReadSingle();
            this.variables[index].maxValue = reader.ReadSingle();
            this.variables[index].value = this.variables[index].initValue;
            this.variableLookup.Add(strArray2[index], index);
          }
          this.rpcCurves = new AudioEngine.RpcCurve[(IntPtr) num7];
          reader.BaseStream.Seek((long) num16, SeekOrigin.Begin);
          for (int index1 = 0; (long) index1 < (long) num7; ++index1)
          {
            this.rpcCurves[index1].variable = (int) reader.ReadUInt16();
            int length = (int) reader.ReadByte();
            this.rpcCurves[index1].parameter = (AudioEngine.RpcParameter) reader.ReadUInt16();
            this.rpcCurves[index1].points = new AudioEngine.RpcPoint[length];
            for (int index2 = 0; index2 < length; ++index2)
            {
              this.rpcCurves[index1].points[index2].x = reader.ReadSingle();
              this.rpcCurves[index1].points[index2].y = reader.ReadSingle();
              this.rpcCurves[index1].points[index2].type = (AudioEngine.RpcPointType) reader.ReadByte();
            }
          }
        }
      }
    }

    private static string[] readNullTerminatedStrings(uint count, BinaryReader reader)
    {
      string[] strArray = new string[(IntPtr) count];
      for (int index = 0; (long) index < (long) count; ++index)
      {
        List<char> list = new List<char>();
        while (reader.PeekChar() != 0)
          list.Add(reader.ReadChar());
        int num = (int) reader.ReadChar();
        strArray[index] = new string(list.ToArray());
      }
      return strArray;
    }

    public void Update()
    {
    }

    public AudioCategory GetCategory(string name)
    {
      return this.categories[this.categoryLookup[name]];
    }

    public float GetGlobalVariable(string name)
    {
      return this.variables[this.variableLookup[name]].value;
    }

    public void SetGlobalVariable(string name, float value)
    {
      this.variables[this.variableLookup[name]].value = value;
    }

    public void Dispose()
    {
    }

    private struct Variable
    {
      public string name;
      public float value;
      public bool isGlobal;
      public bool isReadOnly;
      public bool isPublic;
      public bool isReserved;
      public float initValue;
      public float maxValue;
      public float minValue;
    }

    private enum RpcPointType
    {
      Linear,
      Fast,
      Slow,
      SinCos,
    }

    private struct RpcPoint
    {
      public float x;
      public float y;
      public AudioEngine.RpcPointType type;
    }

    private enum RpcParameter
    {
      Volume,
      Pitch,
      ReverbSend,
      FilterFrequency,
      FilterQFactor,
    }

    private struct RpcCurve
    {
      public int variable;
      public AudioEngine.RpcParameter parameter;
      public AudioEngine.RpcPoint[] points;
    }
  }
}
