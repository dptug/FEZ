// Type: FezGame.Services.Scripting.PlaneService
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Components.Scripting;
using FezEngine.Services;
using FezEngine.Services.Scripting;
using FezEngine.Structure;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using System;

namespace FezGame.Services.Scripting
{
  internal class PlaneService : IPlaneService, IScriptingBase
  {
    [ServiceDependency]
    public ILevelManager LevelManager { get; set; }

    public void ResetEvents()
    {
    }

    public LongRunningAction FadeIn(int id, float seconds)
    {
      float wasOpacity = this.LevelManager.BackgroundPlanes[id].Opacity;
      return new LongRunningAction((Func<float, float, bool>) ((_, elapsedSeconds) =>
      {
        BackgroundPlane local_0;
        if (!this.LevelManager.BackgroundPlanes.TryGetValue(id, out local_0))
          return true;
        local_0.Opacity = MathHelper.Lerp(wasOpacity, 1f, FezMath.Saturate(elapsedSeconds / seconds));
        return (double) local_0.Opacity == 1.0;
      }));
    }

    public LongRunningAction FadeOut(int id, float seconds)
    {
      float wasOpacity = this.LevelManager.BackgroundPlanes[id].Opacity;
      return new LongRunningAction((Func<float, float, bool>) ((_, elapsedSeconds) =>
      {
        BackgroundPlane local_0;
        if (!this.LevelManager.BackgroundPlanes.TryGetValue(id, out local_0))
          return true;
        local_0.Opacity = MathHelper.Lerp(wasOpacity, 0.0f, FezMath.Saturate(elapsedSeconds / seconds));
        return (double) local_0.Opacity == 0.0;
      }));
    }

    public LongRunningAction Flicker(int id, float factor)
    {
      Vector3 baseScale = this.LevelManager.BackgroundPlanes[id].Scale;
      return new LongRunningAction((Func<float, float, bool>) ((elapsedSeconds, _) =>
      {
        if (RandomHelper.Probability(0.25))
        {
          BackgroundPlane local_0;
          if (!this.LevelManager.BackgroundPlanes.TryGetValue(id, out local_0))
            return true;
          local_0.Scale = baseScale + new Vector3(RandomHelper.Centered((double) factor));
        }
        return false;
      }));
    }
  }
}
