// Type: Newtonsoft.Json.Converters.XmlNodeConverter
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
using System.Globalization;
using System.Xml;

namespace Newtonsoft.Json.Converters
{
  public class XmlNodeConverter : JsonConverter
  {
    private const string TextName = "#text";
    private const string CommentName = "#comment";
    private const string CDataName = "#cdata-section";
    private const string WhitespaceName = "#whitespace";
    private const string SignificantWhitespaceName = "#significant-whitespace";
    private const string DeclarationName = "?xml";
    private const string JsonNamespaceUri = "http://james.newtonking.com/projects/json";

    public string DeserializeRootElementName { get; set; }

    public bool WriteArrayAttribute { get; set; }

    public bool OmitRootObject { get; set; }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      IXmlNode node = this.WrapXml(value);
      XmlNamespaceManager manager = new XmlNamespaceManager((XmlNameTable) new NameTable());
      this.PushParentNamespaces(node, manager);
      if (!this.OmitRootObject)
        writer.WriteStartObject();
      this.SerializeNode(writer, node, manager, !this.OmitRootObject);
      if (this.OmitRootObject)
        return;
      writer.WriteEndObject();
    }

    private IXmlNode WrapXml(object value)
    {
      if (value is XmlNode)
        return (IXmlNode) new XmlNodeWrapper((XmlNode) value);
      else
        throw new ArgumentException("Value must be an XML object.", "value");
    }

    private void PushParentNamespaces(IXmlNode node, XmlNamespaceManager manager)
    {
      List<IXmlNode> list = (List<IXmlNode>) null;
      IXmlNode xmlNode1 = node;
      while ((xmlNode1 = xmlNode1.ParentNode) != null)
      {
        if (xmlNode1.NodeType == XmlNodeType.Element)
        {
          if (list == null)
            list = new List<IXmlNode>();
          list.Add(xmlNode1);
        }
      }
      if (list == null)
        return;
      list.Reverse();
      foreach (IXmlNode xmlNode2 in list)
      {
        manager.PushScope();
        foreach (IXmlNode xmlNode3 in (IEnumerable<IXmlNode>) xmlNode2.Attributes)
        {
          if (xmlNode3.NamespaceUri == "http://www.w3.org/2000/xmlns/" && xmlNode3.LocalName != "xmlns")
            manager.AddNamespace(xmlNode3.LocalName, xmlNode3.Value);
        }
      }
    }

    private string ResolveFullName(IXmlNode node, XmlNamespaceManager manager)
    {
      string str = node.NamespaceUri == null || node.LocalName == "xmlns" && node.NamespaceUri == "http://www.w3.org/2000/xmlns/" ? (string) null : manager.LookupPrefix(node.NamespaceUri);
      if (!string.IsNullOrEmpty(str))
        return str + ":" + node.LocalName;
      else
        return node.LocalName;
    }

    private string GetPropertyName(IXmlNode node, XmlNamespaceManager manager)
    {
      switch (node.NodeType)
      {
        case XmlNodeType.Element:
          return this.ResolveFullName(node, manager);
        case XmlNodeType.Attribute:
          if (node.NamespaceUri == "http://james.newtonking.com/projects/json")
            return "$" + node.LocalName;
          else
            return "@" + this.ResolveFullName(node, manager);
        case XmlNodeType.Text:
          return "#text";
        case XmlNodeType.CDATA:
          return "#cdata-section";
        case XmlNodeType.ProcessingInstruction:
          return "?" + this.ResolveFullName(node, manager);
        case XmlNodeType.Comment:
          return "#comment";
        case XmlNodeType.Whitespace:
          return "#whitespace";
        case XmlNodeType.SignificantWhitespace:
          return "#significant-whitespace";
        case XmlNodeType.XmlDeclaration:
          return "?xml";
        default:
          throw new JsonSerializationException("Unexpected XmlNodeType when getting node name: " + (object) node.NodeType);
      }
    }

