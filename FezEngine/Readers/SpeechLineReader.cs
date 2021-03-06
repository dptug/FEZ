﻿// Type: FezEngine.Readers.SpeechLineReader
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Structure;
using Microsoft.Xna.Framework.Content;

namespace FezEngine.Readers
{
  public class SpeechLineReader : ContentTypeReader<SpeechLine>
  {
    protected override SpeechLine Read(ContentReader input, SpeechLine existingInstance)
    {
      if (existingInstance == null)
        existingInstance = new SpeechLine();
      existingInstance.Text = input.ReadObject<string>();
      existingInstance.OverrideContent = input.ReadObject<NpcActionContent>(existingInstance.OverrideContent);
      return existingInstance;
    }
  }
}
