// Type: FezEngine.Readers.AnimatedTextureReader
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using FezEngine.Content;
using FezEngine.Structure;
using FezEngine.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FezEngine.Readers
{
  public class AnimatedTextureReader : ContentTypeReader<AnimatedTexture>
  {
    protected override AnimatedTexture Read(ContentReader input, AnimatedTexture existingInstance)
    {
      if (existingInstance == null)
        existingInstance = new AnimatedTexture();
      GraphicsDevice graphicsDevice = ((IGraphicsDeviceService) input.ContentManager.ServiceProvider.GetService(typeof (IGraphicsDeviceService))).GraphicsDevice;
      int width = input.ReadInt32();
      int height = input.ReadInt32();
      existingInstance.FrameWidth = input.ReadInt32();
      existingInstance.FrameHeight = input.ReadInt32();
      byte[] data = input.ReadBytes(input.ReadInt32());
      List<FrameContent> list = input.ReadObject<List<FrameContent>>();
      existingInstance.Texture = new Texture2D(graphicsDevice, width, height, false, SurfaceFormat.Color);
      existingInstance.Texture.SetData<byte>(data);
      existingInstance.Offsets = Enumerable.ToArray<Rectangle>(Enumerable.Select<FrameContent, Rectangle>((IEnumerable<FrameContent>) list, (Func<FrameContent, Rectangle>) (x => x.Rectangle)));
      existingInstance.Timing = new AnimationTiming(0, list.Count - 1, Enumerable.ToArray<float>(Enumerable.Select<FrameContent, float>((IEnumerable<FrameContent>) list, (Func<FrameContent, float>) (x => (float) x.Duration.TotalSeconds))));
      existingInstance.PotOffset = new Vector2((float) (FezMath.NextPowerOfTwo((double) existingInstance.FrameWidth) - existingInstance.FrameWidth), (float) (FezMath.NextPowerOfTwo((double) existingInstance.FrameHeight) - existingInstance.FrameHeight));
      return existingInstance;
    }
  }
}
