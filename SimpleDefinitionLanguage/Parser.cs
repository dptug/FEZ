// Type: SDL.Parser
// Assembly: SimpleDefinitionLanguage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8A9B110C-63DC-4C6F-B639-88CD09E9B5B5
// Assembly location: F:\Program Files (x86)\FEZ\SimpleDefinitionLanguage.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace SDL
{
  internal class Parser
  {
    internal int lineNumber = -1;
    private readonly StringBuilder numberSb = new StringBuilder();
    private const string AlphaNumericChars = "0123456789.-+:abcdefghijklmnopqrstuvwxyz";
    private readonly TextReader reader;
    private string line;
    private List<Token> toks;
    private StringBuilder sb;
    private bool startEscapedQuoteLine;
    internal int pos;
    internal int lineLength;
    internal int tokenStart;

    internal Parser(TextReader reader)
    {
      this.reader = reader;
    }

    internal IList<Tag> Parse()
    {
      List<Tag> list = new List<Tag>();
      List<Token> lineTokens;
      while ((lineTokens = this.GetLineTokens()) != null)
      {
        int count = lineTokens.Count;
        if (lineTokens[count - 1].type == Type.START_BLOCK)
        {
          Tag parent = this.ConstructTag(lineTokens.GetRange(0, count - 1));
          this.AddChildren(parent);
          list.Add(parent);
        }
        else if (lineTokens[0].type == Type.END_BLOCK)
          this.ParseException("No opening block ({) for close block (}).", lineTokens[0].line, lineTokens[0].position);
        else
          list.Add(this.ConstructTag(lineTokens));
      }
      this.reader.Close();
      return (IList<Tag>) list;
    }

    private void AddChildren(Tag parent)
    {
      List<Token> lineTokens;
      while ((lineTokens = this.GetLineTokens()) != null)
      {
        int count = lineTokens.Count;
        if (lineTokens[0].type == Type.END_BLOCK)
          return;
        if (lineTokens[count - 1].type == Type.START_BLOCK)
        {
          Tag tag = this.ConstructTag(lineTokens.GetRange(0, count - 1));
          this.AddChildren(tag);
          parent.AddChild(tag);
        }
        else
          parent.AddChild(this.ConstructTag(lineTokens));
      }
      this.ParseException("No close block (}).", this.lineNumber, -2);
    }

    private Tag ConstructTag(List<Token> toks)
    {
      if (toks.Count == 0)
        this.ParseException("Internal Error: Empty token list", this.lineNumber, -2);
      Token token1 = toks[0];
      if (token1.literal)
        toks.Insert(0, token1 = new Token(this, "content", -1, -1));
      else if (token1.type != Type.IDENTIFIER)
        this.ExpectingButGot("IDENTIFIER", (object) string.Concat(new object[4]
        {
          (object) token1.type,
          (object) " (",
          (object) token1.text,
          (object) ")"
        }), token1.line, token1.position);
      int count = toks.Count;
      Tag tag;
      if (count == 1)
      {
        tag = new Tag(token1.text);
      }
      else
      {
        int tpos1 = 1;
        Token token2 = toks[1];
        if (token2.type == Type.COLON)
        {
          if (count == 2 || toks[2].type != Type.IDENTIFIER)
            this.ParseException("Colon (:) encountered in unexpected location.", token2.line, token2.position);
          Token token3 = toks[2];
          tag = new Tag(token1.text, token3.text);
          tpos1 = 3;
        }
        else
          tag = new Tag(token1.text);
        int tpos2 = this.AddTagValues(tag, (IList<Token>) toks, tpos1);
        if (tpos2 < count)
          this.AddTagAttributes(tag, (IList<Token>) toks, tpos2);
      }
      return tag;
    }

    private int AddTagValues(Tag tag, IList<Token> toks, int tpos)
    {
      int count = toks.Count;
      int index;
      for (index = tpos; index < count; ++index)
      {
        Token token = toks[index];
        if (token.literal)
        {
          if (token.type == Type.DATE && index + 1 < count && toks[index + 1].type == Type.TIME)
          {
            SDLDateTime dt = (SDLDateTime) token.GetObjectForLiteral();
            TimeSpanWithZone tswz = (TimeSpanWithZone) toks[index + 1].GetObjectForLiteral();
            if (tswz.Days != 0)
            {
              tag.AddValue((object) dt);
              tag.AddValue((object) new TimeSpan(tswz.Days, tswz.Hours, tswz.Minutes, tswz.Seconds, tswz.Milliseconds));
              if (tswz.TimeZone != null)
                this.ParseException("TimeSpan cannot have a timezone", token.line, token.position);
            }
            else
              tag.AddValue((object) Parser.Combine(dt, tswz));
            ++index;
          }
          else
          {
            object objectForLiteral = token.GetObjectForLiteral();
            if (objectForLiteral is TimeSpanWithZone)
            {
              TimeSpanWithZone timeSpanWithZone = (TimeSpanWithZone) objectForLiteral;
              if (timeSpanWithZone.TimeZone != null)
                this.ExpectingButGot("TIME SPAN", (object) "TIME (component of date/time)", token.line, token.position);
              tag.AddValue((object) new TimeSpan(timeSpanWithZone.Days, timeSpanWithZone.Hours, timeSpanWithZone.Minutes, timeSpanWithZone.Seconds, timeSpanWithZone.Milliseconds));
            }
            else
              tag.AddValue(objectForLiteral);
          }
        }
        else if (token.type != Type.IDENTIFIER)
          this.ExpectingButGot("LITERAL or IDENTIFIER", (object) token.type, token.line, token.position);
        else
          break;
      }
      return index;
    }

    private void AddTagAttributes(Tag tag, IList<Token> toks, int tpos)
    {
      int index1 = tpos;
      for (int count = toks.Count; index1 < count; {
        int num;
        index1 = num + 1;
      }
      )
      {
        Token token1 = toks[index1];
        if (token1.type != Type.IDENTIFIER)
          this.ExpectingButGot("IDENTIFIER", (object) token1.type, token1.line, token1.position);
        string index2 = token1.text;
        if (index1 == count - 1)
          this.ExpectingButGot("\":\" or \"=\" \"LITERAL\"", (object) "END OF LINE.", token1.line, token1.position);
        Token token2 = toks[num = index1 + 1];
        if (token2.type == Type.COLON)
        {
          if (num == count - 1)
            this.ExpectingButGot("IDENTIFIER", (object) "END OF LINE", token2.line, token2.position);
          int num1;
          Token token3 = toks[num1 = num + 1];
          if (token3.type != Type.IDENTIFIER)
            this.ExpectingButGot("IDENTIFIER", (object) token3.type, token3.line, token3.position);
          string index3 = token3.text;
          if (num1 == count - 1)
            this.ExpectingButGot("\"=\"", (object) "END OF LINE", token3.line, token3.position);
          int num2;
          Token token4 = toks[num2 = num1 + 1];
          if (token4.type != Type.EQUALS)
            this.ExpectingButGot("\"=\"", (object) token4.type, token4.line, token4.position);
          if (num2 == count - 1)
            this.ExpectingButGot("LITERAL", (object) "END OF LINE", token4.line, token4.position);
          Token token5 = toks[num = num2 + 1];
          if (!token5.literal)
            this.ExpectingButGot("LITERAL", (object) token5.type, token5.line, token5.position);
          if (token5.type == Type.DATE && num + 1 < count && toks[num + 1].type == Type.TIME)
          {
            SDLDateTime dt = (SDLDateTime) token5.GetObjectForLiteral();
            TimeSpanWithZone tswz = (TimeSpanWithZone) toks[num + 1].GetObjectForLiteral();
            if (tswz.Days != 0)
              this.ExpectingButGot("TIME (component of date/time) in attribute value", (object) "TIME SPAN", token5.line, token5.position);
            tag[index2, index3] = (object) Parser.Combine(dt, tswz);
            ++num;
          }
          else
          {
            object objectForLiteral = token5.GetObjectForLiteral();
            if (objectForLiteral is TimeSpanWithZone)
            {
              TimeSpanWithZone timeSpanWithZone = (TimeSpanWithZone) objectForLiteral;
              if (timeSpanWithZone.TimeZone != null)
                this.ExpectingButGot("TIME SPAN", (object) "TIME (component of date/time)", token5.line, token5.position);
              TimeSpan timeSpan = new TimeSpan(timeSpanWithZone.Days, timeSpanWithZone.Hours, timeSpanWithZone.Minutes, timeSpanWithZone.Seconds, timeSpanWithZone.Milliseconds);
              tag[index2, index3] = (object) timeSpan;
            }
            else
              tag[index2, index3] = objectForLiteral;
          }
        }
        else if (token2.type == Type.EQUALS)
        {
          if (num == count - 1)
            this.ExpectingButGot("LITERAL", (object) "END OF LINE", token2.line, token2.position);
          Token token3 = toks[++num];
          if (!token3.literal)
            this.ExpectingButGot("LITERAL", (object) token3.type, token3.line, token3.position);
          if (token3.type == Type.DATE && num + 1 < count && toks[num + 1].type == Type.TIME)
          {
            SDLDateTime dt = (SDLDateTime) token3.GetObjectForLiteral();
            TimeSpanWithZone tswz = (TimeSpanWithZone) toks[num + 1].GetObjectForLiteral();
            if (tswz.Days != 0)
              this.ExpectingButGot("TIME (component of date/time) in attribute value", (object) "TIME SPAN", token3.line, token3.position);
            tag[index2] = (object) Parser.Combine(dt, tswz);
            ++num;
          }
          else
          {
            object objectForLiteral = token3.GetObjectForLiteral();
            if (objectForLiteral is TimeSpanWithZone)
            {
              TimeSpanWithZone timeSpanWithZone = (TimeSpanWithZone) objectForLiteral;
              if (timeSpanWithZone.TimeZone != null)
                this.ExpectingButGot("TIME SPAN", (object) "TIME (component of date/time)", token3.line, token3.position);
              TimeSpan timeSpan = new TimeSpan(timeSpanWithZone.Days, timeSpanWithZone.Hours, timeSpanWithZone.Minutes, timeSpanWithZone.Seconds, timeSpanWithZone.Milliseconds);
              tag[index2] = (object) timeSpan;
            }
            else
              tag[index2] = objectForLiteral;
          }
        }
        else
          this.ExpectingButGot("\":\" or \"=\"", (object) token2.type, token2.line, token2.position);
      }
    }

    private List<Token> GetLineTokens()
    {
      this.line = this.ReadLine();
      if (this.line == null)
        return (List<Token>) null;
      this.toks = new List<Token>();
      this.lineLength = this.line.Length;
      this.sb = (StringBuilder) null;
      this.tokenStart = 0;
      for (; this.pos < this.lineLength; ++this.pos)
      {
        char c = this.line[this.pos];
        if (this.sb != null)
        {
          this.toks.Add(new Token(this, ((object) this.sb).ToString(), this.lineNumber, this.tokenStart));
          this.sb = (StringBuilder) null;
        }
        if ((int) c == 34)
          this.HandleDoubleQuoteString();
        else if ((int) c == 39)
          this.HandleCharacterLiteral();
        else if ("{}=:".IndexOf(c) != -1)
        {
          this.toks.Add(new Token(this, string.Concat((object) c), this.lineNumber, this.pos));
          this.sb = (StringBuilder) null;
        }
        else if ((int) c != 35)
        {
          if ((int) c == 47)
          {
            if (this.pos + 1 >= this.lineLength || (int) this.line[this.pos + 1] != 47)
              this.HandleSlashComment();
            else
              break;
          }
          else if ((int) c == 96)
            this.HandleBackQuoteString();
          else if ((int) c == 91)
            this.HandleBinaryLiteral();
          else if ((int) c == 32 || (int) c == 9)
          {
            while (this.pos + 1 < this.lineLength && " \t".IndexOf(this.line[this.pos + 1]) != -1)
              ++this.pos;
          }
          else if ((int) c == 92)
            this.HandleLineContinuation();
          else if ("0123456789-.".IndexOf(c) != -1)
          {
            if ((int) c != 45 || this.pos + 1 >= this.lineLength || (int) this.line[this.pos + 1] != 45)
              this.HandleNumberDateOrTimeSpan();
            else
              break;
          }
          else if (char.IsLetter(c) || (int) c == 95)
            this.HandleIdentifier();
          else
            this.ParseException("Unexpected character \"" + (object) c + "\".)", this.lineNumber, this.pos);
        }
        else
          break;
      }
      if (this.sb != null)
        this.toks.Add(new Token(this, ((object) this.sb).ToString(), this.lineNumber, this.tokenStart));
      while (this.toks != null && this.toks.Count == 0)
        this.toks = this.GetLineTokens();
      return this.toks;
    }

    private void AddEscapedCharInString(char c)
    {
      switch (c)
      {
        case 'n':
          this.sb.Append('\n');
          break;
        case 'r':
          this.sb.Append('\r');
          break;
        case 't':
          this.sb.Append('\t');
          break;
        case '"':
          this.sb.Append(c);
          break;
        case '\\':
          this.sb.Append(c);
          break;
        default:
          this.ParseException("Ellegal escape character in string literal: \"" + (object) c + "\".", this.lineNumber, this.pos);
          break;
      }
    }

    private void HandleDoubleQuoteString()
    {
      bool flag = false;
      this.startEscapedQuoteLine = false;
      this.sb = new StringBuilder("\"");
      for (++this.pos; this.pos < this.lineLength; ++this.pos)
      {
        char c = this.line[this.pos];
        if (" \t".IndexOf(c) == -1 || !this.startEscapedQuoteLine)
        {
          this.startEscapedQuoteLine = false;
          if (flag)
          {
            this.AddEscapedCharInString(c);
            flag = false;
          }
          else if ((int) c == 92)
          {
            if (this.pos == this.lineLength - 1 || this.pos + 1 < this.lineLength && " \t".IndexOf(this.line[this.pos + 1]) != -1)
              this.HandleEscapedDoubleQuotedString();
            else
              flag = true;
          }
          else
          {
            this.sb.Append(c);
            if ((int) c == 34)
            {
              this.toks.Add(new Token(this, ((object) this.sb).ToString(), this.lineNumber, this.tokenStart));
              this.sb = (StringBuilder) null;
              return;
            }
          }
        }
      }
      if (this.sb == null)
        return;
      string str = ((object) this.sb).ToString();
      if (str.Length > 0 && (int) str[0] == 34 && (int) str[str.Length - 1] != 34)
      {
        this.ParseException("String literal \"" + str + "\" not terminated by end quote.", this.lineNumber, this.line.Length);
      }
      else
      {
        if (str.Length != 1 || (int) str[0] != 34)
          return;
        this.ParseException("Orphan quote (unterminated string)", this.lineNumber, this.line.Length);
      }
    }

    private void HandleEscapedDoubleQuotedString()
    {
      if (this.pos == this.lineLength - 1)
      {
        this.line = this.ReadLine();
        if (this.line == null)
          this.ParseException("Escape at end of file.", this.lineNumber, this.pos);
        this.lineLength = this.line.Length;
        this.pos = -1;
        this.startEscapedQuoteLine = true;
      }
      else
      {
        int index = this.pos + 1;
        while (index < this.lineLength && " \t".IndexOf(this.line[index]) != -1)
          ++index;
        if (index == this.lineLength)
        {
          this.line = this.ReadLine();
          if (this.line == null)
            this.ParseException("Escape at end of file.", this.lineNumber, this.pos);
          this.lineLength = this.line.Length;
          this.pos = -1;
          this.startEscapedQuoteLine = true;
        }
        else
          this.ParseException("Malformed string literal - escape followed by whitespace followed by non-whitespace.", this.lineNumber, this.pos);
      }
    }

    private void HandleCharacterLiteral()
    {
      if (this.pos == this.lineLength - 1)
        this.ParseException("Got ' at end of line", this.lineNumber, this.pos);
      ++this.pos;
      char ch1 = this.line[this.pos];
      if ((int) ch1 == 92)
      {
        if (this.pos == this.lineLength - 1)
          this.ParseException("Got '\\ at end of line", this.lineNumber, this.pos);
        ++this.pos;
        char ch2 = this.line[this.pos];
        if (this.pos == this.lineLength - 1)
          this.ParseException("Got '\\" + (object) ch2 + " at end of line", this.lineNumber, this.pos);
        if ((int) ch2 == 92)
          this.toks.Add(new Token(this, "'\\'", this.lineNumber, this.pos));
        else if ((int) ch2 == 39)
          this.toks.Add(new Token(this, "'''", this.lineNumber, this.pos));
        else if ((int) ch2 == 110)
          this.toks.Add(new Token(this, "'\n'", this.lineNumber, this.pos));
        else if ((int) ch2 == 114)
          this.toks.Add(new Token(this, "'\r'", this.lineNumber, this.pos));
        else if ((int) ch2 == 116)
          this.toks.Add(new Token(this, "'\t'", this.lineNumber, this.pos));
        else
          this.ParseException("Illegal escape character " + (object) this.line[this.pos], this.lineNumber, this.pos);
        ++this.pos;
        if ((int) this.line[this.pos] == 39)
          return;
        this.ExpectingButGot("single quote (')", (object) ("\"" + (object) this.line[this.pos] + "\""), this.lineNumber, this.pos);
      }
      else
      {
        this.toks.Add(new Token(this, "'" + (object) ch1 + "'", this.lineNumber, this.pos));
        if (this.pos == this.lineLength - 1)
          this.ParseException("Got '" + (object) ch1 + " at end of line", this.lineNumber, this.pos);
        ++this.pos;
        if ((int) this.line[this.pos] == 39)
          return;
        this.ExpectingButGot("quote (')", (object) ("\"" + (object) this.line[this.pos] + "\""), this.lineNumber, this.pos);
      }
    }

    private void HandleSlashComment()
    {
      if (this.pos == this.lineLength - 1)
        this.ParseException("Got slash (/) at end of line.", this.lineNumber, this.pos);
      if ((int) this.line[this.pos + 1] == 42)
      {
        int num1 = this.line.IndexOf("*/", this.pos + 1);
        if (num1 != -1)
        {
          this.pos = num1 + 1;
        }
        else
        {
          int num2;
          do
          {
            this.line = this.ReadRawLine();
            if (this.line == null)
              this.ParseException("/* comment not terminated.", this.lineNumber, -2);
            num2 = this.line.IndexOf("*/");
          }
          while (num2 == -1);
          this.lineLength = this.line.Length;
          this.pos = num2 + 1;
        }
      }
      else
      {
        if ((int) this.line[this.pos + 1] != 47)
          return;
        this.ParseException("Got slash (/) in unexpected location.", this.lineNumber, this.pos);
      }
    }

    private void HandleBackQuoteString()
    {
      int num1 = this.line.IndexOf("`", this.pos + 1);
      if (num1 != -1)
      {
        this.toks.Add(new Token(this, this.line.Substring(this.pos, num1 + 1 - this.pos), this.lineNumber, this.pos));
        this.sb = (StringBuilder) null;
        this.pos = num1;
      }
      else
      {
        this.sb = new StringBuilder(this.line.Substring(this.pos) + "\n");
        int position = this.pos;
        int num2;
        while (true)
        {
          this.line = this.ReadRawLine();
          if (this.line == null)
            this.ParseException("` quote not terminated.", this.lineNumber, -2);
          num2 = this.line.IndexOf('`');
          if (num2 == -1)
            this.sb.Append(this.line + "\n");
          else
            break;
        }
        this.sb.Append(this.line.Substring(0, num2 + 1));
        this.line = this.line.Trim();
        this.lineLength = this.line.Length;
        this.pos = num2;
        this.toks.Add(new Token(this, ((object) this.sb).ToString(), this.lineNumber, position));
        this.sb = (StringBuilder) null;
      }
    }

    private void HandleBinaryLiteral()
    {
      int num1 = this.line.IndexOf(']', this.pos + 1);
      if (num1 != -1)
      {
        this.toks.Add(new Token(this, this.line.Substring(this.pos, num1 + 1 - this.pos), this.lineNumber, this.pos));
        this.sb = (StringBuilder) null;
        this.pos = num1;
      }
      else
      {
        this.sb = new StringBuilder(this.line.Substring(this.pos) + "\n");
        int position = this.pos;
        int num2;
        while (true)
        {
          this.line = this.ReadRawLine();
          if (this.line == null)
            this.ParseException("[base64] binary literal not terminated.", this.lineNumber, -2);
          num2 = this.line.IndexOf(']');
          if (num2 == -1)
            this.sb.Append(this.line + "\n");
          else
            break;
        }
        this.sb.Append(this.line.Substring(0, num2 + 1));
        this.line = this.line.Trim();
        this.lineLength = this.line.Length;
        this.pos = num2;
        this.toks.Add(new Token(this, ((object) this.sb).ToString(), this.lineNumber, position));
        this.sb = (StringBuilder) null;
      }
    }

    private void HandleLineContinuation()
    {
      if (this.line.Substring(this.pos + 1).Trim().Length != 0)
      {
        this.ParseException("Line continuation (\\) before end of line", this.lineNumber, this.pos);
      }
      else
      {
        this.line = this.ReadLine();
        if (this.line == null)
          this.ParseException("Line continuation at end of file.", this.lineNumber, this.pos);
        this.lineLength = this.line.Length;
        this.pos = -1;
      }
    }

    private void HandleNumberDateOrTimeSpan()
    {
      this.tokenStart = this.pos;
      for (; this.pos < this.lineLength; ++this.pos)
      {
        char c = this.line[this.pos];
        if ("0123456789.-+:abcdefghijklmnopqrstuvwxyz".IndexOf(char.ToLower(c)) != -1)
          this.numberSb.Append(c);
        else if ((int) c == 47 && (this.pos + 1 >= this.lineLength || (int) this.line[this.pos + 1] != 42))
        {
          this.numberSb.Append(c);
        }
        else
        {
          --this.pos;
          break;
        }
      }
      this.toks.Add(new Token(this, ((object) this.numberSb).ToString(), this.lineNumber, this.tokenStart));
      this.numberSb.Remove(0, this.numberSb.Length);
    }

    private void HandleIdentifier()
    {
      this.tokenStart = this.pos;
      this.sb = new StringBuilder();
      for (; this.pos < this.lineLength; ++this.pos)
      {
        char c = this.line[this.pos];
        if (char.IsLetterOrDigit(c) || (int) c == 45 || ((int) c == 95 || (int) c == 46))
        {
          this.sb.Append(c);
        }
        else
        {
          --this.pos;
          break;
        }
      }
      this.toks.Add(new Token(this, ((object) this.sb).ToString(), this.lineNumber, this.tokenStart));
      this.sb = (StringBuilder) null;
    }

    private string ReadLine()
    {
      string str1 = this.reader.ReadLine();
      this.pos = 0;
      if (str1 == null)
        return (string) null;
      ++this.lineNumber;
      for (string str2 = str1.Trim(); str2.StartsWith("#") || str2.Length == 0; str2 = str1.Trim())
      {
        str1 = this.reader.ReadLine();
        if (str1 == null)
          return (string) null;
        ++this.lineNumber;
      }
      return str1;
    }

    private string ReadRawLine()
    {
      string str = this.reader.ReadLine();
      this.pos = 0;
      if (str == null)
        return (string) null;
      ++this.lineNumber;
      return str;
    }

    private static SDLDateTime Combine(SDLDateTime dt, TimeSpanWithZone tswz)
    {
      return new SDLDateTime(dt.Year, dt.Month, dt.Day, tswz.Hours, tswz.Minutes, tswz.Seconds, tswz.Milliseconds, tswz.TimeZone);
    }

    internal static int ReverseIfPositive(int val)
    {
      if (val < 1)
        return val;
      else
        return -val;
    }

    internal void ParseException(string description, int line, int position)
    {
      try
      {
        this.reader.Close();
      }
      catch
      {
      }
      throw new SDLParseException(description, line + 1, position + 1);
    }

    internal void ExpectingButGot(string expecting, object got, int line, int position)
    {
      this.ParseException(string.Concat(new object[4]
      {
        (object) "Was expecting ",
        (object) expecting,
        (object) " but got ",
        got
      }), line, position);
    }

    internal static string ParseString(string literal)
    {
      if ((int) literal[0] != (int) literal[literal.Length - 1])
        throw new FormatException("Malformed string <" + literal + ">.  Strings must start and end with \" or `");
      else
        return literal.Substring(1, literal.Length - 2);
    }

    internal static char ParseCharacter(string literal)
    {
      if ((int) literal[0] != 39 || (int) literal[literal.Length - 1] != 39)
        throw new FormatException("Malformed character <" + literal + ">.  Character literals must start and end with single quotes.");
      else
        return literal[1];
    }

    internal static object ParseNumber(string literal)
    {
      int length = literal.Length;
      bool flag = false;
      int num = 0;
      for (int index = 0; index < length; ++index)
      {
        char ch = literal[index];
        if ("-0123456789".IndexOf(ch) == -1)
        {
          if ((int) ch == 46)
          {
            if (flag)
            {
              FormatException formatException1 = new FormatException("Encountered second decimal point.");
            }
            else if (index == length - 1)
            {
              FormatException formatException2 = new FormatException("Encountered decimal point at the end of the number.");
            }
            else
              flag = true;
          }
          else
          {
            num = index;
            break;
          }
        }
        else
          num = index + 1;
      }
      string s = literal.Substring(0, num);
      string str = literal.Substring(num);
      if (str.Length == 0)
      {
        if (flag)
          return (object) Convert.ToDouble(s, (IFormatProvider) NumberFormatInfo.InvariantInfo);
        else
          return (object) int.Parse(s, NumberStyles.Integer, (IFormatProvider) NumberFormatInfo.InvariantInfo);
      }
      else
      {
        if (str.ToUpper(CultureInfo.InvariantCulture).Equals("BD"))
          return (object) Convert.ToDecimal(s, (IFormatProvider) NumberFormatInfo.InvariantInfo);
        if (str.ToUpper(CultureInfo.InvariantCulture).Equals("L"))
        {
          if (flag)
          {
            FormatException formatException = new FormatException("Long literal with decimal point");
          }
          return (object) Convert.ToInt64(s, (IFormatProvider) NumberFormatInfo.InvariantInfo);
        }
        else
        {
          if (str.ToUpper(CultureInfo.InvariantCulture).Equals("F"))
            return (object) Convert.ToSingle(s, (IFormatProvider) NumberFormatInfo.InvariantInfo);
          if (str.ToUpper(CultureInfo.InvariantCulture).Equals("D"))
            return (object) Convert.ToDouble(s, (IFormatProvider) NumberFormatInfo.InvariantInfo);
          else
            throw new FormatException("Could not parse number <" + literal + ">");
        }
      }
    }

    internal static SDLDateTime ParseDateTime(string literal)
    {
      int length1 = literal.IndexOf(' ');
      if (length1 == -1)
        return Parser.ParseDate(literal);
      SDLDateTime sdlDateTime = Parser.ParseDate(literal.Substring(0, length1));
      string str1 = literal.Substring(length1 + 1);
      int length2 = str1.IndexOf('-');
      string timeZone = (string) null;
      if (length2 != -1)
      {
        timeZone = str1.Substring(length2 + 1);
        str1 = str1.Substring(0, length2);
      }
      string[] strArray = str1.Split(new char[1]
      {
        ':'
      });
      if (strArray.Length < 2 || strArray.Length > 3)
        throw new FormatException("Malformed time component in date/time literal.  Must use hh:mm(:ss)(.xxx)");
      int second = 0;
      int millisecond = 0;
      int hour;
      int minute;
      try
      {
        hour = Convert.ToInt32(strArray[0]);
        minute = Convert.ToInt32(strArray[1]);
        if (strArray.Length == 3)
        {
          string str2 = strArray[2];
          int length3 = str2.IndexOf('.');
          if (length3 == -1)
          {
            second = Convert.ToInt32(str2);
          }
          else
          {
            second = Convert.ToInt32(str2.Substring(0, length3));
            string str3 = str2.Substring(length3 + 1);
            if (str3.Length == 1)
              str3 = str3 + "00";
            else if (str3.Length == 2)
              str3 = str3 + "0";
            millisecond = Convert.ToInt32(str3);
          }
        }
      }
      catch (FormatException ex)
      {
        throw new FormatException("Number format exception in time portion of date/time literal \"" + ex.Message + "\"");
      }
      return new SDLDateTime(sdlDateTime.Year, sdlDateTime.Month, sdlDateTime.Day, hour, minute, second, millisecond, timeZone);
    }

    internal static SDLDateTime ParseDate(string literal)
    {
      string[] strArray = literal.Split(new char[1]
      {
        '/'
      });
      if (strArray.Length != 3)
        throw new FormatException("Malformed Date <" + literal + ">");
      try
      {
        return new SDLDateTime(Convert.ToInt32(strArray[0]), Convert.ToInt32(strArray[1]), Convert.ToInt32(strArray[2]));
      }
      catch (FormatException ex)
      {
        throw new FormatException("Number format exception in date literal \"" + ex.Message + "\"");
      }
    }

    internal static byte[] ParseBinary(string literal)
    {
      string str = literal.Substring(1, literal.Length - 2);
      StringBuilder stringBuilder = new StringBuilder();
      int length = str.Length;
      for (int index = 0; index < length; ++index)
      {
        char ch = str[index];
        if ("\n\r\t ".IndexOf(ch) == -1)
          stringBuilder.Append(ch);
      }
      return Convert.FromBase64String(((object) stringBuilder).ToString());
    }

    internal static TimeSpan ParseTimeSpan(string literal)
    {
      int days = 0;
      int num1 = 0;
      int num2 = 0;
      string[] strArray = literal.Split(new char[1]
      {
        ':'
      });
      if (strArray.Length >= 3)
      {
        if (strArray.Length <= 4)
        {
          int num3;
          int num4;
          try
          {
            if (strArray.Length == 4)
            {
              string str1 = strArray[0];
              if (!str1.EndsWith("d"))
              {
                FormatException formatException = new FormatException("The day component of a time span must end with a lower case d");
              }
              days = Convert.ToInt32(str1.Substring(0, str1.Length - 1));
              num3 = Convert.ToInt32(strArray[1]);
              num4 = Convert.ToInt32(strArray[2]);
              if (strArray.Length == 4)
              {
                string str2 = strArray[3];
                int length = str2.IndexOf('.');
                if (length == -1)
                {
                  num1 = Convert.ToInt32(str2);
                }
                else
                {
                  num1 = Convert.ToInt32(str2.Substring(0, length));
                  string str3 = str2.Substring(length + 1);
                  if (str3.Length == 1)
                    str3 = str3 + "00";
                  else if (str3.Length == 2)
                    str3 = str3 + "0";
                  num2 = Convert.ToInt32(str3);
                }
              }
              if (days < 0)
              {
                num3 = Parser.ReverseIfPositive(num3);
                num4 = Parser.ReverseIfPositive(num4);
                num1 = Parser.ReverseIfPositive(num1);
                num2 = Parser.ReverseIfPositive(num2);
              }
            }
            else
            {
              num3 = Convert.ToInt32(strArray[0]);
              num4 = Convert.ToInt32(strArray[1]);
              string str1 = strArray[2];
              int length = str1.IndexOf(".");
              if (length == -1)
              {
                num1 = Convert.ToInt32(str1);
              }
              else
              {
                num1 = Convert.ToInt32(str1.Substring(0, length));
                string str2 = str1.Substring(length + 1);
                if (str2.Length == 1)
                  str2 = str2 + "00";
                else if (str2.Length == 2)
                  str2 = str2 + "0";
                num2 = Convert.ToInt32(str2);
              }
              if (num3 < 0)
              {
                num4 = Parser.ReverseIfPositive(num4);
                num1 = Parser.ReverseIfPositive(num1);
                num2 = Parser.ReverseIfPositive(num2);
              }
            }
          }
          catch (FormatException ex)
          {
            throw new FormatException("Number format in time span exception: \"" + ex.Message + "\" for literal <" + literal + ">");
          }
          return new TimeSpan(days, num3, num4, num1, num2);
        }
      }
      throw new FormatException("Malformed time span <" + literal + ">.  Time spans must use the format (d:)hh:mm:ss(.xxx) Note: if the day component is included it must be suffixed with lower case \"d\"");
    }
  }
}
