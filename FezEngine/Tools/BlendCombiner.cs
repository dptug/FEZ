// Type: FezEngine.Tools.BlendCombiner
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FezEngine.Tools
{
  public class BlendCombiner
  {
    private readonly Dictionary<short, BlendState> stateObjectCache = new Dictionary<short, BlendState>();
    private BlendingMode blendingMode = BlendingMode.Alphablending;
    private BlendFunction colorBlendFunction;
    private Blend colorSourceBlend;
    private Blend colorDestinationBlend;

    public BlendingMode BlendingMode
    {
      get
      {
        return this.blendingMode;
      }
      set
      {
        this.blendingMode = value;
        switch (value)
        {
          case BlendingMode.Additive:
            this.colorSourceBlend = BlendState.Additive.ColorSourceBlend;
            this.colorDestinationBlend = BlendState.Additive.ColorDestinationBlend;
            this.colorBlendFunction = BlendState.Additive.ColorBlendFunction;
            break;
          case BlendingMode.Screen:
            this.colorBlendFunction = BlendFunction.Add;
            this.colorSourceBlend = Blend.InverseDestinationColor;
            this.colorDestinationBlend = Blend.One;
            break;
          case BlendingMode.Multiply:
            this.colorBlendFunction = BlendFunction.Add;
            this.colorSourceBlend = Blend.DestinationColor;
            this.colorDestinationBlend = Blend.Zero;
            break;
          case BlendingMode.Alphablending:
            this.colorSourceBlend = BlendState.NonPremultiplied.ColorSourceBlend;
            this.colorDestinationBlend = BlendState.NonPremultiplied.ColorDestinationBlend;
            this.colorBlendFunction = BlendState.NonPremultiplied.ColorBlendFunction;
            break;
          case BlendingMode.Multiply2X:
            this.colorBlendFunction = BlendFunction.Add;
            this.colorSourceBlend = Blend.DestinationColor;
            this.colorDestinationBlend = Blend.SourceColor;
            break;
          case BlendingMode.Maximum:
            this.colorBlendFunction = BlendFunction.Max;
            this.colorSourceBlend = Blend.One;
            this.colorDestinationBlend = Blend.One;
            break;
          case BlendingMode.Minimum:
            this.colorBlendFunction = BlendFunction.Min;
            this.colorSourceBlend = Blend.One;
            this.colorDestinationBlend = Blend.One;
            break;
          case BlendingMode.Subtract:
            this.colorBlendFunction = BlendFunction.ReverseSubtract;
            this.colorSourceBlend = Blend.One;
            this.colorDestinationBlend = Blend.One;
            break;
          case BlendingMode.StarsOverClouds:
            this.colorBlendFunction = BlendFunction.Add;
            this.colorSourceBlend = Blend.One;
            this.colorDestinationBlend = Blend.InverseSourceColor;
            break;
          case BlendingMode.Opaque:
            this.colorSourceBlend = BlendState.Opaque.ColorSourceBlend;
            this.colorDestinationBlend = BlendState.Opaque.ColorDestinationBlend;
            this.colorBlendFunction = BlendState.Opaque.ColorBlendFunction;
            break;
        }
      }
    }

    public ColorWriteChannels ColorWriteChannels { get; set; }

    public BlendState Current
    {
      get
      {
        return this.FindOrCreateStateObject(this.CalculateNewHash());
      }
    }

    internal void Apply(GraphicsDevice device)
    {
      short hash = this.CalculateNewHash();
      device.BlendState = this.FindOrCreateStateObject(hash);
    }

    private BlendState FindOrCreateStateObject(short hash)
    {
      BlendState blendState;
      if (!this.stateObjectCache.TryGetValue(hash, out blendState))
      {
        blendState = new BlendState()
        {
          ColorBlendFunction = this.colorBlendFunction,
          ColorSourceBlend = this.colorSourceBlend,
          ColorDestinationBlend = this.colorDestinationBlend,
          ColorWriteChannels = this.ColorWriteChannels
        };
        this.stateObjectCache.Add(hash, blendState);
      }
      return blendState;
    }

    private short CalculateNewHash()
    {
      return (short) ((int) (byte) this.colorBlendFunction | (int) (byte) this.colorSourceBlend << 3 | (int) (byte) this.colorDestinationBlend << 7 | (int) (byte) this.ColorWriteChannels << 11);
    }
  }
}
