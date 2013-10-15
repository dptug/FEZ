// Type: Microsoft.Xna.Framework.Color
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System;

namespace Microsoft.Xna.Framework
{
  [Serializable]
  public struct Color : IEquatable<Color>
  {
    private uint _packedValue;

    public byte B
    {
      get
      {
        return (byte) (this._packedValue >> 16);
      }
      set
      {
        this._packedValue = (uint) ((int) this._packedValue & -16711681 | (int) value << 16);
      }
    }

    public byte G
    {
      get
      {
        return (byte) (this._packedValue >> 8);
      }
      set
      {
        this._packedValue = (uint) ((int) this._packedValue & -65281 | (int) value << 8);
      }
    }

    public byte R
    {
      get
      {
        return (byte) this._packedValue;
      }
      set
      {
        this._packedValue = this._packedValue & 4294967040U | (uint) value;
      }
    }

    public byte A
    {
      get
      {
        return (byte) (this._packedValue >> 24);
      }
      set
      {
        this._packedValue = (uint) ((int) this._packedValue & 16777215 | (int) value << 24);
      }
    }

    public static Color TransparentBlack
    {
      get
      {
        return new Color(0U);
      }
    }

    public static Color Transparent
    {
      get
      {
        return new Color(0U);
      }
    }

    public static Color AliceBlue
    {
      get
      {
        return new Color(4294965488U);
      }
    }

    public static Color AntiqueWhite
    {
      get
      {
        return new Color(4292340730U);
      }
    }

    public static Color Aqua
    {
      get
      {
        return new Color(4294967040U);
      }
    }

    public static Color Aquamarine
    {
      get
      {
        return new Color(4292149119U);
      }
    }

    public static Color Azure
    {
      get
      {
        return new Color(4294967280U);
      }
    }

    public static Color Beige
    {
      get
      {
        return new Color(4292670965U);
      }
    }

    public static Color Bisque
    {
      get
      {
        return new Color(4291093759U);
      }
    }

    public static Color Black
    {
      get
      {
        return new Color(4278190080U);
      }
    }

    public static Color BlanchedAlmond
    {
      get
      {
        return new Color(4291685375U);
      }
    }

    public static Color Blue
    {
      get
      {
        return new Color(4294901760U);
      }
    }

    public static Color BlueViolet
    {
      get
      {
        return new Color(4293012362U);
      }
    }

    public static Color Brown
    {
      get
      {
        return new Color(4280953509U);
      }
    }

    public static Color BurlyWood
    {
      get
      {
        return new Color(4287084766U);
      }
    }

    public static Color CadetBlue
    {
      get
      {
        return new Color(4288716383U);
      }
    }

    public static Color Chartreuse
    {
      get
      {
        return new Color(4278255487U);
      }
    }

    public static Color Chocolate
    {
      get
      {
        return new Color(4280183250U);
      }
    }

    public static Color Coral
    {
      get
      {
        return new Color(4283465727U);
      }
    }

    public static Color CornflowerBlue
    {
      get
      {
        return new Color(4293760356U);
      }
    }

    public static Color Cornsilk
    {
      get
      {
        return new Color(4292671743U);
      }
    }

    public static Color Crimson
    {
      get
      {
        return new Color(4282127580U);
      }
    }

    public static Color Cyan
    {
      get
      {
        return new Color(4294967040U);
      }
    }

    public static Color DarkBlue
    {
      get
      {
        return new Color(4287299584U);
      }
    }

    public static Color DarkCyan
    {
      get
      {
        return new Color(4287335168U);
      }
    }

    public static Color DarkGoldenrod
    {
      get
      {
        return new Color(4278945464U);
      }
    }

    public static Color DarkGray
    {
      get
      {
        return new Color(4289309097U);
      }
    }

    public static Color DarkGreen
    {
      get
      {
        return new Color(4278215680U);
      }
    }

    public static Color DarkKhaki
    {
      get
      {
        return new Color(4285249469U);
      }
    }

    public static Color DarkMagenta
    {
      get
      {
        return new Color(4287299723U);
      }
    }

    public static Color DarkOliveGreen
    {
      get
      {
        return new Color(4281297749U);
      }
    }

    public static Color DarkOrange
    {
      get
      {
        return new Color(4278226175U);
      }
    }

    public static Color DarkOrchid
    {
      get
      {
        return new Color(4291572377U);
      }
    }

    public static Color DarkRed
    {
      get
      {
        return new Color(4278190219U);
      }
    }