    private bool IsArray(IXmlNode node)
    {
      IXmlNode xmlNode = node.Attributes != null ? Enumerable.SingleOrDefault<IXmlNode>((IEnumerable<IXmlNode>) node.Attributes, (Func<IXmlNode, bool>) (a =>
      {
        if (a.LocalName == "Array")
          return a.NamespaceUri == "http://james.newtonking.com/projects/json";
        else
          return false;
      })) : (IXmlNode) null;
      if (xmlNode != null)
        return XmlConvert.ToBoolean(xmlNode.Value);
      else
        return false;
    }

    private void SerializeGroupedNodes(JsonWriter writer, IXmlNode node, XmlNamespaceManager manager, bool writePropertyName)
    {
      Dictionary<string, List<IXmlNode>> dictionary = new Dictionary<string, List<IXmlNode>>();
      for (int index = 0; index < node.ChildNodes.Count; ++index)
      {
        IXmlNode node1 = node.ChildNodes[index];
        string propertyName = this.GetPropertyName(node1, manager);
        List<IXmlNode> list;
        if (!dictionary.TryGetValue(propertyName, out list))
        {
          list = new List<IXmlNode>();
          dictionary.Add(propertyName, list);
        }
        list.Add(node1);
      }
      foreach (KeyValuePair<string, List<IXmlNode>> keyValuePair in dictionary)
      {
        List<IXmlNode> list = keyValuePair.Value;
        if (list.Count == 1 && !this.IsArray(list[0]))
        {
          this.SerializeNode(writer, list[0], manager, writePropertyName);
        }
        else
        {
          string key = keyValuePair.Key;
          if (writePropertyName)
            writer.WritePropertyName(key);
          writer.WriteStartArray();
          for (int index = 0; index < list.Count; ++index)
            this.SerializeNode(writer, list[index], manager, false);
          writer.WriteEndArray();
        }
      }
    }

