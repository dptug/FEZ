// Type: OpenTK.Platform.X11.Constants
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System.Runtime.InteropServices;

namespace OpenTK.Platform.X11
{
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  internal struct Constants
  {
    public const int QueuedAlready = 0;
    public const int QueuedAfterReading = 1;
    public const int QueuedAfterFlush = 2;
    public const int CopyFromParent = 0;
    public const int CWX = 1;
    public const int InputOutput = 1;
    public const int InputOnly = 2;
    public const string XA_WIN_PROTOCOLS = "_WIN_PROTOCOLS";
    public const string XA_WIN_ICONS = "_WIN_ICONS";
    public const string XA_WIN_WORKSPACE = "_WIN_WORKSPACE";
    public const string XA_WIN_WORKSPACE_COUNT = "_WIN_WORKSPACE_COUNT";
    public const string XA_WIN_WORKSPACE_NAMES = "_WIN_WORKSPACE_NAMES";
    public const string XA_WIN_LAYER = "_WIN_LAYER";
    public const string XA_WIN_STATE = "_WIN_STATE";
    public const string XA_WIN_HINTS = "_WIN_HINTS";
    public const string XA_WIN_WORKAREA = "_WIN_WORKAREA";
    public const string XA_WIN_CLIENT_LIST = "_WIN_CLIENT_LIST";
    public const string XA_WIN_APP_STATE = "_WIN_APP_STATE";
    public const string XA_WIN_EXPANDED_SIZE = "_WIN_EXPANDED_SIZE";
    public const string XA_WIN_CLIENT_MOVING = "_WIN_CLIENT_MOVING";
    public const string XA_WIN_SUPPORTING_WM_CHECK = "_WIN_SUPPORTING_WM_CHECK";
  }
}
