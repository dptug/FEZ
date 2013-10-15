// Type: Newtonsoft.Json.Bson.BsonWriter
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;
using System.IO;

namespace Newtonsoft.Json.Bson
{
  public class BsonWriter : JsonWriter
  {
    private readonly BsonBinaryWriter _writer;
    private BsonToken _root;
    private BsonToken _parent;
    private string _propertyName;

    public DateTimeKind DateTimeKindHandling
    {
      get
      {
        return this._writer.DateTimeKindHandling;
      }
      set
      {
        this._writer.DateTimeKindHandling = value;
      }
    }

    public BsonWriter(Stream stream)
    {
      ValidationUtils.ArgumentNotNull((object) stream, "stream");
      this._writer = new BsonBinaryWriter(new BinaryWriter(stream));
    }

    public BsonWriter(BinaryWriter writer)
    {
      ValidationUtils.ArgumentNotNull((object) writer, "writer");
      this._writer = new BsonBinaryWriter(writer);
    }

    public override void Flush()
    {
      this._writer.Flush();
    }

    protected override void WriteEnd(JsonToken token)
    {
      base.WriteEnd(token);
      this.RemoveParent();
      if (this.Top != 0)
        return;
      this._writer.WriteToken(this._root);
    }

    public override void WriteComment(string text)
    {
      throw JsonWriterException.Create((JsonWriter) this, "Cannot write JSON comment as BSON.", (Exception) null);
    }

    public override void WriteStartConstructor(string name)
    {
      throw JsonWriterException.Create((JsonWriter) this, "Cannot write JSON constructor as BSON.", (Exception) null);
    }

    public override void WriteRaw(string json)
    {
      throw JsonWriterException.Create((JsonWriter) this, "Cannot write raw JSON as BSON.", (Exception) null);
    }

    public override void WriteRawValue(string json)
    {
      throw JsonWriterException.Create((JsonWriter) this, "Cannot write raw JSON as BSON.", (Exception) null);
    }

    public override void WriteStartArray()
    {
      base.WriteStartArray();
      this.AddParent((BsonToken) new BsonArray());
    }

    public override void WriteStartObject()
    {
      base.WriteStartObject();
      this.AddParent((BsonToken) new BsonObject());
    }

    public override void WritePropertyName(string name)
    {
      base.WritePropertyName(name);
      this._propertyName = name;
    }

    public override void Close()
    {
      base.Close();
      if (!this.CloseOutput || this._writer == null)
        return;
      this._writer.Close();
    }

    private void AddParent(BsonToken container)
    {
      this.AddToken(container);
      this._parent = container;
    }

    private void RemoveParent()
    {
      this._parent = this._parent.Parent;
    }

    private void AddValue(object value, BsonType type)
    {
      this.AddToken((BsonToken) new BsonValue(value, type));
    }

    internal void AddToken(BsonToken token)
    {
      if (this._parent != null)
      {
        if (this._parent is BsonObject)
        {
          ((BsonObject) this._parent).Add(this._propertyName, token);
          this._propertyName = (string) null;
        }
        else
          ((BsonArray) this._parent).Add(token);
      }
      else
      {
        if (token.Type != BsonType.Object && token.Type != BsonType.Array)
          throw JsonWriterException.Create((JsonWriter) this, StringUtils.FormatWith("Error writing {0} value. BSON must start with an Object or Array.", (IFormatProvider) CultureInfo.InvariantCulture, (object) token.Type), (Exception) null);
        this._parent = token;
        this._root = token;
      }
    }

    public override void WriteNull()
    {
      base.WriteNull();
      this.AddValue((object) null, BsonType.Null);
    }

    public override void WriteUndefined()
    {
      base.WriteUndefined();
      this.AddValue((object) null, BsonType.Undefined);
    }

    public override void WriteValue(string value)
    {
      base.WriteValue(value);
      if (value == null)
        this.AddValue((object) null, BsonType.Null);
      else
        this.AddToken((BsonToken) new BsonString((object) value, true));
    }

