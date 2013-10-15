// Type: CommunityExpressNS.CommunityExpress
// Assembly: CommunityExpress, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3B3F745C-AA2A-4DDF-AA8A-F5898AF84B8D
// Assembly location: F:\Program Files (x86)\FEZ\CommunityExpress.dll

using CommunityExpressNS.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace CommunityExpressNS
{
  public sealed class CommunityExpress
  {
    private static readonly CommunityExpress _instance = new CommunityExpress();
    private List<ulong> _gameserverUserStatsReceivedCallHandles = new List<ulong>();
    private List<OnUserStatsReceivedFromSteam> _gameserverUserStatsReceivedCallbacks = new List<OnUserStatsReceivedFromSteam>();
    public const uint k_uAppIdInvalid = 0U;
    private CommunityExpress.OnChallengeResponseFromSteam _challengeResponse;
    private CommunityExpress.OnSteamAPIDebugTextHook _steamAPIDebugTextHook;
    private CommunityExpress.OnLog _logger;
    private bool _shutdown;
    private User _user;
    private GameServer _gameserver;
    private Friends _friends;
    private Groups _groups;
    private Stats _userStats;
    private Friends _serverFriends;
    private Achievements _achievements;
    private Leaderboards _leaderboards;
    private Matchmaking _matchmaking;
    private RemoteStorage _remoteStorage;
    private Networking _networking;
    private InGamePurchasing _inGamePurchasing;
    private SteamWebAPI _steamWebAPI;
    private ulong _userGetEncryptedAppTicketCallHandle;
    private OnUserGetEncryptedAppTicketFromSteam _userGetEncryptedAppTicketCallback;
    private ulong _lobbyCreatedCallHandle;
    private OnMatchmakingLobbyCreatedBySteam _lobbyCreatedCallback;
    private ulong _lobbyListReceivedCallHandle;
    private OnMatchmakingLobbyListReceivedFromSteam _lobbyListReceivedCallback;
    private ulong _lobbyJoinedCallHandle;
    private OnMatchmakingLobbyJoinedFromSteam _lobbyJoinedCallback;

    public static CommunityExpress Instance
    {
      get
      {
        return CommunityExpress._instance;
      }
    }

    public uint AppID
    {
      get
      {
        return CommunityExpress.SteamUnityAPI_SteamUtils_GetAppID();
      }
    }

    public User User
    {
      get
      {
        if (this._user == null)
          this._user = new User();
        return this._user;
      }
    }

    public GameServer GameServer
    {
      get
      {
        if (this._gameserver == null)
          this._gameserver = new GameServer();
        return this._gameserver;
      }
    }

    public Friends Friends
    {
      get
      {
        if (this._friends == null)
          this._friends = new Friends();
        return this._friends;
      }
    }

    public Groups Groups
    {
      get
      {
        if (this._groups == null)
          this._groups = new Groups(this.Friends);
        return this._groups;
      }
    }

    public Stats UserStats
    {
      get
      {
        if (this._userStats == null)
        {
          this._userStats = new Stats();
          this._userStats.Init((SteamID) null, false);
        }
        return this._userStats;
      }
    }

    public Achievements UserAchievements
    {
      get
      {
        if (this._achievements == null)
        {
          this._achievements = new Achievements();
          this._achievements.Init((SteamID) null, false);
        }
        return this._achievements;
      }
    }

    public Leaderboards Leaderboards
    {
      get
      {
        if (this._leaderboards == null)
          this._leaderboards = new Leaderboards();
        return this._leaderboards;
      }
    }

    public Matchmaking Matchmaking
    {
      get
      {
        if (this._matchmaking == null)
          this._matchmaking = new Matchmaking();
        return this._matchmaking;
      }
    }

    public RemoteStorage RemoteStorage
    {
      get
      {
        if (this._remoteStorage == null)
          this._remoteStorage = new RemoteStorage();
        return this._remoteStorage;
      }
    }

    public Networking Networking
    {
      get
      {
        if (this._networking == null)
          this._networking = new Networking();
        return this._networking;
      }
    }

    public InGamePurchasing InGamePurchasing
    {
      get
      {
        if (this._inGamePurchasing == null)
          this._inGamePurchasing = new InGamePurchasing();
        return this._inGamePurchasing;
      }
    }

    public SteamWebAPI SteamWebAPI
    {
      get
      {
        if (this._steamWebAPI == null)
          this._steamWebAPI = new SteamWebAPI();
        return this._steamWebAPI;
      }
    }

    public CommunityExpress.OnLog Logger
    {
      set
      {
        this._logger = value;
      }
    }

    public bool IsGameServerInitialized
    {
      get
      {
        if (this._gameserver != null)
          return this._gameserver.IsInitialized;
        else
          return false;
      }
    }

    static CommunityExpress()
    {
    }

    private CommunityExpress()
    {
    }

    ~CommunityExpress()
    {
      this.Shutdown();
    }

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_RestartAppIfNecessary(uint unOwnAppID);

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_Init(IntPtr OnChallengeResponse);

    [DllImport("CommunityExpressSW")]
    private static void SteamUnityAPI_RunCallbacks();

    [DllImport("CommunityExpressSW")]
    private static void SteamUnityAPI_SteamGameServer_RunCallbacks();

    [DllImport("CommunityExpressSW")]
    private static uint SteamUnityAPI_SteamUtils_GetAppID();

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_SteamUtils_IsAPICallCompleted(ulong callHandle, out byte failed);

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_SteamUtils_GetGameServerUserStatsReceivedResult(ulong callHandle, out GSStatsReceived_t result, out byte failed);

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_SteamUtils_GetLobbyCreatedResult(ulong callHandle, out LobbyCreated_t result, out byte failed);

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_SteamUtils_GetLobbyListReceivedResult(ulong callHandle, out LobbyMatchList_t result, out byte failed);

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_SteamUtils_GetLobbyEnteredResult(ulong callHandle, out LobbyEnter_t result, out byte failed);

    [DllImport("CommunityExpressSW")]
    private static void SteamUnityAPI_Shutdown();

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_SetWarningMessageHook(IntPtr OnSteamAPIDebugTextHook);

    [DllImport("CommunityExpressSW")]
    private static void SteamUnityAPI_WriteMiniDump(uint exceptionCode, IntPtr exceptionInfo, uint buildID);

    [DllImport("CommunityExpressSW")]
    private static void SteamUnityAPI_SetMiniDumpComment([MarshalAs(UnmanagedType.LPStr)] string comment);

    public bool RestartAppIfNecessary(uint unOwnAppID)
    {
      return CommunityExpress.SteamUnityAPI_RestartAppIfNecessary(unOwnAppID);
    }

    public void WriteMiniDump(uint exceptionCode, IntPtr exceptionInfo, uint buildID)
    {
      CommunityExpress.SteamUnityAPI_WriteMiniDump(exceptionCode, exceptionInfo, buildID);
    }

    public void SetMiniDumpComment(string comment)
    {
      CommunityExpress.SteamUnityAPI_SetMiniDumpComment(comment);
    }

    public bool Initialize()
    {
      this._challengeResponse = new CommunityExpress.OnChallengeResponseFromSteam(this.OnChallengeResponseCallback);
      if (!CommunityExpress.SteamUnityAPI_Init(Marshal.GetFunctionPointerForDelegate((Delegate) this._challengeResponse)))
        return false;
      this._steamAPIDebugTextHook = new CommunityExpress.OnSteamAPIDebugTextHook(this.OnSteamAPIDebugTextHookCallback);
      CommunityExpress.SteamUnityAPI_SetWarningMessageHook(Marshal.GetFunctionPointerForDelegate((Delegate) this._steamAPIDebugTextHook));
      this.ValidateLicense();
      return true;
    }

    public void Log(string msg)
    {
      if (this._logger == null)
        return;
      this._logger(msg);
    }

    public void RunCallbacks()
    {
      CommunityExpress.SteamUnityAPI_RunCallbacks();
      if (this._gameserver != null && this._gameserver.IsInitialized)
      {
        CommunityExpress.SteamUnityAPI_SteamGameServer_RunCallbacks();
        for (int index = 0; index < this._gameserverUserStatsReceivedCallHandles.Count; ++index)
        {
          ulong callHandle = this._gameserverUserStatsReceivedCallHandles[index];
          byte failed;
          if (CommunityExpress.SteamUnityAPI_SteamUtils_IsAPICallCompleted(callHandle, out failed))
          {
            GSStatsReceived_t result = new GSStatsReceived_t();
            UserStatsReceived_t CallbackData = new UserStatsReceived_t();
            CommunityExpress.SteamUnityAPI_SteamUtils_GetGameServerUserStatsReceivedResult(callHandle, out result, out failed);
            CallbackData.m_nGameID = (ulong) CommunityExpress.SteamUnityAPI_SteamUtils_GetAppID();
            CallbackData.m_steamIDUser = result.m_steamIDUser;
            CallbackData.m_eResult = result.m_eResult;
            this._gameserverUserStatsReceivedCallbacks[index](ref CallbackData);
            this._gameserverUserStatsReceivedCallHandles.RemoveAt(index);
            this._gameserverUserStatsReceivedCallbacks.RemoveAt(index);
            --index;
          }
        }
      }
      if (this._networking != null && this._networking.IsInitialized)
        this._networking.CheckForNewP2PPackets();
      byte failed1;
      if ((long) this._userGetEncryptedAppTicketCallHandle != 0L && CommunityExpress.SteamUnityAPI_SteamUtils_IsAPICallCompleted(this._userGetEncryptedAppTicketCallHandle, out failed1))
      {
        this._userGetEncryptedAppTicketCallback();
        this._userGetEncryptedAppTicketCallHandle = 0UL;
      }
      byte failed2;
      if ((long) this._lobbyCreatedCallHandle != 0L && CommunityExpress.SteamUnityAPI_SteamUtils_IsAPICallCompleted(this._lobbyCreatedCallHandle, out failed2))
      {
        LobbyCreated_t result = new LobbyCreated_t();
        CommunityExpress.SteamUnityAPI_SteamUtils_GetLobbyCreatedResult(this._lobbyCreatedCallHandle, out result, out failed2);
        this._lobbyCreatedCallback(ref result);
        this._lobbyCreatedCallHandle = 0UL;
      }
      byte failed3;
      if ((long) this._lobbyListReceivedCallHandle != 0L && CommunityExpress.SteamUnityAPI_SteamUtils_IsAPICallCompleted(this._lobbyListReceivedCallHandle, out failed3))
      {
        LobbyMatchList_t result = new LobbyMatchList_t();
        CommunityExpress.SteamUnityAPI_SteamUtils_GetLobbyListReceivedResult(this._lobbyListReceivedCallHandle, out result, out failed3);
        this._lobbyListReceivedCallback(ref result);
        this._lobbyListReceivedCallHandle = 0UL;
      }
      byte failed4;
      if ((long) this._lobbyJoinedCallHandle == 0L || !CommunityExpress.SteamUnityAPI_SteamUtils_IsAPICallCompleted(this._lobbyJoinedCallHandle, out failed4))
        return;
      LobbyEnter_t result1 = new LobbyEnter_t();
      CommunityExpress.SteamUnityAPI_SteamUtils_GetLobbyEnteredResult(this._lobbyJoinedCallHandle, out result1, out failed4);
      this._lobbyJoinedCallback(ref result1);
      this._lobbyJoinedCallHandle = 0UL;
    }

    public void Shutdown()
    {
      if (this._shutdown)
        return;
      this._shutdown = true;
      CommunityExpress.SteamUnityAPI_Shutdown();
    }

    public bool InitalizeGameServer()
    {
      return true;
    }

    internal void AddGameServerUserStatsReceivedCallback(ulong handle, OnUserStatsReceivedFromSteam callback)
    {
      this._gameserverUserStatsReceivedCallHandles.Add(handle);
      this._gameserverUserStatsReceivedCallbacks.Add(callback);
    }

    internal void AddUserGetEncryptedAppTicketCallback(ulong handle, OnUserGetEncryptedAppTicketFromSteam callback)
    {
      this._userGetEncryptedAppTicketCallHandle = handle;
      this._userGetEncryptedAppTicketCallback = callback;
    }

    internal void AddCreateLobbyCallback(ulong handle, OnMatchmakingLobbyCreatedBySteam callback)
    {
      this._lobbyCreatedCallHandle = handle;
      this._lobbyCreatedCallback = callback;
    }

    internal void AddLobbyListRequestCallback(ulong handle, OnMatchmakingLobbyListReceivedFromSteam callback)
    {
      this._lobbyListReceivedCallHandle = handle;
      this._lobbyListReceivedCallback = callback;
    }

    internal void RemoveLobbyListRequestCallback(ulong handle, OnMatchmakingLobbyListReceivedFromSteam callback)
    {
      this._lobbyListReceivedCallHandle = 0UL;
    }

    internal void AddLobbyJoinedCallback(ulong handle, OnMatchmakingLobbyJoinedFromSteam callback)
    {
      this._lobbyJoinedCallHandle = handle;
      this._lobbyJoinedCallback = callback;
    }

    private void OnSteamAPIDebugTextHookCallback(int nSeverity, IntPtr pchDebugText)
    {
      this.Log(Marshal.PtrToStringAnsi(pchDebugText));
    }

    private ulong OnChallengeResponseCallback(ulong challenge)
    {
      return (ulong) Math.Sqrt((double) challenge);
    }

    private void ValidateLicense()
    {
      string filename = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "CESDKLicense.xml");
      uint appId = CommunityExpress.SteamUnityAPI_SteamUtils_GetAppID();
      string communityExpressSdk = Resources.CommunityExpressSDK;
      using (RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider())
      {
        cryptoServiceProvider.FromXmlString(communityExpressSdk);
        XmlDocument document = new XmlDocument();
        document.Load(filename);
        SignedXml signedXml = new SignedXml(document);
        try
        {
          XmlNode xmlNode = document.GetElementsByTagName("Signature", "http://www.w3.org/2000/09/xmldsig#")[0];
          signedXml.LoadXml((XmlElement) xmlNode);
        }
        catch (Exception ex)
        {
          throw new Exception("Error: no signature found.", ex);
        }
        if (!signedXml.CheckSignature((AsymmetricAlgorithm) cryptoServiceProvider))
          throw new Exception(string.Format("Error: License '{0}' invalid.", (object) filename));
        try
        {
          XmlNodeList elementsByTagName = document.GetElementsByTagName("AppId");
          if (elementsByTagName == null || elementsByTagName.Count == 0 || string.IsNullOrEmpty(elementsByTagName[0].InnerText))
            throw new Exception("AppId missing from license");
          if (elementsByTagName[0].InnerText != appId.ToString())
            throw new Exception(string.Format("AppId mismatch: {0} vs {1}", (object) elementsByTagName[0].InnerText, (object) appId));
        }
        catch (Exception ex)
        {
          throw new Exception(string.Format("Error: License '{0}' invalid.", (object) filename), ex);
        }
      }
    }

    private delegate ulong OnChallengeResponseFromSteam(ulong challenge);

    private delegate void OnSteamAPIDebugTextHook(int nSeverity, IntPtr pchDebugText);

    public delegate void OnLog(string msg);
  }
}
