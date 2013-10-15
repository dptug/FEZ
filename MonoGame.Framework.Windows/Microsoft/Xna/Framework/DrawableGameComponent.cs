// Type: Microsoft.Xna.Framework.DrawableGameComponent
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System;

namespace Microsoft.Xna.Framework
{
  public class DrawableGameComponent : GameComponent, IDrawable
  {
    private bool _initialized;
    private int _drawOrder;
    private bool _visible;

    public int DrawOrder
    {
      get
      {
        return this._drawOrder;
      }
      set
      {
        if (this._drawOrder == value)
          return;
        this._drawOrder = value;
        if (this.DrawOrderChanged != null)
          this.DrawOrderChanged((object) this, (EventArgs) null);
        this.OnDrawOrderChanged((object) this, (EventArgs) null);
      }
    }

    public bool Visible
    {
      get
      {
        return this._visible;
      }
      set
      {
        if (this._visible == value)
          return;
        this._visible = value;
        if (this.VisibleChanged != null)
          this.VisibleChanged((object) this, EventArgs.Empty);
        this.OnVisibleChanged((object) this, EventArgs.Empty);
      }
    }

    public event EventHandler<EventArgs> DrawOrderChanged;

    public event EventHandler<EventArgs> VisibleChanged;

    public DrawableGameComponent(Game game)
      : base(game)
    {
      this.Visible = true;
    }

    public override void Initialize()
    {
      if (this._initialized)
        return;
      this._initialized = true;
      this.LoadContent();
    }

    protected virtual void LoadContent()
    {
    }

    protected virtual void UnloadContent()
    {
    }

    public virtual void Draw(GameTime gameTime)
    {
    }

    protected virtual void OnVisibleChanged(object sender, EventArgs args)
    {
    }

    protected virtual void OnDrawOrderChanged(object sender, EventArgs args)
    {
    }
  }
}
