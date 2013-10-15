// Type: Microsoft.Xna.Framework.Graphics.Localization.WordWrap
// Assembly: XnaWordWrapCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 278B215C-3D4B-45DC-8E40-65DA36A71393
// Assembly location: F:\Program Files (x86)\FEZ\XnaWordWrapCore.dll

using Microsoft.Xna.Framework.Graphics;
using System;

namespace Microsoft.Xna.Framework.Graphics.Localization
{
  public static class WordWrap
  {
    private static Prohibition prohibition = Prohibition.On;
    private static NoHangulWrap nohangul = NoHangulWrap.Off;
    private static BreakInfo[] BreakArray = new BreakInfo[146]
    {
      new BreakInfo(94U, true, true),
      new BreakInfo(33U, true, false),
      new BreakInfo(36U, false, true),
      new BreakInfo(37U, true, false),
      new BreakInfo(39U, true, true),
      new BreakInfo(40U, false, true),
      new BreakInfo(41U, true, false),
      new BreakInfo(44U, true, false),
      new BreakInfo(46U, true, false),
      new BreakInfo(47U, true, true),
      new BreakInfo(58U, true, false),
      new BreakInfo(59U, true, false),
      new BreakInfo(63U, true, false),
      new BreakInfo(91U, false, true),
      new BreakInfo(92U, false, true),
      new BreakInfo(93U, true, false),
      new BreakInfo(123U, false, true),
      new BreakInfo(125U, true, false),
      new BreakInfo(162U, true, false),
      new BreakInfo(163U, false, true),
      new BreakInfo(165U, false, true),
      new BreakInfo(167U, false, true),
      new BreakInfo(168U, true, false),
      new BreakInfo(169U, true, false),
      new BreakInfo(174U, true, false),
      new BreakInfo(176U, true, false),
      new BreakInfo(183U, true, true),
      new BreakInfo(711U, true, false),
      new BreakInfo(713U, true, false),
      new BreakInfo(8211U, true, false),
      new BreakInfo(8212U, true, false),
      new BreakInfo(8213U, true, false),
      new BreakInfo(8214U, true, false),
      new BreakInfo(8216U, false, true),
      new BreakInfo(8217U, true, false),
      new BreakInfo(8220U, false, true),
      new BreakInfo(8221U, true, false),
      new BreakInfo(8226U, true, false),
      new BreakInfo(8229U, true, false),
      new BreakInfo(8230U, true, false),
      new BreakInfo(8231U, true, false),
      new BreakInfo(8242U, true, false),
      new BreakInfo(8243U, true, false),
      new BreakInfo(8245U, false, true),
      new BreakInfo(8451U, true, false),
      new BreakInfo(8482U, true, false),
      new BreakInfo(8758U, true, false),
      new BreakInfo(9588U, true, false),
      new BreakInfo(9839U, false, true),
      new BreakInfo(12289U, true, false),
      new BreakInfo(12290U, true, false),
      new BreakInfo(12291U, true, false),
      new BreakInfo(12293U, true, false),
      new BreakInfo(12296U, false, true),
      new BreakInfo(12297U, true, false),
      new BreakInfo(12298U, false, true),
      new BreakInfo(12299U, true, false),
      new BreakInfo(12300U, false, true),
      new BreakInfo(12301U, true, false),
      new BreakInfo(12302U, false, true),
      new BreakInfo(12303U, true, false),
      new BreakInfo(12304U, false, true),
      new BreakInfo(12305U, true, false),
      new BreakInfo(12306U, false, true),
      new BreakInfo(12308U, false, true),
      new BreakInfo(12309U, true, false),
      new BreakInfo(12310U, false, true),
      new BreakInfo(12311U, true, false),
      new BreakInfo(12317U, false, true),
      new BreakInfo(12318U, true, false),
      new BreakInfo(12319U, true, false),
      new BreakInfo(12353U, true, false),
      new BreakInfo(12355U, true, false),
      new BreakInfo(12357U, true, false),
      new BreakInfo(12359U, true, false),
      new BreakInfo(12361U, true, false),
      new BreakInfo(12387U, true, false),
      new BreakInfo(12419U, true, false),
      new BreakInfo(12421U, true, false),
      new BreakInfo(12423U, true, false),
      new BreakInfo(12430U, true, false),
      new BreakInfo(12441U, true, false),
      new BreakInfo(12442U, true, false),
      new BreakInfo(12443U, true, false),
      new BreakInfo(12444U, true, false),
      new BreakInfo(12445U, true, false),
      new BreakInfo(12446U, true, false),
      new BreakInfo(12449U, true, false),
      new BreakInfo(12451U, true, false),
      new BreakInfo(12453U, true, false),
      new BreakInfo(12455U, true, false),
      new BreakInfo(12457U, true, false),
      new BreakInfo(12483U, true, false),
      new BreakInfo(12515U, true, false),
      new BreakInfo(12517U, true, false),
      new BreakInfo(12519U, true, false),
      new BreakInfo(12526U, true, false),
      new BreakInfo(12533U, true, false),
      new BreakInfo(12534U, true, false),
      new BreakInfo(12539U, true, false),
      new BreakInfo(12540U, true, false),
      new BreakInfo(12541U, true, false),
      new BreakInfo(12542U, true, false),
      new BreakInfo(65072U, true, false),
      new BreakInfo(65104U, true, false),
      new BreakInfo(65105U, true, false),
      new BreakInfo(65106U, true, false),
      new BreakInfo(65108U, true, false),
      new BreakInfo(65109U, true, false),
      new BreakInfo(65110U, true, false),
      new BreakInfo(65111U, true, false),
      new BreakInfo(65113U, false, true),
      new BreakInfo(65114U, true, false),
      new BreakInfo(65115U, false, true),
      new BreakInfo(65116U, true, false),
      new BreakInfo(65117U, false, true),
      new BreakInfo(65118U, true, false),
      new BreakInfo(65281U, true, false),
      new BreakInfo(65282U, true, false),
      new BreakInfo(65284U, false, true),
      new BreakInfo(65285U, true, false),
      new BreakInfo(65287U, true, false),
      new BreakInfo(65288U, false, true),
      new BreakInfo(65289U, true, false),
      new BreakInfo(65292U, true, false),
      new BreakInfo(65294U, true, false),
      new BreakInfo(65306U, true, false),
      new BreakInfo(65307U, true, false),
      new BreakInfo(65311U, true, false),
      new BreakInfo(65312U, false, true),
      new BreakInfo(65339U, false, true),
      new BreakInfo(65341U, true, false),
      new BreakInfo(65344U, true, false),
      new BreakInfo(65371U, false, true),
      new BreakInfo(65372U, true, false),
      new BreakInfo(65373U, true, false),
      new BreakInfo(65374U, true, false),
      new BreakInfo(65377U, true, false),
      new BreakInfo(65380U, true, false),
      new BreakInfo(65392U, true, false),
      new BreakInfo(65438U, true, false),
      new BreakInfo(65439U, true, false),
      new BreakInfo(65504U, true, true),
      new BreakInfo(65505U, false, true),
      new BreakInfo(65509U, false, true),
      new BreakInfo(65510U, false, true)
    };
    private static WordWrap.CB_GetWidth GetWidth;
    private static WordWrap.CB_Reserved Reserved;

