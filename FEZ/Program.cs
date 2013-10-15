// Type: FezGame.Program
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using Common;
using FezEngine.Tools;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;

namespace FezGame
{
  internal static class Program
  {
    private static Fez fez;

    [STAThread]
    private static void Main(string[] args)
    {
      Queue<string> queue = new Queue<string>((IEnumerable<string>) args);
      while (queue.Count > 0)
      {
        switch (queue.Dequeue().ToLower(CultureInfo.InvariantCulture))
        {
          case "-c":
          case "--clear-save-file":
            string str1 = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Fez\\SaveSlot";
            for (int index = -1; index < 3; ++index)
            {
              if (File.Exists(str1 + (object) index))
                File.Delete(str1 + (object) index);
            }
            continue;
          case "-w":
          case "--windowed":
            SettingsManager.Settings.ScreenMode = ScreenMode.Windowed;
            continue;
          case "-as-is":
            SettingsManager.Settings.UseCurrentMode = true;
            continue;
          case "--variable-time-step":
            Fez.VariableTimeStep = true;
            continue;
          case "-ng":
          case "--no-gamepad":
            Fez.NoGamePad = true;
            continue;
          case "-r":
          case "--region":
            SettingsManager.Settings.Language = (Language) Enum.Parse(typeof (Language), queue.Dequeue());
            continue;
          case "--trace":
            TraceFlags.TraceContentLoad = true;
            continue;
          case "--force-60hz":
            Fez.Force60Hz = true;
            continue;
          case "-pd":
          case "--public-demo":
            Fez.PublicDemo = true;
            string str2 = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Fez\\SaveSlot";
            for (int index = -1; index < 3; ++index)
            {
              if (File.Exists(str2 + (object) index))
                File.Delete(str2 + (object) index);
            }
            continue;
          case "-st":
          case "--singlethreaded":
            PersistentThreadPool.SingleThreaded = true;
            continue;
          default:
            continue;
        }
      }
      if (!Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FEZ")))
        Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FEZ"));
      Logger.Clear();
      Logger.Try(new Action(Program.MainInternal));
      if (Program.fez != null)
        Program.fez.Dispose();
      Program.fez = (Fez) null;
    }

    private static void MainInternal()
    {
      Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;
      Program.fez = new Fez();
      if (Program.fez.IsDisposed)
        return;
      Program.fez.Run();
    }
  }
}
