// Type: FezGame.Tools.ThreadExecutionState
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using System;
using System.Runtime.InteropServices;

namespace FezGame.Tools
{
  internal static class ThreadExecutionState
  {
    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static ThreadExecutionState.EXECUTION_STATE SetThreadExecutionState(ThreadExecutionState.EXECUTION_STATE esFlags);

    public static void SetUp()
    {
      int num = (int) ThreadExecutionState.SetThreadExecutionState(ThreadExecutionState.EXECUTION_STATE.ES_CONTINUOUS | ThreadExecutionState.EXECUTION_STATE.ES_DISPLAY_REQUIRED);
    }

    [Flags]
    public enum EXECUTION_STATE : uint
    {
      ES_AWAYMODE_REQUIRED = 64U,
      ES_CONTINUOUS = 2147483648U,
      ES_DISPLAY_REQUIRED = 2U,
      ES_SYSTEM_REQUIRED = 1U,
    }
  }
}
