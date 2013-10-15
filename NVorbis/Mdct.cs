// Type: NVorbis.Mdct
// Assembly: NVorbis, Version=0.5.5.0, Culture=neutral, PublicKeyToken=null
// MVID: CF8FE41E-969B-4426-8E05-8E0EFF882245
// Assembly location: F:\Program Files (x86)\FEZ\NVorbis.dll

using System;
using System.Collections.Generic;

namespace NVorbis
{
  internal class Mdct
  {
    private static Dictionary<int, Mdct> _setupCache = new Dictionary<int, Mdct>(2);
    private const float M_PI = 3.141593f;
    private int n;
    private int n2;
    private int n4;
    private int n8;
    private int ld;
    private float[] A;
    private float[] B;
    private float[] C;
    private ushort[] bitrev;

    static Mdct()
    {
    }

    private Mdct(int n)
    {
      this.n = n;
      this.n2 = n >> 1;
      this.n4 = this.n2 >> 1;
      this.n8 = this.n4 >> 1;
      this.ld = Utils.ilog(n) - 1;
      this.A = new float[this.n2];
      this.B = new float[this.n2];
      this.C = new float[this.n4];
      int index1;
      int num1 = index1 = 0;
      while (num1 < this.n4)
      {
        this.A[index1] = (float) Math.Cos((double) (4 * num1) * 3.14159274101257 / (double) n);
        this.A[index1 + 1] = (float) -Math.Sin((double) (4 * num1) * 3.14159274101257 / (double) n);
        this.B[index1] = (float) Math.Cos((double) (index1 + 1) * 3.14159274101257 / (double) n / 2.0) * 0.5f;
        this.B[index1 + 1] = (float) Math.Sin((double) (index1 + 1) * 3.14159274101257 / (double) n / 2.0) * 0.5f;
        ++num1;
        index1 += 2;
      }
      int index2;
      int num2 = index2 = 0;
      while (num2 < this.n8)
      {
        this.C[index2] = (float) Math.Cos((double) (2 * (index2 + 1)) * 3.14159274101257 / (double) n);
        this.C[index2 + 1] = (float) -Math.Sin((double) (2 * (index2 + 1)) * 3.14159274101257 / (double) n);
        ++num2;
        index2 += 2;
      }
      this.bitrev = new ushort[this.n8];
      for (int index3 = 0; index3 < this.n8; ++index3)
        this.bitrev[index3] = (ushort) (Utils.BitReverse((uint) index3, this.ld - 3) << 2);
    }

    public static float[] Reverse(float[] samples)
    {
      Mdct.GetSetup(samples.Length).CalcReverse(samples);
      return samples;
    }

    private static Mdct GetSetup(int n)
    {
      if (!Mdct._setupCache.ContainsKey(n))
      {
        lock (Mdct._setupCache)
        {
          if (!Mdct._setupCache.ContainsKey(n))
            Mdct._setupCache[n] = new Mdct(n);
        }
      }
      return Mdct._setupCache[n];
    }

