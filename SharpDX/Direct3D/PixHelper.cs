// Type: SharpDX.Direct3D.PixHelper
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX;
using System.Runtime.InteropServices;

namespace SharpDX.Direct3D
{
  public static class PixHelper
  {
    public static bool IsCurrentlyProfiled
    {
      get
      {
        return PixHelper.D3DPERF_GetStatus() != 0;
      }
    }

    public static int BeginEvent(Color color, string name)
    {
      return PixHelper.D3DPERF_BeginEvent(color.ToBgra(), name);
    }

    public static int BeginEvent(Color color, string name, params object[] parameters)
    {
      return PixHelper.D3DPERF_BeginEvent(color.ToBgra(), string.Format(name, parameters));
    }

    public static int EndEvent()
    {
      return PixHelper.D3DPERF_EndEvent();
    }

    public static void SetMarker(Color color, string name)
    {
      PixHelper.D3DPERF_SetMarker(color.ToBgra(), name);
    }

    public static void SetMarker(Color color, string name, params object[] parameters)
    {
      PixHelper.D3DPERF_SetMarker(color.ToBgra(), string.Format(name, parameters));
    }

    public static void AllowProfiling(bool enableFlag)
    {
      PixHelper.D3DPERF_SetOptions(enableFlag ? 0 : 1);
    }

    [DllImport("d3d9.dll", CharSet = CharSet.Unicode)]
    private static int D3DPERF_BeginEvent(int color, string name);

    [DllImport("d3d9.dll", CharSet = CharSet.Unicode)]
    private static int D3DPERF_EndEvent();

    [DllImport("d3d9.dll", CharSet = CharSet.Unicode)]
    private static void D3DPERF_SetMarker(int color, string wszName);

    [DllImport("d3d9.dll", CharSet = CharSet.Unicode)]
    private static void D3DPERF_SetOptions(int options);

    [DllImport("d3d9.dll", CharSet = CharSet.Unicode)]
    private static int D3DPERF_GetStatus();
  }
}
