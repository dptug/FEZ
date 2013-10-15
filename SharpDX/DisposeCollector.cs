// Type: SharpDX.DisposeCollector
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using System;
using System.Collections.Generic;

namespace SharpDX
{
  public class DisposeCollector : DisposeBase
  {
    private List<object> disposables;

    public int Count
    {
      get
      {
        return this.disposables.Count;
      }
    }

    protected override void Dispose(bool disposeManagedResources)
    {
      if (this.disposables != null)
      {
        for (int index = this.disposables.Count - 1; index >= 0; --index)
        {
          object toDisposeArg = this.disposables[index];
          if (toDisposeArg is IDisposable)
            ((IDisposable) toDisposeArg).Dispose();
          else
            Utilities.FreeMemory((IntPtr) toDisposeArg);
          this.Remove<object>(toDisposeArg);
        }
      }
      this.disposables = (List<object>) null;
    }

    public T Collect<T>(T toDispose)
    {
      if (!((object) toDispose is IDisposable) && !((object) toDispose is IntPtr))
        throw new ArgumentException("Argument must be IDisposable or IntPtr");
      if ((object) toDispose is IntPtr && !Utilities.IsMemoryAligned((IntPtr) (object) toDispose, 16))
        throw new ArgumentException("Memory pointer is invalid. Memory must have been allocated with Utilties.AllocateMemory");
      Component component = (object) toDispose as Component;
      if (component != null && component.IsAttached || object.Equals((object) toDispose, (object) default (T)))
        return toDispose;
      if (this.disposables == null)
        this.disposables = new List<object>();
      if (!this.disposables.Contains((object) toDispose))
      {
        this.disposables.Add((object) toDispose);
        if (component != null)
          component.IsAttached = true;
      }
      return toDispose;
    }

    public void RemoveAndDispose<T>(ref T objectToDispose)
    {
      if (this.disposables == null)
        return;
      this.Remove<T>(objectToDispose);
      IDisposable disposable = (object) objectToDispose as IDisposable;
      if (disposable != null)
        disposable.Dispose();
      else
        Utilities.FreeMemory((IntPtr) (object) objectToDispose);
      objectToDispose = default (T);
    }

    public void Remove<T>(T toDisposeArg)
    {
      if (this.disposables == null || !this.disposables.Contains((object) toDisposeArg))
        return;
      this.disposables.Remove((object) toDisposeArg);
      Component component = (object) toDisposeArg as Component;
      if (component == null)
        return;
      component.IsAttached = false;
    }
  }
}
