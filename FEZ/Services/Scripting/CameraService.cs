// Type: FezGame.Services.Scripting.CameraService
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using Common;
using FezEngine;
using FezEngine.Components;
using FezEngine.Components.Scripting;
using FezEngine.Services;
using FezEngine.Services.Scripting;
using FezEngine.Tools;
using FezGame.Services;
using Microsoft.Xna.Framework;
using System;

namespace FezGame.Services.Scripting
{
  public class CameraService : ICameraService, IScriptingBase
  {
    [ServiceDependency]
    public ILevelManager LevelManager { private get; set; }

    [ServiceDependency]
    public IGameCameraManager CameraManager { private get; set; }

    [ServiceDependency]
    public IPlayerManager PlayerManager { private get; set; }

    [ServiceDependency]
    public ITargetRenderingManager TargetRenderer { private get; set; }

    [ServiceDependency]
    public IEngineStateManager EngineState { private get; set; }

    public event Action Rotated = new Action(Util.NullAction);

    public void ResetEvents()
    {
      this.Rotated = new Action(Util.NullAction);
    }

    public void OnRotate()
    {
      this.Rotated();
    }

    public void SetPixelsPerTrixel(int pixelsPerTrixel)
    {
      if (this.EngineState.FarawaySettings.InTransition)
        return;
      this.CameraManager.PixelsPerTrixel = (float) pixelsPerTrixel;
    }

    public void SetCanRotate(bool canRotate)
    {
      this.PlayerManager.CanRotate = canRotate;
    }

    public void Rotate(int distance)
    {
      this.CameraManager.ChangeViewpoint(FezMath.GetRotatedView(this.CameraManager.Viewpoint, distance));
    }

    public void RotateTo(string viewName)
    {
      Viewpoint view = (Viewpoint) Enum.Parse(typeof (Viewpoint), viewName, true);
      if (view == this.CameraManager.Viewpoint)
        return;
      this.CameraManager.ChangeViewpoint(view);
    }

    public LongRunningAction FadeTo(string colorName)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CameraService.\u003C\u003Ec__DisplayClass2 cDisplayClass2 = new CameraService.\u003C\u003Ec__DisplayClass2();
      Color color = Util.FromName(colorName);
      // ISSUE: reference to a compiler-generated field
      cDisplayClass2.component = new ScreenFade(ServiceHelper.Game)
      {
        FromColor = new Color(new Vector4(color.ToVector3(), 0.0f)),
        ToColor = color,
        Duration = 2f
      };
      // ISSUE: reference to a compiler-generated field
      ServiceHelper.AddComponent((IGameComponent) cDisplayClass2.component);
      // ISSUE: reference to a compiler-generated method
      return new LongRunningAction(new Func<float, float, bool>(cDisplayClass2.\u003CFadeTo\u003Eb__1));
    }

    public LongRunningAction FadeFrom(string colorName)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CameraService.\u003C\u003Ec__DisplayClass6 cDisplayClass6 = new CameraService.\u003C\u003Ec__DisplayClass6();
      Color color = Util.FromName(colorName);
      // ISSUE: reference to a compiler-generated field
      cDisplayClass6.component = new ScreenFade(ServiceHelper.Game)
      {
        FromColor = color,
        ToColor = new Color(new Vector4(color.ToVector3(), 0.0f)),
        Duration = 2f
      };
      // ISSUE: reference to a compiler-generated field
      ServiceHelper.AddComponent((IGameComponent) cDisplayClass6.component);
      // ISSUE: reference to a compiler-generated method
      return new LongRunningAction(new Func<float, float, bool>(cDisplayClass6.\u003CFadeFrom\u003Eb__5));
    }

    public void Flash(string colorName)
    {
      Color color = Util.FromName(colorName);
      ServiceHelper.AddComponent((IGameComponent) new ScreenFade(ServiceHelper.Game)
      {
        FromColor = color,
        ToColor = new Color(new Vector4(color.ToVector3(), 0.0f)),
        Duration = 0.1f
      });
    }

    public void Shake(float distance, float durationSeconds)
    {
      ServiceHelper.AddComponent((IGameComponent) new CamShake(ServiceHelper.Game)
      {
        Duration = TimeSpan.FromSeconds((double) durationSeconds),
        Distance = distance
      });
    }

    public void SetDescending(bool descending)
    {
      this.LevelManager.Descending = descending;
    }

    public void Unconstrain()
    {
      this.CameraManager.Constrained = false;
    }
  }
}
