// Type: CommunityExpressNS.Server
// Assembly: CommunityExpress, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3B3F745C-AA2A-4DDF-AA8A-F5898AF84B8D
// Assembly location: F:\Program Files (x86)\FEZ\CommunityExpress.dll

using System.Net;

namespace CommunityExpressNS
{
  public class Server
  {
    public int Version { get; private set; }

    public IPAddress IP { get; private set; }

    public ushort Port { get; private set; }

    public ushort QueryPort { get; private set; }

    public int Ping { get; private set; }

    public uint AppId { get; private set; }

    public string ServerName { get; private set; }

    public string MapName { get; private set; }

    public string GameDescription { get; private set; }

    public string GameDir { get; private set; }

    public bool IsSecure { get; private set; }

    public bool IsPassworded { get; private set; }

    public int Players { get; private set; }

    public int MaxPlayers { get; private set; }

    public int BotPlayers { get; private set; }

    public string GameTags { get; private set; }

    internal Server(int version, IPAddress ip, ushort port, ushort queryPort, int ping, string serverName, string mapName, string gameDesc, bool isSecure, bool isPassworded, int players, int maxPlayers, int botPlayers, string gameTags, string gameDir, uint appId)
    {
      this.Version = version;
      this.IP = ip;
      this.Port = port;
      this.QueryPort = queryPort;
      this.Ping = ping;
      this.ServerName = serverName;
      this.MapName = mapName;
      this.GameDescription = gameDesc;
      this.IsSecure = isSecure;
      this.IsPassworded = isPassworded;
      this.Players = players;
      this.MaxPlayers = maxPlayers;
      this.BotPlayers = botPlayers;
      this.GameTags = gameTags;
      this.GameDir = gameDir;
      this.AppId = appId;
    }
  }
}
