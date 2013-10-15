// Type: Microsoft.Xna.Framework.GamerServices.GamerServicesDispatcher
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System;

namespace Microsoft.Xna.Framework.GamerServices
{
  public static class GamerServicesDispatcher
  {
    public static bool IsInitialized
    {
      get
      {
        return false;
      }
    }

    public static IntPtr WindowHandle { get; set; }

    public static event EventHandler<EventArgs> InstallingTitleUpdate;

    public static void Initialize(IServiceProvider serviceProvider)
    {
      throw new NotImplementedException();
    }

    public static void Update()
    {
    }
  }
}
