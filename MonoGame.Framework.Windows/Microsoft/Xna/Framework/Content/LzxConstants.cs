// Type: Microsoft.Xna.Framework.Content.LzxConstants
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System.Runtime.InteropServices;

namespace Microsoft.Xna.Framework.Content
{
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct LzxConstants
  {
    public const ushort MIN_MATCH = (ushort) 2;
    public const ushort MAX_MATCH = (ushort) 257;
    public const ushort NUM_CHARS = (ushort) 256;
    public const ushort PRETREE_NUM_ELEMENTS = (ushort) 20;
    public const ushort ALIGNED_NUM_ELEMENTS = (ushort) 8;
    public const ushort NUM_PRIMARY_LENGTHS = (ushort) 7;
    public const ushort NUM_SECONDARY_LENGTHS = (ushort) 249;
    public const ushort PRETREE_MAXSYMBOLS = (ushort) 20;
    public const ushort PRETREE_TABLEBITS = (ushort) 6;
    public const ushort MAINTREE_MAXSYMBOLS = (ushort) 656;
    public const ushort MAINTREE_TABLEBITS = (ushort) 12;
    public const ushort LENGTH_MAXSYMBOLS = (ushort) 250;
    public const ushort LENGTH_TABLEBITS = (ushort) 12;
    public const ushort ALIGNED_MAXSYMBOLS = (ushort) 8;
    public const ushort ALIGNED_TABLEBITS = (ushort) 7;
    public const ushort LENTABLE_SAFETY = (ushort) 64;

    public enum BLOCKTYPE
    {
      INVALID,
      VERBATIM,
      ALIGNED,
      UNCOMPRESSED,
    }
  }
}
