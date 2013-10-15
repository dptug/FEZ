// Type: FezEngine.Structure.Geometry.ShaderInstancedIndexedPrimitives`2
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Effects;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FezEngine.Structure.Geometry
{
  public class ShaderInstancedIndexedPrimitives<TemplateType, InstanceType> : IndexedPrimitiveCollectionBase<TemplateType, int>, IFakeDisposable where TemplateType : struct, IShaderInstantiatableVertex where InstanceType : struct
  {
    public int PredictiveBatchSize = 16;
    private int[] tempIndices = new int[0];
    private TemplateType[] tempVertices = new TemplateType[0];
    private readonly int InstancesPerBatch;
    private VertexBuffer vertexBuffer;
    private IndexBuffer indexBuffer;
    public InstanceType[] Instances;
    public int InstanceCount;
    private int oldInstanceCount;
    private InstanceType[] tempInstances;

    public bool NeedsEffectCommit { get; set; }

    public override TemplateType[] Vertices
    {
      get
      {
        return base.Vertices;
      }
      set
      {
        base.Vertices = value;
        this.UpdateBuffers(true);
      }
    }

    public override int[] Indices
    {
      get
      {
        return base.Indices;
      }
      set
      {
        base.Indices = value;
        this.UpdateBuffers(true);
      }
    }

    public bool IsDisposed { get; private set; }

    public ShaderInstancedIndexedPrimitives(PrimitiveType type, int instancesPerBatch)
      : base(type)
    {
      this.InstancesPerBatch = instancesPerBatch;
    }

    public void MaximizeBuffers(int maxInstances)
    {
      int num = this.InstanceCount;
      this.InstanceCount = maxInstances;
      this.UpdateBuffers();
      this.InstanceCount = num;
    }

    public void ResetBuffers()
    {
      this.oldInstanceCount = 0;
      if (this.indexBuffer != null)
        this.indexBuffer.Dispose();
      this.indexBuffer = (IndexBuffer) null;
      Array.Resize<int>(ref this.tempIndices, 0);
      Array.Resize<InstanceType>(ref this.tempInstances, 0);
      Array.Resize<TemplateType>(ref this.tempVertices, 0);
      if (this.vertexBuffer != null)
        this.vertexBuffer.Dispose();
      this.vertexBuffer = (VertexBuffer) null;
    }

    public void Dispose()
    {
      this.ResetBuffers();
      this.IsDisposed = true;
    }

    public void UpdateBuffers()
    {
      this.UpdateBuffers(false);
    }

    private void UpdateBuffers(bool rebuild)
    {
      if (this.device == null || this.vertices == null || (this.vertices.Length <= 0 || this.indices == null) || (this.indices.Length <= 0 || this.Instances == null || this.InstanceCount <= 0))
        return;
      int num1 = (int) Math.Ceiling((double) this.oldInstanceCount / (double) this.PredictiveBatchSize) * this.PredictiveBatchSize;
      int num2 = (int) Math.Ceiling((double) this.InstanceCount / (double) this.PredictiveBatchSize) * this.PredictiveBatchSize;
      bool flag1 = num2 > num1;
      bool flag2 = this.InstanceCount > this.oldInstanceCount;
      if (this.vertexBuffer == null || rebuild || flag1)
      {
        if (this.vertexBuffer != null)
          this.vertexBuffer.Dispose();
        this.vertexBuffer = new VertexBuffer(this.device, typeof (TemplateType), num2 * this.vertices.Length, BufferUsage.WriteOnly);
      }
      if (this.indexBuffer == null || rebuild || flag1)
      {
        if (this.indexBuffer != null)
          this.indexBuffer.Dispose();
        this.indexBuffer = new IndexBuffer(this.device, IndexElementSize.ThirtyTwoBits, num2 * this.indices.Length, BufferUsage.WriteOnly);
      }
      if (rebuild || flag1)
      {
        Array.Resize<int>(ref this.tempIndices, num2 * this.indices.Length);
        Array.Resize<TemplateType>(ref this.tempVertices, num2 * this.vertices.Length);
      }
      if (rebuild)
        this.oldInstanceCount = 0;
      if (this.oldInstanceCount == 0)
        Array.Copy((Array) this.indices, (Array) this.tempIndices, this.indices.Length);
      for (int index1 = this.oldInstanceCount; index1 < this.InstanceCount; ++index1)
      {
        int destinationIndex = this.vertices.Length * index1;
        Array.Copy((Array) this.vertices, 0, (Array) this.tempVertices, destinationIndex, this.vertices.Length);
        for (int index2 = 0; index2 < this.vertices.Length; ++index2)
          this.tempVertices[destinationIndex + index2].InstanceIndex = (float) index1;
        if (index1 != 0)
        {
          int num3 = index1 * this.indices.Length;
          for (int index2 = 0; index2 < this.indices.Length; ++index2)
            this.tempIndices[num3 + index2] = this.indices[index2] + destinationIndex;
        }
      }
      if (!rebuild && !flag2)
        return;
      this.vertexBuffer.SetData<TemplateType>(this.tempVertices);
      this.indexBuffer.SetData<int>(this.tempIndices);
      this.oldInstanceCount = this.InstanceCount;
    }

    public override void Draw(BaseEffect effect)
    {
      if (this.device == null || this.primitiveCount <= 0 || (this.vertices == null || this.vertices.Length <= 0) || (this.indexBuffer == null || this.vertexBuffer == null || (this.Instances == null || this.InstanceCount <= 0)))
        return;
      IShaderInstantiatableEffect<InstanceType> instantiatableEffect = effect as IShaderInstantiatableEffect<InstanceType>;
      this.device.SetVertexBuffer(this.vertexBuffer);
      this.device.Indices = this.indexBuffer;
      int val1 = this.InstanceCount;
      while (val1 > 0)
      {
        int length = Math.Min(val1, this.InstancesPerBatch);
        int sourceIndex = this.InstanceCount - val1;
        if (this.tempInstances == null || this.tempInstances.Length < length)
          this.tempInstances = new InstanceType[Math.Min((int) Math.Ceiling((double) length / (double) this.PredictiveBatchSize) * this.PredictiveBatchSize, this.InstancesPerBatch)];
        Array.Copy((Array) this.Instances, sourceIndex, (Array) this.tempInstances, 0, length);
        instantiatableEffect.SetInstanceData(this.tempInstances);
        effect.Apply();
        this.device.DrawIndexedPrimitives(this.primitiveType, 0, 0, length * this.vertices.Length, 0, length * this.primitiveCount);
        val1 -= length;
      }
    }

    public override IIndexedPrimitiveCollection Clone()
    {
      ShaderInstancedIndexedPrimitives<TemplateType, InstanceType> indexedPrimitives = new ShaderInstancedIndexedPrimitives<TemplateType, InstanceType>(this.primitiveType, this.InstancesPerBatch);
      indexedPrimitives.NeedsEffectCommit = true;
      indexedPrimitives.Vertices = this.Vertices;
      indexedPrimitives.Indices = this.Indices;
      return (IIndexedPrimitiveCollection) indexedPrimitives;
    }
  }
}
