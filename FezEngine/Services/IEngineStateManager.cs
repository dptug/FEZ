// Type: FezEngine.Services.IEngineStateManager
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Microsoft.Xna.Framework;
using System;

namespace FezEngine.Services
{
  public interface IEngineStateManager
  {
    bool Paused { get; }

    bool InMap { get; }

    bool InMenuCube { get; }

    float FramesPerSecond { get; set; }

    bool LoopRender { get; set; }

    bool SkyRender { get; set; }

    bool Loading { get; set; }

    bool InEditor { get; set; }

    bool TimePaused { get; }

    float SkyOpacity { get; set; }

    bool SkipRendering { get; set; }

    float WaterLevelOffset { get; }

    bool StereoMode { get; set; }

    bool DotLoading { get; set; }

    Vector3 WaterBodyColor { get; set; }

    Vector3 WaterFoamColor { get; set; }

    FarawayTransitionSettings FarawaySettings { get; }

    event Action PauseStateChanged;
  }
}
