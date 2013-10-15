// Type: FezEngine.Tools.GraphicsDeviceExtensions
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Structure;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FezEngine.Tools
{
  public static class GraphicsDeviceExtensions
  {
    private static readonly DepthStencilCombiner dssCombiner = new DepthStencilCombiner();
    private static readonly BlendCombiner blendCombiner = new BlendCombiner();
    private static readonly RasterizerCombiner rasterCombiner = new RasterizerCombiner();

    static GraphicsDeviceExtensions()
    {
    }

    public static void BeginPoint(this SpriteBatch spriteBatch)
    {
      spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, (DepthStencilState) null, RasterizerState.CullCounterClockwise);
    }

    public static void BeginLinear(this SpriteBatch spriteBatch)
    {
      spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.LinearClamp, (DepthStencilState) null, RasterizerState.CullCounterClockwise);
    }

    public static void SetGamma(this GraphicsDevice device, float brightness)
    {
      double num1 = ((double) brightness - 0.5) * 0.25;
      double num2 = (double) brightness - 0.5 + 1.0;
      short[] numArray1 = new short[256];
      short[] numArray2 = new short[256];
      short[] numArray3 = new short[256];
      for (int index = 0; index < 256; ++index)
      {
        ushort num3 = (ushort) Math.Round(Math.Max(Math.Min(Math.Pow((double) index / (double) byte.MaxValue, 1.0 / num2) + num1, 1.0), 0.0) * (double) ushort.MaxValue);
        numArray1[index] = numArray2[index] = numArray3[index] = (short) num3;
      }
    }

    public static void PrepareDraw(this GraphicsDevice device)
    {
      device.DepthStencilState = DepthStencilStates.DefaultWithStencil;
      device.BlendState = BlendState.NonPremultiplied;
      device.SamplerStates[0] = SamplerState.PointClamp;
      device.RasterizerState = RasterizerState.CullCounterClockwise;
    }

    public static DepthStencilCombiner GetDssCombiner(this GraphicsDevice _)
    {
      return GraphicsDeviceExtensions.dssCombiner;
    }

    public static void PrepareStencilWrite(this GraphicsDevice _, StencilMask? reference)
    {
      GraphicsDeviceExtensions.dssCombiner.StencilEnable = true;
      GraphicsDeviceExtensions.dssCombiner.StencilPass = StencilOperation.Replace;
      GraphicsDeviceExtensions.dssCombiner.StencilFunction = CompareFunction.Always;
      if (!reference.HasValue)
        return;
      GraphicsDeviceExtensions.dssCombiner.ReferenceStencil = (int) reference.Value;
    }

    public static void PrepareStencilRead(this GraphicsDevice _, CompareFunction comparison, StencilMask reference)
    {
      GraphicsDeviceExtensions.dssCombiner.StencilEnable = true;
      GraphicsDeviceExtensions.dssCombiner.StencilPass = StencilOperation.Keep;
      GraphicsDeviceExtensions.dssCombiner.StencilFunction = comparison;
      GraphicsDeviceExtensions.dssCombiner.ReferenceStencil = (int) reference;
    }

    public static void PrepareStencilReadWrite(this GraphicsDevice _, CompareFunction comparison, StencilMask reference)
    {
      GraphicsDeviceExtensions.dssCombiner.StencilEnable = true;
      GraphicsDeviceExtensions.dssCombiner.StencilPass = StencilOperation.Replace;
      GraphicsDeviceExtensions.dssCombiner.StencilFunction = comparison;
      GraphicsDeviceExtensions.dssCombiner.ReferenceStencil = (int) reference;
    }

    public static BlendCombiner GetBlendCombiner(this GraphicsDevice _)
    {
      return GraphicsDeviceExtensions.blendCombiner;
    }

    public static RasterizerCombiner GetRasterCombiner(this GraphicsDevice _)
    {
      return GraphicsDeviceExtensions.rasterCombiner;
    }

    public static void ApplyCombiners(this GraphicsDevice device)
    {
      GraphicsDeviceExtensions.dssCombiner.Apply(device);
      GraphicsDeviceExtensions.blendCombiner.Apply(device);
      GraphicsDeviceExtensions.rasterCombiner.Apply(device);
    }

    public static void SetCullMode(this GraphicsDevice _, CullMode cullMode)
    {
      GraphicsDeviceExtensions.rasterCombiner.CullMode = cullMode;
    }

    public static void SetBlendingMode(this GraphicsDevice _, BlendingMode blendingMode)
    {
      GraphicsDeviceExtensions.blendCombiner.BlendingMode = blendingMode;
    }

    public static void SetColorWriteChannels(this GraphicsDevice _, ColorWriteChannels channels)
    {
      GraphicsDeviceExtensions.blendCombiner.ColorWriteChannels = channels;
    }
  }
}
