// Type: CommunityExpressNS.User
// Assembly: CommunityExpress, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3B3F745C-AA2A-4DDF-AA8A-F5898AF84B8D
// Assembly location: F:\Program Files (x86)\FEZ\CommunityExpress.dll

using System;
using System.Net;
using System.Runtime.InteropServices;

namespace CommunityExpressNS
{
  public class User
  {
    private const int AuthTicketSizeMax = 2048;
    private IntPtr _user;
    private Friends _friends;
    private uint _serverIP;
    private ushort _serverPort;
    private OnUserEncryptedAppTicketCreated _onUserEncryptedAppTicketCreated;

    public bool IsBehindNAT
    {
      get
      {
        return User.SteamUnityAPI_SteamUser_BIsBehindNAT(this._user);
      }
    }

    public bool LoggedOn
    {
      get
      {
        return User.SteamUnityAPI_SteamUser_BLoggedOn(this._user);
      }
    }

    public int HSteamUser
    {
      get
      {
        return User.SteamUnityAPI_SteamUser_GetHSteamUser(this._user);
      }
    }

    public SteamID SteamID
    {
      get
      {
        return new SteamID(User.SteamUnityAPI_SteamUser_GetSteamID(this._user));
      }
    }

    public string PersonaName
    {
      get
      {
        return User.SteamUnityAPI_GetPersonaNameByID(this.SteamID.ToUInt64());
      }
    }

    public Image SmallAvatar
    {
      get
      {
        return this._friends.GetSmallFriendAvatar(this.SteamID);
      }
    }

    public Image MediumAvatar
    {
      get
      {
        return this._friends.GetMediumFriendAvatar(this.SteamID);
      }
    }

    public Image LargeAvatar
    {
      get
      {
        return this._friends.GetLargeFriendAvatar(this.SteamID, (OnLargeAvatarReceived) null);
      }
    }

    internal User()
    {
      this._user = User.SteamUnityAPI_SteamUser();
      this._friends = new Friends();
    }

    ~User()
    {
      this.OnDisconnect();
    }

    [DllImport("CommunityExpressSW")]
    private static IntPtr SteamUnityAPI_SteamUser();

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_SteamUser_BLoggedOn(IntPtr user);

    [DllImport("CommunityExpressSW")]
    private static int SteamUnityAPI_SteamUser_GetHSteamUser(IntPtr user);

    [DllImport("CommunityExpressSW")]
    private static ulong SteamUnityAPI_SteamUser_GetSteamID(IntPtr user);

