// Type: System.Drawing.ImageEx
// Assembly: MonoGame.Framework.Windows, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2107839-320D-467B-B82A-28CB452CC584
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.Windows.dll

using System.Drawing.Imaging;

namespace System.Drawing
{
  internal static class ImageEx
  {
    private static float[][] rgbtobgr;

    static ImageEx()
    {
      float[][] numArray1 = new float[5][];
      float[][] numArray2 = numArray1;
      int index1 = 0;
      float[] numArray3 = new float[5];
      numArray3[2] = 1f;
      float[] numArray4 = numArray3;
      numArray2[index1] = numArray4;
      float[][] numArray5 = numArray1;
      int index2 = 1;
      float[] numArray6 = new float[5];
      numArray6[1] = 1f;
      float[] numArray7 = numArray6;
      numArray5[index2] = numArray7;
      float[][] numArray8 = numArray1;
      int index3 = 2;
      float[] numArray9 = new float[5];
      numArray9[0] = 1f;
      float[] numArray10 = numArray9;
      numArray8[index3] = numArray10;
      float[][] numArray11 = numArray1;
      int index4 = 3;
      float[] numArray12 = new float[5];
      numArray12[3] = 1f;
      float[] numArray13 = numArray12;
      numArray11[index4] = numArray13;
      float[][] numArray14 = numArray1;
      int index5 = 4;
      float[] numArray15 = new float[5];
      numArray15[4] = 1f;
      float[] numArray16 = numArray15;
      numArray14[index5] = numArray16;
      ImageEx.rgbtobgr = numArray1;
    }

    internal static void RGBToBGR(this Image bmp)
    {
      ImageAttributes imageAttr = new ImageAttributes();
      ColorMatrix newColorMatrix = new ColorMatrix(ImageEx.rgbtobgr);
      imageAttr.SetColorMatrix(newColorMatrix);
      using (Graphics graphics = Graphics.FromImage(bmp))
        graphics.DrawImage(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, imageAttr);
    }
  }
}
