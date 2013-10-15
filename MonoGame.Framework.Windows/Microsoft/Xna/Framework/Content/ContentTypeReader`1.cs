// Type: Microsoft.Xna.Framework.Content.ContentTypeReader`1
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

namespace Microsoft.Xna.Framework.Content
{
  public abstract class ContentTypeReader<T> : ContentTypeReader
  {
    protected ContentTypeReader()
      : base(typeof (T))
    {
    }

    protected internal override object Read(ContentReader input, object existingInstance)
    {
      if (existingInstance == null)
        return (object) this.Read(input, default (T));
      else
        return (object) this.Read(input, (T) existingInstance);
    }

    protected internal abstract T Read(ContentReader input, T existingInstance);
  }
}
