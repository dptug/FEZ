// Type: Microsoft.Xna.Framework.Graphics.RenderTargetBinding
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System;

namespace Microsoft.Xna.Framework.Graphics
{
  public struct RenderTargetBinding
  {
    private Texture _renderTarget;
    private CubeMapFace _cubeMapFace;

    public Texture RenderTarget
    {
      get
      {
        return this._renderTarget;
      }
    }

    public CubeMapFace CubeMapFace
    {
      get
      {
        return this._cubeMapFace;
      }
    }

    public RenderTargetBinding(RenderTarget2D renderTarget)
    {
      if (renderTarget == null)
        throw new ArgumentNullException("renderTarget");
      this._renderTarget = (Texture) renderTarget;
      this._cubeMapFace = CubeMapFace.PositiveX;
    }

    public RenderTargetBinding(RenderTargetCube renderTarget, CubeMapFace cubeMapFace)
    {
      if (renderTarget == null)
        throw new ArgumentNullException("renderTarget");
      if (cubeMapFace < CubeMapFace.PositiveX || cubeMapFace > CubeMapFace.NegativeZ)
        throw new ArgumentOutOfRangeException("cubeMapFace");
      this._renderTarget = (Texture) renderTarget;
      this._cubeMapFace = cubeMapFace;
    }

    public static implicit operator RenderTargetBinding(RenderTarget2D renderTarget)
    {
      return new RenderTargetBinding(renderTarget);
    }
  }
}
