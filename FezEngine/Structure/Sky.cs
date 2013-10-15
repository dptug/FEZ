// Type: FezEngine.Structure.Sky
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using ContentSerialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezEngine.Structure
{
  public class Sky
  {
    public string Name { get; set; }

    public float WindSpeed { get; set; }

    public float Density { get; set; }

    public float FogDensity { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public List<string> Clouds { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public List<SkyLayer> Layers { get; set; }

    public string Background { get; set; }

    [Serialization(Optional = true)]
    public string Shadows { get; set; }

    [Serialization(Optional = true)]
    public string Stars { get; set; }

    [Serialization(Optional = true)]
    public string CloudTint { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public bool VerticalTiling { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public bool HorizontalScrolling { get; set; }

    [Serialization(Optional = true)]
    public float InterLayerVerticalDistance { get; set; }

    [Serialization(Optional = true)]
    public float InterLayerHorizontalDistance { get; set; }

    [Serialization(Optional = true)]
    public float HorizontalDistance { get; set; }

    [Serialization(Optional = true)]
    public float VerticalDistance { get; set; }

    [Serialization(Optional = true)]
    public float LayerBaseHeight { get; set; }

    [Serialization(Optional = true)]
    public float LayerBaseSpacing { get; set; }

    [Serialization(Optional = true)]
    public float WindParallax { get; set; }

    [Serialization(Optional = true)]
    public float WindDistance { get; set; }

    [Serialization(Optional = true)]
    public float CloudsParallax { get; set; }

    [Serialization(Optional = true)]
    public float ShadowOpacity { get; set; }

    [Serialization(Optional = true)]
    public bool FoliageShadows { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public bool NoPerFaceLayerXOffset { get; set; }

    [Serialization(DefaultValueOptional = true, Optional = true)]
    public float LayerBaseXOffset { get; set; }

    public Sky()
    {
      this.Name = "Default";
      this.Background = "SkyBack";
      this.Clouds = new List<string>();
      this.Layers = new List<SkyLayer>();
      this.WindSpeed = 1f;
      this.Density = 1f;
      this.FogDensity = 0.02f;
      this.LayerBaseHeight = 0.5f;
      this.CloudsParallax = 1f;
      this.ShadowOpacity = 0.7f;
    }

    public Sky ShallowCopy()
    {
      return new Sky()
      {
        Name = this.Name,
        Background = this.Background,
        Clouds = new List<string>((IEnumerable<string>) this.Clouds),
        Layers = Enumerable.ToList<SkyLayer>(Enumerable.Select<SkyLayer, SkyLayer>((IEnumerable<SkyLayer>) this.Layers, (Func<SkyLayer, SkyLayer>) (x => x.ShallowCopy()))),
        WindSpeed = this.WindSpeed,
        Density = this.Density,
        Shadows = this.Shadows,
        Stars = this.Stars,
        FogDensity = this.FogDensity,
        VerticalTiling = this.VerticalTiling,
        HorizontalScrolling = this.HorizontalScrolling,
        LayerBaseHeight = this.LayerBaseHeight,
        InterLayerVerticalDistance = this.InterLayerVerticalDistance,
        InterLayerHorizontalDistance = this.InterLayerHorizontalDistance,
        HorizontalDistance = this.HorizontalDistance,
        VerticalDistance = this.VerticalDistance,
        LayerBaseSpacing = this.LayerBaseSpacing,
        WindParallax = this.WindParallax,
        WindDistance = this.WindDistance,
        CloudsParallax = this.CloudsParallax,
        FoliageShadows = this.FoliageShadows,
        ShadowOpacity = this.ShadowOpacity,
        NoPerFaceLayerXOffset = this.NoPerFaceLayerXOffset,
        LayerBaseXOffset = this.LayerBaseXOffset
      };
    }

    public void UpdateFromCopy(Sky copy)
    {
      this.Name = copy.Name;
      this.Background = copy.Background;
      this.Clouds = new List<string>((IEnumerable<string>) copy.Clouds);
      this.Layers = Enumerable.ToList<SkyLayer>(Enumerable.Select<SkyLayer, SkyLayer>((IEnumerable<SkyLayer>) copy.Layers, (Func<SkyLayer, SkyLayer>) (x => x.ShallowCopy())));
      this.WindSpeed = copy.WindSpeed;
      this.Density = copy.Density;
      this.Shadows = copy.Shadows;
      this.Stars = copy.Stars;
      this.FogDensity = copy.FogDensity;
      this.VerticalTiling = copy.VerticalTiling;
      this.HorizontalScrolling = copy.HorizontalScrolling;
      this.InterLayerVerticalDistance = copy.InterLayerVerticalDistance;
      this.InterLayerHorizontalDistance = copy.InterLayerHorizontalDistance;
      this.HorizontalDistance = copy.HorizontalDistance;
      this.VerticalDistance = copy.VerticalDistance;
      this.LayerBaseHeight = copy.LayerBaseHeight;
      this.LayerBaseSpacing = copy.LayerBaseSpacing;
      this.WindParallax = copy.WindParallax;
      this.WindDistance = copy.WindDistance;
      this.CloudsParallax = copy.CloudsParallax;
      this.FoliageShadows = copy.FoliageShadows;
      this.ShadowOpacity = copy.ShadowOpacity;
      this.NoPerFaceLayerXOffset = copy.NoPerFaceLayerXOffset;
      this.LayerBaseXOffset = copy.LayerBaseXOffset;
    }
  }
}
