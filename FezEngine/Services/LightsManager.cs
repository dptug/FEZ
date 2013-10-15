// Type: FezEngine.Services.LightsManager
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using FezEngine.Structure;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace FezEngine.Services
{
  public class LightsManager : ILightsManager
  {
    private readonly List<PointLight> pointLights;
    private readonly List<DirectionalLight> directionalLights;
    private Vector3 globalAmbient;

    public IList<DirectionalLight> DirectionalLights
    {
      get
      {
        return (IList<DirectionalLight>) this.directionalLights;
      }
    }

    public IList<PointLight> PointLights
    {
      get
      {
        return (IList<PointLight>) this.pointLights;
      }
    }

    public int DirectionalLightsCount
    {
      get
      {
        return this.directionalLights.Count;
      }
    }

    public int PointLightsCount
    {
      get
      {
        return this.directionalLights.Count;
      }
    }

    public Vector3 GlobalAmbient
    {
      get
      {
        return this.globalAmbient;
      }
      set
      {
        this.globalAmbient = value;
        this.GlobalAmbientChanged();
      }
    }

    public event Action<LightEventArgs> DirectionalLightAdded = new Action<LightEventArgs>(Util.NullAction<LightEventArgs>);

    public event Action<LightEventArgs> DirectionalLightRemoved = new Action<LightEventArgs>(Util.NullAction<LightEventArgs>);

    public event Action<LightEventArgs> DirectionalLightChanged = new Action<LightEventArgs>(Util.NullAction<LightEventArgs>);

    public event Action<LightEventArgs> PointLightAdded = new Action<LightEventArgs>(Util.NullAction<LightEventArgs>);

    public event Action<LightEventArgs> PointLightChanged = new Action<LightEventArgs>(Util.NullAction<LightEventArgs>);

    public event Action<LightEventArgs> PointLightRemoved = new Action<LightEventArgs>(Util.NullAction<LightEventArgs>);

    public event Action GlobalAmbientChanged = new Action(Util.NullAction);

    public LightsManager()
    {
      this.directionalLights = new List<DirectionalLight>();
      this.pointLights = new List<PointLight>();
    }

    public DirectionalLight GetDirectionalLight(int lightNumber)
    {
      return this.directionalLights[lightNumber];
    }

    public PointLight GetPointLight(int lightNumber)
    {
      return this.pointLights[lightNumber];
    }

    public void OnDirectionalLightAdded(int newIndex)
    {
      this.DirectionalLightAdded(new LightEventArgs(newIndex));
    }

    public void OnDirectionalLightChanged(int index)
    {
      this.DirectionalLightChanged(new LightEventArgs(index));
    }

    public void OnDirectionalLightRemoved(int oldIndex)
    {
      this.DirectionalLightRemoved(new LightEventArgs(oldIndex));
    }

    public void OnPointLightAdded(int newIndex)
    {
      this.PointLightAdded(new LightEventArgs(newIndex));
    }

    public void OnPointLightChanged(int index)
    {
      this.PointLightChanged(new LightEventArgs(index));
    }

    public void OnPointLightRemoved(int oldIndex)
    {
      this.PointLightRemoved(new LightEventArgs(oldIndex));
    }
  }
}
