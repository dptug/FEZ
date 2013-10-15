// Type: FezGame.Structure.CachedLeaderboard
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using CommunityExpressNS;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezGame.Structure
{
  public class CachedLeaderboard
  {
    private readonly List<LeaderboardEntry> cachedEntries = new List<LeaderboardEntry>();
    private const int ReadPageSize = 100;
    private const string LeaderboardKey = "CompletionPercentage";
    private readonly int virtualPageSize;
    private Leaderboard leaderboard;
    private int startIndex;
    private Action callback;
    private Action onLeaderboardFound;

    public LeaderboardView View { get; private set; }

    public User ActiveGamer { get; set; }

    public bool InError
    {
      get
      {
        if (this.leaderboard == null)
          return !this.Reading;
        else
          return false;
      }
    }

    public bool Reading { get; private set; }

    public bool ChangingPage { get; private set; }

    public bool CanPageUp
    {
      get
      {
        if (this.CanChangePage)
          return this.startIndex > 0;
        else
          return false;
      }
    }

    public bool CanPageDown
    {
      get
      {
        if (this.CanChangePage)
          return this.TotalEntries > this.startIndex + this.virtualPageSize;
        else
          return false;
      }
    }

    private bool CanChangePage { get; set; }

    public IEnumerable<LeaderboardEntry> Entries
    {
      get
      {
        return Enumerable.Take<LeaderboardEntry>(Enumerable.Skip<LeaderboardEntry>((IEnumerable<LeaderboardEntry>) this.cachedEntries, this.startIndex), this.virtualPageSize);
      }
    }

    public int TotalEntries
    {
      get
      {
        if (this.leaderboard != null)
          return this.leaderboard.EntryCount;
        else
          return 0;
      }
    }

    public CachedLeaderboard(User activeGamer, int virtualPageSize)
    {
      this.virtualPageSize = virtualPageSize;
      this.ActiveGamer = activeGamer;
      CommunityExpress.Instance.Leaderboards.FindLeaderboard((OnLeaderboardRetrieved) (l =>
      {
        this.leaderboard = l;
        if (this.onLeaderboardFound != null)
          this.onLeaderboardFound();
        this.onLeaderboardFound = (Action) null;
      }), "CompletionPercentage");
    }

    private void CacheEntries(LeaderboardEntries entries, int pageSign)
    {
      if (entries != null)
      {
        int num1 = this.cachedEntries.Count == 0 || this.startIndex == -1 ? 0 : this.cachedEntries[this.startIndex].GlobalRank;
        if (pageSign == 0)
          this.cachedEntries.Clear();
        if (this.leaderboard != null)
        {
          switch (pageSign)
          {
            case -1:
              num1 = Math.Max(num1 - this.virtualPageSize, 1);
              using (IEnumerator<LeaderboardEntry> enumerator = Enumerable.Reverse<LeaderboardEntry>((IEnumerable<LeaderboardEntry>) entries).GetEnumerator())
              {
                while (enumerator.MoveNext())
                  this.cachedEntries.Insert(0, enumerator.Current);
                break;
              }
            case 1:
              num1 = Math.Min(num1 + this.virtualPageSize, this.TotalEntries);
              using (IEnumerator<LeaderboardEntry> enumerator = entries.GetEnumerator())
              {
                while (enumerator.MoveNext())
                  this.cachedEntries.Add(enumerator.Current);
                break;
              }
            default:
              using (IEnumerator<LeaderboardEntry> enumerator = entries.GetEnumerator())
              {
                while (enumerator.MoveNext())
                  this.cachedEntries.Add(enumerator.Current);
                break;
              }
          }
          int num2 = -1;
          for (int index = 0; index < this.cachedEntries.Count; ++index)
          {
            if (pageSign == 0)
            {
              if (this.cachedEntries[index].PersonaName == this.ActiveGamer.PersonaName)
              {
                num2 = Math.Max(index - this.virtualPageSize / 2, 0);
                break;
              }
            }
            else if (this.cachedEntries[index].GlobalRank == num1)
            {
              num2 = index;
              break;
            }
          }
          this.startIndex = pageSign != 0 ? num2 : (this.View == LeaderboardView.Overall ? 0 : num2);
        }
      }
      this.Reading = false;
      this.callback();
    }

    public void ChangeView(LeaderboardView leaderboardView, Action onFinished)
    {
      if (this.Reading)
        onFinished();
      else if (this.leaderboard == null)
      {
        this.onLeaderboardFound = (Action) (() => this.ChangeView(leaderboardView, onFinished));
      }
      else
      {
        this.Reading = true;
        this.View = leaderboardView;
        this.callback = onFinished;
        switch (this.View)
        {
          case LeaderboardView.Friends:
            this.leaderboard.RequestFriendLeaderboardEntries(0, (OnLeaderboardEntriesRetrieved) (e => this.CacheEntries(e, 0)));
            this.CanChangePage = false;
            break;
          case LeaderboardView.MyScore:
            this.leaderboard.RequestLeaderboardEntriesAroundCurrentUser(50, 50, 0, (OnLeaderboardEntriesRetrieved) (e => this.CacheEntries(e, 0)));
            this.CanChangePage = true;
            break;
          case LeaderboardView.Overall:
            this.leaderboard.RequestLeaderboardEntries(0, 100, 0, (OnLeaderboardEntriesRetrieved) (e => this.CacheEntries(e, 0)));
            this.CanChangePage = true;
            break;
        }
      }
    }

    public void PageUp(Action onFinished)
    {
      if (this.InError)
        onFinished();
      else if (this.startIndex > 0)
      {
        this.startIndex = Math.Max(this.startIndex - this.virtualPageSize, 0);
        onFinished();
      }
      else if (!this.CanPageUp)
      {
        onFinished();
      }
      else
      {
        this.callback = onFinished;
        this.leaderboard.RequestLeaderboardEntries(this.startIndex - 100, this.startIndex, 0, (OnLeaderboardEntriesRetrieved) (e => this.CacheEntries(e, -1)));
      }
    }

    public void PageDown(Action onFinished)
    {
      if (this.InError)
        onFinished();
      else if (this.startIndex < this.cachedEntries.Count - this.virtualPageSize)
      {
        this.startIndex += this.virtualPageSize;
        onFinished();
      }
      else if (!this.CanPageDown)
      {
        onFinished();
      }
      else
      {
        this.callback = onFinished;
        this.leaderboard.RequestLeaderboardEntries(this.startIndex + this.virtualPageSize, this.startIndex + this.virtualPageSize + 100, 0, (OnLeaderboardEntriesRetrieved) (e => this.CacheEntries(e, 1)));
      }
    }
  }
}
