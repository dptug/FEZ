// Type: FezEngine.Structure.ActorTypeExtensions
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Microsoft.Xna.Framework;

namespace FezEngine.Structure
{
  public static class ActorTypeExtensions
  {
    public static string GetArtObjectName(this ActorType type)
    {
      switch (type)
      {
        case ActorType.NumberCube:
          return "NUMBER_CUBEAO";
        case ActorType.LetterCube:
          return "LETTER_CUBEAO";
        case ActorType.TriSkull:
          return "TRI_SKULLAO";
        case ActorType.Tome:
          return "TOMEAO";
        default:
          return (string) null;
      }
    }

    public static Vector2 GetArtifactOffset(this ActorType type)
    {
      switch (type)
      {
        case ActorType.NumberCube:
        case ActorType.LetterCube:
        case ActorType.Tome:
          return new Vector2(6.5f);
        case ActorType.TriSkull:
          return new Vector2(4f, 3f);
        default:
          return Vector2.Zero;
      }
    }

    public static bool IsTreasure(this ActorType type)
    {
      switch (type)
      {
        case ActorType.TreasureMap:
        case ActorType.Mail:
        case ActorType.PieceOfHeart:
        case ActorType.CubeShard:
        case ActorType.SkeletonKey:
        case ActorType.NumberCube:
        case ActorType.LetterCube:
        case ActorType.TriSkull:
        case ActorType.Tome:
        case ActorType.SecretCube:
          return true;
        default:
          return false;
      }
    }

    public static bool IsCollectible(this ActorType type)
    {
      return type == ActorType.GoldenCube;
    }

    public static bool IsPickable(this ActorType type)
    {
      switch (type)
      {
        case ActorType.Couch:
        case ActorType.SinkPickup:
        case ActorType.PickUp:
        case ActorType.Bomb:
        case ActorType.Vase:
        case ActorType.BigBomb:
        case ActorType.TntPickup:
          return true;
        default:
          return false;
      }
    }

    public static bool IsBomb(this ActorType type)
    {
      switch (type)
      {
        case ActorType.Bomb:
        case ActorType.BigBomb:
          return true;
        default:
          return false;
      }
    }

    public static bool IsDestructible(this ActorType type)
    {
      switch (type)
      {
        case ActorType.Destructible:
        case ActorType.DestructiblePermanent:
        case ActorType.Vase:
          return true;
        default:
          return false;
      }
    }

    public static bool IsChainsploding(this ActorType type)
    {
      switch (type)
      {
        case ActorType.Bomb:
        case ActorType.BigBomb:
        case ActorType.TntBlock:
        case ActorType.TntPickup:
          return true;
        default:
          return false;
      }
    }

    public static bool IsClimbable(this ActorType type)
    {
      switch (type)
      {
        case ActorType.Ladder:
        case ActorType.Vine:
          return true;
        default:
          return false;
      }
    }

    public static bool IsFragile(this ActorType type)
    {
      return type == ActorType.Vase;
    }

    public static bool IsFaceDependant(this ActorType type)
    {
      switch (type)
      {
        case ActorType.Vine:
        case ActorType.Tombstone:
        case ActorType.UnlockedDoor:
        case ActorType.Couch:
        case ActorType.Rumbler:
        case ActorType.Ladder:
        case ActorType.Sign:
        case ActorType.Door:
          return true;
        default:
          return false;
      }
    }

    public static bool IsSafe(this ActorType type)
    {
      switch (type)
      {
        case ActorType.Crystal:
        case ActorType.Hurt:
        case ActorType.LightningPlatform:
        case ActorType.Bouncer:
        case ActorType.PickUp:
          return false;
        default:
          return true;
      }
    }

    public static bool SupportsPlanes(this ActorType type)
    {
      switch (type)
      {
        case ActorType.None:
        case ActorType.Waterfall:
        case ActorType.Trickle:
        case ActorType.Drips:
        case ActorType.BigWaterfall:
          return true;
        default:
          return false;
      }
    }

