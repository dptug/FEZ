// Type: Common.EndTriggerAttribute
// Assembly: Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9992B00D-7E50-4755-8BAA-4E3BBC8F3470
// Assembly location: F:\Program Files (x86)\FEZ\Common.dll

using System;

namespace Common
{
  public class EndTriggerAttribute : Attribute
  {
    public string Trigger { get; set; }

    public EndTriggerAttribute(string trigger)
    {
      this.Trigger = trigger;
    }
  }
}
