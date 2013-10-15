// Type: Newtonsoft.Json.JsonTextWriter
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.IO;

namespace Newtonsoft.Json
{
  public class JsonTextWriter : JsonWriter
  {
    private readonly TextWriter _writer;
    private Base64Encoder _base64Encoder;
    private char _indentChar;
    private int _indentation;
    private char _quoteChar;
    private bool _quoteName;

    private Base64Encoder Base64Encoder
    {
      get
      {
        if (this._base64Encoder == null)
          this._base64Encoder = new Base64Encoder(this._writer);
        return this._base64Encoder;
      }
    }

    public int Indentation
    {
      get
      {
        return this._indentation;
      }
      set
      {
        if (value < 0)
          throw new ArgumentException("Indentation value must be greater than 0.");
        this._indentation = value;
      }
    }

    public char QuoteChar
    {
      get
      {
        return this._quoteChar;
      }
      set
      {
        if ((int) value != 34 && (int) value != 39)
          throw new ArgumentException("Invalid JavaScript string quote character. Valid quote characters are ' and \".");
        this._quoteChar = value;
      }
    }

    public char IndentChar
    {
      get
      {
        return this._indentChar;
      }
      set
      {
        this._indentChar = value;
      }
    }

    public bool QuoteName
    {
      get
      {
        return this._quoteName;
      }
      set
      {
        this._quoteName = value;
      }
    }

    public JsonTextWriter(TextWriter textWriter)
    {
      if (textWriter == null)
        throw new ArgumentNullException("textWriter");
      this._writer = textWriter;
      this._quoteChar = '"';
      this._quoteName = true;
      this._indentChar = ' ';
      this._indentation = 2;
    }

    public override void Flush()
    {
      this._writer.Flush();
    }

    public override void Close()
    {
      base.Close();
      if (!this.CloseOutput || this._writer == null)
        return;
      this._writer.Close();
    }

    public override void WriteStartObject()
    {
      base.WriteStartObject();
      this._writer.Write("{");
    }

    public override void WriteStartArray()
    {
      base.WriteStartArray();
      this._writer.Write("[");
    }

    public override void WriteStartConstructor(string name)
    {
      base.WriteStartConstructor(name);
      this._writer.Write("new ");
      this._writer.Write(name);
      this._writer.Write("(");
    }

    protected override void WriteEnd(JsonToken token)
    {
      switch (token)
      {
        case JsonToken.EndObject:
          this._writer.Write("}");
          break;
        case JsonToken.EndArray:
          this._writer.Write("]");
          break;
        case JsonToken.EndConstructor:
          this._writer.Write(")");
          break;
        default:
          throw JsonWriterException.Create((JsonWriter) this, "Invalid JsonToken: " + (object) token, (Exception) null);
      }
    }

    public override void WritePropertyName(string name)
    {
      base.WritePropertyName(name);
      JavaScriptUtils.WriteEscapedJavaScriptString(this._writer, name, this._quoteChar, this._quoteName);
      this._writer.Write(':');
    }

    protected override void WriteIndent()
    {
      this._writer.Write(Environment.NewLine);
      int val1 = this.Top * this._indentation;
      while (val1 > 0)
      {
        int count = Math.Min(val1, 10);
        this._writer.Write(new string(this._indentChar, count));
        val1 -= count;
      }
    }

    protected override void WriteValueDelimiter()
    {
      this._writer.Write(',');
    }

    protected override void WriteIndentSpace()
    {
      this._writer.Write(' ');
    }

    private void WriteValueInternal(string value, JsonToken token)
    {
      this._writer.Write(value);
    }

    public override void WriteNull()
    {
      base.WriteNull();
      this.WriteValueInternal(JsonConvert.Null, JsonToken.Null);
    }

    public override void WriteUndefined()
    {
      base.WriteUndefined();
      this.WriteValueInternal(JsonConvert.Undefined, JsonToken.Undefined);
    }

    public override void WriteRaw(string json)
    {
      base.WriteRaw(json);
      this._writer.Write(json);
    }

    public override void WriteValue(string value)
    {
      base.WriteValue(value);
      if (value == null)
        this.WriteValueInternal(JsonConvert.Null, JsonToken.Null);
      else
        JavaScriptUtils.WriteEscapedJavaScriptString(this._writer, value, this._quoteChar, true);
    }

    public override void WriteValue(int value)
    {
      base.WriteValue(value);
      this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.Integer);
    }

    [CLSCompliant(false)]
    public override void WriteValue(uint value)
    {
      base.WriteValue(value);
      this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.Integer);
    }

    public override void WriteValue(long value)
    {
      base.WriteValue(value);
      this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.Integer);
    }

    [CLSCompliant(false)]
    public override void WriteValue(ulong value)
    {
      base.WriteValue(value);
      this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.Integer);
    }

    public override void WriteValue(float value)
    {
      base.WriteValue(value);
      this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.Float);
    }

    public override void WriteValue(double value)
    {
      base.WriteValue(value);
      this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.Float);
    }

    public override void WriteValue(bool value)
    {
      base.WriteValue(value);
      this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.Boolean);
    }

    public override void WriteValue(short value)
    {
      base.WriteValue(value);
      this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.Integer);
    }

    [CLSCompliant(false)]
    public override void WriteValue(ushort value)
    {
      base.WriteValue(value);
      this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.Integer);
    }

    public override void WriteValue(char value)
    {
      base.WriteValue(value);
      this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.Integer);
    }

    public override void WriteValue(byte value)
    {
      base.WriteValue(value);
      this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.Integer);
    }

    [CLSCompliant(false)]
    public override void WriteValue(sbyte value)
    {
      base.WriteValue(value);
      this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.Integer);
    }

    public override void WriteValue(Decimal value)
    {
      base.WriteValue(value);
      this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.Float);
    }

    public override void WriteValue(DateTime value)
    {
      base.WriteValue(value);
      value = JsonConvert.EnsureDateTime(value, this.DateTimeZoneHandling);
      JsonConvert.WriteDateTimeString(this._writer, value, this.DateFormatHandling);
    }

    public override void WriteValue(byte[] value)
    {
      base.WriteValue(value);
      if (value == null)
        return;
      this._writer.Write(this._quoteChar);
      this.Base64Encoder.Encode(value, 0, value.Length);
      this.Base64Encoder.Flush();
      this._writer.Write(this._quoteChar);
    }

    public override void WriteValue(Guid value)
    {
      base.WriteValue(value);
      this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.String);
    }

    public override void WriteValue(TimeSpan value)
    {
      base.WriteValue(value);
      this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.String);
    }

    public override void WriteValue(Uri value)
    {
      base.WriteValue(value);
      this.WriteValueInternal(JsonConvert.ToString(value), JsonToken.String);
    }

    public override void WriteComment(string text)
    {
      base.WriteComment(text);
      this._writer.Write("/*");
      this._writer.Write(text);
      this._writer.Write("*/");
    }

    public override void WriteWhitespace(string ws)
    {
      base.WriteWhitespace(ws);
      this._writer.Write(ws);
    }
  }
}
