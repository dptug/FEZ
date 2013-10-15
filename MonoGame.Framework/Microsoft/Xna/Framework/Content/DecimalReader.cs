// Type: Microsoft.Xna.Framework.Content.DecimalReader
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System;

namespace Microsoft.Xna.Framework.Content
{
  internal class DecimalReader : ContentTypeReader<Decimal>
  {
    internal DecimalReader()
    {
    }

    protected internal override Decimal Read(ContentReader input, Decimal existingInstance)
    {
      return input.ReadDecimal();
    }
  }
}
