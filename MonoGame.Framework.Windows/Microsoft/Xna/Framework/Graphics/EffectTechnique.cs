// Type: Microsoft.Xna.Framework.Graphics.EffectTechnique
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

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
