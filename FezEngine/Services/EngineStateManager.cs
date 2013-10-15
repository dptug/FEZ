// Type: FezEngine.Services.EngineStateManager
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Microsoft.Xna.Framework;
using System;

namespace FezEngine.Services
{
  public abstract class EngineStateManager : IEngineStateManager
  {
    protected bool paused;
    protected bool inMap;
    protected bool inMenuCube;
    private bool loading;

    public bool Paused
    {
      get
      {
        return this.paused;
      }
    }

    public bool InMap
    {
      get
      {
        return this.inMap;
      }
    }

    public bool InMenuCube
    {
      get
      {
        return this.inMenuCube;
      }
    }

    public abstract bool TimePaused { get; }

    public float FramesPerSecond { get; set; }

    public bool LoopRender { get; set; }

    public bool SkyRender { get; set; }

    public bool Loading
    {
      get
      {
        return this.loading;
      }
      set
      {
        this.loading = value;
      }
    }

    public virtual float WaterLevelOffset
    {
      get
      {
        return 0.0f;
      }
    }

    public Vector3 WaterBodyColor { get; set; }

    public Vector3 WaterFoamColor { get; set; }

    public bool InEditor { get; set; }

    public float SkyOpacity { get; set; }

    public bool SkipRendering { get; set; }

    public bool StereoMode { get; set; }

    public bool DotLoading { get; set; }

    public FarawayTransitionSettings FarawaySettings { get; private set; }

    public event Action PauseStateChanged;

    public EngineStateManager()
    {
      this.FarawaySettings = new FarawayTransitionSettings();
      this.SkyOpacity = 1f;
    }

    protected void OnPauseStateChanged()
    {
      this.PauseStateChanged();
    }
  }
}
