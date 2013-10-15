// Type: Microsoft.Xna.Framework.Audio.OALSoundBuffer
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using OpenTK.Audio.OpenAL;
using System;

namespace Microsoft.Xna.Framework.Audio
{
  internal class OALSoundBuffer : IDisposable
  {
    private int openALDataBuffer;
    internal byte[] pcmDataBuffer;
    private ALFormat openALFormat;
    private int dataSize;
    private int sampleRate;
    private int _sourceId;

    public int OpenALDataBuffer
    {
      get
      {
        return this.openALDataBuffer;
      }
    }

    public double Duration { get; set; }

    public int SourceId
    {
      get
      {
        return this._sourceId;
      }
      set
      {
        this._sourceId = value;
        if (this.Reserved == null)
          return;
        this.Reserved((object) this, EventArgs.Empty);
      }
    }

    public event EventHandler<EventArgs> Reserved;

    public event EventHandler<EventArgs> Recycled;

    public OALSoundBuffer()
    {
      AL.GetError();
      AL.GenBuffers(1, out this.openALDataBuffer);
      ALError error = AL.GetError();
      if (error == ALError.NoError)
        return;
      Console.WriteLine("Failed to generate OpenAL data buffer: ", (object) AL.GetErrorString(error));
    }

    public void BindDataBuffer(byte[] dataBuffer, ALFormat format, int size, int sampleRate)
    {
      this.pcmDataBuffer = dataBuffer;
      this.openALFormat = format;
      this.dataSize = size;
      this.sampleRate = sampleRate;
      AL.BufferData<byte>(this.openALDataBuffer, this.openALFormat, this.pcmDataBuffer, this.dataSize, this.sampleRate);
      int num1;
      AL.GetBuffer(this.openALDataBuffer, ALGetBufferi.Bits, out num1);
      int num2;
      AL.GetBuffer(this.openALDataBuffer, ALGetBufferi.Channels, out num2);
      ALError error = AL.GetError();
      if (error != ALError.NoError)
      {
        Console.WriteLine("Failed to get buffer attributes: ", (object) AL.GetErrorString(error));
        this.Duration = -1.0;
      }
      else
        this.Duration = (double) (size / (num1 / 8 * num2)) / (double) sampleRate;
    }

    public void Dispose()
    {
      this.CleanUpBuffer();
    }

    public void CleanUpBuffer()
    {
      if (AL.IsBuffer(this.openALDataBuffer))
        AL.DeleteBuffers(1, ref this.openALDataBuffer);
      this.pcmDataBuffer = (byte[]) null;
    }

    public void RecycleSoundBuffer()
    {
      if (this.Recycled == null)
        return;
      this.Recycled((object) this, EventArgs.Empty);
    }
  }
}
