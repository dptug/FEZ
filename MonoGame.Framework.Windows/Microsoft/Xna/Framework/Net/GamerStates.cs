// Type: Microsoft.Xna.Framework.Net.GamerStates
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System;

namespace Microsoft.Xna.Framework.Net
{
  [Flags]
  public enum GamerStates
  {
    Local = 1,
    Host = 16,
    HasVoice = 256,
    Guest = 4096,
    MutedByLocalUser = 65536,
    PrivateSlot = 1048576,
    Ready = 16777216,
  }
}
