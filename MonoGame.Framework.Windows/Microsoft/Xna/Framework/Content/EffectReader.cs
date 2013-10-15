// Type: Microsoft.Xna.Framework.Content.EffectReader
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Xna.Framework.Content
{
  internal class EffectReader : ContentTypeReader<Effect>
  {
    private static string[] supportedExtensions = new string[1]
    {
      ".fxg"
    };

    static EffectReader()
    {
    }

    public static string Normalize(string FileName)
    {
      return ContentTypeReader.Normalize(FileName, EffectReader.supportedExtensions);
    }

    private static string TryFindAnyCased(string search, string[] arr, params string[] extensions)
    {
      return Enumerable.FirstOrDefault<string>((IEnumerable<string>) arr, (Func<string, bool>) (s => Enumerable.Any<string>((IEnumerable<string>) extensions, (Func<string, bool>) (ext => s.ToLower() == search.ToLower() + ext))));
    }

    private static bool Contains(string search, string[] arr)
    {
      return Enumerable.Any<string>((IEnumerable<string>) arr, (Func<string, bool>) (s => s == search));
    }

    protected internal override Effect Read(ContentReader input, Effect existingInstance)
    {
      int count = input.ReadInt32();
      Effect effect = new Effect(input.GraphicsDevice, input.ReadBytes(count));
      effect.Name = input.AssetName;
      return effect;
    }
  }
}
