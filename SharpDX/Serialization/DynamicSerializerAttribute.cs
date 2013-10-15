// Type: SharpDX.Serialization.DynamicSerializerAttribute
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX.Multimedia;
using System;

namespace SharpDX.Serialization
{
  public class DynamicSerializerAttribute : Attribute
  {
    private readonly FourCC id;

    public FourCC Id
    {
      get
      {
        return this.id;
      }
    }

    public DynamicSerializerAttribute(int id)
    {
      this.id = (FourCC) id;
    }

    public DynamicSerializerAttribute(string id)
    {
      this.id = (FourCC) id;
    }
  }
}
