// Type: Newtonsoft.Json.Converters.XmlDocumentWrapper
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using System.Xml;

namespace Newtonsoft.Json.Converters
{
  internal class XmlDocumentWrapper : XmlNodeWrapper, IXmlDocument, IXmlNode
  {
    private readonly XmlDocument _document;

    public IXmlElement DocumentElement
    {
      get
      {
        if (this._document.DocumentElement == null)
          return (IXmlElement) null;
        else
          return (IXmlElement) new XmlElementWrapper(this._document.DocumentElement);
      }
    }

    public XmlDocumentWrapper(XmlDocument document)
      : base((XmlNode) document)
    {
      this._document = document;
    }

    public IXmlNode CreateComment(string data)
    {
      return (IXmlNode) new XmlNodeWrapper((XmlNode) this._document.CreateComment(data));
    }

    public IXmlNode CreateTextNode(string text)
    {
      return (IXmlNode) new XmlNodeWrapper((XmlNode) this._document.CreateTextNode(text));
    }

    public IXmlNode CreateCDataSection(string data)
    {
      return (IXmlNode) new XmlNodeWrapper((XmlNode) this._document.CreateCDataSection(data));
    }

    public IXmlNode CreateWhitespace(string text)
    {
      return (IXmlNode) new XmlNodeWrapper((XmlNode) this._document.CreateWhitespace(text));
    }

    public IXmlNode CreateSignificantWhitespace(string text)
    {
      return (IXmlNode) new XmlNodeWrapper((XmlNode) this._document.CreateSignificantWhitespace(text));
    }

    public IXmlNode CreateXmlDeclaration(string version, string encoding, string standalone)
    {
      return (IXmlNode) new XmlNodeWrapper((XmlNode) this._document.CreateXmlDeclaration(version, encoding, standalone));
    }

    public IXmlNode CreateProcessingInstruction(string target, string data)
    {
      return (IXmlNode) new XmlNodeWrapper((XmlNode) this._document.CreateProcessingInstruction(target, data));
    }

    public IXmlElement CreateElement(string elementName)
    {
      return (IXmlElement) new XmlElementWrapper(this._document.CreateElement(elementName));
    }

    public IXmlElement CreateElement(string qualifiedName, string namespaceUri)
    {
      return (IXmlElement) new XmlElementWrapper(this._document.CreateElement(qualifiedName, namespaceUri));
    }

    public IXmlNode CreateAttribute(string name, string value)
    {
      return (IXmlNode) new XmlNodeWrapper((XmlNode) this._document.CreateAttribute(name))
      {
        Value = value
      };
    }

    public IXmlNode CreateAttribute(string qualifiedName, string namespaceUri, string value)
    {
      return (IXmlNode) new XmlNodeWrapper((XmlNode) this._document.CreateAttribute(qualifiedName, namespaceUri))
      {
        Value = value
      };
    }
  }
}
