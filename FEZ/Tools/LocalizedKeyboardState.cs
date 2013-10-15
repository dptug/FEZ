// Type: FezGame.Tools.LocalizedKeyboardState
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using Microsoft.Xna.Framework.Input;
using System;
using System.Runtime.InteropServices;

namespace FezGame.Tools
{
  public struct LocalizedKeyboardState
  {
    internal const uint KLF_NOTELLSHELL = 128U;
    public readonly KeyboardState Native;

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static uint MapVirtualKeyEx(uint key, LocalizedKeyboardState.MAPVK mappingType, IntPtr keyboardLayout);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static IntPtr LoadKeyboardLayout(string keyboardLayoutID, uint flags);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static bool UnloadKeyboardLayout(IntPtr handle);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static IntPtr GetKeyboardLayout(IntPtr threadId);

    public static Keys USEnglishToLocal(Keys key)
    {
      return (Keys) LocalizedKeyboardState.MapVirtualKeyEx(LocalizedKeyboardState.MapVirtualKeyEx((uint) key, LocalizedKeyboardState.MAPVK.VK_TO_VSC, LocalizedKeyboardState.KeyboardLayout.US_English.Handle), LocalizedKeyboardState.MAPVK.VSC_TO_VK, LocalizedKeyboardState.KeyboardLayout.Active.Handle);
    }

    public class KeyboardLayout : IDisposable
    {
      public static LocalizedKeyboardState.KeyboardLayout US_English = new LocalizedKeyboardState.KeyboardLayout("00000409");
      public readonly IntPtr Handle;

      public bool IsDisposed { get; private set; }

      public static LocalizedKeyboardState.KeyboardLayout Active
      {
        get
        {
          return new LocalizedKeyboardState.KeyboardLayout(LocalizedKeyboardState.GetKeyboardLayout(IntPtr.Zero));
        }
      }

      static KeyboardLayout()
      {
      }

      public KeyboardLayout(IntPtr handle)
      {
        this.Handle = handle;
      }

      public KeyboardLayout(string keyboardLayoutID)
        : this(LocalizedKeyboardState.LoadKeyboardLayout(keyboardLayoutID, 128U))
      {
      }

      ~KeyboardLayout()
      {
        this.Dispose(false);
      }

      public void Dispose()
      {
        this.Dispose(true);
        GC.SuppressFinalize((object) this);
      }

      private void Dispose(bool disposing)
      {
        if (this.IsDisposed)
          return;
        LocalizedKeyboardState.UnloadKeyboardLayout(this.Handle);
        this.IsDisposed = true;
      }
    }

    internal enum MAPVK : uint
    {
      VK_TO_VSC,
      VSC_TO_VK,
      VK_TO_CHAR,
    }
  }
}
