// Type: OpenTK.Toolkit
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK.Platform;

namespace OpenTK
{
  public sealed class Toolkit
  {
    private static readonly object InitLock = new object();
    private static volatile bool initialized;

    static Toolkit()
    {
    }

    private Toolkit()
    {
    }

    public static void Init()
    {
      lock (Toolkit.InitLock)
      {
        if (Toolkit.initialized)
          return;
        Toolkit.initialized = true;
        Configuration.Init();
        Factory temp_4 = new Factory();
      }
    }
  }
}
