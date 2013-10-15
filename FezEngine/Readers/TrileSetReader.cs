// Type: FezEngine.Readers.TrileSetReader
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Structure;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FezEngine.Readers
{
  public class TrileSetReader : ContentTypeReader<TrileSet>
  {
    protected override TrileSet Read(ContentReader input, TrileSet existingInstance)
    {
      if (existingInstance == null)
        existingInstance = new TrileSet();
      existingInstance.Name = input.ReadString();
      existingInstance.Triles = input.ReadObject<Dictionary<int, Trile>>(existingInstance.Triles);
      existingInstance.TextureAtlas = input.ReadObject<Texture2D>();
      existingInstance.OnDeserialization();
      return existingInstance;
    }
  }
}
