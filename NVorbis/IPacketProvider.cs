// Type: NVorbis.IPacketProvider
// Assembly: NVorbis, Version=0.5.5.0, Culture=neutral, PublicKeyToken=null
// MVID: CF8FE41E-969B-4426-8E05-8E0EFF882245
// Assembly location: F:\Program Files (x86)\FEZ\NVorbis.dll

using System;

namespace NVorbis
{
  internal interface IPacketProvider : IDisposable
  {
    bool CanSeek { get; }

    long ContainerBits { get; }

    void Init();

    bool FindNextStream(int currentStreamSerial);

    DataPacket GetNextPacket(int streamSerial);

    long GetLastGranulePos(int streamSerial);

    int GetTotalPageCount(int streamSerial);

    int FindPacket(int streamSerial, long granulePos, Func<DataPacket, DataPacket, DataPacket, int> packetGranuleCountCallback);

    void SeekToPacket(int streamSerial, int packetIndex);

    DataPacket GetPacket(int streamSerial, int packetIndex);
  }
}
