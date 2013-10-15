// Type: FezEngine.Readers.FrameReader
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;

namespace FezEngine.Readers
{
  public class FrameReader : ContentTypeReader<FrameContent>
  {
    protected override FrameContent Read(ContentReader input, FrameContent existingInstance)
    {
      if (existingInstance == null)
        existingInstance = new FrameContent();
      existingInstance.Duration = input.ReadObject<TimeSpan>();
      existingInstance.Rectangle = input.ReadObject<Rectangle>();
      return existingInstance;
    }
  }
}
