// Type: FezEngine.Structure.Input.PatternTester
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using System.Collections.Generic;

namespace FezEngine.Structure.Input
{
  public static class PatternTester
  {
    public static bool Test(IList<CodeInput> input, CodeInput[] pattern)
    {
      int count = input.Count;
      bool flag = false;
      for (int index = 0; index < pattern.Length && index < count && input[count - index - 1] == pattern[pattern.Length - index - 1]; ++index)
      {
        if (index == pattern.Length - 1)
        {
          flag = true;
          input.Clear();
          break;
        }
      }
      return flag;
    }

    public static bool Test(IList<VibrationMotor> input, VibrationMotor[] pattern)
    {
      int count = input.Count;
      bool flag = false;
      int num = 0;
      for (int index = 0; index + num < pattern.Length && index < count; ++index)
      {
        while (pattern[pattern.Length - index - 1 - num] == VibrationMotor.None)
        {
          ++num;
          if (index + num >= pattern.Length)
            break;
        }
        if (input[count - index - 1] == pattern[pattern.Length - index - 1 - num])
        {
          if (index == pattern.Length - 1 - num)
          {
            flag = true;
            input.Clear();
            break;
          }
        }
        else
          break;
      }
      return flag;
    }
  }
}
