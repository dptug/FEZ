// Type: Microsoft.Xna.Framework.Content.EffectMaterialReader
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

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
