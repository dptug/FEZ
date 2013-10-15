// Type: NVorbis.Huffman
// Assembly: NVorbis, Version=0.5.5.0, Culture=neutral, PublicKeyToken=null
// MVID: CF8FE41E-969B-4426-8E05-8E0EFF882245
// Assembly location: F:\Program Files (x86)\FEZ\NVorbis.dll

using System;

namespace NVorbis
{
  internal static class Huffman
  {
    internal static HuffmanListNode<T> BuildLinkedList<T>(T[] values, int[] lengthList, int[] codeList)
    {
      HuffmanListNode<T>[] array = new HuffmanListNode<T>[lengthList.Length];
      for (int index = 0; index < array.Length; ++index)
        array[index] = new HuffmanListNode<T>()
        {
          Value = values[index],
          Length = lengthList[index] <= 0 ? 99999 : lengthList[index],
          Bits = codeList[index],
          Mask = (1 << lengthList[index]) - 1
        };
      Array.Sort<HuffmanListNode<T>>(array, (Comparison<HuffmanListNode<T>>) ((i1, i2) =>
      {
        int local_0 = i1.Length - i2.Length;
        if (local_0 == 0)
          return i1.Bits - i2.Bits;
        else
          return local_0;
      }));
      for (int index = 1; index < array.Length && array[index].Length < 99999; ++index)
        array[index - 1].Next = array[index];
      return array[0];
    }
  }
}
