// Type: FezEngine.Structure.Scripting.PropertyDescriptor
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using System;

namespace FezEngine.Structure.Scripting
{
  public struct PropertyDescriptor
  {
    public readonly string Name;
    public readonly string Description;
    public readonly Type Type;
    public readonly DynamicMethodDelegate GetValue;

    public PropertyDescriptor(string name, string description, Type type, DynamicMethodDelegate @delegate)
    {
      this.Name = name;
      this.Description = description;
      this.Type = type;
      this.GetValue = @delegate;
    }
  }
}
