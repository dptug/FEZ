// Type: OpenTK.Platform.Windows.RawKeyboard
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

namespace OpenTK.Platform.Windows
{
  internal struct RawKeyboard
  {
    internal short MakeCode;
    internal RawInputKeyboardDataFlags Flags;
    private ushort Reserved;
    internal VirtualKeys VKey;
    internal int Message;
    internal int ExtraInformation;
  }
}
