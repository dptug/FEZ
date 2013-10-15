// Type: FezEngine.Structure.Geometry.IndexedPrimitiveCollectionBase`2
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Effects;
using FezEngine.Tools;
using Microsoft.Xna.Framework.Graphics;

namespace FezEngine.Structure.Geometry
{
  public abstract class IndexedPrimitiveCollectionBase<VertexType, IndexType> : IIndexedPrimitiveCollection
  {
    protected readonly GraphicsDevice device;
    protected readonly IGraphicsDeviceService GraphicsDeviceService;
    protected VertexType[] vertices;
    protected IndexType[] indices;
    protected PrimitiveType primitiveType;
    protected int primitiveCount;

    public PrimitiveType PrimitiveType
    {
      get
      {
        return this.primitiveType;
      }
      set
      {
        this.primitiveType = value;
        this.UpdatePrimitiveCount();
      }
    }

    public virtual VertexType[] Vertices
    {
      get
      {
        return this.vertices;
      }
      set
      {
        this.vertices = value;
      }
    }

    public virtual IndexType[] Indices
    {
      get
      {
        return this.indices;
      }
      set
      {
        this.indices = value;
        this.UpdatePrimitiveCount();
      }
    }

    public int VertexCount
    {
      get
      {
        return this.vertices.Length;
      }
    }

    public bool Empty
    {
      get
      {
        return this.primitiveCount == 0;
      }
    }

    private IndexedPrimitiveCollectionBase()
    {
      if (!ServiceHelper.IsFull)
        return;
      this.GraphicsDeviceService = ServiceHelper.Get<IGraphicsDeviceService>();
    }

    protected IndexedPrimitiveCollectionBase(PrimitiveType type)
      : this()
    {
      this.primitiveType = type;
      if (this.GraphicsDeviceService == null)
        return;
      this.device = this.GraphicsDeviceService.GraphicsDevice;
    }

    protected void UpdatePrimitiveCount()
    {
      this.primitiveCount = this.indices.Length;
      switch (this.primitiveType)
      {
        case PrimitiveType.TriangleList:
          this.primitiveCount /= 3;
          break;
        case PrimitiveType.TriangleStrip:
          this.primitiveCount -= 2;
          break;
        case PrimitiveType.LineList:
          this.primitiveCount /= 2;
          break;
        case PrimitiveType.LineStrip:
          --this.primitiveCount;
          break;
      }
    }

    public abstract void Draw(BaseEffect effect);

    public abstract IIndexedPrimitiveCollection Clone();
  }
}
