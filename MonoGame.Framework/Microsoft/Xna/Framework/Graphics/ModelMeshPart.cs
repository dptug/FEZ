// Type: Microsoft.Xna.Framework.Graphics.ModelMeshPart
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System.Collections.ObjectModel;

namespace Microsoft.Xna.Framework.Graphics
{
  public sealed class ModelMeshPart
  {
    private Effect _effect;
    internal ModelMesh parent;

    public Effect Effect
    {
      get
      {
        return this._effect;
      }
      set
      {
        if (value == this._effect)
          return;
        if (this._effect != null)
        {
          bool flag = true;
          foreach (ModelMeshPart modelMeshPart in (ReadOnlyCollection<ModelMeshPart>) this.parent.MeshParts)
          {
            if (modelMeshPart != this && modelMeshPart._effect == this._effect)
            {
              flag = false;
              break;
            }
          }
          if (flag)
            this.parent.Effects.Remove(this._effect);
        }
        this._effect = value;
        this.parent.Effects.Add(value);
      }
    }

    public IndexBuffer IndexBuffer { get; set; }

    public int NumVertices { get; set; }

    public int PrimitiveCount { get; set; }

    public int StartIndex { get; set; }

    public object Tag { get; set; }

    public VertexBuffer VertexBuffer { get; set; }

    public int VertexOffset { get; set; }

    internal int VertexBufferIndex { get; set; }

    internal int IndexBufferIndex { get; set; }

    internal int EffectIndex { get; set; }
  }
}
