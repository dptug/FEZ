// Type: SDL.SDLDateTime
// Assembly: SimpleDefinitionLanguage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8A9B110C-63DC-4C6F-B639-88CD09E9B5B5
// Assembly location: F:\Program Files (x86)\FEZ\SimpleDefinitionLanguage.dll

using System;
using System.Text;

namespace SDL
{
  public class SDLDateTime
  {
    private DateTime dateTime;
    private string timeZone;

    public DateTime DateTime
    {
      get
      {
        return this.dateTime;
      }
    }

    public bool HasTime
    {
      get
      {
        if (this.dateTime.Hour == 0 && this.dateTime.Minute == 0 && this.dateTime.Second == 0)
          return this.dateTime.Millisecond != 0;
        else
          return true;
      }
    }

    public string TimeZone
    {
      get
      {
        return this.timeZone;
      }
    }

    public int Year
    {
      get
      {
        return this.dateTime.Year;
      }
    }

    public int Month
    {
      get
      {
        return this.dateTime.Month;
      }
    }

    public int Day
    {
      get
      {
        return this.dateTime.Day;
      }
    }

    public int Hour
    {
      get
      {
        return this.dateTime.Hour;
      }
    }

    public int Minute
    {
      get
      {
        return this.dateTime.Minute;
      }
    }

    public int Second
    {
      get
      {
        return this.dateTime.Second;
      }
    }

    public int Millisecond
    {
      get
      {
        return this.dateTime.Millisecond;
      }
    }

    public SDLDateTime(DateTime dateTime, string timeZone)
    {
      this.dateTime = dateTime;
      if (timeZone == null)
        timeZone = SDLDateTime.getCurrentTimeZone();
      this.timeZone = timeZone;
    }

    public SDLDateTime(int year, int month, int day)
      : this(new DateTime(year, month, day), (string) null)
    {
    }

    public SDLDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond)
      : this(new DateTime(year, month, day, hour, minute, second, millisecond), (string) null)
    {
    }

    public SDLDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, string timeZone)
      : this(new DateTime(year, month, day, hour, minute, second, millisecond), timeZone)
    {
    }

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder((string) (object) this.Year + (object) "/");
      if (this.Month < 10)
        stringBuilder.Append("0");
      stringBuilder.Append((string) (object) this.Month + (object) "/");
      if (this.Day < 10)
        stringBuilder.Append("0");
      stringBuilder.Append(this.Day);
      if (this.HasTime)
      {
        stringBuilder.Append(" ");
        if (this.Hour < 10)
          stringBuilder.Append("0");
        stringBuilder.Append(this.Hour);
        stringBuilder.Append(":");
        if (this.Minute < 10)
          stringBuilder.Append("0");
        stringBuilder.Append(this.Minute);
        if (this.Second != 0 || this.Millisecond != 0)
        {
          stringBuilder.Append(":");
          if (this.Second < 10)
            stringBuilder.Append("0");
          stringBuilder.Append(this.Second);
          if (this.Millisecond != 0)
          {
            stringBuilder.Append(".");
            string str = string.Concat((object) this.Millisecond);
            if (str.Length == 1)
              str = "00" + str;
            else if (str.Length == 2)
              str = "0" + str;
            stringBuilder.Append(str);
          }
        }
        stringBuilder.Append("-");
        stringBuilder.Append(this.TimeZone == null ? SDLDateTime.getCurrentTimeZone() : this.TimeZone);
      }
      return ((object) stringBuilder).ToString();
    }

    internal static string getCurrentTimeZone()
    {
      TimeSpan utcOffset = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now);
      StringBuilder stringBuilder = new StringBuilder("GMT");
      stringBuilder.Append(utcOffset.Hours < 0 ? "-" : "+");
      int num1 = Math.Abs(utcOffset.Hours);
      stringBuilder.Append(num1 < 10 ? "0" + (object) num1 : string.Concat((object) num1));
      stringBuilder.Append(":");
      int num2 = Math.Abs(utcOffset.Minutes);
      stringBuilder.Append(num2 < 10 ? "0" + (object) num2 : string.Concat((object) num2));
      return ((object) stringBuilder).ToString();
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      else
        return this.ToString().Equals(obj.ToString());
    }

    public override int GetHashCode()
    {
      return this.ToString().GetHashCode();
    }
  }
}