    private unsafe void CalcReverse(float[] buf)
    {
      float[] buffer = ACache.Get<float>(this.n2);
      fixed (float* numPtr1 = buf)
        fixed (float* numPtr2 = buffer)
        {
          float* e;
          float* numPtr3;
          fixed (float* A1 = this.A)
          {
            float* numPtr4 = numPtr2 + (this.n2 - 2);
            float* numPtr5 = A1;
            float* numPtr6 = numPtr1;
            float* numPtr7 = numPtr1 + this.n2;
            while (numPtr6 != numPtr7)
            {
              numPtr4[1] = (float) ((double) *numPtr6 * (double) *numPtr5 - (double) numPtr6[2] * (double) numPtr5[1]);
              *numPtr4 = (float) ((double) *numPtr6 * (double) numPtr5[1] + (double) numPtr6[2] * (double) *numPtr5);
              numPtr4 -= 2;
              numPtr5 += 2;
              numPtr6 += 4;
            }
            float* numPtr8 = numPtr1 + (this.n2 - 3);
            // ISSUE: cast to a reference type
            while (numPtr4 >= (float&) numPtr2)
            {
              numPtr4[1] = (float) (-(double) numPtr8[2] * (double) *numPtr5 - -(double) *numPtr8 * (double) numPtr5[1]);
              *numPtr4 = (float) (-(double) numPtr8[2] * (double) numPtr5[1] + -(double) *numPtr8 * (double) *numPtr5);
              numPtr4 -= 2;
              numPtr5 += 2;
              numPtr8 -= 4;
            }
            e = numPtr1;
            numPtr3 = numPtr2;
            float* numPtr9 = A1 + (this.n2 - 8);
            float* numPtr10 = numPtr3 + this.n4;
            float* numPtr11 = numPtr3;
            float* numPtr12 = e + this.n4;
            float* numPtr13 = e;
            // ISSUE: cast to a reference type
            while (numPtr9 >= (float&) A1)
            {
              float num1 = numPtr10[1] - numPtr11[1];
              float num2 = *numPtr10 - *numPtr11;
              numPtr12[1] = numPtr10[1] + numPtr11[1];
              *numPtr12 = *numPtr10 + *numPtr11;
              numPtr13[1] = (float) ((double) num1 * (double) numPtr9[4] - (double) num2 * (double) numPtr9[5]);
              *numPtr13 = (float) ((double) num2 * (double) numPtr9[4] + (double) num1 * (double) numPtr9[5]);
              float num3 = numPtr10[3] - numPtr11[3];
              float num4 = numPtr10[2] - numPtr11[2];
              numPtr12[3] = numPtr10[3] + numPtr11[3];
              numPtr12[2] = numPtr10[2] + numPtr11[2];
              numPtr13[3] = (float) ((double) num3 * (double) *numPtr9 - (double) num4 * (double) numPtr9[1]);
              numPtr13[2] = (float) ((double) num4 * (double) *numPtr9 + (double) num3 * (double) numPtr9[1]);
              numPtr9 -= 8;
              numPtr12 += 4;
              numPtr13 += 4;
              numPtr10 += 4;
              numPtr11 += 4;
            }
            this.imdct_step3_iter0_loop(this.n >> 4, e, this.n2 - 1, -(this.n >> 3), A1);
            this.imdct_step3_iter0_loop(this.n >> 4, e, this.n2 - 1 - this.n4, -(this.n >> 3), A1);
            this.imdct_step3_inner_r_loop(this.n >> 5, e, this.n2 - 1, -(this.n >> 4), A1, 16);
            this.imdct_step3_inner_r_loop(this.n >> 5, e, this.n2 - 1 - this.n8, -(this.n >> 4), A1, 16);
            this.imdct_step3_inner_r_loop(this.n >> 5, e, this.n2 - 1 - this.n8 * 2, -(this.n >> 4), A1, 16);
            this.imdct_step3_inner_r_loop(this.n >> 5, e, this.n2 - 1 - this.n8 * 3, -(this.n >> 4), A1, 16);
            int num5;
            for (num5 = 2; num5 < this.ld - 3 >> 1; ++num5)
            {
              int num1 = this.n >> num5 + 2;
              int num2 = num1 >> 1;
              int num3 = 1 << num5 + 1;
              for (int index = 0; index < num3; ++index)
                this.imdct_step3_inner_r_loop(this.n >> num5 + 4, e, this.n2 - 1 - num1 * index, -num2, A1, 1 << num5 + 3);
            }
            for (; num5 < this.ld - 6; ++num5)
            {
              int k0 = this.n >> num5 + 2;
              int a_off = 1 << num5 + 3;
              int num1 = k0 >> 1;
              int num2 = this.n >> num5 + 6;
              int n = 1 << num5 + 1;
              float* A2 = A1;
              int i_off = this.n2 - 1;
              for (int index = num2; index > 0; --index)
              {
                this.imdct_step3_inner_s_loop(n, e, i_off, -num1, A2, a_off, k0);
                A2 += a_off * 4;
                i_off -= 8;
              }
            }
            this.imdct_step3_inner_s_loop_ld654(this.n >> 5, e, this.n2 - 1, A1, this.n);
          }
          fixed (ushort* numPtr4 = this.bitrev)
          {
            float* numPtr5 = numPtr3 + (this.n4 - 4);
            float* numPtr6 = numPtr3 + (this.n2 - 4);
            while (numPtr5 >= numPtr3)
            {
              int index1 = (int) numPtr4[0];
              numPtr6[3] = e[index1];
              numPtr6[2] = e[index1 + 1];
              numPtr5[3] = e[index1 + 2];
              numPtr5[2] = e[index1 + 3];
              int index2 = (int) numPtr4[1];
              numPtr6[1] = e[index2];
              *numPtr6 = e[index2 + 1];
              numPtr5[1] = e[index2 + 2];
              *numPtr5 = e[index2 + 3];
              numPtr5 -= 4;
              numPtr6 -= 4;
              numPtr4 += 2;
            }
          }
          fixed (float* numPtr4 = this.C)
          {
            float* numPtr5 = numPtr3;
            float* numPtr6 = numPtr3 + this.n2 - 4;
            while (numPtr5 < numPtr6)
            {
              float num1 = *numPtr5 - numPtr6[2];
              float num2 = numPtr5[1] + numPtr6[3];
              float num3 = (float) ((double) numPtr4[1] * (double) num1 + (double) numPtr4[0] * (double) num2);
              float num4 = (float) ((double) numPtr4[1] * (double) num2 - (double) numPtr4[0] * (double) num1);
              float num5 = *numPtr5 + numPtr6[2];
              float num6 = numPtr5[1] - numPtr6[3];
              *numPtr5 = num5 + num3;
              numPtr5[1] = num6 + num4;
              numPtr6[2] = num5 - num3;
              numPtr6[3] = num4 - num6;
              float num7 = numPtr5[2] - *numPtr6;
              float num8 = numPtr5[3] + numPtr6[1];
              float num9 = (float) ((double) numPtr4[3] * (double) num7 + (double) numPtr4[2] * (double) num8);
              float num10 = (float) ((double) numPtr4[3] * (double) num8 - (double) numPtr4[2] * (double) num7);
              float num11 = numPtr5[2] + *numPtr6;
              float num12 = numPtr5[3] - numPtr6[1];
              numPtr5[2] = num11 + num9;
              numPtr5[3] = num12 + num10;
              *numPtr6 = num11 - num9;
              numPtr6[1] = num10 - num12;
              numPtr4 += 4;
              numPtr5 += 4;
              numPtr6 -= 4;
            }
          }
          fixed (float* numPtr4 = this.B)
          {
            float* numPtr5 = numPtr4 + this.n2 - 8;
            float* numPtr6 = numPtr2 + this.n2 - 8;
            float* numPtr7 = numPtr1;
            float* numPtr8 = numPtr1 + (this.n2 - 4);
            float* numPtr9 = numPtr1 + this.n2;
            float* numPtr10 = numPtr1 + (this.n - 4);
            while (numPtr6 >= numPtr3)
            {
              float num1 = (float) ((double) numPtr6[6] * (double) numPtr5[7] - (double) numPtr6[7] * (double) numPtr5[6]);
              float num2 = (float) (-(double) numPtr6[6] * (double) numPtr5[6] - (double) numPtr6[7] * (double) numPtr5[7]);
              *numPtr7 = num1;
              numPtr8[3] = -num1;
              *numPtr9 = num2;
              numPtr10[3] = num2;
              float num3 = (float) ((double) numPtr6[4] * (double) numPtr5[5] - (double) numPtr6[5] * (double) numPtr5[4]);
              float num4 = (float) (-(double) numPtr6[4] * (double) numPtr5[4] - (double) numPtr6[5] * (double) numPtr5[5]);
              numPtr7[1] = num3;
              numPtr8[2] = -num3;
              numPtr9[1] = num4;
              numPtr10[2] = num4;
              float num5 = (float) ((double) numPtr6[2] * (double) numPtr5[3] - (double) numPtr6[3] * (double) numPtr5[2]);
              float num6 = (float) (-(double) numPtr6[2] * (double) numPtr5[2] - (double) numPtr6[3] * (double) numPtr5[3]);
              numPtr7[2] = num5;
              numPtr8[1] = -num5;
              numPtr9[2] = num6;
              numPtr10[1] = num6;
              float num7 = (float) ((double) *numPtr6 * (double) numPtr5[1] - (double) numPtr6[1] * (double) *numPtr5);
              float num8 = (float) (-(double) *numPtr6 * (double) *numPtr5 - (double) numPtr6[1] * (double) numPtr5[1]);
              numPtr7[3] = num7;
              *numPtr8 = -num7;
              numPtr9[3] = num8;
              *numPtr10 = num8;
              numPtr5 -= 8;
              numPtr6 -= 8;
              numPtr7 += 4;
              numPtr9 += 4;
              numPtr8 -= 4;
              numPtr10 -= 4;
            }
          }
        }
      ACache.Return<float>(ref buffer);
    }

