// Type: FezEngine.Readers.VolumeReader
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine;
using FezEngine.Structure;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace FezEngine.Readers
{
  public class VolumeReader : ContentTypeReader<Volume>
  {
    protected override Volume Read(ContentReader input, Volume existingInstance)
    {
      if (existingInstance == null)
        existingInstance = new Volume();
      existingInstance.Orientations = new HashSet<FaceOrientation>((IEnumerable<FaceOrientation>) input.ReadObject<FaceOrientation[]>(), (IEqualityComparer<FaceOrientation>) FaceOrientationComparer.Default);
      existingInstance.From = input.ReadVector3();
      existingInstance.To = input.ReadVector3();
      existingInstance.ActorSettings = input.ReadObject<VolumeActorSettings>(existingInstance.ActorSettings);
      return existingInstance;
    }
  }
}
