// Type: FezGame.Services.Scripting.ArtObjectService
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using Common;
using FezEngine.Components.Scripting;
using FezEngine.Services;
using FezEngine.Services.Scripting;
using FezEngine.Structure;
using FezEngine.Tools;
using FezGame.Components;
using FezGame.Services;
using FezGame.Structure;
using FezGame.Tools;
using Microsoft.Xna.Framework;
using System;

namespace FezGame.Services.Scripting
{
  public class ArtObjectService : IArtObjectService, IScriptingBase
  {
    [ServiceDependency]
    public IGameStateManager GameState { private get; set; }

    [ServiceDependency]
    public IPlayerManager PlayerManager { private get; set; }

    [ServiceDependency]
    public ISpeechBubbleManager SpeechBubble { private get; set; }

    [ServiceDependency]
    public IDefaultCameraManager CameraManager { private get; set; }

    [ServiceDependency]
    public IGameLevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public ILevelMaterializer LevelMaterializer { private get; set; }

    [ServiceDependency]
    public IContentManagerProvider CMProvider { private get; set; }

    public event Action<int> TreasureOpened = new Action<int>(Util.NullAction<int>);

    public void ResetEvents()
    {
      this.TreasureOpened = new Action<int>(Util.NullAction<int>);
    }

    public void OnTreasureOpened(int id)
    {
      this.TreasureOpened(id);
    }

    public void SetRotation(int id, float x, float y, float z)
    {
      this.LevelManager.ArtObjects[id].Rotation = Quaternion.CreateFromYawPitchRoll(MathHelper.ToRadians(y), MathHelper.ToRadians(x), MathHelper.ToRadians(z));
    }

    public void GlitchOut(int id, bool permanent, string spawnedActor)
    {
      if (!this.LevelManager.ArtObjects.ContainsKey(id))
        return;
      if (string.IsNullOrEmpty(spawnedActor))
        ServiceHelper.AddComponent((IGameComponent) new GlitchyDespawner(ServiceHelper.Game, this.LevelManager.ArtObjects[id]));
      else
        ServiceHelper.AddComponent((IGameComponent) new GlitchyDespawner(ServiceHelper.Game, this.LevelManager.ArtObjects[id], this.LevelManager.ArtObjects[id].Position)
        {
          ActorToSpawn = (ActorType) Enum.Parse(typeof (ActorType), spawnedActor, true)
        });
      if (!permanent)
        return;
      this.GameState.SaveData.ThisLevel.InactiveArtObjects.Add(-id - 1);
    }

    public LongRunningAction Move(int id, float dX, float dY, float dZ, float easeInFor, float easeOutAfter, float easeOutFor)
    {
      TrileGroup group = (TrileGroup) null;
      int? attachedGroup = this.LevelManager.ArtObjects[id].ActorSettings.AttachedGroup;
      if (attachedGroup.HasValue && this.LevelManager.Groups.ContainsKey(attachedGroup.Value))
        group = this.LevelManager.Groups[attachedGroup.Value];
      return new LongRunningAction((Func<float, float, bool>) ((elapsedSeconds, totalSeconds) =>
      {
        ArtObjectInstance local_0;
        if (!this.LevelManager.ArtObjects.TryGetValue(id, out local_0))
          return true;
        if ((double) totalSeconds < (double) easeInFor)
          elapsedSeconds *= Easing.EaseIn((double) totalSeconds / (double) easeInFor, EasingType.Quadratic);
        if ((double) easeOutFor != 0.0 && (double) totalSeconds > (double) easeOutAfter)
          elapsedSeconds *= Easing.EaseOut(1.0 - ((double) totalSeconds - (double) easeOutAfter) / (double) easeOutFor, EasingType.Quadratic);
        Vector3 local_1 = new Vector3(dX, dY, dZ) * elapsedSeconds;
        if (group != null)
        {
          foreach (TrileInstance item_0 in group.Triles)
          {
            item_0.Position += local_1;
            this.LevelManager.UpdateInstance(item_0);
          }
        }
        local_0.Position += local_1;
        return false;
      }));
    }

    public LongRunningAction TiltOnVertex(int id, float durationSeconds)
    {
      return new LongRunningAction((Func<float, float, bool>) ((_, totalSeconds) =>
      {
        ArtObjectInstance local_0;
        if (!this.LevelManager.ArtObjects.TryGetValue(id, out local_0))
          return true;
        float local_1 = Easing.EaseInOut((double) totalSeconds / (double) durationSeconds, EasingType.Sine);
        local_0.Rotation = Quaternion.CreateFromAxisAngle(Vector3.Right, (float) Math.Asin(Math.Sqrt(2.0) / Math.Sqrt(3.0)) * FezMath.Saturate(local_1)) * Quaternion.CreateFromAxisAngle(Vector3.Up, 0.7853982f * FezMath.Saturate(local_1));
        return false;
      }));
    }

