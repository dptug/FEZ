// Type: FezEngine.Services.ILevelManager
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine;
using FezEngine.Structure;
using FezEngine.Structure.Scripting;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace FezEngine.Services
{
  public interface ILevelManager
  {
    Vector3 Size { get; }

    TrileFace StartingPosition { get; set; }

    string Name { get; set; }

    string FullPath { get; }

    bool Flat { get; set; }

    string SequenceSamplesPath { get; set; }

    bool Rainy { get; set; }

    bool SkipPostProcess { get; set; }

    float BaseDiffuse { get; set; }

    Color ActualDiffuse { get; set; }

    float BaseAmbient { get; set; }

    Color ActualAmbient { get; set; }

    string GomezHaloName { get; set; }

    bool HaloFiltering { get; set; }

    bool BlinkingAlpha { get; set; }

    string SongName { get; }

    bool LowPass { get; set; }

    LiquidType WaterType { get; set; }

    float WaterHeight { get; set; }

    float WaterSpeed { get; set; }

    float OriginalWaterHeight { get; set; }

    bool Loops { get; set; }

    bool Descending { get; set; }

    LevelNodeType NodeType { get; set; }

    int FAPFadeOutStart { get; set; }

    int FAPFadeOutLength { get; set; }

    bool SkipInvalidation { get; set; }

    bool Quantum { get; set; }

    IDictionary<TrileEmplacement, TrileInstance> Triles { get; }

    IDictionary<int, Volume> Volumes { get; }

    IDictionary<int, ArtObjectInstance> ArtObjects { get; }

    IDictionary<int, BackgroundPlane> BackgroundPlanes { get; }

    IDictionary<int, TrileGroup> Groups { get; }

    IDictionary<int, NpcInstance> NonPlayerCharacters { get; }

    IDictionary<int, Script> Scripts { get; }

    IDictionary<int, MovementPath> Paths { get; }

    IList<string> MutedLoops { get; }

    IList<AmbienceTrack> AmbienceTracks { get; }

    bool IsInvalidatingScreen { get; }

    Dictionary<Point, Limit> ScreenSpaceLimits { get; }

    TrileSet TrileSet { get; }

    Sky Sky { get; }

    TrackedSong Song { get; }

    event Action LevelChanged;

    event Action LevelChanging;

    event Action SkyChanged;

    event Action LightingChanged;

    event Action<TrileInstance> TrileRestored;

    event Action ScreenInvalidated;

    void Load(string levelName);

    void Rebuild();

    Trile SafeGetTrile(int trileId);

    TrileInstance TrileInstanceAt(ref TrileEmplacement id);

    bool IsCornerTrile(ref TrileEmplacement id, ref FaceOrientation face1, ref FaceOrientation face2);

    bool IsBorderTrileFace(ref TrileEmplacement id, ref FaceOrientation face);

    bool IsBorderTrile(ref TrileEmplacement id);

    bool IsInRange(Vector3 position);

    bool IsInRange(ref TrileEmplacement id);

    bool TrileExists(TrileEmplacement id);

    bool VolumeExists(int id);

    void SwapTrile(TrileInstance instance, Trile newTrile);

    void RestoreTrile(TrileInstance instance);

    bool ClearTrile(TrileInstance instance);

    bool ClearTrile(TrileEmplacement id);

    bool ClearTrile(TrileInstance instance, bool skipRecull);

    void RecullAt(TrileInstance instance);

    void RecullAt(TrileEmplacement emplacement);

    void RecullAt(Point ssPos);

    NearestTriles NearestTrile(Vector3 position);

    NearestTriles NearestTrile(Vector3 position, QueryOptions options);

    NearestTriles NearestTrile(Vector3 position, QueryOptions options, Viewpoint? viewpoint);

    void AddPlane(BackgroundPlane plane);

    void RemovePlane(BackgroundPlane plane);

    TrileInstance ActualInstanceAt(Vector3 position);

    IEnumerable<Trile> ActorTriles(ActorType type);

    IEnumerable<string> LinkedLevels();

    void UpdateInstance(TrileInstance instance);

    void ClearArtSatellites();

    void RecordMoveToEnd(int groupId);

    bool IsPathRecorded(int groupId);

    bool WasPathSupposedToBeRecorded(int id);

    void WaitForScreenInvalidation();

    void AbortInvalidation();
  }
}
