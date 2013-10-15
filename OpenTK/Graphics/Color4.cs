// Type: OpenTK.Graphics.Color4
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using System;
using System.Drawing;

namespace OpenTK.Graphics
{
  [Serializable]
  public struct Color4 : IEquatable<Color4>
  {
    public float R;
    public float G;
    public float B;
    public float A;

    public static Color4 Transparent
    {
      get
      {
        return new Color4(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte) 0);
      }
    }

    public static Color4 AliceBlue
    {
      get
      {
        return new Color4((byte) 240, (byte) 248, byte.MaxValue, byte.MaxValue);
      }
    }

    public static Color4 AntiqueWhite
    {
      get
      {
        return new Color4((byte) 250, (byte) 235, (byte) 215, byte.MaxValue);
      }
    }

    public static Color4 Aqua
    {
      get
      {
        return new Color4((byte) 0, byte.MaxValue, byte.MaxValue, byte.MaxValue);
      }
    }

    public static Color4 Aquamarine
    {
      get
      {
        return new Color4((byte) 127, byte.MaxValue, (byte) 212, byte.MaxValue);
      }
    }

    public static Color4 Azure
    {
      get
      {
        return new Color4((byte) 240, byte.MaxValue, byte.MaxValue, byte.MaxValue);
      }
    }

    public static Color4 Beige
    {
      get
      {
        return new Color4((byte) 245, (byte) 245, (byte) 220, byte.MaxValue);
      }
    }

    public static Color4 Bisque
    {
      get
      {
        return new Color4(byte.MaxValue, (byte) 228, (byte) 196, byte.MaxValue);
      }
    }

    public static Color4 Black
    {
      get
      {
        return new Color4((byte) 0, (byte) 0, (byte) 0, byte.MaxValue);
      }
    }

    public static Color4 BlanchedAlmond
    {
      get
      {
        return new Color4(byte.MaxValue, (byte) 235, (byte) 205, byte.MaxValue);
      }
    }

    public static Color4 Blue
    {
      get
      {
        return new Color4((byte) 0, (byte) 0, byte.MaxValue, byte.MaxValue);
      }
    }

    public static Color4 BlueViolet
    {
      get
      {
        return new Color4((byte) 138, (byte) 43, (byte) 226, byte.MaxValue);
      }
    }

    public static Color4 Brown
    {
      get
      {
        return new Color4((byte) 165, (byte) 42, (byte) 42, byte.MaxValue);
      }
    }

    public static Color4 BurlyWood
    {
      get
      {
        return new Color4((byte) 222, (byte) 184, (byte) 135, byte.MaxValue);
      }
    }

    public static Color4 CadetBlue
    {
      get
      {
        return new Color4((byte) 95, (byte) 158, (byte) 160, byte.MaxValue);
      }
    }

    public static Color4 Chartreuse
    {
      get
      {
        return new Color4((byte) 127, byte.MaxValue, (byte) 0, byte.MaxValue);
      }
    }

    public static Color4 Chocolate
    {
      get
      {
        return new Color4((byte) 210, (byte) 105, (byte) 30, byte.MaxValue);
      }
    }

    public static Color4 Coral
    {
      get
      {
        return new Color4(byte.MaxValue, (byte) 127, (byte) 80, byte.MaxValue);
      }
    }

    public static Color4 CornflowerBlue
    {
      get
      {
        return new Color4((byte) 100, (byte) 149, (byte) 237, byte.MaxValue);
      }
    }

    public static Color4 Cornsilk
    {
      get
      {
        return new Color4(byte.MaxValue, (byte) 248, (byte) 220, byte.MaxValue);
      }
    }

    public static Color4 Crimson
    {
      get
      {
        return new Color4((byte) 220, (byte) 20, (byte) 60, byte.MaxValue);
      }
    }

    public static Color4 Cyan
    {
      get
      {
        return new Color4((byte) 0, byte.MaxValue, byte.MaxValue, byte.MaxValue);
      }
    }

    public static Color4 DarkBlue
    {
      get
      {
        return new Color4((byte) 0, (byte) 0, (byte) 139, byte.MaxValue);
      }
    }

    public static Color4 DarkCyan
    {
      get
      {
        return new Color4((byte) 0, (byte) 139, (byte) 139, byte.MaxValue);
      }
    }

    public static Color4 DarkGoldenrod
    {
      get
      {
        return new Color4((byte) 184, (byte) 134, (byte) 11, byte.MaxValue);
      }
    }

