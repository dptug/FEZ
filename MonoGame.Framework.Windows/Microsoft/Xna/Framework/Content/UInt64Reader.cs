// Type: Microsoft.Xna.Framework.Content.UInt64Reader
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

namespace Microsoft.Xna.Framework.Content
{
  internal class UInt64Reader : ContentTypeReader<ulong>
  {
    internal UInt64Reader()
    {
    }

    protected internal override ulong Read(ContentReader input, ulong existingInstance)
    {
      return input.ReadUInt64();
    }
  }
}
