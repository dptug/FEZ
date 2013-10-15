// Type: Newtonsoft.Json.Linq.JObject
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;
using Newtonsoft.Json.Utilities.LinqBridge;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;

namespace Newtonsoft.Json.Linq
{
  public class JObject : JContainer, IDictionary<string, JToken>, ICollection<KeyValuePair<string, JToken>>, IEnumerable<KeyValuePair<string, JToken>>, IEnumerable, INotifyPropertyChanged, ICustomTypeDescriptor
  {
    private readonly JPropertyKeyedCollection _properties = new JPropertyKeyedCollection();

    protected override IList<JToken> ChildrenTokens
    {
      get
      {
        return (IList<JToken>) this._properties;
      }
    }

    public override JTokenType Type
    {
      get
      {
        return JTokenType.Object;
      }
    }

    public override JToken this[object key]
    {
      get
      {
        ValidationUtils.ArgumentNotNull(key, "o");
        string index = key as string;
        if (index == null)
          throw new ArgumentException(StringUtils.FormatWith("Accessed JObject values with invalid key value: {0}. Object property name expected.", (IFormatProvider) CultureInfo.InvariantCulture, (object) MiscellaneousUtils.ToString(key)));
        else
          return this[index];
      }
      set
      {
        ValidationUtils.ArgumentNotNull(key, "o");
        string index = key as string;
        if (index == null)
          throw new ArgumentException(StringUtils.FormatWith("Set JObject values with invalid key value: {0}. Object property name expected.", (IFormatProvider) CultureInfo.InvariantCulture, (object) MiscellaneousUtils.ToString(key)));
        this[index] = value;
      }
    }

    public JToken this[string propertyName]
    {
      get
      {
        ValidationUtils.ArgumentNotNull((object) propertyName, "propertyName");
        JProperty jproperty = this.Property(propertyName);
        if (jproperty == null)
          return (JToken) null;
        else
          return jproperty.Value;
      }
      set
      {
        JProperty jproperty = this.Property(propertyName);
        if (jproperty != null)
        {
          jproperty.Value = value;
        }
        else
        {
          base.Add((object) new JProperty(propertyName, (object) value));
          this.OnPropertyChanged(propertyName);
        }
      }
    }

    ICollection<string> IDictionary<string, JToken>.Keys
    {
      get
      {
        return this._properties.Keys;
      }
    }

