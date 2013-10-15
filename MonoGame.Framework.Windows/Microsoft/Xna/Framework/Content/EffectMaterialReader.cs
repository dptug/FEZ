// Type: Microsoft.Xna.Framework.Content.EffectMaterialReader
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Content
{
  internal class EffectMaterialReader : ContentTypeReader<EffectMaterial>
  {
    protected internal override EffectMaterial Read(ContentReader input, EffectMaterial existingInstance)
    {
      EffectMaterial effectMaterial = new EffectMaterial(input.ReadExternalReference<Effect>());
      foreach (KeyValuePair<string, object> keyValuePair in input.ReadObject<Dictionary<string, object>>())
      {
        EffectParameter effectParameter = effectMaterial.Parameters[keyValuePair.Key];
        if (effectParameter != null)
        {
          if (!typeof (Texture).IsAssignableFrom(keyValuePair.Value.GetType()))
            throw new NotImplementedException();
          effectParameter.SetValue((Texture) keyValuePair.Value);
        }
      }
      return effectMaterial;
    }
  }
}
