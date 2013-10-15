// Type: Microsoft.Xna.Framework.Content.Int16Reader
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

namespace Microsoft.Xna.Framework.Content
{
  internal class Int16Reader : ContentTypeReader<short>
  {
    internal Int16Reader()
    {
    }

    protected internal override short Read(ContentReader input, short existingInstance)
    {
      return input.ReadInt16();
    }
  }
}
