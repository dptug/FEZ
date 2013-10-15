// Type: FezEngine.Structure.MapTree
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using ContentSerialization;
using FezEngine;
using System.Collections.Generic;

namespace FezEngine.Structure
{
  public class MapTree
  {
    private const string Hub = "NATURE_HUB";
    public MapNode Root;

    public void Fill(string contentRoot)
    {
      MapTree.TreeFillContext context = new MapTree.TreeFillContext()
      {
        ContentRoot = contentRoot
      };
      this.Root = new MapNode()
      {
        LevelName = "NATURE_HUB"
      };
      context.LoadedNodes.Add("NATURE_HUB", this.Root);
      this.Root.Fill(context, (MapNode) null, FaceOrientation.Front);
      SdlSerializer.Serialize<MapTree>(contentRoot + "\\MapTree.map.sdl", this);
    }

    public MapTree Clone()
    {
      return new MapTree()
      {
        Root = this.Root.Clone()
      };
    }

    public class TreeFillContext
    {
      public readonly Dictionary<string, MapNode> LoadedNodes = new Dictionary<string, MapNode>();
      public readonly Dictionary<string, TrileSet> TrileSetCache = new Dictionary<string, TrileSet>();
      public string ContentRoot;
    }
  }
}
