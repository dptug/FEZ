// Type: OpenTK.Configuration
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK.Platform.X11;
using System;
using System.Runtime.InteropServices;

namespace OpenTK
{
  public static class Configuration
  {
    private static readonly object InitLock = new object();
    private static bool runningOnWindows;
    private static bool runningOnUnix;
    private static bool runningOnX11;
    private static bool runningOnMacOS;
    private static bool runningOnLinux;
    private static bool runningOnMono;
    private static volatile bool initialized;

    public static bool RunningOnWindows
    {
      get
      {
        return Configuration.runningOnWindows;
      }
    }

    public static bool RunningOnX11
    {
      get
      {
        return Configuration.runningOnX11;
      }
    }

    public static bool RunningOnUnix
    {
      get
      {
        return Configuration.runningOnUnix;
      }
    }

    public static bool RunningOnLinux
    {
      get
      {
        return Configuration.runningOnLinux;
      }
    }

    public static bool RunningOnMacOS
    {
      get
      {
        return Configuration.runningOnMacOS;
      }
    }

    public static bool RunningOnMono
    {
      get
      {
        return Configuration.runningOnMono;
      }
    }

    static Configuration()
    {
      Toolkit.Init();
    }

    private static string DetectUnixKernel()
    {
      Configuration.utsname uname_struct = new Configuration.utsname();
      Configuration.uname(out uname_struct);
      return ((object) uname_struct.sysname).ToString();
    }

    [DllImport("libc")]
    private static void uname(out Configuration.utsname uname_struct);

    internal static void Init()
    {
      lock (Configuration.InitLock)
      {
        if (Configuration.initialized)
          return;
        Configuration.initialized = true;
        if (Environment.OSVersion.Platform == PlatformID.Win32NT || Environment.OSVersion.Platform == PlatformID.Win32S || (Environment.OSVersion.Platform == PlatformID.Win32Windows || Environment.OSVersion.Platform == PlatformID.WinCE))
        {
          Configuration.runningOnWindows = true;
        }
        else
        {
          if (Environment.OSVersion.Platform != PlatformID.Unix && Environment.OSVersion.Platform != PlatformID.Unix)
            throw new PlatformNotSupportedException("Unknown platform. Please report this error at http://www.opentk.com.");
          switch (Configuration.DetectUnixKernel())
          {
            case "":
            case null:
              throw new PlatformNotSupportedException("Unknown platform. Please file a bug report at http://www.opentk.com/node/add/project-issue/opentk");
            case "Linux":
              Configuration.runningOnLinux = Configuration.runningOnUnix = true;
              break;
            case "Darwin":
              Configuration.runningOnMacOS = Configuration.runningOnUnix = true;
              break;
            default:
              Configuration.runningOnUnix = true;
              break;
          }
        }
        if (!Configuration.RunningOnMacOS)
        {
          try
          {
            Configuration.runningOnX11 = API.DefaultDisplay != IntPtr.Zero;
          }
          catch
          {
          }
        }
        if (Type.GetType("Mono.Runtime") == null)
          return;
        Configuration.runningOnMono = true;
      }
    }

    private struct utsname
    {
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
      public string sysname;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
      public string nodename;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
      public string release;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
      public string version;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
      public string machine;
      [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
      public string extraJustInCase;
    }
  }
}
