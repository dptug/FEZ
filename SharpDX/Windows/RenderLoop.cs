// Type: SharpDX.Windows.RenderLoop
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX;
using SharpDX.Win32;
using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SharpDX.Windows
{
  public class RenderLoop
  {
    public static bool IsIdle
    {
      get
      {
        Win32Native.NativeMessage lpMsg;
        return Win32Native.PeekMessage(out lpMsg, IntPtr.Zero, 0, 0, 0) == 0;
      }
    }

    public static bool UseCustomDoEvents { get; set; }

    private RenderLoop()
    {
    }

    public static void Run(ApplicationContext context, RenderLoop.RenderCallback renderCallback)
    {
      RenderLoop.Run((Control) context.MainForm, renderCallback);
    }

    public static void Run(Control form, RenderLoop.RenderCallback renderCallback)
    {
      new RenderLoop.ProxyNativeWindow(form).Run(renderCallback);
    }

    public delegate void RenderCallback();

    private class ProxyNativeWindow : IMessageFilter
    {
      private readonly Control _form;
      private readonly IntPtr _windowHandle;
      private bool _isAlive;

      public ProxyNativeWindow(Control form)
      {
        this._form = form;
        this._windowHandle = form.Handle;
        this._form.Disposed += new EventHandler(this._form_Disposed);
        MessageFilterHook.AddMessageFilter(this._windowHandle, (IMessageFilter) this);
        this._isAlive = true;
      }

      private void _form_Disposed(object sender, EventArgs e)
      {
        this._isAlive = false;
      }

      public void Run(RenderLoop.RenderCallback renderCallback)
      {
        this._form.Show();
        while (this._isAlive)
        {
          if (RenderLoop.UseCustomDoEvents)
          {
            Win32Native.NativeMessage lpMsg;
            while (Win32Native.PeekMessage(out lpMsg, this._windowHandle, 0, 0, 0) != 0)
            {
              if (Win32Native.GetMessage(out lpMsg, this._windowHandle, 0, 0) == -1)
              {
                throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "An error happened in rendering loop while processing windows messages. Error: {0}", new object[1]
                {
                  (object) Marshal.GetLastWin32Error()
                }));
              }
              else
              {
                Win32Native.TranslateMessage(ref lpMsg);
                Win32Native.DispatchMessage(ref lpMsg);
              }
            }
          }
          else
            Application.DoEvents();
          if (this._isAlive)
            renderCallback();
        }
        this._form.Disposed -= new EventHandler(this._form_Disposed);
        MessageFilterHook.RemoveMessageFilter(this._windowHandle, (IMessageFilter) this);
      }

      public bool PreFilterMessage(ref Message m)
      {
        if (m.Msg == 130)
          this._isAlive = false;
        return false;
      }
    }
  }
}
