// Type: ContentSerialization.Attributes.SerializationAttribute
// Assembly: ContentSerialization, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 458E2D00-F22E-495B-9E0A-A944909C9571
// Assembly location: F:\Program Files (x86)\FEZ\ContentSerialization.dll

using System;

namespace ContentSerialization.Attributes
{
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
  public class SerializationAttribute : Attribute
  {
    public bool Ignore { get; set; }

    public bool UseAttribute { get; set; }

    public string Name { get; set; }

    public bool Optional { get; set; }

    public bool DefaultValueOptional { get; set; }

    public string CollectionItemName { get; set; }
  }
}
