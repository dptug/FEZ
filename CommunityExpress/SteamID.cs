// Type: CommunityExpressNS.SteamID
// Assembly: CommunityExpress, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3B3F745C-AA2A-4DDF-AA8A-F5898AF84B8D
// Assembly location: F:\Program Files (x86)\FEZ\CommunityExpress.dll

namespace CommunityExpressNS
{
  public class SteamID
  {
    private ulong _id;

    public uint AccountID
    {
      get
      {
        return (uint) (this._id & (ulong) uint.MaxValue);
      }
    }

    public uint AccountInstance
    {
      get
      {
        return (uint) (this._id >> 32 & 1048575UL);
      }
    }

    public EAccountType AccountType
    {
      get
      {
        return (EAccountType) ((long) (this._id >> 52) & 15L);
      }
    }

    public EUniverse Universe
    {
      get
      {
        return (EUniverse) ((long) (this._id >> 56) & (long) byte.MaxValue);
      }
    }

    public SteamID(ulong id)
    {
      this._id = id;
    }

    public static bool operator ==(SteamID a, SteamID b)
    {
      if (object.ReferenceEquals((object) a, (object) b))
        return true;
      if (object.ReferenceEquals((object) a, (object) null) || object.ReferenceEquals((object) b, (object) null))
        return false;
      else
        return (long) a.ToUInt64() == (long) b.ToUInt64();
    }

    public static bool operator !=(SteamID a, SteamID b)
    {
      return !(a == b);
    }

    public static bool operator ==(SteamID a, ulong b)
    {
      if (object.ReferenceEquals((object) a, (object) null))
        return (long) b == 0L;
      else
        return (long) a.ToUInt64() == (long) b;
    }

    public static bool operator !=(SteamID a, ulong b)
    {
      return !(a == b);
    }

    public ulong ToUInt64()
    {
      return this._id;
    }

    public override string ToString()
    {
      return this._id.ToString();
    }

    public override bool Equals(object obj)
    {
      SteamID steamId = obj as SteamID;
      if (steamId == null)
        return false;
      else
        return (long) this._id == (long) steamId.ToUInt64();
    }

    public override int GetHashCode()
    {
      return base.GetHashCode() ^ (int) this._id;
    }
  }
}
