// Type: SharpDX.Win32.MessageFilterHook
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SharpDX.Win32
{
  public class MessageFilterHook
  {
    private static readonly Dictionary<IntPtr, MessageFilterHook> RegisteredHooks = new Dictionary<IntPtr, MessageFilterHook>();
    private readonly IntPtr defaultWndProc;
    private readonly IntPtr hwnd;
    private readonly Win32Native.WndProc newWndProc;
    private readonly IntPtr newWndProcPtr;
    private List<IMessageFilter> currentFilters;
    private bool isDisposed;

    static MessageFilterHook()
    {
    }

    private MessageFilterHook(IntPtr hwnd)
    {
      this.currentFilters = new List<IMessageFilter>();
      this.hwnd = hwnd;
      this.defaultWndProc = Win32Native.GetWindowLong(new HandleRef((object) this, hwnd), Win32Native.WindowLongType.WndProc);
      this.newWndProc = new Win32Native.WndProc(this.WndProc);
      this.newWndProcPtr = Marshal.GetFunctionPointerForDelegate((Delegate) this.newWndProc);
      Win32Native.SetWindowLong(new HandleRef((object) this, hwnd), Win32Native.WindowLongType.WndProc, this.newWndProcPtr);
    }

    public static void AddMessageFilter(IntPtr hwnd, IMessageFilter messageFilter)
    {
      lock (MessageFilterHook.RegisteredHooks)
      {
        hwnd = MessageFilterHook.GetSafeWindowHandle(hwnd);
        MessageFilterHook local_0;
        if (!MessageFilterHook.RegisteredHooks.TryGetValue(hwnd, out local_0))
        {
          local_0 = new MessageFilterHook(hwnd);
          MessageFilterHook.RegisteredHooks.Add(hwnd, local_0);
        }
        local_0.AddMessageMilter(messageFilter);
      }
    }

    public static void RemoveMessageFilter(IntPtr hwnd, IMessageFilter messageFilter)
    {
      lock (MessageFilterHook.RegisteredHooks)
      {
        hwnd = MessageFilterHook.GetSafeWindowHandle(hwnd);
        MessageFilterHook local_0;
        if (!MessageFilterHook.RegisteredHooks.TryGetValue(hwnd, out local_0))
          return;
        local_0.RemoveMessageFilter(messageFilter);
        if (!local_0.isDisposed)
          return;
        MessageFilterHook.RegisteredHooks.Remove(hwnd);
        local_0.RestoreWndProc();
      }
    }

    private void AddMessageMilter(IMessageFilter filter)
    {
      List<IMessageFilter> list = new List<IMessageFilter>((IEnumerable<IMessageFilter>) this.currentFilters);
      if (!list.Contains(filter))
        list.Add(filter);
      this.currentFilters = list;
    }

    private void RemoveMessageFilter(IMessageFilter filter)
    {
      List<IMessageFilter> list = new List<IMessageFilter>((IEnumerable<IMessageFilter>) this.currentFilters);
      list.Remove(filter);
      if (list.Count == 0)
      {
        this.isDisposed = true;
        this.RestoreWndProc();
      }
      this.currentFilters = list;
    }

    private void RestoreWndProc()
    {
      if (!(Win32Native.GetWindowLong(new HandleRef((object) this, this.hwnd), Win32Native.WindowLongType.WndProc) == this.newWndProcPtr))
        return;
      Win32Native.SetWindowLong(new HandleRef((object) this, this.hwnd), Win32Native.WindowLongType.WndProc, this.defaultWndProc);
    }

    private IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam)
    {
      if (this.isDisposed)
      {
        this.RestoreWndProc();
      }
      else
      {
        Message m = new Message()
        {
          HWnd = this.hwnd,
          LParam = lParam,
          Msg = msg,
          WParam = wParam
        };
        foreach (IMessageFilter messageFilter in this.currentFilters)
        {
          if (messageFilter.PreFilterMessage(ref m))
            return m.Result;
        }
      }
      return Win32Native.CallWindowProc(this.defaultWndProc, hWnd, msg, wParam, lParam);
    }

    private static IntPtr GetSafeWindowHandle(IntPtr hwnd)
    {
      if (!(hwnd == IntPtr.Zero))
        return hwnd;
      else
        return Process.GetCurrentProcess().MainWindowHandle;
    }
  }
}
