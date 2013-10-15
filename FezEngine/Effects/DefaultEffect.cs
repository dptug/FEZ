// Type: FezEngine.Effects.DefaultEffect
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine;
using FezEngine.Effects.Structures;
using FezEngine.Structure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FezEngine.Effects
{
  public abstract class DefaultEffect : BaseEffect
  {
    private readonly SemanticMappedBoolean alphaIsEmissive;
    private readonly SemanticMappedBoolean fullbright;
    private readonly SemanticMappedSingle emissive;
    private Vector3 lastDiffuse;

    public LightingEffectPass Pass
    {
      set
      {
        this.currentPass = this.currentTechnique.Passes[value == LightingEffectPass.Pre ? 1 : 0];
      }
    }

    public bool AlphaIsEmissive
    {
      get
      {
        return this.alphaIsEmissive.Get();
      }
      set
      {
        this.alphaIsEmissive.Set(value);
      }
    }

    public bool Fullbright
    {
      get
      {
        return this.fullbright.Get();
      }
      set
      {
        this.fullbright.Set(value);
      }
    }

    public float Emissive
    {
      get
      {
        return this.emissive.Get();
      }
      set
      {
        this.emissive.Set(value);
      }
    }

    private DefaultEffect(string effectName)
      : base(effectName)
    {
      this.alphaIsEmissive = new SemanticMappedBoolean(this.effect.Parameters, "AlphaIsEmissive");
      this.fullbright = new SemanticMappedBoolean(this.effect.Parameters, "Fullbright");
      this.emissive = new SemanticMappedSingle(this.effect.Parameters, "Emissive");
      this.Pass = LightingEffectPass.Main;
    }

    public override void Prepare(Group group)
    {
      base.Prepare(group);
      if (group.Material != null)
      {
        if (!(this.lastDiffuse != group.Material.Diffuse))
          return;
        this.material.Diffuse = group.Material.Diffuse;
        this.lastDiffuse = group.Material.Diffuse;
      }
      else
      {
        this.material.Diffuse = group.Mesh.Material.Diffuse;
        this.lastDiffuse = group.Mesh.Material.Diffuse;
      }
    }

    public class Textured : DefaultEffect
    {
      private readonly SemanticMappedTexture texture;
      private readonly SemanticMappedBoolean textureEnabled;
      private bool groupTextureDirty;

      public Textured()
        : base("DefaultEffect_Textured")
      {
        this.textureEnabled = new SemanticMappedBoolean(this.effect.Parameters, "TextureEnabled");
        this.texture = new SemanticMappedTexture(this.effect.Parameters, "BaseTexture");
      }

      public override BaseEffect Clone()
      {
        DefaultEffect.Textured textured = new DefaultEffect.Textured();
        textured.Fullbright = this.Fullbright;
        textured.Emissive = this.Emissive;
        textured.AlphaIsEmissive = this.AlphaIsEmissive;
        return (BaseEffect) textured;
      }

      public override void Prepare(Mesh mesh)
      {
        base.Prepare(mesh);
        this.textureEnabled.Set(mesh.TexturingType == TexturingType.Texture2D);
        this.texture.Set((Texture) mesh.Texture);
        this.groupTextureDirty = false;
      }

      public override void Prepare(Group group)
      {
        base.Prepare(group);
        if (group.TexturingType == TexturingType.Texture2D)
        {
          this.textureEnabled.Set(true);
          this.texture.Set(group.Texture);
          this.groupTextureDirty = true;
        }
        else
        {
          if (!this.groupTextureDirty)
            return;
          this.textureEnabled.Set(group.Mesh.TexturingType == TexturingType.Texture2D);
          this.texture.Set((Texture) group.Mesh.Texture);
        }
      }
    }

    public class VertexColored : DefaultEffect
    {
      public VertexColored()
        : base("DefaultEffect_VertexColored")
      {
      }

      public override BaseEffect Clone()
      {
        DefaultEffect.VertexColored vertexColored = new DefaultEffect.VertexColored();
        vertexColored.Fullbright = this.Fullbright;
        vertexColored.Emissive = this.Emissive;
        vertexColored.AlphaIsEmissive = this.AlphaIsEmissive;
        return (BaseEffect) vertexColored;
      }
    }

    public class LitVertexColored : DefaultEffect
    {
      private readonly SemanticMappedBoolean specularEnabled;

      public bool Specular
      {
        get
        {
          return this.specularEnabled.Get();
        }
        set
        {
          this.specularEnabled.Set(value);
        }
      }

      public LitVertexColored()
        : base("DefaultEffect_LitVertexColored")
      {
        this.specularEnabled = new SemanticMappedBoolean(this.effect.Parameters, "SpecularEnabled");
      }

      public override BaseEffect Clone()
      {
        DefaultEffect.LitVertexColored litVertexColored = new DefaultEffect.LitVertexColored();
        litVertexColored.Fullbright = this.Fullbright;
        litVertexColored.Emissive = this.Emissive;
        litVertexColored.Specular = this.Specular;
        litVertexColored.AlphaIsEmissive = this.AlphaIsEmissive;
        return (BaseEffect) litVertexColored;
      }

      public override void Prepare(Group group)
      {
        base.Prepare(group);
        if (!this.IgnoreCache && group.EffectOwner && !group.InverseTransposeWorldMatrix.Dirty)
          return;
        this.matrices.WorldInverseTranspose = (Matrix) group.InverseTransposeWorldMatrix;
        group.InverseTransposeWorldMatrix.Clean();
      }
    }

    public class LitTextured : DefaultEffect
    {
      private readonly SemanticMappedTexture texture;
      private readonly SemanticMappedBoolean textureEnabled;
      private readonly SemanticMappedBoolean specularEnabled;
      private bool groupTextureDirty;

      public bool Specular
      {
        get
        {
          return this.specularEnabled.Get();
        }
        set
        {
          this.specularEnabled.Set(value);
        }
      }

      public LitTextured()
        : base("DefaultEffect_LitTextured")
      {
        this.texture = new SemanticMappedTexture(this.effect.Parameters, "BaseTexture");
        this.textureEnabled = new SemanticMappedBoolean(this.effect.Parameters, "TextureEnabled");
        this.specularEnabled = new SemanticMappedBoolean(this.effect.Parameters, "SpecularEnabled");
      }

      public override BaseEffect Clone()
      {
        DefaultEffect.LitTextured litTextured = new DefaultEffect.LitTextured();
        litTextured.Specular = this.Specular;
        litTextured.AlphaIsEmissive = this.AlphaIsEmissive;
        litTextured.Emissive = this.Emissive;
        litTextured.Fullbright = this.Fullbright;
        return (BaseEffect) litTextured;
      }

      public override void Prepare(Mesh mesh)
      {
        base.Prepare(mesh);
        this.textureEnabled.Set(mesh.TexturingType == TexturingType.Texture2D);
        this.texture.Set((Texture) mesh.Texture);
        this.groupTextureDirty = false;
      }

      public override void Prepare(Group group)
      {
        base.Prepare(group);
        if (this.IgnoreCache || !group.EffectOwner || group.InverseTransposeWorldMatrix.Dirty)
        {
          this.matrices.WorldInverseTranspose = (Matrix) group.InverseTransposeWorldMatrix;
          group.InverseTransposeWorldMatrix.Clean();
        }
        if (group.TexturingType == TexturingType.Texture2D)
        {
          this.textureEnabled.Set(true);
          this.texture.Set(group.Texture);
          this.groupTextureDirty = true;
        }
        else
        {
          if (!this.groupTextureDirty)
            return;
          this.textureEnabled.Set(group.Mesh.TexturingType == TexturingType.Texture2D);
          this.texture.Set((Texture) group.Mesh.Texture);
        }
      }
    }

    public class TexturedVertexColored : DefaultEffect
    {
      private readonly SemanticMappedTexture texture;
      private readonly SemanticMappedBoolean textureEnabled;
      private bool groupTextureDirty;

      public TexturedVertexColored()
        : base("DefaultEffect_TexturedVertexColored")
      {
        this.texture = new SemanticMappedTexture(this.effect.Parameters, "BaseTexture");
        this.textureEnabled = new SemanticMappedBoolean(this.effect.Parameters, "TextureEnabled");
      }

      public override BaseEffect Clone()
      {
        DefaultEffect.TexturedVertexColored texturedVertexColored = new DefaultEffect.TexturedVertexColored();
        texturedVertexColored.AlphaIsEmissive = this.AlphaIsEmissive;
        texturedVertexColored.Fullbright = this.Fullbright;
        texturedVertexColored.Emissive = this.Emissive;
        return (BaseEffect) texturedVertexColored;
      }

      public override void Prepare(Mesh mesh)
      {
        base.Prepare(mesh);
        this.textureEnabled.Set(mesh.TexturingType == TexturingType.Texture2D);
        this.texture.Set((Texture) mesh.Texture);
        this.groupTextureDirty = false;
      }

      public override void Prepare(Group group)
      {
        base.Prepare(group);
        if (group.TexturingType == TexturingType.Texture2D)
        {
          this.textureEnabled.Set(true);
          this.texture.Set(group.Texture);
          this.groupTextureDirty = true;
        }
        else
        {
          if (!this.groupTextureDirty)
            return;
          this.textureEnabled.Set(group.Mesh.TexturingType == TexturingType.Texture2D);
          this.texture.Set((Texture) group.Mesh.Texture);
        }
      }
    }

    public class LitTexturedVertexColored : DefaultEffect
    {
      private readonly SemanticMappedTexture texture;
      private readonly SemanticMappedBoolean textureEnabled;
      private readonly SemanticMappedBoolean specularEnabled;
      private bool groupTextureDirty;

      public bool Specular
      {
        get
        {
          return this.specularEnabled.Get();
        }
        set
        {
          this.specularEnabled.Set(value);
        }
      }

      public LitTexturedVertexColored()
        : base("DefaultEffect_LitTexturedVertexColored")
      {
        this.texture = new SemanticMappedTexture(this.effect.Parameters, "BaseTexture");
        this.textureEnabled = new SemanticMappedBoolean(this.effect.Parameters, "TextureEnabled");
        this.specularEnabled = new SemanticMappedBoolean(this.effect.Parameters, "SpecularEnabled");
      }

      public override BaseEffect Clone()
      {
        DefaultEffect.LitTexturedVertexColored texturedVertexColored = new DefaultEffect.LitTexturedVertexColored();
        texturedVertexColored.Fullbright = this.Fullbright;
        texturedVertexColored.Emissive = this.Emissive;
        texturedVertexColored.Specular = this.Specular;
        texturedVertexColored.AlphaIsEmissive = this.AlphaIsEmissive;
        return (BaseEffect) texturedVertexColored;
      }

      public override void Prepare(Mesh mesh)
      {
        base.Prepare(mesh);
        this.textureEnabled.Set(mesh.TexturingType == TexturingType.Texture2D);
        this.texture.Set((Texture) mesh.Texture);
        this.groupTextureDirty = false;
      }

      public override void Prepare(Group group)
      {
        base.Prepare(group);
        if (this.IgnoreCache || !group.EffectOwner || group.InverseTransposeWorldMatrix.Dirty)
        {
          this.matrices.WorldInverseTranspose = (Matrix) group.InverseTransposeWorldMatrix;
          group.InverseTransposeWorldMatrix.Clean();
        }
        if (group.TexturingType == TexturingType.Texture2D)
        {
          this.textureEnabled.Set(true);
          this.texture.Set(group.Texture);
          this.groupTextureDirty = true;
        }
        else
        {
          if (!this.groupTextureDirty)
            return;
          this.textureEnabled.Set(group.Mesh.TexturingType == TexturingType.Texture2D);
          this.texture.Set((Texture) group.Mesh.Texture);
        }
      }
    }
  }
}
