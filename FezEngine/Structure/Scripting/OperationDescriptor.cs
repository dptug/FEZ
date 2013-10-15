// Type: FezEngine.Structure.Scripting.OperationDescriptor
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using System.Collections.Generic;
using System.Linq;

namespace FezEngine.Structure.Scripting
{
  public struct OperationDescriptor
  {
    public readonly string Name;
    public readonly string Description;
    public readonly ParameterDescriptor[] Parameters;
    public readonly DynamicMethodDelegate Call;

    public OperationDescriptor(string name, string description, DynamicMethodDelegate @delegate, IEnumerable<ParameterDescriptor> parameters)
    {
      this.Name = name;
      this.Description = description;
      this.Parameters = Enumerable.ToArray<ParameterDescriptor>(parameters);
      this.Call = @delegate;
    }
  }
}