    public override void WriteValue(int value)
    {
      base.WriteValue(value);
      this.AddValue((object) value, BsonType.Integer);
    }

    [CLSCompliant(false)]
    public override void WriteValue(uint value)
    {
      if (value > (uint) int.MaxValue)
        throw JsonWriterException.Create((JsonWriter) this, "Value is too large to fit in a signed 32 bit integer. BSON does not support unsigned values.", (Exception) null);
      base.WriteValue(value);
      this.AddValue((object) value, BsonType.Integer);
    }

    public override void WriteValue(long value)
    {
      base.WriteValue(value);
      this.AddValue((object) value, BsonType.Long);
    }

    [CLSCompliant(false)]
    public override void WriteValue(ulong value)
    {
      if (value > 9223372036854775807UL)
        throw JsonWriterException.Create((JsonWriter) this, "Value is too large to fit in a signed 64 bit integer. BSON does not support unsigned values.", (Exception) null);
      base.WriteValue(value);
      this.AddValue((object) value, BsonType.Long);
    }

    public override void WriteValue(float value)
    {
      base.WriteValue(value);
      this.AddValue((object) value, BsonType.Number);
    }

    public override void WriteValue(double value)
    {
      base.WriteValue(value);
      this.AddValue((object) value, BsonType.Number);
    }

    public override void WriteValue(bool value)
    {
      base.WriteValue(value);
      this.AddValue((object) (bool) (value ? 1 : 0), BsonType.Boolean);
    }

    public override void WriteValue(short value)
    {
      base.WriteValue(value);
      this.AddValue((object) value, BsonType.Integer);
    }

    [CLSCompliant(false)]
    public override void WriteValue(ushort value)
    {
      base.WriteValue(value);
      this.AddValue((object) value, BsonType.Integer);
    }

    public override void WriteValue(char value)
    {
      base.WriteValue(value);
      this.AddToken((BsonToken) new BsonString((object) value.ToString((IFormatProvider) CultureInfo.InvariantCulture), true));
    }

    public override void WriteValue(byte value)
    {
      base.WriteValue(value);
      this.AddValue((object) value, BsonType.Integer);
    }

    [CLSCompliant(false)]
    public override void WriteValue(sbyte value)
    {
      base.WriteValue(value);
      this.AddValue((object) value, BsonType.Integer);
    }

    public override void WriteValue(Decimal value)
    {
      base.WriteValue(value);
      this.AddValue((object) value, BsonType.Number);
    }

    public override void WriteValue(DateTime value)
    {
      base.WriteValue(value);
      value = JsonConvert.EnsureDateTime(value, this.DateTimeZoneHandling);
      this.AddValue((object) value, BsonType.Date);
    }

    public override void WriteValue(byte[] value)
    {
      base.WriteValue(value);
      this.AddValue((object) value, BsonType.Binary);
    }

    public override void WriteValue(Guid value)
    {
      base.WriteValue(value);
      this.AddToken((BsonToken) new BsonString((object) value.ToString(), true));
    }

    public override void WriteValue(TimeSpan value)
    {
      base.WriteValue(value);
      this.AddToken((BsonToken) new BsonString((object) value.ToString(), true));
    }

    public override void WriteValue(Uri value)
    {
      base.WriteValue(value);
      this.AddToken((BsonToken) new BsonString((object) value.ToString(), true));
    }

    public void WriteObjectId(byte[] value)
    {
      ValidationUtils.ArgumentNotNull((object) value, "value");
      if (value.Length != 12)
        throw JsonWriterException.Create((JsonWriter) this, "An object id must be 12 bytes", (Exception) null);
      this.UpdateScopeWithFinishedValue();
      this.AutoComplete(JsonToken.Undefined);
      this.AddValue((object) value, BsonType.Oid);
    }

    public void WriteRegex(string pattern, string options)
    {
      ValidationUtils.ArgumentNotNull((object) pattern, "pattern");
      this.UpdateScopeWithFinishedValue();
      this.AutoComplete(JsonToken.Undefined);
      this.AddToken((BsonToken) new BsonRegex(pattern, options));
    }
  }
}
