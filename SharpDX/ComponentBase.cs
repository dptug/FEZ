// Type: SharpDX.ComponentBase
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using System;

namespace SharpDX
{
  public abstract class ComponentBase : IComponent
  {
    private string name;
    private readonly bool isNameImmutable;

    public string Name
    {
      get
      {
        return this.name;
      }
      set
      {
        if (this.isNameImmutable)
          throw new ArgumentException("Name property is immutable for this instance", "value");
        this.name = value;
        this.OnNameChanged();
      }
    }

    public object Tag { get; set; }

    protected ComponentBase()
    {
    }

    protected ComponentBase(string name)
    {
      if (name == null)
        return;
      this.name = name;
      this.isNameImmutable = true;
    }

    protected virtual void OnNameChanged()
    {
    }
  }
}
