// Type: Microsoft.Xna.Framework.Graphics.ModelMeshPartCollection
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Microsoft.Xna.Framework.Graphics
{
  public sealed class ModelMeshPartCollection : ReadOnlyCollection<ModelMeshPart>
  {
    public ModelMeshPartCollection(IList<ModelMeshPart> list)
      : base(list)
    {
    }
  }
}