    public static bool SupportsArtObjects(this ActorType type)
    {
      switch (type)
      {
        case ActorType.None:
        case ActorType.Checkpoint:
        case ActorType.TreasureChest:
        case ActorType.EightBitDoor:
        case ActorType.WarpGate:
        case ActorType.OneBitDoor:
        case ActorType.SpinBlock:
        case ActorType.PivotHandle:
        case ActorType.FourBitDoor:
        case ActorType.Tombstone:
        case ActorType.SplitUpCube:
        case ActorType.Valve:
        case ActorType.Rumbler:
        case ActorType.ConnectiveRail:
        case ActorType.BoltHandle:
        case ActorType.BoltNutBottom:
        case ActorType.BoltNutTop:
        case ActorType.CodeMachine:
        case ActorType.NumberCube:
        case ActorType.LetterCube:
        case ActorType.TriSkull:
        case ActorType.Tome:
        case ActorType.LesserGate:
        case ActorType.LaserEmitter:
        case ActorType.LaserBender:
        case ActorType.LaserReceiver:
        case ActorType.RebuildingHexahedron:
        case ActorType.TreasureMap:
        case ActorType.Timeswitch:
        case ActorType.TimeswitchMovingPart:
        case ActorType.Mailbox:
        case ActorType.Bookcase:
        case ActorType.TwoBitDoor:
        case ActorType.SixteenBitDoor:
        case ActorType.ThirtyTwoBitDoor:
        case ActorType.SixtyFourBitDoor:
        case ActorType.Bell:
        case ActorType.Telescope:
        case ActorType.QrCode:
        case ActorType.FpsPost:
        case ActorType.SecretPassage:
          return true;
        default:
          return false;
      }
    }

    public static bool SupportsGroups(this ActorType type)
    {
      switch (type)
      {
        case ActorType.Geyser:
        case ActorType.RotatingGroup:
        case ActorType.Piston:
        case ActorType.None:
        case ActorType.ExploSwitch:
        case ActorType.PushSwitch:
        case ActorType.PushSwitchSticky:
        case ActorType.PushSwitchPermanent:
        case ActorType.SuckBlock:
          return true;
        default:
          return false;
      }
    }

    public static bool SupportsNPCs(this ActorType type)
    {
      switch (type)
      {
        case ActorType.None:
        case ActorType.LightningGhost:
        case ActorType.Owl:
          return true;
        default:
          return false;
      }
    }

    public static bool SupportsTriles(this ActorType type)
    {
      if (type == ActorType.None)
        return true;
      if (!ActorTypeExtensions.SupportsArtObjects(type) && !ActorTypeExtensions.SupportsGroups(type))
        return !ActorTypeExtensions.SupportsNPCs(type) & !ActorTypeExtensions.SupportsPlanes(type);
      else
        return false;
    }

    public static bool IsLight(this ActorType type)
    {
      switch (type)
      {
        case ActorType.Bomb:
        case ActorType.Vase:
          return true;
        default:
          return false;
      }
    }

    public static bool IsPushSwitch(this ActorType type)
    {
      switch (type)
      {
        case ActorType.PushSwitch:
        case ActorType.PushSwitchSticky:
        case ActorType.PushSwitchPermanent:
          return true;
        default:
          return false;
      }
    }

    public static bool IsBitDoor(this ActorType type)
    {
      switch (type)
      {
        case ActorType.FourBitDoor:
        case ActorType.TwoBitDoor:
        case ActorType.SixteenBitDoor:
        case ActorType.ThirtyTwoBitDoor:
        case ActorType.SixtyFourBitDoor:
        case ActorType.EightBitDoor:
        case ActorType.OneBitDoor:
          return true;
        default:
          return false;
      }
    }

    public static int GetBitCount(this ActorType type)
    {
      switch (type)
      {
        case ActorType.FourBitDoor:
          return 4;
        case ActorType.TwoBitDoor:
          return 2;
        case ActorType.SixteenBitDoor:
          return 16;
        case ActorType.ThirtyTwoBitDoor:
          return 32;
        case ActorType.SixtyFourBitDoor:
          return 64;
        case ActorType.EightBitDoor:
          return 8;
        case ActorType.OneBitDoor:
          return 1;
        default:
          return 0;
      }
    }

    public static bool IsHeavy(this ActorType type)
    {
      if (ActorTypeExtensions.IsPickable(type))
        return !ActorTypeExtensions.IsLight(type);
      else
        return false;
    }

    public static bool IsBuoyant(this ActorType type)
    {
      return type == ActorType.PickUp;
    }

    public static bool IsDoor(this ActorType type)
    {
      switch (type)
      {
        case ActorType.Door:
        case ActorType.UnlockedDoor:
          return true;
        default:
          return false;
      }
    }

    public static bool IsCubeShard(this ActorType type)
    {
      switch (type)
      {
        case ActorType.CubeShard:
        case ActorType.SecretCube:
        case ActorType.PieceOfHeart:
          return true;
        default:
          return false;
      }
    }

    public static bool UsesLasers(this ActorType type)
    {
      switch (type)
      {
        case ActorType.LaserEmitter:
        case ActorType.LaserBender:
        case ActorType.LaserReceiver:
          return true;
        default:
          return false;
      }
    }
  }
}
