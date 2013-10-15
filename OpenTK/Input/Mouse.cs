// Type: OpenTK.Input.Mouse
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK.Platform;
using System;

namespace OpenTK.Input
{
  public static class Mouse
  {
    private static readonly IMouseDriver2 driver = Factory.Default.CreateMouseDriver();
    private static readonly object SyncRoot = new object();

    static Mouse()
    {
    }

    public static MouseState GetState()
    {
      lock (Mouse.SyncRoot)
        return Mouse.driver.GetState();
    }

    public static MouseState GetState(int index)
    {
      if (index < 0)
        throw new ArgumentOutOfRangeException("index");
      lock (Mouse.SyncRoot)
        return Mouse.driver.GetState(index);
    }

    public static void SetPosition(double x, double y)
    {
      lock (Mouse.SyncRoot)
        Mouse.driver.SetPosition(x, y);
    }
  }
}
