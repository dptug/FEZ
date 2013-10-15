// Type: NVorbis.RingBuffer
// Assembly: NVorbis, Version=0.5.5.0, Culture=neutral, PublicKeyToken=null
// MVID: CF8FE41E-969B-4426-8E05-8E0EFF882245
// Assembly location: F:\Program Files (x86)\FEZ\NVorbis.dll

using System;

namespace NVorbis
{
  internal class RingBuffer
  {
    private float[] _buffer;
    private int _start;
    private int _end;
    private int _bufLen;
    internal int Channels;

    internal int Length
    {
      get
      {
        int num = this._end - this._start;
        if (num < 0)
          num += this._bufLen;
        return num;
      }
    }

    internal RingBuffer(int size)
    {
      this._buffer = new float[size];
      this._start = this._end = 0;
      this._bufLen = size;
    }

    internal void EnsureSize(int size)
    {
      size += this.Channels;
      if (this._bufLen >= size)
        return;
      float[] numArray = new float[size];
      Array.Copy((Array) this._buffer, this._start, (Array) numArray, 0, this._bufLen - this._start);
      if (this._end < this._start)
        Array.Copy((Array) this._buffer, 0, (Array) numArray, this._bufLen - this._start, this._end);
      int length = this.Length;
      this._start = 0;
      this._end = length;
      this._buffer = numArray;
      this._bufLen = size;
    }

    internal void CopyTo(float[] buffer, int index, int count)
    {
      if (index < 0 || index + count > buffer.Length)
        throw new ArgumentOutOfRangeException("index");
      int sourceIndex = this._start;
      this.RemoveItems(count);
      int num = (this._end - sourceIndex + this._bufLen) % this._bufLen;
      if (count > num)
        throw new ArgumentOutOfRangeException("count");
      int length = Math.Min(count, this._bufLen - sourceIndex);
      Array.Copy((Array) this._buffer, sourceIndex, (Array) buffer, index, length);
      if (length >= count)
        return;
      Array.Copy((Array) this._buffer, 0, (Array) buffer, index + length, count - length);
    }

    internal void RemoveItems(int count)
    {
      int num = (count + this._start) % this._bufLen;
      if (this._end > this._start)
      {
        if (num > this._end || num < this._start)
          throw new ArgumentOutOfRangeException();
      }
      else if (num < this._start && num > this._end)
        throw new ArgumentOutOfRangeException();
      this._start = num;
    }

    internal void Clear()
    {
      this._start = this._end;
    }

    internal void Write(int channel, int index, int start, int switchPoint, int end, float[] pcm, float[] window)
    {
      int index1 = (index + start) * this.Channels + channel + this._start;
      while (index1 >= this._bufLen)
        index1 -= this._bufLen;
      if (index1 < 0)
      {
        start -= index;
        index1 = 0;
      }
      for (; index1 < this._bufLen && start < switchPoint; ++start)
      {
        this._buffer[index1] += pcm[start] * window[start];
        index1 += this.Channels;
      }
      if (index1 >= this._bufLen)
      {
        index1 -= this._bufLen;
        for (; start < switchPoint; ++start)
        {
          this._buffer[index1] += pcm[start] * window[start];
          index1 += this.Channels;
        }
      }
      for (; index1 < this._bufLen && start < end; ++start)
      {
        this._buffer[index1] = pcm[start] * window[start];
        index1 += this.Channels;
      }
      if (index1 >= this._bufLen)
      {
        index1 -= this._bufLen;
        for (; start < end; ++start)
        {
          this._buffer[index1] = pcm[start] * window[start];
          index1 += this.Channels;
        }
      }
      this._end = index1;
    }
  }
}
