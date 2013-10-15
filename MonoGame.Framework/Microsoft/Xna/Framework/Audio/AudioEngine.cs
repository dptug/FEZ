// Type: Microsoft.Xna.Framework.Audio.AudioEngine
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

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
          int num1 = (int) reader.ReadUInt16();
          int num2 = (int) reader.ReadUInt16();
          int num3 = (int) reader.ReadUInt16();
          int num4 = (int) reader.ReadUInt32();
          int num5 = (int) reader.ReadUInt32();
          int num6 = (int) reader.ReadByte();
          uint count1 = (uint) reader.ReadUInt16();
          uint count2 = (uint) reader.ReadUInt16();
          int num7 = (int) reader.ReadUInt16();
          int num8 = (int) reader.ReadUInt16();
          uint num9 = (uint) reader.ReadUInt16();
          int num10 = (int) reader.ReadUInt16();
          int num11 = (int) reader.ReadUInt16();
          uint num12 = reader.ReadUInt32();
          uint num13 = reader.ReadUInt32();
          int num14 = (int) reader.ReadUInt32();
          int num15 = (int) reader.ReadUInt32();
          int num16 = (int) reader.ReadUInt32();
          int num17 = (int) reader.ReadUInt32();
          uint num18 = reader.ReadUInt32();
          uint num19 = reader.ReadUInt32();
          uint num20 = reader.ReadUInt32();
          int num21 = (int) reader.ReadUInt32();
          int num22 = (int) reader.ReadUInt32();
          reader.BaseStream.Seek((long) num18, SeekOrigin.Begin);
          string[] strArray1 = AudioEngine.readNullTerminatedStrings(count1, reader);
          this.categories = new AudioCategory[(IntPtr) count1];
          reader.BaseStream.Seek((long) num12, SeekOrigin.Begin);
          for (int index = 0; (long) index < (long) count1; ++index)
          {
            this.categories[index] = new AudioCategory(this, strArray1[index], reader);
            this.categoryLookup.Add(strArray1[index], index);
          }
          reader.BaseStream.Seek((long) num19, SeekOrigin.Begin);
          string[] strArray2 = AudioEngine.readNullTerminatedStrings(count2, reader);
          this.variables = new AudioEngine.Variable[(IntPtr) count2];
          reader.BaseStream.Seek((long) num13, SeekOrigin.Begin);
          for (int index = 0; (long) index < (long) count2; ++index)
          {
            this.variables[index].name = strArray2[index];
            byte num23 = reader.ReadByte();
            this.variables[index].isPublic = ((int) num23 & 1) != 0;
            this.variables[index].isReadOnly = ((int) num23 & 2) != 0;
            this.variables[index].isGlobal = ((int) num23 & 4) == 0;
            this.variables[index].isReserved = ((int) num23 & 8) != 0;
            this.variables[index].initValue = reader.ReadSingle();
            this.variables[index].minValue = reader.ReadSingle();
            this.variables[index].maxValue = reader.ReadSingle();
            this.variables[index].value = this.variables[index].initValue;
            this.variableLookup.Add(strArray2[index], index);
          }
          this.rpcCurves = new AudioEngine.RpcCurve[(IntPtr) num9];
          reader.BaseStream.Seek((long) num20, SeekOrigin.Begin);
          for (int index1 = 0; (long) index1 < (long) num9; ++index1)
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
