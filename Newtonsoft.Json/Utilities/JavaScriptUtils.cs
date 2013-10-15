// Type: Newtonsoft.Json.Utilities.JavaScriptUtils
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using System.IO;

namespace Newtonsoft.Json.Utilities
{
  internal static class JavaScriptUtils
  {
    private const string EscapedUnicodeText = "!";

    public static void WriteEscapedJavaScriptString(TextWriter writer, string s, char delimiter, bool appendDelimiters)
    {
      if (appendDelimiters)
        writer.Write(delimiter);
      if (s != null)
      {
        char[] buffer1 = (char[]) null;
        char[] buffer2 = (char[]) null;
        int index1 = 0;
        for (int index2 = 0; index2 < s.Length; ++index2)
        {
          char c = s[index2];
          if ((int) c < 32 || (int) c >= 128 || ((int) c == 92 || (int) c == (int) delimiter))
          {
            string a;
            switch (c)
            {
              case '\\':
                a = "\\\\";
                break;
              case '\x0085':
                a = "\\u0085";
                break;
              case '\x2028':
                a = "\\u2028";
                break;
              case '\x2029':
                a = "\\u2029";
                break;
              case '\b':
                a = "\\b";
                break;
              case '\t':
                a = "\\t";
                break;
              case '\n':
                a = "\\n";
                break;
              case '\f':
                a = "\\f";
                break;
              case '\r':
                a = "\\r";
                break;
              case '"':
                a = "\\\"";
                break;
              case '\'':
                a = "\\'";
                break;
              default:
                if ((int) c <= 31)
                {
                  if (buffer2 == null)
                    buffer2 = new char[6];
                  StringUtils.ToCharAsUnicode(c, buffer2);
                  a = "!";
                  break;
                }
                else
                {
                  a = (string) null;
                  break;
                }
            }
            if (a != null)
            {
              if (index2 > index1)
              {
                if (buffer1 == null)
                  buffer1 = s.ToCharArray();
                writer.Write(buffer1, index1, index2 - index1);
              }
              index1 = index2 + 1;
              if (!string.Equals(a, "!"))
                writer.Write(a);
              else
                writer.Write(buffer2);
            }
          }
        }
        if (index1 == 0)
        {
          writer.Write(s);
        }
        else
        {
          if (buffer1 == null)
            buffer1 = s.ToCharArray();
          writer.Write(buffer1, index1, s.Length - index1);
        }
      }
      if (!appendDelimiters)
        return;
      writer.Write(delimiter);
    }

    public static string ToEscapedJavaScriptString(string value)
    {
      return JavaScriptUtils.ToEscapedJavaScriptString(value, '"', true);
    }

    public static string ToEscapedJavaScriptString(string value, char delimiter, bool appendDelimiters)
    {
      using (StringWriter stringWriter = StringUtils.CreateStringWriter(StringUtils.GetLength(value) ?? 16))
      {
        JavaScriptUtils.WriteEscapedJavaScriptString((TextWriter) stringWriter, value, delimiter, appendDelimiters);
        return stringWriter.ToString();
      }
    }
  }
}
