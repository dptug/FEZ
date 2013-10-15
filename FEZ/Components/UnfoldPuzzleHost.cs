// Type: FezGame.Components.UnfoldPuzzleHost
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Services;
using FezEngine.Services.Scripting;
using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Services;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezGame.Components
{
  internal class UnfoldPuzzleHost : GameComponent
  {
    private static readonly Vector3 PuzzleCenter = new Vector3(9f, 57.5f, 11f);
    private static readonly Vector3[] Slots1 = new Vector3[6]
    {
      new Vector3(-2f, 1f, 0.0f),
      new Vector3(-1f, 1f, 0.0f),
      new Vector3(0.0f, 1f, 0.0f),
      new Vector3(0.0f, 0.0f, 0.0f),
      new Vector3(1f, 0.0f, 0.0f),
      new Vector3(2f, 0.0f, 0.0f)
    };
    private static readonly Vector3[] Slots2 = new Vector3[6]
    {
      new Vector3(-3f, 1f, 0.0f),
      new Vector3(-2f, 1f, 0.0f),
      new Vector3(-1f, 1f, 0.0f),
      new Vector3(-1f, 0.0f, 0.0f),
      new Vector3(0.0f, 0.0f, 0.0f),
      new Vector3(1f, 0.0f, 0.0f)
    };
    private static readonly Vector3[] Slots3 = new Vector3[6]
    {
      new Vector3(-3f, 1f, 0.0f),
      new Vector3(-2f, 1f, 0.0f),
      new Vector3(-1f, 1f, 0.0f),
      new Vector3(-5f, 0.0f, 0.0f),
      new Vector3(-4f, 0.0f, 0.0f),
      new Vector3(-3f, 0.0f, 0.0f)
    };
    private static readonly Vector3[] Slots4 = new Vector3[6]
    {
      new Vector3(-4f, 1f, 0.0f),
      new Vector3(-3f, 1f, 0.0f),
      new Vector3(-2f, 1f, 0.0f),
      new Vector3(-6f, 0.0f, 0.0f),
      new Vector3(-5f, 0.0f, 0.0f),
      new Vector3(-4f, 0.0f, 0.0f)
    };
    private int RightPositions1;
    private int RightPositions2;
    private int RightPositions3;
    private int RightPositions4;
    private List<TrileInstance> Blocks;

    [ServiceDependency]
    public IGroupService GroupService { get; set; }

    [ServiceDependency]
    public ILevelService LevelService { get; set; }

    [ServiceDependency]
    public ILevelMaterializer LevelMaterializer { get; set; }

    [ServiceDependency]
    public IPlayerManager PlayerManager { get; set; }

    [ServiceDependency]
    public IGameStateManager GameState { get; set; }

    [ServiceDependency]
    public IGameCameraManager CameraManager { get; set; }

    [ServiceDependency]
    public IGameLevelManager LevelManager { get; set; }

    static UnfoldPuzzleHost()
    {
    }

    public UnfoldPuzzleHost(Game game)
      : base(game)
    {
    }

    public override void Initialize()
    {
      base.Initialize();
      this.LevelManager.LevelChanged += new Action(this.TryInitialize);
      this.TryInitialize();
    }

    private void TryInitialize()
    {
      this.Blocks = (List<TrileInstance>) null;
      if (this.LevelManager.Name == "ZU_UNFOLD")
      {
        if (this.GameState.SaveData.ThisLevel.InactiveArtObjects.Contains(0))
        {
          foreach (TrileInstance instance in Enumerable.ToArray<TrileInstance>(Enumerable.Where<TrileInstance>((IEnumerable<TrileInstance>) this.LevelManager.Triles.Values, (Func<TrileInstance, bool>) (x => x.Trile.ActorSettings.Type == ActorType.SinkPickup))))
          {
            instance.PhysicsState = (InstancePhysicsState) null;
            this.LevelManager.ClearTrile(instance);
          }
          if (!this.GameState.SaveData.ThisLevel.DestroyedTriles.Contains(new TrileEmplacement(UnfoldPuzzleHost.PuzzleCenter + Vector3.UnitY * 2f + Vector3.UnitZ - FezMath.HalfVector)))
          {
            Trile trile = Enumerable.FirstOrDefault<Trile>(this.LevelManager.ActorTriles(ActorType.SecretCube));
            if (trile != null)
            {
              Vector3 position = UnfoldPuzzleHost.PuzzleCenter + Vector3.UnitY * 2f - FezMath.HalfVector + Vector3.UnitZ;
              this.LevelManager.ClearTrile(new TrileEmplacement(position));
              TrileInstance toAdd;
              this.LevelManager.RestoreTrile(toAdd = new TrileInstance(position, trile.Id)
              {
                OriginalEmplacement = new TrileEmplacement(position)
              });
              if (toAdd.InstanceId == -1)
                this.LevelMaterializer.CullInstanceIn(toAdd);
            }
          }
          this.Enabled = false;
        }
        else
          this.Enabled = true;
      }
      else
        this.Enabled = false;
      if (!this.Enabled)
        return;
      this.Blocks = new List<TrileInstance>();
      this.Blocks.AddRange(Enumerable.Where<TrileInstance>((IEnumerable<TrileInstance>) this.LevelManager.Triles.Values, (Func<TrileInstance, bool>) (x => x.Trile.ActorSettings.Type == ActorType.SinkPickup)));
    }

    public override void Update(GameTime gameTime)
    {
      if (this.GameState.Paused || this.GameState.InMap || (!this.CameraManager.ActionRunning || !FezMath.IsOrthographic(this.CameraManager.Viewpoint)) || this.GameState.Loading)
        return;
      Vector3 vector3_1 = FezMath.ScreenSpaceMask(this.CameraManager.Viewpoint);
      Vector3 b = FezMath.SideMask(this.CameraManager.Viewpoint);
      if (this.RightPositions1 == 6 || this.RightPositions2 == 6 || (this.RightPositions3 == 6 || this.RightPositions4 == 6))
      {
        bool flag = false;
        foreach (TrileInstance instance in this.Blocks)
        {
          if (!flag)
          {
            ServiceHelper.AddComponent((IGameComponent) new GlitchyDespawner(this.Game, instance, UnfoldPuzzleHost.PuzzleCenter + Vector3.UnitY * 2f + Vector3.UnitZ)
            {
              FlashOnSpawn = true
            });
            flag = true;
          }
          else
            ServiceHelper.AddComponent((IGameComponent) new GlitchyDespawner(this.Game, instance));
        }
        this.GameState.SaveData.ThisLevel.InactiveArtObjects.Add(0);
        foreach (Volume volume in Enumerable.Where<Volume>((IEnumerable<Volume>) this.LevelManager.Volumes.Values, (Func<Volume, bool>) (x =>
        {
          if (x.ActorSettings != null && x.ActorSettings.IsPointOfInterest)
            return x.Enabled;
          else
            return false;
        })))
        {
          volume.Enabled = false;
          this.GameState.SaveData.ThisLevel.InactiveVolumes.Add(volume.Id);
        }
        this.LevelService.ResolvePuzzle();
        this.Enabled = false;
      }
      else
      {
        if (FezMath.ForwardVector(this.CameraManager.Viewpoint) != -Vector3.UnitZ)
          return;
        this.RightPositions1 = this.RightPositions2 = this.RightPositions3 = this.RightPositions4 = 0;
        foreach (TrileInstance instance in this.Blocks)
        {
          if (!instance.PhysicsState.Background && instance.Enabled && (double) instance.Position.Y >= 57.0 && ((double) instance.Position.Z > 9.0 || (double) instance.Center.X >= 11.0 || (double) instance.Center.X <= 6.0 && (double) instance.Center.Y > 57.0 || (double) instance.Center.X <= 5.0 && (double) instance.Center.Y == 56.5))
          {
            if (this.PlayerManager.PushedInstance == instance && instance.PhysicsState.WallCollision.First.Collided && (double) Math.Abs(FezMath.Dot(instance.PhysicsState.WallCollision.First.Destination.Center - instance.Center, Vector3.UnitY)) > 15.0 / 16.0)
            {
              instance.PhysicsState.WallCollision = new MultipleHits<CollisionResult>();
              instance.PhysicsState.Center += (float) FezMath.Sign(this.PlayerManager.LookingDirection) * FezMath.RightVector(this.CameraManager.Viewpoint) * 0.01f;
            }
            if (this.PlayerManager.HeldInstance != instance && this.PlayerManager.PushedInstance != instance && instance.PhysicsState.Grounded && instance.PhysicsState.Ground.First != this.PlayerManager.PushedInstance)
            {
              Vector3 vector3_2 = Vector3.Min(Vector3.Max(FezMath.Round((instance.Center - UnfoldPuzzleHost.PuzzleCenter) / 1f), new Vector3(-7f, 0.0f, 0.0f)), new Vector3(7f, 1f, 0.0f));
              Vector3 vector3_3 = UnfoldPuzzleHost.PuzzleCenter + vector3_2 * 1f;
              if ((double) vector3_2.Y == 0.0)
                vector3_3 += Vector3.UnitZ;
              Vector3 vector3_4 = (vector3_3 - instance.Center) * vector3_1;
              instance.PhysicsState.Velocity += 0.25f * (float) Math.Sign(vector3_4.X) * Vector3.UnitX * (float) gameTime.ElapsedGameTime.TotalSeconds;
              if ((double) Math.Abs(vector3_4.X) <= 1.0 / 64.0 && (double) Math.Abs(vector3_4.Y) <= 1.0 / 64.0)
              {
                instance.PhysicsState.Velocity = Vector3.Zero;
                instance.PhysicsState.Center = vector3_3;
                instance.PhysicsState.UpdateInstance();
                this.LevelManager.UpdateInstance(instance);
              }
              if ((instance.PhysicsState.Ground.NearLow == null || (double) Math.Abs(FezMath.Dot(instance.PhysicsState.Ground.NearLow.Center - instance.Center, b)) > 0.875) && (instance.PhysicsState.Ground.FarHigh == null || (double) Math.Abs(FezMath.Dot(instance.PhysicsState.Ground.FarHigh.Center - instance.Center, b)) > 0.875))
              {
                instance.PhysicsState.Ground = new MultipleHits<TrileInstance>();
                instance.PhysicsState.Center += Vector3.Down * 0.1f;
              }
              if (instance.PhysicsState.Grounded && instance.PhysicsState.Velocity == Vector3.Zero)
              {
                for (int index = 0; index < UnfoldPuzzleHost.Slots1.Length; ++index)
                {
                  if ((UnfoldPuzzleHost.PuzzleCenter + UnfoldPuzzleHost.Slots1[index]) * vector3_1 == instance.Center * vector3_1)
                  {
                    ++this.RightPositions1;
                    break;
                  }
                }
                for (int index = 0; index < UnfoldPuzzleHost.Slots2.Length; ++index)
                {
                  if ((UnfoldPuzzleHost.PuzzleCenter + UnfoldPuzzleHost.Slots2[index]) * vector3_1 == instance.Center * vector3_1)
                  {
                    ++this.RightPositions2;
                    break;
                  }
                }
                for (int index = 0; index < UnfoldPuzzleHost.Slots3.Length; ++index)
                {
                  if ((UnfoldPuzzleHost.PuzzleCenter + UnfoldPuzzleHost.Slots3[index]) * vector3_1 == instance.Center * vector3_1)
                  {
                    ++this.RightPositions3;
                    break;
                  }
                }
                for (int index = 0; index < UnfoldPuzzleHost.Slots4.Length; ++index)
                {
                  if ((UnfoldPuzzleHost.PuzzleCenter + UnfoldPuzzleHost.Slots4[index]) * vector3_1 == instance.Center * vector3_1)
                  {
                    ++this.RightPositions4;
                    break;
                  }
                }
              }
            }
          }
        }
      }
    }
  }
}
