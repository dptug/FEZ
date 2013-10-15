// Type: SharpDX.ComArray`1
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using System;
using System.Collections;
using System.Collections.Generic;

namespace SharpDX
{
  public class ComArray<T> : ComArray, IEnumerable<T>, IEnumerable where T : ComObject
  {
    public T this[int i]
    {
      get
      {
        return (T) this.Get(i);
      }
      set
      {
        this.Set(i, (ComObject) value);
      }
    }

    public ComArray(params T[] array)
      : base((ComObject[]) array)
    {
    }

    public ComArray(int size)
      : base(size)
    {
    }

    public IEnumerator<T> GetEnumerator()
    {
      return (IEnumerator<T>) new ComArray<T>.ArrayEnumerator<T>(this.values.GetEnumerator());
    }

    private struct ArrayEnumerator<T1> : IEnumerator<T1>, IDisposable, IEnumerator where T1 : ComObject
    {
      private readonly IEnumerator enumerator;

      public T1 Current
      {
        get
        {
          return (T1) this.enumerator.Current;
        }
      }

      object IEnumerator.Current
      {
        get
        {
          return (object) this.Current;
        }
      }

      public ArrayEnumerator(IEnumerator enumerator)
      {
        this.enumerator = enumerator;
      }

      public void Dispose()
      {
      }

      public bool MoveNext()
      {
        return this.enumerator.MoveNext();
      }

      public void Reset()
      {
        this.enumerator.Reset();
      }
    }
  }
}
