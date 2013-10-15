// Type: SDL.SDLUtil
// Assembly: SimpleDefinitionLanguage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8A9B110C-63DC-4C6F-B639-88CD09E9B5B5
// Assembly location: F:\Program Files (x86)\FEZ\SimpleDefinitionLanguage.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace SDL
{
  public class SDLUtil
  {
    public const string TIME_FORMAT = "HH:mm:ss.fff-z";
    public const string DATE_FORMAT = "yyyy/MM/dd";
    public const string DATE_TIME_FORMAT = "yyyy/MM/dd HH:mm:ss.fff-z";

    public static string ValidateIdentifier(string identifier)
    {
      if (identifier == null || identifier.Length == 0)
        throw new ArgumentException("SDL identifiers cannot be null or empty.");
      if (!char.IsLetter(identifier[0]) && (int) identifier[0] != 95)
        throw new ArgumentException("'" + (object) identifier[0] + "' is not a legal first character for an SDL identifier. SDL Identifiers must start with a unicode letter or underscore (_).");
      int length = identifier.Length;
      for (int index = 1; index < length; ++index)
      {
        if (!char.IsLetterOrDigit(identifier[index]) && (int) identifier[index] != 95 && ((int) identifier[index] != 45 && (int) identifier[index] != 46))
          throw new ArgumentException("'" + (object) identifier[index] + "' is not a legal character for an SDL identifier. SDL Identifiers must start with a unicode letter or underscore (_) followed by zero or more unicode letters, digits, dashes (-) or underscores (_)");
      }
      return identifier;
    }

    public static string Format(object obj)
    {
      return SDLUtil.Format(obj, true);
    }

    public static string Format(object obj, bool addQuotes)
    {
      if (obj == null)
        return "null";
      if (obj is int)
        return ((int) obj).ToString((IFormatProvider) CultureInfo.InvariantCulture);
      if (obj is string)
      {
        if (addQuotes)
          return "\"" + SDLUtil.Escape((string) obj) + "\"";
        else
          return SDLUtil.Escape((string) obj);
      }
      else if (obj is char)
      {
        if (addQuotes)
          return "'" + SDLUtil.Escape((char) obj) + "'";
        else
          return SDLUtil.Escape((char) obj);
      }
      else
      {
        if (obj is Decimal)
          return ((Decimal) obj).ToString((IFormatProvider) NumberFormatInfo.InvariantInfo) + "BD";
        if (obj is float)
          return ((float) obj).ToString("0.######", (IFormatProvider) NumberFormatInfo.InvariantInfo) + "F";
        if (obj is long)
          return ((long) obj).ToString((IFormatProvider) NumberFormatInfo.InvariantInfo) + "L";
        if (obj is byte[])
          return "[" + Convert.ToBase64String((byte[]) obj) + "]";
        if (obj is bool)
        {
          return !obj.Equals((object) true) ? "false" : "true";
        }
        else
        {
          if (!(obj is TimeSpan))
            return obj.ToString();
          TimeSpan timeSpan = (TimeSpan) obj;
          StringBuilder stringBuilder = new StringBuilder();
          if (timeSpan.Days != 0)
          {
            stringBuilder.Append(timeSpan.Days).Append("d:");
            string str = string.Concat((object) Math.Abs(timeSpan.Hours));
            if (str.Length == 1)
              str = "0" + str;
            stringBuilder.Append(str);
          }
          else
          {
            if (timeSpan.Hours < 0)
              stringBuilder.Append('-');
            string str = string.Concat((object) Math.Abs(timeSpan.Hours));
            if (str.Length == 1)
              str = "0" + str;
            stringBuilder.Append(str);
          }
          stringBuilder.Append(":");
          string str1 = string.Concat((object) Math.Abs(timeSpan.Minutes));
          if (str1.Length == 1)
            str1 = "0" + str1;
          stringBuilder.Append(str1);
          stringBuilder.Append(":");
          string str2 = string.Concat((object) Math.Abs(timeSpan.Seconds));
          if (str2.Length == 1)
            str2 = "0" + str2;
          stringBuilder.Append(str2);
          if (timeSpan.Milliseconds == 0)
            return ((object) stringBuilder).ToString();
          string str3 = string.Concat((object) Math.Abs(timeSpan.Milliseconds));
          if (str3.Length == 1)
            str3 = "00" + str3;
          else if (str3.Length == 2)
            str3 = "0" + str3;
          stringBuilder.Append(".").Append(str3);
          string str4 = ((object) stringBuilder).ToString();
          int index = str4.Length - 1;
          while (index > -1 && (int) str4[index] == 48)
            --index;
          return str4.Substring(0, index + 1);
        }
      }
    }

    private static string Escape(string s)
    {
      StringBuilder stringBuilder = new StringBuilder();
      int length = s.Length;
      for (int index = 0; index < length; ++index)
      {
        char ch = s[index];
        switch (ch)
        {
          case '\\':
            stringBuilder.Append("\\\\");
            break;
          case '"':
            stringBuilder.Append("\\\"");
            break;
          case '\t':
            stringBuilder.Append("\\t");
            break;
          case '\r':
            stringBuilder.Append("\\r");
            break;
          case '\n':
            stringBuilder.Append("\\n");
            break;
          default:
            stringBuilder.Append(ch);
            break;
        }
      }
      return ((object) stringBuilder).ToString();
    }

    private static string Escape(char c)
    {
      switch (c)
      {
        case '\t':
          return "\\t";
        case '\n':
          return "\\n";
        case '\r':
          return "\\r";
        case '\'':
          return "\\'";
        case '\\':
          return "\\\\";
        default:
          return string.Concat((object) c);
      }
    }

    public static object CoerceOrFail(object obj)
    {
      bool succeeded;
      object obj1 = SDLUtil.TryCoerce(obj, out succeeded);
      if (!succeeded)
        throw new ArgumentException((string) (object) obj.GetType() + (object) " is not coercible to an SDL type.");
      else
        return obj1;
    }

    public static object TryCoerce(object obj, out bool succeeded)
    {
      succeeded = true;
      if (obj == null)
        return (object) null;
      if (obj is string || obj is char || (obj is int || obj is long) || (obj is float || obj is double || (obj is Decimal || obj is bool)) || (obj is TimeSpan || obj is SDLDateTime || obj is byte[]))
        return obj;
      if (obj is DateTime)
        return (object) new SDLDateTime((DateTime) obj, (string) null);
      if (obj is sbyte || obj is byte || (obj is short || obj is ushort))
        return (object) Convert.ToInt32(obj);
      if (obj is uint)
        return (object) Convert.ToInt64(obj);
      succeeded = false;
      return obj;
    }

    public static bool IsCoercible(System.Type type)
    {
      if (!(type == typeof (string)) && !(type == typeof (char)) && (!(type == typeof (int)) && !(type == typeof (long))) && (!(type == typeof (float)) && !(type == typeof (double)) && (!(type == typeof (Decimal)) && !(type == typeof (bool)))) && (!(type == typeof (TimeSpan)) && !(type == typeof (SDLDateTime)) && (!(type == typeof (byte[])) && !(type == typeof (DateTime))) && (!(type == typeof (sbyte)) && !(type == typeof (byte)) && (!(type == typeof (short)) && !(type == typeof (ushort))))))
        return type == typeof (uint);
      else
        return true;
    }

    public static object Value(string literal)
    {
      if (literal == null)
        throw new ArgumentNullException("literal argument to SDL.Value(string) cannot be null");
      if (literal.StartsWith("\"") || literal.StartsWith("`"))
        return (object) Parser.ParseString(literal);
      if (literal.StartsWith("'"))
        return (object) Parser.ParseCharacter(literal);
      if (literal.Equals("null"))
        return (object) null;
      if (literal.Equals("true") || literal.Equals("on"))
        return (object) true;
      if (literal.Equals("false") || literal.Equals("off"))
        return (object) false;
      if (literal.StartsWith("["))
        return (object) Parser.ParseBinary(literal);
      if ((int) literal[0] != 47 && literal.IndexOf('/') != -1)
        return (object) Parser.ParseDateTime(literal);
      if ((int) literal[0] != 58 && literal.IndexOf(':') != -1)
        return (object) Parser.ParseTimeSpan(literal);
      if ("01234567890-.".IndexOf(literal[0]) != -1)
        return Parser.ParseNumber(literal);
      else
        throw new FormatException("String " + literal + " does not represent an SDL type.");
    }

    public static IList<object> List(string valueList)
    {
      if (valueList == null)
        throw new ArgumentNullException("valueList cannot be null");
      else
        return new Tag("root").ReadString(valueList).GetChild("content").Values;
    }

    public static IDictionary<string, object> Map(string attributeString)
    {
      if (attributeString == null)
        throw new ArgumentNullException("attributeString cannot be null");
      else
        return new Tag("root").ReadString("atts " + attributeString).GetChild("atts").Attributes;
    }
  }
}
