// Type: FezEngine.Structure.Dirtyable`1
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

namespace FezEngine.Structure
{
  public class Dirtyable<T>
  {
    public T Value;
    public bool Dirty;

    public static implicit operator T(Dirtyable<T> dirtyable)
    {
      return dirtyable.Value;
    }

    public static implicit operator Dirtyable<T>(T dirtyable)
    {
      return new Dirtyable<T>()
      {
        Value = dirtyable
      };
    }

    public void Clean()
    {
      this.Dirty = false;
    }

    public void Set(T newValue)
    {
      this.Value = newValue;
      this.Dirty = true;
    }
  }
}
