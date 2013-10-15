// Type: SharpDX.Multimedia.SoundStream
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX;
using System;
using System.Collections.Generic;
using System.IO;

namespace SharpDX.Multimedia
{
  public class SoundStream : Stream
  {
    private readonly bool isOwnerOfInput;
    private Stream input;
    private long startPositionOfData;
    private long length;

    public uint[] DecodedPacketsInfo { get; private set; }

    public WaveFormat Format { get; protected set; }

    public override bool CanRead
    {
      get
      {
        return true;
      }
    }

    public override bool CanSeek
    {
      get
      {
        return true;
      }
    }

    public override bool CanWrite
    {
      get
      {
        return false;
      }
    }

    public override long Position
    {
      get
      {
        return this.input.Position - this.startPositionOfData;
      }
      set
      {
        this.Seek(value, SeekOrigin.Begin);
      }
    }

    private string FileFormatName { get; set; }

    public override long Length
    {
      get
      {
        return this.length;
      }
    }

    public SoundStream(Stream stream)
    {
      if (stream == null)
        throw new ArgumentNullException("stream");
      this.input = stream;
      this.Initialize(stream);
    }

    public static implicit operator DataStream(SoundStream stream)
    {
      return stream.ToDataStream();
    }

    private void Initialize(Stream stream)
    {
      RiffParser riffParser = new RiffParser(stream);
      this.FileFormatName = "Unknown";
      if (!riffParser.MoveNext() || riffParser.Current == null)
      {
        this.ThrowInvalidFileFormat((Exception) null);
      }
      else
      {
        this.FileFormatName = (string) riffParser.Current.Type;
        if (this.FileFormatName != "WAVE" && this.FileFormatName != "XWMA")
          throw new InvalidOperationException("Unsupported " + this.FileFormatName + " file format. Only WAVE or XWMA");
        riffParser.Descend();
        IList<RiffChunk> allChunks = riffParser.GetAllChunks();
        RiffChunk riffChunk1 = this.Chunk((IEnumerable<RiffChunk>) allChunks, "fmt ");
        if ((long) riffChunk1.Size < (long) sizeof (WaveFormat.__PcmNative))
          this.ThrowInvalidFileFormat((Exception) null);
        try
        {
          this.Format = WaveFormat.MarshalFrom(riffChunk1.GetData());
        }
        catch (InvalidOperationException ex)
        {
          this.ThrowInvalidFileFormat((Exception) ex);
        }
        if (this.FileFormatName == "XWMA")
        {
          if (this.Format.Encoding != WaveFormatEncoding.Wmaudio2 && this.Format.Encoding != WaveFormatEncoding.Wmaudio3)
            this.ThrowInvalidFileFormat((Exception) null);
          this.DecodedPacketsInfo = this.Chunk((IEnumerable<RiffChunk>) allChunks, "dpds").GetDataAsArray<uint>();
        }
        else
        {
          switch (this.Format.Encoding)
          {
            case WaveFormatEncoding.Extensible:
            case WaveFormatEncoding.Pcm:
            case WaveFormatEncoding.Adpcm:
            case WaveFormatEncoding.IeeeFloat:
              break;
            default:
              this.ThrowInvalidFileFormat((Exception) null);
              break;
          }
        }
        RiffChunk riffChunk2 = this.Chunk((IEnumerable<RiffChunk>) allChunks, "data");
        this.startPositionOfData = (long) riffChunk2.DataPosition;
        this.length = (long) riffChunk2.Size;
        this.input.Position = this.startPositionOfData;
      }
    }

    protected void ThrowInvalidFileFormat(Exception nestedException = null)
    {
      throw new InvalidOperationException("Invalid " + this.FileFormatName + " file format", nestedException);
    }

    public DataStream ToDataStream()
    {
      byte[] numArray = new byte[this.Length];
      if ((long) this.Read(numArray, 0, (int) this.Length) != this.Length)
        throw new InvalidOperationException("Unable to get a valid DataStream");
      else
        return DataStream.Create<byte>(numArray, true, true, 0, true);
    }

    protected override void Dispose(bool disposing)
    {
      if (this.input != null)
      {
        this.input.Dispose();
        this.input = (Stream) null;
      }
      base.Dispose(disposing);
    }

    protected RiffChunk Chunk(IEnumerable<RiffChunk> chunks, string id)
    {
      RiffChunk riffChunk1 = (RiffChunk) null;
      foreach (RiffChunk riffChunk2 in chunks)
      {
        if (riffChunk2.Type == (FourCC) id)
        {
          riffChunk1 = riffChunk2;
          break;
        }
      }
      if (riffChunk1 == null || riffChunk1.Type != (FourCC) id)
        throw new InvalidOperationException("Invalid " + this.FileFormatName + " file format");
      else
        return riffChunk1;
    }

    public override void Flush()
    {
      throw new NotSupportedException();
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
      long num = this.input.Position;
      switch (origin)
      {
        case SeekOrigin.Begin:
          num = this.startPositionOfData + offset;
          break;
        case SeekOrigin.Current:
          num = this.input.Position + offset;
          break;
        case SeekOrigin.End:
          num = this.startPositionOfData + this.length + offset;
          break;
      }
      if (num < this.startPositionOfData || num > this.startPositionOfData + this.length)
        throw new InvalidOperationException("Cannot seek outside the range of this stream");
      else
        return this.input.Seek(offset, origin);
    }

    public override void SetLength(long value)
    {
      throw new NotSupportedException();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
      if (this.input.Position + (long) count > this.startPositionOfData + this.length)
        throw new InvalidOperationException("Cannot read more than the length of the stream");
      else
        return this.input.Read(buffer, offset, count);
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
      throw new NotSupportedException();
    }
  }
}
