// Type: Newtonsoft.Json.Converters.IXmlNode
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using System.Collections.Generic;
using System.Xml;

namespace Newtonsoft.Json.Converters
{
  internal interface IXmlNode
  {
    XmlNodeType NodeType { get; }

    string LocalName { get; }

    IList<IXmlNode> ChildNodes { get; }

    IList<IXmlNode> Attributes { get; }

    IXmlNode ParentNode { get; }

    string Value { get; set; }

    string NamespaceUri { get; }

    object WrappedNode { get; }

    IXmlNode AppendChild(IXmlNode newChild);
  }
}
