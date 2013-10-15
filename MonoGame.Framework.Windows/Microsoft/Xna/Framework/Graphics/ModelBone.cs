// Type: Microsoft.Xna.Framework.Graphics.ModelBone
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

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
