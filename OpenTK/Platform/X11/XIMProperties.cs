// Type: OpenTK.Platform.X11.XIMProperties
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Platform.X11
{
  [Flags]
  internal enum XIMProperties
  {
    XIMPreeditArea = 1,
    XIMPreeditCallbacks = 2,
    XIMPreeditPosition = 4,
    XIMPreeditNothing = 8,
    XIMPreeditNone = 16,
    XIMStatusArea = 256,
    XIMStatusCallbacks = 512,
    XIMStatusNothing = 1024,
    XIMStatusNone = 2048,
  }
}
