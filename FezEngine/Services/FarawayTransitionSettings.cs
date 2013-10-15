// Type: FezEngine.Services.FarawayTransitionSettings
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FezEngine.Services
{
  public class FarawayTransitionSettings
  {
    public bool InTransition;
    public bool LoadingAllowed;
    public float TransitionStep;
    public float OriginFadeOutStep;
    public float DestinationCrossfadeStep;
    public float InterpolatedFakeRadius;
    public float DestinationRadius;
    public float DestinationPixelsPerTrixel;
    public Vector2 DestinationOffset;
    public RenderTarget2D SkyRt;

    public void Reset()
    {
      this.OriginFadeOutStep = 0.0f;
      this.DestinationCrossfadeStep = 0.0f;
      this.TransitionStep = 0.0f;
      this.LoadingAllowed = this.InTransition = false;
      this.InterpolatedFakeRadius = 0.0f;
      this.DestinationRadius = 0.0f;
      this.DestinationPixelsPerTrixel = 0.0f;
      this.DestinationOffset = Vector2.Zero;
      this.SkyRt = (RenderTarget2D) null;
    }
  }
}
