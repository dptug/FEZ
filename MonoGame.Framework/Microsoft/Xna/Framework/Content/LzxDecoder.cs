// Type: Microsoft.Xna.Framework.Content.LzxDecoder
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System;
using System.IO;

namespace Microsoft.Xna.Framework.Content
{
  public class LzxDecoder
  {
    public static uint[] position_base;
    public static byte[] extra_bits;
    private LzxDecoder.LzxState m_state;

    static LzxDecoder()
    {
    }

    public LzxDecoder(int window)
    {
      uint num1 = (uint) (1 << window);
      if (window < 15 || window > 21)
        throw new UnsupportedWindowSizeRange();
      this.m_state = new LzxDecoder.LzxState();
      this.m_state.actual_size = 0U;
      this.m_state.window = new byte[(IntPtr) num1];
      for (int index = 0; (long) index < (long) num1; ++index)
        this.m_state.window[index] = (byte) 220;
      this.m_state.actual_size = num1;
      this.m_state.window_size = num1;
      this.m_state.window_posn = 0U;
      if (LzxDecoder.extra_bits == null)
      {
        LzxDecoder.extra_bits = new byte[52];
        int index = 0;
        int num2 = 0;
        while (index <= 50)
        {
          LzxDecoder.extra_bits[index] = LzxDecoder.extra_bits[index + 1] = (byte) num2;
          if (index != 0 && num2 < 17)
            ++num2;
          index += 2;
        }
      }
      if (LzxDecoder.position_base == null)
      {
        LzxDecoder.position_base = new uint[51];
        int index = 0;
        int num2 = 0;
        for (; index <= 50; ++index)
        {
          LzxDecoder.position_base[index] = (uint) num2;
          num2 += 1 << (int) LzxDecoder.extra_bits[index];
        }
      }
      int num3 = window != 20 ? (window != 21 ? window << 1 : 50) : 42;
      this.m_state.R0 = this.m_state.R1 = this.m_state.R2 = 1U;
      this.m_state.main_elements = (ushort) (256 + (num3 << 3));
      this.m_state.header_read = 0;
      this.m_state.frames_read = 0U;
      this.m_state.block_remaining = 0U;
      this.m_state.block_type = LzxConstants.BLOCKTYPE.INVALID;
      this.m_state.intel_curpos = 0;
      this.m_state.intel_started = 0;
      this.m_state.PRETREE_table = new ushort[104];
      this.m_state.PRETREE_len = new byte[84];
      this.m_state.MAINTREE_table = new ushort[5408];
      this.m_state.MAINTREE_len = new byte[720];
      this.m_state.LENGTH_table = new ushort[4596];
      this.m_state.LENGTH_len = new byte[314];
      this.m_state.ALIGNED_table = new ushort[144];
      this.m_state.ALIGNED_len = new byte[72];
      for (int index = 0; index < 656; ++index)
        this.m_state.MAINTREE_len[index] = (byte) 0;
      for (int index = 0; index < 250; ++index)
        this.m_state.LENGTH_len[index] = (byte) 0;
    }

