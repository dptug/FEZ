// Type: NVorbis.VorbisCodebook
// Assembly: NVorbis, Version=0.5.5.0, Culture=neutral, PublicKeyToken=null
// MVID: CF8FE41E-969B-4426-8E05-8E0EFF882245
// Assembly location: F:\Program Files (x86)\FEZ\NVorbis.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NVorbis
{
  internal class VorbisCodebook
  {
    internal int BookNum;
    internal int Dimensions;
    private int Entries;
    private int[] Lengths;
    private float[] LookupTable;
    internal int MapType;
    private HuffmanListNode<int> LTree;
    private int MaxBits;

    internal float this[int entry, int dim]
    {
      get
      {
        return this.LookupTable[entry * this.Dimensions + dim];
      }
    }

    private VorbisCodebook()
    {
    }

    internal static VorbisCodebook Init(VorbisStreamDecoder vorbis, DataPacket packet, int number)
    {
      VorbisCodebook vorbisCodebook = new VorbisCodebook();
      vorbisCodebook.BookNum = number;
      vorbisCodebook.Init(packet);
      return vorbisCodebook;
    }

    internal void Init(DataPacket packet)
    {
      if ((long) packet.ReadBits(24) != 5653314L)
        throw new InvalidDataException();
      this.Dimensions = (int) packet.ReadBits(16);
      this.Entries = (int) packet.ReadBits(24);
      this.Lengths = new int[this.Entries];
      this.InitTree(packet);
      this.InitLookupTable(packet);
    }

    private void InitTree(DataPacket packet)
    {
      int num1 = 0;
      bool flag;
      if (packet.ReadBit())
      {
        int num2 = (int) packet.ReadBits(5) + 1;
        int num3 = 0;
        while (num3 < this.Entries)
        {
          int num4 = (int) packet.ReadBits(Utils.ilog(this.Entries - num3));
          while (--num4 >= 0)
            this.Lengths[num3++] = num2;
          ++num2;
        }
        num1 = 0;
        flag = false;
      }
      else
      {
        flag = packet.ReadBit();
        for (int index = 0; index < this.Entries; ++index)
        {
          if (!flag || packet.ReadBit())
          {
            this.Lengths[index] = (int) packet.ReadBits(5) + 1;
            ++num1;
          }
          else
            this.Lengths[index] = -1;
        }
      }
      this.MaxBits = Enumerable.Max((IEnumerable<int>) this.Lengths);
      int[] numArray1 = (int[]) null;
      if (flag && num1 >= this.Entries >> 2)
      {
        numArray1 = new int[this.Entries];
        Array.Copy((Array) this.Lengths, (Array) numArray1, this.Entries);
        flag = false;
      }
      int length = !flag ? 0 : num1;
      int[] numArray2 = (int[]) null;
      int[] codeList = (int[]) null;
      if (!flag)
        codeList = new int[this.Entries];
      else if (length != 0)
      {
        numArray1 = new int[length];
        codeList = new int[length];
        numArray2 = new int[length];
      }
      VorbisCodebook vorbisCodebook = this;
      int[] numArray3 = this.Lengths;
      int num5 = this.Entries;
      int[] numArray4 = numArray2;
      int num6 = flag ? 1 : 0;
      int sortedEntries = length;
      int[] codewords = codeList;
      int[] codewordLengths = numArray1;
      int[] len = numArray3;
      int n = num5;
      int[] values = numArray4;
      if (!vorbisCodebook.ComputeCodewords(num6 != 0, sortedEntries, codewords, codewordLengths, len, n, values))
        throw new InvalidDataException();
      this.LTree = Huffman.BuildLinkedList<int>(numArray2 ?? Enumerable.ToArray<int>(Enumerable.Range(0, codeList.Length)), numArray1 ?? this.Lengths, codeList);
    }

    private bool ComputeCodewords(bool sparse, int sortedEntries, int[] codewords, int[] codewordLengths, int[] len, int n, int[] values)
    {
      int num1 = 0;
      uint[] numArray = new uint[32];
      int index1 = 0;
      while (index1 < n && len[index1] <= 0)
        ++index1;
      if (index1 == n)
        return true;
      VorbisCodebook vorbisCodebook = this;
      int num2 = sparse ? 1 : 0;
      int[] codewords1 = codewords;
      int[] codewordLengths1 = codewordLengths;
      int num3 = 0;
      int symbol1 = index1;
      int count = num1;
      int num4 = 1;
      int num5 = count + num4;
      int len1 = len[index1];
      int[] values1 = values;
      vorbisCodebook.AddEntry(num2 != 0, codewords1, codewordLengths1, (uint) num3, symbol1, count, len1, values1);
      for (int index2 = 1; index2 <= len[index1]; ++index2)
        numArray[index2] = (uint) (1 << 32 - index2);
      for (int symbol2 = index1 + 1; symbol2 < n; ++symbol2)
      {
        int index2 = len[symbol2];
        if (index2 > 0)
        {
          while (index2 > 0 && (int) numArray[index2] == 0)
            --index2;
          if (index2 == 0)
            return false;
          uint n1 = numArray[index2];
          numArray[index2] = 0U;
          this.AddEntry(sparse, codewords, codewordLengths, Utils.BitReverse(n1), symbol2, num5++, len[symbol2], values);
          if (index2 != len[symbol2])
          {
            for (int index3 = len[symbol2]; index3 > index2; --index3)
              numArray[index3] = n1 + (uint) (1 << 32 - index3);
          }
        }
      }
      return true;
    }

    private void AddEntry(bool sparse, int[] codewords, int[] codewordLengths, uint huffCode, int symbol, int count, int len, int[] values)
    {
      if (sparse)
      {
        codewords[count] = (int) huffCode;
        codewordLengths[count] = len;
        values[count] = symbol;
      }
      else
        codewords[symbol] = (int) huffCode;
    }

    private void InitLookupTable(DataPacket packet)
    {
      this.MapType = (int) packet.ReadBits(4);
      if (this.MapType == 0)
        return;
      float num1 = Utils.ConvertFromVorbisFloat32(packet.ReadUInt32());
      float num2 = Utils.ConvertFromVorbisFloat32(packet.ReadUInt32());
      int count = (int) packet.ReadBits(4) + 1;
      bool flag = packet.ReadBit();
      int length = this.Entries * this.Dimensions;
      float[] numArray1 = new float[length];
      if (this.MapType == 1)
        length = this.lookup1_values();
      uint[] numArray2 = new uint[length];
      for (int index = 0; index < length; ++index)
        numArray2[index] = (uint) packet.ReadBits(count);
      if (this.MapType == 1)
      {
        for (int index1 = 0; index1 < this.Entries; ++index1)
        {
          double num3 = 0.0;
          int num4 = 1;
          for (int index2 = 0; index2 < this.Dimensions; ++index2)
          {
            int index3 = index1 / num4 % length;
            double num5 = (double) numArray2[index3] * (double) num2 + (double) num1 + num3;
            numArray1[index1 * this.Dimensions + index2] = (float) num5;
            if (flag)
              num3 = num5;
            num4 *= length;
          }
        }
      }
      else
      {
        for (int index1 = 0; index1 < this.Entries; ++index1)
        {
          double num3 = 0.0;
          int index2 = index1 * this.Dimensions;
          for (int index3 = 0; index3 < this.Dimensions; ++index3)
          {
            double num4 = (double) numArray2[index2] * (double) num2 + (double) num1 + num3;
            numArray1[index1 * this.Dimensions + index3] = (float) num4;
            if (flag)
              num3 = num4;
            ++index2;
          }
        }
      }
      this.LookupTable = numArray1;
    }

    private int lookup1_values()
    {
      int num = (int) Math.Floor(Math.Exp(Math.Log((double) this.Entries) / (double) this.Dimensions));
      if (Math.Floor(Math.Pow((double) (num + 1), (double) this.Dimensions)) <= (double) this.Entries)
        ++num;
      return num;
    }

    internal int DecodeScalar(DataPacket packet)
    {
      int bitsRead;
      int num = (int) packet.TryPeekBits(this.MaxBits, out bitsRead);
      if (bitsRead == 0)
        throw new InvalidDataException();
      for (HuffmanListNode<int> huffmanListNode = this.LTree; huffmanListNode != null; huffmanListNode = huffmanListNode.Next)
      {
        if (huffmanListNode.Bits == (num & huffmanListNode.Mask))
        {
          ++huffmanListNode.HitCount;
          packet.SkipBits(huffmanListNode.Length);
          return huffmanListNode.Value;
        }
      }
      throw new InvalidDataException();
    }
  }
}
