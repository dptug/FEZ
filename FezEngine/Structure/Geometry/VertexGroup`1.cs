// Type: FezEngine.Structure.Geometry.VertexGroup`1
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Tools;
using System;
using System.Collections.Generic;

namespace FezEngine.Structure.Geometry
{
  public class VertexGroup<T> where T : struct, IEquatable<T>, IVertex
  {
    private static Dictionary<T, SharedVertex<T>> vertexPresence;
    private static bool allocated;
    private static Pool<SharedVertex<T>> vertexPool;

    public ICollection<SharedVertex<T>> Vertices
    {
      get
      {
        return (ICollection<SharedVertex<T>>) VertexGroup<T>.vertexPresence.Values;
      }
    }

    public VertexGroup()
      : this(0)
    {
    }

    public VertexGroup(int capacity)
    {
      if (!VertexGroup<T>.allocated)
      {
        VertexGroup<T>.vertexPresence = new Dictionary<T, SharedVertex<T>>(capacity);
        VertexGroup<T>.vertexPool = new Pool<SharedVertex<T>>(capacity);
        VertexGroup<T>.allocated = true;
      }
      else
      {
        foreach (SharedVertex<T> sharedVertex in VertexGroup<T>.vertexPresence.Values)
          VertexGroup<T>.vertexPool.Return(sharedVertex);
        VertexGroup<T>.vertexPresence.Clear();
      }
    }

    public void Dereference(SharedVertex<T> sv)
    {
      if (sv.References == 1)
        VertexGroup<T>.vertexPresence.Remove(sv.Vertex);
      else
        --sv.References;
    }

    public SharedVertex<T> Reference(T vertex)
    {
      SharedVertex<T> sharedVertex;
      if (!VertexGroup<T>.vertexPresence.TryGetValue(vertex, out sharedVertex))
      {
        sharedVertex = VertexGroup<T>.vertexPool.Take();
        sharedVertex.Vertex = vertex;
        VertexGroup<T>.vertexPresence.Add(vertex, sharedVertex);
      }
      ++sharedVertex.References;
      return sharedVertex;
    }

    public static void Deallocate()
    {
      VertexGroup<T>.vertexPresence = (Dictionary<T, SharedVertex<T>>) null;
      VertexGroup<T>.vertexPool = (Pool<SharedVertex<T>>) null;
      VertexGroup<T>.allocated = false;
    }
  }
}