    private void SerializeNode(JsonWriter writer, IXmlNode node, XmlNamespaceManager manager, bool writePropertyName)
    {
      switch (node.NodeType)
      {
        case XmlNodeType.Element:
          if (this.IsArray(node) && (Enumerable.All<IXmlNode>((IEnumerable<IXmlNode>) node.ChildNodes, (Func<IXmlNode, bool>) (n => n.LocalName == node.LocalName)) && node.ChildNodes.Count > 0))
          {
            this.SerializeGroupedNodes(writer, node, manager, false);
            break;
          }
          else
          {
            foreach (IXmlNode xmlNode in (IEnumerable<IXmlNode>) node.Attributes)
            {
              if (xmlNode.NamespaceUri == "http://www.w3.org/2000/xmlns/")
              {
                string prefix = xmlNode.LocalName != "xmlns" ? xmlNode.LocalName : string.Empty;
                manager.AddNamespace(prefix, xmlNode.Value);
              }
            }
            if (writePropertyName)
              writer.WritePropertyName(this.GetPropertyName(node, manager));
            if (!Enumerable.Any<IXmlNode>(this.ValueAttributes((IEnumerable<IXmlNode>) node.Attributes)) && node.ChildNodes.Count == 1 && node.ChildNodes[0].NodeType == XmlNodeType.Text)
            {
              writer.WriteValue(node.ChildNodes[0].Value);
              break;
            }
            else if (node.ChildNodes.Count == 0 && CollectionUtils.IsNullOrEmpty<IXmlNode>((ICollection<IXmlNode>) node.Attributes))
            {
              writer.WriteNull();
              break;
            }
            else
            {
              writer.WriteStartObject();
              for (int index = 0; index < node.Attributes.Count; ++index)
                this.SerializeNode(writer, node.Attributes[index], manager, true);
              this.SerializeGroupedNodes(writer, node, manager, true);
              writer.WriteEndObject();
              break;
            }
          }
        case XmlNodeType.Attribute:
        case XmlNodeType.Text:
        case XmlNodeType.CDATA:
        case XmlNodeType.ProcessingInstruction:
        case XmlNodeType.Whitespace:
        case XmlNodeType.SignificantWhitespace:
          if (node.NamespaceUri == "http://www.w3.org/2000/xmlns/" && node.Value == "http://james.newtonking.com/projects/json" || node.NamespaceUri == "http://james.newtonking.com/projects/json" && node.LocalName == "Array")
            break;
          if (writePropertyName)
            writer.WritePropertyName(this.GetPropertyName(node, manager));
          writer.WriteValue(node.Value);
          break;
        case XmlNodeType.Comment:
          if (!writePropertyName)
            break;
          writer.WriteComment(node.Value);
          break;
        case XmlNodeType.Document:
        case XmlNodeType.DocumentFragment:
          this.SerializeGroupedNodes(writer, node, manager, writePropertyName);
          break;
        case XmlNodeType.XmlDeclaration:
          IXmlDeclaration xmlDeclaration = (IXmlDeclaration) node;
          writer.WritePropertyName(this.GetPropertyName(node, manager));
          writer.WriteStartObject();
          if (!string.IsNullOrEmpty(xmlDeclaration.Version))
          {
            writer.WritePropertyName("@version");
            writer.WriteValue(xmlDeclaration.Version);
          }
          if (!string.IsNullOrEmpty(xmlDeclaration.Encoding))
          {
            writer.WritePropertyName("@encoding");
            writer.WriteValue(xmlDeclaration.Encoding);
          }
          if (!string.IsNullOrEmpty(xmlDeclaration.Standalone))
          {
            writer.WritePropertyName("@standalone");
            writer.WriteValue(xmlDeclaration.Standalone);
          }
          writer.WriteEndObject();
          break;
        default:
          throw new JsonSerializationException("Unexpected XmlNodeType when serializing nodes: " + (object) node.NodeType);
      }
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      XmlNamespaceManager manager = new XmlNamespaceManager((XmlNameTable) new NameTable());
      IXmlDocument document = (IXmlDocument) null;
      IXmlNode currentNode = (IXmlNode) null;
      if (typeof (XmlNode).IsAssignableFrom(objectType))
      {
        if (objectType != typeof (XmlDocument))
          throw new JsonSerializationException("XmlNodeConverter only supports deserializing XmlDocuments");
        document = (IXmlDocument) new XmlDocumentWrapper(new XmlDocument());
        currentNode = (IXmlNode) document;
      }
      if (document == null || currentNode == null)
        throw new JsonSerializationException("Unexpected type when converting XML: " + (object) objectType);
      if (reader.TokenType != JsonToken.StartObject)
        throw new JsonSerializationException("XmlNodeConverter can only convert JSON that begins with an object.");
      if (!string.IsNullOrEmpty(this.DeserializeRootElementName))
      {
        this.ReadElement(reader, document, currentNode, this.DeserializeRootElementName, manager);
      }
      else
      {
        reader.Read();
        this.DeserializeNode(reader, document, manager, currentNode);
      }
      return document.WrappedNode;
    }

    private void DeserializeValue(JsonReader reader, IXmlDocument document, XmlNamespaceManager manager, string propertyName, IXmlNode currentNode)
    {
      switch (propertyName)
      {
        case "#text":
          currentNode.AppendChild(document.CreateTextNode(reader.Value.ToString()));
          break;
        case "#cdata-section":
          currentNode.AppendChild(document.CreateCDataSection(reader.Value.ToString()));
          break;
        case "#whitespace":
          currentNode.AppendChild(document.CreateWhitespace(reader.Value.ToString()));
          break;
        case "#significant-whitespace":
          currentNode.AppendChild(document.CreateSignificantWhitespace(reader.Value.ToString()));
          break;
        default:
          if (!string.IsNullOrEmpty(propertyName) && (int) propertyName[0] == 63)
          {
            this.CreateInstruction(reader, document, currentNode, propertyName);
            break;
          }
          else if (reader.TokenType == JsonToken.StartArray)
          {
            this.ReadArrayElements(reader, document, propertyName, currentNode, manager);
            break;
          }
          else
          {
            this.ReadElement(reader, document, currentNode, propertyName, manager);
            break;
          }
      }
    }

