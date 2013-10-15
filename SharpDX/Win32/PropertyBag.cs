// Type: SharpDX.Win32.PropertyBag
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SharpDX.Win32
{
  public class PropertyBag : ComObject
  {
    private PropertyBag.IPropertyBag2 nativePropertyBag;

    public int Count
    {
      get
      {
        this.CheckIfInitialized();
        int pcProperties;
        this.nativePropertyBag.CountProperties(out pcProperties);
        return pcProperties;
      }
    }

    public string[] Keys
    {
      get
      {
        this.CheckIfInitialized();
        List<string> list = new List<string>();
        for (int iProperty = 0; iProperty < this.Count; ++iProperty)
        {
          PropertyBag.PROPBAG2 pPropBag;
          int pcProperties;
          this.nativePropertyBag.GetPropertyInfo(iProperty, 1, out pPropBag, out pcProperties);
          list.Add(pPropBag.Name);
        }
        return list.ToArray();
      }
    }

    public PropertyBag(IntPtr propertyBagPointer)
      : base(propertyBagPointer)
    {
    }

    protected override void NativePointerUpdated(IntPtr oldNativePointer)
    {
      base.NativePointerUpdated(oldNativePointer);
      if (this.NativePointer != IntPtr.Zero)
        this.nativePropertyBag = (PropertyBag.IPropertyBag2) Marshal.GetObjectForIUnknown(this.NativePointer);
      else
        this.nativePropertyBag = (PropertyBag.IPropertyBag2) null;
    }

    private void CheckIfInitialized()
    {
      if (this.nativePropertyBag == null)
        throw new InvalidOperationException("This instance is not bound to an unmanaged IPropertyBag2");
    }

    public object Get(string name)
    {
      this.CheckIfInitialized();
      PropertyBag.PROPBAG2 pPropBag = new PropertyBag.PROPBAG2()
      {
        Name = name
      };
      object pvarValue;
      Result phrError;
      if (this.nativePropertyBag.Read(1, ref pPropBag, IntPtr.Zero, out pvarValue, out phrError).Failure || phrError.Failure)
      {
        throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Property with name [{0}] is not valid for this instance", new object[1]
        {
          (object) name
        }));
      }
      else
      {
        pPropBag.Dispose();
        return pvarValue;
      }
    }

    public T1 Get<T1, T2>(PropertyKey<T1, T2> propertyKey)
    {
      return (T1) Convert.ChangeType(this.Get(propertyKey.Name), typeof (T1));
    }

    public void Set(string name, object value)
    {
      this.CheckIfInitialized();
      object obj1 = this.Get(name);
      value = Convert.ChangeType(value, obj1 == null ? value.GetType() : obj1.GetType());
      PropertyBag.PROPBAG2 propbaG2 = new PropertyBag.PROPBAG2()
      {
        Name = name
      };
      PropertyBag.IPropertyBag2 propertyBag2 = this.nativePropertyBag;
      int cProperties = 1;
      // ISSUE: explicit reference operation
      // ISSUE: variable of a reference type
      PropertyBag.PROPBAG2& pPropBag = @propbaG2;
      object obj2 = value;
      // ISSUE: explicit reference operation
      // ISSUE: variable of a reference type
      object& local = @obj2;
      propertyBag2.Write(cProperties, pPropBag, local).CheckError();
      propbaG2.Dispose();
    }

    public void Set<T1, T2>(PropertyKey<T1, T2> propertyKey, T1 value)
    {
      this.Set(propertyKey.Name, (object) value);
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    private struct PROPBAG2 : IDisposable
    {
      internal uint type;
      internal ushort vt;
      internal ushort cfType;
      internal IntPtr dwHint;
      internal IntPtr pstrName;
      internal Guid clsid;

      public string Name
      {
        get
        {
          return Marshal.PtrToStringUni(this.pstrName);
        }
        set
        {
          this.pstrName = Marshal.StringToCoTaskMemUni(value);
        }
      }

      public void Dispose()
      {
        if (!(this.pstrName != IntPtr.Zero))
          return;
        Marshal.FreeCoTaskMem(this.pstrName);
        this.pstrName = IntPtr.Zero;
      }
    }

    [Guid("22F55882-280B-11D0-A8A9-00A0C90C2004")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [ComImport]
    private interface IPropertyBag2
    {
      [MethodImpl(MethodImplOptions.PreserveSig)]
      Result Read([In] int cProperties, [In] ref PropertyBag.PROPBAG2 pPropBag, IntPtr pErrLog, out object pvarValue, out Result phrError);

      [MethodImpl(MethodImplOptions.PreserveSig)]
      Result Write([In] int cProperties, [In] ref PropertyBag.PROPBAG2 pPropBag, ref object value);

      [MethodImpl(MethodImplOptions.PreserveSig)]
      Result CountProperties(out int pcProperties);

      [MethodImpl(MethodImplOptions.PreserveSig)]
      Result GetPropertyInfo([In] int iProperty, [In] int cProperties, out PropertyBag.PROPBAG2 pPropBag, out int pcProperties);

      [MethodImpl(MethodImplOptions.PreserveSig)]
      Result LoadObject([MarshalAs(UnmanagedType.LPWStr), In] string pstrName, [In] uint dwHint, [MarshalAs(UnmanagedType.IUnknown), In] object pUnkObject, IntPtr pErrLog);
    }
  }
}
