// Type: ContentSerialization.Attributes.TypeSerializationAttribute
// Assembly: ContentSerialization, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 458E2D00-F22E-495B-9E0A-A944909C9571
// Assembly location: F:\Program Files (x86)\FEZ\ContentSerialization.dll

using System;

namespace ContentSerialization.Attributes
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
  public class TypeSerializationAttribute : Attribute
  {
    public bool FlattenToList { get; set; }
  }
}
