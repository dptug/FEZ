// Type: Microsoft.Xna.Framework.Graphics.ModelMeshPart
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

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
