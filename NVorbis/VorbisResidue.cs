// Type: NVorbis.VorbisResidue
// Assembly: NVorbis, Version=0.5.5.0, Culture=neutral, PublicKeyToken=null
// MVID: CF8FE41E-969B-4426-8E05-8E0EFF882245
// Assembly location: F:\Program Files (x86)\FEZ\NVorbis.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NVorbis
{
  internal abstract class VorbisResidue
  {
    private VorbisStreamDecoder _vorbis;

    protected VorbisResidue(VorbisStreamDecoder vorbis)
    {
      this._vorbis = vorbis;
    }

    internal static VorbisResidue Init(VorbisStreamDecoder vorbis, DataPacket packet)
    {
      int num = (int) packet.ReadBits(16);
      VorbisResidue vorbisResidue = (VorbisResidue) null;
      switch (num)
      {
        case 0:
          vorbisResidue = (VorbisResidue) new VorbisResidue.Residue0(vorbis);
          break;
        case 1:
          vorbisResidue = (VorbisResidue) new VorbisResidue.Residue1(vorbis);
          break;
        case 2:
          vorbisResidue = (VorbisResidue) new VorbisResidue.Residue2(vorbis);
          break;
      }
      if (vorbisResidue == null)
        throw new InvalidDataException();
      vorbisResidue.Init(packet);
      return vorbisResidue;
    }

    internal abstract float[][] Decode(DataPacket packet, bool[] doNotDecode, int channels, int blockSize);

    protected abstract void Init(DataPacket packet);

    private class Residue0 : VorbisResidue
    {
      protected int _begin;
      protected int _end;
      protected int _partitionSize;
      protected int _classifications;
      private int _classBookNum;
      protected int _maxPasses;
      protected int[] _cascade;
      private int[][] _bookNums;
      protected VorbisCodebook[][] _books;
      protected VorbisCodebook _classBook;
      protected int _classWordsPerCodeWord;
      protected int _nToRead;
      protected int _partsToRead;
      protected int _partWords;

      internal Residue0(VorbisStreamDecoder vorbis)
        : base(vorbis)
      {
      }

      protected override void Init(DataPacket packet)
      {
        this._begin = (int) packet.ReadBits(24);
        this._end = (int) packet.ReadBits(24);
        this._partitionSize = (int) packet.ReadBits(24) + 1;
        this._classifications = (int) packet.ReadBits(6) + 1;
        this._classBookNum = (int) packet.ReadBits(8);
        this._classBook = this._vorbis.Books[this._classBookNum];
        this._cascade = new int[this._classifications];
        int length = 0;
        int val2 = 0;
        for (int index = 0; index < this._classifications; ++index)
        {
          int num1 = 0;
          int num2 = (int) packet.ReadBits(3);
          if (packet.ReadBit())
            num1 = (int) packet.ReadBits(5);
          this._cascade[index] = num1 << 3 | num2;
          length += VorbisResidue.Residue0.icount(this._cascade[index]);
          val2 = Math.Max(Utils.ilog(this._cascade[index]), val2);
        }
        this._maxPasses = val2;
        int[] numArray = new int[length];
        for (int index = 0; index < length; ++index)
          numArray[index] = (int) packet.ReadBits(8);
        int num3 = 0;
        this._books = new VorbisCodebook[this._classifications][];
        this._bookNums = new int[this._classifications][];
        for (int index1 = 0; index1 < this._classifications; ++index1)
        {
          this._books[index1] = new VorbisCodebook[8];
          this._bookNums[index1] = new int[8];
          int num1 = 1;
          int index2 = 0;
          while (num1 < 256)
          {
            if ((this._cascade[index1] & num1) == num1)
            {
              int index3 = numArray[num3++];
              this._books[index1][index2] = this._vorbis.Books[index3];
              this._bookNums[index1][index2] = index3;
              if (this._books[index1][index2].MapType == 0)
                throw new InvalidDataException();
            }
            num1 <<= 1;
            ++index2;
          }
        }
        this._classWordsPerCodeWord = this._classBook.Dimensions;
        this._nToRead = this._end - this._begin;
        this._partsToRead = this._nToRead / this._partitionSize;
        this._partWords = (this._partsToRead + this._classWordsPerCodeWord - 1) / this._classWordsPerCodeWord;
      }

      private static int icount(int v)
      {
        int num = 0;
        while (v != 0)
        {
          num += v & 1;
          v >>= 1;
        }
        return num;
      }

      internal override float[][] Decode(DataPacket packet, bool[] doNotDecode, int channels, int blockSize)
      {
        float[][] numArray = ACache.Get<float>(doNotDecode.Length, blockSize);
        if (this._nToRead > 0)
        {
          int[][][] buffer = ACache.Get<int>(channels, this._partsToRead, this._classWordsPerCodeWord);
          for (int index1 = 0; index1 < this._maxPasses; ++index1)
          {
            int num1 = 0;
            int index2 = 0;
            int num2 = this._begin;
            while (num1 < this._partsToRead)
            {
              if (index1 == 0)
              {
                for (int index3 = 0; index3 < channels; ++index3)
                {
                  if (!doNotDecode[index3])
                  {
                    int num3 = this._classBook.DecodeScalar(packet);
                    for (int index4 = this._classWordsPerCodeWord - 1; index4 >= 0; --index4)
                    {
                      buffer[index3][index2][index4] = num3 % this._classifications;
                      num3 /= this._classifications;
                    }
                  }
                }
              }
              int index5 = 0;
              while (index5 < this._classWordsPerCodeWord && num1 < this._partsToRead)
              {
                for (int index3 = 0; index3 < channels; ++index3)
                {
                  if (!doNotDecode[index3])
                  {
                    VorbisCodebook vorbisCodebook = this._books[buffer[index3][index2][index5]][index1];
                    if (vorbisCodebook != null && vorbisCodebook.MapType != 0)
                    {
                      VorbisResidue.Residue0 residue0 = this;
                      int num3 = index3;
                      VorbisCodebook codebook = vorbisCodebook;
                      DataPacket packet1 = packet;
                      float[] residue = numArray[index3];
                      int offset = num2;
                      int channel = num3;
                      residue0.WriteVectors(codebook, packet1, residue, offset, channel);
                    }
                  }
                }
                ++index5;
                ++num1;
                num2 += this._partitionSize;
              }
              ++index2;
            }
          }
          ACache.Return<int>(ref buffer);
        }
        return numArray;
      }

      protected virtual void WriteVectors(VorbisCodebook codebook, DataPacket packet, float[] residue, int offset, int channel)
      {
        int num1 = this._nToRead / codebook.Dimensions;
        for (int index1 = 0; index1 < num1; ++index1)
        {
          int num2 = 0;
          int index2 = codebook.DecodeScalar(packet);
          for (int index3 = 0; index3 < codebook.Dimensions; ++index3)
            residue[offset + index1 + num2++ * num1] += codebook[index2, index3];
        }
      }
    }

    private class Residue1 : VorbisResidue.Residue0
    {
      internal Residue1(VorbisStreamDecoder vorbis)
        : base(vorbis)
      {
      }

      protected override void WriteVectors(VorbisCodebook codebook, DataPacket packet, float[] residue, int offset, int channel)
      {
        int num = 0;
        while (num < this._partitionSize)
        {
          int index1 = codebook.DecodeScalar(packet);
          for (int index2 = 0; index2 < codebook.Dimensions; ++index2)
            residue[offset + num++] += codebook[index1, index2];
        }
      }
    }

    private class Residue2 : VorbisResidue.Residue1
    {
      internal Residue2(VorbisStreamDecoder vorbis)
        : base(vorbis)
      {
      }

      internal override float[][] Decode(DataPacket packet, bool[] doNotDecode, int channels, int blockSize)
      {
        float[][] numArray = ACache.Get<float>(channels, blockSize);
        int num1 = this._partitionSize / channels;
        if (Enumerable.Contains<bool>((IEnumerable<bool>) doNotDecode, false) && this._nToRead > 0)
        {
          int[][] buffer = ACache.Get<int>(this._partWords, this._classWordsPerCodeWord);
          for (int index1 = 0; index1 < this._maxPasses; ++index1)
          {
            int num2 = 0;
            int index2 = 0;
            int num3 = this._begin;
            while (num2 < this._partsToRead)
            {
              if (index1 == 0)
              {
                int num4 = this._classBook.DecodeScalar(packet);
                for (int index3 = this._classWordsPerCodeWord - 1; index3 >= 0 && num4 > 0; --index3)
                {
                  buffer[index2][index3] = num4 % this._classifications;
                  num4 /= this._classifications;
                }
              }
              for (int index3 = 0; index3 < this._classWordsPerCodeWord && num2 < this._partsToRead; ++index3)
              {
                VorbisCodebook vorbisCodebook = this._books[buffer[index2][index3]][index1];
                if (vorbisCodebook != null && vorbisCodebook.MapType != 0)
                {
                  int index4 = 0;
                  int num4 = vorbisCodebook.Dimensions;
                  int num5 = 0;
                  int index5 = num3 / channels;
                  while (num5 < num1)
                  {
                    int index6 = vorbisCodebook.DecodeScalar(packet);
                    for (int index7 = 0; index7 < num4; ++index7)
                    {
                      numArray[index4][index5] += vorbisCodebook[index6, index7];
                      if (++index4 == channels)
                      {
                        index4 = 0;
                        ++num5;
                        ++index5;
                      }
                    }
                  }
                }
                ++num2;
                num3 += this._partitionSize;
              }
              ++index2;
            }
          }
          ACache.Return<int>(ref buffer);
        }
        return numArray;
      }
    }
  }
}
