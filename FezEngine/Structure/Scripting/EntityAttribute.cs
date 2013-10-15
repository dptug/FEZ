// Type: FezEngine.Structure.Scripting.EntityAttribute
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Structure;
using System;

namespace FezEngine.Structure.Scripting
{
  [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
  public class EntityAttribute : Attribute
  {
    public Type Model { get; set; }

    public ActorType[] RestrictTo { get; set; }

    public bool Static { get; set; }
  }
}