    public static Color DarkSalmon
    {
      get
      {
        return new Color(4286224105U);
      }
    }

    public static Color DarkSeaGreen
    {
      get
      {
        return new Color(4287347855U);
      }
    }

    public static Color DarkSlateBlue
    {
      get
      {
        return new Color(4287315272U);
      }
    }

    public static Color DarkSlateGray
    {
      get
      {
        return new Color(4283387695U);
      }
    }

    public static Color DarkTurquoise
    {
      get
      {
        return new Color(4291939840U);
      }
    }

    public static Color DarkViolet
    {
      get
      {
        return new Color(4292018324U);
      }
    }

    public static Color DeepPink
    {
      get
      {
        return new Color(4287829247U);
      }
    }

    public static Color DeepSkyBlue
    {
      get
      {
        return new Color(4294950656U);
      }
    }

    public static Color DimGray
    {
      get
      {
        return new Color(4285098345U);
      }
    }

    public static Color DodgerBlue
    {
      get
      {
        return new Color(4294938654U);
      }
    }

    public static Color Firebrick
    {
      get
      {
        return new Color(4280427186U);
      }
    }

    public static Color FloralWhite
    {
      get
      {
        return new Color(4293982975U);
      }
    }

    public static Color ForestGreen
    {
      get
      {
        return new Color(4280453922U);
      }
    }

    public static Color Fuchsia
    {
      get
      {
        return new Color(4294902015U);
      }
    }

    public static Color Gainsboro
    {
      get
      {
        return new Color(4292664540U);
      }
    }

    public static Color GhostWhite
    {
      get
      {
        return new Color(4294965496U);
      }
    }

    public static Color Gold
    {
      get
      {
        return new Color(4278245375U);
      }
    }

    public static Color Goldenrod
    {
      get
      {
        return new Color(4280329690U);
      }
    }

    public static Color Gray
    {
      get
      {
        return new Color(4286611584U);
      }
    }

    public static Color Green
    {
      get
      {
        return new Color(4278222848U);
      }
    }

    public static Color GreenYellow
    {
      get
      {
        return new Color(4281335725U);
      }
    }

    public static Color Honeydew
    {
      get
      {
        return new Color(4293984240U);
      }
    }

    public static Color HotPink
    {
      get
      {
        return new Color(4290013695U);
      }
    }

    public static Color IndianRed
    {
      get
      {
        return new Color(4284243149U);
      }
    }

    public static Color Indigo
    {
      get
      {
        return new Color(4286709835U);
      }
    }

    public static Color Ivory
    {
      get
      {
        return new Color(4293984255U);
      }
    }

    public static Color Khaki
    {
      get
      {
        return new Color(4287424240U);
      }
    }

    public static Color Lavender
    {
      get
      {
        return new Color(4294633190U);
      }
    }

    public static Color LavenderBlush
    {
      get
      {
        return new Color(4294308095U);
      }
    }

    public static Color LawnGreen
    {
      get
      {
        return new Color(4278254716U);
      }
    }

    public static Color LemonChiffon
    {
      get
      {
        return new Color(4291689215U);
      }
    }

    public static Color LightBlue
    {
      get
      {
        return new Color(4293318829U);
      }
    }

    public static Color LightCoral
    {
      get
      {
        return new Color(4286611696U);
      }
    }

    public static Color LightCyan
    {
      get
      {
        return new Color(4294967264U);
      }
    }

    public static Color LightGoldenrodYellow
    {
      get
      {
        return new Color(4292016890U);
      }
    }

    public static Color LightGray
    {
      get
      {
        return new Color(4292072403U);
      }
    }

    public static Color LightGreen
    {
      get
      {
        return new Color(4287688336U);
      }
    }

    public static Color LightPink
    {
      get
      {
        return new Color(4290885375U);
      }
    }

    public static Color LightSalmon
    {
      get
      {
        return new Color(4286226687U);
      }
    }

    public static Color LightSeaGreen
    {
      get
      {
        return new Color(4289376800U);
      }
    }

    public static Color LightSkyBlue
    {
      get
      {
        return new Color(4294626951U);
      }
    }

    public static Color LightSlateGray
    {
      get
      {
        return new Color(4288252023U);
      }
    }

    public static Color LightSteelBlue
    {
      get
      {
        return new Color(4292789424U);
      }
    }

    public static Color LightYellow
    {
      get
      {
        return new Color(4292935679U);
      }
    }

    public static Color Lime
    {
      get
      {
        return new Color(4278255360U);
      }
    }

