// Type: Microsoft.Xna.Framework.Audio.MSADPCMToPCM
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System;
using System.IO;

namespace Microsoft.Xna.Framework.Audio
{
  internal class MSADPCMToPCM
  {
    private static readonly int[] AdaptionTable = new int[16]
    {
      230,
      230,
      230,
      230,
      307,
      409,
      512,
      614,
      768,
      614,
      512,
      409,
      307,
      230,
      230,
      230
    };
    private static readonly int[] AdaptCoeff_1 = new int[7]
    {
      256,
      512,
      0,
      192,
      240,
      460,
      392
    };
    private static readonly int[] AdaptCoeff_2 = new int[7]
    {
      0,
      -256,
      0,
      64,
      0,
      -208,
      -232
    };

    static MSADPCMToPCM()
    {
    }

    private static void getNibbleBlock(byte block, byte[] nibbleBlock)
    {
      nibbleBlock[0] = (byte) ((uint) block >> 4);
      nibbleBlock[1] = (byte) ((uint) block & 15U);
    }

    private static short calculateSample(byte nibble, byte predictor, ref short sample_1, ref short sample_2, ref short delta)
    {
      sbyte num1 = (sbyte) nibble;
      if (((int) num1 & 8) == 8)
        num1 -= (sbyte) 16;
      int num2 = ((int) sample_1 * MSADPCMToPCM.AdaptCoeff_1[(int) predictor] + (int) sample_2 * MSADPCMToPCM.AdaptCoeff_2[(int) predictor]) / 256 + (int) num1 * (int) delta;
      short num3 = num2 >= (int) short.MinValue ? (num2 <= (int) short.MaxValue ? (short) num2 : short.MaxValue) : short.MinValue;
      sample_2 = sample_1;
      sample_1 = num3;
      delta = (short) (MSADPCMToPCM.AdaptionTable[(int) nibble] * (int) delta / 256);
      if ((int) delta < 16)
        delta = (short) 16;
      return num3;
    }

    public static byte[] MSADPCM_TO_PCM(BinaryReader Source, short numChannels, short blockAlign)
    {
      MemoryStream memoryStream = new MemoryStream();
      BinaryWriter binaryWriter = new BinaryWriter((Stream) memoryStream);
      byte[] nibbleBlock = new byte[2];
      long num = Source.BaseStream.Length - (long) blockAlign;
      if ((int) numChannels == 1)
      {
        while (Source.BaseStream.Position <= num)
        {
          byte predictor = Source.ReadByte();
          short delta = Source.ReadInt16();
          short sample_1 = Source.ReadInt16();
          short sample_2 = Source.ReadInt16();
          binaryWriter.Write(sample_2);
          binaryWriter.Write(sample_1);
          for (int index1 = 0; index1 < (int) blockAlign + 15; ++index1)
          {
            MSADPCMToPCM.getNibbleBlock(Source.ReadByte(), nibbleBlock);
            for (int index2 = 0; index2 < 2; ++index2)
              binaryWriter.Write(MSADPCMToPCM.calculateSample(nibbleBlock[index2], predictor, ref sample_1, ref sample_2, ref delta));
          }
        }
      }
      else if ((int) numChannels == 2)
      {
        while (Source.BaseStream.Position <= num)
        {
          byte predictor1 = Source.ReadByte();
          byte predictor2 = Source.ReadByte();
          short delta1 = Source.ReadInt16();
          short delta2 = Source.ReadInt16();
          short sample_1_1 = Source.ReadInt16();
          short sample_1_2 = Source.ReadInt16();
          short sample_2_1 = Source.ReadInt16();
          short sample_2_2 = Source.ReadInt16();
          binaryWriter.Write(sample_2_1);
          binaryWriter.Write(sample_2_2);
          binaryWriter.Write(sample_1_1);
          binaryWriter.Write(sample_1_2);
          for (int index = 0; index < ((int) blockAlign + 15) * 2; ++index)
          {
            MSADPCMToPCM.getNibbleBlock(Source.ReadByte(), nibbleBlock);
            binaryWriter.Write(MSADPCMToPCM.calculateSample(nibbleBlock[0], predictor1, ref sample_1_1, ref sample_2_1, ref delta1));
            binaryWriter.Write(MSADPCMToPCM.calculateSample(nibbleBlock[1], predictor2, ref sample_1_2, ref sample_2_2, ref delta2));
          }
        }
      }
      else
      {
        Console.WriteLine("MSADPCM WAVEDATA IS NOT MONO OR STEREO!");
        binaryWriter.Close();
        memoryStream.Close();
        return (byte[]) null;
      }
      binaryWriter.Close();
      memoryStream.Close();
      return memoryStream.ToArray();
    }
  }
}
