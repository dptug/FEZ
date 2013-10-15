// Type: Microsoft.Xna.Framework.Content.SpriteFontReader
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Microsoft.Xna.Framework.Content
{
  internal class SpriteFontReader : ContentTypeReader<SpriteFont>
  {
    private static string[] supportedExtensions = new string[1]
    {
      ".spritefont"
    };

    static SpriteFontReader()
    {
    }

    internal SpriteFontReader()
    {
    }

    internal static string Normalize(string fileName)
    {
      return ContentTypeReader.Normalize(fileName, SpriteFontReader.supportedExtensions);
    }

    protected internal override SpriteFont Read(ContentReader input, SpriteFont existingInstance)
    {
      if (existingInstance != null)
      {
        input.ReadObject<Texture2D>(existingInstance._texture);
        input.ReadObject<List<Rectangle>>();
        input.ReadObject<List<Rectangle>>();
        input.ReadObject<List<char>>();
        input.ReadInt32();
        double num1 = (double) input.ReadSingle();
        input.ReadObject<List<Vector3>>();
        if (input.ReadBoolean())
        {
          int num2 = (int) input.ReadChar();
        }
        return existingInstance;
      }
      else
      {
        Texture2D texture = input.ReadObject<Texture2D>();
        List<Rectangle> glyphBounds = input.ReadObject<List<Rectangle>>();
        List<Rectangle> cropping = input.ReadObject<List<Rectangle>>();
        List<char> characters = input.ReadObject<List<char>>();
        int lineSpacing = input.ReadInt32();
        float spacing = input.ReadSingle();
        List<Vector3> kerning = input.ReadObject<List<Vector3>>();
        char? defaultCharacter = new char?();
        if (input.ReadBoolean())
          defaultCharacter = new char?(input.ReadChar());
        return new SpriteFont(texture, glyphBounds, cropping, characters, lineSpacing, spacing, kerning, defaultCharacter);
      }
    }
  }
}
