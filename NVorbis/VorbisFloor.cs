// Type: NVorbis.VorbisFloor
// Assembly: NVorbis, Version=0.5.5.0, Culture=neutral, PublicKeyToken=null
// MVID: CF8FE41E-969B-4426-8E05-8E0EFF882245
// Assembly location: F:\Program Files (x86)\FEZ\NVorbis.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NVorbis
{
  internal abstract class VorbisFloor
  {
    private VorbisStreamDecoder _vorbis;

    protected VorbisFloor(VorbisStreamDecoder vorbis)
    {
      this._vorbis = vorbis;
    }

    internal static VorbisFloor Init(VorbisStreamDecoder vorbis, DataPacket packet)
    {
      int num = (int) packet.ReadBits(16);
      VorbisFloor vorbisFloor = (VorbisFloor) null;
      switch (num)
      {
        case 0:
          vorbisFloor = (VorbisFloor) new VorbisFloor.Floor0(vorbis);
          break;
        case 1:
          vorbisFloor = (VorbisFloor) new VorbisFloor.Floor1(vorbis);
          break;
      }
      if (vorbisFloor == null)
        throw new InvalidDataException();
      vorbisFloor.Init(packet);
      return vorbisFloor;
    }

    protected abstract void Init(DataPacket packet);

    internal abstract VorbisFloor.PacketData UnpackPacket(DataPacket packet, int blockSize);

    internal abstract void Apply(VorbisFloor.PacketData packetData, float[] residue);

    internal abstract class PacketData
    {
      internal int BlockSize;

      protected abstract bool HasEnergy { get; }

      internal bool ForceEnergy { get; set; }

      internal bool ForceNoEnergy { get; set; }

      internal bool ExecuteChannel
      {
        get
        {
          return (this.ForceEnergy | this.HasEnergy) & !this.ForceNoEnergy;
        }
      }
    }

    private class Floor0 : VorbisFloor
    {
      private int _order;
      private int _rate;
      private int _bark_map_size;
      private int _ampBits;
      private int _ampOfs;
      private VorbisCodebook[] _books;
      private int _bookBits;
      private Dictionary<int, float[]> _barkMaps;

      internal Floor0(VorbisStreamDecoder vorbis)
        : base(vorbis)
      {
      }

      protected override void Init(DataPacket packet)
      {
        this._order = (int) packet.ReadBits(8);
        this._rate = (int) packet.ReadBits(16);
        this._bark_map_size = (int) packet.ReadBits(16);
        this._ampBits = (int) packet.ReadBits(6);
        this._ampOfs = (int) packet.ReadBits(8);
        this._books = new VorbisCodebook[(int) packet.ReadBits(4) + 1];
        for (int index = 0; index < this._books.Length; ++index)
          this._books[index] = this._vorbis.Books[(int) packet.ReadBits(8)];
        this._bookBits = Utils.ilog(this._books.Length);
        this._barkMaps = new Dictionary<int, float[]>();
        this._barkMaps[this._vorbis.Block0Size] = this.SynthesizeBarkCurve(this._vorbis.Block0Size);
        this._barkMaps[this._vorbis.Block1Size] = this.SynthesizeBarkCurve(this._vorbis.Block1Size);
      }

      private float[] SynthesizeBarkCurve(int n)
      {
        float[] numArray = new float[n + 1];
        for (int index = 0; index < n - 1; ++index)
        {
          float val2 = VorbisFloor.Floor0.toBARK((double) (this._rate * index / (2 * n))) * ((float) this._bark_map_size / VorbisFloor.Floor0.toBARK(0.5 * (double) this._rate));
          numArray[index] = Math.Min((float) (this._bark_map_size - 1), val2);
        }
        numArray[n] = -1f;
        return numArray;
      }

      private static float toBARK(double lsp)
      {
        return (float) (13.1 * Math.Atan(0.00074 * lsp) + 2.24 * Math.Atan(1.85E-08 * lsp * lsp) + 0.0001 * lsp);
      }

      internal override VorbisFloor.PacketData UnpackPacket(DataPacket packet, int blockSize)
      {
        VorbisFloor.Floor0.PacketData0 packetData0_1 = new VorbisFloor.Floor0.PacketData0();
        packetData0_1.BlockSize = blockSize;
        VorbisFloor.Floor0.PacketData0 packetData0_2 = packetData0_1;
        packetData0_2.Amp = (float) packet.ReadBits(this._ampBits);
        if ((double) packetData0_2.Amp > 0.0)
        {
          try
          {
            List<float> list = new List<float>();
            uint num = (uint) packet.ReadBits(this._bookBits);
            if ((long) num >= (long) this._books.Length)
              throw new InvalidDataException();
            VorbisCodebook vorbisCodebook = this._books[(IntPtr) num];
            for (int index1 = 0; index1 < this._order; ++index1)
            {
              int index2 = vorbisCodebook.DecodeScalar(packet);
              for (int index3 = 0; index3 < vorbisCodebook.Dimensions; ++index3)
                list.Add(vorbisCodebook[index2, index3]);
            }
            packetData0_2.Coeff = list.ToArray();
          }
          catch (EndOfStreamException ex)
          {
            packetData0_2.Amp = 0.0f;
          }
        }
        return (VorbisFloor.PacketData) packetData0_2;
      }

      internal override void Apply(VorbisFloor.PacketData packetData, float[] residue)
      {
        VorbisFloor.Floor0.PacketData0 packetData0 = packetData as VorbisFloor.Floor0.PacketData0;
        if (packetData0 == null)
          throw new ArgumentException("Incorrect packet data!");
        if ((double) packetData0.Amp <= 0.0)
          return;
        float[] numArray = this._barkMaps[packetData0.BlockSize];
        int index = 0;
label_9:
        if (index >= packetData0.BlockSize)
          return;
        double d = Math.PI * (double) numArray[index] / (double) this._bark_map_size;
        float num1;
        int num2;
        float num3;
        int num4;
        if (this._order % 2 == 1)
        {
          num1 = (float) (1.0 - Math.Pow(Math.Cos(d), 2.0));
          num2 = 3;
          num3 = 0.25f;
          num4 = 1;
        }
        else
        {
          num1 = (float) (1.0 - Math.Cos(d));
          num2 = 2;
          num3 = (float) (1.0 + Math.Cos(d));
          num4 = 2;
        }
        double num5 = (double) num1 * (double) (this._order - num2) / (4.0 * Math.Pow(Math.Cos((double) packetData0.Coeff[1]) - Math.Cos(d), 2.0) * (4.0 * Math.Pow(Math.Cos((double) packetData0.Coeff[3]) - Math.Cos(d), 2.0)) * (4.0 * Math.Pow(Math.Cos((double) packetData0.Coeff[5]) - Math.Cos(d), 2.0)));
        double num6 = (double) num3 * (double) (this._order - num4) / (4.0 * Math.Pow(Math.Cos((double) packetData0.Coeff[0]) - Math.Cos(d), 2.0) * (4.0 * Math.Pow(Math.Cos((double) packetData0.Coeff[2]) - Math.Cos(d), 2.0)) * (4.0 * Math.Pow(Math.Cos((double) packetData0.Coeff[4]) - Math.Cos(d), 2.0)));
        float num7 = (float) Math.Exp(0.11512925 * ((double) packetData0.Amp * (double) this._ampOfs / ((double) ((1 << this._ampBits) - 1) * Math.Sqrt(num5 + num6)) - (double) this._ampOfs));
        float num8;
        do
        {
          num8 = numArray[index];
          residue[index] *= num7;
          ++index;
        }
        while ((double) numArray[index] == (double) num8);
        goto label_9;
      }

      private class PacketData0 : VorbisFloor.PacketData
      {
        internal float[] Coeff;
        internal float Amp;

        protected override bool HasEnergy
        {
          get
          {
            return (double) this.Amp > 0.0;
          }
        }
      }
    }

    private class Floor1 : VorbisFloor
    {
      private static int[] _rangeLookup = new int[4]
      {
        256,
        128,
        86,
        64
      };
      private static int[] _yBitsLookup = new int[4]
      {
        8,
        7,
        7,
        6
      };
      private static readonly float[] inverse_dB_table = new float[256]
      {
        1.064986E-07f,
        1.134195E-07f,
        1.207901E-07f,
        1.286398E-07f,
        1.369995E-07f,
        1.459025E-07f,
        1.553841E-07f,
        1.654818E-07f,
        1.762357E-07f,
        1.876886E-07f,
        1.998856E-07f,
        2.128753E-07f,
        2.267091E-07f,
        2.41442E-07f,
        2.571322E-07f,
        2.738421E-07f,
        2.916379E-07f,
        3.105902E-07f,
        3.307741E-07f,
        3.522697E-07f,
        3.751621E-07f,
        3.995423E-07f,
        4.255068E-07f,
        4.531586E-07f,
        4.826074E-07f,
        5.1397E-07f,
        5.473706E-07f,
        5.829419E-07f,
        6.208247E-07f,
        6.611694E-07f,
        7.041359E-07f,
        7.498946E-07f,
        7.98627E-07f,
        8.505263E-07f,
        9.057983E-07f,
        9.646621E-07f,
        1.027351E-06f,
        1.094114E-06f,
        1.165216E-06f,
        1.240938E-06f,
        1.321582E-06f,
        1.407465E-06f,
        1.49893E-06f,
        1.596339E-06f,
        1.700079E-06f,
        1.810559E-06f,
        1.928219E-06f,
        2.053526E-06f,
        2.186976E-06f,
        2.329098E-06f,
        2.480456E-06f,
        2.64165E-06f,
        2.813319E-06f,
        2.996144E-06f,
        3.190851E-06f,
        3.39821E-06f,
        3.619045E-06f,
        3.854231E-06f,
        4.104701E-06f,
        4.371447E-06f,
        4.655528E-06f,
        4.958071E-06f,
        5.280274E-06f,
        5.623416E-06f,
        5.988857E-06f,
        6.378047E-06f,
        6.792528E-06f,
        7.233945E-06f,
        7.704048E-06f,
        8.2047E-06f,
        8.737888E-06f,
        9.305725E-06f,
        9.910464E-06f,
        1.05545E-05f,
        1.124039E-05f,
        1.197086E-05f,
        1.274879E-05f,
        1.357728E-05f,
        1.445961E-05f,
        1.539927E-05f,
        1.64E-05f,
        1.746577E-05f,
        1.860079E-05f,
        1.980958E-05f,
        2.109691E-05f,
        2.246791E-05f,
        2.3928E-05f,
        2.548298E-05f,
        2.713901E-05f,
        2.890265E-05f,
        3.078091E-05f,
        3.278123E-05f,
        3.491153E-05f,
        3.718028E-05f,
        3.959647E-05f,
        4.216967E-05f,
        4.491009E-05f,
        4.78286E-05f,
        5.093677E-05f,
        5.424693E-05f,
        5.77722E-05f,
        6.152657E-05f,
        6.552491E-05f,
        6.978308E-05f,
        7.431798E-05f,
        7.914758E-05f,
        8.429104E-05f,
        8.976875E-05f,
        9.560242E-05f,
        0.0001018152f,
        0.0001084317f,
        0.0001154782f,
        0.0001229827f,
        0.0001309748f,
        0.0001394862f,
        0.0001485509f,
        0.0001582045f,
        0.0001684856f,
        0.0001794347f,
        0.0001910954f,
        0.0002035138f,
        0.0002167393f,
        0.0002308242f,
        0.0002458245f,
        0.0002617995f,
        0.0002788127f,
        0.0002969316f,
        0.0003162279f,
        0.0003367781f,
        0.0003586639f,
        0.0003819719f,
        0.0004067946f,
        0.0004332304f,
        0.0004613841f,
        0.0004913675f,
        0.0005232993f,
        0.0005573062f,
        0.0005935231f,
        0.0006320936f,
        0.0006731706f,
        0.000716917f,
        0.0007635063f,
        0.0008131232f,
        0.0008659646f,
        0.0009222399f,
        0.0009821722f,
        0.001045999f,
        0.001113974f,
        0.001186367f,
        0.001263463f,
        0.00134557f,
        0.001433013f,
        0.001526138f,
        0.001625315f,
        0.001730937f,
        0.001843423f,
        0.00196322f,
        0.002090801f,
        0.002226673f,
        0.002371374f,
        0.00252548f,
        0.002689599f,
        0.002864385f,
        0.003050529f,
        0.003248769f,
        0.003459892f,
        0.003684736f,
        0.003924191f,
        0.004179207f,
        0.004450795f,
        0.004740033f,
        0.005048067f,
        0.005376119f,
        0.005725489f,
        0.006097564f,
        0.006493818f,
        0.006915823f,
        0.007365251f,
        0.007843887f,
        0.008353627f,
        0.008896492f,
        0.009474637f,
        0.01009035f,
        0.01074608f,
        0.01144442f,
        0.01218814f,
        0.0129802f,
        0.01382373f,
        0.01472207f,
        0.01567879f,
        0.01669769f,
        0.0177828f,
        0.01893842f,
        0.02016915f,
        0.02147985f,
        0.02287574f,
        0.02436233f,
        0.02594553f,
        0.02763162f,
        0.02942728f,
        0.03133963f,
        0.03337625f,
        0.03554523f,
        0.03785516f,
        0.0403152f,
        0.04293511f,
        0.04572527f,
        0.04869676f,
        0.05186135f,
        0.05523159f,
        0.05882085f,
        0.06264336f,
        0.06671428f,
        0.07104975f,
        0.07566696f,
        0.08058423f,
        0.08582105f,
        0.09139818f,
        0.09733775f,
        0.1036633f,
        0.1103999f,
        0.1175743f,
        0.125215f,
        0.1333521f,
        0.1420181f,
        0.1512473f,
        0.1610762f,
        0.1715438f,
        0.1826917f,
        0.194564f,
        0.2072079f,
        0.2206734f,
        0.235014f,
        0.2502865f,
        0.2665516f,
        0.2838736f,
        0.3023213f,
        0.3219679f,
        0.3428911f,
        0.3651741f,
        0.3889052f,
        0.4141785f,
        0.4410941f,
        0.4697589f,
        0.5002865f,
        0.5327979f,
        0.5674221f,
        0.6042964f,
        0.643567f,
        0.6853896f,
        0.72993f,
        0.777365f,
        0.8278826f,
        0.8816831f,
        0.9389798f,
        1f
      };
      private int[] _partitionClass;
      private int[] _classDimensions;
      private int[] _classSubclasses;
      private int[] _xList;
      private int[] _classMasterBookIndex;
      private int[] _hNeigh;
      private int[] _lNeigh;
      private int[] _sortIdx;
      private int _multiplier;
      private int _range;
      private int _yBits;
      private VorbisCodebook[] _classMasterbooks;
      private VorbisCodebook[][] _subclassBooks;
      private int[][] _subclassBookIndex;

      static Floor1()
      {
      }

      internal Floor1(VorbisStreamDecoder vorbis)
        : base(vorbis)
      {
      }

      protected override void Init(DataPacket packet)
      {
        this._partitionClass = new int[(int) packet.ReadBits(5)];
        for (int index = 0; index < this._partitionClass.Length; ++index)
          this._partitionClass[index] = (int) packet.ReadBits(4);
        int num1 = Enumerable.Max((IEnumerable<int>) this._partitionClass);
        this._classDimensions = new int[num1 + 1];
        this._classSubclasses = new int[num1 + 1];
        this._classMasterbooks = new VorbisCodebook[num1 + 1];
        this._classMasterBookIndex = new int[num1 + 1];
        this._subclassBooks = new VorbisCodebook[num1 + 1][];
        this._subclassBookIndex = new int[num1 + 1][];
        for (int index1 = 0; index1 <= num1; ++index1)
        {
          this._classDimensions[index1] = (int) packet.ReadBits(3) + 1;
          this._classSubclasses[index1] = (int) packet.ReadBits(2);
          if (this._classSubclasses[index1] > 0)
          {
            this._classMasterBookIndex[index1] = (int) packet.ReadBits(8);
            this._classMasterbooks[index1] = this._vorbis.Books[this._classMasterBookIndex[index1]];
          }
          this._subclassBooks[index1] = new VorbisCodebook[1 << this._classSubclasses[index1]];
          this._subclassBookIndex[index1] = new int[this._subclassBooks[index1].Length];
          for (int index2 = 0; index2 < this._subclassBooks[index1].Length; ++index2)
          {
            int index3 = (int) packet.ReadBits(8) - 1;
            if (index3 >= 0)
              this._subclassBooks[index1][index2] = this._vorbis.Books[index3];
            this._subclassBookIndex[index1][index2] = index3;
          }
        }
        this._multiplier = (int) packet.ReadBits(2);
        this._range = VorbisFloor.Floor1._rangeLookup[this._multiplier];
        this._yBits = VorbisFloor.Floor1._yBitsLookup[this._multiplier];
        ++this._multiplier;
        int count = (int) packet.ReadBits(4);
        List<int> list = new List<int>();
        list.Add(0);
        list.Add(1 << count);
        for (int index1 = 0; index1 < this._partitionClass.Length; ++index1)
        {
          int index2 = this._partitionClass[index1];
          for (int index3 = 0; index3 < this._classDimensions[index2]; ++index3)
            list.Add((int) packet.ReadBits(count));
        }
        this._xList = list.ToArray();
        this._lNeigh = new int[list.Count];
        this._hNeigh = new int[list.Count];
        this._sortIdx = new int[list.Count];
        this._sortIdx[0] = 0;
        this._sortIdx[1] = 1;
        for (int index1 = 2; index1 < this._lNeigh.Length; ++index1)
        {
          this._lNeigh[index1] = 0;
          this._hNeigh[index1] = 1;
          this._sortIdx[index1] = index1;
          for (int index2 = 2; index2 < index1; ++index2)
          {
            int num2 = this._xList[index2];
            if (num2 < this._xList[index1])
            {
              if (num2 > this._xList[this._lNeigh[index1]])
                this._lNeigh[index1] = index2;
            }
            else if (num2 < this._xList[this._hNeigh[index1]])
              this._hNeigh[index1] = index2;
          }
        }
        for (int index1 = 0; index1 < this._sortIdx.Length - 1; ++index1)
        {
          for (int index2 = index1 + 1; index2 < this._sortIdx.Length; ++index2)
          {
            if (this._xList[index1] == this._xList[index2])
              throw new InvalidDataException();
            if (this._xList[this._sortIdx[index1]] > this._xList[this._sortIdx[index2]])
            {
              int num2 = this._sortIdx[index1];
              this._sortIdx[index1] = this._sortIdx[index2];
              this._sortIdx[index2] = num2;
            }
          }
        }
      }

      internal override VorbisFloor.PacketData UnpackPacket(DataPacket packet, int blockSize)
      {
        VorbisFloor.Floor1.PacketData1 packetData1_1 = new VorbisFloor.Floor1.PacketData1();
        packetData1_1.BlockSize = blockSize;
        VorbisFloor.Floor1.PacketData1 packetData1_2 = packetData1_1;
        if (packet.ReadBit())
        {
          try
          {
            int index1 = 2;
            int[] numArray = ACache.Get<int>(64);
            numArray[0] = (int) packet.ReadBits(this._yBits);
            numArray[1] = (int) packet.ReadBits(this._yBits);
            for (int index2 = 0; index2 < this._partitionClass.Length; ++index2)
            {
              int index3 = this._partitionClass[index2];
              int num1 = this._classDimensions[index3];
              int num2 = this._classSubclasses[index3];
              int num3 = (1 << num2) - 1;
              uint num4 = 0U;
              if (num2 > 0)
                num4 = (uint) this._classMasterbooks[index3].DecodeScalar(packet);
              for (int index4 = 0; index4 < num1; ++index4)
              {
                VorbisCodebook vorbisCodebook = this._subclassBooks[index3][(long) num4 & (long) num3];
                num4 >>= num2;
                if (vorbisCodebook != null)
                  numArray[index1] = vorbisCodebook.DecodeScalar(packet);
                ++index1;
              }
            }
            packetData1_2.Posts = numArray;
            packetData1_2.PostCount = index1;
          }
          catch (EndOfStreamException ex)
          {
          }
        }
        return (VorbisFloor.PacketData) packetData1_2;
      }

      internal override void Apply(VorbisFloor.PacketData packetData, float[] residue)
      {
        VorbisFloor.Floor1.PacketData1 data = packetData as VorbisFloor.Floor1.PacketData1;
        if (data == null)
          throw new InvalidDataException("Incorrect packet data!");
        if (data.Posts == null)
          return;
        bool[] buffer = this.UnwrapPosts(data);
        int num1 = data.BlockSize / 2;
        int x0 = 0;
        int num2 = data.Posts[0] * this._multiplier;
        for (int index1 = 1; index1 < data.PostCount; ++index1)
        {
          int index2 = this._sortIdx[index1];
          if (buffer[index2])
          {
            int val1 = this._xList[index2];
            int y1 = data.Posts[index2] * this._multiplier;
            if (x0 < num1)
              this.RenderLineMulti(x0, num2, Math.Min(val1, num1), y1, residue);
            x0 = val1;
            num2 = y1;
          }
          if (x0 >= num1)
            break;
        }
        ACache.Return<bool>(ref buffer);
        if (x0 < num1)
          this.RenderLineMulti(x0, num2, num1, num2, residue);
        ACache.Return<int>(ref data.Posts);
      }

      private bool[] UnwrapPosts(VorbisFloor.Floor1.PacketData1 data)
      {
        bool[] flagArray = ACache.Get<bool>(data.PostCount, false);
        flagArray[0] = true;
        flagArray[1] = true;
        int[] buffer = ACache.Get<int>(data.PostCount);
        buffer[0] = data.Posts[0];
        buffer[1] = data.Posts[1];
        for (int index1 = 2; index1 < data.PostCount; ++index1)
        {
          int index2 = this._lNeigh[index1];
          int index3 = this._hNeigh[index1];
          int num1 = this.RenderPoint(this._xList[index2], buffer[index2], this._xList[index3], buffer[index3], this._xList[index1]);
          int num2 = data.Posts[index1];
          int num3 = this._range - num1;
          int num4 = num1;
          int num5 = num3 >= num4 ? num4 * 2 : num3 * 2;
          if (num2 != 0)
          {
            flagArray[index2] = true;
            flagArray[index3] = true;
            flagArray[index1] = true;
            buffer[index1] = num2 < num5 ? (num2 % 2 != 1 ? num1 + num2 / 2 : num1 - (num2 + 1) / 2) : (num3 <= num4 ? num1 - num2 + num3 - 1 : num2 - num4 + num1);
          }
          else
          {
            flagArray[index1] = false;
            buffer[index1] = num1;
          }
        }
        for (int index = 0; index < data.PostCount; ++index)
          data.Posts[index] = buffer[index];
        ACache.Return<int>(ref buffer);
        return flagArray;
      }

      private int RenderPoint(int x0, int y0, int x1, int y1, int X)
      {
        int num1 = y1 - y0;
        int num2 = x1 - x0;
        int num3 = Math.Abs(num1) * (X - x0) / num2;
        if (num1 < 0)
          return y0 - num3;
        else
          return y0 + num3;
      }

      private void RenderLineMulti(int x0, int y0, int x1, int y1, float[] v)
      {
        int num1 = y1 - y0;
        int num2 = x1 - x0;
        int num3 = Math.Abs(num1);
        int num4 = 1 - (num1 >> 31 & 1) * 2;
        int num5 = num1 / num2;
        int index1 = x0;
        int index2 = y0;
        int num6 = -num2;
        v[x0] *= VorbisFloor.Floor1.inverse_dB_table[y0];
        int num7 = num3 - Math.Abs(num5) * num2;
        while (++index1 < x1)
        {
          index2 += num5;
          num6 += num7;
          if (num6 >= 0)
          {
            num6 -= num2;
            index2 += num4;
          }
          v[index1] *= VorbisFloor.Floor1.inverse_dB_table[index2];
        }
      }

      private class PacketData1 : VorbisFloor.PacketData
      {
        public int[] Posts;
        public int PostCount;

        protected override bool HasEnergy
        {
          get
          {
            return this.Posts != null;
          }
        }
      }
    }
  }
}
