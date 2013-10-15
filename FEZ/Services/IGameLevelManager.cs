// Type: FezGame.Services.IGameLevelManager
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Services;
using FezEngine.Structure;
using System.Collections.Generic;

namespace FezGame.Services
{
  public interface IGameLevelManager : ILevelManager
  {
    IDictionary<TrileInstance, TrileGroup> PickupGroups { get; }

    string LastLevelName { get; set; }

    int? DestinationVolumeId { get; set; }

    bool DestinationIsFarAway { get; set; }

    bool WentThroughSecretPassage { get; set; }

    bool SongChanged { get; set; }

    void RemoveArtObject(ArtObjectInstance aoInstance);

    void ChangeLevel(string levelName);

    void ChangeSky(Sky sky);

    void Reset();
  }
}
