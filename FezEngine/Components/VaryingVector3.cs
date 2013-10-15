// Type: FezEngine.Components.VaryingVector3
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Tools;
using Microsoft.Xna.Framework;
using System;

namespace FezEngine.Components
{
  public class VaryingVector3 : VaryingValue<Vector3>
  {
    protected override Func<Vector3, Vector3, Vector3> DefaultFunction
    {
      get
      {
        return (Func<Vector3, Vector3, Vector3>) ((b, v) =>
        {
          if (v == Vector3.Zero)
            return b;
          else
            return b + new Vector3(RandomHelper.Centered((double) v.X), RandomHelper.Centered((double) v.Y), RandomHelper.Centered((double) v.Z));
        });
      }
    }

    public static Func<Vector3, Vector3, Vector3> Uniform
    {
      get
      {
        return (Func<Vector3, Vector3, Vector3>) ((b, v) =>
        {
          float local_0 = RandomHelper.Centered(1.0);
          return new Vector3(b.X + local_0 * v.X, b.Y + local_0 * v.Y, b.Z + local_0 * v.Z);
        });
      }
    }

    public static Func<Vector3, Vector3, Vector3> ClampToTrixels
    {
      get
      {
        return (Func<Vector3, Vector3, Vector3>) ((b, v) => FezMath.Round((b + new Vector3(RandomHelper.Centered((double) v.X), RandomHelper.Centered((double) v.Y), RandomHelper.Centered((double) v.Z))) * 16f) / 16f);
      }
    }

    public static implicit operator VaryingVector3(Vector3 value)
    {
      VaryingVector3 varyingVector3 = new VaryingVector3();
      varyingVector3.Base = value;
      return varyingVector3;
    }
  }
}
