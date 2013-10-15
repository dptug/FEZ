// Type: SharpDX.Serialization.InvalidChunkException
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX.Multimedia;
using System;

namespace SharpDX.Serialization
{
  public class InvalidChunkException : Exception
  {
    public FourCC ChunkId { get; private set; }

    public FourCC ExpectedChunkId { get; private set; }

    public InvalidChunkException(FourCC chunkId, FourCC expectedChunkId)
      : base(string.Format("Unexpected chunk [{0}/0x{1:X}] instead of [{2}/0x{3:X}]", (object) chunkId, (object) (int) chunkId, (object) expectedChunkId, (object) (int) expectedChunkId))
    {
      this.ChunkId = chunkId;
      this.ExpectedChunkId = expectedChunkId;
    }
  }
}
