// Type: NVorbis.VorbisMode
// Assembly: NVorbis, Version=0.5.5.0, Culture=neutral, PublicKeyToken=null
// MVID: CF8FE41E-969B-4426-8E05-8E0EFF882245
// Assembly location: F:\Program Files (x86)\FEZ\NVorbis.dll

using System;
using System.IO;

namespace NVorbis
{
  internal class VorbisMode
  {
    private const float M_PI = 3.141593f;
    private const float M_PI2 = 1.570796f;
    private VorbisStreamDecoder _vorbis;
    private float[][] _windows;
    internal bool BlockFlag;
    internal int WindowType;
    internal int TransformType;
    internal VorbisMapping Mapping;
    internal int BlockSize;

    private VorbisMode(VorbisStreamDecoder vorbis)
    {
      this._vorbis = vorbis;
    }

    internal static VorbisMode Init(VorbisStreamDecoder vorbis, DataPacket packet)
    {
      VorbisMode vorbisMode = new VorbisMode(vorbis);
      vorbisMode.BlockFlag = packet.ReadBit();
      vorbisMode.WindowType = (int) packet.ReadBits(16);
      vorbisMode.TransformType = (int) packet.ReadBits(16);
      int index = (int) packet.ReadBits(8);
      if (vorbisMode.WindowType != 0 || vorbisMode.TransformType != 0 || index >= vorbis.Maps.Length)
        throw new InvalidDataException();
      vorbisMode.Mapping = vorbis.Maps[index];
      vorbisMode.BlockSize = vorbisMode.BlockFlag ? vorbis.Block1Size : vorbis.Block0Size;
      if (vorbisMode.BlockFlag)
      {
        vorbisMode._windows = new float[4][];
        vorbisMode._windows[0] = new float[vorbis.Block1Size];
        vorbisMode._windows[1] = new float[vorbis.Block1Size];
        vorbisMode._windows[2] = new float[vorbis.Block1Size];
        vorbisMode._windows[3] = new float[vorbis.Block1Size];
      }
      else
      {
        vorbisMode._windows = new float[1][];
        vorbisMode._windows[0] = new float[vorbis.Block0Size];
      }
      vorbisMode.CalcWindows();
      return vorbisMode;
    }

    private void CalcWindows()
    {
      for (int index1 = 0; index1 < this._windows.Length; ++index1)
      {
        float[] numArray = this._windows[index1];
        int num1 = ((index1 & 1) == 0 ? this._vorbis.Block0Size : this._vorbis.Block1Size) / 2;
        int num2 = this.BlockSize;
        int num3 = ((index1 & 2) == 0 ? this._vorbis.Block0Size : this._vorbis.Block1Size) / 2;
        int num4 = num2 / 4 - num1 / 2;
        int num5 = num2 - num2 / 4 - num3 / 2;
        for (int index2 = 0; index2 < num1; ++index2)
        {
          float num6 = (float) Math.Sin(((double) index2 + 0.5) / (double) num1 * 1.57079637050629);
          float num7 = num6 * num6;
          numArray[num4 + index2] = (float) Math.Sin((double) num7 * 1.57079637050629);
        }
        for (int index2 = num4 + num1; index2 < num5; ++index2)
          numArray[index2] = 1f;
        for (int index2 = 0; index2 < num3; ++index2)
        {
          float num6 = (float) Math.Sin(((double) (num3 - index2) - 0.5) / (double) num3 * 1.57079637050629);
          float num7 = num6 * num6;
          numArray[num5 + index2] = (float) Math.Sin((double) num7 * 1.57079637050629);
        }
        for (int index2 = 0; index2 < num2; ++index2)
          numArray[index2] /= 1.38396f;
      }
    }

    internal float[] GetWindow(bool prev, bool next)
    {
      if (this.BlockFlag)
      {
        if (next)
        {
          if (prev)
            return this._windows[3];
          else
            return this._windows[2];
        }
        else if (prev)
          return this._windows[1];
      }
      return this._windows[0];
    }
  }
}
