// Type: SharpDX.Win32.PropertyKey`2
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

namespace SharpDX.Win32
{
  public class PropertyKey<T1, T2>
  {
    public string Name { get; private set; }

    public PropertyKey(string name)
    {
      this.Name = name;
    }
  }
}
