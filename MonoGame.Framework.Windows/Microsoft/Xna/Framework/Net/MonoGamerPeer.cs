// Type: Microsoft.Xna.Framework.Net.MonoGamerPeer
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using Lidgren.Network;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace Microsoft.Xna.Framework.Net
{
  internal class MonoGamerPeer
  {
    private BackgroundWorker MGServerWorker = new BackgroundWorker();
    private bool done = false;
    private string myLocalAddress = string.Empty;
    private IPEndPoint myLocalEndPoint = (IPEndPoint) null;
    private Dictionary<long, NetConnection> pendingGamers = new Dictionary<long, NetConnection>();
    private bool online = false;
    private static int port = 3074;
    private static int masterserverport = 6000;
    private static string masterServer = "monolive.servegame.com";
    internal static string applicationIdentifier = "monogame";
    private NetServer peer;
    private NetworkSession session;
    private AvailableNetworkSession availableSession;
    private static IPEndPoint m_masterServer;
    private static NetPeer netPeer;
    private static List<NetIncomingMessage> discoveryMsgs;

    public TimeSpan SimulatedLatency
    {
      get
      {
        return new TimeSpan(0L);
      }
      set
      {
      }
    }

    public float SimulatedPacketLoss
    {
      get
      {
        return 0.0f;
      }
      set
      {
      }
    }

    internal bool IsReady
    {
      get
      {
        return this.peer != null;
      }
    }

    static MonoGamerPeer()
    {
      Assembly assembly = Assembly.GetAssembly(Game.Instance.GetType());
      if (!(assembly != (Assembly) null))
        return;
      object[] customAttributes = assembly.GetCustomAttributes(typeof (GuidAttribute), false);
      if (customAttributes.Length > 0)
        MonoGamerPeer.applicationIdentifier = ((GuidAttribute) customAttributes[0]).Value;
    }

    public MonoGamerPeer(NetworkSession session, AvailableNetworkSession availableSession)
    {
      this.session = session;
      this.online = this.session.SessionType == NetworkSessionType.PlayerMatch;
      this.availableSession = availableSession;
      this.MGServerWorker.WorkerSupportsCancellation = true;
      this.MGServerWorker.DoWork += new DoWorkEventHandler(this.MGServer_DoWork);
      this.MGServerWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.MGServer_RunWorkerCompleted);
      this.MGServerWorker.RunWorkerAsync();
      this.HookEvents();
    }

    private void HookEvents()
    {
      this.session.GameEnded += new EventHandler<GameEndedEventArgs>(this.HandleSessionStateChanged);
      this.session.SessionEnded += new EventHandler<NetworkSessionEndedEventArgs>(this.HandleSessionStateChanged);
      this.session.GameStarted += new EventHandler<GameStartedEventArgs>(this.HandleSessionStateChanged);
    }

    private void HandleSessionStateChanged(object sender, EventArgs e)
    {
      this.SendSessionStateChange();
      if (this.session.SessionState != NetworkSessionState.Ended)
        return;
      this.MGServerWorker.CancelAsync();
    }

    internal void ShutDown()
    {
      this.MGServerWorker.CancelAsync();
    }

    private void MGServer_DoWork(object sender, DoWorkEventArgs e)
    {
      BackgroundWorker backgroundWorker = sender as BackgroundWorker;
      NetPeerConfiguration peerConfiguration = new NetPeerConfiguration(MonoGamerPeer.applicationIdentifier);
      peerConfiguration.EnableMessageType((NetIncomingMessageType) 32);
      peerConfiguration.EnableMessageType((NetIncomingMessageType) 64);
      peerConfiguration.EnableMessageType((NetIncomingMessageType) 2048);
      if (this.availableSession == null)
        peerConfiguration.set_Port(MonoGamerPeer.port);
      this.peer = new NetServer(peerConfiguration);
      ((NetPeer) this.peer).Start();
      this.myLocalAddress = MonoGamerPeer.GetMyLocalIpAddress();
      this.myLocalEndPoint = new IPEndPoint(IPAddress.Parse(this.myLocalAddress), MonoGamerPeer.port);
      while (this.session.LocalGamers.Count <= 0)
        Thread.Sleep(10);
      if (this.availableSession != null)
      {
        if (!this.online)
          ((NetPeer) this.peer).Connect(this.availableSession.EndPoint);
        else
          MonoGamerPeer.RequestNATIntroduction(this.availableSession.EndPoint, (NetPeer) this.peer);
      }
      else if (this.online)
      {
        IPAddress address = NetUtility.Resolve(MonoGamerPeer.masterServer);
        if (address == null)
          throw new Exception("Could not resolve live host");
        MonoGamerPeer.m_masterServer = new IPEndPoint(address, MonoGamerPeer.masterserverport);
        LocalNetworkGamer localNetworkGamer = this.session.LocalGamers[0];
        NetOutgoingMessage message = ((NetPeer) this.peer).CreateMessage();
        message.Write((byte) 0);
        message.Write(this.session.AllGamers.Count);
        message.Write(localNetworkGamer.Gamertag);
        message.Write(this.session.PrivateGamerSlots);
        message.Write(this.session.MaxGamers);
        message.Write(localNetworkGamer.IsHost);
        message.Write(this.myLocalEndPoint);
        message.Write(((NetPeer) this.peer).get_Configuration().get_AppIdentifier());
        int[] propertyData = new int[this.session.SessionProperties.Count * 2];
        NetworkSessionProperties.WriteProperties(this.session.SessionProperties, propertyData);
        for (int index = 0; index < propertyData.Length; ++index)
          message.Write(propertyData[index]);
        ((NetPeer) this.peer).SendUnconnectedMessage(message, MonoGamerPeer.m_masterServer);
      }
      do
      {
        NetIncomingMessage netIncomingMessage;
        while ((netIncomingMessage = ((NetPeer) this.peer).ReadMessage()) != null)
        {
          NetIncomingMessageType messageType = netIncomingMessage.get_MessageType();
          if (messageType <= 128)
          {
            if (messageType <= 8)
            {
              switch (messageType - 1)
              {
                case 0:
                  NetConnectionStatus connectionStatus = (NetConnectionStatus) (int) netIncomingMessage.ReadByte();
                  if (connectionStatus == 5)
                    this.session.commandQueue.Enqueue(new CommandEvent((ICommand) new CommandGamerLeft(netIncomingMessage.get_SenderConnection().get_RemoteUniqueIdentifier())));
                  if (connectionStatus == 3 && !this.pendingGamers.ContainsKey(netIncomingMessage.get_SenderConnection().get_RemoteUniqueIdentifier()))
                  {
                    this.pendingGamers.Add(netIncomingMessage.get_SenderConnection().get_RemoteUniqueIdentifier(), netIncomingMessage.get_SenderConnection());
                    this.SendProfileRequest(netIncomingMessage.get_SenderConnection());
                    break;
                  }
                  else
                    break;
                case 1:
                  break;
                default:
                  if (messageType == 8)
                  {
                    switch (netIncomingMessage.ReadByte())
                    {
                      case (byte) 0:
                        byte[] data = new byte[netIncomingMessage.get_LengthBytes() - 1];
                        netIncomingMessage.ReadBytes(data, 0, data.Length);
                        this.session.commandQueue.Enqueue(new CommandEvent((ICommand) new CommandReceiveData(netIncomingMessage.get_SenderConnection().get_RemoteUniqueIdentifier(), data)));
                        break;
                      case (byte) 3:
                        string endPoint1 = netIncomingMessage.ReadString();
                        try
                        {
                          IPEndPoint endPoint2 = MonoGamerPeer.ParseIPEndPoint(endPoint1);
                          if (this.myLocalEndPoint.ToString() != endPoint2.ToString() && !this.AlreadyConnected(endPoint2))
                          {
                            ((NetPeer) this.peer).Connect(endPoint2);
                            break;
                          }
                          else
                            break;
                        }
                        catch (Exception ex)
                        {
                          break;
                        }
                      case (byte) 4:
                        if (this.pendingGamers.ContainsKey(netIncomingMessage.get_SenderConnection().get_RemoteUniqueIdentifier()))
                        {
                          this.pendingGamers.Remove(netIncomingMessage.get_SenderConnection().get_RemoteUniqueIdentifier());
                          netIncomingMessage.ReadInt32();
                          string str = netIncomingMessage.ReadString();
                          netIncomingMessage.ReadInt32();
                          netIncomingMessage.ReadInt32();
                          GamerStates gamerStates = (GamerStates) (netIncomingMessage.ReadInt32() & -2);
                          this.session.commandQueue.Enqueue(new CommandEvent((ICommand) new CommandGamerJoined(netIncomingMessage.get_SenderConnection().get_RemoteUniqueIdentifier())
                          {
                            GamerTag = str,
                            State = gamerStates
                          }));
                          break;
                        }
                        else
                          break;
                      case (byte) 5:
                        this.SendProfile(netIncomingMessage.get_SenderConnection());
                        break;
                      case (byte) 6:
                        GamerStates gamerStates1 = (GamerStates) (netIncomingMessage.ReadInt32() & -2);
                        using (IEnumerator<NetworkGamer> enumerator = this.session.RemoteGamers.GetEnumerator())
                        {
                          while (enumerator.MoveNext())
                          {
                            NetworkGamer current = enumerator.Current;
                            if (current.RemoteUniqueIdentifier == netIncomingMessage.get_SenderConnection().get_RemoteUniqueIdentifier())
                              current.State = gamerStates1;
                          }
                          break;
                        }
                      case (byte) 7:
                        NetworkSessionState networkSessionState = (NetworkSessionState) netIncomingMessage.ReadInt32();
                        using (IEnumerator<NetworkGamer> enumerator = this.session.RemoteGamers.GetEnumerator())
                        {
                          while (enumerator.MoveNext())
                          {
                            NetworkGamer current = enumerator.Current;
                            if (current.RemoteUniqueIdentifier == netIncomingMessage.get_SenderConnection().get_RemoteUniqueIdentifier() && (current.IsHost && networkSessionState == NetworkSessionState.Playing))
                              this.session.StartGame();
                          }
                          break;
                        }
                    }
                  }
                  else
                    break;
              }
            }
            else if (messageType != 32)
            {
              if (messageType == 128)
                ;
            }
            else
            {
              LocalNetworkGamer localNetworkGamer = this.session.LocalGamers[0];
              NetOutgoingMessage message = ((NetPeer) this.peer).CreateMessage();
              message.Write(this.session.RemoteGamers.Count);
              message.Write(localNetworkGamer.Gamertag);
              message.Write(this.session.PrivateGamerSlots);
              message.Write(this.session.MaxGamers);
              message.Write(localNetworkGamer.IsHost);
              int[] propertyData = new int[this.session.SessionProperties.Count * 2];
              NetworkSessionProperties.WriteProperties(this.session.SessionProperties, propertyData);
              for (int index = 0; index < propertyData.Length; ++index)
                message.Write(propertyData[index]);
              ((NetPeer) this.peer).SendDiscoveryResponse(message, netIncomingMessage.get_SenderEndpoint());
            }
          }
          else if (messageType <= 512)
          {
            if (messageType == 256 || messageType == 512)
              ;
          }
          else if (messageType != 1024 && messageType == 2048)
            ((NetPeer) this.peer).Connect(netIncomingMessage.get_SenderEndpoint());
        }
        Thread.Sleep(1);
        if (backgroundWorker.CancellationPending)
        {
          e.Cancel = true;
          this.done = true;
        }
      }
      while (!this.done);
    }

    private bool AlreadyConnected(IPEndPoint endPoint)
    {
      using (List<NetConnection>.Enumerator enumerator = ((NetPeer) this.peer).get_Connections().GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          if (enumerator.Current.get_RemoteEndpoint() == endPoint)
            return true;
        }
      }
      return false;
    }

    private void MGServer_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if (e.Cancelled || e.Error == null)
        ;
      if (this.online && this.availableSession == null)
      {
        NetOutgoingMessage message = ((NetPeer) this.peer).CreateMessage();
        message.Write((byte) 3);
        message.Write(this.session.Host.Gamertag);
        message.Write(((NetPeer) this.peer).get_Configuration().get_AppIdentifier());
        ((NetPeer) this.peer).SendUnconnectedMessage(message, MonoGamerPeer.m_masterServer);
      }
      ((NetPeer) this.peer).Shutdown("app exiting");
    }

    internal void SendProfile(NetConnection player)
    {
      NetOutgoingMessage message = ((NetPeer) this.peer).CreateMessage();
      message.Write((byte) 4);
      message.Write(this.session.AllGamers.Count);
      message.Write(this.session.LocalGamers[0].Gamertag);
      message.Write(this.session.PrivateGamerSlots);
      message.Write(this.session.MaxGamers);
      message.Write((int) this.session.LocalGamers[0].State);
      ((NetPeer) this.peer).SendMessage(message, player, (NetDeliveryMethod) 67);
    }

    internal void SendProfileRequest(NetConnection player)
    {
      NetOutgoingMessage message = ((NetPeer) this.peer).CreateMessage();
      message.Write((byte) 5);
      ((NetPeer) this.peer).SendMessage(message, player, (NetDeliveryMethod) 67);
    }

    internal void SendPeerIntroductions(NetworkGamer gamer)
    {
      NetConnection netConnection = (NetConnection) null;
      using (List<NetConnection>.Enumerator enumerator = ((NetPeer) this.peer).get_Connections().GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          NetConnection current = enumerator.Current;
          if (current.get_RemoteUniqueIdentifier() == gamer.RemoteUniqueIdentifier)
            netConnection = current;
        }
      }
      if (netConnection == null)
        return;
      using (List<NetConnection>.Enumerator enumerator = ((NetPeer) this.peer).get_Connections().GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          NetConnection current = enumerator.Current;
          NetOutgoingMessage message = ((NetPeer) this.peer).CreateMessage();
          message.Write((byte) 3);
          message.Write(netConnection.get_RemoteEndpoint().ToString());
          ((NetPeer) this.peer).SendMessage(message, current, (NetDeliveryMethod) 67);
        }
      }
    }

    internal void SendGamerStateChange(NetworkGamer gamer)
    {
      NetOutgoingMessage message = ((NetPeer) this.peer).CreateMessage();
      message.Write((byte) 6);
      message.Write((int) gamer.State);
      this.SendMessage(message, SendDataOptions.Reliable, gamer);
    }

    internal void SendSessionStateChange()
    {
      NetOutgoingMessage message = ((NetPeer) this.peer).CreateMessage();
      message.Write((byte) 7);
      message.Write((int) this.session.SessionState);
      this.SendMessage(message, SendDataOptions.Reliable, (NetworkGamer) null);
    }

    public static IPEndPoint ParseIPEndPoint(string endPoint)
    {
      string[] strArray = endPoint.Split(new char[1]
      {
        ':'
      });
      if (strArray.Length != 2)
        throw new FormatException("Invalid endpoint format");
      IPAddress address;
      if (!IPAddress.TryParse(strArray[0], out address))
        throw new FormatException("Invalid ip-adress");
      int result;
      if (!int.TryParse(strArray[1], out result))
        throw new FormatException("Invalid port");
      else
        return new IPEndPoint(address, result);
    }

    internal static string GetMyLocalIpAddress()
    {
      string str = "?";
      foreach (IPAddress ipAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
      {
        if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
        {
          str = ipAddress.ToString();
          break;
        }
      }
      return str;
    }

    internal void DiscoverPeers()
    {
      ((NetPeer) this.peer).DiscoverLocalPeers(MonoGamerPeer.port);
    }

    internal void SendData(byte[] data, SendDataOptions options)
    {
      this.SendMessage(NetworkMessageType.Data, data, options, (NetworkGamer) null);
    }

    internal void SendData(byte[] data, SendDataOptions options, NetworkGamer gamer)
    {
      this.SendMessage(NetworkMessageType.Data, data, options, gamer);
    }

    private void SendMessage(NetworkMessageType messageType, byte[] data, SendDataOptions options, NetworkGamer gamer)
    {
      NetOutgoingMessage message = ((NetPeer) this.peer).CreateMessage();
      message.Write((byte) messageType);
      message.Write(data);
      this.SendMessage(message, options, gamer);
    }

    private void SendMessage(NetOutgoingMessage om, SendDataOptions options, NetworkGamer gamer)
    {
      NetDeliveryMethod netDeliveryMethod = (NetDeliveryMethod) 1;
      switch (options)
      {
        case SendDataOptions.None:
          netDeliveryMethod = (NetDeliveryMethod) 0;
          break;
        case SendDataOptions.Reliable:
          netDeliveryMethod = (NetDeliveryMethod) 35;
          break;
        case SendDataOptions.InOrder:
          netDeliveryMethod = (NetDeliveryMethod) 2;
          break;
        case SendDataOptions.ReliableInOrder:
          netDeliveryMethod = (NetDeliveryMethod) 67;
          break;
      }
      this.peer.SendToAll(om, netDeliveryMethod);
    }

    internal static void Find(NetworkSessionType sessionType)
    {
      NetPeerConfiguration peerConfiguration = new NetPeerConfiguration(MonoGamerPeer.applicationIdentifier);
      if (sessionType == NetworkSessionType.PlayerMatch)
      {
        peerConfiguration.EnableMessageType((NetIncomingMessageType) 2);
        peerConfiguration.EnableMessageType((NetIncomingMessageType) 2048);
      }
      else
        peerConfiguration.EnableMessageType((NetIncomingMessageType) 32);
      if (MonoGameNetworkConfiguration.Broadcast != IPAddress.None)
        peerConfiguration.set_BroadcastAddress(MonoGameNetworkConfiguration.Broadcast);
      MonoGamerPeer.netPeer = new NetPeer(peerConfiguration);
      MonoGamerPeer.netPeer.Start();
      if (sessionType == NetworkSessionType.PlayerMatch)
        MonoGamerPeer.GetServerList(MonoGamerPeer.netPeer);
      else
        MonoGamerPeer.netPeer.DiscoverLocalPeers(MonoGamerPeer.port);
      DateTime now = DateTime.Now;
      MonoGamerPeer.discoveryMsgs = new List<NetIncomingMessage>();
      do
      {
        NetIncomingMessage netIncomingMessage;
        while ((netIncomingMessage = MonoGamerPeer.netPeer.ReadMessage()) != null)
        {
          NetIncomingMessageType messageType = netIncomingMessage.get_MessageType();
          if (messageType <= 128)
          {
            if (messageType != 2)
            {
              if (messageType != 64)
              {
                if (messageType == 128)
                  ;
              }
              else
                MonoGamerPeer.discoveryMsgs.Add(netIncomingMessage);
            }
            else if (netIncomingMessage.get_SenderEndpoint().Equals((object) MonoGamerPeer.m_masterServer))
              MonoGamerPeer.discoveryMsgs.Add(netIncomingMessage);
          }
          else if (messageType == 256 || messageType == 512 || messageType == 1024)
            ;
        }
      }
      while ((DateTime.Now - now).Seconds <= 2);
      MonoGamerPeer.netPeer.Shutdown("Find shutting down");
    }

    private static void GetServerList(NetPeer netPeer)
    {
      MonoGamerPeer.m_masterServer = new IPEndPoint(NetUtility.Resolve(MonoGamerPeer.masterServer), MonoGamerPeer.masterserverport);
      NetOutgoingMessage message = netPeer.CreateMessage();
      message.Write((byte) 1);
      message.Write(netPeer.get_Configuration().get_AppIdentifier());
      netPeer.SendUnconnectedMessage(message, MonoGamerPeer.m_masterServer);
    }

    public static void RequestNATIntroduction(IPEndPoint host, NetPeer peer)
    {
      if (host == null)
        return;
      if (MonoGamerPeer.m_masterServer == null)
        throw new Exception("Must connect to master server first!");
      NetOutgoingMessage message = peer.CreateMessage();
      message.Write((byte) 2);
      IPAddress address = IPAddress.Parse(MonoGamerPeer.GetMyLocalIpAddress());
      message.Write(new IPEndPoint(address, peer.get_Port()));
      IPEndPoint ipEndPoint = new IPEndPoint(host.Address, MonoGamerPeer.port);
      message.Write(ipEndPoint);
      message.Write(peer.get_Configuration().get_AppIdentifier());
      peer.SendUnconnectedMessage(message, MonoGamerPeer.m_masterServer);
    }

    internal static void FindResults(List<AvailableNetworkSession> networkSessions)
    {
      using (List<NetIncomingMessage>.Enumerator enumerator = MonoGamerPeer.discoveryMsgs.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          NetIncomingMessage current = enumerator.Current;
          AvailableNetworkSession availableNetworkSession = new AvailableNetworkSession();
          NetIncomingMessageType messageType = current.get_MessageType();
          bool flag;
          if (messageType != 2)
          {
            if (messageType == 64)
            {
              int num1 = current.ReadInt32();
              string str = current.ReadString();
              int num2 = current.ReadInt32();
              int num3 = current.ReadInt32();
              flag = current.ReadBoolean();
              NetworkSessionProperties properties = new NetworkSessionProperties();
              int[] propertyData = new int[properties.Count * 2];
              for (int index = 0; index < propertyData.Length; ++index)
                propertyData[index] = current.ReadInt32();
              NetworkSessionProperties.ReadProperties(properties, propertyData);
              availableNetworkSession.SessionProperties = properties;
              availableNetworkSession.SessionType = NetworkSessionType.SystemLink;
              availableNetworkSession.CurrentGamerCount = num1;
              availableNetworkSession.HostGamertag = str;
              availableNetworkSession.OpenPrivateGamerSlots = num2;
              availableNetworkSession.OpenPublicGamerSlots = num3;
              availableNetworkSession.EndPoint = current.get_SenderEndpoint();
              availableNetworkSession.InternalEndpont = (IPEndPoint) null;
            }
          }
          else if (current.get_SenderEndpoint().Equals((object) MonoGamerPeer.m_masterServer))
          {
            int num1 = current.ReadInt32();
            string str = current.ReadString();
            int num2 = current.ReadInt32();
            int num3 = current.ReadInt32();
            flag = current.ReadBoolean();
            IPEndPoint ipEndPoint1 = current.ReadIPEndpoint();
            IPEndPoint ipEndPoint2 = current.ReadIPEndpoint();
            availableNetworkSession.SessionType = NetworkSessionType.PlayerMatch;
            availableNetworkSession.CurrentGamerCount = num1;
            availableNetworkSession.HostGamertag = str;
            availableNetworkSession.OpenPrivateGamerSlots = num2;
            availableNetworkSession.OpenPublicGamerSlots = num3;
            availableNetworkSession.EndPoint = ipEndPoint2;
            availableNetworkSession.InternalEndpont = ipEndPoint1;
          }
          networkSessions.Add(availableNetworkSession);
        }
      }
    }

    internal void UpdateLiveSession(NetworkSession networkSession)
    {
      if (this.peer == null || MonoGamerPeer.m_masterServer == null || !networkSession.IsHost)
        return;
      NetOutgoingMessage message = ((NetPeer) this.peer).CreateMessage();
      message.Write((byte) 0);
      message.Write(this.session.AllGamers.Count);
      message.Write(this.session.LocalGamers[0].Gamertag);
      message.Write(this.session.PrivateGamerSlots);
      message.Write(this.session.MaxGamers);
      message.Write(this.session.LocalGamers[0].IsHost);
      IPAddress address = IPAddress.Parse(MonoGamerPeer.GetMyLocalIpAddress());
      message.Write(new IPEndPoint(address, MonoGamerPeer.port));
      message.Write(((NetPeer) this.peer).get_Configuration().get_AppIdentifier());
      ((NetPeer) this.peer).SendUnconnectedMessage(message, MonoGamerPeer.m_masterServer);
    }
  }
}
