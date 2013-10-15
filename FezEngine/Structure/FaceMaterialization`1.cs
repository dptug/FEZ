// Type: FezEngine.Structure.FaceMaterialization`1
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine;
using FezEngine.Structure.Geometry;
using System;

namespace FezEngine.Structure
{
  public struct FaceMaterialization<T> where T : struct, IEquatable<T>, IVertex
  {
    private SharedVertex<T> i0;
    private SharedVertex<T> i1;
    private SharedVertex<T> i2;
    private SharedVertex<T> i3;
    private SharedVertex<T> i4;
    private SharedVertex<T> i5;

    public SharedVertex<T> V0 { get; set; }

    public SharedVertex<T> V1 { get; set; }

    public SharedVertex<T> V2 { get; set; }

    public SharedVertex<T> V3 { get; set; }

    public int GetIndex(ushort relativeIndex)
    {
      switch (relativeIndex)
      {
        case (ushort) 0:
          return this.i0.Index;
        case (ushort) 1:
          return this.i1.Index;
        case (ushort) 2:
          return this.i2.Index;
        case (ushort) 3:
          return this.i3.Index;
        case (ushort) 4:
          return this.i4.Index;
        default:
          return this.i5.Index;
      }
    }

    public void SetupIndices(FaceOrientation face)
    {
      if (face == FaceOrientation.Front || face == FaceOrientation.Top || face == FaceOrientation.Right)
      {
        this.i0 = this.V0;
        this.i1 = this.V1;
        this.i2 = this.V2;
        this.i3 = this.V0;
        this.i4 = this.V2;
        this.i5 = this.V3;
      }
      else
      {
        this.i0 = this.V0;
        this.i1 = this.V2;
        this.i2 = this.V1;
        this.i3 = this.V0;
        this.i4 = this.V3;
        this.i5 = this.V2;
      }
    }
  }
}
