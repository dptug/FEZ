// Type: OpenTK.BezierCurve
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;
using System.Collections.Generic;

namespace OpenTK
{
  [Serializable]
  public struct BezierCurve
  {
    private List<Vector2> points;
    public float Parallel;

    public IList<Vector2> Points
    {
      get
      {
        return (IList<Vector2>) this.points;
      }
    }

    public BezierCurve(IEnumerable<Vector2> points)
    {
      if (points == null)
        throw new ArgumentNullException("points", "Must point to a valid list of Vector2 structures.");
      this.points = new List<Vector2>(points);
      this.Parallel = 0.0f;
    }

    public BezierCurve(params Vector2[] points)
    {
      if (points == null)
        throw new ArgumentNullException("points", "Must point to a valid list of Vector2 structures.");
      this.points = new List<Vector2>((IEnumerable<Vector2>) points);
      this.Parallel = 0.0f;
    }

    public BezierCurve(float parallel, params Vector2[] points)
    {
      if (points == null)
        throw new ArgumentNullException("points", "Must point to a valid list of Vector2 structures.");
      this.Parallel = parallel;
      this.points = new List<Vector2>((IEnumerable<Vector2>) points);
    }

    public BezierCurve(float parallel, IEnumerable<Vector2> points)
    {
      if (points == null)
        throw new ArgumentNullException("points", "Must point to a valid list of Vector2 structures.");
      this.Parallel = parallel;
      this.points = new List<Vector2>(points);
    }

    public Vector2 CalculatePoint(float t)
    {
      return BezierCurve.CalculatePoint((IList<Vector2>) this.points, t, this.Parallel);
    }

    public float CalculateLength(float precision)
    {
      return BezierCurve.CalculateLength((IList<Vector2>) this.points, precision, this.Parallel);
    }

    public static float CalculateLength(IList<Vector2> points, float precision)
    {
      return BezierCurve.CalculateLength(points, precision, 0.0f);
    }

    public static float CalculateLength(IList<Vector2> points, float precision, float parallel)
    {
      float num = 0.0f;
      Vector2 vector2_1 = BezierCurve.CalculatePoint(points, 0.0f, parallel);
      float t = precision;
      while ((double) t < 1.0 + (double) precision)
      {
        Vector2 vector2_2 = BezierCurve.CalculatePoint(points, t, parallel);
        num += (vector2_2 - vector2_1).Length;
        vector2_1 = vector2_2;
        t += precision;
      }
      return num;
    }

    public static Vector2 CalculatePoint(IList<Vector2> points, float t)
    {
      return BezierCurve.CalculatePoint(points, t, 0.0f);
    }

    public static Vector2 CalculatePoint(IList<Vector2> points, float t, float parallel)
    {
      Vector2 vector2_1 = new Vector2();
      double x = 1.0 - (double) t;
      int k = 0;
      foreach (Vector2 vector2_2 in (IEnumerable<Vector2>) points)
      {
        float num = (float) MathHelper.BinomialCoefficient(points.Count - 1, k) * (float) (Math.Pow((double) t, (double) k) * Math.Pow(x, (double) (points.Count - 1 - k)));
        vector2_1.X += num * vector2_2.X;
        vector2_1.Y += num * vector2_2.Y;
        ++k;
      }
      if ((double) parallel == 0.0)
        return vector2_1;
      Vector2 vector2_3 = new Vector2();
      Vector2 vec = (double) t == 0.0 ? points[1] - points[0] : vector2_1 - BezierCurve.CalculatePointOfDerivative(points, t);
      return vector2_1 + Vector2.Normalize(vec).PerpendicularRight * parallel;
    }

    private static Vector2 CalculatePointOfDerivative(IList<Vector2> points, float t)
    {
      Vector2 vector2_1 = new Vector2();
      double x = 1.0 - (double) t;
      int k = 0;
      foreach (Vector2 vector2_2 in (IEnumerable<Vector2>) points)
      {
        float num = (float) MathHelper.BinomialCoefficient(points.Count - 2, k) * (float) (Math.Pow((double) t, (double) k) * Math.Pow(x, (double) (points.Count - 2 - k)));
        vector2_1.X += num * vector2_2.X;
        vector2_1.Y += num * vector2_2.Y;
        ++k;
      }
      return vector2_1;
    }
  }
}
