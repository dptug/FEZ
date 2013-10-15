// Type: SDL.Token
// Assembly: SimpleDefinitionLanguage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8A9B110C-63DC-4C6F-B639-88CD09E9B5B5
// Assembly location: F:\Program Files (x86)\FEZ\SimpleDefinitionLanguage.dll

using System;

namespace SDL
{
  internal class Token
  {
    private const string NumericalChars = "01234567890-.";
    internal readonly Parser parser;
    internal readonly Type type;
    internal readonly string text;
    internal readonly int line;
    internal readonly int position;
    internal readonly int size;
    internal readonly object obj;
    internal readonly bool punctuation;
    internal readonly bool literal;

    internal Token(Parser parser, string text, int line, int position)
    {
      this.parser = parser;
      this.text = text;
      this.line = line;
      this.position = position;
      this.size = text.Length;
      this.obj = (object) null;
      try
      {
        char ch = text[0];
        switch (ch)
        {
          case '"':
          case '`':
            this.type = Type.STRING;
            this.obj = (object) Parser.ParseString(text);
            break;
          case '\'':
            this.type = Type.CHARACTER;
            this.obj = (object) text[1];
            break;
          default:
            if (text == "null")
            {
              this.type = Type.NULL;
              break;
            }
            else if (text == "true" || text == "on")
            {
              this.type = Type.BOOLEAN;
              this.obj = (object) true;
              break;
            }
            else if (text == "false" || text == "off")
            {
              this.type = Type.BOOLEAN;
              this.obj = (object) false;
              break;
            }
            else if ((int) ch == 91)
            {
              this.type = Type.BINARY;
              this.obj = (object) Parser.ParseBinary(text);
              break;
            }
            else if ((int) ch != 47 && text.IndexOf('/') != -1 && text.IndexOf(':') == -1)
            {
              this.type = Type.DATE;
              this.obj = (object) Parser.ParseDateTime(text);
              break;
            }
            else if ((int) ch != 58 && text.IndexOf(':') != -1)
            {
              this.type = Type.TIME;
              this.obj = (object) Token.ParseTimeSpanWithZone(text, parser, line, position);
              break;
            }
            else if ("01234567890-.".IndexOf(ch) != -1)
            {
              this.type = Type.NUMBER;
              this.obj = Parser.ParseNumber(text);
              break;
            }
            else
            {
              this.type = (int) ch != 123 ? ((int) ch != 125 ? ((int) ch != 61 ? ((int) ch != 58 ? Type.IDENTIFIER : Type.COLON) : Type.EQUALS) : Type.END_BLOCK) : Type.START_BLOCK;
              break;
            }
        }
      }
      catch (FormatException ex)
      {
        throw new SDLParseException(ex.Message, line, position);
      }
      this.punctuation = this.type == Type.COLON || this.type == Type.EQUALS || this.type == Type.START_BLOCK || this.type == Type.END_BLOCK;
      this.literal = this.type != Type.IDENTIFIER && !this.punctuation;
    }

    internal object GetObjectForLiteral()
    {
      return this.obj;
    }

    public override string ToString()
    {
      return (string) (object) this.type + (object) " " + this.text + " pos:" + (string) (object) this.position;
    }

    internal static TimeSpanWithZone ParseTimeSpanWithZone(string text, Parser parser, int line, int position)
    {
      int days = 0;
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      int num4 = 0;
      string timeZone = (string) null;
      string str1 = text;
      int length1 = str1.IndexOf('-', 1);
      if (length1 != -1)
      {
        timeZone = str1.Substring(length1 + 1);
        str1 = text.Substring(0, length1);
      }
      string[] strArray = str1.Split(new char[1]
      {
        ':'
      });
      if (timeZone != null)
      {
        if (strArray.Length >= 2)
        {
          if (strArray.Length <= 3)
            goto label_9;
        }
        parser.ParseException("date/time format exception.  Must use hh:mm(:ss)(.xxx)(-z)", line, position);
      }
      else
      {
        if (strArray.Length >= 2)
        {
          if (strArray.Length <= 4)
            goto label_9;
        }
        parser.ParseException("Time format exception.  For time spans use (d:)hh:mm:ss(.xxx) and for the time component of a date/time type use hh:mm(:ss)(.xxx)(-z)  If you use the day component of a time span make sure to prefix it with a lower case d", line, position);
      }
label_9:
      try
      {
        if (strArray.Length == 4)
        {
          string str2 = strArray[0];
          if (!str2.EndsWith("d"))
            parser.ParseException("The day component of a time span must end with a lower case d", line, position);
          days = Convert.ToInt32(str2.Substring(0, str2.Length - 1));
          num1 = Convert.ToInt32(strArray[1]);
          num2 = Convert.ToInt32(strArray[2]);
          if (strArray.Length == 4)
          {
            string str3 = strArray[3];
            int length2 = str3.IndexOf('.');
            if (length2 == -1)
            {
              num3 = Convert.ToInt32(str3);
            }
            else
            {
              num3 = Convert.ToInt32(str3.Substring(0, length2));
              string str4 = str3.Substring(length2 + 1);
              if (str4.Length == 1)
                str4 = str4 + "00";
              else if (str4.Length == 2)
                str4 = str4 + "0";
              num4 = Convert.ToInt32(str4);
            }
          }
          if (days < 0)
          {
            num1 = Parser.ReverseIfPositive(num1);
            num2 = Parser.ReverseIfPositive(num2);
            num3 = Parser.ReverseIfPositive(num3);
            num4 = Parser.ReverseIfPositive(num4);
          }
        }
        else
        {
          num1 = Convert.ToInt32(strArray[0]);
          num2 = Convert.ToInt32(strArray[1]);
          if (strArray.Length == 3)
          {
            string str2 = strArray[2];
            int length2 = str2.IndexOf(".");
            if (length2 == -1)
            {
              num3 = Convert.ToInt32(str2);
            }
            else
            {
              num3 = Convert.ToInt32(str2.Substring(0, length2));
              string str3 = str2.Substring(length2 + 1);
              if (str3.Length == 1)
                str3 = str3 + "00";
              else if (str3.Length == 2)
                str3 = str3 + "0";
              num4 = Convert.ToInt32(str3);
            }
          }
          if (num1 < 0)
          {
            num2 = Parser.ReverseIfPositive(num2);
            num3 = Parser.ReverseIfPositive(num3);
            num4 = Parser.ReverseIfPositive(num4);
          }
        }
      }
      catch (FormatException ex)
      {
        parser.ParseException("Time format: " + ex.Message, line, position);
      }
      return new TimeSpanWithZone(days, num1, num2, num3, num4, timeZone);
    }
  }
}