    public static Color4 DarkGray
    {
      get
      {
        return new Color4((byte) 169, (byte) 169, (byte) 169, byte.MaxValue);
      }
    }

    public static Color4 DarkGreen
    {
      get
      {
        return new Color4((byte) 0, (byte) 100, (byte) 0, byte.MaxValue);
      }
    }

    public static Color4 DarkKhaki
    {
      get
      {
        return new Color4((byte) 189, (byte) 183, (byte) 107, byte.MaxValue);
      }
    }

    public static Color4 DarkMagenta
    {
      get
      {
        return new Color4((byte) 139, (byte) 0, (byte) 139, byte.MaxValue);
      }
    }

    public static Color4 DarkOliveGreen
    {
      get
      {
        return new Color4((byte) 85, (byte) 107, (byte) 47, byte.MaxValue);
      }
    }

    public static Color4 DarkOrange
    {
      get
      {
        return new Color4(byte.MaxValue, (byte) 140, (byte) 0, byte.MaxValue);
      }
    }

    public static Color4 DarkOrchid
    {
      get
      {
        return new Color4((byte) 153, (byte) 50, (byte) 204, byte.MaxValue);
      }
    }

    public static Color4 DarkRed
    {
      get
      {
        return new Color4((byte) 139, (byte) 0, (byte) 0, byte.MaxValue);
      }
    }

    public static Color4 DarkSalmon
    {
      get
      {
        return new Color4((byte) 233, (byte) 150, (byte) 122, byte.MaxValue);
      }
    }

    public static Color4 DarkSeaGreen
    {
      get
      {
        return new Color4((byte) 143, (byte) 188, (byte) 139, byte.MaxValue);
      }
    }

    public static Color4 DarkSlateBlue
    {
      get
      {
        return new Color4((byte) 72, (byte) 61, (byte) 139, byte.MaxValue);
      }
    }

    public static Color4 DarkSlateGray
    {
      get
      {
        return new Color4((byte) 47, (byte) 79, (byte) 79, byte.MaxValue);
      }
    }

    public static Color4 DarkTurquoise
    {
      get
      {
        return new Color4((byte) 0, (byte) 206, (byte) 209, byte.MaxValue);
      }
    }

    public static Color4 DarkViolet
    {
      get
      {
        return new Color4((byte) 148, (byte) 0, (byte) 211, byte.MaxValue);
      }
    }

    public static Color4 DeepPink
    {
      get
      {
        return new Color4(byte.MaxValue, (byte) 20, (byte) 147, byte.MaxValue);
      }
    }

    public static Color4 DeepSkyBlue
    {
      get
      {
        return new Color4((byte) 0, (byte) 191, byte.MaxValue, byte.MaxValue);
      }
    }

    public static Color4 DimGray
    {
      get
      {
        return new Color4((byte) 105, (byte) 105, (byte) 105, byte.MaxValue);
      }
    }

    public static Color4 DodgerBlue
    {
      get
      {
        return new Color4((byte) 30, (byte) 144, byte.MaxValue, byte.MaxValue);
      }
    }

    public static Color4 Firebrick
    {
      get
      {
        return new Color4((byte) 178, (byte) 34, (byte) 34, byte.MaxValue);
      }
    }

    public static Color4 FloralWhite
    {
      get
      {
        return new Color4(byte.MaxValue, (byte) 250, (byte) 240, byte.MaxValue);
      }
    }

    public static Color4 ForestGreen
    {
      get
      {
        return new Color4((byte) 34, (byte) 139, (byte) 34, byte.MaxValue);
      }
    }

    public static Color4 Fuchsia
    {
      get
      {
        return new Color4(byte.MaxValue, (byte) 0, byte.MaxValue, byte.MaxValue);
      }
    }

    public static Color4 Gainsboro
    {
      get
      {
        return new Color4((byte) 220, (byte) 220, (byte) 220, byte.MaxValue);
      }
    }

    public static Color4 GhostWhite
    {
      get
      {
        return new Color4((byte) 248, (byte) 248, byte.MaxValue, byte.MaxValue);
      }
    }

    public static Color4 Gold
    {
      get
      {
        return new Color4(byte.MaxValue, (byte) 215, (byte) 0, byte.MaxValue);
      }
    }

    public static Color4 Goldenrod
    {
      get
      {
        return new Color4((byte) 218, (byte) 165, (byte) 32, byte.MaxValue);
      }
    }

