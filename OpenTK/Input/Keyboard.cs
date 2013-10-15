// Type: OpenTK.Input.Keyboard
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK.Platform;
using System;

namespace OpenTK.Input
{
  public static class Keyboard
  {
    private static readonly IKeyboardDriver2 driver = Factory.Default.CreateKeyboardDriver();
    private static readonly object SyncRoot = new object();

    static Keyboard()
    {
    }

    public static KeyboardState GetState()
    {
      lock (Keyboard.SyncRoot)
        return Keyboard.driver.GetState();
    }

    public static KeyboardState GetState(int index)
    {
      if (index < 0)
        throw new ArgumentOutOfRangeException("index");
      lock (Keyboard.SyncRoot)
        return Keyboard.driver.GetState(index);
    }
  }
}
