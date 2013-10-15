// Type: Microsoft.Xna.Framework.Content.ContentTypeReader
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

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
