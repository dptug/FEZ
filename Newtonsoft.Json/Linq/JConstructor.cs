// Type: Newtonsoft.Json.Linq.JConstructor
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
  public class JConstructor : JContainer
  {
    private readonly List<JToken> _values = new List<JToken>();
    private string _name;

    protected override IList<JToken> ChildrenTokens
    {
      get
      {
        return (IList<JToken>) this._values;
      }
    }

    public string Name
    {
      get
      {
        return this._name;
      }
      set
      {
        this._name = value;
      }
    }

    public override JTokenType Type
    {
      get
      {
        return JTokenType.Constructor;
      }
    }

    public override JToken this[object key]
    {
      get
      {
        ValidationUtils.ArgumentNotNull(key, "o");
        if (!(key is int))
          throw new ArgumentException(StringUtils.FormatWith("Accessed JConstructor values with invalid key value: {0}. Argument position index expected.", (IFormatProvider) CultureInfo.InvariantCulture, (object) MiscellaneousUtils.ToString(key)));
        else
          return this.GetItem((int) key);
      }
      set
      {
        ValidationUtils.ArgumentNotNull(key, "o");
        if (!(key is int))
          throw new ArgumentException(StringUtils.FormatWith("Set JConstructor values with invalid key value: {0}. Argument position index expected.", (IFormatProvider) CultureInfo.InvariantCulture, (object) MiscellaneousUtils.ToString(key)));
        this.SetItem((int) key, value);
      }
    }

    public JConstructor()
    {
    }

    public JConstructor(JConstructor other)
      : base((JContainer) other)
    {
      this._name = other.Name;
    }

    public JConstructor(string name, params object[] content)
      : this(name, (object) content)
    {
    }

    public JConstructor(string name, object content)
      : this(name)
    {
      this.Add(content);
    }

    public JConstructor(string name)
    {
      ValidationUtils.ArgumentNotNullOrEmpty(name, "name");
      this._name = name;
    }

    internal override bool DeepEquals(JToken node)
    {
      JConstructor jconstructor = node as JConstructor;
      if (jconstructor != null && this._name == jconstructor.Name)
        return this.ContentsEqual((JContainer) jconstructor);
      else
        return false;
    }

    internal override JToken CloneToken()
    {
      return (JToken) new JConstructor(this);
    }

    public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
    {
      writer.WriteStartConstructor(this._name);
      foreach (JToken jtoken in this.Children())
        jtoken.WriteTo(writer, converters);
      writer.WriteEndConstructor();
    }

    internal override int GetDeepHashCode()
    {
      return this._name.GetHashCode() ^ this.ContentsHashCode();
    }

    public static JConstructor Load(JsonReader reader)
    {
      if (reader.TokenType == JsonToken.None && !reader.Read())
        throw JsonReaderException.Create(reader, "Error reading JConstructor from JsonReader.");
      while (reader.TokenType == JsonToken.Comment)
        reader.Read();
      if (reader.TokenType != JsonToken.StartConstructor)
        throw JsonReaderException.Create(reader, StringUtils.FormatWith("Error reading JConstructor from JsonReader. Current JsonReader item is not a constructor: {0}", (IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType));
      JConstructor jconstructor = new JConstructor((string) reader.Value);
      jconstructor.SetLineInfo(reader as IJsonLineInfo);
      jconstructor.ReadTokenFrom(reader);
      return jconstructor;
    }
  }
}
