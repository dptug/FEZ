// Type: SharpDX.Multimedia.Speakers
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using System;

namespace SharpDX.Multimedia
{
  [Flags]
  public enum Speakers
  {
    FrontLeft = 1,
    FrontRight = 2,
    FrontCenter = 4,
    LowFrequency = 8,
    BackLeft = 16,
    BackRight = 32,
    FrontLeftOfCenter = 64,
    FrontRightOfCenter = 128,
    BackCenter = 256,
    SideLeft = 512,
    SideRight = 1024,
    TopCenter = 2048,
    TopFrontLeft = 4096,
    TopFrontCenter = 8192,
    TopFrontRight = 16384,
    TopBackLeft = 32768,
    TopBackCenter = 65536,
    TopBackRight = 131072,
    Reserved = 2147221504,
    All = -2147483648,
    Mono = FrontCenter,
    Stereo = FrontRight | FrontLeft,
    TwoPointOne = Stereo | LowFrequency,
    Surround = Stereo | Mono | BackCenter,
    Quad = Stereo | BackRight | BackLeft,
    FourPointOne = Quad | LowFrequency,
    FivePointOne = FourPointOne | Mono,
    SevenPointOne = FivePointOne | FrontRightOfCenter | FrontLeftOfCenter,
    FivePointOneSurround = TwoPointOne | Mono | SideRight | SideLeft,
    SevenPointOneSurround = FivePointOneSurround | BackRight | BackLeft,
    None = 0,
  }
}
