// Type: FezGame.Services.MockUser
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

namespace FezGame.Services
{
  public class MockUser
  {
    public static readonly MockUser Default = new MockUser()
    {
      PersonaName = "DefaultUser"
    };

    public string PersonaName { get; set; }

    static MockUser()
    {
    }
  }
}
