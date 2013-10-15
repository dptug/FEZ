// Type: Newtonsoft.Json.JsonWriter
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
  public abstract class JsonWriter : IDisposable
  {
    internal static readonly JsonWriter.State[][] StateArrayTempate = new JsonWriter.State[8][]
    {
      new JsonWriter.State[10]
      {
        JsonWriter.State.Error,
        JsonWriter.State.Error,
        JsonWriter.State.Error,
        JsonWriter.State.Error,
        JsonWriter.State.Error,
        JsonWriter.State.Error,
        JsonWriter.State.Error,
        JsonWriter.State.Error,
        JsonWriter.State.Error,
        JsonWriter.State.Error
      },
      new JsonWriter.State[10]
      {
        JsonWriter.State.ObjectStart,
        JsonWriter.State.ObjectStart,
        JsonWriter.State.Error,
        JsonWriter.State.Error,
        JsonWriter.State.ObjectStart,
        JsonWriter.State.ObjectStart,
        JsonWriter.State.ObjectStart,
        JsonWriter.State.ObjectStart,
        JsonWriter.State.Error,
        JsonWriter.State.Error
      },
      new JsonWriter.State[10]
      {
        JsonWriter.State.ArrayStart,
        JsonWriter.State.ArrayStart,
        JsonWriter.State.Error,
        JsonWriter.State.Error,
        JsonWriter.State.ArrayStart,
        JsonWriter.State.ArrayStart,
        JsonWriter.State.ArrayStart,
        JsonWriter.State.ArrayStart,
        JsonWriter.State.Error,
        JsonWriter.State.Error
      },
      new JsonWriter.State[10]
      {
        JsonWriter.State.ConstructorStart,
        JsonWriter.State.ConstructorStart,
        JsonWriter.State.Error,
        JsonWriter.State.Error,
        JsonWriter.State.ConstructorStart,
        JsonWriter.State.ConstructorStart,
        JsonWriter.State.ConstructorStart,
        JsonWriter.State.ConstructorStart,
        JsonWriter.State.Error,
        JsonWriter.State.Error
      },
      new JsonWriter.State[10]
      {
        JsonWriter.State.Property,
        JsonWriter.State.Error,
        JsonWriter.State.Property,
        JsonWriter.State.Property,
        JsonWriter.State.Error,
        JsonWriter.State.Error,
        JsonWriter.State.Error,
        JsonWriter.State.Error,
        JsonWriter.State.Error,
        JsonWriter.State.Error
      },
      new JsonWriter.State[10]
      {
        JsonWriter.State.Start,
        JsonWriter.State.Property,
        JsonWriter.State.ObjectStart,
        JsonWriter.State.Object,
        JsonWriter.State.ArrayStart,
        JsonWriter.State.Array,
        JsonWriter.State.Constructor,
        JsonWriter.State.Constructor,
        JsonWriter.State.Error,
        JsonWriter.State.Error
      },
      new JsonWriter.State[10]
      {
        JsonWriter.State.Start,
        JsonWriter.State.Property,
        JsonWriter.State.ObjectStart,
        JsonWriter.State.Object,
        JsonWriter.State.ArrayStart,
        JsonWriter.State.Array,
        JsonWriter.State.Constructor,
        JsonWriter.State.Constructor,
        JsonWriter.State.Error,
        JsonWriter.State.Error
      },
      new JsonWriter.State[10]
      {
        JsonWriter.State.Start,
        JsonWriter.State.Object,
        JsonWriter.State.Error,
        JsonWriter.State.Error,
        JsonWriter.State.Array,
        JsonWriter.State.Array,
        JsonWriter.State.Constructor,
        JsonWriter.State.Constructor,
        JsonWriter.State.Error,
        JsonWriter.State.Error
      }
    };
    private static readonly JsonWriter.State[][] StateArray = JsonWriter.BuildStateArray();
    private readonly List<JsonPosition> _stack;
    private JsonPosition _currentPosition;
    private JsonWriter.State _currentState;
    private Formatting _formatting;
    private DateFormatHandling _dateFormatHandling;
    private DateTimeZoneHandling _dateTimeZoneHandling;

    public bool CloseOutput { get; set; }

    protected internal int Top
    {
      get
      {
        int count = this._stack.Count;
        if (this.Peek() != JsonContainerType.None)
          ++count;
        return count;
      }
    }

    internal string ContainerPath
    {
      get
      {
        if (this._currentPosition.Type == JsonContainerType.None)
          return string.Empty;
        IEnumerable<JsonPosition> positions;
        if (!this._currentPosition.InsideContainer())
          positions = Enumerable.Concat<JsonPosition>((IEnumerable<JsonPosition>) this._stack, (IEnumerable<JsonPosition>) new JsonPosition[1]
          {
            this._currentPosition
          });
        else
          positions = (IEnumerable<JsonPosition>) this._stack;
        return JsonPosition.BuildPath(positions);
      }
    }

    public WriteState WriteState
    {
      get
      {
        switch (this._currentState)
        {
          case JsonWriter.State.Start:
            return WriteState.Start;
          case JsonWriter.State.Property:
            return WriteState.Property;
          case JsonWriter.State.ObjectStart:
          case JsonWriter.State.Object:
            return WriteState.Object;
          case JsonWriter.State.ArrayStart:
          case JsonWriter.State.Array:
            return WriteState.Array;
          case JsonWriter.State.ConstructorStart:
          case JsonWriter.State.Constructor:
            return WriteState.Constructor;
          case JsonWriter.State.Closed:
            return WriteState.Closed;
          case JsonWriter.State.Error:
            return WriteState.Error;
          default:
            throw JsonWriterException.Create(this, "Invalid state: " + (object) this._currentState, (Exception) null);
        }
      }
    }

    public string Path
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

    public Formatting Formatting
    {
      get
      {
        return this._formatting;
      }
      set
      {
        this._formatting = value;
      }
    }

    public DateFormatHandling DateFormatHandling
    {
      get
      {
        return this._dateFormatHandling;
      }
      set
      {
        this._dateFormatHandling = value;
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

    static JsonWriter()
    {
    }

    protected JsonWriter()
    {
      this._stack = new List<JsonPosition>(4);
      this._currentState = JsonWriter.State.Start;
      this._formatting = Formatting.None;
      this._dateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind;
      this.CloseOutput = true;
    }

    internal static JsonWriter.State[][] BuildStateArray()
    {
      List<JsonWriter.State[]> list = Enumerable.ToList<JsonWriter.State[]>((IEnumerable<JsonWriter.State[]>) JsonWriter.StateArrayTempate);
      JsonWriter.State[] stateArray1 = JsonWriter.StateArrayTempate[0];
      JsonWriter.State[] stateArray2 = JsonWriter.StateArrayTempate[7];
      foreach (JsonToken jsonToken in (IEnumerable<object>) EnumUtils.GetValues(typeof (JsonToken)))
      {
        if ((JsonToken) list.Count <= jsonToken)
        {
          switch (jsonToken)
          {
            case JsonToken.Integer:
            case JsonToken.Float:
            case JsonToken.String:
            case JsonToken.Boolean:
            case JsonToken.Null:
            case JsonToken.Undefined:
            case JsonToken.Date:
            case JsonToken.Bytes:
              list.Add(stateArray2);
              continue;
            default:
              list.Add(stateArray1);
              continue;
          }
        }
      }
      return list.ToArray();
    }

    internal void UpdateScopeWithFinishedValue()
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

    private void Push(JsonContainerType value)
    {
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
      return jsonPosition.Type;
    }

    private JsonContainerType Peek()
    {
      return this._currentPosition.Type;
    }

    public abstract void Flush();

    public virtual void Close()
    {
      this.AutoCompleteAll();
    }

    public virtual void WriteStartObject()
    {
      this.UpdateScopeWithFinishedValue();
      this.AutoComplete(JsonToken.StartObject);
      this.Push(JsonContainerType.Object);
    }

    public virtual void WriteEndObject()
    {
      this.AutoCompleteClose(JsonContainerType.Object);
    }

    public virtual void WriteStartArray()
    {
      this.UpdateScopeWithFinishedValue();
      this.AutoComplete(JsonToken.StartArray);
      this.Push(JsonContainerType.Array);
    }

    public virtual void WriteEndArray()
    {
      this.AutoCompleteClose(JsonContainerType.Array);
    }

    public virtual void WriteStartConstructor(string name)
    {
      this.UpdateScopeWithFinishedValue();
      this.AutoComplete(JsonToken.StartConstructor);
      this.Push(JsonContainerType.Constructor);
    }

    public virtual void WriteEndConstructor()
    {
      this.AutoCompleteClose(JsonContainerType.Constructor);
    }

    public virtual void WritePropertyName(string name)
    {
      this._currentPosition.PropertyName = name;
      this.AutoComplete(JsonToken.PropertyName);
    }

    public virtual void WriteEnd()
    {
      this.WriteEnd(this.Peek());
    }

    public void WriteToken(JsonReader reader)
    {
      ValidationUtils.ArgumentNotNull((object) reader, "reader");
      int initialDepth = reader.TokenType != JsonToken.None ? (this.IsStartToken(reader.TokenType) ? reader.Depth : reader.Depth + 1) : -1;
      this.WriteToken(reader, initialDepth);
    }

    internal void WriteToken(JsonReader reader, int initialDepth)
    {
      do
      {
        switch (reader.TokenType)
        {
          case JsonToken.None:
            continue;
          case JsonToken.StartObject:
            this.WriteStartObject();
            goto case 0;
          case JsonToken.StartArray:
            this.WriteStartArray();
            goto case 0;
          case JsonToken.StartConstructor:
            if (string.Equals(reader.Value.ToString(), "Date", StringComparison.Ordinal))
            {
              this.WriteConstructorDate(reader);
              goto case 0;
            }
            else
            {
              this.WriteStartConstructor(reader.Value.ToString());
              goto case 0;
            }
          case JsonToken.PropertyName:
            this.WritePropertyName(reader.Value.ToString());
            goto case 0;
          case JsonToken.Comment:
            this.WriteComment(reader.Value != null ? reader.Value.ToString() : (string) null);
            goto case 0;
          case JsonToken.Raw:
            this.WriteRawValue(reader.Value != null ? reader.Value.ToString() : (string) null);
            goto case 0;
          case JsonToken.Integer:
            this.WriteValue(Convert.ToInt64(reader.Value, (IFormatProvider) CultureInfo.InvariantCulture));
            goto case 0;
          case JsonToken.Float:
            object obj = reader.Value;
            if (obj is Decimal)
            {
              this.WriteValue((Decimal) obj);
              goto case 0;
            }
            else if (obj is double)
            {
              this.WriteValue((double) obj);
              goto case 0;
            }
            else if (obj is float)
            {
              this.WriteValue((float) obj);
              goto case 0;
            }
            else
            {
              this.WriteValue(Convert.ToDouble(obj, (IFormatProvider) CultureInfo.InvariantCulture));
              goto case 0;
            }
          case JsonToken.String:
            this.WriteValue(reader.Value.ToString());
            goto case 0;
          case JsonToken.Boolean:
            this.WriteValue(Convert.ToBoolean(reader.Value, (IFormatProvider) CultureInfo.InvariantCulture));
            goto case 0;
          case JsonToken.Null:
            this.WriteNull();
            goto case 0;
          case JsonToken.Undefined:
            this.WriteUndefined();
            goto case 0;
          case JsonToken.EndObject:
            this.WriteEndObject();
            goto case 0;
          case JsonToken.EndArray:
            this.WriteEndArray();
            goto case 0;
          case JsonToken.EndConstructor:
            this.WriteEndConstructor();
            goto case 0;
          case JsonToken.Date:
            this.WriteValue(Convert.ToDateTime(reader.Value, (IFormatProvider) CultureInfo.InvariantCulture));
            goto case 0;
          case JsonToken.Bytes:
            this.WriteValue((byte[]) reader.Value);
            goto case 0;
          default:
            throw MiscellaneousUtils.CreateArgumentOutOfRangeException("TokenType", (object) reader.TokenType, "Unexpected token type.");
        }
      }
      while (initialDepth - 1 < reader.Depth - (this.IsEndToken(reader.TokenType) ? 1 : 0) && reader.Read());
    }

    private void WriteConstructorDate(JsonReader reader)
    {
      if (!reader.Read())
        throw JsonWriterException.Create(this, "Unexpected end when reading date constructor.", (Exception) null);
      if (reader.TokenType != JsonToken.Integer)
        throw JsonWriterException.Create(this, "Unexpected token when reading date constructor. Expected Integer, got " + (object) reader.TokenType, (Exception) null);
      DateTime dateTime = JsonConvert.ConvertJavaScriptTicksToDateTime((long) reader.Value);
      if (!reader.Read())
        throw JsonWriterException.Create(this, "Unexpected end when reading date constructor.", (Exception) null);
      if (reader.TokenType != JsonToken.EndConstructor)
        throw JsonWriterException.Create(this, "Unexpected token when reading date constructor. Expected EndConstructor, got " + (object) reader.TokenType, (Exception) null);
      this.WriteValue(dateTime);
    }

    private bool IsEndToken(JsonToken token)
    {
      switch (token)
      {
        case JsonToken.EndObject:
        case JsonToken.EndArray:
        case JsonToken.EndConstructor:
          return true;
        default:
          return false;
      }
    }

    private bool IsStartToken(JsonToken token)
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

    private void WriteEnd(JsonContainerType type)
    {
      switch (type)
      {
        case JsonContainerType.Object:
          this.WriteEndObject();
          break;
        case JsonContainerType.Array:
          this.WriteEndArray();
          break;
        case JsonContainerType.Constructor:
          this.WriteEndConstructor();
          break;
        default:
          throw JsonWriterException.Create(this, "Unexpected type when writing end: " + (object) type, (Exception) null);
      }
    }

    private void AutoCompleteAll()
    {
      while (this.Top > 0)
        this.WriteEnd();
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
          throw JsonWriterException.Create(this, "No type for token: " + (object) token, (Exception) null);
      }
    }

    private JsonToken GetCloseTokenForType(JsonContainerType type)
    {
      switch (type)
      {
        case JsonContainerType.Object:
          return JsonToken.EndObject;
        case JsonContainerType.Array:
          return JsonToken.EndArray;
        case JsonContainerType.Constructor:
          return JsonToken.EndConstructor;
        default:
          throw JsonWriterException.Create(this, "No close token for type: " + (object) type, (Exception) null);
      }
    }

    private void AutoCompleteClose(JsonContainerType type)
    {
      int num1 = 0;
      if (this._currentPosition.Type == type)
      {
        num1 = 1;
      }
      else
      {
        int num2 = this.Top - 2;
        for (int index = num2; index >= 0; --index)
        {
          if (this._stack[num2 - index].Type == type)
          {
            num1 = index + 2;
            break;
          }
        }
      }
      if (num1 == 0)
        throw JsonWriterException.Create(this, "No token to close.", (Exception) null);
      for (int index = 0; index < num1; ++index)
      {
        JsonToken closeTokenForType = this.GetCloseTokenForType(this.Pop());
        if (this._currentState == JsonWriter.State.Property)
          this.WriteNull();
        if (this._formatting == Formatting.Indented && this._currentState != JsonWriter.State.ObjectStart && this._currentState != JsonWriter.State.ArrayStart)
          this.WriteIndent();
        this.WriteEnd(closeTokenForType);
        JsonContainerType jsonContainerType = this.Peek();
        switch (jsonContainerType)
        {
          case JsonContainerType.None:
            this._currentState = JsonWriter.State.Start;
            break;
          case JsonContainerType.Object:
            this._currentState = JsonWriter.State.Object;
            break;
          case JsonContainerType.Array:
            this._currentState = JsonWriter.State.Array;
            break;
          case JsonContainerType.Constructor:
            this._currentState = JsonWriter.State.Array;
            break;
          default:
            throw JsonWriterException.Create(this, "Unknown JsonType: " + (object) jsonContainerType, (Exception) null);
        }
      }
    }

    protected virtual void WriteEnd(JsonToken token)
    {
    }

    protected virtual void WriteIndent()
    {
    }

    protected virtual void WriteValueDelimiter()
    {
    }

    protected virtual void WriteIndentSpace()
    {
    }

    internal void AutoComplete(JsonToken tokenBeingWritten)
    {
      JsonWriter.State state = JsonWriter.StateArray[(int) tokenBeingWritten][(int) this._currentState];
      if (state == JsonWriter.State.Error)
        throw JsonWriterException.Create(this, StringUtils.FormatWith("Token {0} in state {1} would result in an invalid JSON object.", (IFormatProvider) CultureInfo.InvariantCulture, (object) ((object) tokenBeingWritten).ToString(), (object) ((object) this._currentState).ToString()), (Exception) null);
      if ((this._currentState == JsonWriter.State.Object || this._currentState == JsonWriter.State.Array || this._currentState == JsonWriter.State.Constructor) && tokenBeingWritten != JsonToken.Comment)
        this.WriteValueDelimiter();
      else if (this._currentState == JsonWriter.State.Property && this._formatting == Formatting.Indented)
        this.WriteIndentSpace();
      if (this._formatting == Formatting.Indented)
      {
        WriteState writeState = this.WriteState;
        if (tokenBeingWritten == JsonToken.PropertyName && writeState != WriteState.Start || (writeState == WriteState.Array || writeState == WriteState.Constructor))
          this.WriteIndent();
      }
      this._currentState = state;
    }

    public virtual void WriteNull()
    {
      this.UpdateScopeWithFinishedValue();
      this.AutoComplete(JsonToken.Null);
    }

    public virtual void WriteUndefined()
    {
      this.UpdateScopeWithFinishedValue();
      this.AutoComplete(JsonToken.Undefined);
    }

    public virtual void WriteRaw(string json)
    {
    }

    public virtual void WriteRawValue(string json)
    {
      this.UpdateScopeWithFinishedValue();
      this.AutoComplete(JsonToken.Undefined);
      this.WriteRaw(json);
    }

    public virtual void WriteValue(string value)
    {
      this.UpdateScopeWithFinishedValue();
      this.AutoComplete(JsonToken.String);
    }

    public virtual void WriteValue(int value)
    {
      this.UpdateScopeWithFinishedValue();
      this.AutoComplete(JsonToken.Integer);
    }

    [CLSCompliant(false)]
    public virtual void WriteValue(uint value)
    {
      this.UpdateScopeWithFinishedValue();
      this.AutoComplete(JsonToken.Integer);
    }

    public virtual void WriteValue(long value)
    {
      this.UpdateScopeWithFinishedValue();
      this.AutoComplete(JsonToken.Integer);
    }

    [CLSCompliant(false)]
    public virtual void WriteValue(ulong value)
    {
      this.UpdateScopeWithFinishedValue();
      this.AutoComplete(JsonToken.Integer);
    }

    public virtual void WriteValue(float value)
    {
      this.UpdateScopeWithFinishedValue();
      this.AutoComplete(JsonToken.Float);
    }

    public virtual void WriteValue(double value)
    {
      this.UpdateScopeWithFinishedValue();
      this.AutoComplete(JsonToken.Float);
    }

    public virtual void WriteValue(bool value)
    {
      this.UpdateScopeWithFinishedValue();
      this.AutoComplete(JsonToken.Boolean);
    }

    public virtual void WriteValue(short value)
    {
      this.UpdateScopeWithFinishedValue();
      this.AutoComplete(JsonToken.Integer);
    }

    [CLSCompliant(false)]
    public virtual void WriteValue(ushort value)
    {
      this.UpdateScopeWithFinishedValue();
      this.AutoComplete(JsonToken.Integer);
    }

    public virtual void WriteValue(char value)
    {
      this.UpdateScopeWithFinishedValue();
      this.AutoComplete(JsonToken.String);
    }

    public virtual void WriteValue(byte value)
    {
      this.UpdateScopeWithFinishedValue();
      this.AutoComplete(JsonToken.Integer);
    }

    [CLSCompliant(false)]
    public virtual void WriteValue(sbyte value)
    {
      this.UpdateScopeWithFinishedValue();
      this.AutoComplete(JsonToken.Integer);
    }

    public virtual void WriteValue(Decimal value)
    {
      this.UpdateScopeWithFinishedValue();
      this.AutoComplete(JsonToken.Float);
    }

    public virtual void WriteValue(DateTime value)
    {
      this.UpdateScopeWithFinishedValue();
      this.AutoComplete(JsonToken.Date);
    }

    public virtual void WriteValue(Guid value)
    {
      this.UpdateScopeWithFinishedValue();
      this.AutoComplete(JsonToken.String);
    }

    public virtual void WriteValue(TimeSpan value)
    {
      this.UpdateScopeWithFinishedValue();
      this.AutoComplete(JsonToken.String);
    }

    public virtual void WriteValue(int? value)
    {
      if (!value.HasValue)
        this.WriteNull();
      else
        this.WriteValue(value.Value);
    }

    [CLSCompliant(false)]
    public virtual void WriteValue(uint? value)
    {
      if (!value.HasValue)
        this.WriteNull();
      else
        this.WriteValue(value.Value);
    }

    public virtual void WriteValue(long? value)
    {
      if (!value.HasValue)
        this.WriteNull();
      else
        this.WriteValue(value.Value);
    }

    [CLSCompliant(false)]
    public virtual void WriteValue(ulong? value)
    {
      if (!value.HasValue)
        this.WriteNull();
      else
        this.WriteValue(value.Value);
    }

    public virtual void WriteValue(float? value)
    {
      if (!value.HasValue)
        this.WriteNull();
      else
        this.WriteValue(value.Value);
    }

    public virtual void WriteValue(double? value)
    {
      if (!value.HasValue)
        this.WriteNull();
      else
        this.WriteValue(value.Value);
    }

    public virtual void WriteValue(bool? value)
    {
      if (!value.HasValue)
        this.WriteNull();
      else
        this.WriteValue(value.Value);
    }

    public virtual void WriteValue(short? value)
    {
      short? nullable = value;
      if (!(nullable.HasValue ? new int?((int) nullable.GetValueOrDefault()) : new int?()).HasValue)
        this.WriteNull();
      else
        this.WriteValue(value.Value);
    }

    [CLSCompliant(false)]
    public virtual void WriteValue(ushort? value)
    {
      ushort? nullable = value;
      if (!(nullable.HasValue ? new int?((int) nullable.GetValueOrDefault()) : new int?()).HasValue)
        this.WriteNull();
      else
        this.WriteValue(value.Value);
    }

    public virtual void WriteValue(char? value)
    {
      char? nullable = value;
      if (!(nullable.HasValue ? new int?((int) nullable.GetValueOrDefault()) : new int?()).HasValue)
        this.WriteNull();
      else
        this.WriteValue(value.Value);
    }

    public virtual void WriteValue(byte? value)
    {
      byte? nullable = value;
      if (!(nullable.HasValue ? new int?((int) nullable.GetValueOrDefault()) : new int?()).HasValue)
        this.WriteNull();
      else
        this.WriteValue(value.Value);
    }

    [CLSCompliant(false)]
    public virtual void WriteValue(sbyte? value)
    {
      sbyte? nullable = value;
      if (!(nullable.HasValue ? new int?((int) nullable.GetValueOrDefault()) : new int?()).HasValue)
        this.WriteNull();
      else
        this.WriteValue(value.Value);
    }

    public virtual void WriteValue(Decimal? value)
    {
      if (!value.HasValue)
        this.WriteNull();
      else
        this.WriteValue(value.Value);
    }

    public virtual void WriteValue(DateTime? value)
    {
      if (!value.HasValue)
        this.WriteNull();
      else
        this.WriteValue(value.Value);
    }

    public virtual void WriteValue(Guid? value)
    {
      if (!value.HasValue)
        this.WriteNull();
      else
        this.WriteValue(value.Value);
    }

    public virtual void WriteValue(TimeSpan? value)
    {
      if (!value.HasValue)
        this.WriteNull();
      else
        this.WriteValue(value.Value);
    }

    public virtual void WriteValue(byte[] value)
    {
      if (value == null)
      {
        this.WriteNull();
      }
      else
      {
        this.UpdateScopeWithFinishedValue();
        this.AutoComplete(JsonToken.Bytes);
      }
    }

    public virtual void WriteValue(Uri value)
    {
      if (value == (Uri) null)
      {
        this.WriteNull();
      }
      else
      {
        this.UpdateScopeWithFinishedValue();
        this.AutoComplete(JsonToken.String);
      }
    }

    public virtual void WriteValue(object value)
    {
      if (value == null)
      {
        this.WriteNull();
      }
      else
      {
        if (ConvertUtils.IsConvertible(value))
        {
          IConvertible convertible = ConvertUtils.ToConvertible(value);
          switch (convertible.GetTypeCode())
          {
            case TypeCode.DBNull:
              this.WriteNull();
              return;
            case TypeCode.Boolean:
              this.WriteValue(convertible.ToBoolean((IFormatProvider) CultureInfo.InvariantCulture));
              return;
            case TypeCode.Char:
              this.WriteValue(convertible.ToChar((IFormatProvider) CultureInfo.InvariantCulture));
              return;
            case TypeCode.SByte:
              this.WriteValue(convertible.ToSByte((IFormatProvider) CultureInfo.InvariantCulture));
              return;
            case TypeCode.Byte:
              this.WriteValue(convertible.ToByte((IFormatProvider) CultureInfo.InvariantCulture));
              return;
            case TypeCode.Int16:
              this.WriteValue(convertible.ToInt16((IFormatProvider) CultureInfo.InvariantCulture));
              return;
            case TypeCode.UInt16:
              this.WriteValue(convertible.ToUInt16((IFormatProvider) CultureInfo.InvariantCulture));
              return;
            case TypeCode.Int32:
              this.WriteValue(convertible.ToInt32((IFormatProvider) CultureInfo.InvariantCulture));
              return;
            case TypeCode.UInt32:
              this.WriteValue(convertible.ToUInt32((IFormatProvider) CultureInfo.InvariantCulture));
              return;
            case TypeCode.Int64:
              this.WriteValue(convertible.ToInt64((IFormatProvider) CultureInfo.InvariantCulture));
              return;
            case TypeCode.UInt64:
              this.WriteValue(convertible.ToUInt64((IFormatProvider) CultureInfo.InvariantCulture));
              return;
            case TypeCode.Single:
              this.WriteValue(convertible.ToSingle((IFormatProvider) CultureInfo.InvariantCulture));
              return;
            case TypeCode.Double:
              this.WriteValue(convertible.ToDouble((IFormatProvider) CultureInfo.InvariantCulture));
              return;
            case TypeCode.Decimal:
              this.WriteValue(convertible.ToDecimal((IFormatProvider) CultureInfo.InvariantCulture));
              return;
            case TypeCode.DateTime:
              this.WriteValue(convertible.ToDateTime((IFormatProvider) CultureInfo.InvariantCulture));
              return;
            case TypeCode.String:
              this.WriteValue(convertible.ToString((IFormatProvider) CultureInfo.InvariantCulture));
              return;
          }
        }
        else if (value is byte[])
        {
          this.WriteValue((byte[]) value);
          return;
        }
        else if (value is Guid)
        {
          this.WriteValue((Guid) value);
          return;
        }
        else if (value is Uri)
        {
          this.WriteValue((Uri) value);
          return;
        }
        else if (value is TimeSpan)
        {
          this.WriteValue((TimeSpan) value);
          return;
        }
        throw JsonWriterException.Create(this, StringUtils.FormatWith("Unsupported type: {0}. Use the JsonSerializer class to get the object's JSON representation.", (IFormatProvider) CultureInfo.InvariantCulture, (object) value.GetType()), (Exception) null);
      }
    }

    public virtual void WriteComment(string text)
    {
      this.UpdateScopeWithFinishedValue();
      this.AutoComplete(JsonToken.Comment);
    }

    public virtual void WriteWhitespace(string ws)
    {
      if (ws != null && !StringUtils.IsWhiteSpace(ws))
        throw JsonWriterException.Create(this, "Only white space characters should be used.", (Exception) null);
    }

    void IDisposable.Dispose()
    {
      this.Dispose(true);
    }

    private void Dispose(bool disposing)
    {
      if (this._currentState == JsonWriter.State.Closed)
        return;
      this.Close();
    }

    internal enum State
    {
      Start,
      Property,
      ObjectStart,
      Object,
      ArrayStart,
      Array,
      ConstructorStart,
      Constructor,
      Bytes,
      Closed,
      Error,
    }
  }
}
