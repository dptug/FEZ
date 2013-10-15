// Type: Microsoft.Xna.Framework.Content.UInt16Reader
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

namespace Microsoft.Xna.Framework.Content
{
  internal class UInt16Reader : ContentTypeReader<ushort>
  {
    internal UInt16Reader()
    {
    }

    protected internal override ushort Read(ContentReader input, ushort existingInstance)
    {
      return input.ReadUInt16();
    }
  }
}
