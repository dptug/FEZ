// Type: FezEngine.Services.FogManager
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using FezEngine;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using System;

namespace FezEngine.Services
{
  public class FogManager : GameComponent, IFogManager
  {
    private FogType type;
    private Color color;
    private float density;
    private float start;
    private float end;

    public FogType Type
    {
      get
      {
        return this.type;
      }
      set
      {
        this.type = value;
        this.FogSettingsChanged();
      }
    }

    public Color Color
    {
      get
      {
        return this.color;
      }
      set
      {
        if (!(this.color != value))
          return;
        this.color = value;
        this.FogSettingsChanged();
      }
    }

    public float Density
    {
      get
      {
        return this.density;
      }
      set
      {
        this.density = value;
        this.FogSettingsChanged();
      }
    }

    public float Start
    {
      get
      {
        return this.start;
      }
      set
      {
        this.start = value;
        this.FogSettingsChanged();
      }
    }

    public float End
    {
      get
      {
        return this.end;
      }
      set
      {
        this.end = value;
        this.FogSettingsChanged();
      }
    }

    [ServiceDependency]
    public ILevelManager LevelManager { private get; set; }

    public event Action FogSettingsChanged = new Action(Util.NullAction);

    public FogManager(Game game)
      : base(game)
    {
    }

    public override void Initialize()
    {
      this.LevelManager.LevelChanged += (Action) (() =>
      {
        this.type = FogType.ExponentialSquared;
        if (this.LevelManager.Sky == null)
          return;
        this.density = this.LevelManager.Sky.FogDensity;
        this.FogSettingsChanged();
      });
    }
  }
}
