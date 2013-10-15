// Type: Microsoft.Xna.Framework.Graphics.SpriteBatchItem
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework;

namespace Microsoft.Xna.Framework.Graphics
{
  internal class SpriteBatchItem
  {
    public Texture2D Texture;
    public float Depth;
    public VertexPositionColorTexture vertexTL;
    public VertexPositionColorTexture vertexTR;
    public VertexPositionColorTexture vertexBL;
    public VertexPositionColorTexture vertexBR;

    public SpriteBatchItem()
    {
      this.vertexTL = new VertexPositionColorTexture();
      this.vertexTR = new VertexPositionColorTexture();
      this.vertexBL = new VertexPositionColorTexture();
      this.vertexBR = new VertexPositionColorTexture();
    }

    public void Set(float x, float y, float w, float h, Color color, Vector2 texCoordTL, Vector2 texCoordBR)
    {
      this.vertexTL.Position.X = x;
      this.vertexTL.Position.Y = y;
      this.vertexTL.Position.Z = this.Depth;
      this.vertexTL.Color = color;
      this.vertexTL.TextureCoordinate.X = texCoordTL.X;
      this.vertexTL.TextureCoordinate.Y = texCoordTL.Y;
      this.vertexTR.Position.X = x + w;
      this.vertexTR.Position.Y = y;
      this.vertexTR.Position.Z = this.Depth;
      this.vertexTR.Color = color;
      this.vertexTR.TextureCoordinate.X = texCoordBR.X;
      this.vertexTR.TextureCoordinate.Y = texCoordTL.Y;
      this.vertexBL.Position.X = x;
      this.vertexBL.Position.Y = y + h;
      this.vertexBL.Position.Z = this.Depth;
      this.vertexBL.Color = color;
      this.vertexBL.TextureCoordinate.X = texCoordTL.X;
      this.vertexBL.TextureCoordinate.Y = texCoordBR.Y;
      this.vertexBR.Position.X = x + w;
      this.vertexBR.Position.Y = y + h;
      this.vertexBR.Position.Z = this.Depth;
      this.vertexBR.Color = color;
      this.vertexBR.TextureCoordinate.X = texCoordBR.X;
      this.vertexBR.TextureCoordinate.Y = texCoordBR.Y;
    }

    public void Set(float x, float y, float dx, float dy, float w, float h, float sin, float cos, Color color, Vector2 texCoordTL, Vector2 texCoordBR)
    {
      this.vertexTL.Position.X = (float) ((double) x + (double) dx * (double) cos - (double) dy * (double) sin);
      this.vertexTL.Position.Y = (float) ((double) y + (double) dx * (double) sin + (double) dy * (double) cos);
      this.vertexTL.Position.Z = this.Depth;
      this.vertexTL.Color = color;
      this.vertexTL.TextureCoordinate.X = texCoordTL.X;
      this.vertexTL.TextureCoordinate.Y = texCoordTL.Y;
      this.vertexTR.Position.X = (float) ((double) x + ((double) dx + (double) w) * (double) cos - (double) dy * (double) sin);
      this.vertexTR.Position.Y = (float) ((double) y + ((double) dx + (double) w) * (double) sin + (double) dy * (double) cos);
      this.vertexTR.Position.Z = this.Depth;
      this.vertexTR.Color = color;
      this.vertexTR.TextureCoordinate.X = texCoordBR.X;
      this.vertexTR.TextureCoordinate.Y = texCoordTL.Y;
      this.vertexBL.Position.X = (float) ((double) x + (double) dx * (double) cos - ((double) dy + (double) h) * (double) sin);
      this.vertexBL.Position.Y = (float) ((double) y + (double) dx * (double) sin + ((double) dy + (double) h) * (double) cos);
      this.vertexBL.Position.Z = this.Depth;
      this.vertexBL.Color = color;
      this.vertexBL.TextureCoordinate.X = texCoordTL.X;
      this.vertexBL.TextureCoordinate.Y = texCoordBR.Y;
      this.vertexBR.Position.X = (float) ((double) x + ((double) dx + (double) w) * (double) cos - ((double) dy + (double) h) * (double) sin);
      this.vertexBR.Position.Y = (float) ((double) y + ((double) dx + (double) w) * (double) sin + ((double) dy + (double) h) * (double) cos);
      this.vertexBR.Position.Z = this.Depth;
      this.vertexBR.Color = color;
      this.vertexBR.TextureCoordinate.X = texCoordBR.X;
      this.vertexBR.TextureCoordinate.Y = texCoordBR.Y;
    }
  }
}
