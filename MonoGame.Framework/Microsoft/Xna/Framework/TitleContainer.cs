// Type: Microsoft.Xna.Framework.TitleContainer
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System;
using System.IO;

namespace Microsoft.Xna.Framework
{
  public static class TitleContainer
  {
    internal static string Location { get; private set; }

    static TitleContainer()
    {
      TitleContainer.Location = AppDomain.CurrentDomain.BaseDirectory;
    }

    public static Stream OpenStream(string name)
    {
      string filename = TitleContainer.GetFilename(name);
      if (Path.IsPathRooted(filename))
        throw new ArgumentException("Invalid filename. TitleContainer.OpenStream requires a relative path.");
      else
        return (Stream) File.OpenRead(Path.Combine(TitleContainer.Location, filename));
    }

    internal static string GetFilename(string name)
    {
      name = name.Replace('\\', Path.DirectorySeparatorChar);
      return name;
    }
  }
}