    public static Color LimeGreen
    {
      get
      {
        return new Color(4281519410U);
      }
    }

    public static Color Linen
    {
      get
      {
        return new Color(4293325050U);
      }
    }

    public static Color Magenta
    {
      get
      {
        return new Color(4294902015U);
      }
    }

    public static Color Maroon
    {
      get
      {
        return new Color(4278190208U);
      }
    }

    public static Color MediumAquamarine
    {
      get
      {
        return new Color(4289383782U);
      }
    }

    public static Color MediumBlue
    {
      get
      {
        return new Color(4291624960U);
      }
    }

    public static Color MediumOrchid
    {
      get
      {
        return new Color(4292040122U);
      }
    }

    public static Color MediumPurple
    {
      get
      {
        return new Color(4292571283U);
      }
    }

    public static Color MediumSeaGreen
    {
      get
      {
        return new Color(4285641532U);
      }
    }

    public static Color MediumSlateBlue
    {
      get
      {
        return new Color(4293814395U);
      }
    }

    public static Color MediumSpringGreen
    {
      get
      {
        return new Color(4288346624U);
      }
    }

    public static Color MediumTurquoise
    {
      get
      {
        return new Color(4291613000U);
      }
    }

    public static Color MediumVioletRed
    {
      get
      {
        return new Color(4286911943U);
      }
    }

    public static Color MidnightBlue
    {
      get
      {
        return new Color(4285536537U);
      }
    }

    public static Color MintCream
    {
      get
      {
        return new Color(4294639605U);
      }
    }

    public static Color MistyRose
    {
      get
      {
        return new Color(4292994303U);
      }
    }

    public static Color Moccasin
    {
      get
      {
        return new Color(4290110719U);
      }
    }

    public static Color NavajoWhite
    {
      get
      {
        return new Color(4289584895U);
      }
    }

    public static Color Navy
    {
      get
      {
        return new Color(4286578688U);
      }
    }

    public static Color OldLace
    {
      get
      {
        return new Color(4293326333U);
      }
    }

    public static Color Olive
    {
      get
      {
        return new Color(4278222976U);
      }
    }

    public static Color OliveDrab
    {
      get
      {
        return new Color(4280520299U);
      }
    }

    public static Color Orange
    {
      get
      {
        return new Color(4278232575U);
      }
    }

    public static Color OrangeRed
    {
      get
      {
        return new Color(4278207999U);
      }
    }

    public static Color Orchid
    {
      get
      {
        return new Color(4292243674U);
      }
    }

    public static Color PaleGoldenrod
    {
      get
      {
        return new Color(4289390830U);
      }
    }

    public static Color PaleGreen
    {
      get
      {
        return new Color(4288215960U);
      }
    }

    public static Color PaleTurquoise
    {
      get
      {
        return new Color(4293848751U);
      }
    }

    public static Color PaleVioletRed
    {
      get
      {
        return new Color(4287852763U);
      }
    }

    public static Color PapayaWhip
    {
      get
      {
        return new Color(4292210687U);
      }
    }

    public static Color PeachPuff
    {
      get
      {
        return new Color(4290370303U);
      }
    }

    public static Color Peru
    {
      get
      {
        return new Color(4282353101U);
      }
    }

    public static Color Pink
    {
      get
      {
        return new Color(4291543295U);
      }
    }

    public static Color Plum
    {
      get
      {
        return new Color(4292714717U);
      }
    }

    public static Color PowderBlue
    {
      get
      {
        return new Color(4293320880U);
      }
    }

    public static Color Purple
    {
      get
      {
        return new Color(4286578816U);
      }
    }

    public static Color Red
    {
      get
      {
        return new Color(4278190335U);
      }
    }

    public static Color RosyBrown
    {
      get
      {
        return new Color(4287598524U);
      }
    }

    public static Color RoyalBlue
    {
      get
      {
        return new Color(4292962625U);
      }
    }

    public static Color SaddleBrown
    {
      get
      {
        return new Color(4279453067U);
      }
    }

    public static Color Salmon
    {
      get
      {
        return new Color(4285694202U);
      }
    }

    public static Color SandyBrown
    {
      get
      {
        return new Color(4284523764U);
      }
    }

    public static Color SeaGreen
    {
      get
      {
        return new Color(4283927342U);
      }
    }

    public static Color SeaShell
    {
      get
      {
        return new Color(4293850623U);
      }
    }

    public static Color Sienna
    {
      get
      {
        return new Color(4281160352U);
      }
    }

