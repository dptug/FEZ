// Type: Microsoft.Xna.Framework.TitleContainer
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System.IO;

namespace Microsoft.Xna.Framework
{
  public static class TitleContainer
  {
    public static Stream OpenStream(string name)
    {
      return (Stream) File.OpenRead(TitleContainer.GetFilename(name));
    }

    internal static string GetFilename(string name)
    {
      name = name.Replace('\\', Path.DirectorySeparatorChar);
      return name;
    }
  }
}
