// Type: Microsoft.Xna.Framework.GamerServices.GamerServicesDispatcher
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

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
