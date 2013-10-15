// Type: Microsoft.Xna.Framework.Graphics.ModelMesh
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Graphics
{
  public sealed class ModelMesh
  {
    private GraphicsDevice graphicsDevice;
    private List<ModelMeshPart> parts;

    public BoundingSphere BoundingSphere { get; set; }

    public ModelEffectCollection Effects { get; set; }

    public ModelMeshPartCollection MeshParts { get; set; }

    public string Name { get; set; }

    public ModelBone ParentBone { get; set; }

    public object Tag { get; set; }

    public ModelMesh(GraphicsDevice graphicsDevice, List<ModelMeshPart> parts)
    {
      this.parts = parts;
      this.graphicsDevice = graphicsDevice;
      this.MeshParts = new ModelMeshPartCollection((IList<ModelMeshPart>) parts);
      for (int index = 0; index < parts.Count; ++index)
        parts[index].parent = this;
      this.Effects = new ModelEffectCollection();
    }

    public void Draw()
    {
      for (int index1 = 0; index1 < this.MeshParts.Count; ++index1)
      {
        ModelMeshPart modelMeshPart = this.MeshParts[index1];
        Effect effect = modelMeshPart.Effect;
        if (modelMeshPart.PrimitiveCount > 0)
        {
          this.graphicsDevice.SetVertexBuffer(modelMeshPart.VertexBuffer);
          this.graphicsDevice.Indices = modelMeshPart.IndexBuffer;
          for (int index2 = 0; index2 < effect.CurrentTechnique.Passes.Count; ++index2)
          {
            effect.CurrentTechnique.Passes[index2].Apply();
            this.graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, modelMeshPart.VertexOffset, 0, modelMeshPart.NumVertices, modelMeshPart.StartIndex, modelMeshPart.PrimitiveCount);
          }
        }
      }
    }
  }
}
