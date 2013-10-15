// Type: Microsoft.Xna.Framework.Graphics.Model
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Microsoft.Xna.Framework.Graphics
{
  public class Model
  {
    private static Matrix[] sharedDrawBoneMatrices;
    private GraphicsDevice graphicsDevice;
    private List<ModelBone> bones;
    private List<ModelMesh> meshes;

    public ModelBoneCollection Bones { get; private set; }

    public ModelMeshCollection Meshes { get; private set; }

    public ModelBone Root { get; set; }

    public object Tag { get; set; }

    public Model()
    {
    }

    public Model(GraphicsDevice graphicsDevice, List<ModelBone> bones, List<ModelMesh> meshes)
    {
      this.bones = bones;
      this.meshes = meshes;
      this.graphicsDevice = graphicsDevice;
      this.Bones = new ModelBoneCollection((IList<ModelBone>) bones);
      this.Meshes = new ModelMeshCollection((IList<ModelMesh>) meshes);
    }

    public void BuildHierarchy()
    {
      Matrix scale = Matrix.CreateScale(0.01f);
      foreach (ModelBone node in (ReadOnlyCollection<ModelBone>) this.Root.Children)
        this.BuildHierarchy(node, this.Root.Transform * scale, 0);
    }

    private void BuildHierarchy(ModelBone node, Matrix parentTransform, int level)
    {
      node.ModelTransform = node.Transform * parentTransform;
      foreach (ModelBone node1 in (ReadOnlyCollection<ModelBone>) node.Children)
        this.BuildHierarchy(node1, node.ModelTransform, level + 1);
    }

    public void Draw(Matrix world, Matrix view, Matrix projection)
    {
      int count = this.bones.Count;
      if (Model.sharedDrawBoneMatrices == null || Model.sharedDrawBoneMatrices.Length < count)
        Model.sharedDrawBoneMatrices = new Matrix[count];
      this.CopyAbsoluteBoneTransformsTo(Model.sharedDrawBoneMatrices);
      foreach (ModelMesh modelMesh in (ReadOnlyCollection<ModelMesh>) this.Meshes)
      {
        foreach (Effect effect in modelMesh.Effects)
        {
          IEffectMatrices effectMatrices = effect as IEffectMatrices;
          if (effectMatrices == null)
            throw new InvalidOperationException();
          effectMatrices.World = Model.sharedDrawBoneMatrices[modelMesh.ParentBone.Index] * world;
          effectMatrices.View = view;
          effectMatrices.Projection = projection;
        }
        modelMesh.Draw();
      }
    }

    public void CopyAbsoluteBoneTransformsTo(Matrix[] destinationBoneTransforms)
    {
      if (destinationBoneTransforms == null)
        throw new ArgumentNullException("destinationBoneTransforms");
      if (destinationBoneTransforms.Length < this.bones.Count)
        throw new ArgumentOutOfRangeException("destinationBoneTransforms");
      int count = this.bones.Count;
      for (int index1 = 0; index1 < count; ++index1)
      {
        ModelBone modelBone = this.bones[index1];
        if (modelBone.Parent == null)
        {
          destinationBoneTransforms[index1] = modelBone.transform;
        }
        else
        {
          int index2 = modelBone.Parent.Index;
          Matrix.Multiply(ref modelBone.transform, ref destinationBoneTransforms[index2], out destinationBoneTransforms[index1]);
        }
      }
    }
  }
}
