// Type: FezEngine.Readers.TrileFaceReader
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine;
using FezEngine.Structure;
using Microsoft.Xna.Framework.Content;

namespace FezEngine.Readers
{
  public class TrileFaceReader : ContentTypeReader<TrileFace>
  {
    protected override TrileFace Read(ContentReader input, TrileFace existingInstance)
    {
      if (existingInstance == (TrileFace) null)
        existingInstance = new TrileFace();
      existingInstance.Id = input.ReadObject<TrileEmplacement>();
      existingInstance.Face = input.ReadObject<FaceOrientation>();
      return existingInstance;
    }
  }
}
