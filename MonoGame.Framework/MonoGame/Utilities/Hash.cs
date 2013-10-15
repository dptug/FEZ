// Type: MonoGame.Utilities.Hash
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

namespace MonoGame.Utilities
{
  internal class Hash
  {
    internal static int ComputeHash(params byte[] data)
    {
      int num1 = -2128831035;
      for (int index = 0; index < data.Length; ++index)
        num1 = (num1 ^ (int) data[index]) * 16777619;
      int num2 = num1 + (num1 << 13);
      int num3 = num2 ^ num2 >> 7;
      int num4 = num3 + (num3 << 3);
      int num5 = num4 ^ num4 >> 17;
      return num5 + (num5 << 5);
    }
  }
}
