// Type: OpenTK.Platform.Windows.WindowInfo
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

namespace OpenTK.Platform.Windows
{
  internal struct WindowInfo
  {
    public int Size;
    public Win32Rectangle Window;
    public Win32Rectangle Client;
    public WindowStyle Style;
    public ExtendedWindowStyle ExStyle;
    public int WindowStatus;
    public uint WindowBordersX;
    public uint WindowBordersY;
    public int WindowType;
    public short CreatorVersion;
  }
}