    public int Decompress(Stream inData, int inLen, Stream outData, int outLen)
    {
      LzxDecoder.BitBuffer bitbuf = new LzxDecoder.BitBuffer(inData);
      long position = inData.Position;
      long num1 = inData.Position + (long) inLen;
      byte[] buffer1 = this.m_state.window;
      uint num2 = this.m_state.window_posn;
      uint num3 = this.m_state.window_size;
      uint num4 = this.m_state.R0;
      uint num5 = this.m_state.R1;
      uint num6 = this.m_state.R2;
      int num7 = outLen;
      bitbuf.InitBitStream();
      if (this.m_state.header_read == 0)
      {
        if ((int) bitbuf.ReadBits((byte) 1) != 0)
          this.m_state.intel_filesize = (int) bitbuf.ReadBits((byte) 16) << 16 | (int) bitbuf.ReadBits((byte) 16);
        this.m_state.header_read = 1;
      }
label_87:
      while (num7 > 0)
      {
        if ((int) this.m_state.block_remaining == 0)
        {
          if (this.m_state.block_type == LzxConstants.BLOCKTYPE.UNCOMPRESSED)
          {
            if (((int) this.m_state.block_length & 1) == 1)
              inData.ReadByte();
            bitbuf.InitBitStream();
          }
          this.m_state.block_type = (LzxConstants.BLOCKTYPE) bitbuf.ReadBits((byte) 3);
          this.m_state.block_remaining = this.m_state.block_length = bitbuf.ReadBits((byte) 16) << 8 | bitbuf.ReadBits((byte) 8);
          switch (this.m_state.block_type)
          {
            case LzxConstants.BLOCKTYPE.VERBATIM:
              this.ReadLengths(this.m_state.MAINTREE_len, 0U, 256U, bitbuf);
              this.ReadLengths(this.m_state.MAINTREE_len, 256U, (uint) this.m_state.main_elements, bitbuf);
              this.MakeDecodeTable(656U, 12U, this.m_state.MAINTREE_len, this.m_state.MAINTREE_table);
              if ((int) this.m_state.MAINTREE_len[232] != 0)
                this.m_state.intel_started = 1;
              this.ReadLengths(this.m_state.LENGTH_len, 0U, 249U, bitbuf);
              this.MakeDecodeTable(250U, 12U, this.m_state.LENGTH_len, this.m_state.LENGTH_table);
              break;
            case LzxConstants.BLOCKTYPE.ALIGNED:
              uint num8 = 0U;
              for (; num8 < 8U; ++num8)
              {
                uint num9 = bitbuf.ReadBits((byte) 3);
                this.m_state.ALIGNED_len[(IntPtr) num8] = (byte) num9;
              }
              this.MakeDecodeTable(8U, 7U, this.m_state.ALIGNED_len, this.m_state.ALIGNED_table);
              goto case LzxConstants.BLOCKTYPE.VERBATIM;
            case LzxConstants.BLOCKTYPE.UNCOMPRESSED:
              this.m_state.intel_started = 1;
              bitbuf.EnsureBits((byte) 16);
              if ((int) bitbuf.GetBitsLeft() > 16)
                inData.Seek(-2L, SeekOrigin.Current);
              num4 = (uint) ((int) (byte) inData.ReadByte() | (int) (byte) inData.ReadByte() << 8 | (int) (byte) inData.ReadByte() << 16 | (int) (byte) inData.ReadByte() << 24);
              num5 = (uint) ((int) (byte) inData.ReadByte() | (int) (byte) inData.ReadByte() << 8 | (int) (byte) inData.ReadByte() << 16 | (int) (byte) inData.ReadByte() << 24);
              num6 = (uint) ((int) (byte) inData.ReadByte() | (int) (byte) inData.ReadByte() << 8 | (int) (byte) inData.ReadByte() << 16 | (int) (byte) inData.ReadByte() << 24);
              break;
            default:
              return -1;
          }
        }
        if (inData.Position > position + (long) inLen && (inData.Position > position + (long) inLen + 2L || (int) bitbuf.GetBitsLeft() < 16))
          return -1;
label_86:
        int count;
        while (true)
        {
          if ((count = (int) this.m_state.block_remaining) > 0 && num7 > 0)
          {
            if (count > num7)
              count = num7;
            num7 -= count;
            this.m_state.block_remaining -= (uint) count;
            num2 &= num3 - 1U;
            if ((long) num2 + (long) count <= (long) num3)
            {
              switch (this.m_state.block_type)
              {
                case LzxConstants.BLOCKTYPE.VERBATIM:
                  goto label_52;
                case LzxConstants.BLOCKTYPE.ALIGNED:
                  goto label_81;
                case LzxConstants.BLOCKTYPE.UNCOMPRESSED:
                  if (inData.Position + (long) count <= num1)
                  {
                    byte[] buffer2 = new byte[count];
                    inData.Read(buffer2, 0, count);
                    buffer2.CopyTo((Array) buffer1, (int) num2);
                    num2 += (uint) count;
                    continue;
                  }
                  else
                    goto label_83;
                default:
                  goto label_85;
              }
            }
            else
              break;
          }
          else
            goto label_87;
        }
        return -1;
label_52:
        while (count > 0)
        {
          int num8 = (int) this.ReadHuffSym(this.m_state.MAINTREE_table, this.m_state.MAINTREE_len, 656U, 12U, bitbuf);
          if (num8 < 256)
          {
            buffer1[(IntPtr) num2++] = (byte) num8;
            --count;
          }
          else
          {
            int num9 = num8 - 256;
            int num10 = num9 & 7;
            if (num10 == 7)
            {
              int num11 = (int) this.ReadHuffSym(this.m_state.LENGTH_table, this.m_state.LENGTH_len, 250U, 12U, bitbuf);
              num10 += num11;
            }
            int num12 = num10 + 2;
            int index = num9 >> 3;
            int num13;
            if (index > 2)
            {
              if (index != 3)
              {
                int num11 = (int) LzxDecoder.extra_bits[index];
                int num14 = (int) bitbuf.ReadBits((byte) num11);
                num13 = (int) LzxDecoder.position_base[index] - 2 + num14;
              }
              else
                num13 = 1;
              num6 = num5;
              num5 = num4;
              num4 = (uint) num13;
            }
            else if (index == 0)
              num13 = (int) num4;
            else if (index == 1)
            {
              num13 = (int) num5;
              num5 = num4;
              num4 = (uint) num13;
            }
            else
            {
              num13 = (int) num6;
              num6 = num4;
              num4 = (uint) num13;
            }
            int num15 = (int) num2;
            count -= num12;
            int num16;
            if ((long) num2 >= (long) num13)
            {
              num16 = num15 - num13;
            }
            else
            {
              num16 = num15 + ((int) num3 - num13);
              int num11 = num13 - (int) num2;
              if (num11 < num12)
              {
                num12 -= num11;
                num2 += (uint) num11;
                while (num11-- > 0)
                  buffer1[num15++] = buffer1[num16++];
                num16 = 0;
              }
            }
            num2 += (uint) num12;
            while (num12-- > 0)
              buffer1[num15++] = buffer1[num16++];
          }
        }
        goto label_86;
label_81:
        while (count > 0)
        {
          int num8 = (int) this.ReadHuffSym(this.m_state.MAINTREE_table, this.m_state.MAINTREE_len, 656U, 12U, bitbuf);
          if (num8 < 256)
          {
            buffer1[(IntPtr) num2++] = (byte) num8;
            --count;
          }
          else
          {
            int num9 = num8 - 256;
            int num10 = num9 & 7;
            if (num10 == 7)
            {
              int num11 = (int) this.ReadHuffSym(this.m_state.LENGTH_table, this.m_state.LENGTH_len, 250U, 12U, bitbuf);
              num10 += num11;
            }
            int num12 = num10 + 2;
            int index = num9 >> 3;
            int num13;
            if (index > 2)
            {
              int num11 = (int) LzxDecoder.extra_bits[index];
              int num14 = (int) LzxDecoder.position_base[index] - 2;
              if (num11 > 3)
              {
                int num15 = num11 - 3;
                int num16 = (int) bitbuf.ReadBits((byte) num15);
                num13 = num14 + (num16 << 3) + (int) this.ReadHuffSym(this.m_state.ALIGNED_table, this.m_state.ALIGNED_len, 8U, 7U, bitbuf);
              }
              else if (num11 == 3)
              {
                int num15 = (int) this.ReadHuffSym(this.m_state.ALIGNED_table, this.m_state.ALIGNED_len, 8U, 7U, bitbuf);
                num13 = num14 + num15;
              }
              else if (num11 > 0)
              {
                int num15 = (int) bitbuf.ReadBits((byte) num11);
                num13 = num14 + num15;
              }
              else
                num13 = 1;
              num6 = num5;
              num5 = num4;
              num4 = (uint) num13;
            }
            else if (index == 0)
              num13 = (int) num4;
            else if (index == 1)
            {
              num13 = (int) num5;
              num5 = num4;
              num4 = (uint) num13;
            }
            else
            {
              num13 = (int) num6;
              num6 = num4;
              num4 = (uint) num13;
            }
            int num17 = (int) num2;
            count -= num12;
            int num18;
            if ((long) num2 >= (long) num13)
            {
              num18 = num17 - num13;
            }
            else
            {
              num18 = num17 + ((int) num3 - num13);
              int num11 = num13 - (int) num2;
              if (num11 < num12)
              {
                num12 -= num11;
                num2 += (uint) num11;
                while (num11-- > 0)
                  buffer1[num17++] = buffer1[num18++];
                num18 = 0;
              }
            }
            num2 += (uint) num12;
            while (num12-- > 0)
              buffer1[num17++] = buffer1[num18++];
          }
        }
        goto label_86;
label_83:
        return -1;
label_85:
        return -1;
      }
      if (num7 != 0)
        return -1;
      int num19 = (int) num2;
      if (num19 == 0)
        num19 = (int) num3;
      int offset = num19 - outLen;
      outData.Write(buffer1, offset, outLen);
      this.m_state.window_posn = num2;
      this.m_state.R0 = num4;
      this.m_state.R1 = num5;
      this.m_state.R2 = num6;
      if (this.m_state.frames_read++ >= 32768U || this.m_state.intel_filesize == 0)
        return 0;
      if (outLen <= 6 || this.m_state.intel_started == 0)
      {
        this.m_state.intel_curpos += outLen;
      }
      else
      {
        int num8 = outLen - 10;
        uint num9 = (uint) this.m_state.intel_curpos;
        this.m_state.intel_curpos = (int) num9 + outLen;
        while (outData.Position < (long) num8)
        {
          if (outData.ReadByte() != 232)
            ++num9;
        }
      }
      return -1;
    }

