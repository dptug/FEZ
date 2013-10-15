// Type: OpenTK.Input.IMouseDriver2
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

namespace OpenTK.Input
{
  internal interface IMouseDriver2
  {
    MouseState GetState();

    MouseState GetState(int index);

    void SetPosition(double x, double y);
  }
}
