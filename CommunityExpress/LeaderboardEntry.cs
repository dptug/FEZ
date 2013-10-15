// Type: CommunityExpressNS.LeaderboardEntry
// Assembly: CommunityExpress, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3B3F745C-AA2A-4DDF-AA8A-F5898AF84B8D
// Assembly location: F:\Program Files (x86)\FEZ\CommunityExpress.dll

using System.Collections.Generic;

namespace CommunityExpressNS
{
  public class LeaderboardEntry
  {
    private SteamID _id;
    private int _rank;
    private int _score;
    private List<int> _scoreDetails;

    public SteamID SteamID
    {
      get
      {
        return this._id;
      }
    }

    public int GlobalRank
    {
      get
      {
        return this._rank;
      }
    }

    public int Score
    {
      get
      {
        return this._score;
      }
    }

    public string PersonaName
    {
      get
      {
        return CommunityExpress.Instance.Friends.GetFriendPersonaName(this._id);
      }
    }

    public IList<int> ScoreDetails
    {
      get
      {
        return (IList<int>) this._scoreDetails;
      }
    }

    internal LeaderboardEntry(ulong steamIDUser, int globalRank, int score, List<int> scoreDetails)
    {
      this._id = new SteamID(steamIDUser);
      this._rank = globalRank;
      this._score = score;
      this._scoreDetails = scoreDetails;
    }
  }
}
