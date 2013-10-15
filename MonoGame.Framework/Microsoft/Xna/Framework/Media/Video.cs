// Type: Microsoft.Xna.Framework.Media.Video
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework;
using System;
using System.IO;

namespace Microsoft.Xna.Framework.Media
{
  public sealed class Video : IDisposable
  {
    private Color _backColor = Color.Black;
    private string _fileName;

    public Color BackgroundColor
    {
      get
      {
        return this._backColor;
      }
      set
      {
        this._backColor = value;
      }
    }

    public string FileName
    {
      get
      {
        return this._fileName;
      }
    }

    internal Video(string FileName)
    {
      this._fileName = FileName;
      this.Prepare();
    }

    internal static string Normalize(string FileName)
    {
      if (File.Exists(FileName))
        return FileName;
      if (!string.IsNullOrEmpty(Path.GetExtension(FileName)))
        return (string) null;
      if (File.Exists(FileName + ".mp4"))
        return FileName + ".mp4";
      if (File.Exists(FileName + ".mov"))
        return FileName + ".mov";
      if (File.Exists(FileName + ".avi"))
        return FileName + ".avi";
      if (File.Exists(FileName + ".m4v"))
        return FileName + ".m4v";
      else
        return (string) null;
    }

    internal void Prepare()
    {
    }

    public void Dispose()
    {
    }
  }
}
