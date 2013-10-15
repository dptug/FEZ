// Type: Microsoft.Xna.Framework.Net.AvailableNetworkSession
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System.Net;

namespace Microsoft.Xna.Framework.Net
{
  public sealed class AvailableNetworkSession
  {
    private int _currentGameCount;
    private string _hostGamertag;
    private int _openPrivateGamerSlots;
    private int _openPublicGamerSlots;
    private QualityOfService _QualityOfService;
    private NetworkSessionProperties _sessionProperties;
    private IPEndPoint _endPoint;
    private IPEndPoint _internalendPoint;

    public int CurrentGamerCount
    {
      get
      {
        return this._currentGameCount;
      }
      internal set
      {
        this._currentGameCount = value;
      }
    }

    public string HostGamertag
    {
      get
      {
        return this._hostGamertag;
      }
      internal set
      {
        this._hostGamertag = value;
      }
    }

    public int OpenPrivateGamerSlots
    {
      get
      {
        return this._openPrivateGamerSlots;
      }
      internal set
      {
        this._openPrivateGamerSlots = value;
      }
    }

    public int OpenPublicGamerSlots
    {
      get
      {
        return this._openPublicGamerSlots;
      }
      internal set
      {
        this._openPublicGamerSlots = value;
      }
    }

    public QualityOfService QualityOfService
    {
      get
      {
        return this._QualityOfService;
      }
      internal set
      {
        this._QualityOfService = value;
      }
    }

    public NetworkSessionProperties SessionProperties
    {
      get
      {
        return this._sessionProperties;
      }
      internal set
      {
        this._sessionProperties = value;
      }
    }

    internal IPEndPoint EndPoint
    {
      get
      {
        return this._endPoint;
      }
      set
      {
        this._endPoint = value;
      }
    }

    internal IPEndPoint InternalEndpont
    {
      get
      {
        return this._internalendPoint;
      }
      set
      {
        this._internalendPoint = value;
      }
    }

    internal NetworkSessionType SessionType { get; set; }

    public AvailableNetworkSession()
    {
      this._QualityOfService = new QualityOfService();
    }
  }
}
