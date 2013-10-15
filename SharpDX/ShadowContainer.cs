// Type: SharpDX.ShadowContainer
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SharpDX
{
  internal class ShadowContainer : DisposeBase
  {
    private static readonly Dictionary<Type, List<Type>> typeToShadowTypes = new Dictionary<Type, List<Type>>();
    private readonly Dictionary<Guid, CppObjectShadow> guidToShadow = new Dictionary<Guid, CppObjectShadow>();
    private IntPtr guidPtr;

    public IntPtr[] Guids { get; private set; }

    static ShadowContainer()
    {
    }

    public void Initialize(ICallbackable callbackable)
    {
      callbackable.Shadow = (IDisposable) this;
      Type type1 = callbackable.GetType();
      List<Type> list;
      lock (ShadowContainer.typeToShadowTypes)
      {
        if (!ShadowContainer.typeToShadowTypes.TryGetValue(type1, out list))
        {
          Type[] local_2 = type1.GetInterfaces();
          list = new List<Type>();
          list.AddRange((IEnumerable<Type>) local_2);
          ShadowContainer.typeToShadowTypes.Add(type1, list);
          foreach (Type item_1 in local_2)
          {
            if (ShadowAttribute.Get(item_1) == null)
            {
              list.Remove(item_1);
            }
            else
            {
              foreach (Type item_0 in item_1.GetInterfaces())
                list.Remove(item_0);
            }
          }
        }
      }
      CppObjectShadow cppObjectShadow1 = (CppObjectShadow) null;
      foreach (Type type2 in list)
      {
        CppObjectShadow cppObjectShadow2 = (CppObjectShadow) Activator.CreateInstance(ShadowAttribute.Get(type2).Type);
        cppObjectShadow2.Initialize(callbackable);
        if (cppObjectShadow1 == null)
        {
          cppObjectShadow1 = cppObjectShadow2;
          this.guidToShadow.Add(ComObjectShadow.IID_IUnknown, cppObjectShadow1);
        }
        this.guidToShadow.Add(Utilities.GetGuidFromType(type2), cppObjectShadow2);
        foreach (Type type3 in type2.GetInterfaces())
        {
          if (ShadowAttribute.Get(type3) != null)
            this.guidToShadow.Add(Utilities.GetGuidFromType(type3), cppObjectShadow2);
        }
      }
    }

    internal IntPtr Find(Type type)
    {
      return this.Find(Utilities.GetGuidFromType(type));
    }

    internal IntPtr Find(Guid guidType)
    {
      CppObjectShadow shadow = this.FindShadow(guidType);
      if (shadow != null)
        return shadow.NativePointer;
      else
        return IntPtr.Zero;
    }

    internal CppObjectShadow FindShadow(Guid guidType)
    {
      CppObjectShadow cppObjectShadow;
      this.guidToShadow.TryGetValue(guidType, out cppObjectShadow);
      return cppObjectShadow;
    }

    protected override void Dispose(bool disposing)
    {
      if (!disposing)
        return;
      foreach (DisposeBase disposeBase in this.guidToShadow.Values)
        disposeBase.Dispose();
      this.guidToShadow.Clear();
      if (!(this.guidPtr != IntPtr.Zero))
        return;
      Marshal.FreeHGlobal(this.guidPtr);
      this.guidPtr = IntPtr.Zero;
    }
  }
}
