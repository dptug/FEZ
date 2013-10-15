// Type: Newtonsoft.Json.Linq.JProperty
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace Newtonsoft.Json.Linq
{
  public class JProperty : JContainer
  {
    private readonly List<JToken> _content = new List<JToken>();
    private readonly string _name;

    protected override IList<JToken> ChildrenTokens
    {
      get
      {
        return (IList<JToken>) this._content;
      }
    }

    public string Name
    {
      [DebuggerStepThrough] get
      {
        return this._name;
      }
    }

    public JToken Value
    {
      [DebuggerStepThrough] get
      {
        if (this._content.Count <= 0)
          return (JToken) null;
        else
          return this._content[0];
      }
      set
      {
        this.CheckReentrancy();
        JToken jtoken = value ?? (JToken) new JValue((object) null);
        if (this._content.Count == 0)
          this.InsertItem(0, jtoken, false);
        else
          this.SetItem(0, jtoken);
      }
    }

    public override JTokenType Type
    {
      [DebuggerStepThrough] get
      {
        return JTokenType.Property;
      }
    }

    public JProperty(JProperty other)
      : base((JContainer) other)
    {
      this._name = other.Name;
    }

    internal JProperty(string name)
    {
      ValidationUtils.ArgumentNotNull((object) name, "name");
      this._name = name;
    }

    public JProperty(string name, params object[] content)
      : this(name, (object) content)
    {
    }

    public JProperty(string name, object content)
    {
      ValidationUtils.ArgumentNotNull((object) name, "name");
      this._name = name;
      this.Value = this.IsMultiContent(content) ? (JToken) new JArray(content) : this.CreateFromContent(content);
    }

    internal override JToken GetItem(int index)
    {
      if (index != 0)
        throw new ArgumentOutOfRangeException();
      else
        return this.Value;
    }

    internal override void SetItem(int index, JToken item)
    {
      if (index != 0)
        throw new ArgumentOutOfRangeException();
      if (JContainer.IsTokenUnchanged(this.Value, item))
        return;
      if (this.Parent != null)
        ((JObject) this.Parent).InternalPropertyChanging(this);
      base.SetItem(0, item);
      if (this.Parent == null)
        return;
      ((JObject) this.Parent).InternalPropertyChanged(this);
    }

    internal override bool RemoveItem(JToken item)
    {
      throw new JsonException(StringUtils.FormatWith("Cannot add or remove items from {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) typeof (JProperty)));
    }

    internal override void RemoveItemAt(int index)
    {
      throw new JsonException(StringUtils.FormatWith("Cannot add or remove items from {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) typeof (JProperty)));
    }

    internal override void InsertItem(int index, JToken item, bool skipParentCheck)
    {
      if (this.Value != null)
        throw new JsonException(StringUtils.FormatWith("{0} cannot have multiple values.", (IFormatProvider) CultureInfo.InvariantCulture, (object) typeof (JProperty)));
      base.InsertItem(0, item, false);
    }

    internal override bool ContainsItem(JToken item)
    {
      return this.Value == item;
    }

    internal override void ClearItems()
    {
      throw new JsonException(StringUtils.FormatWith("Cannot add or remove items from {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) typeof (JProperty)));
    }

    internal override bool DeepEquals(JToken node)
    {
      JProperty jproperty = node as JProperty;
      if (jproperty != null && this._name == jproperty.Name)
        return this.ContentsEqual((JContainer) jproperty);
      else
        return false;
    }

    internal override JToken CloneToken()
    {
      return (JToken) new JProperty(this);
    }

    public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
    {
      writer.WritePropertyName(this._name);
      JToken jtoken = this.Value;
      if (jtoken != null)
        jtoken.WriteTo(writer, converters);
      else
        writer.WriteNull();
    }

    internal override int GetDeepHashCode()
    {
      return this._name.GetHashCode() ^ (this.Value != null ? this.Value.GetDeepHashCode() : 0);
    }

    public static JProperty Load(JsonReader reader)
    {
      if (reader.TokenType == JsonToken.None && !reader.Read())
        throw JsonReaderException.Create(reader, "Error reading JProperty from JsonReader.");
      while (reader.TokenType == JsonToken.Comment)
        reader.Read();
      if (reader.TokenType != JsonToken.PropertyName)
        throw JsonReaderException.Create(reader, StringUtils.FormatWith("Error reading JProperty from JsonReader. Current JsonReader item is not a property: {0}", (IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType));
      JProperty jproperty = new JProperty((string) reader.Value);
      jproperty.SetLineInfo(reader as IJsonLineInfo);
      jproperty.ReadTokenFrom(reader);
      return jproperty;
    }
  }
}
