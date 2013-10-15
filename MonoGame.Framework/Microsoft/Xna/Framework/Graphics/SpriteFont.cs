// Type: Microsoft.Xna.Framework.Graphics.SpriteFont
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Microsoft.Xna.Framework.Graphics
{
  public sealed class SpriteFont
  {
    private readonly Dictionary<char, SpriteFont.Glyph> _glyphs;
    internal readonly Texture2D _texture;
    private ReadOnlyCollection<char> _characters;

    public ReadOnlyCollection<char> Characters
    {
      get
      {
        return this._characters;
      }
    }

    public char? DefaultCharacter { get; set; }

    public int LineSpacing { get; set; }

    public float Spacing { get; set; }

    internal SpriteFont(Texture2D texture, List<Rectangle> glyphBounds, List<Rectangle> cropping, List<char> characters, int lineSpacing, float spacing, List<Vector3> kerning, char? defaultCharacter)
    {
      this._characters = new ReadOnlyCollection<char>((IList<char>) characters.ToArray());
      this._texture = texture;
      this.LineSpacing = lineSpacing;
      this.Spacing = spacing;
      this.DefaultCharacter = defaultCharacter;
      this._glyphs = new Dictionary<char, SpriteFont.Glyph>(characters.Count);
      for (int index = 0; index < characters.Count; ++index)
      {
        SpriteFont.Glyph glyph = new SpriteFont.Glyph()
        {
          BoundsInTexture = glyphBounds[index],
          Cropping = cropping[index],
          Character = characters[index],
          LeftSideBearing = kerning[index].X,
          Width = kerning[index].Y,
          RightSideBearing = kerning[index].Z,
          WidthIncludingBearings = kerning[index].X + kerning[index].Y + kerning[index].Z
        };
        this._glyphs.Add(glyph.Character, glyph);
      }
    }

    public Vector2 MeasureString(string text)
    {
      SpriteFont.CharacterSource text1 = new SpriteFont.CharacterSource(text);
      Vector2 size;
      this.MeasureString(ref text1, out size);
      return size;
    }

    public Vector2 MeasureString(StringBuilder text)
    {
      SpriteFont.CharacterSource text1 = new SpriteFont.CharacterSource(text);
      Vector2 size;
      this.MeasureString(ref text1, out size);
      return size;
    }

    private void MeasureString(ref SpriteFont.CharacterSource text, out Vector2 size)
    {
      if (text.Length == 0)
      {
        size = Vector2.Zero;
      }
      else
      {
        SpriteFont.Glyph? nullable = new SpriteFont.Glyph?();
        if (this.DefaultCharacter.HasValue)
          nullable = new SpriteFont.Glyph?(this._glyphs[this.DefaultCharacter.Value]);
        float num1 = 0.0f;
        float num2 = (float) this.LineSpacing;
        int num3 = 0;
        SpriteFont.Glyph glyph = SpriteFont.Glyph.Empty;
        Vector2 zero = Vector2.Zero;
        bool flag = false;
        for (int index = 0; index < text.Length; ++index)
        {
          char key = text[index];
          switch (key)
          {
            case '\r':
              flag = false;
              break;
            case '\n':
              ++num3;
              num2 = (float) this.LineSpacing;
              zero.X = 0.0f;
              zero.Y = (float) (this.LineSpacing * num3);
              flag = false;
              break;
            default:
              if (flag)
                zero.X += this.Spacing + glyph.WidthIncludingBearings;
              flag = this._glyphs.TryGetValue(key, out glyph);
              if (!flag)
              {
                if (!nullable.HasValue)
                  throw new ArgumentException("Text contains characters that cannot be resolved by this SpriteFont.", "text");
                glyph = nullable.Value;
                flag = true;
              }
              float num4 = zero.X + glyph.WidthIncludingBearings;
              if ((double) num4 > (double) num1)
                num1 = num4;
              if ((double) glyph.Cropping.Height > (double) num2)
              {
                num2 = (float) glyph.Cropping.Height;
                break;
              }
              else
                break;
          }
        }
        size.X = num1;
        size.Y = (float) (num3 * this.LineSpacing) + num2;
      }
    }

    internal void DrawInto(SpriteBatch spriteBatch, ref SpriteFont.CharacterSource text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effect, float depth)
    {
      Vector2 zero1 = Vector2.Zero;
      bool flag1 = (effect & SpriteEffects.FlipVertically) == SpriteEffects.FlipVertically;
      bool flag2 = (effect & SpriteEffects.FlipHorizontally) == SpriteEffects.FlipHorizontally;
      if (flag1 || flag2)
      {
        Vector2 size;
        this.MeasureString(ref text, out size);
        if (flag2)
        {
          origin.X *= -1f;
          scale.X *= -1f;
          zero1.X = -size.X;
        }
        if (flag1)
        {
          origin.Y *= -1f;
          scale.Y *= -1f;
          zero1.Y = (float) this.LineSpacing - size.Y;
        }
      }
      Matrix result1;
      Matrix.CreateTranslation(-origin.X, -origin.Y, 0.0f, out result1);
      Matrix result2;
      Matrix.CreateScale(scale.X, scale.Y, 1f, out result2);
      Matrix.Multiply(ref result1, ref result2, out result1);
      Matrix.CreateTranslation(zero1.X, zero1.Y, 0.0f, out result2);
      Matrix.Multiply(ref result2, ref result1, out result1);
      Matrix.CreateRotationZ(rotation, out result2);
      Matrix.Multiply(ref result1, ref result2, out result1);
      Matrix.CreateTranslation(position.X, position.Y, 0.0f, out result2);
      Matrix.Multiply(ref result1, ref result2, out result1);
      SpriteFont.Glyph? nullable = new SpriteFont.Glyph?();
      if (this.DefaultCharacter.HasValue)
        nullable = new SpriteFont.Glyph?(this._glyphs[this.DefaultCharacter.Value]);
      SpriteFont.Glyph glyph = SpriteFont.Glyph.Empty;
      Vector2 zero2 = Vector2.Zero;
      bool flag3 = false;
      for (int index = 0; index < text.Length; ++index)
      {
        char key = text[index];
        switch (key)
        {
          case '\r':
            flag3 = false;
            break;
          case '\n':
            zero2.X = 0.0f;
            zero2.Y += (float) this.LineSpacing;
            flag3 = false;
            break;
          default:
            if (flag3)
              zero2.X += this.Spacing + glyph.Width + glyph.RightSideBearing;
            flag3 = this._glyphs.TryGetValue(key, out glyph);
            if (!flag3)
            {
              if (!nullable.HasValue)
                throw new ArgumentException("Text contains characters that cannot be resolved by this SpriteFont.", "text");
              glyph = nullable.Value;
              flag3 = true;
            }
            zero2.X += glyph.LeftSideBearing;
            Vector2 result3 = zero2;
            if (flag2)
              result3.X += (float) glyph.BoundsInTexture.Width;
            result3.X += (float) glyph.Cropping.X;
            if (flag1)
              result3.Y += (float) (glyph.BoundsInTexture.Height - this.LineSpacing);
            result3.Y += (float) glyph.Cropping.Y;
            Vector2.Transform(ref result3, ref result1, out result3);
            Vector4 destinationRectangle = new Vector4(result3.X, result3.Y, (float) glyph.BoundsInTexture.Width * scale.X, (float) glyph.BoundsInTexture.Height * scale.Y);
            spriteBatch.DrawInternal(this._texture, destinationRectangle, new Rectangle?(glyph.BoundsInTexture), color, rotation, Vector2.Zero, effect, depth);
            break;
        }
      }
    }

    private static class Errors
    {
      public const string TextContainsUnresolvableCharacters = "Text contains characters that cannot be resolved by this SpriteFont.";
    }

    internal struct CharacterSource
    {
      private readonly string _string;
      private readonly StringBuilder _builder;
      public readonly int Length;

      public char this[int index]
      {
        get
        {
          if (this._string != null)
            return this._string[index];
          else
            return this._builder[index];
        }
      }

      public CharacterSource(string s)
      {
        this._string = s;
        this._builder = (StringBuilder) null;
        this.Length = s.Length;
      }

      public CharacterSource(StringBuilder builder)
      {
        this._builder = builder;
        this._string = (string) null;
        this.Length = this._builder.Length;
      }
    }

    private struct Glyph
    {
      public static readonly SpriteFont.Glyph Empty = new SpriteFont.Glyph();
      public char Character;
      public Rectangle BoundsInTexture;
      public Rectangle Cropping;
      public float LeftSideBearing;
      public float RightSideBearing;
      public float Width;
      public float WidthIncludingBearings;

      static Glyph()
      {
      }

      public override string ToString()
      {
        return string.Format("CharacterIndex={0}, Glyph={1}, Cropping={2}, Kerning={3},{4},{5}", (object) this.Character, (object) this.BoundsInTexture, (object) this.Cropping, (object) this.LeftSideBearing, (object) this.Width, (object) this.RightSideBearing);
      }
    }
  }
}