    public static Color Silver
    {
      get
      {
        return new Color(4290822336U);
      }
    }

    public static Color SkyBlue
    {
      get
      {
        return new Color(4293643911U);
      }
    }

    public static Color SlateBlue
    {
      get
      {
        return new Color(4291648106U);
      }
    }

    public static Color SlateGray
    {
      get
      {
        return new Color(4287660144U);
      }
    }

    public static Color Snow
    {
      get
      {
        return new Color(4294638335U);
      }
    }

    public static Color SpringGreen
    {
      get
      {
        return new Color(4286578432U);
      }
    }

    public static Color SteelBlue
    {
      get
      {
        return new Color(4290019910U);
      }
    }

    public static Color Tan
    {
      get
      {
        return new Color(4287411410U);
      }
    }

    public static Color Teal
    {
      get
      {
        return new Color(4286611456U);
      }
    }

    public static Color Thistle
    {
      get
      {
        return new Color(4292394968U);
      }
    }

    public static Color Tomato
    {
      get
      {
        return new Color(4282868735U);
      }
    }

    public static Color Turquoise
    {
      get
      {
        return new Color(4291878976U);
      }
    }

    public static Color Violet
    {
      get
      {
        return new Color(4293821166U);
      }
    }

    public static Color Wheat
    {
      get
      {
        return new Color(4289978101U);
      }
    }

    public static Color White
    {
      get
      {
        return new Color(uint.MaxValue);
      }
    }

    public static Color WhiteSmoke
    {
      get
      {
        return new Color(4294309365U);
      }
    }

    public static Color Yellow
    {
      get
      {
        return new Color(4278255615U);
      }
    }

    public static Color YellowGreen
    {
      get
      {
        return new Color(4281519514U);
      }
    }

    public uint PackedValue
    {
      get
      {
        return this._packedValue;
      }
      set
      {
        this._packedValue = value;
      }
    }

    private Color(uint packedValue)
    {
      this._packedValue = packedValue;
    }

    public Color(Vector4 color)
    {
      this._packedValue = 0U;
      this.R = (byte) MathHelper.Clamp(color.X * (float) byte.MaxValue, 0.0f, (float) byte.MaxValue);
      this.G = (byte) MathHelper.Clamp(color.Y * (float) byte.MaxValue, 0.0f, (float) byte.MaxValue);
      this.B = (byte) MathHelper.Clamp(color.Z * (float) byte.MaxValue, 0.0f, (float) byte.MaxValue);
      this.A = (byte) MathHelper.Clamp(color.W * (float) byte.MaxValue, 0.0f, (float) byte.MaxValue);
    }

    public Color(Vector3 color)
    {
      this._packedValue = 0U;
      this.R = (byte) MathHelper.Clamp(color.X * (float) byte.MaxValue, 0.0f, (float) byte.MaxValue);
      this.G = (byte) MathHelper.Clamp(color.Y * (float) byte.MaxValue, 0.0f, (float) byte.MaxValue);
      this.B = (byte) MathHelper.Clamp(color.Z * (float) byte.MaxValue, 0.0f, (float) byte.MaxValue);
      this.A = byte.MaxValue;
    }

    public Color(Color color, int alpha)
    {
      this._packedValue = 0U;
      this.R = color.R;
      this.G = color.G;
      this.B = color.B;
      this.A = (byte) MathHelper.Clamp(alpha, 0, (int) byte.MaxValue);
    }

    public Color(Color color, float alpha)
    {
      this._packedValue = 0U;
      this.R = color.R;
      this.G = color.G;
      this.B = color.B;
      this.A = (byte) MathHelper.Clamp(alpha * (float) byte.MaxValue, 0.0f, (float) byte.MaxValue);
    }

    public Color(float r, float g, float b)
    {
      this._packedValue = 0U;
      this.R = (byte) MathHelper.Clamp(r * (float) byte.MaxValue, 0.0f, (float) byte.MaxValue);
      this.G = (byte) MathHelper.Clamp(g * (float) byte.MaxValue, 0.0f, (float) byte.MaxValue);
      this.B = (byte) MathHelper.Clamp(b * (float) byte.MaxValue, 0.0f, (float) byte.MaxValue);
      this.A = byte.MaxValue;
    }

    public Color(int r, int g, int b)
    {
      this._packedValue = 0U;
      this.R = (byte) MathHelper.Clamp(r, 0, (int) byte.MaxValue);
      this.G = (byte) MathHelper.Clamp(g, 0, (int) byte.MaxValue);
      this.B = (byte) MathHelper.Clamp(b, 0, (int) byte.MaxValue);
      this.A = byte.MaxValue;
    }

