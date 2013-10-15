// Type: Microsoft.Xna.Framework.Graphics.ModelBoneCollection
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

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