    private int MakeDecodeTable(uint nsyms, uint nbits, byte[] length, ushort[] table)
    {
      byte num1 = (byte) 1;
      uint num2 = 0U;
      uint num3 = 1U << (int) nbits;
      uint num4 = num3 >> 1;
      uint num5 = num4;
      for (; (uint) num1 <= nbits; ++num1)
      {
        for (ushort index = (ushort) 0; (uint) index < nsyms; ++index)
        {
          if ((int) length[(int) index] == (int) num1)
          {
            uint num6 = num2;
            if ((num2 += num4) > num3)
              return 1;
            uint num7 = num4;
            while (num7-- > 0U)
              table[(IntPtr) num6++] = index;
          }
        }
        num4 >>= 1;
      }
      if ((int) num2 != (int) num3)
      {
        for (ushort index = (ushort) num2; (uint) index < num3; ++index)
          table[(int) index] = (ushort) 0;
        num2 <<= 16;
        num3 <<= 16;
        uint num6 = 32768U;
        for (; (int) num1 <= 16; ++num1)
        {
          for (ushort index1 = (ushort) 0; (uint) index1 < nsyms; ++index1)
          {
            if ((int) length[(int) index1] == (int) num1)
            {
              uint num7 = num2 >> 16;
              for (uint index2 = 0U; index2 < (uint) num1 - nbits; ++index2)
              {
                if ((int) table[(IntPtr) num7] == 0)
                {
                  table[(IntPtr) (num5 << 1)] = (ushort) 0;
                  table[(IntPtr) (uint) (((int) num5 << 1) + 1)] = (ushort) 0;
                  table[(IntPtr) num7] = (ushort) num5++;
                }
                num7 = (uint) table[(IntPtr) num7] << 1;
                if (((int) (num2 >> 15 - (int) index2) & 1) == 1)
                  ++num7;
              }
              table[(IntPtr) num7] = index1;
              if ((num2 += num6) > num3)
                return 1;
            }
          }
          num6 >>= 1;
        }
      }
      if ((int) num2 == (int) num3)
        return 0;
      for (ushort index = (ushort) 0; (uint) index < nsyms; ++index)
      {
        if ((int) length[(int) index] != 0)
          return 1;
      }
      return 0;
    }

