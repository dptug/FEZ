// Type: NVorbis.Hashing
// Assembly: NVorbis, Version=0.5.5.0, Culture=neutral, PublicKeyToken=null
// MVID: CF8FE41E-969B-4426-8E05-8E0EFF882245
// Assembly location: F:\Program Files (x86)\FEZ\NVorbis.dll

namespace NVorbis
{
  internal static class Hashing
  {
    private static unsafe void Hash(byte* d, int len, ref uint h)
    {
      for (int index = 0; index < len; ++index)
      {
        h += (uint) d[index];
        h += h << 10;
        h ^= h >> 6;
      }
    }

    private static unsafe void Hash(ref uint h, int data)
    {
      Hashing.Hash((byte*) &data, 4, ref h);
    }

    private static unsafe int Avalanche(uint h)
    {
      h += h << 3;
      h ^= h >> 11;
      h += h << 15;
      return (int) *&h;
    }

    public static int CombineHashCodes(int first, int second)
    {
      uint h = 0U;
      Hashing.Hash(ref h, first);
      Hashing.Hash(ref h, second);
      return Hashing.Avalanche(h);
    }
  }
}
