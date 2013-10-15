// Type: FezEngine.Structure.TrixelSurface
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using ContentSerialization;
using ContentSerialization.Attributes;
using FezEngine;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezEngine.Structure
{
  public class TrixelSurface : IDeserializationCallback
  {
    private Vector3 normal;
    private FaceOrientation tangentFace;
    private FaceOrientation bitangentFace;
    private FaceOrientation[] tangentFaces;
    private int depth;

    public FaceOrientation Orientation { get; set; }

    [Serialization(CollectionItemName = "part", Name = "parts")]
    public List<RectangularTrixelSurfacePart> RectangularParts { get; set; }

    public Vector3 Tangent { get; private set; }

    public Vector3 Bitangent { get; private set; }

    public HashSet<TrixelEmplacement> Trixels { get; private set; }

    public bool Dirty { get; private set; }

    public TrixelSurface()
    {
    }

    public TrixelSurface(FaceOrientation orientation, TrixelEmplacement firstTrixel)
    {
      this.RectangularParts = new List<RectangularTrixelSurfacePart>();
      this.Orientation = orientation;
      this.Initialize();
      this.Trixels.Add(firstTrixel);
      this.MarkAsDirty();
      this.InitializeDepth();
    }

    public void OnDeserialization()
    {
      this.Initialize();
      this.RebuildFromParts();
      this.InitializeDepth();
    }

    private void Initialize()
    {
      this.Trixels = new HashSet<TrixelEmplacement>();
      this.tangentFace = FezMath.GetTangent(this.Orientation);
      this.bitangentFace = FezMath.GetBitangent(this.Orientation);
      this.tangentFaces = new FaceOrientation[4]
      {
        this.tangentFace,
        this.bitangentFace,
        FezMath.GetOpposite(this.tangentFace),
        FezMath.GetOpposite(this.bitangentFace)
      };
      this.normal = FezMath.AsVector(this.Orientation);
      this.Tangent = FezMath.AsVector(this.tangentFace);
      this.Bitangent = FezMath.AsVector(this.bitangentFace);
    }

    private void InitializeDepth()
    {
      this.depth = (int) Vector3.Dot(Enumerable.First<TrixelEmplacement>((IEnumerable<TrixelEmplacement>) this.Trixels).Position, this.normal);
    }

    public bool CanContain(TrixelEmplacement trixel, FaceOrientation face)
    {
      return face == this.Orientation && (int) Vector3.Dot(trixel.Position, this.normal) == this.depth && (this.Trixels.Contains(trixel + this.Tangent) || this.Trixels.Contains(trixel + this.Bitangent) || (this.Trixels.Contains(trixel - this.Tangent) || this.Trixels.Contains(trixel - this.Bitangent)));
    }

    public void MarkAsDirty()
    {
      this.Dirty = true;
    }

    public void RebuildFromParts()
    {
      foreach (RectangularTrixelSurfacePart trixelSurfacePart in this.RectangularParts)
      {
        trixelSurfacePart.Orientation = this.Orientation;
        for (int index1 = 0; index1 < trixelSurfacePart.TangentSize; ++index1)
        {
          for (int index2 = 0; index2 < trixelSurfacePart.BitangentSize; ++index2)
            this.Trixels.Add(new TrixelEmplacement(trixelSurfacePart.Start + this.Tangent * (float) index1 + this.Bitangent * (float) index2));
        }
      }
      this.MarkAsDirty();
    }

    public void RebuildParts()
    {
      this.Dirty = false;
      this.RectangularParts.Clear();
      Queue<HashSet<TrixelEmplacement>> queue = new Queue<HashSet<TrixelEmplacement>>();
      if (this.Trixels.Count > 0)
        queue.Enqueue(new HashSet<TrixelEmplacement>((IEnumerable<TrixelEmplacement>) this.Trixels));
      while (queue.Count > 0)
      {
        HashSet<TrixelEmplacement> hashSet1 = queue.Dequeue();
        TrixelEmplacement center = new TrixelEmplacement();
        foreach (TrixelEmplacement trixelEmplacement in hashSet1)
          center.Position += trixelEmplacement.Position;
        center.Position = FezMath.Floor(center.Position / (float) hashSet1.Count);
        if ((double) Vector3.Dot(center.Position, this.normal) != (double) this.depth)
          center.Position = center.Position * (Vector3.One - FezMath.Abs(this.normal)) + (float) this.depth * this.normal;
        if (!hashSet1.Contains(center))
          center = this.FindNearestTrixel(center, (ICollection<TrixelEmplacement>) hashSet1);
        Rectangle rectangle;
        List<TrixelEmplacement> biggestRectangle = this.FindBiggestRectangle(center, (ICollection<TrixelEmplacement>) hashSet1, out rectangle);
        rectangle.Offset((int) Vector3.Dot(center.Position, this.Tangent), (int) Vector3.Dot(center.Position, this.Bitangent));
        this.RectangularParts.Add(new RectangularTrixelSurfacePart()
        {
          Orientation = this.Orientation,
          Start = new TrixelEmplacement((float) rectangle.X * this.Tangent + (float) rectangle.Y * this.Bitangent + (float) this.depth * this.normal),
          TangentSize = rectangle.Width,
          BitangentSize = rectangle.Height
        });
        hashSet1.ExceptWith((IEnumerable<TrixelEmplacement>) biggestRectangle);
        while (hashSet1.Count > 0)
        {
          HashSet<TrixelEmplacement> hashSet2 = this.TraverseSurface(Enumerable.First<TrixelEmplacement>((IEnumerable<TrixelEmplacement>) hashSet1), (ICollection<TrixelEmplacement>) hashSet1);
          queue.Enqueue(hashSet2);
          if (hashSet1.Count == hashSet2.Count)
            hashSet1.Clear();
          else
            hashSet1.ExceptWith((IEnumerable<TrixelEmplacement>) hashSet2);
        }
      }
    }

    public bool AnyRectangleContains(TrixelEmplacement trixel)
    {
      foreach (RectangularTrixelSurfacePart trixelSurfacePart in this.RectangularParts)
      {
        Vector3 vector1 = trixel.Position - trixelSurfacePart.Start.Position;
        if ((double) vector1.X >= 0.0 && (double) vector1.Y >= 0.0 && (double) vector1.Z >= 0.0)
        {
          int num1 = (int) Vector3.Dot(vector1, this.Tangent);
          int num2 = (int) Vector3.Dot(vector1, this.Bitangent);
          if (num1 < trixelSurfacePart.TangentSize && num2 < trixelSurfacePart.BitangentSize)
            return true;
        }
      }
      return false;
    }

    private HashSet<TrixelEmplacement> TraverseSurface(TrixelEmplacement origin, ICollection<TrixelEmplacement> subSurface)
    {
      HashSet<TrixelEmplacement> hashSet = new HashSet<TrixelEmplacement>()
      {
        origin
      };
      Queue<TrixelSurface.TrixelToTraverse> queue = new Queue<TrixelSurface.TrixelToTraverse>();
      queue.Enqueue(new TrixelSurface.TrixelToTraverse()
      {
        Trixel = origin
      });
      while (queue.Count != 0)
      {
        TrixelSurface.TrixelToTraverse toTraverse = queue.Dequeue();
        TrixelEmplacement trixelEmplacement = toTraverse.Trixel;
        using (IEnumerator<FaceOrientation> enumerator = Enumerable.Where<FaceOrientation>((IEnumerable<FaceOrientation>) this.tangentFaces, (Func<FaceOrientation, bool>) (x =>
        {
          if (toTraverse.Except.HasValue)
            return toTraverse.Except.Value != x;
          else
            return true;
        })).GetEnumerator())
        {
label_8:
          while (enumerator.MoveNext())
          {
            FaceOrientation current = enumerator.Current;
            TrixelEmplacement traversal = trixelEmplacement.GetTraversal(current);
            while (true)
            {
              if (!hashSet.Contains(traversal) && subSurface.Contains(traversal))
              {
                hashSet.Add(traversal);
                if (hashSet.Count != subSurface.Count)
                {
                  queue.Enqueue(new TrixelSurface.TrixelToTraverse()
                  {
                    Trixel = traversal,
                    Except = new FaceOrientation?(current)
                  });
                  traversal = traversal.GetTraversal(current);
                }
                else
                  break;
              }
              else
                goto label_8;
            }
            return hashSet;
          }
        }
      }
      return hashSet;
    }

    private List<TrixelEmplacement> FindBiggestRectangle(TrixelEmplacement center, ICollection<TrixelEmplacement> subSurface, out Rectangle rectangle)
    {
      List<TrixelEmplacement> rectangleTrixels = new List<TrixelEmplacement>();
      TrixelEmplacement other = new TrixelEmplacement(center);
      int num1 = 1;
      int num2 = 0;
      int num3 = 1;
      int num4 = 0;
      int num5 = 1;
      int num6 = -1;
      do
      {
        rectangleTrixels.Add(new TrixelEmplacement(other));
        if (num3 > 0)
        {
          other.Position += this.Tangent * (float) num5;
          if (--num3 == 0)
          {
            num6 *= -1;
            num4 = ++num2;
          }
        }
        else if (num4 > 0)
        {
          other.Position += this.Bitangent * (float) num6;
          if (--num4 == 0)
          {
            num5 *= -1;
            num3 = ++num1;
          }
        }
      }
      while (subSurface.Contains(other));
      int num7 = TrixelSurface.ClampToRectangleSpiral(rectangleTrixels.Count);
      if (num7 != rectangleTrixels.Count)
        rectangleTrixels.RemoveRange(num7, rectangleTrixels.Count - num7);
      rectangle = TrixelSurface.GetRectangleSpiralLimits(num7);
      if (rectangleTrixels.Count < subSurface.Count)
      {
        this.ExpandSide(ref rectangle, center, subSurface, rectangleTrixels, true, 1);
        this.ExpandSide(ref rectangle, center, subSurface, rectangleTrixels, true, -1);
        this.ExpandSide(ref rectangle, center, subSurface, rectangleTrixels, false, 1);
        this.ExpandSide(ref rectangle, center, subSurface, rectangleTrixels, false, -1);
      }
      return rectangleTrixels;
    }

    private static int ClampToRectangleSpiral(int trixelCount)
    {
      int num1 = (int) Math.Floor(Math.Sqrt((double) trixelCount));
      int num2 = num1 * num1;
      int num3 = num2 + num1;
      if (num3 >= trixelCount)
        return num2;
      else
        return num3;
    }

    private static Rectangle GetRectangleSpiralLimits(int trixelCount)
    {
      double d = Math.Sqrt((double) trixelCount);
      int num = (int) Math.Floor(d);
      Point point1;
      point1.X = point1.Y = (int) Math.Floor(d / 2.0) + 1;
      Point point2;
      point2.X = point2.Y = (int) Math.Ceiling(-(d - 1.0) / 2.0);
      if ((double) num != d)
      {
        if (d % 2.0 == 0.0)
          --point2.X;
        else
          ++point1.X;
      }
      return new Rectangle(point2.X, point2.Y, point1.X - point2.X, point1.Y - point2.Y);
    }

    private void ExpandSide(ref Rectangle rectangle, TrixelEmplacement center, ICollection<TrixelEmplacement> subSurface, List<TrixelEmplacement> rectangleTrixels, bool useTangent, int sign)
    {
      TrixelEmplacement other1 = center + (float) rectangle.X * this.Tangent + (float) rectangle.Y * this.Bitangent;
      if (sign > 0)
        other1 += useTangent ? this.Tangent * (float) (rectangle.Width - 1) : this.Bitangent * (float) (rectangle.Height - 1);
      int num = useTangent ? rectangle.Height : rectangle.Width;
      bool flag;
      do
      {
        other1.Position += (useTangent ? this.Tangent : this.Bitangent) * (float) sign;
        TrixelEmplacement other2 = new TrixelEmplacement(other1);
        int count = 0;
        for (flag = subSurface.Contains(other2); flag; flag = subSurface.Contains(other2))
        {
          rectangleTrixels.Add(new TrixelEmplacement(other2));
          if (++count != num)
            other2.Position += useTangent ? this.Bitangent : this.Tangent;
          else
            break;
        }
        if (flag)
        {
          if (useTangent)
          {
            if (sign < 0)
              --rectangle.X;
            ++rectangle.Width;
          }
          else
          {
            if (sign < 0)
              --rectangle.Y;
            ++rectangle.Height;
          }
        }
        else if (count > 0)
          rectangleTrixels.RemoveRange(rectangleTrixels.Count - count, count);
      }
      while (flag);
    }

    private TrixelEmplacement FindNearestTrixel(TrixelEmplacement center, ICollection<TrixelEmplacement> subSurface)
    {
      TrixelEmplacement trixelEmplacement = new TrixelEmplacement(center);
      int num1 = 1;
      int num2 = 0;
      int num3 = 1;
      int num4 = 0;
      int num5 = 1;
      int num6 = -1;
      do
      {
        if (num3 > 0)
        {
          trixelEmplacement.Position += this.Tangent * (float) num5;
          if (--num3 == 0)
          {
            num6 *= -1;
            num4 = ++num2;
          }
        }
        else if (num4 > 0)
        {
          trixelEmplacement.Position += this.Bitangent * (float) num6;
          if (--num4 == 0)
          {
            num5 *= -1;
            num3 = ++num1;
          }
        }
      }
      while (!subSurface.Contains(trixelEmplacement));
      return trixelEmplacement;
    }

    private struct TrixelToTraverse
    {
      public TrixelEmplacement Trixel;
      public FaceOrientation? Except;
    }
  }
}
