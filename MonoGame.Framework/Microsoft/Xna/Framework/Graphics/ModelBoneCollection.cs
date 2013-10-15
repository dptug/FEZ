// Type: Microsoft.Xna.Framework.Graphics.ModelBoneCollection
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Microsoft.Xna.Framework.Graphics
{
  public class ModelBoneCollection : ReadOnlyCollection<ModelBone>
  {
    public ModelBone this[string boneName]
    {
      get
      {
        ModelBone modelBone;
        if (this.TryGetValue(boneName, out modelBone))
          return modelBone;
        else
          throw new KeyNotFoundException();
      }
    }

    public ModelBoneCollection(IList<ModelBone> list)
      : base(list)
    {
    }

    public bool TryGetValue(string boneName, out ModelBone value)
    {
      foreach (ModelBone modelBone in (IEnumerable<ModelBone>) this.Items)
      {
        if (modelBone.Name == boneName)
        {
          value = modelBone;
          return true;
        }
      }
      value = (ModelBone) null;
      return false;
    }
  }
}
