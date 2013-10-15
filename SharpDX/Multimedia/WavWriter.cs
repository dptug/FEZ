// Type: SharpDX.Multimedia.WavWriter
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX;
using SharpDX.Serialization;
using System;
using System.Collections.Generic;
using System.IO;

namespace SharpDX.Multimedia
{
  public class WavWriter
  {
    private readonly BinarySerializer serializer;
    private bool isBegin;

    public WavWriter(Stream outputStream)
    {
      this.serializer = new BinarySerializer(outputStream, SerializerMode.Write);
    }

    public void Begin(WaveFormat waveFormat)
    {
      if (this.isBegin)
        throw new InvalidOperationException("Cannot begin a new WAV while another begin has not been closed");
      this.serializer.BeginChunk((FourCC) "RIFF");
      FourCC fourCc = new FourCC("WAVE");
      this.serializer.Serialize<FourCC>(ref fourCc, SerializeFlags.Normal);
      this.serializer.BeginChunk((FourCC) "fmt ");
      this.serializer.Serialize<WaveFormat>(ref waveFormat, SerializeFlags.Normal);
      this.serializer.EndChunk();
      this.serializer.BeginChunk((FourCC) "data");
      this.isBegin = true;
    }

    public void AppendData(IEnumerable<DataPointer> dataPointers)
    {
      this.CheckBegin();
      foreach (DataPointer dataPointer in dataPointers)
        this.AppendData(dataPointer);
    }

    public void AppendData<T>(IEnumerable<T[]> dataBuffers) where T : struct
    {
      this.CheckBegin();
      foreach (T[] buffer in dataBuffers)
        this.AppendData<T>(buffer);
    }

    public void AppendData(DataPointer dataPointer)
    {
      this.CheckBegin();
      this.serializer.SerializeMemoryRegion(dataPointer);
    }

    public unsafe void AppendData<T>(T[] buffer) where T : struct
    {
      this.CheckBegin();
      WavWriter wavWriter = this;
      fixed (T* objPtr = &buffer[0])
      {
        DataPointer dataPointer = new DataPointer((IntPtr) ((void*) objPtr), Utilities.SizeOf<T>(buffer));
        wavWriter.AppendData(dataPointer);
      }
    }

    public void End()
    {
      this.CheckBegin();
      this.serializer.EndChunk();
      this.serializer.EndChunk();
      this.isBegin = false;
    }

    private void CheckBegin()
    {
      if (!this.isBegin)
        throw new InvalidOperationException("Begin was not called");
    }
  }
}
