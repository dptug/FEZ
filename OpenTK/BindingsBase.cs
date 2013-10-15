// Type: OpenTK.BindingsBase
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace OpenTK
{
  public abstract class BindingsBase
  {
    protected readonly SortedList<string, MethodInfo> CoreFunctionMap = new SortedList<string, MethodInfo>();
    private bool rebuildExtensionList = true;
    protected readonly Type DelegatesClass;
    protected readonly Type CoreClass;

    protected bool RebuildExtensionList
    {
      get
      {
        return this.rebuildExtensionList;
      }
      set
      {
        this.rebuildExtensionList = value;
      }
    }

    protected abstract object SyncRoot { get; }

    public BindingsBase()
    {
      this.DelegatesClass = this.GetType().GetNestedType("Delegates", BindingFlags.Static | BindingFlags.NonPublic);
      this.CoreClass = this.GetType().GetNestedType("Core", BindingFlags.Static | BindingFlags.NonPublic);
      if (this.CoreClass == null)
        return;
      MethodInfo[] methods = this.CoreClass.GetMethods(BindingFlags.Static | BindingFlags.NonPublic);
      this.CoreFunctionMap = new SortedList<string, MethodInfo>(methods.Length);
      foreach (MethodInfo methodInfo in methods)
        this.CoreFunctionMap.Add(methodInfo.Name, methodInfo);
    }

    protected abstract IntPtr GetAddress(string funcname);

    internal void LoadEntryPoints()
    {
      int num = 0;
      FieldInfo[] fields = this.DelegatesClass.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
      if (fields == null)
        throw new InvalidOperationException("The specified type does not have any loadable extensions.");
      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Reset();
      stopwatch.Start();
      foreach (FieldInfo fieldInfo in fields)
      {
        Delegate @delegate = this.LoadDelegate(fieldInfo.Name, fieldInfo.FieldType);
        if (@delegate != null)
          ++num;
        lock (this.SyncRoot)
          fieldInfo.SetValue((object) null, (object) @delegate);
      }
      this.rebuildExtensionList = true;
      stopwatch.Stop();
      stopwatch.Reset();
    }

    internal bool LoadEntryPoint(string function)
    {
      FieldInfo field = this.DelegatesClass.GetField(function, BindingFlags.Static | BindingFlags.NonPublic);
      if (field == null)
        return false;
      Delegate delegate1 = field.GetValue((object) null) as Delegate;
      Delegate delegate2 = this.LoadDelegate(field.Name, field.FieldType);
      lock (this.SyncRoot)
      {
        if (delegate1.Target != delegate2.Target)
          field.SetValue((object) null, (object) delegate2);
      }
      return delegate2 != null;
    }

    private Delegate LoadDelegate(string name, Type signature)
    {
      Delegate extensionDelegate = this.GetExtensionDelegate(name, signature);
      if (extensionDelegate != null)
        return extensionDelegate;
      MethodInfo method;
      if (!this.CoreFunctionMap.TryGetValue(name.Substring(2), out method))
        return (Delegate) null;
      else
        return Delegate.CreateDelegate(signature, method);
    }

    internal Delegate GetExtensionDelegate(string name, Type signature)
    {
      IntPtr address = this.GetAddress(name);
      if (address == IntPtr.Zero || address == new IntPtr(1) || address == new IntPtr(2))
        return (Delegate) null;
      else
        return Marshal.GetDelegateForFunctionPointer(address, signature);
    }
  }
}
