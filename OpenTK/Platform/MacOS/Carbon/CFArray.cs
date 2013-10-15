// Type: OpenTK.Platform.MacOS.Carbon.CFArray
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Platform.MacOS.Carbon
{
  internal struct CFArray
  {
    private IntPtr arrayRef;

    public IntPtr Ref
    {
      get
      {
        return this.arrayRef;
      }
      set
      {
        this.arrayRef = value;
      }
    }

    public int Count
    {
      get
      {
        return CF.CFArrayGetCount(this.arrayRef);
      }
    }

    public IntPtr this[int index]
    {
      get
      {
        if (index >= this.Count || index < 0)
          throw new IndexOutOfRangeException();
        else
          return CF.CFArrayGetValueAtIndex(this.arrayRef, index);
      }
    }

    public CFArray(IntPtr reference)
    {
      this.arrayRef = reference;
    }
  }
}
