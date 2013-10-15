// Type: Microsoft.Xna.Framework.Content.DateTimeReader
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using System;

namespace Microsoft.Xna.Framework.Content
{
  internal class DateTimeReader : ContentTypeReader<DateTime>
  {
    internal DateTimeReader()
    {
    }

    protected internal override DateTime Read(ContentReader input, DateTime existingInstance)
    {
      ulong num1 = input.ReadUInt64();
      ulong num2 = 13835058055282163712UL;
      return new DateTime((long) num1 & ~(long) num2, (DateTimeKind) ((long) (num1 >> 62) & 3L));
    }
  }
}
