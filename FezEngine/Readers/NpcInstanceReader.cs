// Type: FezEngine.Readers.NpcInstanceReader
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Structure;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace FezEngine.Readers
{
  public class NpcInstanceReader : ContentTypeReader<NpcInstance>
  {
    protected override NpcInstance Read(ContentReader input, NpcInstance existingInstance)
    {
      if (existingInstance == null)
        existingInstance = new NpcInstance();
      existingInstance.Name = input.ReadString();
      existingInstance.Position = input.ReadVector3();
      existingInstance.DestinationOffset = input.ReadVector3();
      existingInstance.WalkSpeed = input.ReadSingle();
      existingInstance.RandomizeSpeech = input.ReadBoolean();
      existingInstance.SayFirstSpeechLineOnce = input.ReadBoolean();
      existingInstance.AvoidsGomez = input.ReadBoolean();
      existingInstance.ActorType = input.ReadObject<ActorType>();
      existingInstance.Speech = input.ReadObject<List<SpeechLine>>(existingInstance.Speech);
      existingInstance.Actions = input.ReadObject<Dictionary<NpcAction, NpcActionContent>>(existingInstance.Actions);
      return existingInstance;
    }
  }
}
