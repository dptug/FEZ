// Type: Microsoft.Xna.Framework.Content.TimeSpanReader
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System;

namespace Microsoft.Xna.Framework.Content
{
  internal class TimeSpanReader : ContentTypeReader<TimeSpan>
  {
    internal TimeSpanReader()
    {
    }

    protected internal override TimeSpan Read(ContentReader input, TimeSpan existingInstance)
    {
      return new TimeSpan(input.ReadInt64());
    }
  }
}
