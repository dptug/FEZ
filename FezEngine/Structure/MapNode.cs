// Type: FezEngine.Structure.MapNode
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using ContentSerialization;
using ContentSerialization.Attributes;
using FezEngine;
using FezEngine.Structure.Scripting;
using FezEngine.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezEngine.Structure
{
  public class MapNode
  {
    private static readonly string[] UpLevels = new string[7]
    {
      "TREE_ROOTS",
      "TREE",
      "TREE_SKY",
      "FOX",
      "WATER_TOWER",
      "PIVOT_WATERTOWER",
      "VILLAGEVILLE_3D"
    };
    private static readonly string[] DownLevels = new string[5]
    {
      "SEWER_START",
      "MEMORY_CORE",
      "ZU_FORK",
      "STARGATE",
      "QUANTUM"
    };
    private static readonly string[] OppositeLevels = new string[15]
    {
      "NUZU_SCHOOL",
      "NUZU_ABANDONED_A",
      "ZU_HOUSE_EMPTY_B",
      "PURPLE_LODGE",
      "ZU_HOUSE_SCAFFOLDING",
      "MINE_BOMB_PILLAR",
      "CMY_B",
      "INDUSTRIAL_HUB",
      "SUPERSPIN_CAVE",
      "GRAVE_LESSER_GATE",
      "THRONE",
      "VISITOR",
      "ORRERY",
      "LAVA_SKULL",
      "LAVA_FORK"
    };
    private static readonly string[] BackLevels = new string[2]
    {
      "ABANDONED_B",
      "LAVA"
    };
    private static readonly string[] LeftLevels = new string[0];
    private static readonly string[] FrontLevels = new string[2]
    {
      "VILLAGEVILLE_3D",
      "ZU_LIBRARY"
    };
    private static readonly string[] RightLevels = new string[5]
    {
      "WALL_SCHOOL",
      "WALL_KITCHEN",
      "WALL_INTERIOR_HOLE",
      "WALL_INTERIOR_B",
      "WALL_INTERIOR_A"
    };
    private static readonly string[] PuzzleLevels = new string[5]
    {
      "ZU_ZUISH",
      "ZU_UNFOLD",
      "BELL_TOWER",
      "CLOCK",
      "ZU_TETRIS"
    };
    private static readonly Dictionary<string, float> OversizeLinks = new Dictionary<string, float>()
    {
      {
        "SEWER_START",
        5.5f
      },
      {
        "TREE",
        1.25f
      },
      {
        "TREE_SKY",
        1f
      },
      {
        "INDUSTRIAL_HUB",
        0.5f
      },
      {
        "VILLAGEVILLE_3D",
        -0.5f
      },
      {
        "WALL_VILLAGE",
        0.5f
      },
      {
        "ZU_CITY",
        0.5f
      },
      {
        "INDUSTRIAL_CITY",
        0.5f
      },
      {
        "MEMORY_CORE",
        0.5f
      },
      {
        "BIG_TOWER",
        0.5f
      },
      {
        "STARGATE",
        -0.5f
      },
      {
        "WATERFALL",
        0.25f
      },
      {
        "BELL_TOWER",
        0.25f
      },
      {
        "LIGHTHOUSE",
        0.25f
      },
      {
        "ARCH",
        0.25f
      }
    };
    public List<MapNode.Connection> Connections = new List<MapNode.Connection>();
    public string LevelName;
    public LevelNodeType NodeType;
    [Serialization(Optional = true)]
    public WinConditions Conditions;
    [Serialization(DefaultValueOptional = true, Optional = true)]
    public bool HasLesserGate;
    [Serialization(DefaultValueOptional = true, Optional = true)]
    public bool HasWarpGate;
    [Serialization(Ignore = true)]
    public bool Valid;
    [Serialization(Ignore = true)]
    public Group Group;

    static MapNode()
    {
    }

    public MapNode()
    {
      this.Conditions = new WinConditions();
    }

    public void Fill(MapTree.TreeFillContext context, MapNode parent, FaceOrientation origin)
    {
      if (this.Valid)
        return;
      TrileSet trileSet = (TrileSet) null;
      Level level;
      try
      {
        level = SdlSerializer.Deserialize<Level>(context.ContentRoot + "\\Levels\\" + this.LevelName + ".lvl.sdl");
        if (level.TrileSetName != null)
        {
          if (!context.TrileSetCache.TryGetValue(level.TrileSetName, out trileSet))
            context.TrileSetCache.Add(level.TrileSetName, trileSet = SdlSerializer.Deserialize<TrileSet>(context.ContentRoot + "\\Trile Sets\\" + level.TrileSetName + ".ts.sdl"));
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine("Warning : Level " + this.LevelName + " could not be loaded because it has invalid markup. Skipping...");
        this.Valid = false;
        return;
      }
      this.NodeType = level.NodeType;
      this.Conditions.ChestCount = Enumerable.Count<ArtObjectInstance>((IEnumerable<ArtObjectInstance>) level.ArtObjects.Values, (Func<ArtObjectInstance, bool>) (x => x.ArtObjectName.IndexOf("treasure", StringComparison.InvariantCultureIgnoreCase) != -1)) / 2;
      this.Conditions.ScriptIds = Enumerable.ToList<int>(Enumerable.Select<Script, int>(Enumerable.Where<Script>((IEnumerable<Script>) level.Scripts.Values, (Func<Script, bool>) (x => x.IsWinCondition)), (Func<Script, int>) (x => x.Id)));
      this.Conditions.SplitUpCount = Enumerable.Count<TrileInstance>(Enumerable.Union<TrileInstance>((IEnumerable<TrileInstance>) level.Triles.Values, Enumerable.SelectMany<TrileInstance, TrileInstance>(Enumerable.Where<TrileInstance>((IEnumerable<TrileInstance>) level.Triles.Values, (Func<TrileInstance, bool>) (x => x.Overlaps)), (Func<TrileInstance, IEnumerable<TrileInstance>>) (x => (IEnumerable<TrileInstance>) x.OverlappedTriles))), (Func<TrileInstance, bool>) (x =>
      {
        if (x.TrileId >= 0)
          return trileSet[x.TrileId].ActorSettings.Type == ActorType.GoldenCube;
        else
          return false;
      }));
      this.Conditions.CubeShardCount = Enumerable.Count<TrileInstance>((IEnumerable<TrileInstance>) level.Triles.Values, (Func<TrileInstance, bool>) (x =>
      {
        if (x.TrileId >= 0)
          return trileSet[x.TrileId].ActorSettings.Type == ActorType.CubeShard;
        else
          return false;
      }));
      this.Conditions.OtherCollectibleCount = Enumerable.Count<TrileInstance>((IEnumerable<TrileInstance>) level.Triles.Values, (Func<TrileInstance, bool>) (x =>
      {
        if (x.TrileId >= 0 && ActorTypeExtensions.IsTreasure(trileSet[x.TrileId].ActorSettings.Type))
          return trileSet[x.TrileId].ActorSettings.Type != ActorType.CubeShard;
        else
          return false;
      })) + Enumerable.Count<ArtObjectInstance>((IEnumerable<ArtObjectInstance>) level.ArtObjects.Values, (Func<ArtObjectInstance, bool>) (x => x.ArtObjectName == "treasure_mapAO"));
      this.Conditions.LockedDoorCount = Enumerable.Count<TrileInstance>((IEnumerable<TrileInstance>) level.Triles.Values, (Func<TrileInstance, bool>) (x =>
      {
        if (x.TrileId >= 0)
          return trileSet[x.TrileId].ActorSettings.Type == ActorType.Door;
        else
          return false;
      }));
      this.Conditions.UnlockedDoorCount = Enumerable.Count<TrileInstance>((IEnumerable<TrileInstance>) level.Triles.Values, (Func<TrileInstance, bool>) (x =>
      {
        if (x.TrileId >= 0)
          return trileSet[x.TrileId].ActorSettings.Type == ActorType.UnlockedDoor;
        else
          return false;
      }));
      int num1 = Enumerable.Count<KeyValuePair<int, ArtObjectInstance>>((IEnumerable<KeyValuePair<int, ArtObjectInstance>>) level.ArtObjects, (Func<KeyValuePair<int, ArtObjectInstance>, bool>) (x => x.Value.ArtObjectName.IndexOf("fork", StringComparison.InvariantCultureIgnoreCase) != -1));
      int num2 = Enumerable.Count<KeyValuePair<int, ArtObjectInstance>>((IEnumerable<KeyValuePair<int, ArtObjectInstance>>) level.ArtObjects, (Func<KeyValuePair<int, ArtObjectInstance>, bool>) (x => x.Value.ArtObjectName.IndexOf("qr", StringComparison.InvariantCultureIgnoreCase) != -1));
      int num3 = Enumerable.Count<KeyValuePair<int, Volume>>((IEnumerable<KeyValuePair<int, Volume>>) level.Volumes, (Func<KeyValuePair<int, Volume>, bool>) (x =>
      {
        if (x.Value.ActorSettings != null && x.Value.ActorSettings.CodePattern != null)
          return x.Value.ActorSettings.CodePattern.Length > 0;
        else
          return false;
      }));
      int num4 = this.LevelName == "OWL" ? 0 : Enumerable.Count<KeyValuePair<int, NpcInstance>>((IEnumerable<KeyValuePair<int, NpcInstance>>) level.NonPlayerCharacters, (Func<KeyValuePair<int, NpcInstance>, bool>) (x => x.Value.Name == "Owl"));
      int num5 = Enumerable.Count<KeyValuePair<int, ArtObjectInstance>>((IEnumerable<KeyValuePair<int, ArtObjectInstance>>) level.ArtObjects, (Func<KeyValuePair<int, ArtObjectInstance>, bool>) (x =>
      {
        if (x.Value.ArtObjectName.Contains("BIT_DOOR"))
          return !x.Value.ArtObjectName.Contains("BROKEN");
        else
          return false;
      }));
      int num6 = Enumerable.Count<Script>((IEnumerable<Script>) level.Scripts.Values, (Func<Script, bool>) (s => Enumerable.Any<ScriptAction>((IEnumerable<ScriptAction>) s.Actions, (Func<ScriptAction, bool>) (a =>
      {
        if (a.Object.Type == "Level")
          return a.Operation == "ResolvePuzzle";
        else
          return false;
      }))));
      int num7 = Enumerable.Contains<string>((IEnumerable<string>) MapNode.PuzzleLevels, this.LevelName) ? (this.LevelName == "CLOCK" ? 4 : 1) : 0;
      this.Conditions.SecretCount = num1 + num2 + num3 + num4 + num6 + num7 + num5;
      this.HasLesserGate = Enumerable.Any<ArtObjectInstance>((IEnumerable<ArtObjectInstance>) level.ArtObjects.Values, (Func<ArtObjectInstance, bool>) (x =>
      {
        if (x.ArtObjectName.IndexOf("lesser_gate", StringComparison.InvariantCultureIgnoreCase) != -1)
          return x.ArtObjectName.IndexOf("base", StringComparison.InvariantCultureIgnoreCase) == -1;
        else
          return false;
      }));
      this.HasWarpGate = Enumerable.Any<ArtObjectInstance>((IEnumerable<ArtObjectInstance>) level.ArtObjects.Values, (Func<ArtObjectInstance, bool>) (x =>
      {
        if (!(x.ArtObjectName == "GATE_GRAVEAO") && !(x.ArtObjectName == "GATEAO") && (!(x.ArtObjectName == "GATE_INDUSTRIALAO") && !(x.ArtObjectName == "GATE_SEWERAO")) && !(x.ArtObjectName == "ZU_GATEAO"))
          return x.ArtObjectName == "GRAVE_GATEAO";
        else
          return true;
      }));
      foreach (Script script in level.Scripts.Values)
      {
        foreach (ScriptAction scriptAction in script.Actions)
        {
          if (scriptAction.Object.Type == "Level" && scriptAction.Operation.Contains("Level"))
          {
            MapNode.Connection connection = new MapNode.Connection();
            bool flag = true;
            foreach (ScriptTrigger scriptTrigger in script.Triggers)
            {
              if (scriptTrigger.Object.Type == "Volume" && scriptTrigger.Event == "Enter" && scriptTrigger.Object.Identifier.HasValue)
              {
                int key = scriptTrigger.Object.Identifier.Value;
                Volume volume;
                if (!level.Volumes.TryGetValue(key, out volume))
                {
                  Console.WriteLine("Warning : A level-changing script links to a nonexistent volume in " + (object) this.LevelName + " (Volume Id #" + (string) (object) key + ")");
                  flag = false;
                  break;
                }
                else if (volume.ActorSettings != null && volume.ActorSettings.IsSecretPassage)
                {
                  flag = false;
                  break;
                }
                else
                {
                  connection.Face = Enumerable.First<FaceOrientation>((IEnumerable<FaceOrientation>) volume.Orientations);
                  break;
                }
              }
            }
            if (flag)
            {
              string key = scriptAction.Operation == "ReturnToLastLevel" ? parent.LevelName : scriptAction.Arguments[0];
              if (!(key == "PYRAMID") && !(key == "CABIN_INTERIOR_A") && (!(key == "THRONE") || !(this.LevelName == "ZU_CITY_RUINS")) && (!(key == "ZU_CITY_RUINS") || !(this.LevelName == "THRONE")))
              {
                MapNode mapNode;
                if (!context.LoadedNodes.TryGetValue(key, out mapNode))
                {
                  mapNode = new MapNode()
                  {
                    LevelName = key
                  };
                  context.LoadedNodes.Add(key, mapNode);
                  connection.Node = mapNode;
                  if (connection.Node != parent)
                  {
                    if (parent != null && origin == connection.Face)
                      connection.Face = FezMath.GetOpposite(origin);
                    if (Enumerable.Contains<string>((IEnumerable<string>) MapNode.UpLevels, key))
                      connection.Face = FaceOrientation.Top;
                    else if (Enumerable.Contains<string>((IEnumerable<string>) MapNode.DownLevels, key))
                      connection.Face = FaceOrientation.Down;
                    else if (Enumerable.Contains<string>((IEnumerable<string>) MapNode.OppositeLevels, key))
                      connection.Face = FezMath.GetOpposite(connection.Face);
                    else if (Enumerable.Contains<string>((IEnumerable<string>) MapNode.BackLevels, key))
                      connection.Face = FaceOrientation.Back;
                    else if (Enumerable.Contains<string>((IEnumerable<string>) MapNode.LeftLevels, key))
                      connection.Face = FaceOrientation.Left;
                    else if (Enumerable.Contains<string>((IEnumerable<string>) MapNode.RightLevels, key))
                      connection.Face = FaceOrientation.Right;
                    else if (Enumerable.Contains<string>((IEnumerable<string>) MapNode.FrontLevels, key))
                      connection.Face = FaceOrientation.Front;
                    float num8;
                    if (MapNode.OversizeLinks.TryGetValue(key, out num8))
                      connection.BranchOversize = num8;
                    this.Connections.Add(connection);
                    break;
                  }
                  else
                    break;
                }
                else
                  break;
              }
            }
          }
        }
      }
      this.Valid = true;
      foreach (MapNode.Connection connection in this.Connections)
        connection.Node.Fill(context, this, connection.Face);
    }

    public MapNode Clone()
    {
      return new MapNode()
      {
        LevelName = this.LevelName,
        NodeType = this.NodeType,
        HasWarpGate = this.HasWarpGate,
        HasLesserGate = this.HasLesserGate,
        Connections = Enumerable.ToList<MapNode.Connection>(Enumerable.Select<MapNode.Connection, MapNode.Connection>((IEnumerable<MapNode.Connection>) this.Connections, (Func<MapNode.Connection, MapNode.Connection>) (x => new MapNode.Connection()
        {
          Face = x.Face,
          Node = x.Node.Clone(),
          BranchOversize = x.BranchOversize
        }))),
        Conditions = this.Conditions.Clone()
      };
    }

    public class Connection
    {
      public FaceOrientation Face;
      public MapNode Node;
      [Serialization(DefaultValueOptional = true, Optional = true)]
      public float BranchOversize;
      [Serialization(Ignore = true)]
      public int MultiBranchId;
      [Serialization(Ignore = true)]
      public int MultiBranchCount;
      [Serialization(Ignore = true)]
      public List<int> LinkInstances;
    }
  }
}
