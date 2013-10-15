// Type: Microsoft.Xna.Framework.WindowsHelperAccessibilityKeys
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System.Runtime.InteropServices;

namespace Microsoft.Xna.Framework
{
  public class WindowsHelperAccessibilityKeys
  {
    private static bool StartupAccessibilitySet = false;
    private static uint SKEYSize = 8U;
    private static uint FKEYSize = 24U;
    private const uint SPI_GETFILTERKEYS = 50U;
    private const uint SPI_SETFILTERKEYS = 51U;
    private const uint SPI_GETTOGGLEKEYS = 52U;
    private const uint SPI_SETTOGGLEKEYS = 53U;
    private const uint SPI_GETSTICKYKEYS = 58U;
    private const uint SPI_SETSTICKYKEYS = 59U;
    private const uint SKF_STICKYKEYSON = 1U;
    private const uint TKF_TOGGLEKEYSON = 1U;
    private const uint SKF_CONFIRMHOTKEY = 8U;
    private const uint SKF_HOTKEYACTIVE = 4U;
    private const uint TKF_CONFIRMHOTKEY = 8U;
    private const uint TKF_HOTKEYACTIVE = 4U;
    private const uint FKF_CONFIRMHOTKEY = 8U;
    private const uint FKF_HOTKEYACTIVE = 4U;
    private const uint FKF_FILTERKEYSON = 1U;
    private static WindowsHelperAccessibilityKeys.SKEY StartupStickyKeys;
    private static WindowsHelperAccessibilityKeys.SKEY StartupToggleKeys;
    private static WindowsHelperAccessibilityKeys.FILTERKEY StartupFilterKeys;

    static WindowsHelperAccessibilityKeys()
    {
    }

    [DllImport("user32.dll")]
    private static bool SystemParametersInfo(uint action, uint param, ref WindowsHelperAccessibilityKeys.SKEY vparam, uint init);

    [DllImport("user32.dll")]
    private static bool SystemParametersInfo(uint action, uint param, ref WindowsHelperAccessibilityKeys.FILTERKEY vparam, uint init);

    public static void AllowAccessibilityShortcutKeys(bool bAllowKeys)
    {
      if (!WindowsHelperAccessibilityKeys.StartupAccessibilitySet)
      {
        WindowsHelperAccessibilityKeys.StartupStickyKeys.cbSize = WindowsHelperAccessibilityKeys.SKEYSize;
        WindowsHelperAccessibilityKeys.StartupToggleKeys.cbSize = WindowsHelperAccessibilityKeys.SKEYSize;
        WindowsHelperAccessibilityKeys.StartupFilterKeys.cbSize = WindowsHelperAccessibilityKeys.FKEYSize;
        WindowsHelperAccessibilityKeys.SystemParametersInfo(58U, WindowsHelperAccessibilityKeys.SKEYSize, ref WindowsHelperAccessibilityKeys.StartupStickyKeys, 0U);
        WindowsHelperAccessibilityKeys.SystemParametersInfo(52U, WindowsHelperAccessibilityKeys.SKEYSize, ref WindowsHelperAccessibilityKeys.StartupToggleKeys, 0U);
        WindowsHelperAccessibilityKeys.SystemParametersInfo(50U, WindowsHelperAccessibilityKeys.FKEYSize, ref WindowsHelperAccessibilityKeys.StartupFilterKeys, 0U);
        WindowsHelperAccessibilityKeys.StartupAccessibilitySet = true;
      }
      if (bAllowKeys)
      {
        WindowsHelperAccessibilityKeys.SystemParametersInfo(59U, WindowsHelperAccessibilityKeys.SKEYSize, ref WindowsHelperAccessibilityKeys.StartupStickyKeys, 0U);
        WindowsHelperAccessibilityKeys.SystemParametersInfo(53U, WindowsHelperAccessibilityKeys.SKEYSize, ref WindowsHelperAccessibilityKeys.StartupToggleKeys, 0U);
        WindowsHelperAccessibilityKeys.SystemParametersInfo(51U, WindowsHelperAccessibilityKeys.FKEYSize, ref WindowsHelperAccessibilityKeys.StartupFilterKeys, 0U);
      }
      else
      {
        WindowsHelperAccessibilityKeys.SKEY vparam1 = WindowsHelperAccessibilityKeys.StartupStickyKeys;
        if (((int) vparam1.dwFlags & 1) == 0)
        {
          vparam1.dwFlags &= 4294967291U;
          vparam1.dwFlags &= 4294967287U;
          WindowsHelperAccessibilityKeys.SystemParametersInfo(59U, WindowsHelperAccessibilityKeys.SKEYSize, ref vparam1, 0U);
        }
        WindowsHelperAccessibilityKeys.SKEY vparam2 = WindowsHelperAccessibilityKeys.StartupToggleKeys;
        if (((int) vparam2.dwFlags & 1) == 0)
        {
          vparam2.dwFlags &= 4294967291U;
          vparam2.dwFlags &= 4294967287U;
          WindowsHelperAccessibilityKeys.SystemParametersInfo(53U, WindowsHelperAccessibilityKeys.SKEYSize, ref vparam2, 0U);
        }
        WindowsHelperAccessibilityKeys.FILTERKEY vparam3 = WindowsHelperAccessibilityKeys.StartupFilterKeys;
        if (((int) vparam3.dwFlags & 1) != 0)
          return;
        vparam3.dwFlags &= 4294967291U;
        vparam3.dwFlags &= 4294967287U;
        WindowsHelperAccessibilityKeys.SystemParametersInfo(51U, WindowsHelperAccessibilityKeys.FKEYSize, ref vparam3, 0U);
      }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct SKEY
    {
      public uint cbSize;
      public uint dwFlags;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct FILTERKEY
    {
      public uint cbSize;
      public uint dwFlags;
      public uint iWaitMSec;
      public uint iDelayMSec;
      public uint iRepeatMSec;
      public uint iBounceMSec;
    }
  }
}
