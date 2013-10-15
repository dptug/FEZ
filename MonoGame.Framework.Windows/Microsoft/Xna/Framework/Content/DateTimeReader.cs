// Type: Microsoft.Xna.Framework.Content.DateTimeReader
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

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
