// Type: Newtonsoft.Json.Linq.JValue
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Newtonsoft.Json.Linq
{
  public class JValue : JToken, IEquatable<JValue>, IFormattable, IComparable, IComparable<JValue>
  {
    private JTokenType _valueType;
    private object _value;

    public override bool HasValues
    {
      get
      {
        return false;
      }
    }

    public override JTokenType Type
    {
      get
      {
        return this._valueType;
      }
    }

    public object Value
    {
      get
      {
        return this._value;
      }
      set
      {
        if ((this._value != null ? this._value.GetType() : (Type) null) != (value != null ? value.GetType() : (Type) null))
          this._valueType = JValue.GetValueType(new JTokenType?(this._valueType), value);
        this._value = value;
      }
    }

    internal JValue(object value, JTokenType type)
    {
      this._value = value;
      this._valueType = type;
    }

    public JValue(JValue other)
      : this(other.Value, other.Type)
    {
    }

    public JValue(long value)
      : this((object) value, JTokenType.Integer)
    {
    }

    [CLSCompliant(false)]
    public JValue(ulong value)
      : this((object) value, JTokenType.Integer)
    {
    }

    public JValue(double value)
      : this((object) value, JTokenType.Float)
    {
    }

    public JValue(float value)
      : this((object) value, JTokenType.Float)
    {
    }

    public JValue(DateTime value)
      : this((object) value, JTokenType.Date)
    {
    }

    public JValue(bool value)
      : this((object) (bool) (value ? 1 : 0), JTokenType.Boolean)
    {
    }

    public JValue(string value)
      : this((object) value, JTokenType.String)
    {
    }

    public JValue(Guid value)
      : this((object) value, JTokenType.String)
    {
    }

    public JValue(Uri value)
      : this((object) value, JTokenType.String)
    {
    }

    public JValue(TimeSpan value)
      : this((object) value, JTokenType.String)
    {
    }

    public JValue(object value)
      : this(value, JValue.GetValueType(new JTokenType?(), value))
    {
    }

    internal override bool DeepEquals(JToken node)
    {
      JValue v2 = node as JValue;
      if (v2 == null)
        return false;
      if (v2 == this)
        return true;
      else
        return JValue.ValuesEquals(this, v2);
    }

    private static int Compare(JTokenType valueType, object objA, object objB)
    {
      if (objA == null && objB == null)
        return 0;
      if (objA != null && objB == null)
        return 1;
      if (objA == null && objB != null)
        return -1;
      switch (valueType)
      {
        case JTokenType.Comment:
        case JTokenType.String:
        case JTokenType.Raw:
          return string.CompareOrdinal(Convert.ToString(objA, (IFormatProvider) CultureInfo.InvariantCulture), Convert.ToString(objB, (IFormatProvider) CultureInfo.InvariantCulture));
        case JTokenType.Integer:
          if (objA is ulong || objB is ulong || (objA is Decimal || objB is Decimal))
            return Convert.ToDecimal(objA, (IFormatProvider) CultureInfo.InvariantCulture).CompareTo(Convert.ToDecimal(objB, (IFormatProvider) CultureInfo.InvariantCulture));
          if (objA is float || objB is float || (objA is double || objB is double))
            return JValue.CompareFloat(objA, objB);
          else
            return Convert.ToInt64(objA, (IFormatProvider) CultureInfo.InvariantCulture).CompareTo(Convert.ToInt64(objB, (IFormatProvider) CultureInfo.InvariantCulture));
        case JTokenType.Float:
          return JValue.CompareFloat(objA, objB);
        case JTokenType.Boolean:
          return Convert.ToBoolean(objA, (IFormatProvider) CultureInfo.InvariantCulture).CompareTo(Convert.ToBoolean(objB, (IFormatProvider) CultureInfo.InvariantCulture));
        case JTokenType.Date:
          return ((DateTime) objA).CompareTo(!(objB is DateTimeOffset) ? Convert.ToDateTime(objB, (IFormatProvider) CultureInfo.InvariantCulture) : ((DateTimeOffset) objB).DateTime);
        case JTokenType.Bytes:
          if (!(objB is byte[]))
            throw new ArgumentException("Object must be of type byte[].");
          byte[] a1 = objA as byte[];
          byte[] a2 = objB as byte[];
          if (a1 == null)
            return -1;
          if (a2 == null)
            return 1;
          else
            return MiscellaneousUtils.ByteArrayCompare(a1, a2);
        case JTokenType.Guid:
          if (!(objB is Guid))
            throw new ArgumentException("Object must be of type Guid.");
          else
            return ((Guid) objA).CompareTo((Guid) objB);
        case JTokenType.Uri:
          if (!(objB is Uri))
            throw new ArgumentException("Object must be of type Uri.");
          else
            return Comparer<string>.Default.Compare(((Uri) objA).ToString(), ((Uri) objB).ToString());
        case JTokenType.TimeSpan:
          if (!(objB is TimeSpan))
            throw new ArgumentException("Object must be of type TimeSpan.");
          else
            return ((TimeSpan) objA).CompareTo((TimeSpan) objB);
        default:
          throw MiscellaneousUtils.CreateArgumentOutOfRangeException("valueType", (object) valueType, StringUtils.FormatWith("Unexpected value type: {0}", (IFormatProvider) CultureInfo.InvariantCulture, (object) valueType));
      }
    }

    private static int CompareFloat(object objA, object objB)
    {
      double d1 = Convert.ToDouble(objA, (IFormatProvider) CultureInfo.InvariantCulture);
      double d2 = Convert.ToDouble(objB, (IFormatProvider) CultureInfo.InvariantCulture);
      if (MathUtils.ApproxEquals(d1, d2))
        return 0;
      else
        return d1.CompareTo(d2);
    }

    internal override JToken CloneToken()
    {
      return (JToken) new JValue(this);
    }

    public static JValue CreateComment(string value)
    {
      return new JValue((object) value, JTokenType.Comment);
    }

    public static JValue CreateString(string value)
    {
      return new JValue((object) value, JTokenType.String);
    }

    private static JTokenType GetValueType(JTokenType? current, object value)
    {
      if (value == null || value == DBNull.Value)
        return JTokenType.Null;
      if (value is string)
        return JValue.GetStringValueType(current);
      if (value is long || value is int || (value is short || value is sbyte) || (value is ulong || value is uint || (value is ushort || value is byte)) || value is Enum)
        return JTokenType.Integer;
      if (value is double || value is float || value is Decimal)
        return JTokenType.Float;
      if (value is DateTime)
        return JTokenType.Date;
      if (value is byte[])
        return JTokenType.Bytes;
      if (value is bool)
        return JTokenType.Boolean;
      if (value is Guid)
        return JTokenType.Guid;
      if (value is Uri)
        return JTokenType.Uri;
      if (value is TimeSpan)
        return JTokenType.TimeSpan;
      else
        throw new ArgumentException(StringUtils.FormatWith("Could not determine JSON object type for type {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) value.GetType()));
    }

    private static JTokenType GetStringValueType(JTokenType? current)
    {
      if (!current.HasValue)
        return JTokenType.String;
      switch (current.Value)
      {
        case JTokenType.Comment:
        case JTokenType.String:
        case JTokenType.Raw:
          return current.Value;
        default:
          return JTokenType.String;
      }
    }

    public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
    {
      if (converters != null && converters.Length > 0 && this._value != null)
      {
        JsonConverter matchingConverter = JsonSerializer.GetMatchingConverter((IList<JsonConverter>) converters, this._value.GetType());
        if (matchingConverter != null)
        {
          matchingConverter.WriteJson(writer, this._value, new JsonSerializer());
          return;
        }
      }
      switch (this._valueType)
      {
        case JTokenType.Comment:
          writer.WriteComment(this._value != null ? this._value.ToString() : (string) null);
          break;
        case JTokenType.Integer:
          writer.WriteValue(Convert.ToInt64(this._value, (IFormatProvider) CultureInfo.InvariantCulture));
          break;
        case JTokenType.Float:
          if (this._value is Decimal)
          {
            writer.WriteValue((Decimal) this._value);
            break;
          }
          else if (this._value is double)
          {
            writer.WriteValue((double) this._value);
            break;
          }
          else if (this._value is float)
          {
            writer.WriteValue((float) this._value);
            break;
          }
          else
          {
            writer.WriteValue(Convert.ToDouble(this._value, (IFormatProvider) CultureInfo.InvariantCulture));
            break;
          }
        case JTokenType.String:
          writer.WriteValue(this._value != null ? this._value.ToString() : (string) null);
          break;
        case JTokenType.Boolean:
          writer.WriteValue(Convert.ToBoolean(this._value, (IFormatProvider) CultureInfo.InvariantCulture));
          break;
        case JTokenType.Null:
          writer.WriteNull();
          break;
        case JTokenType.Undefined:
          writer.WriteUndefined();
          break;
        case JTokenType.Date:
          writer.WriteValue(Convert.ToDateTime(this._value, (IFormatProvider) CultureInfo.InvariantCulture));
          break;
        case JTokenType.Raw:
          writer.WriteRawValue(this._value != null ? this._value.ToString() : (string) null);
          break;
        case JTokenType.Bytes:
          writer.WriteValue((byte[]) this._value);
          break;
        case JTokenType.Guid:
        case JTokenType.Uri:
        case JTokenType.TimeSpan:
          writer.WriteValue(this._value != null ? this._value.ToString() : (string) null);
          break;
        default:
          throw MiscellaneousUtils.CreateArgumentOutOfRangeException("TokenType", (object) this._valueType, "Unexpected token type.");
      }
    }

    internal override int GetDeepHashCode()
    {
      return this._valueType.GetHashCode() ^ (this._value != null ? this._value.GetHashCode() : 0);
    }

    private static bool ValuesEquals(JValue v1, JValue v2)
    {
      if (v1 == v2)
        return true;
      if (v1._valueType == v2._valueType)
        return JValue.Compare(v1._valueType, v1._value, v2._value) == 0;
      else
        return false;
    }

    public bool Equals(JValue other)
    {
      if (other == null)
        return false;
      else
        return JValue.ValuesEquals(this, other);
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      JValue other = obj as JValue;
      if (other != null)
        return this.Equals(other);
      else
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
      if (this._value == null)
        return 0;
      else
        return this._value.GetHashCode();
    }

    public override string ToString()
    {
      if (this._value == null)
        return string.Empty;
      else
        return this._value.ToString();
    }

    public string ToString(string format)
    {
      return this.ToString(format, (IFormatProvider) CultureInfo.CurrentCulture);
    }

    public string ToString(IFormatProvider formatProvider)
    {
      return this.ToString((string) null, formatProvider);
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
      if (this._value == null)
        return string.Empty;
      IFormattable formattable = this._value as IFormattable;
      if (formattable != null)
        return formattable.ToString(format, formatProvider);
      else
        return this._value.ToString();
    }

    int IComparable.CompareTo(object obj)
    {
      if (obj == null)
        return 1;
      else
        return JValue.Compare(this._valueType, this._value, obj is JValue ? ((JValue) obj).Value : obj);
    }

    public int CompareTo(JValue obj)
    {
      if (obj == null)
        return 1;
      else
        return JValue.Compare(this._valueType, this._value, obj._value);
    }
  }
}
