// Type: Microsoft.Xna.Framework.Content.ExternalReferenceReader
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System;

namespace Microsoft.Xna.Framework.Content
{
  internal class ExternalReferenceReader : ContentTypeReader
  {
    public ExternalReferenceReader()
      : base((Type) null)
    {
    }

    protected internal override object Read(ContentReader input, object existingInstance)
    {
      return input.ReadExternalReference<object>();
    }
  }
}
