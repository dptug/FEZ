// Type: Microsoft.Xna.Framework.Net.GamerStates
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

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
