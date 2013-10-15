// Type: FezEngine.Structure.Geometry.BufferedIndexedPrimitives`1
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Effects;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FezEngine.Structure.Geometry
{
  public class BufferedIndexedPrimitives<T> : IndexedPrimitiveCollectionBase<T, int>, IDisposable where T : struct, IVertexType
  {
    private VertexBuffer vertexBuffer;
    private IndexBuffer indexBuffer;
    private int vertexCount;

    public BufferedIndexedPrimitives(PrimitiveType type)
      : this((T[]) null, (int[]) null, type)
    {
    }

    public BufferedIndexedPrimitives(T[] vertices, int[] indices, PrimitiveType type)
      : base(type)
    {
      this.vertices = vertices ?? new T[0];
      this.Indices = indices ?? new int[0];
    }

    public void UpdateBuffers()
    {
      this.vertexCount = this.VertexCount;
      if (this.vertexBuffer != null)
        this.vertexBuffer.Dispose();
      this.vertexBuffer = new VertexBuffer(this.device, typeof (T), this.vertexCount, BufferUsage.WriteOnly);
      if (this.indexBuffer != null)
        this.indexBuffer.Dispose();
      this.indexBuffer = new IndexBuffer(this.device, IndexElementSize.ThirtyTwoBits, this.indices.Length, BufferUsage.WriteOnly);
      this.device.SetVertexBuffer((VertexBuffer) null);
      this.vertexBuffer.SetData<T>(this.vertices);
      this.device.Indices = (IndexBuffer) null;
      this.indexBuffer.SetData<int>(this.indices);
    }

    public void CleanUp()
    {
      this.indices = (int[]) null;
      this.vertices = (T[]) null;
    }

    public void Dispose()
    {
      this.CleanUp();
      if (this.indexBuffer != null)
        this.indexBuffer.Dispose();
      this.indexBuffer = (IndexBuffer) null;
      if (this.vertexBuffer != null)
        this.vertexBuffer.Dispose();
      this.vertexBuffer = (VertexBuffer) null;
    }

    public override void Draw(BaseEffect effect)
    {
      if (this.device == null || this.primitiveCount <= 0 || (this.indexBuffer == null || this.vertexBuffer == null))
        return;
      this.device.SetVertexBuffer(this.vertexBuffer);
      this.device.Indices = this.indexBuffer;
      effect.Apply();
      this.device.DrawIndexedPrimitives(this.primitiveType, 0, 0, this.vertexCount, 0, this.primitiveCount);
    }

    public override IIndexedPrimitiveCollection Clone()
    {
      throw new NotImplementedException();
    }
  }
}
