// Type: Newtonsoft.Json.Converters.XmlNodeWrapper
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities.LinqBridge;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace Newtonsoft.Json.Converters
{
  internal class XmlNodeWrapper : IXmlNode
  {
    private readonly XmlNode _node;

    public object WrappedNode
    {
      get
      {
        return (object) this._node;
      }
    }

    public XmlNodeType NodeType
    {
      get
      {
        return this._node.NodeType;
      }
    }

    public string Name
    {
      get
      {
        return this._node.Name;
      }
    }

    public string LocalName
    {
      get
      {
        return this._node.LocalName;
      }
    }

    public IList<IXmlNode> ChildNodes
    {
      get
      {
        return (IList<IXmlNode>) Enumerable.ToList<IXmlNode>(Enumerable.Select<XmlNode, IXmlNode>(Enumerable.Cast<XmlNode>((IEnumerable) this._node.ChildNodes), (Func<XmlNode, IXmlNode>) (n => this.WrapNode(n))));
      }
    }

    public IList<IXmlNode> Attributes
    {
      get
      {
        if (this._node.Attributes == null)
          return (IList<IXmlNode>) null;
        else
          return (IList<IXmlNode>) Enumerable.ToList<IXmlNode>(Enumerable.Select<XmlAttribute, IXmlNode>(Enumerable.Cast<XmlAttribute>((IEnumerable) this._node.Attributes), (Func<XmlAttribute, IXmlNode>) (a => this.WrapNode((XmlNode) a))));
      }
    }

    public IXmlNode ParentNode
    {
      get
      {
        XmlNode node = this._node is XmlAttribute ? (XmlNode) ((XmlAttribute) this._node).OwnerElement : this._node.ParentNode;
        if (node == null)
          return (IXmlNode) null;
        else
          return this.WrapNode(node);
      }
    }

    public string Value
    {
      get
      {
        return this._node.Value;
      }
      set
      {
        this._node.Value = value;
      }
    }

    public string Prefix
    {
      get
      {
        return this._node.Prefix;
      }
    }

    public string NamespaceUri
    {
      get
      {
        return this._node.NamespaceURI;
      }
    }

    public XmlNodeWrapper(XmlNode node)
    {
      this._node = node;
    }

    private IXmlNode WrapNode(XmlNode node)
    {
      switch (node.NodeType)
      {
        case XmlNodeType.Element:
          return (IXmlNode) new XmlElementWrapper((XmlElement) node);
        case XmlNodeType.XmlDeclaration:
          return (IXmlNode) new XmlDeclarationWrapper((XmlDeclaration) node);
        default:
          return (IXmlNode) new XmlNodeWrapper(node);
      }
    }

    public IXmlNode AppendChild(IXmlNode newChild)
    {
      this._node.AppendChild(((XmlNodeWrapper) newChild)._node);
      return newChild;
    }
  }
}
