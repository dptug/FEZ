// Type: SharpDX.Diagnostics.ObjectTracker
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SharpDX.Diagnostics
{
  public static class ObjectTracker
  {
    private static readonly Dictionary<IntPtr, List<ObjectReference>> ObjectReferences = new Dictionary<IntPtr, List<ObjectReference>>();

    static ObjectTracker()
    {
      AppDomain.CurrentDomain.DomainUnload += new EventHandler(ObjectTracker.OnProcessExit);
      AppDomain.CurrentDomain.ProcessExit += new EventHandler(ObjectTracker.OnProcessExit);
    }

    private static void OnProcessExit(object sender, EventArgs e)
    {
      if (!Configuration.EnableObjectTracking)
        return;
      string str = ObjectTracker.ReportActiveObjects();
      if (string.IsNullOrEmpty(str))
        return;
      Console.WriteLine(str);
    }

    public static void Track(ComObject comObject)
    {
      if (comObject == null || comObject.NativePointer == IntPtr.Zero)
        return;
      lock (ObjectTracker.ObjectReferences)
      {
        List<ObjectReference> local_0;
        if (!ObjectTracker.ObjectReferences.TryGetValue(comObject.NativePointer, out local_0))
        {
          local_0 = new List<ObjectReference>();
          ObjectTracker.ObjectReferences.Add(comObject.NativePointer, local_0);
        }
        local_0.Add(new ObjectReference(DateTime.Now, comObject, new StackTrace(3, true)));
      }
    }

    public static List<ObjectReference> Find(IntPtr comObjectPtr)
    {
      lock (ObjectTracker.ObjectReferences)
      {
        List<ObjectReference> local_0;
        if (ObjectTracker.ObjectReferences.TryGetValue(comObjectPtr, out local_0))
          return new List<ObjectReference>((IEnumerable<ObjectReference>) local_0);
      }
      return new List<ObjectReference>();
    }

    public static ObjectReference Find(ComObject comObject)
    {
      lock (ObjectTracker.ObjectReferences)
      {
        List<ObjectReference> local_0;
        if (ObjectTracker.ObjectReferences.TryGetValue(comObject.NativePointer, out local_0))
        {
          foreach (ObjectReference item_0 in local_0)
          {
            if (object.ReferenceEquals(item_0.Object.Target, (object) comObject))
              return item_0;
          }
        }
      }
      return (ObjectReference) null;
    }

    public static void UnTrack(ComObject comObject)
    {
      if (comObject == null || comObject.NativePointer == IntPtr.Zero)
        return;
      lock (ObjectTracker.ObjectReferences)
      {
        List<ObjectReference> local_0;
        if (!ObjectTracker.ObjectReferences.TryGetValue(comObject.NativePointer, out local_0))
          return;
        for (int local_1 = local_0.Count - 1; local_1 >= 0; --local_1)
        {
          ObjectReference local_2 = local_0[local_1];
          if (object.ReferenceEquals(local_2.Object.Target, (object) comObject))
            local_0.RemoveAt(local_1);
          else if (!local_2.IsAlive)
            local_0.RemoveAt(local_1);
        }
        if (local_0.Count != 0)
          return;
        ObjectTracker.ObjectReferences.Remove(comObject.NativePointer);
      }
    }

    public static List<ObjectReference> FindActiveObjects()
    {
      List<ObjectReference> list = new List<ObjectReference>();
      lock (ObjectTracker.ObjectReferences)
      {
        foreach (List<ObjectReference> item_0 in ObjectTracker.ObjectReferences.Values)
        {
          foreach (ObjectReference item_1 in item_0)
          {
            if (item_1.IsAlive)
              list.Add(item_1);
          }
        }
      }
      return list;
    }

    public static string ReportActiveObjects()
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (object obj in ObjectTracker.FindActiveObjects())
      {
        string str = obj.ToString();
        if (!string.IsNullOrEmpty(str))
          stringBuilder.AppendLine(str);
      }
      return ((object) stringBuilder).ToString();
    }
  }
}