    private void ReadLengths(byte[] lens, uint first, uint last, LzxDecoder.BitBuffer bitbuf)
    {
      for (uint index = 0U; index < 20U; ++index)
      {
        uint num = bitbuf.ReadBits((byte) 4);
        this.m_state.PRETREE_len[(IntPtr) index] = (byte) num;
      }
      this.MakeDecodeTable(20U, 6U, this.m_state.PRETREE_len, this.m_state.PRETREE_table);
      uint num1 = first;
      while (num1 < last)
      {
        int num2 = (int) this.ReadHuffSym(this.m_state.PRETREE_table, this.m_state.PRETREE_len, 20U, 6U, bitbuf);
        switch (num2)
        {
          case 17:
            uint num3 = bitbuf.ReadBits((byte) 4) + 4U;
            while ((int) num3-- != 0)
              lens[(IntPtr) num1++] = (byte) 0;
            continue;
          case 18:
            uint num4 = bitbuf.ReadBits((byte) 5) + 20U;
            while ((int) num4-- != 0)
              lens[(IntPtr) num1++] = (byte) 0;
            continue;
          case 19:
            uint num5 = bitbuf.ReadBits((byte) 1) + 4U;
            int num6 = (int) this.ReadHuffSym(this.m_state.PRETREE_table, this.m_state.PRETREE_len, 20U, 6U, bitbuf);
            int num7 = (int) lens[(IntPtr) num1] - num6;
            if (num7 < 0)
              num7 += 17;
            while ((int) num5-- != 0)
              lens[(IntPtr) num1++] = (byte) num7;
            continue;
          default:
            int num8 = (int) lens[(IntPtr) num1] - num2;
            if (num8 < 0)
              num8 += 17;
            lens[(IntPtr) num1++] = (byte) num8;
            continue;
        }
      }
    }

