// Type: CommunityExpressNS.AvatarImageLoaded_t
// Assembly: CommunityExpress, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3B3F745C-AA2A-4DDF-AA8A-F5898AF84B8D
// Assembly location: F:\Program Files (x86)\FEZ\CommunityExpress.dll

using System.Runtime.InteropServices;

namespace CommunityExpressNS
{
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  internal struct AvatarImageLoaded_t
  {
    public ulong m_steamID;
    public int m_iImage;
    public int m_iWide;
    public int m_iTall;
  }
}