    public Color(int r, int g, int b, int alpha)
    {
      this._packedValue = 0U;
      this.R = (byte) MathHelper.Clamp(r, 0, (int) byte.MaxValue);
      this.G = (byte) MathHelper.Clamp(g, 0, (int) byte.MaxValue);
      this.B = (byte) MathHelper.Clamp(b, 0, (int) byte.MaxValue);
      this.A = (byte) MathHelper.Clamp(alpha, 0, (int) byte.MaxValue);
    }

    public Color(float r, float g, float b, float alpha)
    {
      this._packedValue = 0U;
      this.R = (byte) MathHelper.Clamp(r * (float) byte.MaxValue, 0.0f, (float) byte.MaxValue);
      this.G = (byte) MathHelper.Clamp(g * (float) byte.MaxValue, 0.0f, (float) byte.MaxValue);
      this.B = (byte) MathHelper.Clamp(b * (float) byte.MaxValue, 0.0f, (float) byte.MaxValue);
      this.A = (byte) MathHelper.Clamp(alpha * (float) byte.MaxValue, 0.0f, (float) byte.MaxValue);
    }

    public static bool operator ==(Color a, Color b)
    {
      if ((int) a.A == (int) b.A && (int) a.R == (int) b.R && (int) a.G == (int) b.G)
        return (int) a.B == (int) b.B;
      else
        return false;
    }

    public static bool operator !=(Color a, Color b)
    {
      return !(a == b);
    }

    public static Color operator *(Color value, float scale)
    {
      return Color.Multiply(value, scale);
    }

    public override int GetHashCode()
    {
      return this._packedValue.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      if (obj is Color)
        return this.Equals((Color) obj);
      else
        return false;
    }

    public static Color Lerp(Color value1, Color value2, float amount)
    {
      return new Color((int) (byte) MathHelper.Clamp(MathHelper.Lerp((float) value1.R, (float) value2.R, amount), 0.0f, (float) byte.MaxValue), (int) (byte) MathHelper.Clamp(MathHelper.Lerp((float) value1.G, (float) value2.G, amount), 0.0f, (float) byte.MaxValue), (int) (byte) MathHelper.Clamp(MathHelper.Lerp((float) value1.B, (float) value2.B, amount), 0.0f, (float) byte.MaxValue), (int) (byte) MathHelper.Clamp(MathHelper.Lerp((float) value1.A, (float) value2.A, amount), 0.0f, (float) byte.MaxValue));
    }

    public static Color Multiply(Color value, float scale)
    {
      return new Color((int) (byte) MathHelper.Clamp((float) value.R * scale, 0.0f, (float) byte.MaxValue), (int) (byte) MathHelper.Clamp((float) value.G * scale, 0.0f, (float) byte.MaxValue), (int) (byte) MathHelper.Clamp((float) value.B * scale, 0.0f, (float) byte.MaxValue), (int) (byte) MathHelper.Clamp((float) value.A * scale, 0.0f, (float) byte.MaxValue));
    }

    public Vector3 ToVector3()
    {
      return new Vector3((float) this.R / (float) byte.MaxValue, (float) this.G / (float) byte.MaxValue, (float) this.B / (float) byte.MaxValue);
    }

    public Vector4 ToVector4()
    {
      return new Vector4((float) this.R / (float) byte.MaxValue, (float) this.G / (float) byte.MaxValue, (float) this.B / (float) byte.MaxValue, (float) this.A / (float) byte.MaxValue);
    }

    public override string ToString()
    {
      return string.Format("[Color: R={0}, G={1}, B={2}, A={3}, PackedValue={4}]", (object) this.R, (object) this.G, (object) this.B, (object) this.A, (object) this.PackedValue);
    }

    public static Color FromNonPremultiplied(Vector4 vector)
    {
      return new Color(vector.X * vector.W, vector.Y * vector.W, vector.Z * vector.W, vector.W);
    }

    public static Color FromNonPremultiplied(int r, int g, int b, int a)
    {
      return new Color((int) (byte) (r * a / (int) byte.MaxValue), (int) (byte) (g * a / (int) byte.MaxValue), (int) (byte) (b * a / (int) byte.MaxValue), a);
    }

    public bool Equals(Color other)
    {
      return (int) this.PackedValue == (int) other.PackedValue;
    }
  }
}
