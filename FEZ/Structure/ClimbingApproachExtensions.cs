// Type: FezGame.Structure.ClimbingApproachExtensions
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;

namespace FezGame.Structure
{
  public static class ClimbingApproachExtensions
  {
    public static HorizontalDirection AsDirection(this ClimbingApproach approach)
    {
      if (approach == ClimbingApproach.Left)
        return HorizontalDirection.Left;
      return approach == ClimbingApproach.Right ? HorizontalDirection.Right : HorizontalDirection.Right;
    }
  }
}
