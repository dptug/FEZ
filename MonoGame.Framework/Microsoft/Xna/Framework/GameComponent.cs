// Type: Microsoft.Xna.Framework.GameComponent
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework.Graphics;
using System;

namespace Microsoft.Xna.Framework
{
  public class GameComponent : IGameComponent, IUpdateable, IComparable<GameComponent>, IDisposable
  {
    private bool _enabled = true;
    private int _updateOrder;

    public Game Game { get; private set; }

    public GraphicsDevice GraphicsDevice
    {
      get
      {
        return this.Game.GraphicsDevice;
      }
    }

    public bool Enabled
    {
      get
      {
        return this._enabled;
      }
      set
      {
        if (this._enabled == value)
          return;
        this._enabled = value;
        if (this.EnabledChanged != null)
          this.EnabledChanged((object) this, EventArgs.Empty);
        this.OnEnabledChanged((object) this, (EventArgs) null);
      }
    }

    public int UpdateOrder
    {
      get
      {
        return this._updateOrder;
      }
      set
      {
        if (this._updateOrder == value)
          return;
        this._updateOrder = value;
        if (this.UpdateOrderChanged != null)
          this.UpdateOrderChanged((object) this, EventArgs.Empty);
        this.OnUpdateOrderChanged((object) this, (EventArgs) null);
      }
    }

    public event EventHandler<EventArgs> EnabledChanged;

    public event EventHandler<EventArgs> UpdateOrderChanged;

    public event EventHandler<EventArgs> Disposed;

    public GameComponent(Game game)
    {
      this.Game = game;
    }

    ~GameComponent()
    {
      this.Dispose(false);
    }

    public virtual void Initialize()
    {
    }

    public virtual void Update(GameTime gameTime)
    {
    }

    protected virtual void OnUpdateOrderChanged(object sender, EventArgs args)
    {
    }

    protected virtual void OnEnabledChanged(object sender, EventArgs args)
    {
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!disposing || this.Disposed == null)
        return;
      this.Disposed((object) this, EventArgs.Empty);
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    public int CompareTo(GameComponent other)
    {
      return other.UpdateOrder - this.UpdateOrder;
    }
  }
}
