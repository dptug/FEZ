// Type: SharpDX.ShadowAttribute
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using System;

namespace SharpDX
{
  [AttributeUsage(AttributeTargets.Interface)]
  internal class ShadowAttribute : Attribute
  {
    private Type type;

    public Type Type
    {
      get
      {
        return this.type;
      }
    }

    public ShadowAttribute(Type typeOfTheAssociatedShadow)
    {
      this.type = typeOfTheAssociatedShadow;
    }

    public static ShadowAttribute Get(Type type)
    {
      object[] customAttributes = type.GetCustomAttributes(typeof (ShadowAttribute), false);
      if (customAttributes.Length == 0)
        return (ShadowAttribute) null;
      else
        return (ShadowAttribute) customAttributes[0];
    }
  }
}
