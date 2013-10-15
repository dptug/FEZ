// Type: Microsoft.Xna.Framework.Graphics.ModelBone
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Graphics
{
  public sealed class ModelBone
  {
    private List<ModelBone> children = new List<ModelBone>();
    private List<ModelMesh> meshes = new List<ModelMesh>();
    internal Matrix transform;

    public List<ModelMesh> Meshes
    {
      get
      {
        return this.meshes;
      }
      private set
      {
        this.meshes = value;
      }
    }

    public ModelBoneCollection Children { get; private set; }

    public int Index { get; set; }

    public string Name { get; set; }

    public ModelBone Parent { get; set; }

    public Matrix Transform
    {
      get
      {
        return this.transform;
      }
      set
      {
        this.transform = value;
      }
    }

    public Matrix ModelTransform { get; set; }

    public ModelBone()
    {
      this.Children = new ModelBoneCollection((IList<ModelBone>) new List<ModelBone>());
    }

    public void AddMesh(ModelMesh mesh)
    {
      this.meshes.Add(mesh);
    }

    public void AddChild(ModelBone modelBone)
    {
      this.children.Add(modelBone);
      this.Children = new ModelBoneCollection((IList<ModelBone>) this.children);
    }
  }
}
