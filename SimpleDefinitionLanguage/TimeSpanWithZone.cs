// Type: SDL.TimeSpanWithZone
// Assembly: SimpleDefinitionLanguage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8A9B110C-63DC-4C6F-B639-88CD09E9B5B5
// Assembly location: F:\Program Files (x86)\FEZ\SimpleDefinitionLanguage.dll

namespace SDL
{
  internal class TimeSpanWithZone
  {
    private readonly string timeZone;
    private readonly int days;
    private readonly int hours;
    private readonly int minutes;
    private readonly int seconds;
    private readonly int milliseconds;

    internal int Days
    {
      get
      {
        return this.days;
      }
    }

    internal int Hours
    {
      get
      {
        return this.hours;
      }
    }

    internal int Minutes
    {
      get
      {
        return this.minutes;
      }
    }

    internal int Seconds
    {
      get
      {
        return this.seconds;
      }
    }

    internal int Milliseconds
    {
      get
      {
        return this.milliseconds;
      }
    }

    internal string TimeZone
    {
      get
      {
        return this.timeZone;
      }
    }

    internal TimeSpanWithZone(int days, int hours, int minutes, int seconds, int milliseconds, string timeZone)
    {
      this.days = days;
      this.hours = hours;
      this.minutes = minutes;
      this.seconds = seconds;
      this.milliseconds = milliseconds;
      this.timeZone = timeZone;
    }
  }
}