    public static Color4 Gray
    {
      get
      {
        return new Color4((byte) sbyte.MinValue, (byte) sbyte.MinValue, (byte) sbyte.MinValue, byte.MaxValue);
      }
    }

    public static Color4 Green
    {
      get
      {
        return new Color4((byte) 0, (byte) sbyte.MinValue, (byte) 0, byte.MaxValue);
      }
    }

    public static Color4 GreenYellow
    {
      get
      {
        return new Color4((byte) 173, byte.MaxValue, (byte) 47, byte.MaxValue);
      }
    }

    public static Color4 Honeydew
    {
      get
      {
        return new Color4((byte) 240, byte.MaxValue, (byte) 240, byte.MaxValue);
      }
    }

    public static Color4 HotPink
    {
      get
      {
        return new Color4(byte.MaxValue, (byte) 105, (byte) 180, byte.MaxValue);
      }
    }

    public static Color4 IndianRed
    {
      get
      {
        return new Color4((byte) 205, (byte) 92, (byte) 92, byte.MaxValue);
      }
    }

    public static Color4 Indigo
    {
      get
      {
        return new Color4((byte) 75, (byte) 0, (byte) 130, byte.MaxValue);
      }
    }

    public static Color4 Ivory
    {
      get
      {
        return new Color4(byte.MaxValue, byte.MaxValue, (byte) 240, byte.MaxValue);
      }
    }

    public static Color4 Khaki
    {
      get
      {
        return new Color4((byte) 240, (byte) 230, (byte) 140, byte.MaxValue);
      }
    }

    public static Color4 Lavender
    {
      get
      {
        return new Color4((byte) 230, (byte) 230, (byte) 250, byte.MaxValue);
      }
    }

    public static Color4 LavenderBlush
    {
      get
      {
        return new Color4(byte.MaxValue, (byte) 240, (byte) 245, byte.MaxValue);
      }
    }

    public static Color4 LawnGreen
    {
      get
      {
        return new Color4((byte) 124, (byte) 252, (byte) 0, byte.MaxValue);
      }
    }

    public static Color4 LemonChiffon
    {
      get
      {
        return new Color4(byte.MaxValue, (byte) 250, (byte) 205, byte.MaxValue);
      }
    }

    public static Color4 LightBlue
    {
      get
      {
        return new Color4((byte) 173, (byte) 216, (byte) 230, byte.MaxValue);
      }
    }

    public static Color4 LightCoral
    {
      get
      {
        return new Color4((byte) 240, (byte) sbyte.MinValue, (byte) sbyte.MinValue, byte.MaxValue);
      }
    }

    public static Color4 LightCyan
    {
      get
      {
        return new Color4((byte) 224, byte.MaxValue, byte.MaxValue, byte.MaxValue);
      }
    }

    public static Color4 LightGoldenrodYellow
    {
      get
      {
        return new Color4((byte) 250, (byte) 250, (byte) 210, byte.MaxValue);
      }
    }

    public static Color4 LightGreen
    {
      get
      {
        return new Color4((byte) 144, (byte) 238, (byte) 144, byte.MaxValue);
      }
    }

    public static Color4 LightGray
    {
      get
      {
        return new Color4((byte) 211, (byte) 211, (byte) 211, byte.MaxValue);
      }
    }

    public static Color4 LightPink
    {
      get
      {
        return new Color4(byte.MaxValue, (byte) 182, (byte) 193, byte.MaxValue);
      }
    }

    public static Color4 LightSalmon
    {
      get
      {
        return new Color4(byte.MaxValue, (byte) 160, (byte) 122, byte.MaxValue);
      }
    }

    public static Color4 LightSeaGreen
    {
      get
      {
        return new Color4((byte) 32, (byte) 178, (byte) 170, byte.MaxValue);
      }
    }

    public static Color4 LightSkyBlue
    {
      get
      {
        return new Color4((byte) 135, (byte) 206, (byte) 250, byte.MaxValue);
      }
    }

    public static Color4 LightSlateGray
    {
      get
      {
        return new Color4((byte) 119, (byte) 136, (byte) 153, byte.MaxValue);
      }
    }

    public static Color4 LightSteelBlue
    {
      get
      {
        return new Color4((byte) 176, (byte) 196, (byte) 222, byte.MaxValue);
      }
    }

