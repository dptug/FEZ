// Type: FezEngine.Structure.PathSegment
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using ContentSerialization.Attributes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;

namespace FezEngine.Structure
{
  public class PathSegment
  {
    public Vector3 Destination { get; set; }

    [Serialization(Optional = true)]
    public Quaternion Orientation { get; set; }

    [Serialization(Optional = true)]
    public TimeSpan WaitTimeOnStart { get; set; }

    [Serialization(Optional = true)]
    public TimeSpan WaitTimeOnFinish { get; set; }

    public TimeSpan Duration { get; set; }

    public float Acceleration { get; set; }

    [Serialization(Optional = true)]
    public float Deceleration { get; set; }

    public float JitterFactor { get; set; }

    [Serialization(Ignore = true)]
    public bool Bounced { get; set; }

    [Serialization(Ignore = true)]
    public SoundEffect Sound { get; set; }

    [Serialization(Optional = true)]
    public ICloneable CustomData { get; set; }

    public PathSegment()
    {
      this.Duration = TimeSpan.FromSeconds(1.0);
      this.Orientation = Quaternion.Identity;
    }

    public bool IsFirstNode(MovementPath path)
    {
      return path.Segments[0] == this;
    }

    public void CopyFrom(PathSegment other)
    {
      this.WaitTimeOnStart = other.WaitTimeOnStart;
      this.WaitTimeOnFinish = other.WaitTimeOnFinish;
      this.Duration = other.Duration;
      this.Acceleration = other.Acceleration;
      this.Deceleration = other.Deceleration;
      this.JitterFactor = other.JitterFactor;
      this.Destination = other.Destination;
      this.Orientation = other.Orientation;
      this.CustomData = other.CustomData == null ? (ICloneable) null : other.CustomData.Clone() as ICloneable;
    }

    public PathSegment Clone()
    {
      return new PathSegment()
      {
        Acceleration = this.Acceleration,
        Deceleration = this.Deceleration,
        Destination = this.Destination,
        Duration = this.Duration,
        JitterFactor = this.JitterFactor,
        WaitTimeOnFinish = this.WaitTimeOnFinish,
        WaitTimeOnStart = this.WaitTimeOnStart,
        Orientation = this.Orientation,
        CustomData = this.CustomData == null ? (ICloneable) null : this.CustomData.Clone() as ICloneable
      };
    }
  }
}