    private unsafe void imdct_step3_iter0_loop(int n, float* e, int i_off, int k_off, float* A)
    {
      float* numPtr1 = e + i_off;
      float* numPtr2 = numPtr1 + k_off;
      for (int index = n >> 2; index > 0; --index)
      {
        float num1 = *numPtr1 - *numPtr2;
        float num2 = numPtr1[-1] - numPtr2[-1];
        float* numPtr3 = numPtr1;
        double num3 = (double) *numPtr3 + (double) *numPtr2;
        *numPtr3 = (float) num3;
        IntPtr num4 = (IntPtr) (numPtr1 + -1);
        double num5 = (double) *(float*) num4 + (double) numPtr2[-1];
        *(float*) num4 = (float) num5;
        *numPtr2 = (float) ((double) num1 * (double) *A - (double) num2 * (double) A[1]);
        numPtr2[-1] = (float) ((double) num2 * (double) *A + (double) num1 * (double) A[1]);
        A += 8;
        float num6 = numPtr1[-2] - numPtr2[-2];
        float num7 = numPtr1[-3] - numPtr2[-3];
        IntPtr num8 = (IntPtr) (numPtr1 + -2);
        double num9 = (double) *(float*) num8 + (double) numPtr2[-2];
        *(float*) num8 = (float) num9;
        IntPtr num10 = (IntPtr) (numPtr1 + -3);
        double num11 = (double) *(float*) num10 + (double) numPtr2[-3];
        *(float*) num10 = (float) num11;
        numPtr2[-2] = (float) ((double) num6 * (double) *A - (double) num7 * (double) A[1]);
        numPtr2[-3] = (float) ((double) num7 * (double) *A + (double) num6 * (double) A[1]);
        A += 8;
        float num12 = numPtr1[-4] - numPtr2[-4];
        float num13 = numPtr1[-5] - numPtr2[-5];
        IntPtr num14 = (IntPtr) (numPtr1 + -4);
        double num15 = (double) *(float*) num14 + (double) numPtr2[-4];
        *(float*) num14 = (float) num15;
        IntPtr num16 = (IntPtr) (numPtr1 + -5);
        double num17 = (double) *(float*) num16 + (double) numPtr2[-5];
        *(float*) num16 = (float) num17;
        numPtr2[-4] = (float) ((double) num12 * (double) *A - (double) num13 * (double) A[1]);
        numPtr2[-5] = (float) ((double) num13 * (double) *A + (double) num12 * (double) A[1]);
        A += 8;
        float num18 = numPtr1[-6] - numPtr2[-6];
        float num19 = numPtr1[-7] - numPtr2[-7];
        IntPtr num20 = (IntPtr) (numPtr1 + -6);
        double num21 = (double) *(float*) num20 + (double) numPtr2[-6];
        *(float*) num20 = (float) num21;
        IntPtr num22 = (IntPtr) (numPtr1 + -7);
        double num23 = (double) *(float*) num22 + (double) numPtr2[-7];
        *(float*) num22 = (float) num23;
        numPtr2[-6] = (float) ((double) num18 * (double) *A - (double) num19 * (double) A[1]);
        numPtr2[-7] = (float) ((double) num19 * (double) *A + (double) num18 * (double) A[1]);
        A += 8;
        numPtr1 -= 8;
        numPtr2 -= 8;
      }
    }

