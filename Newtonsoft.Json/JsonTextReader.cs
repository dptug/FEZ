// Type: Newtonsoft.Json.JsonTextReader
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace Newtonsoft.Json
{
  public class JsonTextReader : JsonReader, IJsonLineInfo
  {
    private const char UnicodeReplacementChar = '�';
    private readonly TextReader _reader;
    private char[] _chars;
    private int _charsUsed;
    private int _charPos;
    private int _lineStartPos;
    private int _lineNumber;
    private bool _isEndOfFile;
    private StringBuffer _buffer;
    private StringReference _stringReference;

    public int LineNumber
    {
      get
      {
        if (this.CurrentState == JsonReader.State.Start && this.LinePosition == 0)
          return 0;
        else
          return this._lineNumber;
      }
    }

    public int LinePosition
    {
      get
      {
        return this._charPos - this._lineStartPos;
      }
    }

    public JsonTextReader(TextReader reader)
    {
      if (reader == null)
        throw new ArgumentNullException("reader");
      this._reader = reader;
      this._lineNumber = 1;
      this._chars = new char[4097];
    }

    internal void SetCharBuffer(char[] chars)
    {
      this._chars = chars;
    }

    private StringBuffer GetBuffer()
    {
      if (this._buffer == null)
        this._buffer = new StringBuffer(4096);
      else
        this._buffer.Position = 0;
      return this._buffer;
    }

    private void OnNewLine(int pos)
    {
      ++this._lineNumber;
      this._lineStartPos = pos - 1;
    }

    private void ParseString(char quote)
    {
      ++this._charPos;
      this.ShiftBufferIfNeeded();
      this.ReadStringIntoBuffer(quote);
      if (this._readType == ReadType.ReadAsBytes)
        this.SetToken(JsonToken.Bytes, this._stringReference.Length != 0 ? (object) Convert.FromBase64CharArray(this._stringReference.Chars, this._stringReference.StartIndex, this._stringReference.Length) : (object) new byte[0]);
      else if (this._readType == ReadType.ReadAsString)
      {
        this.SetToken(JsonToken.String, (object) this._stringReference.ToString());
        this.QuoteChar = quote;
      }
      else
      {
        string text = this._stringReference.ToString();
        if (this._dateParseHandling != DateParseHandling.None && text.Length > 0)
        {
          if ((int) text[0] == 47)
          {
            if (text.StartsWith("/Date(", StringComparison.Ordinal) && text.EndsWith(")/", StringComparison.Ordinal))
            {
              this.ParseDateMicrosoft(text);
              return;
            }
          }
          else if (char.IsDigit(text[0]) && text.Length >= 19 && (text.Length <= 40 && this.ParseDateIso(text)))
            return;
        }
        this.SetToken(JsonToken.String, (object) text);
        this.QuoteChar = quote;
      }
    }

    private bool ParseDateIso(string text)
    {
      DateTime result;
      if (!DateTime.TryParseExact(text, "yyyy-MM-ddTHH:mm:ss.FFFFFFFK", (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out result))
        return false;
      this.SetToken(JsonToken.Date, (object) JsonConvert.EnsureDateTime(result, this.DateTimeZoneHandling));
      return true;
    }

    private void ParseDateMicrosoft(string text)
    {
      string s = text.Substring(6, text.Length - 8);
      DateTimeKind dateTimeKind = DateTimeKind.Utc;
      int num = s.IndexOf('+', 1);
      if (num == -1)
        num = s.IndexOf('-', 1);
      TimeSpan timeSpan = TimeSpan.Zero;
      if (num != -1)
      {
        dateTimeKind = DateTimeKind.Local;
        JsonTextReader.ReadOffset(s.Substring(num));
        s = s.Substring(0, num);
      }
      DateTime dateTime1 = JsonConvert.ConvertJavaScriptTicksToDateTime(long.Parse(s, NumberStyles.Integer, (IFormatProvider) CultureInfo.InvariantCulture));
      DateTime dateTime2;
      switch (dateTimeKind)
      {
        case DateTimeKind.Unspecified:
          dateTime2 = DateTime.SpecifyKind(dateTime1.ToLocalTime(), DateTimeKind.Unspecified);
          break;
        case DateTimeKind.Local:
          dateTime2 = dateTime1.ToLocalTime();
          break;
        default:
          dateTime2 = dateTime1;
          break;
      }
      this.SetToken(JsonToken.Date, (object) JsonConvert.EnsureDateTime(dateTime2, this.DateTimeZoneHandling));
    }

    private static void BlockCopyChars(char[] src, int srcOffset, char[] dst, int dstOffset, int count)
    {
      Buffer.BlockCopy((Array) src, srcOffset * 2, (Array) dst, dstOffset * 2, count * 2);
    }

    private void ShiftBufferIfNeeded()
    {
      int length = this._chars.Length;
      if ((double) (length - this._charPos) > (double) length * 0.1)
        return;
      int count = this._charsUsed - this._charPos;
      if (count > 0)
        JsonTextReader.BlockCopyChars(this._chars, this._charPos, this._chars, 0, count);
      this._lineStartPos -= this._charPos;
      this._charPos = 0;
      this._charsUsed = count;
      this._chars[this._charsUsed] = char.MinValue;
    }

    private int ReadData(bool append)
    {
      return this.ReadData(append, 0);
    }

    private int ReadData(bool append, int charsRequired)
    {
      if (this._isEndOfFile)
        return 0;
      if (this._charsUsed + charsRequired >= this._chars.Length - 1)
      {
        if (append)
        {
          char[] dst = new char[Math.Max(this._chars.Length * 2, this._charsUsed + charsRequired + 1)];
          JsonTextReader.BlockCopyChars(this._chars, 0, dst, 0, this._chars.Length);
          this._chars = dst;
        }
        else
        {
          int count = this._charsUsed - this._charPos;
          if (count + charsRequired + 1 >= this._chars.Length)
          {
            char[] dst = new char[count + charsRequired + 1];
            if (count > 0)
              JsonTextReader.BlockCopyChars(this._chars, this._charPos, dst, 0, count);
            this._chars = dst;
          }
          else if (count > 0)
            JsonTextReader.BlockCopyChars(this._chars, this._charPos, this._chars, 0, count);
          this._lineStartPos -= this._charPos;
          this._charPos = 0;
          this._charsUsed = count;
        }
      }
      int num = this._reader.Read(this._chars, this._charsUsed, this._chars.Length - this._charsUsed - 1);
      this._charsUsed += num;
      if (num == 0)
        this._isEndOfFile = true;
      this._chars[this._charsUsed] = char.MinValue;
      return num;
    }

    private bool EnsureChars(int relativePosition, bool append)
    {
      if (this._charPos + relativePosition >= this._charsUsed)
        return this.ReadChars(relativePosition, append);
      else
        return true;
    }

    private bool ReadChars(int relativePosition, bool append)
    {
      if (this._isEndOfFile)
        return false;
      int num1 = this._charPos + relativePosition - this._charsUsed + 1;
      int num2 = 0;
      do
      {
        int num3 = this.ReadData(append, num1 - num2);
        if (num3 != 0)
          num2 += num3;
        else
          break;
      }
      while (num2 < num1);
      return num2 >= num1;
    }

    private static TimeSpan ReadOffset(string offsetText)
    {
      bool flag = (int) offsetText[0] == 45;
      int num1 = int.Parse(offsetText.Substring(1, 2), NumberStyles.Integer, (IFormatProvider) CultureInfo.InvariantCulture);
      int num2 = 0;
      if (offsetText.Length >= 5)
        num2 = int.Parse(offsetText.Substring(3, 2), NumberStyles.Integer, (IFormatProvider) CultureInfo.InvariantCulture);
      TimeSpan timeSpan = TimeSpan.FromHours((double) num1) + TimeSpan.FromMinutes((double) num2);
      if (flag)
        timeSpan = timeSpan.Negate();
      return timeSpan;
    }

    [DebuggerStepThrough]
    public override bool Read()
    {
      this._readType = ReadType.Read;
      if (this.ReadInternal())
        return true;
      this.SetToken(JsonToken.None);
      return false;
    }

    public override byte[] ReadAsBytes()
    {
      return this.ReadAsBytesInternal();
    }

    public override Decimal? ReadAsDecimal()
    {
      return this.ReadAsDecimalInternal();
    }

    public override int? ReadAsInt32()
    {
      return this.ReadAsInt32Internal();
    }

    public override string ReadAsString()
    {
      return this.ReadAsStringInternal();
    }

    public override DateTime? ReadAsDateTime()
    {
      return this.ReadAsDateTimeInternal();
    }

    internal override bool ReadInternal()
    {
      do
      {
        switch (this._currentState)
        {
          case JsonReader.State.Start:
          case JsonReader.State.Property:
          case JsonReader.State.ArrayStart:
          case JsonReader.State.Array:
          case JsonReader.State.ConstructorStart:
          case JsonReader.State.Constructor:
            return this.ParseValue();
          case JsonReader.State.Complete:
          case JsonReader.State.Closed:
          case JsonReader.State.Error:
            continue;
          case JsonReader.State.ObjectStart:
          case JsonReader.State.Object:
            return this.ParseObject();
          case JsonReader.State.PostValue:
            continue;
          case JsonReader.State.Finished:
            goto label_5;
          default:
            goto label_12;
        }
      }
      while (!this.ParsePostValue());
      return true;
label_5:
      if (!this.EnsureChars(0, false))
        return false;
      this.EatWhitespace(false);
      if (this._isEndOfFile)
        return false;
      if ((int) this._chars[this._charPos] != 47)
        throw JsonReaderException.Create((JsonReader) this, StringUtils.FormatWith("Additional text encountered after finished reading JSON content: {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) this._chars[this._charPos]));
      this.ParseComment();
      return true;
label_12:
      throw JsonReaderException.Create((JsonReader) this, StringUtils.FormatWith("Unexpected state: {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) this.CurrentState));
    }

    private void ReadStringIntoBuffer(char quote)
    {
      int index = this._charPos;
      int startIndex = this._charPos;
      int num1 = this._charPos;
      StringBuffer buffer = (StringBuffer) null;
      do
      {
        char ch1 = this._chars[index++];
        if ((int) ch1 <= 13)
        {
          if ((int) ch1 != 0)
          {
            if ((int) ch1 != 10)
            {
              if ((int) ch1 == 13)
              {
                this._charPos = index - 1;
                this.ProcessCarriageReturn(true);
                index = this._charPos;
              }
            }
            else
            {
              this._charPos = index - 1;
              this.ProcessLineFeed();
              index = this._charPos;
            }
          }
          else if (this._charsUsed == index - 1)
          {
            --index;
            if (this.ReadData(true) == 0)
            {
              this._charPos = index;
              throw JsonReaderException.Create((JsonReader) this, StringUtils.FormatWith("Unterminated string. Expected delimiter: {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) quote));
            }
          }
        }
        else if ((int) ch1 != 34 && (int) ch1 != 39)
        {
          if ((int) ch1 == 92)
          {
            this._charPos = index;
            if (!this.EnsureChars(0, true))
            {
              this._charPos = index;
              throw JsonReaderException.Create((JsonReader) this, StringUtils.FormatWith("Unterminated string. Expected delimiter: {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) quote));
            }
            else
            {
              int writeToPosition = index - 1;
              char ch2 = this._chars[index];
              char ch3;
              switch (ch2)
              {
                case 'n':
                  ++index;
                  ch3 = '\n';
                  break;
                case 'r':
                  ++index;
                  ch3 = '\r';
                  break;
                case 't':
                  ++index;
                  ch3 = '\t';
                  break;
                case 'u':
                  this._charPos = index + 1;
                  ch3 = this.ParseUnicode();
                  if (StringUtils.IsLowSurrogate(ch3))
                    ch3 = '�';
                  else if (StringUtils.IsHighSurrogate(ch3))
                  {
                    bool flag;
                    do
                    {
                      flag = false;
                      if (this.EnsureChars(2, true) && (int) this._chars[this._charPos] == 92 && (int) this._chars[this._charPos + 1] == 117)
                      {
                        char writeChar = ch3;
                        this._charPos += 2;
                        ch3 = this.ParseUnicode();
                        if (!StringUtils.IsLowSurrogate(ch3))
                        {
                          if (StringUtils.IsHighSurrogate(ch3))
                          {
                            writeChar = '�';
                            flag = true;
                          }
                          else
                            writeChar = '�';
                        }
                        if (buffer == null)
                          buffer = this.GetBuffer();
                        this.WriteCharToBuffer(buffer, writeChar, num1, writeToPosition);
                        num1 = this._charPos;
                      }
                      else
                        ch3 = '�';
                    }
                    while (flag);
                  }
                  index = this._charPos;
                  break;
                case 'b':
                  ++index;
                  ch3 = '\b';
                  break;
                case 'f':
                  ++index;
                  ch3 = '\f';
                  break;
                case '/':
                case '"':
                case '\'':
                  ch3 = ch2;
                  ++index;
                  break;
                case '\\':
                  ++index;
                  ch3 = '\\';
                  break;
                default:
                  this._charPos = index + 1;
                  throw JsonReaderException.Create((JsonReader) this, StringUtils.FormatWith("Bad JSON escape sequence: {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) ("\\" + (object) ch2)));
              }
              if (buffer == null)
                buffer = this.GetBuffer();
              this.WriteCharToBuffer(buffer, ch3, num1, writeToPosition);
              num1 = index;
            }
          }
        }
      }
      while ((int) this._chars[index - 1] != (int) quote);
      int num2 = index - 1;
      if (startIndex == num1)
      {
        this._stringReference = new StringReference(this._chars, startIndex, num2 - startIndex);
      }
      else
      {
        if (buffer == null)
          buffer = this.GetBuffer();
        if (num2 > num1)
          buffer.Append(this._chars, num1, num2 - num1);
        this._stringReference = new StringReference(buffer.GetInternalBuffer(), 0, buffer.Position);
      }
      this._charPos = num2 + 1;
    }

    private void WriteCharToBuffer(StringBuffer buffer, char writeChar, int lastWritePosition, int writeToPosition)
    {
      if (writeToPosition > lastWritePosition)
        buffer.Append(this._chars, lastWritePosition, writeToPosition - lastWritePosition);
      buffer.Append(writeChar);
    }

    private char ParseUnicode()
    {
      if (!this.EnsureChars(4, true))
        throw JsonReaderException.Create((JsonReader) this, "Unexpected end while parsing unicode character.");
      char ch = Convert.ToChar(int.Parse(new string(this._chars, this._charPos, 4), NumberStyles.HexNumber, (IFormatProvider) NumberFormatInfo.InvariantInfo));
      this._charPos += 4;
      return ch;
    }

    private void ReadNumberIntoBuffer()
    {
      int num = this._charPos;
      do
      {
        do
        {
          char ch;
          do
          {
            do
            {
              ch = this._chars[num++];
              if ((int) ch <= 70)
              {
                if ((int) ch != 0)
                {
                  switch ((int) ch - 43)
                  {
                    case 0:
                    case 2:
                    case 3:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                    case 10:
                    case 11:
                    case 12:
                    case 13:
                    case 14:
                    case 22:
                    case 23:
                    case 24:
                    case 25:
                    case 26:
                    case 27:
                      continue;
                    default:
                      goto label_10;
                  }
                }
                else
                  goto label_8;
              }
            }
            while ((int) ch == 88);
            switch ((int) ch - 97)
            {
              case 0:
              case 1:
              case 2:
              case 3:
              case 4:
              case 5:
                continue;
              default:
                continue;
            }
          }
          while ((int) ch == 120);
          goto label_10;
label_8:;
        }
        while (this._charsUsed != num - 1);
        --num;
        this._charPos = num;
      }
      while (this.ReadData(true) != 0);
      return;
label_10:
      this._charPos = num - 1;
    }

    private void ClearRecentString()
    {
      if (this._buffer != null)
        this._buffer.Position = 0;
      this._stringReference = new StringReference();
    }

    private bool ParsePostValue()
    {
      char c;
      while (true)
      {
        do
        {
          c = this._chars[this._charPos];
          switch (c)
          {
            case ']':
              goto label_6;
            case '}':
              goto label_5;
            case ',':
              goto label_9;
            case '/':
              goto label_8;
            case ' ':
            case '\t':
              goto label_10;
            case ')':
              goto label_7;
            case char.MinValue:
              if (this._charsUsed == this._charPos)
                continue;
              else
                goto label_4;
            case '\n':
              goto label_12;
            case '\r':
              goto label_11;
            default:
              goto label_13;
          }
        }
        while (this.ReadData(false) != 0);
        break;
label_4:
        ++this._charPos;
        continue;
label_10:
        ++this._charPos;
        continue;
label_11:
        this.ProcessCarriageReturn(false);
        continue;
label_12:
        this.ProcessLineFeed();
        continue;
label_13:
        if (char.IsWhiteSpace(c))
          ++this._charPos;
        else
          goto label_15;
      }
      this._currentState = JsonReader.State.Finished;
      return false;
label_5:
      ++this._charPos;
      this.SetToken(JsonToken.EndObject);
      return true;
label_6:
      ++this._charPos;
      this.SetToken(JsonToken.EndArray);
      return true;
label_7:
      ++this._charPos;
      this.SetToken(JsonToken.EndConstructor);
      return true;
label_8:
      this.ParseComment();
      return true;
label_9:
      ++this._charPos;
      this.SetStateBasedOnCurrent();
      return false;
label_15:
      throw JsonReaderException.Create((JsonReader) this, StringUtils.FormatWith("After parsing a value an unexpected character was encountered: {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) c));
    }

    private bool ParseObject()
    {
      while (true)
      {
        char c;
        do
        {
          c = this._chars[this._charPos];
          switch (c)
          {
            case ' ':
            case '\t':
              goto label_9;
            case '/':
              goto label_6;
            case '}':
              goto label_5;
            case char.MinValue:
              if (this._charsUsed == this._charPos)
                continue;
              else
                goto label_4;
            case '\n':
              goto label_8;
            case '\r':
              goto label_7;
            default:
              goto label_10;
          }
        }
        while (this.ReadData(false) != 0);
        break;
label_4:
        ++this._charPos;
        continue;
label_7:
        this.ProcessCarriageReturn(false);
        continue;
label_8:
        this.ProcessLineFeed();
        continue;
label_9:
        ++this._charPos;
        continue;
label_10:
        if (char.IsWhiteSpace(c))
          ++this._charPos;
        else
          goto label_12;
      }
      return false;
label_5:
      this.SetToken(JsonToken.EndObject);
      ++this._charPos;
      return true;
label_6:
      this.ParseComment();
      return true;
label_12:
      return this.ParseProperty();
    }

    private bool ParseProperty()
    {
      char ch = this._chars[this._charPos];
      char quote;
      switch (ch)
      {
        case '"':
        case '\'':
          ++this._charPos;
          quote = ch;
          this.ShiftBufferIfNeeded();
          this.ReadStringIntoBuffer(quote);
          break;
        default:
          if (!this.ValidIdentifierChar(ch))
            throw JsonReaderException.Create((JsonReader) this, StringUtils.FormatWith("Invalid property identifier character: {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) this._chars[this._charPos]));
          quote = char.MinValue;
          this.ShiftBufferIfNeeded();
          this.ParseUnquotedProperty();
          break;
      }
      string str = this._stringReference.ToString();
      this.EatWhitespace(false);
      if ((int) this._chars[this._charPos] != 58)
        throw JsonReaderException.Create((JsonReader) this, StringUtils.FormatWith("Invalid character after parsing property name. Expected ':' but got: {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) this._chars[this._charPos]));
      ++this._charPos;
      this.SetToken(JsonToken.PropertyName, (object) str);
      this.QuoteChar = quote;
      this.ClearRecentString();
      return true;
    }

    private bool ValidIdentifierChar(char value)
    {
      if (!char.IsLetterOrDigit(value) && (int) value != 95)
        return (int) value == 36;
      else
        return true;
    }

    private void ParseUnquotedProperty()
    {
      int startIndex = this._charPos;
      do
      {
        for (; (int) this._chars[this._charPos] != 0; ++this._charPos)
        {
          char c = this._chars[this._charPos];
          if (!this.ValidIdentifierChar(c))
          {
            if (!char.IsWhiteSpace(c) && (int) c != 58)
              throw JsonReaderException.Create((JsonReader) this, StringUtils.FormatWith("Invalid JavaScript property identifier character: {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) c));
            this._stringReference = new StringReference(this._chars, startIndex, this._charPos - startIndex);
            return;
          }
        }
        if (this._charsUsed != this._charPos)
          goto label_5;
      }
      while (this.ReadData(true) != 0);
      throw JsonReaderException.Create((JsonReader) this, "Unexpected end while parsing unquoted property name.");
label_5:
      this._stringReference = new StringReference(this._chars, startIndex, this._charPos - startIndex);
    }

    private bool ParseValue()
    {
      char ch;
      while (true)
      {
        do
        {
          ch = this._chars[this._charPos];
          switch (ch)
          {
            case 'n':
              goto label_8;
            case 't':
              goto label_6;
            case 'u':
              goto label_22;
            case '{':
              goto label_23;
            case 'N':
              goto label_15;
            case '[':
              goto label_24;
            case ']':
              goto label_25;
            case 'f':
              goto label_7;
            case ' ':
            case '\t':
              goto label_30;
            case '"':
            case '\'':
              goto label_5;
            case ')':
              goto label_27;
            case ',':
              goto label_26;
            case '-':
              goto label_17;
            case '/':
              goto label_21;
            case 'I':
              goto label_16;
            case char.MinValue:
              if (this._charsUsed == this._charPos)
                continue;
              else
                goto label_4;
            case '\n':
              goto label_29;
            case '\r':
              goto label_28;
            default:
              goto label_31;
          }
        }
        while (this.ReadData(false) != 0);
        break;
label_4:
        ++this._charPos;
        continue;
label_28:
        this.ProcessCarriageReturn(false);
        continue;
label_29:
        this.ProcessLineFeed();
        continue;
label_30:
        ++this._charPos;
        continue;
label_31:
        if (char.IsWhiteSpace(ch))
          ++this._charPos;
        else
          goto label_33;
      }
      return false;
label_5:
      this.ParseString(ch);
      return true;
label_6:
      this.ParseTrue();
      return true;
label_7:
      this.ParseFalse();
      return true;
label_8:
      if (!this.EnsureChars(1, true))
        throw JsonReaderException.Create((JsonReader) this, "Unexpected end.");
      switch (this._chars[this._charPos + 1])
      {
        case 'u':
          this.ParseNull();
          break;
        case 'e':
          this.ParseConstructor();
          break;
        default:
          throw JsonReaderException.Create((JsonReader) this, StringUtils.FormatWith("Unexpected character encountered while parsing value: {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) this._chars[this._charPos]));
      }
      return true;
label_15:
      this.ParseNumberNaN();
      return true;
label_16:
      this.ParseNumberPositiveInfinity();
      return true;
label_17:
      if (this.EnsureChars(1, true) && (int) this._chars[this._charPos + 1] == 73)
        this.ParseNumberNegativeInfinity();
      else
        this.ParseNumber();
      return true;
label_21:
      this.ParseComment();
      return true;
label_22:
      this.ParseUndefined();
      return true;
label_23:
      ++this._charPos;
      this.SetToken(JsonToken.StartObject);
      return true;
label_24:
      ++this._charPos;
      this.SetToken(JsonToken.StartArray);
      return true;
label_25:
      ++this._charPos;
      this.SetToken(JsonToken.EndArray);
      return true;
label_26:
      this.SetToken(JsonToken.Undefined);
      return true;
label_27:
      ++this._charPos;
      this.SetToken(JsonToken.EndConstructor);
      return true;
label_33:
      if (!char.IsNumber(ch) && (int) ch != 45 && (int) ch != 46)
        throw JsonReaderException.Create((JsonReader) this, StringUtils.FormatWith("Unexpected character encountered while parsing value: {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) ch));
      this.ParseNumber();
      return true;
    }

    private void ProcessLineFeed()
    {
      ++this._charPos;
      this.OnNewLine(this._charPos);
    }

    private void ProcessCarriageReturn(bool append)
    {
      ++this._charPos;
      if (this.EnsureChars(1, append) && (int) this._chars[this._charPos] == 10)
        ++this._charPos;
      this.OnNewLine(this._charPos);
    }

    private bool EatWhitespace(bool oneOrMore)
    {
      bool flag1 = false;
      bool flag2 = false;
      while (!flag1)
      {
        char c = this._chars[this._charPos];
        switch (c)
        {
          case char.MinValue:
            if (this._charsUsed == this._charPos)
            {
              if (this.ReadData(false) == 0)
              {
                flag1 = true;
                continue;
              }
              else
                continue;
            }
            else
            {
              ++this._charPos;
              continue;
            }
          case '\n':
            this.ProcessLineFeed();
            continue;
          case '\r':
            this.ProcessCarriageReturn(false);
            continue;
          default:
            if ((int) c == 32 || char.IsWhiteSpace(c))
            {
              flag2 = true;
              ++this._charPos;
              continue;
            }
            else
            {
              flag1 = true;
              continue;
            }
        }
      }
      if (oneOrMore)
        return flag2;
      else
        return true;
    }

    private void ParseConstructor()
    {
      if (!this.MatchValueWithTrailingSeperator("new"))
        return;
      this.EatWhitespace(false);
      int startIndex = this._charPos;
      char c;
      while (true)
      {
        do
        {
          c = this._chars[this._charPos];
          if ((int) c == 0)
          {
            if (this._charsUsed != this._charPos)
              goto label_6;
          }
          else
            goto label_7;
        }
        while (this.ReadData(true) != 0);
        break;
label_7:
        if (char.IsLetterOrDigit(c))
          ++this._charPos;
        else
          goto label_9;
      }
      throw JsonReaderException.Create((JsonReader) this, "Unexpected end while parsing constructor.");
label_6:
      int num = this._charPos;
      ++this._charPos;
      goto label_18;
label_9:
      if ((int) c == 13)
      {
        num = this._charPos;
        this.ProcessCarriageReturn(true);
      }
      else if ((int) c == 10)
      {
        num = this._charPos;
        this.ProcessLineFeed();
      }
      else if (char.IsWhiteSpace(c))
      {
        num = this._charPos;
        ++this._charPos;
      }
      else
      {
        if ((int) c != 40)
          throw JsonReaderException.Create((JsonReader) this, StringUtils.FormatWith("Unexpected character while parsing constructor: {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) c));
        num = this._charPos;
      }
label_18:
      this._stringReference = new StringReference(this._chars, startIndex, num - startIndex);
      string str = this._stringReference.ToString();
      this.EatWhitespace(false);
      if ((int) this._chars[this._charPos] != 40)
        throw JsonReaderException.Create((JsonReader) this, StringUtils.FormatWith("Unexpected character while parsing constructor: {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) this._chars[this._charPos]));
      ++this._charPos;
      this.ClearRecentString();
      this.SetToken(JsonToken.StartConstructor, (object) str);
    }

    private void ParseNumber()
    {
      this.ShiftBufferIfNeeded();
      char c = this._chars[this._charPos];
      int startIndex = this._charPos;
      this.ReadNumberIntoBuffer();
      this._stringReference = new StringReference(this._chars, startIndex, this._charPos - startIndex);
      bool flag1 = char.IsDigit(c) && this._stringReference.Length == 1;
      bool flag2 = (int) c == 48 && this._stringReference.Length > 1 && ((int) this._stringReference.Chars[this._stringReference.StartIndex + 1] != 46 && (int) this._stringReference.Chars[this._stringReference.StartIndex + 1] != 101) && (int) this._stringReference.Chars[this._stringReference.StartIndex + 1] != 69;
      object obj;
      JsonToken newToken;
      if (this._readType == ReadType.ReadAsInt32)
      {
        if (flag1)
          obj = (object) ((int) c - 48);
        else if (flag2)
        {
          string str = this._stringReference.ToString();
          obj = (object) (str.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? Convert.ToInt32(str, 16) : Convert.ToInt32(str, 8));
        }
        else
          obj = (object) Convert.ToInt32(this._stringReference.ToString(), (IFormatProvider) CultureInfo.InvariantCulture);
        newToken = JsonToken.Integer;
      }
      else if (this._readType == ReadType.ReadAsDecimal)
      {
        if (flag1)
          obj = (object) ((Decimal) c - new Decimal(48));
        else if (flag2)
        {
          string str = this._stringReference.ToString();
          obj = (object) Convert.ToDecimal(str.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? Convert.ToInt64(str, 16) : Convert.ToInt64(str, 8));
        }
        else
          obj = (object) Decimal.Parse(this._stringReference.ToString(), NumberStyles.Number | NumberStyles.AllowExponent, (IFormatProvider) CultureInfo.InvariantCulture);
        newToken = JsonToken.Float;
      }
      else if (flag1)
      {
        obj = (object) ((long) c - 48L);
        newToken = JsonToken.Integer;
      }
      else if (flag2)
      {
        string str = this._stringReference.ToString();
        obj = (object) (str.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? Convert.ToInt64(str, 16) : Convert.ToInt64(str, 8));
        newToken = JsonToken.Integer;
      }
      else
      {
        string str = this._stringReference.ToString();
        if (str.IndexOf('.') == -1 && str.IndexOf('E') == -1)
        {
          if (str.IndexOf('e') == -1)
          {
            try
            {
              obj = (object) Convert.ToInt64(str, (IFormatProvider) CultureInfo.InvariantCulture);
            }
            catch (OverflowException ex)
            {
              throw JsonReaderException.Create((JsonReader) this, StringUtils.FormatWith("JSON integer {0} is too large or small for an Int64.", (IFormatProvider) CultureInfo.InvariantCulture, (object) str), (Exception) ex);
            }
            newToken = JsonToken.Integer;
            goto label_24;
          }
        }
        obj = (object) Convert.ToDouble(str, (IFormatProvider) CultureInfo.InvariantCulture);
        newToken = JsonToken.Float;
      }
label_24:
      this.ClearRecentString();
      this.SetToken(newToken, obj);
    }

    private void ParseComment()
    {
      ++this._charPos;
      if (!this.EnsureChars(1, false) || (int) this._chars[this._charPos] != 42)
        throw JsonReaderException.Create((JsonReader) this, StringUtils.FormatWith("Error parsing comment. Expected: *, got {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) this._chars[this._charPos]));
      ++this._charPos;
      int startIndex = this._charPos;
      bool flag = false;
      while (!flag)
      {
        switch (this._chars[this._charPos])
        {
          case '\r':
            this.ProcessCarriageReturn(true);
            continue;
          case '*':
            ++this._charPos;
            if (this.EnsureChars(0, true) && (int) this._chars[this._charPos] == 47)
            {
              this._stringReference = new StringReference(this._chars, startIndex, this._charPos - startIndex - 1);
              ++this._charPos;
              flag = true;
              continue;
            }
            else
              continue;
          case char.MinValue:
            if (this._charsUsed == this._charPos)
            {
              if (this.ReadData(true) == 0)
                throw JsonReaderException.Create((JsonReader) this, "Unexpected end while parsing comment.");
              else
                continue;
            }
            else
            {
              ++this._charPos;
              continue;
            }
          case '\n':
            this.ProcessLineFeed();
            continue;
          default:
            ++this._charPos;
            continue;
        }
      }
      this.SetToken(JsonToken.Comment, (object) this._stringReference.ToString());
      this.ClearRecentString();
    }

    private bool MatchValue(string value)
    {
      if (!this.EnsureChars(value.Length - 1, true))
        return false;
      for (int index = 0; index < value.Length; ++index)
      {
        if ((int) this._chars[this._charPos + index] != (int) value[index])
          return false;
      }
      this._charPos += value.Length;
      return true;
    }

    private bool MatchValueWithTrailingSeperator(string value)
    {
      if (!this.MatchValue(value))
        return false;
      if (!this.EnsureChars(0, false) || this.IsSeperator(this._chars[this._charPos]))
        return true;
      else
        return (int) this._chars[this._charPos] == 0;
    }

    private bool IsSeperator(char c)
    {
      switch (c)
      {
        case ']':
        case '}':
        case ',':
          return true;
        case '/':
          if (!this.EnsureChars(1, false))
            return false;
          else
            return (int) this._chars[this._charPos + 1] == 42;
        case '\t':
        case '\n':
        case '\r':
        case ' ':
          return true;
        case ')':
          if (this.CurrentState == JsonReader.State.Constructor || this.CurrentState == JsonReader.State.ConstructorStart)
            return true;
          else
            break;
        default:
          if (char.IsWhiteSpace(c))
            return true;
          else
            break;
      }
      return false;
    }

    private void ParseTrue()
    {
      if (!this.MatchValueWithTrailingSeperator(JsonConvert.True))
        throw JsonReaderException.Create((JsonReader) this, "Error parsing boolean value.");
      this.SetToken(JsonToken.Boolean, (object) true);
    }

    private void ParseNull()
    {
      if (!this.MatchValueWithTrailingSeperator(JsonConvert.Null))
        throw JsonReaderException.Create((JsonReader) this, "Error parsing null value.");
      this.SetToken(JsonToken.Null);
    }

    private void ParseUndefined()
    {
      if (!this.MatchValueWithTrailingSeperator(JsonConvert.Undefined))
        throw JsonReaderException.Create((JsonReader) this, "Error parsing undefined value.");
      this.SetToken(JsonToken.Undefined);
    }

    private void ParseFalse()
    {
      if (!this.MatchValueWithTrailingSeperator(JsonConvert.False))
        throw JsonReaderException.Create((JsonReader) this, "Error parsing boolean value.");
      this.SetToken(JsonToken.Boolean, (object) false);
    }

    private void ParseNumberNegativeInfinity()
    {
      if (!this.MatchValueWithTrailingSeperator(JsonConvert.NegativeInfinity))
        throw JsonReaderException.Create((JsonReader) this, "Error parsing negative infinity value.");
      this.SetToken(JsonToken.Float, (object) double.NegativeInfinity);
    }

    private void ParseNumberPositiveInfinity()
    {
      if (!this.MatchValueWithTrailingSeperator(JsonConvert.PositiveInfinity))
        throw JsonReaderException.Create((JsonReader) this, "Error parsing positive infinity value.");
      this.SetToken(JsonToken.Float, (object) double.PositiveInfinity);
    }

    private void ParseNumberNaN()
    {
      if (!this.MatchValueWithTrailingSeperator(JsonConvert.NaN))
        throw JsonReaderException.Create((JsonReader) this, "Error parsing NaN value.");
      this.SetToken(JsonToken.Float, (object) double.NaN);
    }

    public override void Close()
    {
      base.Close();
      if (this.CloseInput && this._reader != null)
        this._reader.Close();
      if (this._buffer == null)
        return;
      this._buffer.Clear();
    }

    public bool HasLineInfo()
    {
      return true;
    }
  }
}