    private void ReadElement(JsonReader reader, IXmlDocument document, IXmlNode currentNode, string propertyName, XmlNamespaceManager manager)
    {
      if (string.IsNullOrEmpty(propertyName))
        throw new JsonSerializationException("XmlNodeConverter cannot convert JSON with an empty property name to XML.");
      Dictionary<string, string> dictionary = this.ReadAttributeElements(reader, manager);
      string prefix1 = MiscellaneousUtils.GetPrefix(propertyName);
      if (propertyName.StartsWith("@"))
      {
        string str1 = propertyName.Substring(1);
        string str2 = reader.Value.ToString();
        string prefix2 = MiscellaneousUtils.GetPrefix(str1);
        IXmlNode attribute = !string.IsNullOrEmpty(prefix2) ? document.CreateAttribute(str1, manager.LookupNamespace(prefix2), str2) : document.CreateAttribute(str1, str2);
        ((IXmlElement) currentNode).SetAttributeNode(attribute);
      }
      else
      {
        IXmlElement element = this.CreateElement(propertyName, document, prefix1, manager);
        currentNode.AppendChild((IXmlNode) element);
        foreach (KeyValuePair<string, string> keyValuePair in dictionary)
        {
          string prefix2 = MiscellaneousUtils.GetPrefix(keyValuePair.Key);
          IXmlNode attribute = !string.IsNullOrEmpty(prefix2) ? document.CreateAttribute(keyValuePair.Key, manager.LookupNamespace(prefix2), keyValuePair.Value) : document.CreateAttribute(keyValuePair.Key, keyValuePair.Value);
          element.SetAttributeNode(attribute);
        }
        if (reader.TokenType == JsonToken.String || reader.TokenType == JsonToken.Integer || (reader.TokenType == JsonToken.Float || reader.TokenType == JsonToken.Boolean) || reader.TokenType == JsonToken.Date)
        {
          element.AppendChild(document.CreateTextNode(this.ConvertTokenToXmlValue(reader)));
        }
        else
        {
          if (reader.TokenType == JsonToken.Null || reader.TokenType == JsonToken.EndObject)
            return;
          manager.PushScope();
          this.DeserializeNode(reader, document, manager, (IXmlNode) element);
          manager.PopScope();
        }
      }
    }

    private string ConvertTokenToXmlValue(JsonReader reader)
    {
      if (reader.TokenType == JsonToken.String)
        return reader.Value.ToString();
      if (reader.TokenType == JsonToken.Integer)
        return XmlConvert.ToString(Convert.ToInt64(reader.Value, (IFormatProvider) CultureInfo.InvariantCulture));
      if (reader.TokenType == JsonToken.Float)
        return XmlConvert.ToString(Convert.ToDouble(reader.Value, (IFormatProvider) CultureInfo.InvariantCulture));
      if (reader.TokenType == JsonToken.Boolean)
        return XmlConvert.ToString(Convert.ToBoolean(reader.Value, (IFormatProvider) CultureInfo.InvariantCulture));
      if (reader.TokenType == JsonToken.Date)
      {
        DateTime dateTime = Convert.ToDateTime(reader.Value, (IFormatProvider) CultureInfo.InvariantCulture);
        return XmlConvert.ToString(dateTime, DateTimeUtils.ToSerializationMode(dateTime.Kind));
      }
      else if (reader.TokenType == JsonToken.Null)
        return (string) null;
      else
        throw JsonSerializationException.Create(reader, StringUtils.FormatWith("Cannot get an XML string value from token type '{0}'.", (IFormatProvider) CultureInfo.InvariantCulture, (object) reader.TokenType));
    }

