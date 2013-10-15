// Type: Microsoft.Xna.Framework.Graphics.DirectionalLight
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework;

namespace Microsoft.Xna.Framework.Graphics
{
  public sealed class DirectionalLight
  {
    internal EffectParameter diffuseColorParameter;
    internal EffectParameter directionParameter;
    internal EffectParameter specularColorParameter;
    private Vector3 diffuseColor;
    private Vector3 direction;
    private Vector3 specularColor;
    private bool enabled;

    public Vector3 DiffuseColor
    {
      get
      {
        return this.diffuseColor;
      }
      set
      {
        this.diffuseColor = value;
        if (!this.enabled || this.diffuseColorParameter == null)
          return;
        this.diffuseColorParameter.SetValue(this.diffuseColor);
      }
    }

    public Vector3 Direction
    {
      get
      {
        return this.direction;
      }
      set
      {
        this.direction = value;
        if (this.directionParameter == null)
          return;
        this.directionParameter.SetValue(this.direction);
      }
    }

    public Vector3 SpecularColor
    {
      get
      {
        return this.specularColor;
      }
      set
      {
        this.specularColor = value;
        if (!this.enabled || this.specularColorParameter == null)
          return;
        this.specularColorParameter.SetValue(this.specularColor);
      }
    }

    public bool Enabled
    {
      get
      {
        return this.enabled;
      }
      set
      {
        if (this.enabled == value)
          return;
        this.enabled = value;
        if (this.enabled)
        {
          if (this.diffuseColorParameter != null)
            this.diffuseColorParameter.SetValue(this.diffuseColor);
          if (this.specularColorParameter == null)
            return;
          this.specularColorParameter.SetValue(this.specularColor);
        }
        else
        {
          if (this.diffuseColorParameter != null)
            this.diffuseColorParameter.SetValue(Vector3.Zero);
          if (this.specularColorParameter == null)
            return;
          this.specularColorParameter.SetValue(Vector3.Zero);
        }
      }
    }

    public DirectionalLight(EffectParameter directionParameter, EffectParameter diffuseColorParameter, EffectParameter specularColorParameter, DirectionalLight cloneSource)
    {
      this.diffuseColorParameter = diffuseColorParameter;
      this.directionParameter = directionParameter;
      this.specularColorParameter = specularColorParameter;
      if (cloneSource != null)
      {
        this.diffuseColor = cloneSource.diffuseColor;
        this.direction = cloneSource.direction;
        this.specularColor = cloneSource.specularColor;
        this.enabled = cloneSource.enabled;
      }
      else
      {
        this.diffuseColorParameter = diffuseColorParameter;
        this.directionParameter = directionParameter;
        this.specularColorParameter = specularColorParameter;
      }
    }
  }
}
