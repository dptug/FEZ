// Type: FezEngine.Readers.VolumeActorSettingsReader
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Structure;
using FezEngine.Structure.Input;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace FezEngine.Readers
{
  public class VolumeActorSettingsReader : ContentTypeReader<VolumeActorSettings>
  {
    protected override VolumeActorSettings Read(ContentReader input, VolumeActorSettings existingInstance)
    {
      if (existingInstance == null)
        existingInstance = new VolumeActorSettings();
      existingInstance.FarawayPlaneOffset = input.ReadVector2();
      existingInstance.IsPointOfInterest = input.ReadBoolean();
      existingInstance.DotDialogue = input.ReadObject<List<DotDialogueLine>>(existingInstance.DotDialogue);
      existingInstance.WaterLocked = input.ReadBoolean();
      existingInstance.CodePattern = input.ReadObject<CodeInput[]>();
      existingInstance.IsBlackHole = input.ReadBoolean();
      existingInstance.NeedsTrigger = input.ReadBoolean();
      existingInstance.IsSecretPassage = input.ReadBoolean();
      return existingInstance;
    }
  }
}