    public static Color4 LightYellow
    {
      get
      {
        return new Color4(byte.MaxValue, byte.MaxValue, (byte) 224, byte.MaxValue);
      }
    }

    public static Color4 Lime
    {
      get
      {
        return new Color4((byte) 0, byte.MaxValue, (byte) 0, byte.MaxValue);
      }
    }

    public static Color4 LimeGreen
    {
      get
      {
        return new Color4((byte) 50, (byte) 205, (byte) 50, byte.MaxValue);
      }
    }

    public static Color4 Linen
    {
      get
      {
        return new Color4((byte) 250, (byte) 240, (byte) 230, byte.MaxValue);
      }
    }

    public static Color4 Magenta
    {
      get
      {
        return new Color4(byte.MaxValue, (byte) 0, byte.MaxValue, byte.MaxValue);
      }
    }

    public static Color4 Maroon
    {
      get
      {
        return new Color4((byte) sbyte.MinValue, (byte) 0, (byte) 0, byte.MaxValue);
      }
    }

    public static Color4 MediumAquamarine
    {
      get
      {
        return new Color4((byte) 102, (byte) 205, (byte) 170, byte.MaxValue);
      }
    }

    public static Color4 MediumBlue
    {
      get
      {
        return new Color4((byte) 0, (byte) 0, (byte) 205, byte.MaxValue);
      }
    }

    public static Color4 MediumOrchid
    {
      get
      {
        return new Color4((byte) 186, (byte) 85, (byte) 211, byte.MaxValue);
      }
    }

    public static Color4 MediumPurple
    {
      get
      {
        return new Color4((byte) 147, (byte) 112, (byte) 219, byte.MaxValue);
      }
    }

    public static Color4 MediumSeaGreen
    {
      get
      {
        return new Color4((byte) 60, (byte) 179, (byte) 113, byte.MaxValue);
      }
    }

    public static Color4 MediumSlateBlue
    {
      get
      {
        return new Color4((byte) 123, (byte) 104, (byte) 238, byte.MaxValue);
      }
    }

    public static Color4 MediumSpringGreen
    {
      get
      {
        return new Color4((byte) 0, (byte) 250, (byte) 154, byte.MaxValue);
      }
    }

    public static Color4 MediumTurquoise
    {
      get
      {
        return new Color4((byte) 72, (byte) 209, (byte) 204, byte.MaxValue);
      }
    }

    public static Color4 MediumVioletRed
    {
      get
      {
        return new Color4((byte) 199, (byte) 21, (byte) 133, byte.MaxValue);
      }
    }

    public static Color4 MidnightBlue
    {
      get
      {
        return new Color4((byte) 25, (byte) 25, (byte) 112, byte.MaxValue);
      }
    }

    public static Color4 MintCream
    {
      get
      {
        return new Color4((byte) 245, byte.MaxValue, (byte) 250, byte.MaxValue);
      }
    }

    public static Color4 MistyRose
    {
      get
      {
        return new Color4(byte.MaxValue, (byte) 228, (byte) 225, byte.MaxValue);
      }
    }

    public static Color4 Moccasin
    {
      get
      {
        return new Color4(byte.MaxValue, (byte) 228, (byte) 181, byte.MaxValue);
      }
    }

    public static Color4 NavajoWhite
    {
      get
      {
        return new Color4(byte.MaxValue, (byte) 222, (byte) 173, byte.MaxValue);
      }
    }

    public static Color4 Navy
    {
      get
      {
        return new Color4((byte) 0, (byte) 0, (byte) sbyte.MinValue, byte.MaxValue);
      }
    }

    public static Color4 OldLace
    {
      get
      {
        return new Color4((byte) 253, (byte) 245, (byte) 230, byte.MaxValue);
      }
    }

    public static Color4 Olive
    {
      get
      {
        return new Color4((byte) sbyte.MinValue, (byte) sbyte.MinValue, (byte) 0, byte.MaxValue);
      }
    }

    public static Color4 OliveDrab
    {
      get
      {
        return new Color4((byte) 107, (byte) 142, (byte) 35, byte.MaxValue);
      }
    }

    public static Color4 Orange
    {
      get
      {
        return new Color4(byte.MaxValue, (byte) 165, (byte) 0, byte.MaxValue);
      }
    }

    public static Color4 OrangeRed
    {
      get
      {
        return new Color4(byte.MaxValue, (byte) 69, (byte) 0, byte.MaxValue);
      }
    }

