// Type: SharpDX.IO.PathUtility
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using SharpDX;
using System;
using System.Collections.Generic;

namespace SharpDX.IO
{
  public class PathUtility
  {
    public static string GetNormalizedPath(string path)
    {
      path = path.Replace('/', '\\');
      string[] strArray = path.Split(new char[1]
      {
        '\\'
      }, StringSplitOptions.RemoveEmptyEntries);
      Stack<string> stack = new Stack<string>();
      foreach (string str in strArray)
      {
        if (!(str == "."))
        {
          if (str == "..")
          {
            if (stack.Count == 0)
              throw new ArgumentException("Invalid path can't start with '..'");
            stack.Pop();
          }
          else
            stack.Push(str);
        }
      }
      string[] array = stack.ToArray();
      Array.Reverse((Array) array);
      return Utilities.Join<string>("\\", array);
    }
  }
}
