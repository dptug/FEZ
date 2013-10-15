// Type: FezEngine.Readers.LevelReader
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine;
using FezEngine.Structure;
using FezEngine.Structure.Scripting;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace FezEngine.Readers
{
  public class LevelReader : ContentTypeReader<Level>
  {
    public static bool MinimalRead;

    protected override Level Read(ContentReader input, Level existingInstance)
    {
      if (existingInstance == null)
        existingInstance = new Level();
      existingInstance.Name = input.ReadObject<string>();
      existingInstance.Size = input.ReadVector3();
      existingInstance.StartingPosition = input.ReadObject<TrileFace>(existingInstance.StartingPosition);
      existingInstance.SequenceSamplesPath = input.ReadObject<string>();
      existingInstance.Flat = input.ReadBoolean();
      existingInstance.SkipPostProcess = input.ReadBoolean();
      existingInstance.BaseDiffuse = input.ReadSingle();
      existingInstance.BaseAmbient = input.ReadSingle();
      existingInstance.GomezHaloName = input.ReadObject<string>();
      existingInstance.HaloFiltering = input.ReadBoolean();
      existingInstance.BlinkingAlpha = input.ReadBoolean();
      existingInstance.Loops = input.ReadBoolean();
      existingInstance.WaterType = input.ReadObject<LiquidType>();
      existingInstance.WaterHeight = input.ReadSingle();
      existingInstance.SkyName = input.ReadString();
      existingInstance.TrileSetName = input.ReadObject<string>();
      existingInstance.Volumes = input.ReadObject<Dictionary<int, Volume>>(existingInstance.Volumes);
      existingInstance.Scripts = input.ReadObject<Dictionary<int, Script>>(existingInstance.Scripts);
      existingInstance.SongName = input.ReadObject<string>();
      existingInstance.FAPFadeOutStart = input.ReadInt32();
      existingInstance.FAPFadeOutLength = input.ReadInt32();
      if (!LevelReader.MinimalRead)
      {
        existingInstance.Triles = input.ReadObject<Dictionary<TrileEmplacement, TrileInstance>>(existingInstance.Triles);
        existingInstance.ArtObjects = input.ReadObject<Dictionary<int, ArtObjectInstance>>(existingInstance.ArtObjects);
        existingInstance.BackgroundPlanes = input.ReadObject<Dictionary<int, BackgroundPlane>>(existingInstance.BackgroundPlanes);
        existingInstance.Groups = input.ReadObject<Dictionary<int, TrileGroup>>(existingInstance.Groups);
        existingInstance.NonPlayerCharacters = input.ReadObject<Dictionary<int, NpcInstance>>(existingInstance.NonPlayerCharacters);
        existingInstance.Paths = input.ReadObject<Dictionary<int, MovementPath>>(existingInstance.Paths);
        existingInstance.Descending = input.ReadBoolean();
        existingInstance.Rainy = input.ReadBoolean();
        existingInstance.LowPass = input.ReadBoolean();
        existingInstance.MutedLoops = input.ReadObject<List<string>>(existingInstance.MutedLoops);
        existingInstance.AmbienceTracks = input.ReadObject<List<AmbienceTrack>>(existingInstance.AmbienceTracks);
        existingInstance.NodeType = input.ReadObject<LevelNodeType>();
        existingInstance.Quantum = input.ReadBoolean();
      }
      existingInstance.OnDeserialization();
      return existingInstance;
    }
  }
}