    public static Color4 Orchid
    {
      get
      {
        return new Color4((byte) 218, (byte) 112, (byte) 214, byte.MaxValue);
      }
    }

    public static Color4 PaleGoldenrod
    {
      get
      {
        return new Color4((byte) 238, (byte) 232, (byte) 170, byte.MaxValue);
      }
    }

    public static Color4 PaleGreen
    {
      get
      {
        return new Color4((byte) 152, (byte) 251, (byte) 152, byte.MaxValue);
      }
    }

    public static Color4 PaleTurquoise
    {
      get
      {
        return new Color4((byte) 175, (byte) 238, (byte) 238, byte.MaxValue);
      }
    }

    public static Color4 PaleVioletRed
    {
      get
      {
        return new Color4((byte) 219, (byte) 112, (byte) 147, byte.MaxValue);
      }
    }

    public static Color4 PapayaWhip
    {
      get
      {
        return new Color4(byte.MaxValue, (byte) 239, (byte) 213, byte.MaxValue);
      }
    }

    public static Color4 PeachPuff
    {
      get
      {
        return new Color4(byte.MaxValue, (byte) 218, (byte) 185, byte.MaxValue);
      }
    }

    public static Color4 Peru
    {
      get
      {
        return new Color4((byte) 205, (byte) 133, (byte) 63, byte.MaxValue);
      }
    }

    public static Color4 Pink
    {
      get
      {
        return new Color4(byte.MaxValue, (byte) 192, (byte) 203, byte.MaxValue);
      }
    }

    public static Color4 Plum
    {
      get
      {
        return new Color4((byte) 221, (byte) 160, (byte) 221, byte.MaxValue);
      }
    }

    public static Color4 PowderBlue
    {
      get
      {
        return new Color4((byte) 176, (byte) 224, (byte) 230, byte.MaxValue);
      }
    }

    public static Color4 Purple
    {
      get
      {
        return new Color4((byte) sbyte.MinValue, (byte) 0, (byte) sbyte.MinValue, byte.MaxValue);
      }
    }

    public static Color4 Red
    {
      get
      {
        return new Color4(byte.MaxValue, (byte) 0, (byte) 0, byte.MaxValue);
      }
    }

    public static Color4 RosyBrown
    {
      get
      {
        return new Color4((byte) 188, (byte) 143, (byte) 143, byte.MaxValue);
      }
    }

    public static Color4 RoyalBlue
    {
      get
      {
        return new Color4((byte) 65, (byte) 105, (byte) 225, byte.MaxValue);
      }
    }

    public static Color4 SaddleBrown
    {
      get
      {
        return new Color4((byte) 139, (byte) 69, (byte) 19, byte.MaxValue);
      }
    }

    public static Color4 Salmon
    {
      get
      {
        return new Color4((byte) 250, (byte) sbyte.MinValue, (byte) 114, byte.MaxValue);
      }
    }

    public static Color4 SandyBrown
    {
      get
      {
        return new Color4((byte) 244, (byte) 164, (byte) 96, byte.MaxValue);
      }
    }

    public static Color4 SeaGreen
    {
      get
      {
        return new Color4((byte) 46, (byte) 139, (byte) 87, byte.MaxValue);
      }
    }

    public static Color4 SeaShell
    {
      get
      {
        return new Color4(byte.MaxValue, (byte) 245, (byte) 238, byte.MaxValue);
      }
    }

    public static Color4 Sienna
    {
      get
      {
        return new Color4((byte) 160, (byte) 82, (byte) 45, byte.MaxValue);
      }
    }

    public static Color4 Silver
    {
      get
      {
        return new Color4((byte) 192, (byte) 192, (byte) 192, byte.MaxValue);
      }
    }

    public static Color4 SkyBlue
    {
      get
      {
        return new Color4((byte) 135, (byte) 206, (byte) 235, byte.MaxValue);
      }
    }

    public static Color4 SlateBlue
    {
      get
      {
        return new Color4((byte) 106, (byte) 90, (byte) 205, byte.MaxValue);
      }
    }

    public static Color4 SlateGray
    {
      get
      {
        return new Color4((byte) 112, (byte) sbyte.MinValue, (byte) 144, byte.MaxValue);
      }
    }

    public static Color4 Snow
    {
      get
      {
        return new Color4(byte.MaxValue, (byte) 250, (byte) 250, byte.MaxValue);
      }
    }

