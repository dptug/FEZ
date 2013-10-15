// Type: CommunityExpressNS.SteamWebAPI
// Assembly: CommunityExpress, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3B3F745C-AA2A-4DDF-AA8A-F5898AF84B8D
// Assembly location: F:\Program Files (x86)\FEZ\CommunityExpress.dll

namespace CommunityExpressNS
{
  public class SteamWebAPI
  {
    private const string BaseURL = "https://api.steampowered.com/";

    public SteamWebAPIRequest NewRequest(string interfaceString, string functionString, string versionString = "v0001")
    {
      return new SteamWebAPIRequest("https://api.steampowered.com/" + interfaceString + "/" + functionString + "/" + versionString + "/");
    }
  }
}
