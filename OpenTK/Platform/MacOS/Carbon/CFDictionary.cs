// Type: OpenTK.Platform.MacOS.Carbon.CFDictionary
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Platform.MacOS.Carbon
{
  internal struct CFDictionary
  {
    private IntPtr dictionaryRef;

    public IntPtr Ref
    {
      get
      {
        return this.dictionaryRef;
      }
      set
      {
        this.dictionaryRef = value;
      }
    }

    public int Count
    {
      get
      {
        return CF.CFDictionaryGetCount(this.dictionaryRef);
      }
    }

    public CFDictionary(IntPtr reference)
    {
      this.dictionaryRef = reference;
    }

    public unsafe double GetNumberValue(string key)
    {
      double num;
      CF.CFNumberGetValue(CF.CFDictionaryGetValue(this.dictionaryRef, CF.CFSTR(key)), CF.CFNumberType.kCFNumberDoubleType, &num);
      return num;
    }
  }
}
