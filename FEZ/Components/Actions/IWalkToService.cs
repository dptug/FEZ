// Type: FezGame.Components.Actions.IWalkToService
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezGame.Structure;
using Microsoft.Xna.Framework;
using System;

namespace FezGame.Components.Actions
{
  public interface IWalkToService
  {
    Func<Vector3> Destination { get; set; }

    ActionType NextAction { get; set; }
  }
}
