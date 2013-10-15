// Type: FezEngine.Readers.TrackedSongReader
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Structure;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace FezEngine.Readers
{
  public class TrackedSongReader : ContentTypeReader<TrackedSong>
  {
    protected override TrackedSong Read(ContentReader input, TrackedSong existingInstance)
    {
      if (existingInstance == null)
        existingInstance = new TrackedSong();
      existingInstance.Loops = input.ReadObject<List<Loop>>(existingInstance.Loops);
      existingInstance.Name = input.ReadString();
      existingInstance.Tempo = input.ReadInt32();
      existingInstance.TimeSignature = input.ReadInt32();
      existingInstance.Notes = input.ReadObject<ShardNotes[]>();
      existingInstance.AssembleChord = input.ReadObject<AssembleChords>();
      existingInstance.RandomOrdering = input.ReadBoolean();
      existingInstance.CustomOrdering = input.ReadObject<int[]>();
      return existingInstance;
    }
  }
}