    private uint ReadHuffSym(ushort[] table, byte[] lengths, uint nsyms, uint nbits, LzxDecoder.BitBuffer bitbuf)
    {
      bitbuf.EnsureBits((byte) 16);
      uint num1;
      if ((num1 = (uint) table[(IntPtr) bitbuf.PeekBits((byte) nbits)]) >= nsyms)
      {
        uint num2 = (uint) (1 << 32 - (int) nbits);
        uint num3;
        do
        {
          num2 >>= 1;
          num3 = num1 << 1 | (((int) bitbuf.GetBuffer() & (int) num2) != 0 ? 1U : 0U);
          if ((int) num2 == 0)
            return 0U;
        }
        while ((num1 = (uint) table[(IntPtr) num3]) >= nsyms);
      }
      uint num4 = (uint) lengths[(IntPtr) num1];
      bitbuf.RemoveBits((byte) num4);
      return num1;
    }

    private class BitBuffer
    {
      private uint buffer;
      private byte bitsleft;
      private Stream byteStream;

      public BitBuffer(Stream stream)
      {
        this.byteStream = stream;
        this.InitBitStream();
      }

      public void InitBitStream()
      {
        this.buffer = 0U;
        this.bitsleft = (byte) 0;
      }

      public void EnsureBits(byte bits)
      {
        while ((int) this.bitsleft < (int) bits)
        {
          this.buffer |= (uint) (((int) (byte) this.byteStream.ReadByte() << 8 | (int) (byte) this.byteStream.ReadByte()) << 16 - (int) this.bitsleft);
          this.bitsleft += (byte) 16;
        }
      }

      public uint PeekBits(byte bits)
      {
        return this.buffer >> 32 - (int) bits;
      }

      public void RemoveBits(byte bits)
      {
        this.buffer <<= (int) bits;
        this.bitsleft -= bits;
      }

      public uint ReadBits(byte bits)
      {
        uint num = 0U;
        if ((int) bits > 0)
        {
          this.EnsureBits(bits);
          num = this.PeekBits(bits);
          this.RemoveBits(bits);
        }
        return num;
      }

      public uint GetBuffer()
      {
        return this.buffer;
      }

      public byte GetBitsLeft()
      {
        return this.bitsleft;
      }
    }

    private struct LzxState
    {
      public uint R0;
      public uint R1;
      public uint R2;
      public ushort main_elements;
      public int header_read;
      public LzxConstants.BLOCKTYPE block_type;
      public uint block_length;
      public uint block_remaining;
      public uint frames_read;
      public int intel_filesize;
      public int intel_curpos;
      public int intel_started;
      public ushort[] PRETREE_table;
      public byte[] PRETREE_len;
      public ushort[] MAINTREE_table;
      public byte[] MAINTREE_len;
      public ushort[] LENGTH_table;
      public byte[] LENGTH_len;
      public ushort[] ALIGNED_table;
      public byte[] ALIGNED_len;
      public uint actual_size;
      public byte[] window;
      public uint window_size;
      public uint window_posn;
    }
  }
}
