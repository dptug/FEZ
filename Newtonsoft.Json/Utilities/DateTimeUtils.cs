// Type: Newtonsoft.Json.Utilities.DateTimeUtils
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using System;
using System.Globalization;
using System.Xml;

namespace Newtonsoft.Json.Utilities
{
  internal static class DateTimeUtils
  {
    public static string GetUtcOffsetText(this DateTime d)
    {
      TimeSpan utcOffset = DateTimeUtils.GetUtcOffset(d);
      return utcOffset.Hours.ToString("+00;-00", (IFormatProvider) CultureInfo.InvariantCulture) + ":" + utcOffset.Minutes.ToString("00;00", (IFormatProvider) CultureInfo.InvariantCulture);
    }

    public static TimeSpan GetUtcOffset(this DateTime d)
    {
      return TimeZone.CurrentTimeZone.GetUtcOffset(d);
    }

    public static XmlDateTimeSerializationMode ToSerializationMode(DateTimeKind kind)
    {
      switch (kind)
      {
        case DateTimeKind.Unspecified:
          return XmlDateTimeSerializationMode.Unspecified;
        case DateTimeKind.Utc:
          return XmlDateTimeSerializationMode.Utc;
        case DateTimeKind.Local:
          return XmlDateTimeSerializationMode.Local;
        default:
          throw MiscellaneousUtils.CreateArgumentOutOfRangeException("kind", (object) kind, "Unexpected DateTimeKind value.");
      }
    }
  }
}