    public static Prohibition ProhibitionSetting
    {
      get
      {
        return WordWrap.prohibition;
      }
      set
      {
        WordWrap.prohibition = value;
      }
    }

    public static NoHangulWrap NoHangulWrapSetting
    {
      get
      {
        return WordWrap.nohangul;
      }
      set
      {
        WordWrap.nohangul = value;
      }
    }

    static WordWrap()
    {
      WordWrap.SetCallback(new WordWrap.CB_GetWidth(WordWrap.MyGetCharWidth), (WordWrap.CB_Reserved) null);
    }

    private static uint MyGetCharWidth(SpriteFont spriteFont, char c)
    {
      return (uint) spriteFont.MeasureString(new string(c, 1)).X;
    }

    public static string Split(string text, SpriteFont font, float maxTextSize)
    {
      string source = text;
      text = "";
      if (source.Length == 0)
        return text;
      do
      {
        string EOL;
        int EOLOffset;
        string nextLine = WordWrap.FindNextLine(font, source, (uint) maxTextSize, out EOL, out EOLOffset);
        int length = EOL != null ? EOLOffset + 1 : 0;
        if (length != 0)
        {
          string str = source.Substring(0, length);
          text = text + str + Environment.NewLine;
        }
        else
          text = text + Environment.NewLine;
        source = nextLine;
      }
      while (source != null);
      if (text.EndsWith("\n"))
        text = text.Substring(0, text.Length - 2);
      return text;
    }

    public static bool IsNonBeginningChar(char c)
    {
      if (WordWrap.prohibition == Prohibition.Off)
        return false;
      int num1 = 0;
      int num2 = WordWrap.BreakArray.Length;
      while (num1 <= num2)
      {
        int index = (num2 - num1) / 2 + num1;
        if ((int) WordWrap.BreakArray[index].Character == (int) c)
          return WordWrap.BreakArray[index].IsNonBeginningCharacter;
        if ((uint) c < WordWrap.BreakArray[index].Character)
          num2 = index - 1;
        else
          num1 = index + 1;
      }
      return false;
    }

    public static bool IsNonEndingChar(char c)
    {
      if (WordWrap.prohibition == Prohibition.Off)
        return false;
      int num1 = 0;
      int num2 = WordWrap.BreakArray.Length;
      while (num1 <= num2)
      {
        int index = (num2 - num1) / 2 + num1;
        if ((int) WordWrap.BreakArray[index].Character == (int) c)
          return WordWrap.BreakArray[index].IsNonEndingCharacter;
        if ((uint) c < WordWrap.BreakArray[index].Character)
          num2 = index - 1;
        else
          num1 = index + 1;
      }
      return false;
    }

