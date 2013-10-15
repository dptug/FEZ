// Type: Microsoft.Xna.Framework.Curve
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System;

namespace Microsoft.Xna.Framework
{
  [Serializable]
  public class Curve
  {
    private CurveKeyCollection keys;
    private CurveLoopType postLoop;
    private CurveLoopType preLoop;

    public bool IsConstant
    {
      get
      {
        return this.keys.Count <= 1;
      }
    }

    public CurveKeyCollection Keys
    {
      get
      {
        return this.keys;
      }
    }

    public CurveLoopType PostLoop
    {
      get
      {
        return this.postLoop;
      }
      set
      {
        this.postLoop = value;
      }
    }

    public CurveLoopType PreLoop
    {
      get
      {
        return this.preLoop;
      }
      set
      {
        this.preLoop = value;
      }
    }

    public Curve()
    {
      this.keys = new CurveKeyCollection();
    }

    public Curve Clone()
    {
      return new Curve()
      {
        keys = this.keys.Clone(),
        preLoop = this.preLoop,
        postLoop = this.postLoop
      };
    }

    public float Evaluate(float position)
    {
      CurveKey curveKey1 = this.keys[0];
      CurveKey curveKey2 = this.keys[this.keys.Count - 1];
      if ((double) position < (double) curveKey1.Position)
      {
        switch (this.PreLoop)
        {
          case CurveLoopType.Constant:
            return curveKey1.Value;
          case CurveLoopType.Cycle:
            int numberOfCycle1 = this.GetNumberOfCycle(position);
            return this.GetCurvePosition(position - (float) numberOfCycle1 * (curveKey2.Position - curveKey1.Position));
          case CurveLoopType.CycleOffset:
            int numberOfCycle2 = this.GetNumberOfCycle(position);
            return this.GetCurvePosition(position - (float) numberOfCycle2 * (curveKey2.Position - curveKey1.Position)) + (float) numberOfCycle2 * (curveKey2.Value - curveKey1.Value);
          case CurveLoopType.Oscillate:
            int numberOfCycle3 = this.GetNumberOfCycle(position);
            return this.GetCurvePosition(0.0 != (double) numberOfCycle3 % 2.0 ? (float) ((double) curveKey2.Position - (double) position + (double) curveKey1.Position + (double) numberOfCycle3 * ((double) curveKey2.Position - (double) curveKey1.Position)) : position - (float) numberOfCycle3 * (curveKey2.Position - curveKey1.Position));
          case CurveLoopType.Linear:
            return curveKey1.Value - curveKey1.TangentIn * (curveKey1.Position - position);
        }
      }
      else if ((double) position > (double) curveKey2.Position)
      {
        switch (this.PostLoop)
        {
          case CurveLoopType.Constant:
            return curveKey2.Value;
          case CurveLoopType.Cycle:
            int numberOfCycle4 = this.GetNumberOfCycle(position);
            return this.GetCurvePosition(position - (float) numberOfCycle4 * (curveKey2.Position - curveKey1.Position));
          case CurveLoopType.CycleOffset:
            int numberOfCycle5 = this.GetNumberOfCycle(position);
            return this.GetCurvePosition(position - (float) numberOfCycle5 * (curveKey2.Position - curveKey1.Position)) + (float) numberOfCycle5 * (curveKey2.Value - curveKey1.Value);
          case CurveLoopType.Oscillate:
            int numberOfCycle6 = this.GetNumberOfCycle(position);
            float num = position - (float) numberOfCycle6 * (curveKey2.Position - curveKey1.Position);
            return this.GetCurvePosition(0.0 != (double) numberOfCycle6 % 2.0 ? (float) ((double) curveKey2.Position - (double) position + (double) curveKey1.Position + (double) numberOfCycle6 * ((double) curveKey2.Position - (double) curveKey1.Position)) : position - (float) numberOfCycle6 * (curveKey2.Position - curveKey1.Position));
          case CurveLoopType.Linear:
            return curveKey2.Value + curveKey1.TangentOut * (position - curveKey2.Position);
        }
      }
      return this.GetCurvePosition(position);
    }

