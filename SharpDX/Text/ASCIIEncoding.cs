// Type: SharpDX.Text.ASCIIEncoding
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

namespace SharpDX.Text
{
  public class ASCIIEncoding : Encoding
  {
    public override int GetByteCount(char[] chars, int index, int count)
    {
      return count;
    }

    public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
    {
      for (int index = 0; index < charCount; ++index)
        bytes[byteIndex + index] = (byte) ((uint) chars[charIndex + index] & (uint) sbyte.MaxValue);
      return charCount;
    }

    public override int GetCharCount(byte[] bytes, int index, int count)
    {
      return count;
    }

    public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
    {
      for (int index = 0; index < byteCount; ++index)
        chars[charIndex + index] = (char) ((uint) bytes[byteIndex + index] & (uint) sbyte.MaxValue);
      return byteCount;
    }

    public override int GetMaxByteCount(int charCount)
    {
      return charCount;
    }

    public override int GetMaxCharCount(int byteCount)
    {
      return byteCount;
    }
  }
}
