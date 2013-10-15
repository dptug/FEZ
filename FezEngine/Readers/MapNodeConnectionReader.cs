// Type: FezEngine.Readers.MapNodeConnectionReader
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine;
using FezEngine.Structure;
using Microsoft.Xna.Framework.Content;

namespace FezEngine.Readers
{
  public class MapNodeConnectionReader : ContentTypeReader<MapNode.Connection>
  {
    protected override MapNode.Connection Read(ContentReader input, MapNode.Connection existingInstance)
    {
      if (existingInstance == null)
        existingInstance = new MapNode.Connection();
      existingInstance.Face = input.ReadObject<FaceOrientation>();
      existingInstance.Node = input.ReadObject<MapNode>(existingInstance.Node);
      existingInstance.BranchOversize = input.ReadSingle();
      return existingInstance;
    }
  }
}
