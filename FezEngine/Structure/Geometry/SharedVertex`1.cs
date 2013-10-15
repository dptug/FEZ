// Type: FezEngine.Structure.Geometry.SharedVertex`1
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using System;

namespace FezEngine.Structure.Geometry
{
  public class SharedVertex<T> : IEquatable<SharedVertex<T>> where T : struct, IEquatable<T>, IVertex
  {
    public int Index { get; set; }

    public int References { get; set; }

    public T Vertex { get; set; }

    public override int GetHashCode()
    {
      return this.Vertex.GetHashCode();
    }

    public override string ToString()
    {
      return string.Format("{{Vertex:{0} Index:{1} References:{2}}}", (object) this.Vertex, (object) this.Index, (object) this.References);
    }

    public override bool Equals(object obj)
    {
      if (obj is SharedVertex<T>)
        return this.Equals(obj as SharedVertex<T>);
      else
        return false;
    }

    public bool Equals(SharedVertex<T> other)
    {
      return other.Vertex.Equals(this.Vertex);
    }
  }
}
