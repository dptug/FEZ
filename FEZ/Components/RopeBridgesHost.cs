// Type: FezGame.Components.RopeBridgesHost
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine.Services;
using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Services;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace FezGame.Components
{
  public class RopeBridgesHost : GameComponent
  {
    private readonly Dictionary<TrileInstance, BridgeState> ActiveBridgeParts = new Dictionary<TrileInstance, BridgeState>();
    private const float Downforce = 0.1f;

    [ServiceDependency]
    public IPlayerManager PlayerManager { private get; set; }

    [ServiceDependency]
    public ILevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public IDefaultCameraManager CameraManager { private get; set; }

    public RopeBridgesHost(Game game)
      : base(game)
    {
      this.UpdateOrder = -2;
    }

    public override void Initialize()
    {
      this.LevelManager.LevelChanged += (Action) (() => this.ActiveBridgeParts.Clear());
    }

    public override void Update(GameTime gameTime)
    {
      if (this.PlayerManager.Grounded)
      {
        this.AddDownforce(this.PlayerManager.Ground.NearLow, 0.1f, true, false);
        this.AddDownforce(this.PlayerManager.Ground.FarHigh, 0.1f, true, false);
        this.AddDownforce(this.PlayerManager.Ground.NearLow, 0.1f, false, true);
        this.AddDownforce(this.PlayerManager.Ground.FarHigh, 0.1f, false, true);
      }
      foreach (BridgeState bridgeState in this.ActiveBridgeParts.Values)
      {
        bridgeState.Downforce *= 0.8f;
        bridgeState.Dirty = false;
      }
      foreach (BridgeState bridgeState in this.ActiveBridgeParts.Values)
      {
        Vector3 vector3 = new Vector3(bridgeState.Instance.Position.X, bridgeState.OriginalPosition.Y - bridgeState.Downforce, bridgeState.Instance.Position.Z);
        bridgeState.Instance.PhysicsState.Velocity = vector3 - bridgeState.Instance.Position;
        bridgeState.Instance.Position = vector3;
        this.LevelManager.UpdateInstance(bridgeState.Instance);
      }
    }

    private void AddDownforce(TrileInstance instance, float factor, bool apply, bool propagate)
    {
      if (instance == null || instance.TrileId != 286)
        return;
      BridgeState bridgeState;
      if (!this.ActiveBridgeParts.TryGetValue(instance, out bridgeState))
        this.ActiveBridgeParts.Add(instance, bridgeState = new BridgeState(instance));
      else if (apply && bridgeState.Dirty)
        return;
      Vector3 vector3 = FezMath.SideMask(this.CameraManager.Viewpoint);
      if (apply)
      {
        bridgeState.Downforce = MathHelper.Clamp(bridgeState.Downforce + factor, 0.0f, 1f);
        bridgeState.Dirty = true;
      }
      if (!propagate)
        return;
      TrileEmplacement id = new TrileEmplacement(bridgeState.OriginalPosition - vector3);
      this.AddDownforce(this.LevelManager.TrileInstanceAt(ref id), factor / 2f, true, true);
      id = new TrileEmplacement(bridgeState.OriginalPosition + vector3);
      this.AddDownforce(this.LevelManager.TrileInstanceAt(ref id), factor / 2f, true, true);
    }
  }
}
