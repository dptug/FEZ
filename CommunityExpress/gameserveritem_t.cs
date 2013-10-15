// Type: CommunityExpressNS.gameserveritem_t
// Assembly: CommunityExpress, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3B3F745C-AA2A-4DDF-AA8A-F5898AF84B8D
// Assembly location: F:\Program Files (x86)\FEZ\CommunityExpress.dll

using System.Runtime.InteropServices;

namespace CommunityExpressNS
{
  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  internal struct gameserveritem_t
  {
    public servernetadr_t m_NetAdr;
    public int m_nPing;
    public byte m_bHadSuccessfulResponse;
    public byte m_bDoNotRefresh;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public char[] m_szGameDir;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public char[] m_szMap;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
    public char[] m_szGameDescription;
    public uint m_nAppID;
    public int m_nPlayers;
    public int m_nMaxPlayers;
    public int m_nBotPlayers;
    public byte m_bPassword;
    public byte m_bSecure;
    public uint m_ulTimeLastPlayed;
    public int m_nServerVersion;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
    public char[] m_szServerName;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
    public char[] m_szGameTags;
    public ulong m_steamID;
  }
}