    private unsafe void imdct_step3_inner_r_loop(int lim, float* e, int d0, int k_off, float* A, int k1)
    {
      float* numPtr1 = e + d0;
      float* numPtr2 = numPtr1 + k_off;
      for (int index = lim >> 2; index > 0; --index)
      {
        float num1 = *numPtr1 - *numPtr2;
        float num2 = numPtr1[-1] - numPtr2[-1];
        float* numPtr3 = numPtr1;
        double num3 = (double) *numPtr3 + (double) *numPtr2;
        *numPtr3 = (float) num3;
        IntPtr num4 = (IntPtr) (numPtr1 + -1);
        double num5 = (double) *(float*) num4 + (double) numPtr2[-1];
        *(float*) num4 = (float) num5;
        *numPtr2 = (float) ((double) num1 * (double) *A - (double) num2 * (double) A[1]);
        numPtr2[-1] = (float) ((double) num2 * (double) *A + (double) num1 * (double) A[1]);
        A += k1;
        float num6 = numPtr1[-2] - numPtr2[-2];
        float num7 = numPtr1[-3] - numPtr2[-3];
        IntPtr num8 = (IntPtr) (numPtr1 + -2);
        double num9 = (double) *(float*) num8 + (double) numPtr2[-2];
        *(float*) num8 = (float) num9;
        IntPtr num10 = (IntPtr) (numPtr1 + -3);
        double num11 = (double) *(float*) num10 + (double) numPtr2[-3];
        *(float*) num10 = (float) num11;
        numPtr2[-2] = (float) ((double) num6 * (double) *A - (double) num7 * (double) A[1]);
        numPtr2[-3] = (float) ((double) num7 * (double) *A + (double) num6 * (double) A[1]);
        A += k1;
        float num12 = numPtr1[-4] - numPtr2[-4];
        float num13 = numPtr1[-5] - numPtr2[-5];
        IntPtr num14 = (IntPtr) (numPtr1 + -4);
        double num15 = (double) *(float*) num14 + (double) numPtr2[-4];
        *(float*) num14 = (float) num15;
        IntPtr num16 = (IntPtr) (numPtr1 + -5);
        double num17 = (double) *(float*) num16 + (double) numPtr2[-5];
        *(float*) num16 = (float) num17;
        numPtr2[-4] = (float) ((double) num12 * (double) *A - (double) num13 * (double) A[1]);
        numPtr2[-5] = (float) ((double) num13 * (double) *A + (double) num12 * (double) A[1]);
        A += k1;
        float num18 = numPtr1[-6] - numPtr2[-6];
        float num19 = numPtr1[-7] - numPtr2[-7];
        IntPtr num20 = (IntPtr) (numPtr1 + -6);
        double num21 = (double) *(float*) num20 + (double) numPtr2[-6];
        *(float*) num20 = (float) num21;
        IntPtr num22 = (IntPtr) (numPtr1 + -7);
        double num23 = (double) *(float*) num22 + (double) numPtr2[-7];
        *(float*) num22 = (float) num23;
        numPtr2[-6] = (float) ((double) num18 * (double) *A - (double) num19 * (double) A[1]);
        numPtr2[-7] = (float) ((double) num19 * (double) *A + (double) num18 * (double) A[1]);
        numPtr1 -= 8;
        numPtr2 -= 8;
        A += k1;
      }
    }

