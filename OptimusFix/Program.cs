// Type: OptimusFix.Program
// Assembly: OptimusFix, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D73C678E-9F94-43CA-B31C-E01183EAD576
// Assembly location: F:\Program Files (x86)\FEZ\OptimusFix.exe

using System;
using System.Diagnostics;
using System.IO;

namespace OptimusFix
{
  internal class Program
  {
    private static int Main()
    {
      string executableName = Program.GetExecutableName();
      try
      {
        SOP.ResultCodes resultCodes1 = SOP.SOP_RemoveProfile("FEZ");
        SOP.ResultCodes resultCodes2 = SOP.SOP_SetProfile("FEZ", executableName);
        if (!Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FEZ")))
          Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FEZ"));
        using (FileStream fileStream = File.Open(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\FEZ\\Debug Log.txt", FileMode.Append))
        {
          using (StreamWriter streamWriter = new StreamWriter((Stream) fileStream))
          {
            streamWriter.WriteLine("({0}) [{1}] {2}", (object) DateTime.Now.ToString("HH:mm:ss.fff"), (object) "Optimus Fix", (object) string.Concat(new object[4]
            {
              (object) "SOP_Remove returned ",
              (object) resultCodes1,
              (object) " for ",
              (object) executableName
            }));
            streamWriter.WriteLine("({0}) [{1}] {2}", (object) DateTime.Now.ToString("HH:mm:ss.fff"), (object) "Optimus Fix", (object) string.Concat(new object[4]
            {
              (object) "SOP_Set returned ",
              (object) resultCodes2,
              (object) " for ",
              (object) executableName
            }));
          }
        }
      }
      catch (Exception ex)
      {
        using (FileStream fileStream = File.Open(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\FEZ\\Debug Log.txt", FileMode.Append))
        {
          using (StreamWriter streamWriter = new StreamWriter((Stream) fileStream))
            streamWriter.WriteLine("({0}) [{1}] {2}", (object) DateTime.Now.ToString("HH:mm:ss.fff"), (object) "Optimus Fix", (object) string.Concat(new object[4]
            {
              (object) "SOP raised exception : ",
              (object) ex,
              (object) " for ",
              (object) executableName
            }));
        }
      }
      return 0;
    }

    private static string GetExecutableName()
    {
      string fileName = Process.GetCurrentProcess().MainModule.FileName;
      int num = fileName.LastIndexOf('\\');
      return fileName.Substring(num + 1, fileName.Length - num - 1).Replace("OptimusFix", "FEZ");
    }
  }
}
