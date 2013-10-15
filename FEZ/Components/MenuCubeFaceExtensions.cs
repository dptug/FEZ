// Type: FezGame.Components.MenuCubeFaceExtensions
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezGame.Tools;
using Microsoft.Xna.Framework;
using System;

namespace FezGame.Components
{
  internal static class MenuCubeFaceExtensions
  {
    public static Vector3 GetForward(this MenuCubeFace face)
    {
      switch (face)
      {
        case MenuCubeFace.CubeShards:
          return Vector3.UnitZ;
        case MenuCubeFace.Maps:
          return Vector3.UnitX;
        case MenuCubeFace.Artifacts:
          return -Vector3.UnitZ;
        case MenuCubeFace.AntiCubes:
          return -Vector3.UnitX;
        default:
          throw new InvalidOperationException();
      }
    }

    public static Vector3 GetRight(this MenuCubeFace face)
    {
      switch (face)
      {
        case MenuCubeFace.CubeShards:
          return Vector3.UnitX;
        case MenuCubeFace.Maps:
          return -Vector3.UnitZ;
        case MenuCubeFace.Artifacts:
          return -Vector3.UnitX;
        case MenuCubeFace.AntiCubes:
          return Vector3.UnitZ;
        default:
          throw new InvalidOperationException();
      }
    }

    public static int GetCount(this MenuCubeFace face)
    {
      switch (face)
      {
        case MenuCubeFace.CubeShards:
          return 36;
        case MenuCubeFace.Maps:
          return 9;
        case MenuCubeFace.Artifacts:
          return 4;
        case MenuCubeFace.AntiCubes:
          return 36;
        default:
          throw new InvalidOperationException();
      }
    }

    public static int GetOffset(this MenuCubeFace face)
    {
      switch (face)
      {
        case MenuCubeFace.CubeShards:
          return 34;
        case MenuCubeFace.Maps:
          return 36;
        case MenuCubeFace.Artifacts:
          return 44;
        case MenuCubeFace.AntiCubes:
          return 34;
        default:
          throw new InvalidOperationException();
      }
    }

    public static int GetSpacing(this MenuCubeFace face)
    {
      switch (face)
      {
        case MenuCubeFace.CubeShards:
          return 12;
        case MenuCubeFace.Maps:
          return 28;
        case MenuCubeFace.Artifacts:
          return 40;
        case MenuCubeFace.AntiCubes:
          return 12;
        default:
          throw new InvalidOperationException();
      }
    }

    public static int GetSize(this MenuCubeFace face)
    {
      switch (face)
      {
        case MenuCubeFace.CubeShards:
          return 8;
        case MenuCubeFace.Maps:
          return 22;
        case MenuCubeFace.Artifacts:
          return 30;
        case MenuCubeFace.AntiCubes:
          return 8;
        default:
          throw new InvalidOperationException();
      }
    }

    public static int GetDepth(this MenuCubeFace face)
    {
      switch (face)
      {
        case MenuCubeFace.CubeShards:
          return 4;
        case MenuCubeFace.Maps:
          return 16;
        case MenuCubeFace.Artifacts:
          return 16;
        case MenuCubeFace.AntiCubes:
          return 4;
        default:
          throw new InvalidOperationException();
      }
    }

    public static string GetTitle(this MenuCubeFace face)
    {
      switch (face)
      {
        case MenuCubeFace.CubeShards:
          return StaticText.GetString("MenuCube_CubeShards");
        case MenuCubeFace.Maps:
          return StaticText.GetString("MenuCube_Maps");
        case MenuCubeFace.Artifacts:
          return StaticText.GetString("MenuCube_Artifacts");
        case MenuCubeFace.AntiCubes:
          return StaticText.GetString("MenuCube_AntiCubes");
        default:
          throw new InvalidOperationException();
      }
    }
  }
}
