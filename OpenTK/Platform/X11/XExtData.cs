// Type: OpenTK.Platform.X11.XExtData
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK.Platform.X11
{
  internal class XExtData
  {
    private int number;
    private XExtData next;
    private XExtData.FreePrivateDelegate FreePrivate;
    private IntPtr private_data;

    private delegate int FreePrivateDelegate(XExtData extension);
  }
}
