// Type: FezGame.Components.BitDoorState
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Services;
using FezEngine.Services.Scripting;
using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace FezGame.Components
{
  internal class BitDoorState
  {
    private static readonly TimeSpan DoorShakeTime = TimeSpan.FromSeconds(0.5);
    private static readonly TimeSpan DoorOpenTime = TimeSpan.FromSeconds(3.0);
    private readonly Vector3[] SixtyFourOffsets = new Vector3[64]
    {
      new Vector3(8f, 52f, 0.0f),
      new Vector3(12f, 52f, 0.0f),
      new Vector3(8f, 48f, 0.0f),
      new Vector3(12f, 48f, 0.0f),
      new Vector3(0.0f, 44f, 0.0f),
      new Vector3(4f, 44f, 0.0f),
      new Vector3(8f, 44f, 0.0f),
      new Vector3(12f, 44f, 0.0f),
      new Vector3(16f, 44f, 0.0f),
      new Vector3(20f, 44f, 0.0f),
      new Vector3(0.0f, 40f, 0.0f),
      new Vector3(4f, 40f, 0.0f),
      new Vector3(8f, 40f, 0.0f),
      new Vector3(12f, 40f, 0.0f),
      new Vector3(16f, 40f, 0.0f),
      new Vector3(20f, 40f, 0.0f),
      new Vector3(0.0f, 36f, 0.0f),
      new Vector3(4f, 36f, 0.0f),
      new Vector3(8f, 36f, 0.0f),
      new Vector3(12f, 36f, 0.0f),
      new Vector3(16f, 36f, 0.0f),
      new Vector3(20f, 36f, 0.0f),
      new Vector3(0.0f, 32f, 0.0f),
      new Vector3(4f, 32f, 0.0f),
      new Vector3(8f, 32f, 0.0f),
      new Vector3(12f, 32f, 0.0f),
      new Vector3(16f, 32f, 0.0f),
      new Vector3(20f, 32f, 0.0f),
      new Vector3(0.0f, 28f, 0.0f),
      new Vector3(4f, 28f, 0.0f),
      new Vector3(16f, 28f, 0.0f),
      new Vector3(20f, 28f, 0.0f),
      new Vector3(0.0f, 24f, 0.0f),
      new Vector3(4f, 24f, 0.0f),
      new Vector3(16f, 24f, 0.0f),
      new Vector3(20f, 24f, 0.0f),
      new Vector3(0.0f, 20f, 0.0f),
      new Vector3(4f, 20f, 0.0f),
      new Vector3(8f, 20f, 0.0f),
      new Vector3(12f, 20f, 0.0f),
      new Vector3(16f, 20f, 0.0f),
      new Vector3(20f, 20f, 0.0f),
      new Vector3(0.0f, 16f, 0.0f),
      new Vector3(4f, 16f, 0.0f),
      new Vector3(8f, 16f, 0.0f),
      new Vector3(12f, 16f, 0.0f),
      new Vector3(16f, 16f, 0.0f),
      new Vector3(20f, 16f, 0.0f),
      new Vector3(0.0f, 12f, 0.0f),
      new Vector3(4f, 12f, 0.0f),
      new Vector3(8f, 12f, 0.0f),
      new Vector3(12f, 12f, 0.0f),
      new Vector3(16f, 12f, 0.0f),
      new Vector3(20f, 12f, 0.0f),
      new Vector3(0.0f, 8f, 0.0f),
      new Vector3(4f, 8f, 0.0f),
      new Vector3(8f, 8f, 0.0f),
      new Vector3(12f, 8f, 0.0f),
      new Vector3(16f, 8f, 0.0f),
      new Vector3(20f, 8f, 0.0f),
      new Vector3(8f, 4f, 0.0f),
      new Vector3(12f, 4f, 0.0f),
      new Vector3(8f, 0.0f, 0.0f),
      new Vector3(12f, 0.0f, 0.0f)
    };
    private readonly Vector3[] ThirtyTwoOffsets = new Vector3[32]
    {
      new Vector3(0.0f, 2.625f, 0.0f),
      new Vector3(0.375f, 2.625f, 0.0f),
      new Vector3(0.75f, 2.625f, 0.0f),
      new Vector3(1.125f, 2.625f, 0.0f),
      new Vector3(0.0f, 2.25f, 0.0f),
      new Vector3(0.375f, 2.25f, 0.0f),
      new Vector3(0.75f, 2.25f, 0.0f),
      new Vector3(1.125f, 2.25f, 0.0f),
      new Vector3(0.0f, 1.875f, 0.0f),
      new Vector3(0.375f, 1.875f, 0.0f),
      new Vector3(0.75f, 1.875f, 0.0f),
      new Vector3(1.125f, 1.875f, 0.0f),
      new Vector3(0.0f, 1.5f, 0.0f),
      new Vector3(0.375f, 1.5f, 0.0f),
      new Vector3(0.75f, 1.5f, 0.0f),
      new Vector3(1.125f, 1.5f, 0.0f),
      new Vector3(0.0f, 1.125f, 0.0f),
      new Vector3(0.375f, 1.125f, 0.0f),
      new Vector3(0.75f, 1.125f, 0.0f),
      new Vector3(1.125f, 1.125f, 0.0f),
      new Vector3(0.0f, 0.75f, 0.0f),
      new Vector3(0.375f, 0.75f, 0.0f),
      new Vector3(0.75f, 0.75f, 0.0f),
      new Vector3(1.125f, 0.75f, 0.0f),
      new Vector3(0.0f, 0.375f, 0.0f),
      new Vector3(0.375f, 0.375f, 0.0f),
      new Vector3(0.75f, 0.375f, 0.0f),
      new Vector3(1.125f, 0.375f, 0.0f),
      new Vector3(0.0f, 0.0f, 0.0f),
      new Vector3(0.375f, 0.0f, 0.0f),
      new Vector3(0.75f, 0.0f, 0.0f),
      new Vector3(1.125f, 0.0f, 0.0f)
    };
    private readonly Vector3[] SixteenOffsets = new Vector3[16]
    {
      new Vector3(0.5f, 3f, 0.0f),
      new Vector3(0.0f, 2.5f, 0.0f),
      new Vector3(0.5f, 2.5f, 0.0f),
      new Vector3(1f, 2.5f, 0.0f),
      new Vector3(0.0f, 2f, 0.0f),
      new Vector3(0.5f, 2f, 0.0f),
      new Vector3(1f, 2f, 0.0f),
      new Vector3(0.0f, 1.5f, 0.0f),
      new Vector3(1f, 1.5f, 0.0f),
      new Vector3(0.0f, 1f, 0.0f),
      new Vector3(0.5f, 1f, 0.0f),
      new Vector3(1f, 1f, 0.0f),
      new Vector3(0.0f, 0.5f, 0.0f),
      new Vector3(0.5f, 0.5f, 0.0f),
      new Vector3(1f, 0.5f, 0.0f),
      new Vector3(0.5f, 0.0f, 0.0f)
    };
    private readonly Vector3[] EightOffsets = new Vector3[8]
    {
      new Vector3(0.5f, 1.5f, 0.0f),
      new Vector3(0.0f, 1f, 0.0f),
      new Vector3(0.5f, 1f, 0.0f),
      new Vector3(1f, 1f, 0.0f),
      new Vector3(0.0f, 0.5f, 0.0f),
      new Vector3(0.5f, 0.5f, 0.0f),
      new Vector3(1f, 0.5f, 0.0f),
      new Vector3(0.5f, 0.0f, 0.0f)
    };
    private readonly Vector3[] FourOffsets = new Vector3[4]
    {
      new Vector3(0.0f, 1.75f, 0.0f),
      new Vector3(0.0f, 1.25f, 0.0f),
      new Vector3(0.0f, 0.75f, 0.0f),
      new Vector3(0.0f, 0.25f, 0.0f)
    };
    private readonly List<BackgroundPlane> BitPlanes = new List<BackgroundPlane>();
    private readonly Viewpoint ExpectedViewpoint;
    private readonly SoundEffect RumbleSound;
    private readonly SoundEffect sLightUp;
    private readonly SoundEffect sFadeOut;
    private readonly Texture2D BitTexture;
    private readonly Texture2D AntiBitTexture;
    public readonly ArtObjectInstance AoInstance;
    private bool close;
    private bool opening;
    private TimeSpan sinceClose;
    private TimeSpan sinceMoving;
    private Vector3 doorOrigin;
    private Vector3 doorDestination;
    private SoundEmitter rumbleEmitter;
    private int lastBits;

    [ServiceDependency]
    public ILevelService LevelService { private get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { private get; set; }

    [ServiceDependency]
    public IBitDoorService BitDoorService { private get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public ILevelMaterializer LevelMaterializer { private get; set; }

    [ServiceDependency]
    public IGameLevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public IDefaultCameraManager CameraManager { private get; set; }

    [ServiceDependency]
    public IPlayerManager PlayerManager { private get; set; }

    static BitDoorState()
    {
    }

    public BitDoorState(ArtObjectInstance artObject)
    {
      ServiceHelper.InjectServices((object) this);
      this.AoInstance = artObject;
      switch (artObject.ArtObject.ActorType)
      {
        case ActorType.FourBitDoor:
        case ActorType.TwoBitDoor:
        case ActorType.SixteenBitDoor:
        case ActorType.EightBitDoor:
        case ActorType.OneBitDoor:
          this.BitTexture = this.CMProvider.Global.Load<Texture2D>("Other Textures/glow/GLOWBIT");
          this.AntiBitTexture = this.CMProvider.Global.Load<Texture2D>("Other Textures/glow/GLOWBIT_anti");
          break;
        case ActorType.ThirtyTwoBitDoor:
          this.BitTexture = this.CMProvider.Global.Load<Texture2D>("Other Textures/glow/small_glowbit");
          this.AntiBitTexture = this.CMProvider.Global.Load<Texture2D>("Other Textures/glow/small_glowbit_anti");
          break;
        default:
          this.BitTexture = this.CMProvider.Global.Load<Texture2D>("Other Textures/glow/code_machine_glowbit");
          this.AntiBitTexture = this.CMProvider.Global.Load<Texture2D>("Other Textures/glow/code_machine_glowbit_anti");
          for (int index = 0; index < 64; ++index)
            this.SixtyFourOffsets[index] /= 16f;
          break;
      }
      this.RumbleSound = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/MiscActors/Rumble");
      this.sLightUp = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Zu/DoorBitLightUp");
      this.sFadeOut = this.CMProvider.CurrentLevel.Load<SoundEffect>("Sounds/Zu/DoorBitFadeOut");
      this.ExpectedViewpoint = FezMath.AsViewpoint(FezMath.OrientationFromDirection(FezMath.MaxClamp(Vector3.Transform(Vector3.UnitZ, this.AoInstance.Rotation))));
      this.lastBits = 0;
      this.InitBitPlanes();
    }

    private void InitBitPlanes()
    {
      if (this.lastBits == this.GameState.SaveData.CubeShards + this.GameState.SaveData.SecretCubes)
        return;
      foreach (BackgroundPlane plane in this.BitPlanes)
        this.LevelManager.RemovePlane(plane);
      this.BitPlanes.Clear();
      if (this.AoInstance.Rotation == new Quaternion(0.0f, 0.0f, 0.0f, -1f))
        this.AoInstance.Rotation = Quaternion.Identity;
      int bitCount = ActorTypeExtensions.GetBitCount(this.AoInstance.ArtObject.ActorType);
      for (int index = 0; index < bitCount; ++index)
      {
        BackgroundPlane plane = new BackgroundPlane(this.LevelMaterializer.StaticPlanesMesh, index < this.GameState.SaveData.CubeShards ? (Texture) this.BitTexture : (Texture) this.AntiBitTexture)
        {
          Rotation = this.AoInstance.Rotation,
          Opacity = 0.0f,
          Fullbright = true
        };
        this.BitPlanes.Add(plane);
        this.LevelManager.AddPlane(plane);
      }
      this.lastBits = this.GameState.SaveData.CubeShards + this.GameState.SaveData.SecretCubes;
    }

    public void Update(TimeSpan elapsed)
    {
      this.DetermineIsClose();
      if (this.AoInstance.ActorSettings.Inactive)
        return;
      if (!this.opening && !this.close && this.sinceClose.TotalSeconds > 0.0)
        this.sinceClose -= elapsed;
      else if (this.close && this.sinceClose.TotalSeconds < 3.0)
        this.sinceClose += elapsed;
      this.FadeBits();
      if (this.GameState.SaveData.CubeShards + this.GameState.SaveData.SecretCubes < ActorTypeExtensions.GetBitCount(this.AoInstance.ArtObject.ActorType) || this.sinceClose.TotalSeconds <= 0.5)
        return;
      this.OpenDoor(elapsed);
    }

    private void DetermineIsClose()
    {
      this.close = false;
      if (!this.AoInstance.Visible || this.AoInstance.ActorSettings.Inactive || (this.CameraManager.Viewpoint != this.ExpectedViewpoint || this.PlayerManager.Background))
        return;
      Vector3 vector3_1 = this.AoInstance.Position + Vector3.Transform(Vector3.Transform(new Vector3(0.0f, 0.0f, 1f), this.AoInstance.Rotation), this.AoInstance.Rotation);
      Vector3 vector3_2 = FezMath.Abs(vector3_1 - this.PlayerManager.Position) * FezMath.ScreenSpaceMask(this.CameraManager.Viewpoint);
      this.close = (double) vector3_2.X + (double) vector3_2.Z < 2.0 && (double) vector3_2.Y < 2.0 && (double) FezMath.Dot(vector3_1 - this.PlayerManager.Position, FezMath.ForwardVector(this.CameraManager.Viewpoint)) >= 0.0;
    }

    public Vector3 GetOpenOffset()
    {
      switch (this.BitPlanes.Count)
      {
        case 16:
        case 1:
        case 4:
        case 8:
          return new Vector3(0.0f, 4f, 0.0f) - Vector3.Transform(new Vector3(0.0f, 0.0f, 0.125f), this.AoInstance.Rotation);
        case 32:
          return new Vector3(0.0f, -4f, 0.0f) - Vector3.Transform(new Vector3(0.0f, 0.0f, 3.0 / 16.0), this.AoInstance.Rotation);
        case 64:
          return new Vector3(0.0f, -4f, 0.0f) - Vector3.Transform(new Vector3(0.0f, 0.0f, 0.125f), this.AoInstance.Rotation);
        case 2:
          return new Vector3(0.0f, 4f, 0.0f);
        default:
          throw new InvalidOperationException();
      }
    }

    private void OpenDoor(TimeSpan elapsed)
    {
      if (!this.opening)
      {
        this.doorOrigin = this.AoInstance.Position + this.GetOpenOffset() * FezMath.XZMask;
        this.doorDestination = this.AoInstance.Position + this.GetOpenOffset();
        this.opening = true;
        this.rumbleEmitter = SoundEffectExtensions.EmitAt(this.RumbleSound, this.doorOrigin, true);
        this.LevelService.ResolvePuzzle();
      }
      this.sinceMoving += elapsed;
      this.AoInstance.Position = (!(this.sinceMoving > BitDoorState.DoorShakeTime) ? this.doorOrigin : Vector3.Lerp(this.doorOrigin, this.doorDestination, FezMath.Saturate(Easing.EaseInOut((double) (this.sinceMoving.Ticks - BitDoorState.DoorShakeTime.Ticks) / (double) BitDoorState.DoorOpenTime.Ticks, EasingType.Sine)))) + new Vector3(RandomHelper.Centered(0.0149999996647239), RandomHelper.Centered(0.0149999996647239), RandomHelper.Centered(0.0149999996647239)) * FezMath.ScreenSpaceMask(this.CameraManager.Viewpoint);
      if (!(this.sinceMoving > BitDoorState.DoorOpenTime + BitDoorState.DoorShakeTime))
        return;
      this.rumbleEmitter.FadeOutAndDie(0.25f);
      this.BitDoorService.OnOpen(this.AoInstance.Id);
      this.AoInstance.ActorSettings.Inactive = true;
      this.GameState.SaveData.ThisLevel.InactiveArtObjects.Add(this.AoInstance.Id);
      this.GameState.Save();
      this.opening = false;
    }

    private void FadeBits()
    {
      if (this.sinceClose.Ticks == 0L)
        return;
      this.InitBitPlanes();
      Vector3 position = this.AoInstance.Position;
      Vector3 vector3 = new Vector3(0.0f, 0.0f, 1f);
      for (int index = 0; index < this.BitPlanes.Count; ++index)
      {
        Vector3 vec = vector3;
        switch (this.BitPlanes.Count)
        {
          case 16:
            vec += new Vector3(-0.5f, -1.5f, 0.0f) + this.SixteenOffsets[index];
            break;
          case 32:
            vec += new Vector3(-9.0 / 16.0, -21.0 / 16.0, 0.0f) + this.ThirtyTwoOffsets[index];
            break;
          case 64:
            vec += new Vector3(-0.625f, -1.625f, 0.0f) + this.SixtyFourOffsets[index];
            break;
          case 2:
            vec += new Vector3(0.0f, (float) (0.25 - (double) index * 0.5), 0.0f);
            break;
          case 4:
            vec += new Vector3(0.0f, -1f, 0.0f) + this.FourOffsets[index];
            break;
          case 8:
            vec += new Vector3(-0.5f, -0.75f, 0.0f) + this.EightOffsets[index];
            break;
        }
        int num = this.GameState.SaveData.CubeShards + this.GameState.SaveData.SecretCubes;
        this.BitPlanes[index].Position = position + Vector3.Transform(vec, this.AoInstance.Rotation);
        float opacity = this.BitPlanes[index].Opacity;
        this.BitPlanes[index].Opacity = (float) FezMath.AsNumeric(num > index) * Easing.EaseIn(FezMath.Saturate(this.sinceClose.TotalSeconds * 2.0 - (double) index / ((double) this.BitPlanes.Count * 0.666000008583069 + 3.99600005149841)), EasingType.Sine);
        if ((double) this.BitPlanes[index].Opacity > (double) opacity && (double) opacity > 0.100000001490116 && this.BitPlanes[index].Loop)
        {
          SoundEffectExtensions.EmitAt(this.sLightUp, position);
          this.BitPlanes[index].Loop = false;
        }
        else if ((double) this.BitPlanes[index].Opacity < (double) opacity && !this.BitPlanes[index].Loop)
        {
          SoundEffectExtensions.EmitAt(this.sFadeOut, position);
          this.BitPlanes[index].Loop = true;
        }
      }
    }
  }
}
