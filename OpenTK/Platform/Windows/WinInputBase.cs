// Type: OpenTK.Platform.Windows.WinInputBase
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK;
using OpenTK.Input;
using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace OpenTK.Platform.Windows
{
  internal abstract class WinInputBase : IInputDriver2
  {
    private static readonly IntPtr Unhandled = new IntPtr(-1);
    private readonly AutoResetEvent InputReady = new AutoResetEvent(false);
    private readonly WindowProcedure WndProc;
    private readonly Thread InputThread;
    private IntPtr OldWndProc;
    private INativeWindow native;
    protected bool Disposed;

    protected INativeWindow Native
    {
      get
      {
        return this.native;
      }
      private set
      {
        this.native = value;
      }
    }

    protected WinWindowInfo Parent
    {
      get
      {
        return (WinWindowInfo) this.Native.WindowInfo;
      }
    }

    public abstract IMouseDriver2 MouseDriver { get; }

    public abstract IKeyboardDriver2 KeyboardDriver { get; }

    public abstract IGamePadDriver GamePadDriver { get; }

    static WinInputBase()
    {
    }

    public WinInputBase()
    {
      this.WndProc = new WindowProcedure(this.WindowProcedure);
      this.InputThread = new Thread(new ThreadStart(this.ProcessEvents));
      this.InputThread.IsBackground = true;
      this.InputThread.Start();
      this.InputReady.WaitOne();
    }

    ~WinInputBase()
    {
      this.Dispose(false);
    }

    private INativeWindow ConstructMessageWindow()
    {
      INativeWindow nativeWindow = (INativeWindow) new NativeWindow();
      nativeWindow.ProcessEvents();
      Functions.SetParent((nativeWindow.WindowInfo as WinWindowInfo).WindowHandle, Constants.MESSAGE_ONLY);
      nativeWindow.ProcessEvents();
      return nativeWindow;
    }

    private void ProcessEvents()
    {
      this.Native = this.ConstructMessageWindow();
      this.CreateDrivers();
      this.OldWndProc = Functions.SetWindowLong(this.Parent.WindowHandle, this.WndProc);
      this.InputReady.Set();
      MSG msg = new MSG();
      while (this.Native.Exists)
      {
        if (Functions.GetMessage(ref msg, this.Parent.WindowHandle, 0, 0) == -1)
          throw new PlatformException(string.Format("An error happened while processing the message queue. Windows error: {0}", (object) Marshal.GetLastWin32Error()));
        Functions.TranslateMessage(ref msg);
        Functions.DispatchMessage(ref msg);
      }
    }

    private IntPtr WndProcHandler(IntPtr handle, WindowMessage message, IntPtr wParam, IntPtr lParam)
    {
      IntPtr num = this.WindowProcedure(handle, message, wParam, lParam);
      if (num == WinInputBase.Unhandled)
        return Functions.CallWindowProc(this.OldWndProc, handle, message, wParam, lParam);
      else
        return num;
    }

    protected virtual IntPtr WindowProcedure(IntPtr handle, WindowMessage message, IntPtr wParam, IntPtr lParam)
    {
      return WinInputBase.Unhandled;
    }

    protected abstract void CreateDrivers();

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool manual)
    {
      if (this.Disposed)
        return;
      if (manual && this.Native != null)
      {
        this.Native.Close();
        this.Native.Dispose();
      }
      this.Disposed = true;
    }
  }
}
