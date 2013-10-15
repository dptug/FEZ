// Type: Microsoft.Xna.Framework.Graphics.EffectTechnique
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

namespace Microsoft.Xna.Framework.Graphics
{
  public class EffectTechnique
  {
    public EffectPassCollection Passes { get; private set; }

    public EffectAnnotationCollection Annotations { get; private set; }

    public string Name { get; private set; }

    internal EffectTechnique(Effect effect, EffectTechnique cloneSource)
    {
      this.Name = cloneSource.Name;
      this.Annotations = cloneSource.Annotations;
      this.Passes = new EffectPassCollection(effect, cloneSource.Passes);
    }

    internal EffectTechnique(Effect effect, string name, EffectPassCollection passes, EffectAnnotationCollection annotations)
    {
      this.Name = name;
      this.Passes = passes;
      this.Annotations = annotations;
    }
  }
}
