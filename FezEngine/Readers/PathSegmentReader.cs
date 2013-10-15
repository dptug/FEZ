// Type: FezEngine.Readers.PathSegmentReader
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Structure;
using Microsoft.Xna.Framework.Content;
using System;

namespace FezEngine.Readers
{
  public class PathSegmentReader : ContentTypeReader<PathSegment>
  {
    protected override PathSegment Read(ContentReader input, PathSegment existingInstance)
    {
      if (existingInstance == null)
        existingInstance = new PathSegment();
      existingInstance.Destination = input.ReadVector3();
      existingInstance.Duration = input.ReadObject<TimeSpan>();
      existingInstance.WaitTimeOnStart = input.ReadObject<TimeSpan>();
      existingInstance.WaitTimeOnFinish = input.ReadObject<TimeSpan>();
      existingInstance.Acceleration = input.ReadSingle();
      existingInstance.Deceleration = input.ReadSingle();
      existingInstance.JitterFactor = input.ReadSingle();
      existingInstance.Orientation = input.ReadQuaternion();
      if (input.ReadBoolean())
        existingInstance.CustomData = (ICloneable) input.ReadObject<CameraNodeData>();
      return existingInstance;
    }
  }
}
