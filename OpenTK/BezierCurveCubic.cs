// Type: OpenTK.BezierCurveCubic
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;

namespace OpenTK
{
  [Serializable]
  public struct BezierCurveCubic
  {
    public Vector2 StartAnchor;
    public Vector2 EndAnchor;
    public Vector2 FirstControlPoint;
    public Vector2 SecondControlPoint;
    public float Parallel;

    public BezierCurveCubic(Vector2 startAnchor, Vector2 endAnchor, Vector2 firstControlPoint, Vector2 secondControlPoint)
    {
      this.StartAnchor = startAnchor;
      this.EndAnchor = endAnchor;
      this.FirstControlPoint = firstControlPoint;
      this.SecondControlPoint = secondControlPoint;
      this.Parallel = 0.0f;
    }

    public BezierCurveCubic(float parallel, Vector2 startAnchor, Vector2 endAnchor, Vector2 firstControlPoint, Vector2 secondControlPoint)
    {
      this.Parallel = parallel;
      this.StartAnchor = startAnchor;
      this.EndAnchor = endAnchor;
      this.FirstControlPoint = firstControlPoint;
      this.SecondControlPoint = secondControlPoint;
    }

    public Vector2 CalculatePoint(float t)
    {
      Vector2 vector2_1 = new Vector2();
      float num = 1f - t;
      vector2_1.X = (float) ((double) this.StartAnchor.X * (double) num * (double) num * (double) num + (double) this.FirstControlPoint.X * 3.0 * (double) t * (double) num * (double) num + (double) this.SecondControlPoint.X * 3.0 * (double) t * (double) t * (double) num + (double) this.EndAnchor.X * (double) t * (double) t * (double) t);
      vector2_1.Y = (float) ((double) this.StartAnchor.Y * (double) num * (double) num * (double) num + (double) this.FirstControlPoint.Y * 3.0 * (double) t * (double) num * (double) num + (double) this.SecondControlPoint.Y * 3.0 * (double) t * (double) t * (double) num + (double) this.EndAnchor.Y * (double) t * (double) t * (double) t);
      if ((double) this.Parallel == 0.0)
        return vector2_1;
      Vector2 vector2_2 = new Vector2();
      Vector2 vec = (double) t != 0.0 ? vector2_1 - this.CalculatePointOfDerivative(t) : this.FirstControlPoint - this.StartAnchor;
      return vector2_1 + Vector2.Normalize(vec).PerpendicularRight * this.Parallel;
    }

    private Vector2 CalculatePointOfDerivative(float t)
    {
      Vector2 vector2 = new Vector2();
      float num = 1f - t;
      vector2.X = (float) ((double) num * (double) num * (double) this.StartAnchor.X + 2.0 * (double) t * (double) num * (double) this.FirstControlPoint.X + (double) t * (double) t * (double) this.SecondControlPoint.X);
      vector2.Y = (float) ((double) num * (double) num * (double) this.StartAnchor.Y + 2.0 * (double) t * (double) num * (double) this.FirstControlPoint.Y + (double) t * (double) t * (double) this.SecondControlPoint.Y);
      return vector2;
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
