// Type: FezEngine.Components.VaryingColor
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Tools;
using Microsoft.Xna.Framework;
using System;

namespace FezEngine.Components
{
  public class VaryingColor : VaryingValue<Color>
  {
    protected override Func<Color, Color, Color> DefaultFunction
    {
      get
      {
        return (Func<Color, Color, Color>) ((b, v) =>
        {
          if (v == new Color(0, 0, 0, 0))
            return b;
          else
            return new Color((float) b.R / (float) byte.MaxValue + RandomHelper.Centered((double) v.R / (double) byte.MaxValue), (float) b.G / (float) byte.MaxValue + RandomHelper.Centered((double) v.G / (double) byte.MaxValue), (float) b.B / (float) byte.MaxValue + RandomHelper.Centered((double) v.B / (double) byte.MaxValue), (float) b.A / (float) byte.MaxValue + RandomHelper.Centered((double) v.A / (double) byte.MaxValue));
        });
      }
    }

    public static Func<Color, Color, Color> Uniform
    {
      get
      {
        return (Func<Color, Color, Color>) ((b, v) =>
        {
          Vector4 local_0 = b.ToVector4();
          Vector4 local_1 = v.ToVector4();
          float local_2 = RandomHelper.Centered(1.0);
          return new Color(new Vector4(local_0.X + local_2 * local_1.X, local_0.Y + local_2 * local_1.Y, local_0.Z + local_2 * local_1.Z, local_0.W + local_2 * local_1.W));
        });
      }
    }

    public static implicit operator VaryingColor(Color value)
    {
      VaryingColor varyingColor = new VaryingColor();
      varyingColor.Base = value;
      return varyingColor;
    }
  }
}
