// Type: FezEngine.Services.CameraManager
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using Microsoft.Xna.Framework;
using System;

namespace FezEngine.Services
{
  public abstract class CameraManager : GameComponent, ICameraProvider
  {
    protected Matrix view = Matrix.Identity;
    protected Matrix projection = Matrix.Identity;

    public Matrix View
    {
      get
      {
        return this.view;
      }
    }

    public Matrix Projection
    {
      get
      {
        return this.projection;
      }
    }

    public event Action ViewChanged = new Action(Util.NullAction);

    public event Action ProjectionChanged = new Action(Util.NullAction);

    protected CameraManager(Game game)
      : base(game)
    {
      this.UpdateOrder = 10;
    }

    protected virtual void OnViewChanged()
    {
      this.ViewChanged();
    }

    protected virtual void OnProjectionChanged()
    {
      this.ProjectionChanged();
    }
  }
}
