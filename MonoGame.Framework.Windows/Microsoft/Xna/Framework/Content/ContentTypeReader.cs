// Type: Microsoft.Xna.Framework.Content.ContentTypeReader
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System;
using System.IO;

namespace Microsoft.Xna.Framework.Content
{
  public abstract class ContentTypeReader
  {
    private Type targetType;

    public Type TargetType
    {
      get
      {
        return this.targetType;
      }
    }

    public virtual int TypeVersion
    {
      get
      {
        return 0;
      }
    }

    protected ContentTypeReader(Type targetType)
    {
      this.targetType = targetType;
    }

    protected internal virtual void Initialize(ContentTypeReaderManager manager)
    {
    }

    protected internal abstract object Read(ContentReader input, object existingInstance);

    public static string Normalize(string fileName, string[] extensions)
    {
      if (File.Exists(fileName))
        return fileName;
      if (!string.IsNullOrEmpty(Path.GetExtension(fileName)))
        return (string) null;
      foreach (string str in extensions)
      {
        string path = fileName + str;
        if (File.Exists(path))
          return path;
      }
      return (string) null;
    }
  }
}
