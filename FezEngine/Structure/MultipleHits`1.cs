// Type: FezEngine.Structure.MultipleHits`1
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

namespace FezEngine.Structure
{
  public struct MultipleHits<T>
  {
    public T NearLow;
    public T FarHigh;

    public T First
    {
      get
      {
        if (!object.Equals((object) this.NearLow, (object) default (T)))
          return this.NearLow;
        else
          return this.FarHigh;
      }
    }

    public override string ToString()
    {
      return string.Format("{{Near/Low: {0} Far/High: {1}}}", (object) this.NearLow, (object) this.FarHigh);
    }
  }
}