    public static void SetCallback(WordWrap.CB_GetWidth cbGetWidth, WordWrap.CB_Reserved pReserved)
    {
      if (cbGetWidth != null)
        WordWrap.GetWidth = cbGetWidth;
      if (pReserved == null)
        return;
      WordWrap.Reserved = pReserved;
    }

    public static bool IsEastAsianChar(char c)
    {
      if (WordWrap.nohangul == NoHangulWrap.On && (4352 <= (int) c && (int) c <= 4607 || 12592 <= (int) c && (int) c <= 12687 || 44032 <= (int) c && (int) c <= 55203))
        return false;
      if (4352 <= (int) c && (int) c <= 4607 || 12288 <= (int) c && (int) c <= 55215 || 63744 <= (int) c && (int) c <= 64255)
        return true;
      if (65280 <= (int) c)
        return (int) c <= 65500;
      else
        return false;
    }

    public static bool CanBreakLineAt(string pszStart, int index)
    {
      if (index == 0 || WordWrap.IsWhiteSpace(pszStart[index]) && WordWrap.IsNonBeginningChar(pszStart[index + 1]) || index > 1 && WordWrap.IsWhiteSpace(pszStart[index - 2]) && ((int) pszStart[index - 1] == 34 && !WordWrap.IsWhiteSpace(pszStart[index])) || (!WordWrap.IsWhiteSpace(pszStart[index - 1]) && (int) pszStart[index] == 34 && WordWrap.IsWhiteSpace(pszStart[index + 1]) || !WordWrap.IsWhiteSpace(pszStart[index]) && !WordWrap.IsEastAsianChar(pszStart[index]) && (!WordWrap.IsEastAsianChar(pszStart[index - 1]) && (int) pszStart[index - 1] != 45)) || WordWrap.IsNonBeginningChar(pszStart[index]))
        return false;
      else
        return !WordWrap.IsNonEndingChar(pszStart[index - 1]);
    }

    public static string FindNonWhiteSpaceForward(string text)
    {
      int startIndex = 0;
      while (startIndex < text.Length && WordWrap.IsWhiteSpace(text[startIndex]))
        ++startIndex;
      if (startIndex < text.Length && WordWrap.IsLineFeed(text[startIndex]))
        ++startIndex;
      if (startIndex >= text.Length)
        return (string) null;
      else
        return text.Substring(startIndex);
    }

    public static string FindNonWhiteSpaceBackward(string source, int index, out int offset)
    {
      while (index >= 0 && (WordWrap.IsWhiteSpace(source[index]) || WordWrap.IsLineFeed(source[index])))
        --index;
      offset = index;
      if (index >= 0)
        return source.Substring(index);
      else
        return (string) null;
    }

    public static bool IsWhiteSpace(char c)
    {
      if ((int) c != 9 && (int) c != 13 && (int) c != 32)
        return (int) c == 12288;
      else
        return true;
    }

    public static bool IsLineFeed(char c)
    {
      return (int) c == 10;
    }

    public static string FindNextLine(SpriteFont spriteFont, string source, uint width, out string EOL, out int EOLOffset)
    {
      if (WordWrap.GetWidth == null || source == null)
      {
        EOL = (string) null;
        EOLOffset = -1;
        return (string) null;
      }
      else
      {
        int index = 0;
        uint num = 0U;
        for (; index < source.Length && !WordWrap.IsLineFeed(source[index]); ++index)
        {
          num += WordWrap.GetWidth(spriteFont, source[index]);
          if (num > width)
            break;
        }
        if (index == 0)
        {
          EOL = WordWrap.FindNonWhiteSpaceBackward(source, index, out EOLOffset);
          return WordWrap.FindNonWhiteSpaceForward(source.Substring(index + 1));
        }
        else if (num <= width)
        {
          EOL = WordWrap.FindNonWhiteSpaceBackward(source, index - 1, out EOLOffset);
          if (index - 1 >= 0 && WordWrap.IsLineFeed(source[index - 1]))
            return source.Substring(index);
          if (index < 0 || index >= source.Length)
            return (string) null;
          else
            return WordWrap.FindNonWhiteSpaceForward(source.Substring(index));
        }
        else
        {
          int startIndex = index;
          for (; index > 0; --index)
          {
            if (WordWrap.IsWhiteSpace(source[index]))
            {
              EOL = WordWrap.FindNonWhiteSpaceBackward(source, index, out EOLOffset);
              if (EOL != null)
                return WordWrap.FindNonWhiteSpaceForward(source.Substring(index + 1));
              index = EOLOffset + 1;
            }
            if (WordWrap.CanBreakLineAt(source, index))
              break;
          }
          if (index <= 0)
          {
            EOL = source.Substring(startIndex - 1);
            EOLOffset = startIndex - 1;
            return source.Substring(startIndex);
          }
          else
          {
            EOL = source.Substring(index - 1);
            EOLOffset = index - 1;
            return WordWrap.FindNonWhiteSpaceForward(source.Substring(index));
          }
        }
      }
    }

    public delegate uint CB_GetWidth(SpriteFont spriteFont, char c);

    public delegate uint CB_Reserved();
  }
}
