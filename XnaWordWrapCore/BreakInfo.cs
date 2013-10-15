// Type: Microsoft.Xna.Framework.Graphics.Localization.BreakInfo
// Assembly: XnaWordWrapCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 278B215C-3D4B-45DC-8E40-65DA36A71393
// Assembly location: F:\Program Files (x86)\FEZ\XnaWordWrapCore.dll

using System;

namespace Microsoft.Xna.Framework.Graphics.Localization
{
  public struct BreakInfo
  {
    private uint m_Character;
    private bool m_IsNonBeginningCharacter;
    private bool m_IsNonEndingCharacter;

    public uint Character
    {
      get
      {
        return this.m_Character;
      }
    }

    public bool IsNonBeginningCharacter
    {
      get
      {
        return this.m_IsNonBeginningCharacter;
      }
    }

    public bool IsNonEndingCharacter
    {
      get
      {
        return this.m_IsNonEndingCharacter;
      }
    }

    public BreakInfo(uint character, bool isNonBeginningCharacter, bool isNonEndingCharacter)
    {
      if (character > 1114111U)
        throw new ArgumentException("Invalid code point.");
      this.m_Character = character;
      this.m_IsNonBeginningCharacter = isNonBeginningCharacter;
      this.m_IsNonEndingCharacter = isNonEndingCharacter;
    }
  }
}
