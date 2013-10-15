// Type: FezEngine.Structure.Scripting.ParameterDescriptor
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using System;

namespace FezEngine.Structure.Scripting
{
  public struct ParameterDescriptor
  {
    public readonly string Name;
    public readonly Type Type;

    public ParameterDescriptor(string name, Type type)
    {
      this.Name = name;
      this.Type = type;
    }
  }
}
