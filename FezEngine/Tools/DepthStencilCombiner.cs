// Type: FezEngine.Tools.DepthStencilCombiner
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FezEngine.Tools
{
  public class DepthStencilCombiner
  {
    private readonly Dictionary<int, DepthStencilState> stateObjectCache = new Dictionary<int, DepthStencilState>();

    public bool DepthBufferEnable { get; set; }

    public CompareFunction DepthBufferFunction { get; set; }

    public bool DepthBufferWriteEnable { get; set; }

    public bool StencilEnable { get; set; }

    public StencilOperation StencilPass { get; set; }

    public CompareFunction StencilFunction { get; set; }

    public int ReferenceStencil { get; set; }

    public DepthStencilState Current
    {
      get
      {
        return this.FindOrCreateStateObject(this.CalculateNewHash());
      }
    }

    internal void Apply(GraphicsDevice device)
    {
      int hash = this.CalculateNewHash();
      device.DepthStencilState = this.FindOrCreateStateObject(hash);
    }

    private DepthStencilState FindOrCreateStateObject(int hash)
    {
      DepthStencilState depthStencilState;
      if (!this.stateObjectCache.TryGetValue(hash, out depthStencilState))
      {
        depthStencilState = new DepthStencilState()
        {
          DepthBufferEnable = this.DepthBufferEnable,
          DepthBufferWriteEnable = this.DepthBufferWriteEnable,
          DepthBufferFunction = this.DepthBufferFunction,
          StencilEnable = this.StencilEnable,
          StencilPass = this.StencilPass,
          StencilFunction = this.StencilFunction,
          ReferenceStencil = this.ReferenceStencil
        };
        this.stateObjectCache.Add(hash, depthStencilState);
      }
      return depthStencilState;
    }

    private int CalculateNewHash()
    {
      return (this.DepthBufferEnable ? 1 : 0) | (int) (byte) this.DepthBufferFunction << 1 | (this.DepthBufferWriteEnable ? 1 : 0) << 5 | (this.StencilEnable ? 1 : 0) << 6 | (int) (byte) this.StencilPass << 7 | (int) (byte) this.StencilFunction << 11 | (int) (byte) this.ReferenceStencil << 15;
    }
  }
}
