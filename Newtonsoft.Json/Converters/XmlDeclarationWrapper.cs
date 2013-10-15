// Type: Newtonsoft.Json.Converters.XmlDeclarationWrapper
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using System.Xml;

namespace Newtonsoft.Json.Converters
{
  internal class XmlDeclarationWrapper : XmlNodeWrapper, IXmlDeclaration, IXmlNode
  {
    private readonly XmlDeclaration _declaration;

    public string Version
    {
      get
      {
        return this._declaration.Version;
      }
    }

    public string Encoding
    {
      get
      {
        return this._declaration.Encoding;
      }
      set
      {
        this._declaration.Encoding = value;
      }
    }

    public string Standalone
    {
      get
      {
        return this._declaration.Standalone;
      }
      set
      {
        this._declaration.Standalone = value;
      }
    }

    public XmlDeclarationWrapper(XmlDeclaration declaration)
      : base((XmlNode) declaration)
    {
      this._declaration = declaration;
    }
  }
}
