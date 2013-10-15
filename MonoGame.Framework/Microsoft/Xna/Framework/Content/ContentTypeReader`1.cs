// Type: Microsoft.Xna.Framework.Content.ContentTypeReader`1
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

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
