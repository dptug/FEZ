// Type: Microsoft.Xna.Framework.Content.Int32Reader
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

namespace Microsoft.Xna.Framework.Content
{
  internal class Int32Reader : ContentTypeReader<int>
  {
    internal Int32Reader()
    {
    }

    protected internal override int Read(ContentReader input, int existingInstance)
    {
      return input.ReadInt32();
    }
  }
}
