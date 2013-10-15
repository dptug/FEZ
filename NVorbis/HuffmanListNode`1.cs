// Type: NVorbis.HuffmanListNode`1
// Assembly: NVorbis, Version=0.5.5.0, Culture=neutral, PublicKeyToken=null
// MVID: CF8FE41E-969B-4426-8E05-8E0EFF882245
// Assembly location: F:\Program Files (x86)\FEZ\NVorbis.dll

namespace NVorbis
{
  internal class HuffmanListNode<T>
  {
    internal T Value;
    internal int Length;
    internal int Bits;
    internal int Mask;
    internal HuffmanListNode<T> Next;

    public int HitCount { get; set; }
  }
}
