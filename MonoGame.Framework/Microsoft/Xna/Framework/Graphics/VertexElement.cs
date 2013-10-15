// Type: Microsoft.Xna.Framework.Graphics.VertexElement
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

namespace Microsoft.Xna.Framework.Graphics
{
  public struct VertexElement
  {
    internal int _offset;
    internal VertexElementFormat _format;
    internal VertexElementUsage _usage;
    internal int _usageIndex;

    public int Offset
    {
      get
      {
        return this._offset;
      }
      set
      {
        this._offset = value;
      }
    }

    public VertexElementFormat VertexElementFormat
    {
      get
      {
        return this._format;
      }
      set
      {
        this._format = value;
      }
    }

    public VertexElementUsage VertexElementUsage
    {
      get
      {
        return this._usage;
      }
      set
      {
        this._usage = value;
      }
    }

    public int UsageIndex
    {
      get
      {
        return this._usageIndex;
      }
      set
      {
        this._usageIndex = value;
      }
    }

    public VertexElement(int offset, VertexElementFormat elementFormat, VertexElementUsage elementUsage, int usageIndex)
    {
      this._offset = offset;
      this._usageIndex = usageIndex;
      this._format = elementFormat;
      this._usage = elementUsage;
    }

    public static bool operator ==(VertexElement left, VertexElement right)
    {
      if (left._offset == right._offset && left._usageIndex == right._usageIndex && left._usage == right._usage)
        return left._format == right._format;
      else
        return false;
    }

    public static bool operator !=(VertexElement left, VertexElement right)
    {
      return !(left == right);
    }

    public override int GetHashCode()
    {
      return 0;
    }

    public override string ToString()
    {
      return string.Format("{{Offset:{0} Format:{1} Usage:{2} UsageIndex:{3}}}", (object) this.Offset, (object) this.VertexElementFormat, (object) this.VertexElementUsage, (object) this.UsageIndex);
    }

    public override bool Equals(object obj)
    {
      if (obj == null || obj.GetType() != this.GetType())
        return false;
      else
        return this == (VertexElement) obj;
    }
  }
}
