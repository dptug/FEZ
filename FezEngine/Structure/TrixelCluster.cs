// Type: FezEngine.Structure.TrixelCluster
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using ContentSerialization;
using ContentSerialization.Attributes;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezEngine.Structure
{
  public class TrixelCluster : ISpatialStructure<TrixelEmplacement>, IDeserializationCallback
  {
    private static readonly Vector3[] Directions = new Vector3[6]
    {
      Vector3.Up,
      Vector3.Down,
      Vector3.Left,
      Vector3.Right,
      Vector3.Forward,
      Vector3.Backward
    };
    private List<TrixelCluster.Box> deserializedBoxes;
    private List<TrixelEmplacement> deserializedOrphans;

    public List<TrixelCluster.Chunk> Chunks { get; private set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public List<TrixelCluster.Box> Boxes
    {
      get
      {
        return this.deserializedBoxes ?? Enumerable.ToList<TrixelCluster.Box>(Enumerable.SelectMany<TrixelCluster.Chunk, TrixelCluster.Box>((IEnumerable<TrixelCluster.Chunk>) this.Chunks, (Func<TrixelCluster.Chunk, IEnumerable<TrixelCluster.Box>>) (c => (IEnumerable<TrixelCluster.Box>) c.Boxes)));
      }
      set
      {
        this.deserializedBoxes = value;
      }
    }

    [Serialization(CollectionItemName = "content", DefaultValueOptional = true, Optional = true)]
    public List<TrixelEmplacement> Orphans
    {
      get
      {
        return this.deserializedOrphans ?? Enumerable.ToList<TrixelEmplacement>(Enumerable.SelectMany<TrixelCluster.Chunk, TrixelEmplacement>((IEnumerable<TrixelCluster.Chunk>) this.Chunks, (Func<TrixelCluster.Chunk, IEnumerable<TrixelEmplacement>>) (c => (IEnumerable<TrixelEmplacement>) c.Trixels)));
      }
      set
      {
        this.deserializedOrphans = value;
      }
    }

    public bool Empty
    {
      get
      {
        return this.Chunks.Count == 0;
      }
    }

    public IEnumerable<TrixelEmplacement> Cells
    {
      get
      {
        return Enumerable.SelectMany<TrixelCluster.Chunk, TrixelEmplacement>((IEnumerable<TrixelCluster.Chunk>) this.Chunks, (Func<TrixelCluster.Chunk, IEnumerable<TrixelEmplacement>>) (c => Enumerable.Concat<TrixelEmplacement>((IEnumerable<TrixelEmplacement>) c.Trixels, Enumerable.SelectMany<TrixelCluster.Box, TrixelEmplacement>((IEnumerable<TrixelCluster.Box>) c.Boxes, (Func<TrixelCluster.Box, IEnumerable<TrixelEmplacement>>) (b => b.Cells)))));
      }
    }

    static TrixelCluster()
    {
    }

    public TrixelCluster()
    {
      this.Chunks = new List<TrixelCluster.Chunk>();
    }

    public void OnDeserialization()
    {
      if (this.deserializedOrphans != null)
      {
        using (List<TrixelEmplacement>.Enumerator enumerator = this.deserializedOrphans.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            TrixelEmplacement trixel = enumerator.Current;
            TrixelCluster.Chunk chunk = Enumerable.FirstOrDefault<TrixelCluster.Chunk>((IEnumerable<TrixelCluster.Chunk>) this.Chunks, (Func<TrixelCluster.Chunk, bool>) (c => c.IsNeighbor(trixel)));
            if (chunk == null)
              this.Chunks.Add(chunk = new TrixelCluster.Chunk());
            chunk.Trixels.Add(trixel);
          }
        }
        this.deserializedOrphans = (List<TrixelEmplacement>) null;
      }
      if (this.deserializedBoxes == null)
        return;
      using (List<TrixelCluster.Box>.Enumerator enumerator = this.deserializedBoxes.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          TrixelCluster.Box box = enumerator.Current;
          TrixelCluster.Chunk chunk = Enumerable.FirstOrDefault<TrixelCluster.Chunk>((IEnumerable<TrixelCluster.Chunk>) this.Chunks, (Func<TrixelCluster.Chunk, bool>) (c => c.IsNeighbor(box)));
          if (chunk == null)
            this.Chunks.Add(chunk = new TrixelCluster.Chunk());
          chunk.Boxes.Add(box);
        }
      }
      this.deserializedBoxes = (List<TrixelCluster.Box>) null;
    }

    public void Clear()
    {
      this.Chunks.Clear();
    }

    public void Fill(TrixelEmplacement trixel)
    {
      this.Fill(Enumerable.Repeat<TrixelEmplacement>(trixel, 1));
    }

    public void Fill(IEnumerable<TrixelEmplacement> trixels)
    {
      using (IEnumerator<TrixelEmplacement> enumerator = trixels.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          TrixelEmplacement trixel = enumerator.Current;
          if (!Enumerable.Any<TrixelCluster.Chunk>((IEnumerable<TrixelCluster.Chunk>) this.Chunks, (Func<TrixelCluster.Chunk, bool>) (c => c.TryAdd(trixel))))
          {
            TrixelCluster.Chunk chunk = new TrixelCluster.Chunk();
            this.Chunks.Add(chunk);
            chunk.Trixels.Add(trixel);
          }
        }
      }
      this.ConsolidateTrixels();
    }

    public void FillAsChunk(IEnumerable<TrixelEmplacement> trixels)
    {
      TrixelEmplacement firstTrixel = Enumerable.First<TrixelEmplacement>(trixels);
      TrixelCluster.Chunk chunk = Enumerable.FirstOrDefault<TrixelCluster.Chunk>((IEnumerable<TrixelCluster.Chunk>) this.Chunks, (Func<TrixelCluster.Chunk, bool>) (c => c.TryAdd(firstTrixel)));
      if (chunk == null)
      {
        chunk = new TrixelCluster.Chunk();
        this.Chunks.Add(chunk);
      }
      chunk.Trixels.UnionWith(trixels);
      chunk.ConsolidateTrixels();
    }

    public void Free(TrixelEmplacement trixel)
    {
      this.Free(Enumerable.Repeat<TrixelEmplacement>(trixel, 1));
    }

    public void Free(IEnumerable<TrixelEmplacement> trixels)
    {
      using (IEnumerator<TrixelEmplacement> enumerator = trixels.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          TrixelEmplacement trixel = enumerator.Current;
          Enumerable.Any<TrixelCluster.Chunk>((IEnumerable<TrixelCluster.Chunk>) this.Chunks, (Func<TrixelCluster.Chunk, bool>) (c => c.TryRemove(trixel)));
        }
      }
      this.ConsolidateTrixels();
    }

    public void ConsolidateTrixels()
    {
      this.Chunks.RemoveAll((Predicate<TrixelCluster.Chunk>) (c => c.Empty));
      foreach (TrixelCluster.Chunk chunk in Enumerable.Where<TrixelCluster.Chunk>((IEnumerable<TrixelCluster.Chunk>) this.Chunks, (Func<TrixelCluster.Chunk, bool>) (c => c.Dirty)))
        chunk.ConsolidateTrixels();
    }

    public bool IsFilled(TrixelEmplacement trixel)
    {
      for (int index = 0; index < this.Chunks.Count; ++index)
      {
        if (this.Chunks[index].Contains(trixel))
          return true;
      }
      return false;
    }

    public class Chunk
    {
      [Serialization(Optional = true)]
      public List<TrixelCluster.Box> Boxes { get; set; }

      [Serialization(CollectionItemName = "content", Optional = true)]
      public HashSet<TrixelEmplacement> Trixels { get; set; }

      [Serialization(Ignore = true)]
      internal bool Dirty { get; set; }

      internal bool Empty
      {
        get
        {
          if (this.Boxes.Count == 0)
            return this.Trixels.Count == 0;
          else
            return false;
        }
      }

      public Chunk()
      {
        this.Boxes = new List<TrixelCluster.Box>();
        this.Trixels = new HashSet<TrixelEmplacement>();
      }

      internal bool IsNeighbor(TrixelCluster.Box box)
      {
        if (Enumerable.Any<TrixelCluster.Box>((IEnumerable<TrixelCluster.Box>) this.Boxes, (Func<TrixelCluster.Box, bool>) (b => b.IsNeighbor(box))))
          return true;
        else
          return Enumerable.Any<TrixelEmplacement>((IEnumerable<TrixelEmplacement>) this.Trixels, new Func<TrixelEmplacement, bool>(box.IsNeighbor));
      }

      internal bool IsNeighbor(TrixelEmplacement trixel)
      {
        if (Enumerable.Any<TrixelCluster.Box>((IEnumerable<TrixelCluster.Box>) this.Boxes, (Func<TrixelCluster.Box, bool>) (b => b.IsNeighbor(trixel))))
          return true;
        else
          return Enumerable.Any<TrixelEmplacement>((IEnumerable<TrixelEmplacement>) this.Trixels, (Func<TrixelEmplacement, bool>) (t => t.IsNeighbor(trixel)));
      }

      internal bool Contains(TrixelEmplacement trixel)
      {
        for (int index = 0; index < this.Boxes.Count; ++index)
        {
          if (this.Boxes[index].Contains(trixel))
            return true;
        }
        return this.Trixels.Contains(trixel);
      }

      internal bool TryAdd(TrixelEmplacement trixel)
      {
        bool flag = false;
        if (this.Boxes.Count > 0)
        {
          foreach (TrixelCluster.Box box in Enumerable.ToArray<TrixelCluster.Box>(Enumerable.Where<TrixelCluster.Box>((IEnumerable<TrixelCluster.Box>) this.Boxes, (Func<TrixelCluster.Box, bool>) (b => b.IsNeighbor(trixel)))))
          {
            flag = true;
            this.Dismantle(box);
            this.Boxes.Remove(box);
          }
        }
        if (!flag)
        {
          foreach (TrixelEmplacement trixelEmplacement in this.Trixels)
          {
            if (trixelEmplacement.IsNeighbor(trixel))
            {
              flag = true;
              break;
            }
          }
        }
        if (flag)
          this.Trixels.Add(trixel);
        TrixelCluster.Chunk chunk = this;
        int num = chunk.Dirty | flag ? 1 : 0;
        chunk.Dirty = num != 0;
        return flag;
      }

      internal bool TryRemove(TrixelEmplacement trixel)
      {
        bool flag1 = false;
        foreach (TrixelCluster.Box box in Enumerable.Where<TrixelCluster.Box>((IEnumerable<TrixelCluster.Box>) this.Boxes, (Func<TrixelCluster.Box, bool>) (b =>
        {
          if (!b.Contains(trixel))
            return b.IsNeighbor(trixel);
          else
            return true;
        })))
        {
          flag1 = true;
          this.Dismantle(box);
        }
        if (flag1)
          this.Boxes.RemoveAll((Predicate<TrixelCluster.Box>) (b =>
          {
            if (!b.Contains(trixel))
              return b.IsNeighbor(trixel);
            else
              return true;
          }));
        bool flag2 = flag1 | this.Trixels.Remove(trixel);
        TrixelCluster.Chunk chunk = this;
        int num = chunk.Dirty | flag2 ? 1 : 0;
        chunk.Dirty = num != 0;
        return flag2;
      }

      private void Dismantle(TrixelCluster.Box box)
      {
        for (int x = box.Start.X; x < box.End.X; ++x)
        {
          for (int y = box.Start.Y; y < box.End.Y; ++y)
          {
            for (int z = box.Start.Z; z < box.End.Z; ++z)
              this.Trixels.Add(new TrixelEmplacement(x, y, z));
          }
        }
      }

      internal void ConsolidateTrixels()
      {
        if (this.Trixels.Count <= 1)
          return;
        Stack<HashSet<TrixelEmplacement>> stack = new Stack<HashSet<TrixelEmplacement>>();
        stack.Push(new HashSet<TrixelEmplacement>((IEnumerable<TrixelEmplacement>) this.Trixels));
        while (stack.Count > 0)
        {
          HashSet<TrixelEmplacement> hashSet1 = stack.Pop();
          TrixelEmplacement center = new TrixelEmplacement();
          foreach (TrixelEmplacement trixelEmplacement in hashSet1)
            center.Offset(trixelEmplacement.X, trixelEmplacement.Y, trixelEmplacement.Z);
          center.Position = FezMath.Floor(FezMath.Round(center.Position / (float) hashSet1.Count, 3));
          if (!hashSet1.Contains(center))
            center = Enumerable.First<TrixelEmplacement>((IEnumerable<TrixelEmplacement>) hashSet1);
          TrixelCluster.Box box;
          List<TrixelEmplacement> biggestBox = TrixelCluster.Chunk.FindBiggestBox(center, (ICollection<TrixelEmplacement>) hashSet1, out box);
          this.Boxes.Add(box);
          hashSet1.ExceptWith((IEnumerable<TrixelEmplacement>) biggestBox);
          this.Trixels.ExceptWith((IEnumerable<TrixelEmplacement>) biggestBox);
          while (hashSet1.Count > 1)
          {
            HashSet<TrixelEmplacement> hashSet2 = TrixelCluster.Chunk.VisitChunk(Enumerable.First<TrixelEmplacement>((IEnumerable<TrixelEmplacement>) hashSet1), (ICollection<TrixelEmplacement>) hashSet1);
            stack.Push(hashSet2);
            hashSet1.ExceptWith((IEnumerable<TrixelEmplacement>) hashSet2);
          }
        }
        this.Dirty = false;
      }

      private static List<TrixelEmplacement> FindBiggestBox(TrixelEmplacement center, ICollection<TrixelEmplacement> subChunk, out TrixelCluster.Box box)
      {
        List<TrixelEmplacement> boxTrixels = new List<TrixelEmplacement>()
        {
          center
        };
        box = new TrixelCluster.Box()
        {
          Start = center,
          End = center
        };
        int trixelsToRollback;
        do
        {
          box.Start -= Vector3.One;
          box.End += Vector3.One;
          trixelsToRollback = 0;
        }
        while (TrixelCluster.Chunk.TestFace(subChunk, (ICollection<TrixelEmplacement>) boxTrixels, Vector3.UnitZ, false, box, ref trixelsToRollback) && TrixelCluster.Chunk.TestFace(subChunk, (ICollection<TrixelEmplacement>) boxTrixels, -Vector3.UnitZ, false, box, ref trixelsToRollback) && (TrixelCluster.Chunk.TestFace(subChunk, (ICollection<TrixelEmplacement>) boxTrixels, Vector3.UnitX, true, box, ref trixelsToRollback) && TrixelCluster.Chunk.TestFace(subChunk, (ICollection<TrixelEmplacement>) boxTrixels, -Vector3.UnitX, true, box, ref trixelsToRollback)) && (TrixelCluster.Chunk.TestFace(subChunk, (ICollection<TrixelEmplacement>) boxTrixels, Vector3.UnitY, true, box, ref trixelsToRollback) && TrixelCluster.Chunk.TestFace(subChunk, (ICollection<TrixelEmplacement>) boxTrixels, -Vector3.UnitY, true, box, ref trixelsToRollback)));
        boxTrixels.RemoveRange(boxTrixels.Count - trixelsToRollback, trixelsToRollback);
        box.Start += Vector3.One;
        box.End -= Vector3.One;
        if (boxTrixels.Count < subChunk.Count)
        {
          foreach (Vector3 normal in TrixelCluster.Directions)
            TrixelCluster.Chunk.ExpandSide(box, subChunk, boxTrixels, normal);
        }
        box.End += Vector3.One;
        return boxTrixels;
      }

      private static void ExpandSide(TrixelCluster.Box box, ICollection<TrixelEmplacement> subChunk, List<TrixelEmplacement> boxTrixels, Vector3 normal)
      {
        int trixelsToRollback = 0;
        bool flag = (double) Vector3.Dot(normal, Vector3.One) > 0.0;
        if (flag)
          box.End += normal;
        else
          box.Start += normal;
        for (; TrixelCluster.Chunk.TestFace(subChunk, (ICollection<TrixelEmplacement>) boxTrixels, normal, false, box, ref trixelsToRollback); trixelsToRollback = 0)
        {
          if (flag)
            box.End += normal;
          else
            box.Start += normal;
        }
        boxTrixels.RemoveRange(boxTrixels.Count - trixelsToRollback, trixelsToRollback);
        if (flag)
          box.End -= normal;
        else
          box.Start -= normal;
      }

      private static bool TestFace(ICollection<TrixelEmplacement> subChunk, ICollection<TrixelEmplacement> boxTrixels, Vector3 normal, bool partial, TrixelCluster.Box box, ref int trixelsToRollback)
      {
        Vector3 vector3_1 = FezMath.Abs(normal);
        Vector3 vector2_1 = (double) normal.Z != 0.0 ? Vector3.UnitX : Vector3.UnitZ;
        Vector3 vector2_2 = (double) normal.Z != 0.0 ? Vector3.UnitY : new Vector3(1f, 1f, 0.0f) - vector3_1;
        Vector3 vector3_2 = ((double) Vector3.Dot(normal, Vector3.One) > 0.0 ? box.End.Position : box.Start.Position) * vector3_1;
        int num1 = (int) Vector3.Dot(box.Start.Position, vector2_1);
        int num2 = (int) Vector3.Dot(box.End.Position, vector2_1);
        int num3 = (int) Vector3.Dot(box.Start.Position, vector2_2);
        int num4 = (int) Vector3.Dot(box.End.Position, vector2_2);
        if (partial)
        {
          ++num1;
          --num2;
        }
        for (int index1 = num1; index1 <= num2; ++index1)
        {
          for (int index2 = num3; index2 <= num4; ++index2)
          {
            TrixelEmplacement trixelEmplacement = new TrixelEmplacement((float) index1 * vector2_1 + (float) index2 * vector2_2 + vector3_2);
            if (!subChunk.Contains(trixelEmplacement))
              return false;
            ++trixelsToRollback;
            boxTrixels.Add(trixelEmplacement);
          }
        }
        return true;
      }

      private static HashSet<TrixelEmplacement> VisitChunk(TrixelEmplacement origin, ICollection<TrixelEmplacement> subChunk)
      {
        HashSet<TrixelEmplacement> hashSet = new HashSet<TrixelEmplacement>()
        {
          origin
        };
        Queue<TrixelCluster.Chunk.TrixelToVisit> queue = new Queue<TrixelCluster.Chunk.TrixelToVisit>();
        queue.Enqueue(new TrixelCluster.Chunk.TrixelToVisit()
        {
          Trixel = origin
        });
        while (queue.Count != 0)
        {
          TrixelCluster.Chunk.TrixelToVisit toTraverse = queue.Dequeue();
          TrixelEmplacement trixelEmplacement1 = toTraverse.Trixel;
          using (IEnumerator<Vector3> enumerator = Enumerable.Where<Vector3>((IEnumerable<Vector3>) TrixelCluster.Directions, (Func<Vector3, bool>) (x =>
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
              Vector3 current = enumerator.Current;
              TrixelEmplacement trixelEmplacement2 = trixelEmplacement1 + current;
              while (true)
              {
                if (!hashSet.Contains(trixelEmplacement2) && subChunk.Contains(trixelEmplacement2))
                {
                  hashSet.Add(trixelEmplacement2);
                  if (hashSet.Count != subChunk.Count)
                  {
                    queue.Enqueue(new TrixelCluster.Chunk.TrixelToVisit()
                    {
                      Trixel = trixelEmplacement2,
                      Except = new Vector3?(current)
                    });
                    trixelEmplacement2 += current;
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

      private struct TrixelToVisit
      {
        public TrixelEmplacement Trixel;
        public Vector3? Except;
      }
    }

    public class Box
    {
      public TrixelEmplacement Start { get; set; }

      public TrixelEmplacement End { get; set; }

      internal IEnumerable<TrixelEmplacement> Cells
      {
        get
        {
          for (int x = this.Start.X; x < this.End.X; ++x)
          {
            for (int y = this.Start.Y; y < this.End.Y; ++y)
            {
              for (int z = this.Start.Z; z < this.End.Z; ++z)
                yield return new TrixelEmplacement(x, y, z);
            }
          }
        }
      }

      internal bool Contains(TrixelEmplacement trixel)
      {
        if (trixel.X >= this.Start.X && trixel.Y >= this.Start.Y && (trixel.Z >= this.Start.Z && trixel.X < this.End.X) && trixel.Y < this.End.Y)
          return trixel.Z < this.End.Z;
        else
          return false;
      }

      internal bool IsNeighbor(TrixelCluster.Box other)
      {
        return new BoundingBox(this.Start.Position, this.End.Position).Intersects(new BoundingBox(other.Start.Position, other.End.Position));
      }

      internal bool IsNeighbor(TrixelEmplacement trixel)
      {
        return new BoundingBox(this.Start.Position, this.End.Position).Intersects(new BoundingBox(trixel.Position, trixel.Position + Vector3.One));
      }
    }
  }
}
