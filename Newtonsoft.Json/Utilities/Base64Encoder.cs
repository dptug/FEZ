// Type: Newtonsoft.Json.Utilities.Base64Encoder
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using System;
using System.IO;

namespace Newtonsoft.Json.Utilities
{
  internal class Base64Encoder
  {
    private readonly char[] _charsLine = new char[76];
    private const int Base64LineSize = 76;
    private const int LineSizeInBytes = 57;
    private readonly TextWriter _writer;
    private byte[] _leftOverBytes;
    private int _leftOverBytesCount;

    public Base64Encoder(TextWriter writer)
    {
      ValidationUtils.ArgumentNotNull((object) writer, "writer");
      this._writer = writer;
    }

    public void Encode(byte[] buffer, int index, int count)
    {
      if (buffer == null)
        throw new ArgumentNullException("buffer");
      if (index < 0)
        throw new ArgumentOutOfRangeException("index");
      if (count < 0)
        throw new ArgumentOutOfRangeException("count");
      if (count > buffer.Length - index)
        throw new ArgumentOutOfRangeException("count");
      if (this._leftOverBytesCount > 0)
      {
        int num;
        for (num = this._leftOverBytesCount; num < 3 && count > 0; --count)
          this._leftOverBytes[num++] = buffer[index++];
        if (count == 0 && num < 3)
        {
          this._leftOverBytesCount = num;
          return;
        }
        else
          this.WriteChars(this._charsLine, 0, Convert.ToBase64CharArray(this._leftOverBytes, 0, 3, this._charsLine, 0));
      }
      this._leftOverBytesCount = count % 3;
      if (this._leftOverBytesCount > 0)
      {
        count -= this._leftOverBytesCount;
        if (this._leftOverBytes == null)
          this._leftOverBytes = new byte[3];
        for (int index1 = 0; index1 < this._leftOverBytesCount; ++index1)
          this._leftOverBytes[index1] = buffer[index + count + index1];
      }
      int num1 = index + count;
      int length = 57;
      while (index < num1)
      {
        if (index + length > num1)
          length = num1 - index;
        this.WriteChars(this._charsLine, 0, Convert.ToBase64CharArray(buffer, index, length, this._charsLine, 0));
        index += length;
      }
    }

    public void Flush()
    {
      if (this._leftOverBytesCount <= 0)
        return;
      this.WriteChars(this._charsLine, 0, Convert.ToBase64CharArray(this._leftOverBytes, 0, this._leftOverBytesCount, this._charsLine, 0));
      this._leftOverBytesCount = 0;
    }

    private void WriteChars(char[] chars, int index, int count)
    {
      this._writer.Write(chars, index, count);
    }
  }
}
