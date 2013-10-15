// Type: CommunityExpressNS.Stat
// Assembly: CommunityExpress, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3B3F745C-AA2A-4DDF-AA8A-F5898AF84B8D
// Assembly location: F:\Program Files (x86)\FEZ\CommunityExpress.dll

namespace CommunityExpressNS
{
  public class Stat
  {
    private Stats _stats;
    private string _statName;
    private object _statValue;
    private bool _hasChanged;

    public string StatName
    {
      get
      {
        return this._statName;
      }
      private set
      {
        this._statName = value;
      }
    }

    public object StatValue
    {
      get
      {
        return this._statValue;
      }
      set
      {
        this._statValue = value;
        this._hasChanged = true;
      }
    }

    public bool HasChanged
    {
      get
      {
        return this._hasChanged;
      }
      internal set
      {
        this._hasChanged = value;
      }
    }

    public Stat(Stats stats)
    {
      this._stats = stats;
    }

    public Stat(Stats stats, string statName, object statValue)
    {
      this._stats = stats;
      this._statName = statName;
      this._statValue = statValue;
      this._hasChanged = false;
    }
  }
}
