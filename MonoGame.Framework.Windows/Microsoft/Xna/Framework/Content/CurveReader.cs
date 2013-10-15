// Type: Microsoft.Xna.Framework.Content.CurveReader
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using Microsoft.Xna.Framework;

namespace Microsoft.Xna.Framework.Content
{
  internal class CurveReader : ContentTypeReader<Curve>
  {
    protected internal override Curve Read(ContentReader input, Curve existingInstance)
    {
      Curve curve = existingInstance ?? new Curve();
      curve.PreLoop = (CurveLoopType) input.ReadInt32();
      curve.PostLoop = (CurveLoopType) input.ReadInt32();
      int num1 = input.ReadInt32();
      for (int index = 0; index < num1; ++index)
      {
        float position = input.ReadSingle();
        float num2 = input.ReadSingle();
        float tangentIn = input.ReadSingle();
        float tangentOut = input.ReadSingle();
        CurveContinuity continuity = (CurveContinuity) input.ReadInt32();
        curve.Keys.Add(new CurveKey(position, num2, tangentIn, tangentOut, continuity));
      }
      return curve;
    }
  }
}
