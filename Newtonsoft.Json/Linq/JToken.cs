// Type: Newtonsoft.Json.Linq.JToken
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;
using Newtonsoft.Json.Utilities.LinqBridge;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace Newtonsoft.Json.Linq
{
  public abstract class JToken : IJEnumerable<JToken>, IEnumerable<JToken>, IEnumerable, IJsonLineInfo, ICloneable
  {
    private JContainer _parent;
    private JToken _previous;
    private JToken _next;
    private static JTokenEqualityComparer _equalityComparer;
    private int? _lineNumber;
    private int? _linePosition;

    public static JTokenEqualityComparer EqualityComparer
    {
      get
      {
        if (JToken._equalityComparer == null)
          JToken._equalityComparer = new JTokenEqualityComparer();
        return JToken._equalityComparer;
      }
    }

    public JContainer Parent
    {
      [DebuggerStepThrough] get
      {
        return this._parent;
      }
      internal set
      {
        this._parent = value;
      }
    }

    public JToken Root
    {
      get
      {
        JContainer parent = this.Parent;
        if (parent == null)
          return this;
        while (parent.Parent != null)
          parent = parent.Parent;
        return (JToken) parent;
      }
    }

    public abstract JTokenType Type { get; }

    public abstract bool HasValues { get; }

    public JToken Next
    {
      get
      {
        return this._next;
      }
      internal set
      {
        this._next = value;
      }
    }

    public JToken Previous
    {
      get
      {
        return this._previous;
      }
      internal set
      {
        this._previous = value;
      }
    }

    public virtual JToken this[object key]
    {
      get
      {
        throw new InvalidOperationException(StringUtils.FormatWith("Cannot access child value on {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) this.GetType()));
      }
      set
      {
        throw new InvalidOperationException(StringUtils.FormatWith("Cannot set child value on {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) this.GetType()));
      }
    }

    public virtual JToken First
    {
      get
      {
        throw new InvalidOperationException(StringUtils.FormatWith("Cannot access child value on {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) this.GetType()));
      }
    }

    public virtual JToken Last
    {
      get
      {
        throw new InvalidOperationException(StringUtils.FormatWith("Cannot access child value on {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) this.GetType()));
      }
    }

    int IJsonLineInfo.LineNumber
    {
      get
      {
        return this._lineNumber ?? 0;
      }
    }

    int IJsonLineInfo.LinePosition
    {
      get
      {
        return this._linePosition ?? 0;
      }
    }

    internal JToken()
    {
    }

    public static explicit operator bool(JToken value)
    {
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateBoolean((JToken) jvalue, false))
        throw new ArgumentException(StringUtils.FormatWith("Can not convert {0} to Boolean.", (IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      else
        return Convert.ToBoolean(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public static explicit operator bool?(JToken value)
    {
      if (value == null)
        return new bool?();
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateBoolean((JToken) jvalue, true))
        throw new ArgumentException(StringUtils.FormatWith("Can not convert {0} to Boolean.", (IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      if (jvalue.Value == null)
        return new bool?();
      else
        return new bool?(Convert.ToBoolean(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture));
    }

    public static explicit operator long(JToken value)
    {
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateInteger((JToken) jvalue, false))
        throw new ArgumentException(StringUtils.FormatWith("Can not convert {0} to Int64.", (IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      else
        return Convert.ToInt64(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public static explicit operator DateTime?(JToken value)
    {
      if (value == null)
        return new DateTime?();
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateDate((JToken) jvalue, true))
        throw new ArgumentException(StringUtils.FormatWith("Can not convert {0} to DateTime.", (IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      if (jvalue.Value == null)
        return new DateTime?();
      else
        return new DateTime?(Convert.ToDateTime(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture));
    }

    public static explicit operator Decimal?(JToken value)
    {
      if (value == null)
        return new Decimal?();
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateFloat((JToken) jvalue, true))
        throw new ArgumentException(StringUtils.FormatWith("Can not convert {0} to Decimal.", (IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      if (jvalue.Value == null)
        return new Decimal?();
      else
        return new Decimal?(Convert.ToDecimal(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture));
    }

    public static explicit operator double?(JToken value)
    {
      if (value == null)
        return new double?();
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateFloat((JToken) jvalue, true))
        throw new ArgumentException(StringUtils.FormatWith("Can not convert {0} to Double.", (IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      else
        return (double?) jvalue.Value;
    }

    public static explicit operator int(JToken value)
    {
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateInteger((JToken) jvalue, false))
        throw new ArgumentException(StringUtils.FormatWith("Can not convert {0} to Int32.", (IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      else
        return Convert.ToInt32(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public static explicit operator short(JToken value)
    {
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateInteger((JToken) jvalue, false))
        throw new ArgumentException(StringUtils.FormatWith("Can not convert {0} to Int16.", (IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      else
        return Convert.ToInt16(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    [CLSCompliant(false)]
    public static explicit operator ushort(JToken value)
    {
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateInteger((JToken) jvalue, false))
        throw new ArgumentException(StringUtils.FormatWith("Can not convert {0} to UInt16.", (IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      else
        return Convert.ToUInt16(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public static explicit operator int?(JToken value)
    {
      if (value == null)
        return new int?();
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateInteger((JToken) jvalue, true))
        throw new ArgumentException(StringUtils.FormatWith("Can not convert {0} to Int32.", (IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      if (jvalue.Value == null)
        return new int?();
      else
        return new int?(Convert.ToInt32(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture));
    }

    public static explicit operator short?(JToken value)
    {
      if (value == null)
        return new short?();
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateInteger((JToken) jvalue, true))
        throw new ArgumentException(StringUtils.FormatWith("Can not convert {0} to Int16.", (IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      if (jvalue.Value == null)
        return new short?();
      else
        return new short?(Convert.ToInt16(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture));
    }

    [CLSCompliant(false)]
    public static explicit operator ushort?(JToken value)
    {
      if (value == null)
        return new ushort?();
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateInteger((JToken) jvalue, true))
        throw new ArgumentException(StringUtils.FormatWith("Can not convert {0} to UInt16.", (IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      if (jvalue.Value == null)
        return new ushort?();
      else
        return new ushort?((ushort) Convert.ToInt16(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture));
    }

    public static explicit operator DateTime(JToken value)
    {
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateDate((JToken) jvalue, false))
        throw new ArgumentException(StringUtils.FormatWith("Can not convert {0} to DateTime.", (IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      else
        return Convert.ToDateTime(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public static explicit operator long?(JToken value)
    {
      if (value == null)
        return new long?();
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateInteger((JToken) jvalue, true))
        throw new ArgumentException(StringUtils.FormatWith("Can not convert {0} to Int64.", (IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      if (jvalue.Value == null)
        return new long?();
      else
        return new long?(Convert.ToInt64(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture));
    }

    public static explicit operator float?(JToken value)
    {
      if (value == null)
        return new float?();
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateFloat((JToken) jvalue, true))
        throw new ArgumentException(StringUtils.FormatWith("Can not convert {0} to Single.", (IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      if (jvalue.Value == null)
        return new float?();
      else
        return new float?(Convert.ToSingle(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture));
    }

    public static explicit operator Decimal(JToken value)
    {
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateFloat((JToken) jvalue, false))
        throw new ArgumentException(StringUtils.FormatWith("Can not convert {0} to Decimal.", (IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      else
        return Convert.ToDecimal(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    [CLSCompliant(false)]
    public static explicit operator uint?(JToken value)
    {
      if (value == null)
        return new uint?();
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateInteger((JToken) jvalue, true))
        throw new ArgumentException(StringUtils.FormatWith("Can not convert {0} to UInt32.", (IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      if (jvalue.Value == null)
        return new uint?();
      else
        return new uint?(Convert.ToUInt32(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture));
    }

    [CLSCompliant(false)]
    public static explicit operator ulong?(JToken value)
    {
      if (value == null)
        return new ulong?();
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateInteger((JToken) jvalue, true))
        throw new ArgumentException(StringUtils.FormatWith("Can not convert {0} to UInt64.", (IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      if (jvalue.Value == null)
        return new ulong?();
      else
        return new ulong?(Convert.ToUInt64(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture));
    }

    public static explicit operator double(JToken value)
    {
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateFloat((JToken) jvalue, false))
        throw new ArgumentException(StringUtils.FormatWith("Can not convert {0} to Double.", (IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      else
        return Convert.ToDouble(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public static explicit operator float(JToken value)
    {
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateFloat((JToken) jvalue, false))
        throw new ArgumentException(StringUtils.FormatWith("Can not convert {0} to Single.", (IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      else
        return Convert.ToSingle(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public static explicit operator string(JToken value)
    {
      if (value == null)
        return (string) null;
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateString((JToken) jvalue))
        throw new ArgumentException(StringUtils.FormatWith("Can not convert {0} to String.", (IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      if (jvalue.Value == null)
        return (string) null;
      else
        return Convert.ToString(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    [CLSCompliant(false)]
    public static explicit operator uint(JToken value)
    {
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateInteger((JToken) jvalue, false))
        throw new ArgumentException(StringUtils.FormatWith("Can not convert {0} to UInt32.", (IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      else
        return Convert.ToUInt32(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    [CLSCompliant(false)]
    public static explicit operator ulong(JToken value)
    {
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateInteger((JToken) jvalue, false))
        throw new ArgumentException(StringUtils.FormatWith("Can not convert {0} to UInt64.", (IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      else
        return Convert.ToUInt64(jvalue.Value, (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public static explicit operator byte[](JToken value)
    {
      JValue jvalue = JToken.EnsureValue(value);
      if (jvalue == null || !JToken.ValidateBytes((JToken) jvalue))
        throw new ArgumentException(StringUtils.FormatWith("Can not convert {0} to byte array.", (IFormatProvider) CultureInfo.InvariantCulture, (object) JToken.GetType(value)));
      else
        return (byte[]) jvalue.Value;
    }

    public static implicit operator JToken(bool value)
    {
      return (JToken) new JValue(value);
    }

    public static implicit operator JToken(bool? value)
    {
      return (JToken) new JValue((object) value);
    }

    public static implicit operator JToken(long value)
    {
      return (JToken) new JValue(value);
    }

    public static implicit operator JToken(DateTime? value)
    {
      return (JToken) new JValue((object) value);
    }

    public static implicit operator JToken(Decimal? value)
    {
      return (JToken) new JValue((object) value);
    }

    public static implicit operator JToken(double? value)
    {
      return (JToken) new JValue((object) value);
    }

    [CLSCompliant(false)]
    public static implicit operator JToken(short value)
    {
      return (JToken) new JValue((long) value);
    }

    [CLSCompliant(false)]
    public static implicit operator JToken(ushort value)
    {
      return (JToken) new JValue((long) value);
    }

    public static implicit operator JToken(int value)
    {
      return (JToken) new JValue((long) value);
    }

    public static implicit operator JToken(int? value)
    {
      return (JToken) new JValue((object) value);
    }

    public static implicit operator JToken(DateTime value)
    {
      return (JToken) new JValue(value);
    }

    public static implicit operator JToken(long? value)
    {
      return (JToken) new JValue((object) value);
    }

    public static implicit operator JToken(float? value)
    {
      return (JToken) new JValue((object) value);
    }

    public static implicit operator JToken(Decimal value)
    {
      return (JToken) new JValue((object) value);
    }

    [CLSCompliant(false)]
    public static implicit operator JToken(short? value)
    {
      return (JToken) new JValue((object) value);
    }

    [CLSCompliant(false)]
    public static implicit operator JToken(ushort? value)
    {
      return (JToken) new JValue((object) value);
    }

    [CLSCompliant(false)]
    public static implicit operator JToken(uint? value)
    {
      return (JToken) new JValue((object) value);
    }

    [CLSCompliant(false)]
    public static implicit operator JToken(ulong? value)
    {
      return (JToken) new JValue((object) value);
    }

    public static implicit operator JToken(double value)
    {
      return (JToken) new JValue(value);
    }

    public static implicit operator JToken(float value)
    {
      return (JToken) new JValue(value);
    }

    public static implicit operator JToken(string value)
    {
      return (JToken) new JValue(value);
    }

    [CLSCompliant(false)]
    public static implicit operator JToken(uint value)
    {
      return (JToken) new JValue((long) value);
    }

    [CLSCompliant(false)]
    public static implicit operator JToken(ulong value)
    {
      return (JToken) new JValue(value);
    }

    public static implicit operator JToken(byte[] value)
    {
      return (JToken) new JValue((object) value);
    }

    internal abstract JToken CloneToken();

    internal abstract bool DeepEquals(JToken node);

    public static bool DeepEquals(JToken t1, JToken t2)
    {
      if (t1 == t2)
        return true;
      if (t1 != null && t2 != null)
        return t1.DeepEquals(t2);
      else
        return false;
    }

    public void AddAfterSelf(object content)
    {
      if (this._parent == null)
        throw new InvalidOperationException("The parent is missing.");
      this._parent.AddInternal(this._parent.IndexOfItem(this) + 1, content, false);
    }

    public void AddBeforeSelf(object content)
    {
      if (this._parent == null)
        throw new InvalidOperationException("The parent is missing.");
      this._parent.AddInternal(this._parent.IndexOfItem(this), content, false);
    }

    public IEnumerable<JToken> Ancestors()
    {
      for (JToken parent = (JToken) this.Parent; parent != null; parent = (JToken) parent.Parent)
        yield return parent;
    }

    public IEnumerable<JToken> AfterSelf()
    {
      if (this.Parent != null)
      {
        for (JToken o = this.Next; o != null; o = o.Next)
          yield return o;
      }
    }

    public IEnumerable<JToken> BeforeSelf()
    {
      for (JToken o = this.Parent.First; o != this; o = o.Next)
        yield return o;
    }

    public virtual T Value<T>(object key)
    {
      return Extensions.Convert<JToken, T>(this[key]);
    }

    public virtual JEnumerable<JToken> Children()
    {
      return JEnumerable<JToken>.Empty;
    }

    public JEnumerable<T> Children<T>() where T : JToken
    {
      return new JEnumerable<T>(Enumerable.OfType<T>((IEnumerable) this.Children()));
    }

    public virtual IEnumerable<T> Values<T>()
    {
      throw new InvalidOperationException(StringUtils.FormatWith("Cannot access child value on {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) this.GetType()));
    }

    public void Remove()
    {
      if (this._parent == null)
        throw new InvalidOperationException("The parent is missing.");
      this._parent.RemoveItem(this);
    }

    public void Replace(JToken value)
    {
      if (this._parent == null)
        throw new InvalidOperationException("The parent is missing.");
      this._parent.ReplaceItem(this, value);
    }

    public abstract void WriteTo(JsonWriter writer, params JsonConverter[] converters);

    public override string ToString()
    {
      return this.ToString(Formatting.Indented, new JsonConverter[0]);
    }

    public string ToString(Formatting formatting, params JsonConverter[] converters)
    {
      using (StringWriter stringWriter = new StringWriter((IFormatProvider) CultureInfo.InvariantCulture))
      {
        JsonTextWriter jsonTextWriter = new JsonTextWriter((TextWriter) stringWriter);
        jsonTextWriter.Formatting = formatting;
        this.WriteTo((JsonWriter) jsonTextWriter, converters);
        return stringWriter.ToString();
      }
    }

    private static JValue EnsureValue(JToken value)
    {
      if (value == null)
        throw new ArgumentNullException("value");
      if (value is JProperty)
        value = ((JProperty) value).Value;
      return value as JValue;
    }

    private static string GetType(JToken token)
    {
      ValidationUtils.ArgumentNotNull((object) token, "token");
      if (token is JProperty)
        token = ((JProperty) token).Value;
      return ((object) token.Type).ToString();
    }

    private static bool IsNullable(JToken o)
    {
      if (o.Type != JTokenType.Undefined)
        return o.Type == JTokenType.Null;
      else
        return true;
    }

    private static bool ValidateFloat(JToken o, bool nullable)
    {
      if (o.Type == JTokenType.Float || o.Type == JTokenType.Integer)
        return true;
      if (nullable)
        return JToken.IsNullable(o);
      else
        return false;
    }

    private static bool ValidateInteger(JToken o, bool nullable)
    {
      if (o.Type == JTokenType.Integer)
        return true;
      if (nullable)
        return JToken.IsNullable(o);
      else
        return false;
    }

    private static bool ValidateDate(JToken o, bool nullable)
    {
      if (o.Type == JTokenType.Date)
        return true;
      if (nullable)
        return JToken.IsNullable(o);
      else
        return false;
    }

    private static bool ValidateBoolean(JToken o, bool nullable)
    {
      if (o.Type == JTokenType.Boolean)
        return true;
      if (nullable)
        return JToken.IsNullable(o);
      else
        return false;
    }

    private static bool ValidateString(JToken o)
    {
      if (o.Type != JTokenType.String && o.Type != JTokenType.Comment && o.Type != JTokenType.Raw)
        return JToken.IsNullable(o);
      else
        return true;
    }

    private static bool ValidateBytes(JToken o)
    {
      if (o.Type != JTokenType.Bytes)
        return JToken.IsNullable(o);
      else
        return true;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) this.GetEnumerator();
    }

    IEnumerator<JToken> IEnumerable<JToken>.GetEnumerator()
    {
      return this.Children().GetEnumerator();
    }

    internal abstract int GetDeepHashCode();

    IJEnumerable<JToken> IJEnumerable<JToken>.get_Item(object key)
    {
      return (IJEnumerable<JToken>) this[key];
    }

    public JsonReader CreateReader()
    {
      return (JsonReader) new JTokenReader(this);
    }

    internal static JToken FromObjectInternal(object o, JsonSerializer jsonSerializer)
    {
      ValidationUtils.ArgumentNotNull(o, "o");
      ValidationUtils.ArgumentNotNull((object) jsonSerializer, "jsonSerializer");
      using (JTokenWriter jtokenWriter = new JTokenWriter())
      {
        jsonSerializer.Serialize((JsonWriter) jtokenWriter, o);
        return jtokenWriter.Token;
      }
    }

    public static JToken FromObject(object o)
    {
      return JToken.FromObjectInternal(o, new JsonSerializer());
    }

    public static JToken FromObject(object o, JsonSerializer jsonSerializer)
    {
      return JToken.FromObjectInternal(o, jsonSerializer);
    }

    public T ToObject<T>()
    {
      return this.ToObject<T>(new JsonSerializer());
    }

    public T ToObject<T>(JsonSerializer jsonSerializer)
    {
      ValidationUtils.ArgumentNotNull((object) jsonSerializer, "jsonSerializer");
      using (JTokenReader jtokenReader = new JTokenReader(this))
        return jsonSerializer.Deserialize<T>((JsonReader) jtokenReader);
    }

    public static JToken ReadFrom(JsonReader reader)
    {
      ValidationUtils.ArgumentNotNull((object) reader, "reader");
      if (reader.TokenType == JsonToken.None && !reader.Read())
        throw JsonReaderException.Create(reader, "Error reading JToken from JsonReader.");
      if (reader.TokenType == JsonToken.StartObject)
        return (JToken) JObject.Load(reader);
      if (reader.TokenType == JsonToken.StartArray)
        return (JToken) JArray.Load(reader);
      if (reader.TokenType == JsonToken.PropertyName)
        return (JToken) JProperty.Load(reader);
      if (reader.TokenType == JsonToken.StartConstructor)
        return (JToken) JConstructor.Load(reader);
      if (!JsonReader.IsStartToken(reader.TokenType))
        return (JToken) new JValue(reader.Value);
      else
        throw JsonReaderException.Create(reader, StringUtils.FormatWith("Error reading JToken from JsonReader. Unexpected token: {0}", (IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType));
    }

    public static JToken Parse(string json)
    {
      JsonReader reader = (JsonReader) new JsonTextReader((TextReader) new StringReader(json));
      JToken jtoken = JToken.Load(reader);
      if (reader.Read() && reader.TokenType != JsonToken.Comment)
        throw JsonReaderException.Create(reader, "Additional text found in JSON string after parsing content.");
      else
        return jtoken;
    }

    public static JToken Load(JsonReader reader)
    {
      return JToken.ReadFrom(reader);
    }

    internal void SetLineInfo(IJsonLineInfo lineInfo)
    {
      if (lineInfo == null || !lineInfo.HasLineInfo())
        return;
      this.SetLineInfo(lineInfo.LineNumber, lineInfo.LinePosition);
    }

    internal void SetLineInfo(int lineNumber, int linePosition)
    {
      this._lineNumber = new int?(lineNumber);
      this._linePosition = new int?(linePosition);
    }

    bool IJsonLineInfo.HasLineInfo()
    {
      if (this._lineNumber.HasValue)
        return this._linePosition.HasValue;
      else
        return false;
    }

    public JToken SelectToken(string path)
    {
      return this.SelectToken(path, false);
    }

    public JToken SelectToken(string path, bool errorWhenNoMatch)
    {
      return new JPath(path).Evaluate(this, errorWhenNoMatch);
    }

    object ICloneable.Clone()
    {
      return (object) this.DeepClone();
    }

    public JToken DeepClone()
    {
      return this.CloneToken();
    }
  }
}
