// Type: FezEngine.Services.IDebuggingBag
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using System.Collections.Generic;

namespace FezEngine.Services
{
  public interface IDebuggingBag
  {
    IEnumerable<string> Keys { get; }

    object this[string index] { get; }

    void Add(string name, object item);

    void Empty();

    float GetAge(string name);
  }
}