    [DllImport("CommunityExpressSW")]
    private static int SteamUnityAPI_SteamUser_InitiateGameConnection(IntPtr user, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U1)] byte[] authTicket, int authTicketMaxSize, ulong serverSteamID, uint serverIP, ushort serverPort, bool isSecure);

    [DllImport("CommunityExpressSW")]
    private static void SteamUnityAPI_SteamUser_TerminateGameConnection(IntPtr user, uint serverIP, ushort serverPort);

    [DllImport("CommunityExpressSW")]
    private static EUserHasLicenseForAppResult SteamUnityAPI_SteamUser_UserHasLicenseForApp(IntPtr user, ulong steamID, uint appID);

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_SteamUser_BIsBehindNAT(IntPtr user);

    [DllImport("CommunityExpressSW")]
    private static void SteamUnityAPI_SteamUser_AdvertiseGame(IntPtr user, ulong gameServerSteamID, uint serverIP, ushort port);

    [DllImport("CommunityExpressSW")]
    private static ulong SteamUnityAPI_SteamUser_RequestEncryptedAppTicket(IntPtr user, IntPtr dataToInclude, int dataLength);

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_SteamUser_GetEncryptedAppTicket(IntPtr user, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U1)] byte[] ticket, int maxTicket, out uint ticketSize);

    [DllImport("CommunityExpressSW")]
    [return: MarshalAs(UnmanagedType.LPStr)]
    private static string SteamUnityAPI_GetPersonaNameByID(ulong steamID);

    public bool InitiateClientAuthentication(out byte[] authTicket, SteamID steamIDServer, IPAddress ipServer, ushort serverPort, bool isSecure)
    {
      byte[] addressBytes = ipServer.GetAddressBytes();
      this._serverIP = (uint) ((int) addressBytes[0] << 24 | (int) addressBytes[1] << 16 | (int) addressBytes[2] << 8) | (uint) addressBytes[3];
      this._serverPort = serverPort;
      byte[] authTicket1 = new byte[2048];
      int length = User.SteamUnityAPI_SteamUser_InitiateGameConnection(this._user, authTicket1, 2048, steamIDServer.ToUInt64(), this._serverIP, this._serverPort, isSecure);
      if (length > 0)
      {
        authTicket = new byte[length];
        for (int index = 0; index < length; ++index)
          authTicket[index] = authTicket1[index];
        return true;
      }
      else
      {
        authTicket = (byte[]) null;
        return false;
      }
    }

    public void OnDisconnect()
    {
      if ((int) this._serverIP == 0)
        return;
      User.SteamUnityAPI_SteamUser_TerminateGameConnection(this._user, this._serverIP, this._serverPort);
    }

    public EUserHasLicenseForAppResult UserHasLicenseForApp(uint appID)
    {
      return User.SteamUnityAPI_SteamUser_UserHasLicenseForApp(this._user, this.SteamID.ToUInt64(), appID);
    }

    public EUserHasLicenseForAppResult UserHasLicenseForApp(SteamID steamID, uint appID)
    {
      return User.SteamUnityAPI_SteamUser_UserHasLicenseForApp(this._user, steamID.ToUInt64(), appID);
    }

    public void AdvertiseGame(SteamID gameServerSteamID, IPAddress ip, ushort port)
    {
      byte[] addressBytes = ip.GetAddressBytes();
      uint serverIP = (uint) ((int) addressBytes[0] << 24 | (int) addressBytes[1] << 16 | (int) addressBytes[2] << 8) | (uint) addressBytes[3];
      User.SteamUnityAPI_SteamUser_AdvertiseGame(this._user, gameServerSteamID.ToUInt64(), serverIP, port);
    }

    public void RequestEncryptedAppTicket(byte[] dataToInclude, OnUserEncryptedAppTicketCreated onUserEncryptedAppTicketCreated)
    {
      this._onUserEncryptedAppTicketCreated = onUserEncryptedAppTicketCreated;
      IntPtr num = Marshal.AllocHGlobal(dataToInclude.Length);
      Marshal.Copy(dataToInclude, 0, num, dataToInclude.Length);
      CommunityExpress.Instance.AddUserGetEncryptedAppTicketCallback(User.SteamUnityAPI_SteamUser_RequestEncryptedAppTicket(this._user, num, dataToInclude.Length), new OnUserGetEncryptedAppTicketFromSteam(this.OnGetEncryptedAppTicketFromSteam));
      Marshal.FreeHGlobal(num);
    }

    private void OnGetEncryptedAppTicketFromSteam()
    {
      byte[] ticket1 = (byte[]) null;
      byte[] ticket2 = new byte[2048];
      uint ticketSize;
      if (User.SteamUnityAPI_SteamUser_GetEncryptedAppTicket(this._user, ticket2, 2048, out ticketSize))
      {
        ticket1 = new byte[(IntPtr) ticketSize];
        for (int index = 0; (long) index < (long) ticketSize; ++index)
          ticket1[index] = ticket2[index];
      }
      this._onUserEncryptedAppTicketCreated(ticket1);
    }

    public void GetLargeAvatar(OnLargeAvatarReceived largeAvatarReceivedCallback)
    {
      this._friends.GetLargeFriendAvatar(this.SteamID, largeAvatarReceivedCallback);
    }
  }
}
