// Type: FezEngine.Readers.GarbagelessTexture2DReader
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace FezEngine.Readers
{
  public class GarbagelessTexture2DReader : ContentTypeReader<Texture2D>
  {
    private readonly byte[] Buffer;

    public GarbagelessTexture2DReader(byte[] buffer)
    {
      this.Buffer = buffer;
    }

    protected override Texture2D Read(ContentReader input, Texture2D existingInstance)
    {
      GraphicsDevice graphicsDevice = ((IGraphicsDeviceService) input.ContentManager.ServiceProvider.GetService(typeof (IGraphicsDeviceService))).GraphicsDevice;
      SurfaceFormat format = (SurfaceFormat) input.ReadInt32();
      int width = input.ReadInt32();
      int height = input.ReadInt32();
      int num1 = input.ReadInt32();
      Texture2D texture2D = new Texture2D(graphicsDevice, width, height, num1 > 1, format);
      for (int level = 0; level < num1; ++level)
      {
        int elementCount = input.ReadInt32();
        if (elementCount < 0 || elementCount > this.Buffer.Length)
        {
          throw new InvalidDataException(string.Format("sizeof(level{0}, w{1}h{2}nl{3}) = {4}", (object) level, (object) width, (object) height, (object) num1, (object) elementCount));
        }
        else
        {
          int count = elementCount;
          int index = 0;
          while (count > 0)
          {
            int num2 = input.Read(this.Buffer, index, count);
            index += num2;
            count -= num2;
          }
          texture2D.SetData<byte>(level, new Rectangle?(), this.Buffer, 0, elementCount);
        }
      }
      return texture2D;
    }
  }
}