    private unsafe void imdct_step3_inner_s_loop(int n, float* e, int i_off, int k_off, float* A, int a_off, int k0)
    {
      float num1 = *A;
      float num2 = A[1];
      float num3 = A[a_off];
      float num4 = A[a_off + 1];
      float num5 = A[a_off * 2];
      float num6 = A[a_off * 2 + 1];
      float num7 = A[a_off * 3];
      float num8 = A[a_off * 3 + 1];
      float* numPtr1 = e + i_off;
      float* numPtr2 = numPtr1 + k_off;
      for (int index = n; index > 0; --index)
      {
        float num9 = *numPtr1 - *numPtr2;
        float num10 = numPtr1[-1] - numPtr2[-1];
        *numPtr1 = *numPtr1 + *numPtr2;
        numPtr1[-1] = numPtr1[-1] + numPtr2[-1];
        *numPtr2 = (float) ((double) num9 * (double) num1 - (double) num10 * (double) num2);
        numPtr2[-1] = (float) ((double) num10 * (double) num1 + (double) num9 * (double) num2);
        float num11 = numPtr1[-2] - numPtr2[-2];
        float num12 = numPtr1[-3] - numPtr2[-3];
        numPtr1[-2] = numPtr1[-2] + numPtr2[-2];
        numPtr1[-3] = numPtr1[-3] + numPtr2[-3];
        numPtr2[-2] = (float) ((double) num11 * (double) num3 - (double) num12 * (double) num4);
        numPtr2[-3] = (float) ((double) num12 * (double) num3 + (double) num11 * (double) num4);
        float num13 = numPtr1[-4] - numPtr2[-4];
        float num14 = numPtr1[-5] - numPtr2[-5];
        numPtr1[-4] = numPtr1[-4] + numPtr2[-4];
        numPtr1[-5] = numPtr1[-5] + numPtr2[-5];
        numPtr2[-4] = (float) ((double) num13 * (double) num5 - (double) num14 * (double) num6);
        numPtr2[-5] = (float) ((double) num14 * (double) num5 + (double) num13 * (double) num6);
        float num15 = numPtr1[-6] - numPtr2[-6];
        float num16 = numPtr1[-7] - numPtr2[-7];
        numPtr1[-6] = numPtr1[-6] + numPtr2[-6];
        numPtr1[-7] = numPtr1[-7] + numPtr2[-7];
        numPtr2[-6] = (float) ((double) num15 * (double) num7 - (double) num16 * (double) num8);
        numPtr2[-7] = (float) ((double) num16 * (double) num7 + (double) num15 * (double) num8);
        numPtr1 -= k0;
        numPtr2 -= k0;
      }
    }

