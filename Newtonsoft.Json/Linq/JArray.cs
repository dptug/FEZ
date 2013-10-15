// Type: Newtonsoft.Json.Linq.JArray
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Newtonsoft.Json.Linq
{
  public class JArray : JContainer, IList<JToken>, ICollection<JToken>, IEnumerable<JToken>, IEnumerable
  {
    private readonly List<JToken> _values = new List<JToken>();

    protected override IList<JToken> ChildrenTokens
    {
      get
      {
        return (IList<JToken>) this._values;
      }
    }

    public override JTokenType Type
    {
      get
      {
        return JTokenType.Array;
      }
    }

    public override JToken this[object key]
    {
      get
      {
        ValidationUtils.ArgumentNotNull(key, "o");
        if (!(key is int))
          throw new ArgumentException(StringUtils.FormatWith("Accessed JArray values with invalid key value: {0}. Array position index expected.", (IFormatProvider) CultureInfo.InvariantCulture, (object) MiscellaneousUtils.ToString(key)));
        else
          return this.GetItem((int) key);
      }
      set
      {
        ValidationUtils.ArgumentNotNull(key, "o");
        if (!(key is int))
          throw new ArgumentException(StringUtils.FormatWith("Set JArray values with invalid key value: {0}. Array position index expected.", (IFormatProvider) CultureInfo.InvariantCulture, (object) MiscellaneousUtils.ToString(key)));
        this.SetItem((int) key, value);
      }
    }

    public JToken this[int index]
    {
      get
      {
        return this.GetItem(index);
      }
      set
      {
        this.SetItem(index, value);
      }
    }

    bool ICollection<JToken>.IsReadOnly
    {
      get
      {
        return false;
      }
    }

    public JArray()
    {
    }

    public JArray(JArray other)
      : base((JContainer) other)
    {
    }

    public JArray(params object[] content)
      : this((object) content)
    {
    }

    public JArray(object content)
    {
      base.Add(content);
    }

    internal override bool DeepEquals(JToken node)
    {
      JArray jarray = node as JArray;
      if (jarray != null)
        return this.ContentsEqual((JContainer) jarray);
      else
        return false;
    }

    internal override JToken CloneToken()
    {
      return (JToken) new JArray(this);
    }

    public static JArray Load(JsonReader reader)
    {
      if (reader.TokenType == JsonToken.None && !reader.Read())
        throw JsonReaderException.Create(reader, "Error reading JArray from JsonReader.");
      while (reader.TokenType == JsonToken.Comment)
        reader.Read();
      if (reader.TokenType != JsonToken.StartArray)
        throw JsonReaderException.Create(reader, StringUtils.FormatWith("Error reading JArray from JsonReader. Current JsonReader item is not an array: {0}", (IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType));
      JArray jarray = new JArray();
      jarray.SetLineInfo(reader as IJsonLineInfo);
      jarray.ReadTokenFrom(reader);
      return jarray;
    }

    public static JArray Parse(string json)
    {
      JsonReader reader = (JsonReader) new JsonTextReader((TextReader) new StringReader(json));
      JArray jarray = JArray.Load(reader);
      if (reader.Read() && reader.TokenType != JsonToken.Comment)
        throw JsonReaderException.Create(reader, "Additional text found in JSON string after parsing content.");
      else
        return jarray;
    }

    public static JArray FromObject(object o)
    {
      return JArray.FromObject(o, new JsonSerializer());
    }

    public static JArray FromObject(object o, JsonSerializer jsonSerializer)
    {
      JToken jtoken = JToken.FromObjectInternal(o, jsonSerializer);
      if (jtoken.Type != JTokenType.Array)
        throw new ArgumentException(StringUtils.FormatWith("Object serialized to {0}. JArray instance expected.", (IFormatProvider) CultureInfo.InvariantCulture, (object) jtoken.Type));
      else
        return (JArray) jtoken;
    }

    public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
    {
      writer.WriteStartArray();
      for (int index = 0; index < this._values.Count; ++index)
        this._values[index].WriteTo(writer, converters);
      writer.WriteEndArray();
    }

    public int IndexOf(JToken item)
    {
      return this.IndexOfItem(item);
    }

    public void Insert(int index, JToken item)
    {
      this.InsertItem(index, item, false);
    }

    public void RemoveAt(int index)
    {
      this.RemoveItemAt(index);
    }

    public void Add(JToken item)
    {
      base.Add((object) item);
    }

    public void Clear()
    {
      this.ClearItems();
    }

    public bool Contains(JToken item)
    {
      return this.ContainsItem(item);
    }

    void ICollection<JToken>.CopyTo(JToken[] array, int arrayIndex)
    {
      this.CopyItemsTo((Array) array, arrayIndex);
    }

    public bool Remove(JToken item)
    {
      return this.RemoveItem(item);
    }

    internal override int GetDeepHashCode()
    {
      return this.ContentsHashCode();
    }
  }
}
