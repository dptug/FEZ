// Type: SharpDX.Diagnostics.ObjectReference
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace SharpDX.Diagnostics
{
  public class ObjectReference
  {
    public DateTime CreationTime { get; private set; }

    public WeakReference Object { get; private set; }

    public StackTrace StackTrace { get; private set; }

    public bool IsAlive
    {
      get
      {
        return this.Object.IsAlive;
      }
    }

    public ObjectReference(DateTime creationTime, ComObject comObject, StackTrace stackTrace)
    {
      this.CreationTime = creationTime;
      this.Object = new WeakReference((object) comObject, true);
      this.StackTrace = stackTrace;
    }

    public override string ToString()
    {
      ComObject comObject = this.Object.Target as ComObject;
      if (comObject == null)
        return "";
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendFormat((IFormatProvider) CultureInfo.InvariantCulture, "Active COM Object: [0x{0:X}] Class: [{1}] Time [{2}] Stack:", (object) comObject.NativePointer.ToInt64(), (object) comObject.GetType().FullName, (object) this.CreationTime).AppendLine();
      foreach (StackFrame stackFrame in this.StackTrace.GetFrames())
      {
        if (stackFrame.GetFileLineNumber() != 0)
          stringBuilder.AppendFormat((IFormatProvider) CultureInfo.InvariantCulture, "\t{0}({1},{2}) : {3}", (object) stackFrame.GetFileName(), (object) stackFrame.GetFileLineNumber(), (object) stackFrame.GetFileColumnNumber(), (object) stackFrame.GetMethod()).AppendLine();
      }
      return ((object) stringBuilder).ToString();
    }
  }
}
