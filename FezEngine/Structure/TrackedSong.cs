// Type: FezEngine.Structure.TrackedSong
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using ContentSerialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezEngine.Structure
{
  public class TrackedSong
  {
    public string Name = "Untitled";
    public List<Loop> Loops = new List<Loop>();
    public int Tempo = 60;
    public int TimeSignature = 4;
    [Serialization(Optional = true)]
    public ShardNotes[] Notes = new ShardNotes[8]
    {
      ShardNotes.C2,
      ShardNotes.D2,
      ShardNotes.E2,
      ShardNotes.F2,
      ShardNotes.G2,
      ShardNotes.A2,
      ShardNotes.B2,
      ShardNotes.C3
    };
    [Serialization(Optional = true)]
    public AssembleChords AssembleChord;
    [Serialization(Optional = true)]
    public bool RandomOrdering;
    [Serialization(Optional = true)]
    public int[] CustomOrdering;

    public void Initialize()
    {
      foreach (Loop loop in this.Loops)
      {
        if (loop.Initialized)
        {
          loop.Dawn = loop.OriginalDawn;
          loop.Dusk = loop.OriginalDusk;
          loop.Night = loop.OriginalNight;
          loop.Day = loop.OriginalDay;
        }
        else
        {
          loop.OriginalDawn = loop.Dawn;
          loop.OriginalDusk = loop.Dusk;
          loop.OriginalNight = loop.Night;
          loop.OriginalDay = loop.Day;
          loop.Initialized = true;
        }
      }
    }

    public TrackedSong Clone()
    {
      return new TrackedSong()
      {
        Name = this.Name,
        Loops = new List<Loop>(Enumerable.Select<Loop, Loop>((IEnumerable<Loop>) this.Loops, (Func<Loop, Loop>) (loop => loop.Clone()))),
        Tempo = this.Tempo,
        TimeSignature = this.TimeSignature,
        Notes = Enumerable.ToArray<ShardNotes>((IEnumerable<ShardNotes>) this.Notes),
        AssembleChord = this.AssembleChord,
        RandomOrdering = this.RandomOrdering,
        CustomOrdering = this.CustomOrdering == null ? (int[]) null : Enumerable.ToArray<int>((IEnumerable<int>) this.CustomOrdering)
      };
    }

    public void UpdateFromCopy(TrackedSong other)
    {
      this.Name = other.Name;
      this.Loops = other.Loops;
      this.Tempo = other.Tempo;
      this.TimeSignature = other.TimeSignature;
      this.Notes = other.Notes;
      this.AssembleChord = other.AssembleChord;
      this.RandomOrdering = other.RandomOrdering;
      this.CustomOrdering = other.CustomOrdering;
    }
  }
}