    public LongRunningAction Rotate(int id, float dX, float dY, float dZ)
    {
      return new LongRunningAction((Func<float, float, bool>) ((elapsedSeconds, _) =>
      {
        ArtObjectInstance local_0;
        if (!this.LevelManager.ArtObjects.TryGetValue(id, out local_0))
          return true;
        local_0.Rotation = Quaternion.CreateFromYawPitchRoll(dY * 6.283185f * elapsedSeconds, dX * 6.283185f * elapsedSeconds, dZ * 6.283185f * elapsedSeconds) * local_0.Rotation;
        return false;
      }));
    }

    public LongRunningAction RotateIncrementally(int id, float initPitch, float initYaw, float initRoll, float secondsUntilDouble)
    {
      return new LongRunningAction((Func<float, float, bool>) ((elapsedSeconds, _) =>
      {
        ArtObjectInstance local_0;
        if (!this.LevelManager.ArtObjects.TryGetValue(id, out local_0))
          return true;
        initYaw = FezMath.DoubleIter(initYaw, elapsedSeconds, secondsUntilDouble);
        initPitch = FezMath.DoubleIter(initPitch, elapsedSeconds, secondsUntilDouble);
        initRoll = FezMath.DoubleIter(initRoll, elapsedSeconds, secondsUntilDouble);
        local_0.Rotation = Quaternion.CreateFromYawPitchRoll(initYaw, initPitch, initRoll) * local_0.Rotation;
        return false;
      }));
    }

    public LongRunningAction HoverFloat(int id, float height, float cyclesPerSecond)
    {
      float lastDelta = 0.0f;
      return new LongRunningAction((Func<float, float, bool>) ((_, sinceStarted) =>
      {
        ArtObjectInstance local_0;
        if (!this.LevelManager.ArtObjects.TryGetValue(id, out local_0))
          return true;
        float local_1 = (float) Math.Sin((double) sinceStarted * 6.28318548202515 * (double) cyclesPerSecond) * height;
        local_0.Position = new Vector3(local_0.Position.X, local_0.Position.Y - lastDelta + local_1, local_0.Position.Z);
        lastDelta = local_1;
        return false;
      }));
    }

    public LongRunningAction BeamGomez(int id)
    {
      throw new InvalidOperationException();
    }

    public LongRunningAction Pulse(int id, string textureName)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ArtObjectService.\u003C\u003Ec__DisplayClass12 cDisplayClass12 = new ArtObjectService.\u003C\u003Ec__DisplayClass12();
      // ISSUE: reference to a compiler-generated field
      cDisplayClass12.\u003C\u003E4__this = this;
      // ISSUE: reference to a compiler-generated field
      cDisplayClass12.lightPlane = new BackgroundPlane(this.LevelMaterializer.StaticPlanesMesh, textureName, false)
      {
        Position = this.LevelManager.ArtObjects[id].Position - FezMath.ForwardVector(this.CameraManager.Viewpoint) * 10f,
        Rotation = this.CameraManager.Rotation,
        AllowOverbrightness = true,
        LightMap = true,
        AlwaysOnTop = true,
        PixelatedLightmap = true
      };
      // ISSUE: reference to a compiler-generated field
      this.LevelManager.AddPlane(cDisplayClass12.lightPlane);
      // ISSUE: reference to a compiler-generated method
      return new LongRunningAction(new Func<float, float, bool>(cDisplayClass12.\u003CPulse\u003Eb__11));
    }

    public LongRunningAction Say(int id, string text, bool zuish)
    {
      this.SpeechBubble.Font = zuish ? SpeechFont.Zuish : SpeechFont.Pixel;
      this.SpeechBubble.ChangeText(zuish ? text : GameText.GetString(text));
      IPlayerManager playerManager = this.PlayerManager;
      Vector3 vector3 = playerManager.Velocity * Vector3.UnitY;
      playerManager.Velocity = vector3;
      this.PlayerManager.Action = ActionType.ReadingSign;
      return new LongRunningAction((Func<float, float, bool>) ((_, __) =>
      {
        ArtObjectInstance local_0;
        if (!this.LevelManager.ArtObjects.TryGetValue(id, out local_0))
          return true;
        this.SpeechBubble.Origin = local_0.Position + FezMath.RightVector(this.CameraManager.Viewpoint) * local_0.ArtObject.Size * 0.75f - Vector3.UnitY;
        return this.SpeechBubble.Hidden;
      }));
    }

    public LongRunningAction StartEldersSequence(int id)
    {
      ServiceHelper.AddComponent((IGameComponent) new EldersHexahedron(ServiceHelper.Game, this.LevelManager.ArtObjects[id]));
      return new LongRunningAction((Func<float, float, bool>) ((_, __) => false));
    }

    public void MoveNutToEnd(int id)
    {
      this.LevelManager.ArtObjects[id].ActorSettings.ShouldMoveToEnd = true;
    }

    public void MoveNutToHeight(int id, float height)
    {
      this.LevelManager.ArtObjects[id].ActorSettings.ShouldMoveToHeight = new float?(height);
    }
  }
}
