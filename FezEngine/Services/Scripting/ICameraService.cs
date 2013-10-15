// Type: FezEngine.Services.Scripting.ICameraService
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using FezEngine.Components.Scripting;
using FezEngine.Structure.Scripting;
using System;

namespace FezEngine.Services.Scripting
{
  [Entity(Static = true)]
  public interface ICameraService : IScriptingBase
  {
    [Description("When the viewpoint changed")]
    event Action Rotated;

    void OnRotate();

    [Description("Set the number of pixels per trixel (default is 4)")]
    void SetPixelsPerTrixel(int triles);

    [Description("Changes whether Gomez can rotate the camera")]
    void SetCanRotate(bool canRotate);

    [Description("Forces camera rotation, left (-1 to -3) or right (1 to 3)")]
    void Rotate(int distance);

    [Description("Rotates to any view orientation (Left, Right, Front, Back)")]
    void RotateTo(string viewName);

    [Description("Fades to the chosen color (Black, White, etc.)")]
    LongRunningAction FadeTo(string colorName);

    [Description("Fades from the chosen color (Black, White, etc.)")]
    LongRunningAction FadeFrom(string colorName);

    [Description("Flashes the chosen color once (Black, White, etc.)")]
    void Flash(string colorName);

    [Description("Shakes the camera and vibrates controller")]
    void Shake(float distance, float durationSeconds);

    [Description("Sets the camera offset as descending (true) or ascending (false)")]
    void SetDescending(bool descending);

    [Description("Remove the constraints (volume focus etc.)")]
    void Unconstrain();
  }
}
