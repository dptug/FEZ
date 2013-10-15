// Type: FezEngine.Structure.NpcInstance
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using ContentSerialization.Attributes;
using FezEngine.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezEngine.Structure
{
  public class NpcInstance
  {
    [Serialization(Ignore = true)]
    public readonly NpcMetadata Metadata = new NpcMetadata();

    [Serialization(Ignore = true)]
    public int Id { get; set; }

    [Serialization(Ignore = true)]
    public bool Talking { get; set; }

    [Serialization(Ignore = true)]
    public bool Enabled { get; set; }

    [Serialization(Ignore = true)]
    public bool Visible { get; set; }

    public string Name { get; set; }

    public Vector3 Position { get; set; }

    public Vector3 DestinationOffset { get; set; }

    [Serialization(Optional = true)]
    public bool RandomizeSpeech { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public bool SayFirstSpeechLineOnce { get; set; }

    public List<SpeechLine> Speech { get; set; }

    [Serialization(Ignore = true)]
    public SpeechLine CustomSpeechLine { get; set; }

    [Serialization(Ignore = true)]
    public Group Group { get; set; }

    [Serialization(Ignore = true)]
    public NpcState State { get; set; }

    public Dictionary<NpcAction, NpcActionContent> Actions { get; set; }

    [Serialization(Optional = true)]
    public float WalkSpeed
    {
      get
      {
        return this.Metadata.WalkSpeed;
      }
      set
      {
        this.Metadata.WalkSpeed = value;
      }
    }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public bool AvoidsGomez
    {
      get
      {
        return this.Metadata.AvoidsGomez;
      }
      set
      {
        this.Metadata.AvoidsGomez = value;
      }
    }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public ActorType ActorType
    {
      get
      {
        return this.Metadata.ActorType;
      }
      set
      {
        this.Metadata.ActorType = value;
      }
    }

    public NpcInstance()
    {
      this.Speech = new List<SpeechLine>();
      this.Actions = new Dictionary<NpcAction, NpcActionContent>((IEqualityComparer<NpcAction>) NpcActionComparer.Default);
      this.Enabled = true;
      this.Visible = true;
    }

    public NpcInstance Clone()
    {
      List<SpeechLine> list = Enumerable.ToList<SpeechLine>(Enumerable.Select<SpeechLine, SpeechLine>((IEnumerable<SpeechLine>) this.Speech, (Func<SpeechLine, SpeechLine>) (line => line.Clone())));
      Dictionary<NpcAction, NpcActionContent> dictionary = Enumerable.ToDictionary<NpcAction, NpcAction, NpcActionContent>((IEnumerable<NpcAction>) this.Actions.Keys, (Func<NpcAction, NpcAction>) (action => action), (Func<NpcAction, NpcActionContent>) (action => this.Actions[action].Clone()));
      return new NpcInstance()
      {
        Name = this.Name,
        Position = this.Position,
        DestinationOffset = this.DestinationOffset,
        WalkSpeed = this.WalkSpeed,
        RandomizeSpeech = this.RandomizeSpeech,
        SayFirstSpeechLineOnce = this.SayFirstSpeechLineOnce,
        Speech = list,
        Actions = dictionary,
        AvoidsGomez = this.AvoidsGomez,
        ActorType = this.ActorType
      };
    }

    public void CopyFrom(NpcInstance instance)
    {
      this.Name = instance.Name;
      this.Position = instance.Position;
      this.DestinationOffset = instance.DestinationOffset;
      this.WalkSpeed = instance.WalkSpeed;
      this.RandomizeSpeech = instance.RandomizeSpeech;
      this.SayFirstSpeechLineOnce = instance.SayFirstSpeechLineOnce;
      this.Speech = instance.Speech;
      this.Actions = instance.Actions;
      this.AvoidsGomez = instance.AvoidsGomez;
      this.ActorType = instance.ActorType;
    }

    public void FillMetadata(NpcMetadata md)
    {
      this.Metadata.AvoidsGomez = md.AvoidsGomez;
      this.Metadata.WalkSpeed = md.WalkSpeed;
      this.Metadata.SoundPath = md.SoundPath;
      this.Metadata.SoundActions = md.SoundActions;
    }
  }
}
