// Type: FezEngine.Readers.ArtObjectActorSettingsReader
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine;
using FezEngine.Structure;
using FezEngine.Structure.Input;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace FezEngine.Readers
{
  public class ArtObjectActorSettingsReader : ContentTypeReader<ArtObjectActorSettings>
  {
    protected override ArtObjectActorSettings Read(ContentReader input, ArtObjectActorSettings existingInstance)
    {
      if (existingInstance == (ArtObjectActorSettings) null)
        existingInstance = new ArtObjectActorSettings();
      existingInstance.Inactive = input.ReadBoolean();
      existingInstance.ContainedTrile = input.ReadObject<ActorType>();
      existingInstance.AttachedGroup = input.ReadObject<int?>();
      existingInstance.SpinView = input.ReadObject<Viewpoint>();
      existingInstance.SpinEvery = input.ReadSingle();
      existingInstance.SpinOffset = input.ReadSingle();
      existingInstance.OffCenter = input.ReadBoolean();
      existingInstance.RotationCenter = input.ReadVector3();
      existingInstance.VibrationPattern = input.ReadObject<VibrationMotor[]>();
      existingInstance.CodePattern = input.ReadObject<CodeInput[]>();
      existingInstance.Segment = input.ReadObject<PathSegment>();
      existingInstance.NextNode = input.ReadObject<int?>();
      existingInstance.DestinationLevel = input.ReadObject<string>();
      existingInstance.TreasureMapName = input.ReadObject<string>();
      existingInstance.InvisibleSides = new HashSet<FaceOrientation>((IEnumerable<FaceOrientation>) input.ReadObject<FaceOrientation[]>(), (IEqualityComparer<FaceOrientation>) FaceOrientationComparer.Default);
      existingInstance.TimeswitchWindBackSpeed = input.ReadSingle();
      return existingInstance;
    }
  }
}