    public static Color4 SpringGreen
    {
      get
      {
        return new Color4((byte) 0, byte.MaxValue, (byte) 127, byte.MaxValue);
      }
    }

    public static Color4 SteelBlue
    {
      get
      {
        return new Color4((byte) 70, (byte) 130, (byte) 180, byte.MaxValue);
      }
    }

    public static Color4 Tan
    {
      get
      {
        return new Color4((byte) 210, (byte) 180, (byte) 140, byte.MaxValue);
      }
    }

    public static Color4 Teal
    {
      get
      {
        return new Color4((byte) 0, (byte) sbyte.MinValue, (byte) sbyte.MinValue, byte.MaxValue);
      }
    }

    public static Color4 Thistle
    {
      get
      {
        return new Color4((byte) 216, (byte) 191, (byte) 216, byte.MaxValue);
      }
    }

    public static Color4 Tomato
    {
      get
      {
        return new Color4(byte.MaxValue, (byte) 99, (byte) 71, byte.MaxValue);
      }
    }

    public static Color4 Turquoise
    {
      get
      {
        return new Color4((byte) 64, (byte) 224, (byte) 208, byte.MaxValue);
      }
    }

    public static Color4 Violet
    {
      get
      {
        return new Color4((byte) 238, (byte) 130, (byte) 238, byte.MaxValue);
      }
    }

    public static Color4 Wheat
    {
      get
      {
        return new Color4((byte) 245, (byte) 222, (byte) 179, byte.MaxValue);
      }
    }

    public static Color4 White
    {
      get
      {
        return new Color4(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
      }
    }

    public static Color4 WhiteSmoke
    {
      get
      {
        return new Color4((byte) 245, (byte) 245, (byte) 245, byte.MaxValue);
      }
    }

    public static Color4 Yellow
    {
      get
      {
        return new Color4(byte.MaxValue, byte.MaxValue, (byte) 0, byte.MaxValue);
      }
    }

    public static Color4 YellowGreen
    {
      get
      {
        return new Color4((byte) 154, (byte) 205, (byte) 50, byte.MaxValue);
      }
    }

    public Color4(float r, float g, float b, float a)
    {
      this.R = r;
      this.G = g;
      this.B = b;
      this.A = a;
    }

    public Color4(byte r, byte g, byte b, byte a)
    {
      this.R = (float) r / (float) byte.MaxValue;
      this.G = (float) g / (float) byte.MaxValue;
      this.B = (float) b / (float) byte.MaxValue;
      this.A = (float) a / (float) byte.MaxValue;
    }

    [Obsolete("Use new Color4(r, g, b, a) instead.")]
    public Color4(Color color)
    {
      this = new Color4(color.R, color.G, color.B, color.A);
    }

    public static implicit operator Color4(Color color)
    {
      return new Color4(color.R, color.G, color.B, color.A);
    }

    public static explicit operator Color(Color4 color)
    {
      return Color.FromArgb((int) ((double) color.A * (double) byte.MaxValue), (int) ((double) color.R * (double) byte.MaxValue), (int) ((double) color.G * (double) byte.MaxValue), (int) ((double) color.B * (double) byte.MaxValue));
    }

    public static bool operator ==(Color4 left, Color4 right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(Color4 left, Color4 right)
    {
      return !left.Equals(right);
    }

    public int ToArgb()
    {
      return (int) ((uint) ((int) (uint) ((double) this.A * (double) byte.MaxValue) << 24 | (int) (uint) ((double) this.R * (double) byte.MaxValue) << 16 | (int) (uint) ((double) this.G * (double) byte.MaxValue) << 8) | (uint) ((double) this.B * (double) byte.MaxValue));
    }

    public override bool Equals(object obj)
    {
      if (!(obj is Color4))
        return false;
      else
        return this.Equals((Color4) obj);
    }

    public override int GetHashCode()
    {
      return this.ToArgb();
    }

    public override string ToString()
    {
      return string.Format("{{(R, G, B, A) = ({0}, {1}, {2}, {3})}}", (object) this.R.ToString(), (object) this.G.ToString(), (object) this.B.ToString(), (object) this.A.ToString());
    }

    public bool Equals(Color4 other)
    {
      if ((double) this.R == (double) other.R && (double) this.G == (double) other.G && (double) this.B == (double) other.B)
        return (double) this.A == (double) other.A;
      else
        return false;
    }
  }
}
