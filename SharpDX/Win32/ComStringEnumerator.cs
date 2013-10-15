// Type: SharpDX.Win32.ComStringEnumerator
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX;
using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace SharpDX.Win32
{
  internal class ComStringEnumerator : System.Collections.Generic.IEnumerator<string>, IDisposable, System.Collections.IEnumerator, System.Collections.Generic.IEnumerable<string>, System.Collections.IEnumerable
  {
    private readonly IEnumString enumString;
    private string current;

    public string Current
    {
      get
      {
        return this.current;
      }
    }

    object System.Collections.IEnumerator.Current
    {
      get
      {
        return (object) this.Current;
      }
    }

    public ComStringEnumerator(IntPtr ptrToIEnumString)
    {
      this.enumString = (IEnumString) Marshal.GetTypedObjectForIUnknown(ptrToIEnumString, typeof (IEnumString));
    }

    public void Dispose()
    {
    }

    public unsafe bool MoveNext()
    {
      string[] rgelt = new string[1];
      int num = 0;
      bool flag = this.enumString.Next(1, rgelt, new IntPtr((void*) &num)) == Result.Ok.Code;
      this.current = flag ? rgelt[0] : (string) null;
      return flag;
    }

    public void Reset()
    {
      this.enumString.Reset();
    }

    public System.Collections.Generic.IEnumerator<string> GetEnumerator()
    {
      return (System.Collections.Generic.IEnumerator<string>) this;
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return (System.Collections.IEnumerator) this.GetEnumerator();
    }
  }
}