    private unsafe void imdct_step3_inner_s_loop_ld654(int n, float* e, int i_off, float* A, int base_n)
    {
      int index = base_n >> 3;
      float num1 = A[index];
      float* z = e + i_off;
      float* numPtr = z - 16 * n;
      while (z > numPtr)
      {
        float num2 = *z - z[-8];
        float num3 = z[-1] - z[-9];
        *z = *z + z[-8];
        z[-1] = z[-1] + z[-9];
        z[-8] = num2;
        z[-9] = num3;
        float num4 = z[-2] - z[-10];
        float num5 = z[-3] - z[-11];
        z[-2] = z[-2] + z[-10];
        z[-3] = z[-3] + z[-11];
        z[-10] = (num4 + num5) * num1;
        z[-11] = (num5 - num4) * num1;
        float num6 = z[-12] - z[-4];
        float num7 = z[-5] - z[-13];
        z[-4] = z[-4] + z[-12];
        z[-5] = z[-5] + z[-13];
        z[-12] = num7;
        z[-13] = num6;
        float num8 = z[-14] - z[-6];
        float num9 = z[-7] - z[-15];
        z[-6] = z[-6] + z[-14];
        z[-7] = z[-7] + z[-15];
        z[-14] = (num8 + num9) * num1;
        z[-15] = (num8 - num9) * num1;
        this.iter_54(z);
        this.iter_54(z - 8);
        z -= 16;
      }
    }

    private unsafe void iter_54(float* z)
    {
      float num1 = *z - z[-4];
      float num2 = *z + z[-4];
      float num3 = z[-2] + z[-6];
      float num4 = z[-2] - z[-6];
      *z = num2 + num3;
      z[-2] = num2 - num3;
      float num5 = z[-3] - z[-7];
      z[-4] = num1 + num5;
      z[-6] = num1 - num5;
      float num6 = z[-1] - z[-5];
      float num7 = z[-1] + z[-5];
      float num8 = z[-3] + z[-7];
      z[-1] = num7 + num8;
      z[-3] = num7 - num8;
      z[-5] = num6 - num4;
      z[-7] = num6 + num4;
    }
  }
}