    private void ReadArrayElements(JsonReader reader, IXmlDocument document, string propertyName, IXmlNode currentNode, XmlNamespaceManager manager)
    {
      string prefix = MiscellaneousUtils.GetPrefix(propertyName);
      IXmlElement element = this.CreateElement(propertyName, document, prefix, manager);
      currentNode.AppendChild((IXmlNode) element);
      int num = 0;
      while (reader.Read() && reader.TokenType != JsonToken.EndArray)
      {
        this.DeserializeValue(reader, document, manager, propertyName, (IXmlNode) element);
        ++num;
      }
      if (this.WriteArrayAttribute)
        this.AddJsonArrayAttribute(element, document);
      if (num != 1 || !this.WriteArrayAttribute)
        return;
      this.AddJsonArrayAttribute(Enumerable.Single<IXmlElement>(CollectionUtils.CastValid<IXmlElement>((IEnumerable) element.ChildNodes), (Func<IXmlElement, bool>) (n => n.LocalName == propertyName)), document);
    }

    private void AddJsonArrayAttribute(IXmlElement element, IXmlDocument document)
    {
      element.SetAttributeNode(document.CreateAttribute("json:Array", "http://james.newtonking.com/projects/json", "true"));
    }

    private Dictionary<string, string> ReadAttributeElements(JsonReader reader, XmlNamespaceManager manager)
    {
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      bool flag1 = false;
      bool flag2 = false;
      if (reader.TokenType != JsonToken.String && reader.TokenType != JsonToken.Null && (reader.TokenType != JsonToken.Boolean && reader.TokenType != JsonToken.Integer) && (reader.TokenType != JsonToken.Float && reader.TokenType != JsonToken.Date && reader.TokenType != JsonToken.StartConstructor))
      {
        while (!flag1 && !flag2 && reader.Read())
        {
          switch (reader.TokenType)
          {
            case JsonToken.PropertyName:
              string str1 = reader.Value.ToString();
              if (!string.IsNullOrEmpty(str1))
              {
                switch (str1[0])
                {
                  case '$':
                    string str2 = str1.Substring(1);
                    reader.Read();
                    string str3 = reader.Value.ToString();
                    string prefix1 = manager.LookupPrefix("http://james.newtonking.com/projects/json");
                    if (prefix1 == null)
                    {
                      int? nullable = new int?();
                      while (manager.LookupNamespace("json" + (object) nullable) != null)
                        nullable = new int?(nullable.GetValueOrDefault() + 1);
                      prefix1 = "json" + (object) nullable;
                      dictionary.Add("xmlns:" + prefix1, "http://james.newtonking.com/projects/json");
                      manager.AddNamespace(prefix1, "http://james.newtonking.com/projects/json");
                    }
                    dictionary.Add(prefix1 + ":" + str2, str3);
                    continue;
                  case '@':
                    string str4 = str1.Substring(1);
                    reader.Read();
                    string uri = this.ConvertTokenToXmlValue(reader);
                    dictionary.Add(str4, uri);
                    string prefix2;
                    if (this.IsNamespaceAttribute(str4, out prefix2))
                    {
                      manager.AddNamespace(prefix2, uri);
                      continue;
                    }
                    else
                      continue;
                  default:
                    flag1 = true;
                    continue;
                }
              }
              else
              {
                flag1 = true;
                continue;
              }
            case JsonToken.EndObject:
              flag2 = true;
              continue;
            default:
              throw new JsonSerializationException("Unexpected JsonToken: " + (object) reader.TokenType);
          }
        }
      }
      return dictionary;
    }

