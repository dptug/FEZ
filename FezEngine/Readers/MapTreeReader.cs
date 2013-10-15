// Type: FezEngine.Readers.MapTreeReader
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Structure;
using Microsoft.Xna.Framework.Content;

namespace FezEngine.Readers
{
  public class MapTreeReader : ContentTypeReader<MapTree>
  {
    protected override MapTree Read(ContentReader input, MapTree existingInstance)
    {
      if (existingInstance == null)
        existingInstance = new MapTree();
      existingInstance.Root = input.ReadObject<MapNode>(existingInstance.Root);
      return existingInstance;
    }
  }
}
