// Type: CommunityExpressNS.LeaderboardEntries
// Assembly: CommunityExpress, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3B3F745C-AA2A-4DDF-AA8A-F5898AF84B8D
// Assembly location: F:\Program Files (x86)\FEZ\CommunityExpress.dll

using System;
using System.Collections;
using System.Collections.Generic;

namespace CommunityExpressNS
{
  public class LeaderboardEntries : IList<LeaderboardEntry>, ICollection<LeaderboardEntry>, IEnumerable<LeaderboardEntry>, IEnumerable
  {
    private List<LeaderboardEntry> _leaderboardEntryList = new List<LeaderboardEntry>();
    private int _minRank = int.MaxValue;
    private int _maxRank = int.MinValue;
    private Leaderboard _leaderboard;

    public Leaderboard Leaderboard
    {
      get
      {
        return this._leaderboard;
      }
    }

    public int LowestRank
    {
      get
      {
        return this._minRank;
      }
    }

    public int HighestRank
    {
      get
      {
        return this._maxRank;
      }
    }

    public int Count
    {
      get
      {
        return this._leaderboardEntryList.Count;
      }
    }

    public bool IsReadOnly
    {
      get
      {
        return true;
      }
    }

    public LeaderboardEntry this[int index]
    {
      get
      {
        return this._leaderboardEntryList[index];
      }
      set
      {
        throw new NotSupportedException();
      }
    }

    internal LeaderboardEntries(Leaderboard leaderboard)
    {
      this._leaderboard = leaderboard;
    }

    public void Add(LeaderboardEntry item)
    {
      this._leaderboardEntryList.Add(item);
      if (item.GlobalRank < this._minRank)
        this._minRank = item.GlobalRank;
      if (item.GlobalRank <= this._maxRank)
        return;
      this._maxRank = item.GlobalRank;
    }

    public void Clear()
    {
      this._leaderboardEntryList.Clear();
    }

    public bool Contains(LeaderboardEntry item)
    {
      throw new NotImplementedException();
    }

    public void CopyTo(LeaderboardEntry[] array, int arrayIndex)
    {
      throw new NotImplementedException();
    }

    public bool Remove(LeaderboardEntry item)
    {
      throw new NotSupportedException();
    }

    public IEnumerator<LeaderboardEntry> GetEnumerator()
    {
      return (IEnumerator<LeaderboardEntry>) new ListEnumerator<LeaderboardEntry>((IList<LeaderboardEntry>) this._leaderboardEntryList);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.GetEnumerator();
    }

    public int IndexOf(LeaderboardEntry item)
    {
      return this._leaderboardEntryList.IndexOf(item);
    }

    public void Insert(int index, LeaderboardEntry item)
    {
      throw new NotSupportedException();
    }

    public void RemoveAt(int index)
    {
      throw new NotSupportedException();
    }
  }
}
