// Type: FezEngine.Readers.BackgroundPlaneReader
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Structure;
using Microsoft.Xna.Framework.Content;

namespace FezEngine.Readers
{
  public class BackgroundPlaneReader : ContentTypeReader<BackgroundPlane>
  {
    protected override BackgroundPlane Read(ContentReader input, BackgroundPlane existingInstance)
    {
      if (existingInstance == null)
        existingInstance = new BackgroundPlane();
      existingInstance.Position = input.ReadVector3();
      existingInstance.Rotation = input.ReadQuaternion();
      existingInstance.Scale = input.ReadVector3();
      existingInstance.Size = input.ReadVector3();
      existingInstance.TextureName = input.ReadString();
      existingInstance.LightMap = input.ReadBoolean();
      existingInstance.AllowOverbrightness = input.ReadBoolean();
      existingInstance.Filter = input.ReadColor();
      existingInstance.Animated = input.ReadBoolean();
      existingInstance.Doublesided = input.ReadBoolean();
      existingInstance.Opacity = input.ReadSingle();
      existingInstance.AttachedGroup = input.ReadObject<int?>();
      existingInstance.Billboard = input.ReadBoolean();
      existingInstance.SyncWithSamples = input.ReadBoolean();
      existingInstance.Crosshatch = input.ReadBoolean();
      input.ReadBoolean();
      existingInstance.AlwaysOnTop = input.ReadBoolean();
      existingInstance.Fullbright = input.ReadBoolean();
      existingInstance.PixelatedLightmap = input.ReadBoolean();
      existingInstance.XTextureRepeat = input.ReadBoolean();
      existingInstance.YTextureRepeat = input.ReadBoolean();
      existingInstance.ClampTexture = input.ReadBoolean();
      existingInstance.ActorType = input.ReadObject<ActorType>();
      existingInstance.AttachedPlane = input.ReadObject<int?>();
      existingInstance.ParallaxFactor = input.ReadSingle();
      return existingInstance;
    }
  }
}
