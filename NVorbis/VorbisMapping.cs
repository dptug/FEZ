// Type: NVorbis.VorbisMapping
// Assembly: NVorbis, Version=0.5.5.0, Culture=neutral, PublicKeyToken=null
// MVID: CF8FE41E-969B-4426-8E05-8E0EFF882245
// Assembly location: F:\Program Files (x86)\FEZ\NVorbis.dll

using System.IO;

namespace NVorbis
{
  internal abstract class VorbisMapping
  {
    private VorbisStreamDecoder _vorbis;
    internal VorbisMapping.Submap[] Submaps;
    internal VorbisMapping.Submap[] ChannelSubmap;
    internal VorbisMapping.CouplingStep[] CouplingSteps;

    protected VorbisMapping(VorbisStreamDecoder vorbis)
    {
      this._vorbis = vorbis;
    }

    internal static VorbisMapping Init(VorbisStreamDecoder vorbis, DataPacket packet)
    {
      int num = (int) packet.ReadBits(16);
      VorbisMapping vorbisMapping = (VorbisMapping) null;
      if (num == 0)
        vorbisMapping = (VorbisMapping) new VorbisMapping.Mapping0(vorbis);
      if (vorbisMapping == null)
        throw new InvalidDataException();
      vorbisMapping.Init(packet);
      return vorbisMapping;
    }

    protected abstract void Init(DataPacket packet);

    private class Mapping0 : VorbisMapping
    {
      internal Mapping0(VorbisStreamDecoder vorbis)
        : base(vorbis)
      {
      }

      protected override void Init(DataPacket packet)
      {
        int length1 = 1;
        if (packet.ReadBit())
          length1 += (int) packet.ReadBits(4);
        int length2 = 0;
        if (packet.ReadBit())
          length2 = (int) packet.ReadBits(8) + 1;
        int count = Utils.ilog(this._vorbis._channels - 1);
        this.CouplingSteps = new VorbisMapping.CouplingStep[length2];
        for (int index = 0; index < length2; ++index)
        {
          int num1 = (int) packet.ReadBits(count);
          int num2 = (int) packet.ReadBits(count);
          if (num1 == num2 || num1 > this._vorbis._channels - 1 || num2 > this._vorbis._channels - 1)
            throw new InvalidDataException();
          this.CouplingSteps[index] = new VorbisMapping.CouplingStep()
          {
            Angle = num2,
            Magnitude = num1
          };
        }
        if ((long) packet.ReadBits(2) != 0L)
          throw new InvalidDataException();
        int[] numArray = new int[this._vorbis._channels];
        if (length1 > 1)
        {
          for (int index = 0; index < this.ChannelSubmap.Length; ++index)
          {
            numArray[index] = (int) packet.ReadBits(4);
            if (numArray[index] >= length1)
              throw new InvalidDataException();
          }
        }
        this.Submaps = new VorbisMapping.Submap[length1];
        for (int index1 = 0; index1 < length1; ++index1)
        {
          long num = (long) packet.ReadBits(8);
          int index2 = (int) packet.ReadBits(8);
          if (index2 >= this._vorbis.Floors.Length)
            throw new InvalidDataException();
          if ((int) packet.ReadBits(8) >= this._vorbis.Residues.Length)
            throw new InvalidDataException();
          this.Submaps[index1] = new VorbisMapping.Submap()
          {
            Floor = this._vorbis.Floors[index2],
            Residue = this._vorbis.Residues[index2]
          };
        }
        this.ChannelSubmap = new VorbisMapping.Submap[this._vorbis._channels];
        for (int index = 0; index < this.ChannelSubmap.Length; ++index)
          this.ChannelSubmap[index] = this.Submaps[numArray[index]];
      }
    }

    internal class Submap
    {
      internal VorbisFloor Floor;
      internal VorbisResidue Residue;

      internal Submap()
      {
      }
    }

    internal class CouplingStep
    {
      internal int Magnitude;
      internal int Angle;

      internal CouplingStep()
      {
      }
    }
  }
}
