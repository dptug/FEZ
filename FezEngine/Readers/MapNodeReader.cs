// Type: FezEngine.Readers.MapNodeReader
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine;
using FezEngine.Structure;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace FezEngine.Readers
{
  public class MapNodeReader : ContentTypeReader<MapNode>
  {
    protected override MapNode Read(ContentReader input, MapNode existingInstance)
    {
      if (existingInstance == null)
        existingInstance = new MapNode();
      existingInstance.LevelName = input.ReadString();
      existingInstance.Connections = input.ReadObject<List<MapNode.Connection>>(existingInstance.Connections);
      existingInstance.NodeType = input.ReadObject<LevelNodeType>();
      existingInstance.Conditions = input.ReadObject<WinConditions>(existingInstance.Conditions);
      existingInstance.HasLesserGate = input.ReadBoolean();
      existingInstance.HasWarpGate = input.ReadBoolean();
      return existingInstance;
    }
  }
}
