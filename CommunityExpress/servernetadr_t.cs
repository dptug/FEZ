// Type: CommunityExpressNS.servernetadr_t
// Assembly: CommunityExpress, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3B3F745C-AA2A-4DDF-AA8A-F5898AF84B8D
// Assembly location: F:\Program Files (x86)\FEZ\CommunityExpress.dll

using System.Runtime.InteropServices;

namespace CommunityExpressNS
{
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  internal struct servernetadr_t
  {
    public ushort m_usConnectionPort;
    public ushort m_usQueryPort;
    public uint m_unIP;
  }
}
