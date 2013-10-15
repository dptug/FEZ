// Type: Newtonsoft.Json.JsonReader
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using Newtonsoft.Json.Utilities.LinqBridge;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Newtonsoft.Json
{
  public abstract class JsonReader : IDisposable
  {
    private JsonToken _tokenType;
    private object _value;
    private char _quoteChar;
    internal JsonReader.State _currentState;
    internal ReadType _readType;
    private JsonPosition _currentPosition;
    private CultureInfo _culture;
    private DateTimeZoneHandling _dateTimeZoneHandling;
    private int? _maxDepth;
    private bool _hasExceededMaxDepth;
    internal DateParseHandling _dateParseHandling;
    private readonly List<JsonPosition> _stack;

    protected JsonReader.State CurrentState
    {
      get
      {
        return this._currentState;
      }
    }

    public bool CloseInput { get; set; }

    public virtual char QuoteChar
    {
      get
      {
        return this._quoteChar;
      }
      protected internal set
      {
        this._quoteChar = value;
      }
    }

    public DateTimeZoneHandling DateTimeZoneHandling
    {
      get
      {
        return this._dateTimeZoneHandling;
      }
      set
      {
        this._dateTimeZoneHandling = value;
      }
    }

    public DateParseHandling DateParseHandling
    {
      get
      {
        return this._dateParseHandling;
      }
      set
      {
        this._dateParseHandling = value;
      }
    }

    public int? MaxDepth
    {
      get
      {
        return this._maxDepth;
      }
      set
      {
        int? nullable = value;
        if ((nullable.GetValueOrDefault() > 0 ? 0 : (nullable.HasValue ? 1 : 0)) != 0)
          throw new ArgumentException("Value must be positive.", "value");
        this._maxDepth = value;
      }
    }

    public virtual JsonToken TokenType
    {
      get
      {
        return this._tokenType;
      }
    }

    public virtual object Value
    {
      get
      {
        return this._value;
      }
    }

    public virtual Type ValueType
    {
      get
      {
        if (this._value == null)
          return (Type) null;
        else
          return this._value.GetType();
      }
    }

    public virtual int Depth
    {
      get
      {
        int count = this._stack.Count;
        if (JsonReader.IsStartToken(this.TokenType) || this._currentPosition.Type == JsonContainerType.None)
          return count;
        else
          return count + 1;
      }
    }

    public virtual string Path
    {
      get
      {
        if (this._currentPosition.Type == JsonContainerType.None)
          return string.Empty;
        return JsonPosition.BuildPath(Enumerable.Concat<JsonPosition>((IEnumerable<JsonPosition>) this._stack, (IEnumerable<JsonPosition>) new JsonPosition[1]
        {
          this._currentPosition
        }));
      }
    }

    public CultureInfo Culture
    {
      get
      {
        return this._culture ?? CultureInfo.InvariantCulture;
      }
      set
      {
        this._culture = value;
      }
    }

    protected JsonReader()
    {
      this._currentState = JsonReader.State.Start;
      this._stack = new List<JsonPosition>(4);
      this._dateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind;
      this._dateParseHandling = DateParseHandling.DateTime;
      this.CloseInput = true;
    }

    internal JsonPosition GetPosition(int depth)
    {
      if (depth < this._stack.Count)
        return this._stack[depth];
      else
        return this._currentPosition;
    }

    private void Push(JsonContainerType value)
    {
      this.UpdateScopeWithFinishedValue();
      if (this._currentPosition.Type == JsonContainerType.None)
      {
        this._currentPosition.Type = value;
      }
      else
      {
        this._stack.Add(this._currentPosition);
        this._currentPosition = new JsonPosition()
        {
          Type = value
        };
        if (!this._maxDepth.HasValue)
          return;
        int num = this.Depth + 1;
        int? nullable = this._maxDepth;
        if ((num <= nullable.GetValueOrDefault() ? 0 : (nullable.HasValue ? 1 : 0)) == 0 || this._hasExceededMaxDepth)
          return;
        this._hasExceededMaxDepth = true;
        throw JsonReaderException.Create(this, StringUtils.FormatWith("The reader's MaxDepth of {0} has been exceeded.", (IFormatProvider) CultureInfo.InvariantCulture, (object) this._maxDepth));
      }
    }

    private JsonContainerType Pop()
    {
      JsonPosition jsonPosition;
      if (this._stack.Count > 0)
      {
        jsonPosition = this._currentPosition;
        this._currentPosition = this._stack[this._stack.Count - 1];
        this._stack.RemoveAt(this._stack.Count - 1);
      }
      else
      {
        jsonPosition = this._currentPosition;
        this._currentPosition = new JsonPosition();
      }
      if (this._maxDepth.HasValue)
      {
        int depth = this.Depth;
        int? nullable = this._maxDepth;
        if ((depth > nullable.GetValueOrDefault() ? 0 : (nullable.HasValue ? 1 : 0)) != 0)
          this._hasExceededMaxDepth = false;
      }
      return jsonPosition.Type;
    }

    private JsonContainerType Peek()
    {
      return this._currentPosition.Type;
    }

    public abstract bool Read();

    public abstract int? ReadAsInt32();

    public abstract string ReadAsString();

    public abstract byte[] ReadAsBytes();

    public abstract Decimal? ReadAsDecimal();

    public abstract DateTime? ReadAsDateTime();

    internal virtual bool ReadInternal()
    {
      throw new NotImplementedException();
    }

    internal byte[] ReadAsBytesInternal()
    {
      this._readType = ReadType.ReadAsBytes;
      while (this.ReadInternal())
      {
        if (this.TokenType != JsonToken.Comment)
        {
          if (this.IsWrappedInTypeObject())
          {
            byte[] numArray = this.ReadAsBytes();
            this.ReadInternal();
            this.SetToken(JsonToken.Bytes, (object) numArray);
            return numArray;
          }
          else
          {
            if (this.TokenType == JsonToken.String)
            {
              string s = (string) this.Value;
              this.SetToken(JsonToken.Bytes, s.Length == 0 ? (object) new byte[0] : (object) Convert.FromBase64String(s));
            }
            if (this.TokenType == JsonToken.Null)
              return (byte[]) null;
            if (this.TokenType == JsonToken.Bytes)
              return (byte[]) this.Value;
            if (this.TokenType == JsonToken.StartArray)
            {
              List<byte> list = new List<byte>();
              while (this.ReadInternal())
              {
                switch (this.TokenType)
                {
                  case JsonToken.Comment:
                    continue;
                  case JsonToken.Integer:
                    list.Add(Convert.ToByte(this.Value, (IFormatProvider) CultureInfo.InvariantCulture));
                    continue;
                  case JsonToken.EndArray:
                    byte[] numArray = list.ToArray();
                    this.SetToken(JsonToken.Bytes, (object) numArray);
                    return numArray;
                  default:
                    throw JsonReaderException.Create(this, StringUtils.FormatWith("Unexpected token when reading bytes: {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) this.TokenType));
                }
              }
              throw JsonReaderException.Create(this, "Unexpected end when reading bytes.");
            }
            else if (this.TokenType == JsonToken.EndArray)
              return (byte[]) null;
            else
              throw JsonReaderException.Create(this, StringUtils.FormatWith("Error reading bytes. Unexpected token: {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) this.TokenType));
          }
        }
      }
      this.SetToken(JsonToken.None);
      return (byte[]) null;
    }

    internal Decimal? ReadAsDecimalInternal()
    {
      this._readType = ReadType.ReadAsDecimal;
      while (this.ReadInternal())
      {
        if (this.TokenType != JsonToken.Comment)
        {
          if (this.TokenType == JsonToken.Integer || this.TokenType == JsonToken.Float)
          {
            if (!(this.Value is Decimal))
              this.SetToken(JsonToken.Float, (object) Convert.ToDecimal(this.Value, (IFormatProvider) CultureInfo.InvariantCulture));
            return new Decimal?((Decimal) this.Value);
          }
          else
          {
            if (this.TokenType == JsonToken.Null)
              return new Decimal?();
            if (this.TokenType == JsonToken.String)
            {
              string s = (string) this.Value;
              if (string.IsNullOrEmpty(s))
              {
                this.SetToken(JsonToken.Null);
                return new Decimal?();
              }
              else
              {
                Decimal result;
                if (!Decimal.TryParse(s, NumberStyles.Number, (IFormatProvider) this.Culture, out result))
                  throw JsonReaderException.Create(this, StringUtils.FormatWith("Could not convert string to decimal: {0}.", (IFormatProvider) CultureInfo.InvariantCulture, this.Value));
                this.SetToken(JsonToken.Float, (object) result);
                return new Decimal?(result);
              }
            }
            else if (this.TokenType == JsonToken.EndArray)
              return new Decimal?();
            else
              throw JsonReaderException.Create(this, StringUtils.FormatWith("Error reading decimal. Unexpected token: {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) this.TokenType));
          }
        }
      }
      this.SetToken(JsonToken.None);
      return new Decimal?();
    }

    internal int? ReadAsInt32Internal()
    {
      this._readType = ReadType.ReadAsInt32;
      while (this.ReadInternal())
      {
        if (this.TokenType != JsonToken.Comment)
        {
          if (this.TokenType == JsonToken.Integer || this.TokenType == JsonToken.Float)
          {
            if (!(this.Value is int))
              this.SetToken(JsonToken.Integer, (object) Convert.ToInt32(this.Value, (IFormatProvider) CultureInfo.InvariantCulture));
            return new int?((int) this.Value);
          }
          else
          {
            if (this.TokenType == JsonToken.Null)
              return new int?();
            if (this.TokenType == JsonToken.String)
            {
              string s = (string) this.Value;
              if (string.IsNullOrEmpty(s))
              {
                this.SetToken(JsonToken.Null);
                return new int?();
              }
              else
              {
                int result;
                if (!int.TryParse(s, NumberStyles.Integer, (IFormatProvider) this.Culture, out result))
                  throw JsonReaderException.Create(this, StringUtils.FormatWith("Could not convert string to integer: {0}.", (IFormatProvider) CultureInfo.InvariantCulture, this.Value));
                this.SetToken(JsonToken.Integer, (object) result);
                return new int?(result);
              }
            }
            else if (this.TokenType == JsonToken.EndArray)
              return new int?();
            else
              throw JsonReaderException.Create(this, StringUtils.FormatWith("Error reading integer. Unexpected token: {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) this.TokenType));
          }
        }
      }
      this.SetToken(JsonToken.None);
      return new int?();
    }

    internal string ReadAsStringInternal()
    {
      this._readType = ReadType.ReadAsString;
      while (this.ReadInternal())
      {
        if (this.TokenType != JsonToken.Comment)
        {
          if (this.TokenType == JsonToken.String)
            return (string) this.Value;
          if (this.TokenType == JsonToken.Null)
            return (string) null;
          if (JsonReader.IsPrimitiveToken(this.TokenType) && this.Value != null)
          {
            string str = !ConvertUtils.IsConvertible(this.Value) ? (!(this.Value is IFormattable) ? this.Value.ToString() : ((IFormattable) this.Value).ToString((string) null, (IFormatProvider) this.Culture)) : ConvertUtils.ToConvertible(this.Value).ToString((IFormatProvider) this.Culture);
            this.SetToken(JsonToken.String, (object) str);
            return str;
          }
          else if (this.TokenType == JsonToken.EndArray)
            return (string) null;
          else
            throw JsonReaderException.Create(this, StringUtils.FormatWith("Error reading string. Unexpected token: {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) this.TokenType));
        }
      }
      this.SetToken(JsonToken.None);
      return (string) null;
    }

    internal DateTime? ReadAsDateTimeInternal()
    {
      this._readType = ReadType.ReadAsDateTime;
      while (this.ReadInternal())
      {
        if (this.TokenType != JsonToken.Comment)
        {
          if (this.TokenType == JsonToken.Date)
            return new DateTime?((DateTime) this.Value);
          if (this.TokenType == JsonToken.Null)
            return new DateTime?();
          if (this.TokenType == JsonToken.String)
          {
            string s = (string) this.Value;
            if (string.IsNullOrEmpty(s))
            {
              this.SetToken(JsonToken.Null);
              return new DateTime?();
            }
            else
            {
              DateTime result;
              if (!DateTime.TryParse(s, (IFormatProvider) this.Culture, DateTimeStyles.RoundtripKind, out result))
                throw JsonReaderException.Create(this, StringUtils.FormatWith("Could not convert string to DateTime: {0}.", (IFormatProvider) CultureInfo.InvariantCulture, this.Value));
              DateTime dateTime = JsonConvert.EnsureDateTime(result, this.DateTimeZoneHandling);
              this.SetToken(JsonToken.Date, (object) dateTime);
              return new DateTime?(dateTime);
            }
          }
          else if (this.TokenType == JsonToken.EndArray)
            return new DateTime?();
          else
            throw JsonReaderException.Create(this, StringUtils.FormatWith("Error reading date. Unexpected token: {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) this.TokenType));
        }
      }
      this.SetToken(JsonToken.None);
      return new DateTime?();
    }

    private bool IsWrappedInTypeObject()
    {
      this._readType = ReadType.Read;
      if (this.TokenType != JsonToken.StartObject)
        return false;
      if (!this.ReadInternal())
        throw JsonReaderException.Create(this, "Unexpected end when reading bytes.");
      if (this.Value.ToString() == "$type")
      {
        this.ReadInternal();
        if (this.Value != null && this.Value.ToString().StartsWith("System.Byte[]"))
        {
          this.ReadInternal();
          if (this.Value.ToString() == "$value")
            return true;
        }
      }
      throw JsonReaderException.Create(this, StringUtils.FormatWith("Error reading bytes. Unexpected token: {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) JsonToken.StartObject));
    }

    public void Skip()
    {
      if (this.TokenType == JsonToken.PropertyName)
        this.Read();
      if (!JsonReader.IsStartToken(this.TokenType))
        return;
      int depth = this.Depth;
      do
        ;
      while (this.Read() && depth < this.Depth);
    }

    protected void SetToken(JsonToken newToken)
    {
      this.SetToken(newToken, (object) null);
    }

    protected void SetToken(JsonToken newToken, object value)
    {
      this._tokenType = newToken;
      this._value = value;
      switch (newToken)
      {
        case JsonToken.StartObject:
          this._currentState = JsonReader.State.ObjectStart;
          this.Push(JsonContainerType.Object);
          break;
        case JsonToken.StartArray:
          this._currentState = JsonReader.State.ArrayStart;
          this.Push(JsonContainerType.Array);
          break;
        case JsonToken.StartConstructor:
          this._currentState = JsonReader.State.ConstructorStart;
          this.Push(JsonContainerType.Constructor);
          break;
        case JsonToken.PropertyName:
          this._currentState = JsonReader.State.Property;
          this._currentPosition.PropertyName = (string) value;
          break;
        case JsonToken.Raw:
        case JsonToken.Integer:
        case JsonToken.Float:
        case JsonToken.String:
        case JsonToken.Boolean:
        case JsonToken.Null:
        case JsonToken.Undefined:
        case JsonToken.Date:
        case JsonToken.Bytes:
          this._currentState = this.Peek() != JsonContainerType.None ? JsonReader.State.PostValue : JsonReader.State.Finished;
          this.UpdateScopeWithFinishedValue();
          break;
        case JsonToken.EndObject:
          this.ValidateEnd(JsonToken.EndObject);
          break;
        case JsonToken.EndArray:
          this.ValidateEnd(JsonToken.EndArray);
          break;
        case JsonToken.EndConstructor:
          this.ValidateEnd(JsonToken.EndConstructor);
          break;
      }
    }

    private void UpdateScopeWithFinishedValue()
    {
      if (this._currentPosition.Type != JsonContainerType.Array && this._currentPosition.Type != JsonContainerType.Constructor)
        return;
      if (!this._currentPosition.Position.HasValue)
      {
        this._currentPosition.Position = new int?(0);
      }
      else
      {
        // ISSUE: explicit reference operation
        // ISSUE: variable of a reference type
        JsonPosition& local = @this._currentPosition;
        // ISSUE: explicit reference operation
        int? nullable1 = (^local).Position;
        int? nullable2 = nullable1.HasValue ? new int?(nullable1.GetValueOrDefault() + 1) : new int?();
        // ISSUE: explicit reference operation
        (^local).Position = nullable2;
      }
    }

    private void ValidateEnd(JsonToken endToken)
    {
      JsonContainerType jsonContainerType = this.Pop();
      if (this.GetTypeForCloseToken(endToken) != jsonContainerType)
        throw JsonReaderException.Create(this, StringUtils.FormatWith("JsonToken {0} is not valid for closing JsonType {1}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) endToken, (object) jsonContainerType));
      this._currentState = this.Peek() != JsonContainerType.None ? JsonReader.State.PostValue : JsonReader.State.Finished;
    }

    protected void SetStateBasedOnCurrent()
    {
      JsonContainerType jsonContainerType = this.Peek();
      switch (jsonContainerType)
      {
        case JsonContainerType.None:
          this._currentState = JsonReader.State.Finished;
          break;
        case JsonContainerType.Object:
          this._currentState = JsonReader.State.Object;
          break;
        case JsonContainerType.Array:
          this._currentState = JsonReader.State.Array;
          break;
        case JsonContainerType.Constructor:
          this._currentState = JsonReader.State.Constructor;
          break;
        default:
          throw JsonReaderException.Create(this, StringUtils.FormatWith("While setting the reader state back to current object an unexpected JsonType was encountered: {0}", (IFormatProvider) CultureInfo.InvariantCulture, (object) jsonContainerType));
      }
    }

    internal static bool IsPrimitiveToken(JsonToken token)
    {
      switch (token)
      {
        case JsonToken.Integer:
        case JsonToken.Float:
        case JsonToken.String:
        case JsonToken.Boolean:
        case JsonToken.Null:
        case JsonToken.Undefined:
        case JsonToken.Date:
        case JsonToken.Bytes:
          return true;
        default:
          return false;
      }
    }

    internal static bool IsStartToken(JsonToken token)
    {
      switch (token)
      {
        case JsonToken.StartObject:
        case JsonToken.StartArray:
        case JsonToken.StartConstructor:
          return true;
        default:
          return false;
      }
    }

    private JsonContainerType GetTypeForCloseToken(JsonToken token)
    {
      switch (token)
      {
        case JsonToken.EndObject:
          return JsonContainerType.Object;
        case JsonToken.EndArray:
          return JsonContainerType.Array;
        case JsonToken.EndConstructor:
          return JsonContainerType.Constructor;
        default:
          throw JsonReaderException.Create(this, StringUtils.FormatWith("Not a valid close JsonToken: {0}", (IFormatProvider) CultureInfo.InvariantCulture, (object) token));
      }
    }

    void IDisposable.Dispose()
    {
      this.Dispose(true);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this._currentState == JsonReader.State.Closed || !disposing)
        return;
      this.Close();
    }

    public virtual void Close()
    {
      this._currentState = JsonReader.State.Closed;
      this._tokenType = JsonToken.None;
      this._value = (object) null;
    }

    protected internal enum State
    {
      Start,
      Complete,
      Property,
      ObjectStart,
      Object,
      ArrayStart,
      Array,
      Closed,
      PostValue,
      ConstructorStart,
      Constructor,
      Error,
      Finished,
    }
  }
}
