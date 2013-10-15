// Type: Newtonsoft.Json.Utilities.StringBuffer
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using System;

namespace Newtonsoft.Json.Utilities
{
  internal class StringBuffer
  {
    private static readonly char[] EmptyBuffer = new char[0];
    private char[] _buffer;
    private int _position;

    public int Position
    {
      get
      {
        return this._position;
      }
      set
      {
        this._position = value;
      }
    }

    static StringBuffer()
    {
    }

    public StringBuffer()
    {
      this._buffer = StringBuffer.EmptyBuffer;
    }

    public StringBuffer(int initalSize)
    {
      this._buffer = new char[initalSize];
    }

    public void Append(char value)
    {
      if (this._position == this._buffer.Length)
        this.EnsureSize(1);
      this._buffer[this._position++] = value;
    }

    public void Append(char[] buffer, int startIndex, int count)
    {
      if (this._position + count >= this._buffer.Length)
        this.EnsureSize(count);
      Array.Copy((Array) buffer, startIndex, (Array) this._buffer, this._position, count);
      this._position += count;
    }

    public void Clear()
    {
      this._buffer = StringBuffer.EmptyBuffer;
      this._position = 0;
    }

    private void EnsureSize(int appendLength)
    {
      char[] chArray = new char[(this._position + appendLength) * 2];
      Array.Copy((Array) this._buffer, (Array) chArray, this._position);
      this._buffer = chArray;
    }

    public override string ToString()
    {
      return this.ToString(0, this._position);
    }

    public string ToString(int start, int length)
    {
      return new string(this._buffer, start, length);
    }

    public char[] GetInternalBuffer()
    {
      return this._buffer;
    }
  }
}
