// Type: FezGame.Components.WarpPanel
// Assembly: FEZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9D78BCDD-808F-47ED-B61F-DABBAB0FB594
// Assembly location: F:\Program Files (x86)\FEZ\FEZ.exe

using FezEngine;
using FezEngine.Structure;
using System;

namespace FezGame.Components
{
  public class WarpPanel
  {
    public string Destination;
    public Mesh PanelMask;
    public Mesh Layers;
    public FaceOrientation Face;
    public TimeSpan Timer;
    public bool Enabled;
  }
}
