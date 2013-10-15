// Type: SharpDX.FrustumCameraParams
// Assembly: SharpDX, Version=2.4.2.0, Culture=neutral, PublicKeyToken=627a3d6d1956f55a
// MVID: 578390A1-1524-4146-8C27-2E9750400D7A
// Assembly location: F:\Program Files (x86)\FEZ\SharpDX.dll

using System;
using System.Runtime.InteropServices;

namespace SharpDX
{
  [Serializable]
  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  public struct FrustumCameraParams
  {
    public Vector3 Position;
    public Vector3 LookAtDir;
    public Vector3 UpDir;
    public float FOV;
    public float ZNear;
    public float ZFar;
    public float AspectRatio;
  }
}