    ICollection<JToken> IDictionary<string, JToken>.Values
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    bool ICollection<KeyValuePair<string, JToken>>.IsReadOnly
    {
      get
      {
        return false;
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public JObject()
    {
    }

    public JObject(JObject other)
      : base((JContainer) other)
    {
    }

    public JObject(params object[] content)
      : this((object) content)
    {
    }

    public JObject(object content)
    {
      base.Add(content);
    }

    internal override bool DeepEquals(JToken node)
    {
      JObject jobject = node as JObject;
      if (jobject == null)
        return false;
      else
        return this._properties.Compare(jobject._properties);
    }

    internal override void InsertItem(int index, JToken item, bool skipParentCheck)
    {
      if (item != null && item.Type == JTokenType.Comment)
        return;
      base.InsertItem(index, item, skipParentCheck);
    }

    internal override void ValidateToken(JToken o, JToken existing)
    {
      ValidationUtils.ArgumentNotNull((object) o, "o");
      if (o.Type != JTokenType.Property)
        throw new ArgumentException(StringUtils.FormatWith("Can not add {0} to {1}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) o.GetType(), (object) this.GetType()));
      JProperty jproperty1 = (JProperty) o;
      if (existing != null)
      {
        JProperty jproperty2 = (JProperty) existing;
        if (jproperty1.Name == jproperty2.Name)
          return;
      }
      if (this._properties.TryGetValue(jproperty1.Name, out existing))
        throw new ArgumentException(StringUtils.FormatWith("Can not add property {0} to {1}. Property with the same name already exists on object.", (IFormatProvider) CultureInfo.InvariantCulture, (object) jproperty1.Name, (object) this.GetType()));
    }

    internal void InternalPropertyChanged(JProperty childProperty)
    {
      this.OnPropertyChanged(childProperty.Name);
      if (this._listChanged == null)
        return;
      this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, this.IndexOfItem((JToken) childProperty)));
    }

    internal void InternalPropertyChanging(JProperty childProperty)
    {
    }

    internal override JToken CloneToken()
    {
      return (JToken) new JObject(this);
    }

    public IEnumerable<JProperty> Properties()
    {
      return Enumerable.Cast<JProperty>((IEnumerable) this._properties);
    }

    public JProperty Property(string name)
    {
      if (name == null)
        return (JProperty) null;
      JToken jtoken;
      this._properties.TryGetValue(name, out jtoken);
      return (JProperty) jtoken;
    }

    public JEnumerable<JToken> PropertyValues()
    {
      return new JEnumerable<JToken>(Enumerable.Select<JProperty, JToken>(this.Properties(), (Func<JProperty, JToken>) (p => p.Value)));
    }

    public static JObject Load(JsonReader reader)
    {
      ValidationUtils.ArgumentNotNull((object) reader, "reader");
      if (reader.TokenType == JsonToken.None && !reader.Read())
        throw JsonReaderException.Create(reader, "Error reading JObject from JsonReader.");
      while (reader.TokenType == JsonToken.Comment)
        reader.Read();
      if (reader.TokenType != JsonToken.StartObject)
        throw JsonReaderException.Create(reader, StringUtils.FormatWith("Error reading JObject from JsonReader. Current JsonReader item is not an object: {0}", (IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType));
      JObject jobject = new JObject();
      jobject.SetLineInfo(reader as IJsonLineInfo);
      jobject.ReadTokenFrom(reader);
      return jobject;
    }

    public static JObject Parse(string json)
    {
      JsonReader reader = (JsonReader) new JsonTextReader((TextReader) new StringReader(json));
      JObject jobject = JObject.Load(reader);
      if (reader.Read() && reader.TokenType != JsonToken.Comment)
        throw JsonReaderException.Create(reader, "Additional text found in JSON string after parsing content.");
      else
        return jobject;
    }

    public static JObject FromObject(object o)
    {
      return JObject.FromObject(o, new JsonSerializer());
    }

    public static JObject FromObject(object o, JsonSerializer jsonSerializer)
    {
      JToken jtoken = JToken.FromObjectInternal(o, jsonSerializer);
      if (jtoken != null && jtoken.Type != JTokenType.Object)
        throw new ArgumentException(StringUtils.FormatWith("Object serialized to {0}. JObject instance expected.", (IFormatProvider) CultureInfo.InvariantCulture, (object) jtoken.Type));
      else
        return (JObject) jtoken;
    }

    public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
    {
      writer.WriteStartObject();
      for (int index = 0; index < this._properties.Count; ++index)
        ((Collection<JToken>) this._properties)[index].WriteTo(writer, converters);
      writer.WriteEndObject();
    }

    public JToken GetValue(string propertyName)
    {
      return this.GetValue(propertyName, StringComparison.Ordinal);
    }

    public JToken GetValue(string propertyName, StringComparison comparison)
    {
      if (propertyName == null)
        return (JToken) null;
      JProperty jproperty1 = this.Property(propertyName);
      if (jproperty1 != null)
        return jproperty1.Value;
      if (comparison != StringComparison.Ordinal)
      {
        foreach (JProperty jproperty2 in (Collection<JToken>) this._properties)
        {
          if (string.Equals(jproperty2.Name, propertyName, comparison))
            return jproperty2.Value;
        }
      }
      return (JToken) null;
    }

    public bool TryGetValue(string propertyName, StringComparison comparison, out JToken value)
    {
      value = this.GetValue(propertyName, comparison);
      return value != null;
    }

    public void Add(string propertyName, JToken value)
    {
      base.Add((object) new JProperty(propertyName, (object) value));
    }

    bool IDictionary<string, JToken>.ContainsKey(string key)
    {
      return this._properties.Contains(key);
    }

    public bool Remove(string propertyName)
    {
      JProperty jproperty = this.Property(propertyName);
      if (jproperty == null)
        return false;
      jproperty.Remove();
      return true;
    }

    public bool TryGetValue(string propertyName, out JToken value)
    {
      JProperty jproperty = this.Property(propertyName);
      if (jproperty == null)
      {
        value = (JToken) null;
        return false;
      }
      else
      {
        value = jproperty.Value;
        return true;
      }
    }

    void ICollection<KeyValuePair<string, JToken>>.Add(KeyValuePair<string, JToken> item)
    {
      base.Add((object) new JProperty(item.Key, (object) item.Value));
    }

    void ICollection<KeyValuePair<string, JToken>>.Clear()
    {
      this.RemoveAll();
    }

    bool ICollection<KeyValuePair<string, JToken>>.Contains(KeyValuePair<string, JToken> item)
    {
      JProperty jproperty = this.Property(item.Key);
      if (jproperty == null)
        return false;
      else
        return jproperty.Value == item.Value;
    }

    void ICollection<KeyValuePair<string, JToken>>.CopyTo(KeyValuePair<string, JToken>[] array, int arrayIndex)
    {
      if (array == null)
        throw new ArgumentNullException("array");
      if (arrayIndex < 0)
        throw new ArgumentOutOfRangeException("arrayIndex", "arrayIndex is less than 0.");
      if (arrayIndex >= array.Length && arrayIndex != 0)
        throw new ArgumentException("arrayIndex is equal to or greater than the length of array.");
      if (this.Count > array.Length - arrayIndex)
        throw new ArgumentException("The number of elements in the source JObject is greater than the available space from arrayIndex to the end of the destination array.");
      int num = 0;
      foreach (JProperty jproperty in (Collection<JToken>) this._properties)
      {
        array[arrayIndex + num] = new KeyValuePair<string, JToken>(jproperty.Name, jproperty.Value);
        ++num;
      }
    }

    bool ICollection<KeyValuePair<string, JToken>>.Remove(KeyValuePair<string, JToken> item)
    {
      if (!this.Contains(item))
        return false;
      this.Remove(item.Key);
      return true;
    }

    internal override int GetDeepHashCode()
    {
      return this.ContentsHashCode();
    }

    public IEnumerator<KeyValuePair<string, JToken>> GetEnumerator()
    {
      foreach (JProperty jproperty in (Collection<JToken>) this._properties)
        yield return new KeyValuePair<string, JToken>(jproperty.Name, jproperty.Value);
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
      if (this.PropertyChanged == null)
        return;
      this.PropertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
    }

    PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
    {
      return this.GetProperties((Attribute[]) null);
    }

    private static Type GetTokenPropertyType(JToken token)
    {
      if (!(token is JValue))
        return token.GetType();
      JValue jvalue = (JValue) token;
      if (jvalue.Value == null)
        return typeof (object);
      else
        return jvalue.Value.GetType();
    }

    PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
    {
      PropertyDescriptorCollection descriptorCollection = new PropertyDescriptorCollection((PropertyDescriptor[]) null);
      foreach (KeyValuePair<string, JToken> keyValuePair in this)
        descriptorCollection.Add((PropertyDescriptor) new JPropertyDescriptor(keyValuePair.Key, JObject.GetTokenPropertyType(keyValuePair.Value)));
      return descriptorCollection;
    }

    AttributeCollection ICustomTypeDescriptor.GetAttributes()
    {
      return AttributeCollection.Empty;
    }

    string ICustomTypeDescriptor.GetClassName()
    {
      return (string) null;
    }

    string ICustomTypeDescriptor.GetComponentName()
    {
      return (string) null;
    }

    TypeConverter ICustomTypeDescriptor.GetConverter()
    {
      return new TypeConverter();
    }

    EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
    {
      return (EventDescriptor) null;
    }

    PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
    {
      return (PropertyDescriptor) null;
    }

    object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
    {
      return (object) null;
    }

    EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
    {
      return EventDescriptorCollection.Empty;
    }

    EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
    {
      return EventDescriptorCollection.Empty;
    }

    object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
    {
      return (object) null;
    }
  }
}
