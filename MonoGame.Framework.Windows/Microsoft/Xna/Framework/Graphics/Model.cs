// Type: Microsoft.Xna.Framework.Graphics.Model
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Microsoft.Xna.Framework.Graphics
{
  public class Model
  {
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
      string str = string.Empty;
      for (int index = 0; index < level; ++index)
        str = str + "\t";
    }

    public void Draw(Matrix world, Matrix view, Matrix projection)
    {
      Matrix[] destinationBoneTransforms = new Matrix[this.bones.Count];
      this.CopyAbsoluteBoneTransformsTo(destinationBoneTransforms);
      foreach (ModelMesh modelMesh in (ReadOnlyCollection<ModelMesh>) this.Meshes)
      {
        foreach (Effect effect in modelMesh.Effects)
        {
          IEffectMatrices effectMatrices = effect as IEffectMatrices;
          if (effectMatrices == null)
            throw new InvalidOperationException();
          effectMatrices.World = destinationBoneTransforms[modelMesh.ParentBone.Index] * world;
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
