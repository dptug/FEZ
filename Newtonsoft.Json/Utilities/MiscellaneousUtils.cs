// Type: Newtonsoft.Json.Utilities.MiscellaneousUtils
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using System;
using System.Globalization;

namespace Newtonsoft.Json.Utilities
{
  internal static class MiscellaneousUtils
  {
    public static bool ValueEquals(object objA, object objB)
    {
      if (objA == null && objB == null)
        return true;
      if (objA != null && objB == null || objA == null && objB != null)
        return false;
      if (objA.GetType() == objB.GetType())
        return objA.Equals(objB);
      if (ConvertUtils.IsInteger(objA) && ConvertUtils.IsInteger(objB))
        return Convert.ToDecimal(objA, (IFormatProvider) CultureInfo.CurrentCulture).Equals(Convert.ToDecimal(objB, (IFormatProvider) CultureInfo.CurrentCulture));
      if ((objA is double || objA is float || objA is Decimal) && (objB is double || objB is float || objB is Decimal))
        return MathUtils.ApproxEquals(Convert.ToDouble(objA, (IFormatProvider) CultureInfo.CurrentCulture), Convert.ToDouble(objB, (IFormatProvider) CultureInfo.CurrentCulture));
      else
        return false;
    }

    public static ArgumentOutOfRangeException CreateArgumentOutOfRangeException(string paramName, object actualValue, string message)
    {
      string message1 = message + Environment.NewLine + StringUtils.FormatWith("Actual value was {0}.", (IFormatProvider) CultureInfo.InvariantCulture, actualValue);
      return new ArgumentOutOfRangeException(paramName, message1);
    }

    public static bool TryAction<T>(Creator<T> creator, out T output)
    {
      ValidationUtils.ArgumentNotNull((object) creator, "creator");
      try
      {
        output = creator();
        return true;
      }
      catch
      {
        output = default (T);
        return false;
      }
    }

    public static string ToString(object value)
    {
      if (value == null)
        return "{null}";
      if (!(value is string))
        return value.ToString();
      else
        return "\"" + value.ToString() + "\"";
    }

    public static byte[] HexToBytes(string hex)
    {
      string str = hex.Replace("-", string.Empty);
      byte[] numArray = new byte[str.Length / 2];
      int num1 = 4;
      int index = 0;
      foreach (int num2 in str)
      {
        int num3 = (num2 - 48) % 32;
        if (num3 > 9)
          num3 -= 7;
        numArray[index] |= (byte) (num3 << num1);
        num1 ^= 4;
        if (num1 != 0)
          ++index;
      }
      return numArray;
    }

    public static string BytesToHex(byte[] bytes)
    {
      return MiscellaneousUtils.BytesToHex(bytes, false);
    }

    public static string BytesToHex(byte[] bytes, bool removeDashes)
    {
      string str = BitConverter.ToString(bytes);
      if (removeDashes)
        str = str.Replace("-", "");
      return str;
    }

    public static int ByteArrayCompare(byte[] a1, byte[] a2)
    {
      int num1 = a1.Length.CompareTo(a2.Length);
      if (num1 != 0)
        return num1;
      for (int index = 0; index < a1.Length; ++index)
      {
        int num2 = a1[index].CompareTo(a2[index]);
        if (num2 != 0)
          return num2;
      }
      return 0;
    }

    public static string GetPrefix(string qualifiedName)
    {
      string prefix;
      string localName;
      MiscellaneousUtils.GetQualifiedNameParts(qualifiedName, out prefix, out localName);
      return prefix;
    }

    public static string GetLocalName(string qualifiedName)
    {
      string prefix;
      string localName;
      MiscellaneousUtils.GetQualifiedNameParts(qualifiedName, out prefix, out localName);
      return localName;
    }

    public static void GetQualifiedNameParts(string qualifiedName, out string prefix, out string localName)
    {
      int length = qualifiedName.IndexOf(':');
      switch (length)
      {
        case -1:
        case 0:
          prefix = (string) null;
          localName = qualifiedName;
          break;
        default:
          if (qualifiedName.Length - 1 != length)
          {
            prefix = qualifiedName.Substring(0, length);
            localName = qualifiedName.Substring(length + 1);
            break;
          }
          else
            goto case -1;
      }
    }
  }
}
