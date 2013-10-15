// Type: Newtonsoft.Json.Converters.IXmlDocument
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

namespace Newtonsoft.Json.Converters
{
  internal interface IXmlDocument : IXmlNode
  {
    IXmlElement DocumentElement { get; }

    IXmlNode CreateComment(string text);

    IXmlNode CreateTextNode(string text);

    IXmlNode CreateCDataSection(string data);

    IXmlNode CreateWhitespace(string text);

    IXmlNode CreateSignificantWhitespace(string text);

    IXmlNode CreateXmlDeclaration(string version, string encoding, string standalone);

    IXmlNode CreateProcessingInstruction(string target, string data);

    IXmlElement CreateElement(string elementName);

    IXmlElement CreateElement(string qualifiedName, string namespaceUri);

    IXmlNode CreateAttribute(string name, string value);

    IXmlNode CreateAttribute(string qualifiedName, string namespaceUri, string value);
  }
}
