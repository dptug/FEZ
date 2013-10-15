// Type: Common.DescriptionAttribute
// Assembly: Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9992B00D-7E50-4755-8BAA-4E3BBC8F3470
// Assembly location: F:\Program Files (x86)\FEZ\Common.dll

using System;

namespace Common
{
  public class DescriptionAttribute : Attribute
  {
    public string Description { get; set; }

    public DescriptionAttribute(string description)
    {
      this.Description = description;
    }
  }
}