    public void ComputeTangents(CurveTangent tangentType)
    {
      this.ComputeTangents(tangentType, tangentType);
    }

    public void ComputeTangents(CurveTangent tangentInType, CurveTangent tangentOutType)
    {
      for (int keyIndex = 0; keyIndex < this.Keys.Count; ++keyIndex)
        this.ComputeTangent(keyIndex, tangentInType, tangentOutType);
    }

    public void ComputeTangent(int keyIndex, CurveTangent tangentType)
    {
      this.ComputeTangent(keyIndex, tangentType, tangentType);
    }

    public void ComputeTangent(int keyIndex, CurveTangent tangentInType, CurveTangent tangentOutType)
    {
      CurveKey curveKey = this.keys[keyIndex];
      double num1;
      float num2 = (float) (num1 = (double) curveKey.Position);
      float num3 = (float) num1;
      float num4 = (float) num1;
      double num5;
      float num6 = (float) (num5 = (double) curveKey.Value);
      float num7 = (float) num5;
      float num8 = (float) num5;
      if (keyIndex > 0)
      {
        num4 = this.keys[keyIndex - 1].Position;
        num8 = this.keys[keyIndex - 1].Value;
      }
      if (keyIndex < this.keys.Count - 1)
      {
        num2 = this.keys[keyIndex + 1].Position;
        num6 = this.keys[keyIndex + 1].Value;
      }
      switch (tangentInType)
      {
        case CurveTangent.Flat:
          curveKey.TangentIn = 0.0f;
          break;
        case CurveTangent.Linear:
          curveKey.TangentIn = num7 - num8;
          break;
        case CurveTangent.Smooth:
          float num9 = num2 - num4;
          curveKey.TangentIn = (double) Math.Abs(num9) >= 1.40129846432482E-45 ? (float) (((double) num6 - (double) num8) * (((double) num3 - (double) num4) / (double) num9)) : 0.0f;
          break;
      }
      switch (tangentOutType)
      {
        case CurveTangent.Flat:
          curveKey.TangentOut = 0.0f;
          break;
        case CurveTangent.Linear:
          curveKey.TangentOut = num6 - num7;
          break;
        case CurveTangent.Smooth:
          float num10 = num2 - num4;
          if ((double) Math.Abs(num10) < 1.40129846432482E-45)
          {
            curveKey.TangentOut = 0.0f;
            break;
          }
          else
          {
            curveKey.TangentOut = (float) (((double) num6 - (double) num8) * (((double) num2 - (double) num3) / (double) num10));
            break;
          }
      }
    }

    private int GetNumberOfCycle(float position)
    {
      float num = (float) (((double) position - (double) this.keys[0].Position) / ((double) this.keys[this.keys.Count - 1].Position - (double) this.keys[0].Position));
      if ((double) num < 0.0)
        --num;
      return (int) num;
    }

    private float GetCurvePosition(float position)
    {
      CurveKey curveKey1 = this.keys[0];
      for (int index = 1; index < this.keys.Count; ++index)
      {
        CurveKey curveKey2 = this.Keys[index];
        if ((double) curveKey2.Position >= (double) position)
        {
          if (curveKey1.Continuity == CurveContinuity.Step)
          {
            if ((double) position >= 1.0)
              return curveKey2.Value;
            else
              return curveKey1.Value;
          }
          else
          {
            float num1 = (float) (((double) position - (double) curveKey1.Position) / ((double) curveKey2.Position - (double) curveKey1.Position));
            float num2 = num1 * num1;
            float num3 = num2 * num1;
            return (float) ((2.0 * (double) num3 - 3.0 * (double) num2 + 1.0) * (double) curveKey1.Value + ((double) num3 - 2.0 * (double) num2 + (double) num1) * (double) curveKey1.TangentOut + (3.0 * (double) num2 - 2.0 * (double) num3) * (double) curveKey2.Value + ((double) num3 - (double) num2) * (double) curveKey2.TangentIn);
          }
        }
        else
          curveKey1 = curveKey2;
      }
      return 0.0f;
    }
  }
}
