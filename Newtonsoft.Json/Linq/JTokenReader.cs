// Type: Newtonsoft.Json.Linq.JTokenReader
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;
using System;

namespace Newtonsoft.Json.Linq
{
  public class JTokenReader : JsonReader, IJsonLineInfo
  {
    private readonly JToken _root;
    private JToken _parent;
    private JToken _current;

    private bool IsEndElement
    {
      get
      {
        return this._current == this._parent;
      }
    }

    int IJsonLineInfo.LineNumber
    {
      get
      {
        if (this.CurrentState == JsonReader.State.Start)
          return 0;
        IJsonLineInfo jsonLineInfo = this.IsEndElement ? (IJsonLineInfo) null : (IJsonLineInfo) this._current;
        if (jsonLineInfo != null)
          return jsonLineInfo.LineNumber;
        else
          return 0;
      }
    }

    int IJsonLineInfo.LinePosition
    {
      get
      {
        if (this.CurrentState == JsonReader.State.Start)
          return 0;
        IJsonLineInfo jsonLineInfo = this.IsEndElement ? (IJsonLineInfo) null : (IJsonLineInfo) this._current;
        if (jsonLineInfo != null)
          return jsonLineInfo.LinePosition;
        else
          return 0;
      }
    }

    public JTokenReader(JToken token)
    {
      ValidationUtils.ArgumentNotNull((object) token, "token");
      this._root = token;
      this._current = token;
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
      if (this.CurrentState != JsonReader.State.Start)
      {
        JContainer c = this._current as JContainer;
        if (c != null && this._parent != c)
          return this.ReadInto(c);
        else
          return this.ReadOver(this._current);
      }
      else
      {
        this.SetToken(this._current);
        return true;
      }
    }

    public override bool Read()
    {
      this._readType = ReadType.Read;
      return this.ReadInternal();
    }

    private bool ReadOver(JToken t)
    {
      if (t == this._root)
        return this.ReadToEnd();
      JToken next = t.Next;
      if (next == null || next == t || t == t.Parent.Last)
      {
        if (t.Parent == null)
          return this.ReadToEnd();
        else
          return this.SetEnd(t.Parent);
      }
      else
      {
        this._current = next;
        this.SetToken(this._current);
        return true;
      }
    }

    private bool ReadToEnd()
    {
      base.SetToken(JsonToken.None);
      return false;
    }

    private JsonToken? GetEndToken(JContainer c)
    {
      switch (c.Type)
      {
        case JTokenType.Object:
          return new JsonToken?(JsonToken.EndObject);
        case JTokenType.Array:
          return new JsonToken?(JsonToken.EndArray);
        case JTokenType.Constructor:
          return new JsonToken?(JsonToken.EndConstructor);
        case JTokenType.Property:
          return new JsonToken?();
        default:
          throw MiscellaneousUtils.CreateArgumentOutOfRangeException("Type", (object) c.Type, "Unexpected JContainer type.");
      }
    }

    private bool ReadInto(JContainer c)
    {
      JToken first = c.First;
      if (first == null)
        return this.SetEnd(c);
      this.SetToken(first);
      this._current = first;
      this._parent = (JToken) c;
      return true;
    }

    private bool SetEnd(JContainer c)
    {
      JsonToken? endToken = this.GetEndToken(c);
      if (!endToken.HasValue)
        return this.ReadOver((JToken) c);
      base.SetToken(endToken.Value);
      this._current = (JToken) c;
      this._parent = (JToken) c;
      return true;
    }

    private void SetToken(JToken token)
    {
      switch (token.Type)
      {
        case JTokenType.Object:
          base.SetToken(JsonToken.StartObject);
          break;
        case JTokenType.Array:
          base.SetToken(JsonToken.StartArray);
          break;
        case JTokenType.Constructor:
          base.SetToken(JsonToken.StartConstructor);
          break;
        case JTokenType.Property:
          base.SetToken(JsonToken.PropertyName, (object) ((JProperty) token).Name);
          break;
        case JTokenType.Comment:
          base.SetToken(JsonToken.Comment, ((JValue) token).Value);
          break;
        case JTokenType.Integer:
          base.SetToken(JsonToken.Integer, ((JValue) token).Value);
          break;
        case JTokenType.Float:
          base.SetToken(JsonToken.Float, ((JValue) token).Value);
          break;
        case JTokenType.String:
          base.SetToken(JsonToken.String, ((JValue) token).Value);
          break;
        case JTokenType.Boolean:
          base.SetToken(JsonToken.Boolean, ((JValue) token).Value);
          break;
        case JTokenType.Null:
          base.SetToken(JsonToken.Null, ((JValue) token).Value);
          break;
        case JTokenType.Undefined:
          base.SetToken(JsonToken.Undefined, ((JValue) token).Value);
          break;
        case JTokenType.Date:
          base.SetToken(JsonToken.Date, ((JValue) token).Value);
          break;
        case JTokenType.Raw:
          base.SetToken(JsonToken.Raw, ((JValue) token).Value);
          break;
        case JTokenType.Bytes:
          base.SetToken(JsonToken.Bytes, ((JValue) token).Value);
          break;
        case JTokenType.Guid:
          base.SetToken(JsonToken.String, (object) this.SafeToString(((JValue) token).Value));
          break;
        case JTokenType.Uri:
          base.SetToken(JsonToken.String, (object) this.SafeToString(((JValue) token).Value));
          break;
        case JTokenType.TimeSpan:
          base.SetToken(JsonToken.String, (object) this.SafeToString(((JValue) token).Value));
          break;
        default:
          throw MiscellaneousUtils.CreateArgumentOutOfRangeException("Type", (object) token.Type, "Unexpected JTokenType.");
      }
    }

    private string SafeToString(object value)
    {
      if (value == null)
        return (string) null;
      else
        return value.ToString();
    }

    bool IJsonLineInfo.HasLineInfo()
    {
      if (this.CurrentState == JsonReader.State.Start)
        return false;
      IJsonLineInfo jsonLineInfo = this.IsEndElement ? (IJsonLineInfo) null : (IJsonLineInfo) this._current;
      if (jsonLineInfo != null)
        return jsonLineInfo.HasLineInfo();
      else
        return false;
    }
  }
}
