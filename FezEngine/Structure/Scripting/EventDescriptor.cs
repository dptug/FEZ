// Type: FezEngine.Structure.Scripting.EventDescriptor
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;

namespace FezEngine.Structure.Scripting
{
  public struct EventDescriptor
  {
    public readonly string Name;
    public readonly string Description;
    public readonly DynamicMethodDelegate AddHandler;
    public readonly DynamicMethodDelegate AddEndTriggerHandler;

    public EventDescriptor(string name, string description, DynamicMethodDelegate @delegate, DynamicMethodDelegate endTrigger)
    {
      this.Name = name;
      this.Description = description;
      this.AddHandler = @delegate;
      this.AddEndTriggerHandler = endTrigger;
    }
  }
}
