// Type: OpenTK.Platform.Utilities
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK.Graphics;
using OpenTK.Platform.Dummy;
using OpenTK.Platform.MacOS;
using OpenTK.Platform.Windows;
using OpenTK.Platform.X11;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace OpenTK.Platform
{
  public static class Utilities
  {
    private static bool throw_on_error;

    internal static bool ThrowOnX11Error
    {
      get
      {
        return Utilities.throw_on_error;
      }
      set
      {
        if (value && !Utilities.throw_on_error)
        {
          Type.GetType("System.Windows.Forms.XplatUIX11, System.Windows.Forms").GetField("ErrorExceptions", BindingFlags.Static | BindingFlags.NonPublic).SetValue((object) null, (object) true);
          Utilities.throw_on_error = true;
        }
        else
        {
          if (value || !Utilities.throw_on_error)
            return;
          Type.GetType("System.Windows.Forms.XplatUIX11, System.Windows.Forms").GetField("ErrorExceptions", BindingFlags.Static | BindingFlags.NonPublic).SetValue((object) null, (object) false);
          Utilities.throw_on_error = false;
        }
      }
    }

    internal static void LoadExtensions(Type type)
    {
      int num = 0;
      Type nestedType = type.GetNestedType("Delegates", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
      if (nestedType == null)
        throw new InvalidOperationException("The specified type does not have any loadable extensions.");
      FieldInfo[] fields = nestedType.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
      if (fields == null)
        throw new InvalidOperationException("The specified type does not have any loadable extensions.");
      MethodInfo method = type.GetMethod("LoadDelegate", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
      if (method == null)
        throw new InvalidOperationException(type.ToString() + " does not contain a static LoadDelegate method.");
      Utilities.LoadDelegateFunction delegateFunction = (Utilities.LoadDelegateFunction) Delegate.CreateDelegate(typeof (Utilities.LoadDelegateFunction), method);
      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Reset();
      stopwatch.Start();
      foreach (FieldInfo fieldInfo in fields)
      {
        Delegate @delegate = delegateFunction(fieldInfo.Name, fieldInfo.FieldType);
        if (@delegate != null)
          ++num;
        fieldInfo.SetValue((object) null, (object) @delegate);
      }
      FieldInfo field = type.GetField("rebuildExtensionList", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
      if (field != null)
        field.SetValue((object) null, (object) true);
      stopwatch.Stop();
      stopwatch.Reset();
    }

    internal static bool TryLoadExtension(Type type, string extension)
    {
      Type nestedType = type.GetNestedType("Delegates", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
      if (nestedType == null)
        return false;
      Utilities.LoadDelegateFunction delegateFunction = (Utilities.LoadDelegateFunction) Delegate.CreateDelegate(typeof (Utilities.LoadDelegateFunction), type.GetMethod("LoadDelegate", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic));
      if (delegateFunction == null)
        return false;
      FieldInfo field1 = nestedType.GetField(extension, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
      if (field1 == null)
        return false;
      Delegate delegate1 = field1.GetValue((object) null) as Delegate;
      Delegate delegate2 = delegateFunction(field1.Name, field1.FieldType);
      if ((delegate1 != null ? delegate1.Target : (object) null) != (delegate2 != null ? delegate2.Target : (object) null))
      {
        field1.SetValue((object) null, (object) delegate2);
        FieldInfo field2 = type.GetField("rebuildExtensionList", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        if (field2 != null)
          field2.SetValue((object) null, (object) true);
      }
      return delegate2 != null;
    }

    [Obsolete("Call new OpenTK.Graphics.GraphicsContext() directly, instead.")]
    public static IGraphicsContext CreateGraphicsContext(GraphicsMode mode, IWindowInfo window, int major, int minor, GraphicsContextFlags flags)
    {
      GraphicsContext graphicsContext = new GraphicsContext(mode, window, major, minor, flags);
      graphicsContext.MakeCurrent(window);
      graphicsContext.LoadAll();
      return (IGraphicsContext) graphicsContext;
    }

    public static IWindowInfo CreateX11WindowInfo(IntPtr display, int screen, IntPtr windowHandle, IntPtr rootWindow, IntPtr visualInfo)
    {
      return (IWindowInfo) new X11WindowInfo()
      {
        Display = display,
        Screen = screen,
        WindowHandle = windowHandle,
        RootWindow = rootWindow,
        VisualInfo = (XVisualInfo) Marshal.PtrToStructure(visualInfo, typeof (XVisualInfo))
      };
    }

    public static IWindowInfo CreateWindowsWindowInfo(IntPtr windowHandle)
    {
      return (IWindowInfo) new WinWindowInfo(windowHandle, (WinWindowInfo) null);
    }

    public static IWindowInfo CreateMacOSCarbonWindowInfo(IntPtr windowHandle, bool ownHandle, bool isControl)
    {
      return (IWindowInfo) new CarbonWindowInfo(windowHandle, false, isControl);
    }

    public static IWindowInfo CreateDummyWindowInfo()
    {
      return (IWindowInfo) new DummyWindowInfo();
    }

    private delegate Delegate LoadDelegateFunction(string name, Type signature);
  }
}
