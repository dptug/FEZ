// Type: FezEngine.Tools.RasterizerCombiner
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace FezEngine.Tools
{
  public class RasterizerCombiner
  {
    private readonly Dictionary<long, RasterizerState> stateObjectCache = new Dictionary<long, RasterizerState>();

    public CullMode CullMode { get; set; }

    public FillMode FillMode { get; set; }

    public float DepthBias { get; set; }

    public float SlopeScaleDepthBias { get; set; }

    public RasterizerState Current
    {
      get
      {
        return this.FindOrCreateStateObject(this.CalculateNewHash());
      }
    }

    internal void Apply(GraphicsDevice device)
    {
      long hash = this.CalculateNewHash();
      device.RasterizerState = this.FindOrCreateStateObject(hash);
    }

    private RasterizerState FindOrCreateStateObject(long hash)
    {
      RasterizerState rasterizerState;
      if (!this.stateObjectCache.TryGetValue(hash, out rasterizerState))
      {
        rasterizerState = new RasterizerState()
        {
          CullMode = this.CullMode,
          FillMode = this.FillMode,
          DepthBias = this.DepthBias,
          SlopeScaleDepthBias = this.SlopeScaleDepthBias
        };
        this.stateObjectCache.Add(hash, rasterizerState);
      }
      return rasterizerState;
    }

    private long CalculateNewHash()
    {
      return (long) ((int) BitConverter.DoubleToInt64Bits((double) this.DepthBias) >> 4 | (int) BitConverter.DoubleToInt64Bits((double) this.SlopeScaleDepthBias) >> 4 << 30 | (int) (byte) this.CullMode << 28 | (int) (byte) this.FillMode << 30);
    }
  }
}
