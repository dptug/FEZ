// Type: CommunityExpressNS.Networking
// Assembly: CommunityExpress, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3B3F745C-AA2A-4DDF-AA8A-F5898AF84B8D
// Assembly location: F:\Program Files (x86)\FEZ\CommunityExpress.dll

using System;
using System.Runtime.InteropServices;

namespace CommunityExpressNS
{
  public class Networking
  {
    private int[] _listenChannels = new int[1];
    private IntPtr _networking;
    private bool _isInitialized;
    private OnNewP2PSessionFromSteam _internalOnNewP2PSession;
    private OnNewP2PSession _onNewP2PSession;
    private OnSendP2PPacketFailedFromSteam _internalOnSendP2PPacketFailed;
    private OnSendP2PPacketFailed _onSendP2PPacketFailed;
    private OnP2PPacketReceived _onP2PPacketReceived;

    public bool IsInitialized
    {
      get
      {
        return this._isInitialized;
      }
    }

    internal Networking()
    {
      this._networking = Networking.SteamUnityAPI_SteamNetworking();
    }

    [DllImport("CommunityExpressSW")]
    private static IntPtr SteamUnityAPI_SteamNetworking();

    [DllImport("CommunityExpressSW")]
    private static void SteamUnityAPI_SteamNetworking_SetCallbacks(IntPtr onNewP2PSession, IntPtr onSendP2PPacketFailed);

    [DllImport("CommunityExpressSW")]
    private static void SteamUnityAPI_SteamNetworking_AllowP2PPacketRelay(IntPtr networking, bool allow);

    [DllImport("CommunityExpressSW")]
    private static void SteamUnityAPI_SteamNetworking_AcceptP2PSessionWithUser(IntPtr networking, ulong steamID);

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_SteamNetworking_SendP2PPacket(IntPtr networking, ulong steamID, IntPtr data, uint dataLength, byte sendType, int channel);

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_SteamNetworking_IsP2PPacketAvailable(IntPtr networking, out uint packetSize, int channel);

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_SteamNetworking_ReadP2PPacket(IntPtr networking, IntPtr data, uint packetSize, out ulong steamID, int channel);

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_SteamNetworking_CloseP2PSession(IntPtr networking, ulong steamID);

    [DllImport("CommunityExpressSW")]
    private static bool SteamUnityAPI_SteamNetworking_CloseP2PChannel(IntPtr networking, ulong steamID, int channel);

    public void Init(bool allowPacketRelay, OnNewP2PSession onNewP2PSession, OnSendP2PPacketFailed onSendP2PPacketFailed, OnP2PPacketReceived onP2PPacketReceived)
    {
      this._onNewP2PSession = onNewP2PSession;
      this._onSendP2PPacketFailed = onSendP2PPacketFailed;
      this._onP2PPacketReceived = onP2PPacketReceived;
      if (this._internalOnNewP2PSession == null)
      {
        this._internalOnNewP2PSession = new OnNewP2PSessionFromSteam(this.OnNewP2PSession);
        this._internalOnSendP2PPacketFailed = new OnSendP2PPacketFailedFromSteam(this.OnSendP2PPacketFailed);
      }
      Networking.SteamUnityAPI_SteamNetworking_SetCallbacks(Marshal.GetFunctionPointerForDelegate((Delegate) this._internalOnNewP2PSession), Marshal.GetFunctionPointerForDelegate((Delegate) this._internalOnSendP2PPacketFailed));
      Networking.SteamUnityAPI_SteamNetworking_AllowP2PPacketRelay(this._networking, allowPacketRelay);
      this._isInitialized = true;
    }

    private void OnNewP2PSession(P2PSessionRequest_t callbackData)
    {
      if (!this._onNewP2PSession(new SteamID(callbackData.m_steamID)))
        return;
      Networking.SteamUnityAPI_SteamNetworking_AcceptP2PSessionWithUser(this._networking, callbackData.m_steamID);
    }

    public void SendP2PPacket(SteamID steamID, string data, EP2PSend sendType, int channel = 0)
    {
      Networking.SteamUnityAPI_SteamNetworking_SendP2PPacket(this._networking, steamID.ToUInt64(), Marshal.StringToHGlobalAnsi(data), (uint) data.Length, (byte) sendType, channel);
    }

    public void SendP2PPacket(SteamID steamID, byte[] data, EP2PSend sendType, int channel = 0)
    {
      IntPtr num = Marshal.AllocHGlobal(data.Length);
      Marshal.Copy(data, 0, num, data.Length);
      Networking.SteamUnityAPI_SteamNetworking_SendP2PPacket(this._networking, steamID.ToUInt64(), num, (uint) data.Length, (byte) sendType, channel);
      Marshal.FreeHGlobal(num);
    }

    private void OnSendP2PPacketFailed(P2PSessionConnectFail_t callbackData)
    {
      this._onSendP2PPacketFailed(new SteamID(callbackData.m_steamID), (EP2PSessionError) callbackData.m_eP2PSessionError);
    }

    public void SetListenChannels(int[] listenChannels)
    {
      this._listenChannels = listenChannels;
    }

    internal void CheckForNewP2PPackets()
    {
      for (int index = 0; index < this._listenChannels.Length; ++index)
      {
        uint packetSize;
        while (Networking.SteamUnityAPI_SteamNetworking_IsP2PPacketAvailable(this._networking, out packetSize, this._listenChannels[index]))
        {
          IntPtr num = Marshal.AllocHGlobal((int) packetSize);
          ulong steamID;
          if (Networking.SteamUnityAPI_SteamNetworking_ReadP2PPacket(this._networking, num, packetSize, out steamID, this._listenChannels[index]))
          {
            byte[] numArray = new byte[(IntPtr) packetSize];
            Marshal.Copy(num, numArray, 0, (int) packetSize);
            this._onP2PPacketReceived(new SteamID(steamID), numArray, this._listenChannels[index]);
          }
          Marshal.FreeHGlobal(num);
        }
      }
    }

    public void CloseP2PSession(SteamID steamID)
    {
      Networking.SteamUnityAPI_SteamNetworking_CloseP2PSession(this._networking, steamID.ToUInt64());
    }

    public void CloseP2PSession(SteamID steamID, int channel)
    {
      Networking.SteamUnityAPI_SteamNetworking_CloseP2PChannel(this._networking, steamID.ToUInt64(), channel);
    }
  }
}
