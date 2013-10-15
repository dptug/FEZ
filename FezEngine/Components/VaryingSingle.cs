// Type: FezEngine.Components.VaryingSingle
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Tools;
using System;

namespace FezEngine.Components
{
  public class VaryingSingle : VaryingValue<float>
  {
    protected override Func<float, float, float> DefaultFunction
    {
      get
      {
        return (Func<float, float, float>) ((b, v) =>
        {
          if ((double) v != 0.0)
            return b + RandomHelper.Centered((double) v);
          else
            return b;
        });
      }
    }

    public static implicit operator VaryingSingle(float value)
    {
      VaryingSingle varyingSingle = new VaryingSingle();
      varyingSingle.Base = value;
      return varyingSingle;
    }
  }
}