    private void CreateInstruction(JsonReader reader, IXmlDocument document, IXmlNode currentNode, string propertyName)
    {
      if (propertyName == "?xml")
      {
        string version = (string) null;
        string encoding = (string) null;
        string standalone = (string) null;
        while (reader.Read() && reader.TokenType != JsonToken.EndObject)
        {
          switch (reader.Value.ToString())
          {
            case "@version":
              reader.Read();
              version = reader.Value.ToString();
              continue;
            case "@encoding":
              reader.Read();
              encoding = reader.Value.ToString();
              continue;
            case "@standalone":
              reader.Read();
              standalone = reader.Value.ToString();
              continue;
            default:
              throw new JsonSerializationException("Unexpected property name encountered while deserializing XmlDeclaration: " + reader.Value);
          }
        }
        IXmlNode xmlDeclaration = document.CreateXmlDeclaration(version, encoding, standalone);
        currentNode.AppendChild(xmlDeclaration);
      }
      else
      {
        IXmlNode processingInstruction = document.CreateProcessingInstruction(propertyName.Substring(1), reader.Value.ToString());
        currentNode.AppendChild(processingInstruction);
      }
    }

    private IXmlElement CreateElement(string elementName, IXmlDocument document, string elementPrefix, XmlNamespaceManager manager)
    {
      string namespaceUri = string.IsNullOrEmpty(elementPrefix) ? manager.DefaultNamespace : manager.LookupNamespace(elementPrefix);
      return !string.IsNullOrEmpty(namespaceUri) ? document.CreateElement(elementName, namespaceUri) : document.CreateElement(elementName);
    }

    private void DeserializeNode(JsonReader reader, IXmlDocument document, XmlNamespaceManager manager, IXmlNode currentNode)
    {
      do
      {
        switch (reader.TokenType)
        {
          case JsonToken.StartConstructor:
            string propertyName1 = reader.Value.ToString();
            while (reader.Read() && reader.TokenType != JsonToken.EndConstructor)
              this.DeserializeValue(reader, document, manager, propertyName1, currentNode);
            break;
          case JsonToken.PropertyName:
            if (currentNode.NodeType == XmlNodeType.Document && document.DocumentElement != null)
              throw new JsonSerializationException("JSON root object has multiple properties. The root object must have a single property in order to create a valid XML document. Consider specifing a DeserializeRootElementName.");
            string propertyName = reader.Value.ToString();
            reader.Read();
            if (reader.TokenType == JsonToken.StartArray)
            {
              int num = 0;
              while (reader.Read() && reader.TokenType != JsonToken.EndArray)
              {
                this.DeserializeValue(reader, document, manager, propertyName, currentNode);
                ++num;
              }
              if (num == 1 && this.WriteArrayAttribute)
              {
                this.AddJsonArrayAttribute(Enumerable.Single<IXmlElement>(CollectionUtils.CastValid<IXmlElement>((IEnumerable) currentNode.ChildNodes), (Func<IXmlElement, bool>) (n => n.LocalName == propertyName)), document);
                break;
              }
              else
                break;
            }
            else
            {
              this.DeserializeValue(reader, document, manager, propertyName, currentNode);
              break;
            }
          case JsonToken.Comment:
            currentNode.AppendChild(document.CreateComment((string) reader.Value));
            break;
          case JsonToken.EndObject:
            return;
          case JsonToken.EndArray:
            return;
          default:
            throw new JsonSerializationException("Unexpected JsonToken when deserializing node: " + (object) reader.TokenType);
        }
      }
      while (reader.TokenType == JsonToken.PropertyName || reader.Read());
    }

    private bool IsNamespaceAttribute(string attributeName, out string prefix)
    {
      if (attributeName.StartsWith("xmlns", StringComparison.Ordinal))
      {
        if (attributeName.Length == 5)
        {
          prefix = string.Empty;
          return true;
        }
        else if ((int) attributeName[5] == 58)
        {
          prefix = attributeName.Substring(6, attributeName.Length - 6);
          return true;
        }
      }
      prefix = (string) null;
      return false;
    }

    private IEnumerable<IXmlNode> ValueAttributes(IEnumerable<IXmlNode> c)
    {
      return Enumerable.Where<IXmlNode>(c, (Func<IXmlNode, bool>) (a => a.NamespaceUri != "http://james.newtonking.com/projects/json"));
    }

    public override bool CanConvert(Type valueType)
    {
      return typeof (XmlNode).IsAssignableFrom(valueType);
    }
  }
}
