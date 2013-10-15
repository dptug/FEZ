// Type: Common.Logger
// Assembly: Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9992B00D-7E50-4755-8BAA-4E3BBC8F3470
// Assembly location: F:\Program Files (x86)\FEZ\Common.dll

using System;
using System.Globalization;
using System.IO;

namespace Common
{
  public static class Logger
  {
    private static readonly string LogFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FEZ\\Debug Log.txt");
    private const string TimeFormat = "HH:mm:ss.fff";
    private static bool errorEncountered;

    public static bool ErrorEncountered
    {
      get
      {
        return Logger.errorEncountered;
      }
      private set
      {
        Logger.errorEncountered = value;
      }
    }

    static Logger()
    {
    }

    public static void Log(string component, string message)
    {
      Logger.Log(component, LogSeverity.Information, message);
    }

    public static void Log(string component, LogSeverity severity, string message)
    {
      try
      {
        using (FileStream fileStream = File.Open(Logger.LogFilePath, FileMode.Append))
        {
          using (StreamWriter streamWriter = new StreamWriter((Stream) fileStream))
            streamWriter.WriteLine("({0}) [{1}] {2} : {3}", (object) DateTime.Now.ToString("HH:mm:ss.fff"), (object) component, (object) ((object) severity).ToString().ToUpper(CultureInfo.InvariantCulture), (object) message);
        }
      }
      catch (Exception ex)
      {
      }
      if (severity != LogSeverity.Error)
        return;
      Logger.errorEncountered = true;
    }

    public static void Try(Action action)
    {
      try
      {
        action();
      }
      catch (Exception ex)
      {
        Logger.LogError(ex);
      }
    }

    public static void Try<T>(Action<T> action, T arg)
    {
      try
      {
        action(arg);
      }
      catch (Exception ex)
      {
        Logger.LogError(ex);
      }
    }

    public static void Try<T, U>(Action<T, U> action, T arg1, U arg2)
    {
      try
      {
        action(arg1, arg2);
      }
      catch (Exception ex)
      {
        Logger.LogError(ex);
      }
    }

    public static void Try<T, U, V>(Action<T, U, V> action, T arg1, U arg2, V arg3)
    {
      try
      {
        action(arg1, arg2, arg3);
      }
      catch (Exception ex)
      {
        Logger.LogError(ex);
      }
    }

    public static void LogError(Exception e)
    {
      Logger.Log("Unhandled Exception", LogSeverity.Error, ((object) e).ToString());
    }

    public static void Clear()
    {
      if (File.Exists(Logger.LogFilePath))
        File.Delete(Logger.LogFilePath);
      Logger.errorEncountered = false;
    }
  }
}
