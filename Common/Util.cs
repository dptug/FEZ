// Type: Common.Util
// Assembly: Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9992B00D-7E50-4755-8BAA-4E3BBC8F3470
// Assembly location: F:\Program Files (x86)\FEZ\Common.dll

using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Common
{
  public static class Util
  {
    private static unsafe void Hash(byte* d, int len, ref uint h)
    {
      for (int index = 0; index < len; ++index)
      {
        h += (uint) d[index];
        h += h << 10;
        h ^= h >> 6;
      }
    }

    public static unsafe void Hash(ref uint h, string s)
    {
      fixed (char* chPtr = s)
        Util.Hash((byte*) chPtr, s.Length * 2, ref h);
    }

    public static unsafe void Hash(ref uint h, int data)
    {
      Util.Hash((byte*) &data, 4, ref h);
    }

    public static unsafe void Hash(ref uint h, long data)
    {
      Util.Hash((byte*) &data, 8, ref h);
    }

    public static unsafe void Hash(ref uint h, bool data)
    {
      Util.Hash((byte*) &data, 1, ref h);
    }

    public static unsafe void Hash(ref uint h, float data)
    {
      Util.Hash((byte*) &data, 4, ref h);
    }

    public static unsafe int Avalanche(uint h)
    {
      h += h << 3;
      h ^= h >> 11;
      h += h << 15;
      return (int) *&h;
    }

    public static bool ArrayEquals<T>(T[] a, T[] b) where T : struct
    {
      if (a == null != (b == null))
        return false;
      if (a == null)
        return true;
      if (a.Length != b.Length)
        return false;
      for (int index = 0; index < a.Length; ++index)
      {
        if (!b[index].Equals((object) a[index]))
          return false;
      }
      return true;
    }

    public static bool ContainsIgnoreCase(this string source, string value)
    {
      return source.IndexOf(value, StringComparison.CurrentCultureIgnoreCase) != -1;
    }

    public static int CombineHashCodes(int first, int second, int third, int fourth)
    {
      uint h = 0U;
      Util.Hash(ref h, first);
      Util.Hash(ref h, second);
      Util.Hash(ref h, third);
      Util.Hash(ref h, fourth);
      return Util.Avalanche(h);
    }

    public static int CombineHashCodes(int first, int second, int third)
    {
      uint h = 0U;
      Util.Hash(ref h, first);
      Util.Hash(ref h, second);
      Util.Hash(ref h, third);
      return Util.Avalanche(h);
    }

    public static int CombineHashCodes(int first, int second)
    {
      uint h = 0U;
      Util.Hash(ref h, first);
      Util.Hash(ref h, second);
      return Util.Avalanche(h);
    }

    public static int CombineHashCodes(params object[] keys)
    {
      uint h = 0U;
      foreach (object obj in keys)
        Util.Hash(ref h, obj == null ? 0 : obj.GetHashCode());
      return Util.Avalanche(h);
    }

    public static string DeepToString<T>(IEnumerable<T> collection)
    {
      return Util.DeepToString<T>(collection, false);
    }

    public static string DeepToString<T>(IEnumerable<T> collection, bool omitBrackets)
    {
      StringBuilder stringBuilder = new StringBuilder(omitBrackets ? string.Empty : "{");
      foreach (T obj in collection)
      {
        stringBuilder.Append((object) obj == null ? string.Empty : obj.ToString());
        stringBuilder.Append(", ");
      }
      if (stringBuilder.Length >= 2)
        stringBuilder.Remove(stringBuilder.Length - 2, 2);
      if (!omitBrackets)
        stringBuilder.Append("}");
      return ((object) stringBuilder).ToString();
    }

    public static string ReflectToString(object obj)
    {
      StringBuilder stringBuilder = new StringBuilder("{");
      MemberInfo[] serializableMembers = ReflectionHelper.GetSerializableMembers(obj.GetType());
      for (int index = 0; index < serializableMembers.Length; ++index)
      {
        MemberInfo member = serializableMembers[index];
        stringBuilder.AppendFormat("{0}:{1}", (object) member.Name, ReflectionHelper.GetValue(member, obj));
        if (index != serializableMembers.Length - 1)
          stringBuilder.Append(", ");
      }
      stringBuilder.Append("}");
      return ((object) stringBuilder).ToString();
    }

    public static string CompactToString(this Matrix matrix)
    {
      return string.Format("{0:0.##} {1:0.##} {2:0.##} {3:0.##} | {4:0.##} {5:0.##} {6:0.##} {7:0.##} | {8:0.##} {9:0.##} {10:0.##} {11:0.##} | {12:0.##} {13:0.##} {14:0.##} {15:0.##}", (object) matrix.M11, (object) matrix.M12, (object) matrix.M13, (object) matrix.M14, (object) matrix.M21, (object) matrix.M22, (object) matrix.M23, (object) matrix.M24, (object) matrix.M31, (object) matrix.M32, (object) matrix.M33, (object) matrix.M34, (object) matrix.M41, (object) matrix.M42, (object) matrix.M43, (object) matrix.M44);
    }

    public static T[] JoinArrays<T>(T[] first, T[] second)
    {
      T[] objArray = new T[first.Length + second.Length];
      Array.Copy((Array) first, (Array) objArray, first.Length);
      Array.Copy((Array) second, 0, (Array) objArray, first.Length, second.Length);
      return objArray;
    }

    public static T[] AppendToArray<T>(T[] array, T element)
    {
      T[] objArray = new T[array.Length + 1];
      Array.Copy((Array) array, (Array) objArray, array.Length);
      objArray[array.Length] = element;
      return objArray;
    }

    public static string StripExtensions(string path)
    {
      int startIndex = path.LastIndexOf('\\') + 1;
      return path.Substring(0, path.IndexOf('.', startIndex));
    }

    public static string GetFileNameWithoutAnyExtension(string path)
    {
      int startIndex = path.LastIndexOf('\\') + 1;
      return path.Substring(startIndex, path.IndexOf('.', startIndex) - startIndex);
    }

    public static string AllExtensions(this FileInfo file)
    {
      int startIndex = file.FullName.LastIndexOf('\\');
      if (file.FullName.IndexOf('.', startIndex) == -1)
        return "";
      else
        return file.FullName.Substring(file.FullName.IndexOf('.', startIndex));
    }

    public static Array GetValues(Type t)
    {
      return Enum.GetValues(t);
    }

    public static IEnumerable<T> GetValues<T>()
    {
      return Enumerable.Cast<T>((IEnumerable) Enum.GetValues(typeof (T)));
    }

    public static IEnumerable<string> GetNames<T>()
    {
      return (IEnumerable<string>) Enum.GetNames(typeof (T));
    }

    public static string GetName<T>(object value)
    {
      return Util.GetName(typeof (T), value);
    }

    public static string GetName(Type t, object value)
    {
      return Enum.GetName(t, value);
    }

    public static bool Implements(this Type type, Type interfaceType)
    {
      return Enumerable.Contains<Type>((IEnumerable<Type>) type.GetInterfaces(), interfaceType);
    }

    public static Color FromName(string name)
    {
      Color color = Color.White;
      switch (name.ToUpper(CultureInfo.InvariantCulture))
      {
        case "WHITE":
          color = Color.White;
          break;
        case "BLACK":
          color = Color.Black;
          break;
        case "RED":
          color = Color.Red;
          break;
        case "GREEN":
          color = Color.Green;
          break;
        case "BLUE":
          color = Color.Blue;
          break;
        case "CYAN":
          color = Color.Cyan;
          break;
        case "MAGENTA":
          color = Color.Magenta;
          break;
        case "YELLOW":
          color = Color.Yellow;
          break;
      }
      return color;
    }

    public static void ColorToHSV(Color color, out double hue, out double saturation, out double value)
    {
      int num1 = (int) Math.Max(color.R, Math.Max(color.G, color.B));
      int num2 = (int) Math.Min(color.R, Math.Min(color.G, color.B));
      hue = (double) Util.GetHue(color);
      saturation = num1 == 0 ? 0.0 : 1.0 - 1.0 * (double) num2 / (double) num1;
      value = (double) num1 / (double) byte.MaxValue;
    }

    public static float GetHue(this Color color)
    {
      if ((int) color.R == (int) color.G && (int) color.G == (int) color.B)
        return 0.0f;
      float num1 = (float) color.R / (float) byte.MaxValue;
      float num2 = (float) color.G / (float) byte.MaxValue;
      float num3 = (float) color.B / (float) byte.MaxValue;
      float num4 = 0.0f;
      float num5 = num1;
      float num6 = num1;
      if ((double) num2 > (double) num5)
        num5 = num2;
      if ((double) num3 > (double) num5)
        num5 = num3;
      if ((double) num2 < (double) num6)
        num6 = num2;
      if ((double) num3 < (double) num6)
        num6 = num3;
      float num7 = num5 - num6;
      if ((double) num1 == (double) num5)
        num4 = (num2 - num3) / num7;
      else if ((double) num2 == (double) num5)
        num4 = (float) (2.0 + ((double) num3 - (double) num1) / (double) num7);
      else if ((double) num3 == (double) num5)
        num4 = (float) (4.0 + ((double) num1 - (double) num2) / (double) num7);
      float num8 = num4 * 60f;
      if ((double) num8 < 0.0)
        num8 += 360f;
      return num8;
    }

    public static Color ColorFromHSV(double hue, double saturation, double value)
    {
      int num1 = (int) (hue / 60.0) % 6;
      double num2 = hue / 60.0 - (double) (int) (hue / 60.0);
      value *= (double) byte.MaxValue;
      byte num3 = (byte) value;
      byte num4 = (byte) (value * (1.0 - saturation));
      byte num5 = (byte) (value * (1.0 - num2 * saturation));
      byte num6 = (byte) (value * (1.0 - (1.0 - num2) * saturation));
      if (num1 == 0)
        return new Color((int) num3, (int) num6, (int) num4);
      if (num1 == 1)
        return new Color((int) num5, (int) num3, (int) num4);
      if (num1 == 2)
        return new Color((int) num4, (int) num3, (int) num6);
      if (num1 == 3)
        return new Color((int) num4, (int) num5, (int) num3);
      if (num1 == 4)
        return new Color((int) num6, (int) num4, (int) num3);
      else
        return new Color((int) num3, (int) num4, (int) num5);
    }

    public static string StripPunctuation(this string s)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (char c in s)
      {
        if (!char.IsPunctuation(c))
          stringBuilder.Append(c);
      }
      return ((object) stringBuilder).ToString();
    }

    public static void NullAction()
    {
    }

    public static void NullAction<T>(T t)
    {
    }

    public static void NullAction<T, U>(T t, U u)
    {
    }

    public static void NullAction<T, U, V>(T t, U u, V v)
    {
    }

    public static void NullAction<T, U, V, W>(T t, U u, V v, W w)
    {
    }

    public static TResult NullFunc<TResult>()
    {
      return default (TResult);
    }

    public static TResult NullFunc<T, TResult>(T t)
    {
      return default (TResult);
    }

    public static TResult NullFunc<T, U, TResult>(T t, U u)
    {
      return default (TResult);
    }

    public static TResult NullFunc<T, U, V, TResult>(T t, U u, V v)
    {
      return default (TResult);
    }

    public static TResult NullFunc<T, U, V, W, TResult>(T t, U u, V v, W w)
    {
      return default (TResult);
    }
  }
}
