// Type: FezEngine.Structure.Level
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using ContentSerialization;
using ContentSerialization.Attributes;
using FezEngine;
using FezEngine.Structure.Scripting;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace FezEngine.Structure
{
  public class Level : IDeserializationCallback
  {
    [Serialization(CollectionItemName = "Trile")]
    public Dictionary<TrileEmplacement, TrileInstance> Triles;

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public bool Flat { get; set; }

    public string Name { get; set; }

    public TrileFace StartingPosition { get; set; }

    public Vector3 Size { get; set; }

    [Serialization(Optional = true)]
    public string SequenceSamplesPath { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public bool SkipPostProcess { get; set; }

    [Serialization(Optional = true)]
    public float BaseDiffuse { get; set; }

    [Serialization(Optional = true)]
    public float BaseAmbient { get; set; }

    [Serialization(Optional = true)]
    public string GomezHaloName { get; set; }

    [Serialization(Optional = true)]
    public bool HaloFiltering { get; set; }

    [Serialization(Optional = true)]
    public bool BlinkingAlpha { get; set; }

    [Serialization(Optional = true)]
    public bool Loops { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public LiquidType WaterType { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public float WaterHeight { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public bool Descending { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public bool Rainy { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public bool LowPass { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public LevelNodeType NodeType { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public int FAPFadeOutStart { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public int FAPFadeOutLength { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public bool Quantum { get; set; }

    public string SkyName { get; set; }

    public string TrileSetName { get; set; }

    [Serialization(Optional = true)]
    public string SongName { get; set; }

    [Serialization(Ignore = true)]
    public Sky Sky { get; set; }

    [Serialization(Ignore = true)]
    public TrileSet TrileSet { get; set; }

    [Serialization(Ignore = true)]
    public TrackedSong Song { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public Dictionary<int, Volume> Volumes { get; set; }

    [Serialization(CollectionItemName = "Object", DefaultValueOptional = true, Optional = true)]
    public Dictionary<int, ArtObjectInstance> ArtObjects { get; set; }

    [Serialization(CollectionItemName = "Plane", DefaultValueOptional = true, Optional = true)]
    public Dictionary<int, BackgroundPlane> BackgroundPlanes { get; set; }

    [Serialization(CollectionItemName = "Script", DefaultValueOptional = true, Optional = true)]
    public Dictionary<int, Script> Scripts { get; set; }

    [Serialization(CollectionItemName = "Group", DefaultValueOptional = true, Optional = true)]
    public Dictionary<int, TrileGroup> Groups { get; set; }

    [Serialization(CollectionItemName = "Npc", DefaultValueOptional = true, Optional = true)]
    public Dictionary<int, NpcInstance> NonPlayerCharacters { get; set; }

    [Serialization(CollectionItemName = "Path", DefaultValueOptional = true, Optional = true)]
    public Dictionary<int, MovementPath> Paths { get; set; }

    [Serialization(CollectionItemName = "Loop", DefaultValueOptional = true, Optional = true)]
    public List<string> MutedLoops { get; set; }

    [Serialization(CollectionItemName = "Track", DefaultValueOptional = true, Optional = true)]
    public List<AmbienceTrack> AmbienceTracks { get; set; }

    public Level()
    {
      this.Triles = new Dictionary<TrileEmplacement, TrileInstance>();
      this.Volumes = new Dictionary<int, Volume>();
      this.ArtObjects = new Dictionary<int, ArtObjectInstance>();
      this.BackgroundPlanes = new Dictionary<int, BackgroundPlane>();
      this.Groups = new Dictionary<int, TrileGroup>();
      this.Scripts = new Dictionary<int, Script>();
      this.NonPlayerCharacters = new Dictionary<int, NpcInstance>();
      this.Paths = new Dictionary<int, MovementPath>();
      this.MutedLoops = new List<string>();
      this.AmbienceTracks = new List<AmbienceTrack>();
      this.BaseDiffuse = 1f;
      this.BaseAmbient = 0.35f;
      this.HaloFiltering = true;
    }

    public void OnDeserialization()
    {
      foreach (TrileEmplacement index in this.Triles.Keys)
      {
        TrileInstance trileInstance1 = this.Triles[index];
        if (this.Triles[index].Emplacement != index)
          this.Triles[index].Emplacement = index;
        trileInstance1.Update();
        trileInstance1.OriginalEmplacement = index;
        if (trileInstance1.Overlaps)
        {
          foreach (TrileInstance trileInstance2 in trileInstance1.OverlappedTriles)
            trileInstance2.OriginalEmplacement = index;
        }
      }
      foreach (int index in this.Scripts.Keys)
        this.Scripts[index].Id = index;
      foreach (int index in this.Volumes.Keys)
        this.Volumes[index].Id = index;
      foreach (int index in this.NonPlayerCharacters.Keys)
        this.NonPlayerCharacters[index].Id = index;
      foreach (int index in this.ArtObjects.Keys)
        this.ArtObjects[index].Id = index;
      foreach (int index in this.BackgroundPlanes.Keys)
        this.BackgroundPlanes[index].Id = index;
      foreach (int index in this.Paths.Keys)
        this.Paths[index].Id = index;
      foreach (int index1 in this.Groups.Keys)
      {
        TrileGroup trileGroup = this.Groups[index1];
        trileGroup.Id = index1;
        TrileEmplacement[] trileEmplacementArray = new TrileEmplacement[trileGroup.Triles.Count];
        for (int index2 = 0; index2 < trileEmplacementArray.Length; ++index2)
          trileEmplacementArray[index2] = trileGroup.Triles[index2].Emplacement;
        trileGroup.Triles.Clear();
        foreach (TrileEmplacement index2 in trileEmplacementArray)
          trileGroup.Triles.Add(this.Triles[index2]);
      }
    }
  }
}
