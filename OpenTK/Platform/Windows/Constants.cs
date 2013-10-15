// Type: OpenTK.Platform.Windows.Constants
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Platform.Windows
{
  internal static class Constants
  {
    internal static readonly IntPtr MESSAGE_ONLY = new IntPtr(-3);
    internal const int KEYBOARD_OVERRUN_MAKE_CODE = 255;
    internal const int WA_INACTIVE = 0;
    internal const int WA_ACTIVE = 1;
    internal const int WA_CLICKACTIVE = 2;
    internal const int WM_NULL = 0;
    internal const int WM_CREATE = 1;
    internal const int WM_DESTROY = 2;
    internal const int WM_MOVE = 3;
    internal const int WM_SIZE = 5;
    internal const int WM_ACTIVATE = 6;
    internal const int WM_SETFOCUS = 7;
    internal const int WM_KILLFOCUS = 8;
    internal const int WM_ENABLE = 10;
    internal const int WM_SETREDRAW = 11;
    internal const int WM_SETTEXT = 12;
    internal const int WM_GETTEXT = 13;
    internal const int WM_GETTEXTLENGTH = 14;
    internal const int WM_PAINT = 15;
    internal const int WM_CLOSE = 16;
    internal const int WM_QUERYENDSESSION = 17;
    internal const int WM_QUERYOPEN = 19;
    internal const int WM_ENDSESSION = 22;
    internal const int WM_QUIT = 18;
    internal const int WM_ERASEBKGND = 20;
    internal const int WM_SYSCOLORCHANGE = 21;
    internal const int WM_SHOWWINDOW = 24;
    internal const int WM_WININICHANGE = 26;
    internal const int WM_SETTINGCHANGE = 26;
    internal const int WM_DEVMODECHANGE = 27;
    internal const int WM_ACTIVATEAPP = 28;
    internal const int WM_FONTCHANGE = 29;
    internal const int WM_TIMECHANGE = 30;
    internal const int WM_CANCELMODE = 31;
    internal const int WM_SETCURSOR = 32;
    internal const int WM_MOUSEACTIVATE = 33;
    internal const int WM_CHILDACTIVATE = 34;
    internal const int WM_QUEUESYNC = 35;
    internal const int WM_GETMINMAXINFO = 36;
    internal const int WM_WINDOWPOSCHANGING = 70;
    internal const int WM_WINDOWPOSCHANGED = 71;
    internal const int WM_INPUT = 255;
    internal const int WM_KEYDOWN = 256;
    internal const int WM_KEYUP = 257;
    internal const int WM_SYSKEYDOWN = 260;
    internal const int WM_SYSKEYUP = 261;
    internal const int WM_COMMAND = 273;
    internal const int WM_SYSCOMMAND = 274;
    internal const int WM_ENTERIDLE = 289;
    internal const byte PFD_TYPE_RGBA = (byte) 0;
    internal const byte PFD_TYPE_COLORINDEX = (byte) 1;
    internal const byte PFD_MAIN_PLANE = (byte) 0;
    internal const byte PFD_OVERLAY_PLANE = (byte) 1;
    internal const byte PFD_UNDERLAY_PLANE = (byte) 255;
    internal const int DM_BITSPERPEL = 262144;
    internal const int DM_PELSWIDTH = 524288;
    internal const int DM_PELSHEIGHT = 1048576;
    internal const int DM_DISPLAYFLAGS = 2097152;
    internal const int DM_DISPLAYFREQUENCY = 4194304;
    internal const int DISP_CHANGE_SUCCESSFUL = 0;
    internal const int DISP_CHANGE_RESTART = 1;
    internal const int DISP_CHANGE_FAILED = -1;
    internal const int ENUM_REGISTRY_SETTINGS = -2;
    internal const int ENUM_CURRENT_SETTINGS = -1;

    static Constants()
    {
    }
  }
}
