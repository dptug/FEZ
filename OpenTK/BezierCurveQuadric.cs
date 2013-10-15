// Type: OpenTK.BezierCurveQuadric
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK
{
  [Serializable]
  public struct BezierCurveQuadric
  {
    public Vector2 StartAnchor;
    public Vector2 EndAnchor;
    public Vector2 ControlPoint;
    public float Parallel;

    public BezierCurveQuadric(Vector2 startAnchor, Vector2 endAnchor, Vector2 controlPoint)
    {
      this.StartAnchor = startAnchor;
      this.EndAnchor = endAnchor;
      this.ControlPoint = controlPoint;
      this.Parallel = 0.0f;
    }

    public BezierCurveQuadric(float parallel, Vector2 startAnchor, Vector2 endAnchor, Vector2 controlPoint)
    {
      this.Parallel = parallel;
      this.StartAnchor = startAnchor;
      this.EndAnchor = endAnchor;
      this.ControlPoint = controlPoint;
    }

    public Vector2 CalculatePoint(float t)
    {
      Vector2 vector2_1 = new Vector2();
      float num = 1f - t;
      vector2_1.X = (float) ((double) num * (double) num * (double) this.StartAnchor.X + 2.0 * (double) t * (double) num * (double) this.ControlPoint.X + (double) t * (double) t * (double) this.EndAnchor.X);
      vector2_1.Y = (float) ((double) num * (double) num * (double) this.StartAnchor.Y + 2.0 * (double) t * (double) num * (double) this.ControlPoint.Y + (double) t * (double) t * (double) this.EndAnchor.Y);
      if ((double) this.Parallel == 0.0)
        return vector2_1;
      Vector2 vector2_2 = new Vector2();
      Vector2 vec = (double) t != 0.0 ? vector2_1 - this.CalculatePointOfDerivative(t) : this.ControlPoint - this.StartAnchor;
      return vector2_1 + Vector2.Normalize(vec).PerpendicularRight * this.Parallel;
    }

    private Vector2 CalculatePointOfDerivative(float t)
    {
      return new Vector2()
      {
        X = (float) ((1.0 - (double) t) * (double) this.StartAnchor.X + (double) t * (double) this.ControlPoint.X),
        Y = (float) ((1.0 - (double) t) * (double) this.StartAnchor.Y + (double) t * (double) this.ControlPoint.Y)
      };
    }

    public float CalculateLength(float precision)
    {
      float num = 0.0f;
      Vector2 vector2_1 = this.CalculatePoint(0.0f);
      float t = precision;
      while ((double) t < 1.0 + (double) precision)
      {
        Vector2 vector2_2 = this.CalculatePoint(t);
        num += (vector2_2 - vector2_1).Length;
        vector2_1 = vector2_2;
        t += precision;
      }
      return num;
    }
  }
}
