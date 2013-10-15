// Type: OpenTK.Platform.Windows.Wgl
// Assembly: OpenTK, Version=1.1.0.0, Culture=neutral, PublicKeyToken=bad199fe84eb3df4
// MVID: FE2CFFE8-B942-430E-8C15-E783DB6F0AD6
// Assembly location: F:\Program Files (x86)\FEZ\OpenTK.dll

using OpenTK.Platform;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;

namespace OpenTK.Platform.Windows
{
  internal static class Wgl
  {
    private static bool rebuildExtensionList = true;
    private static readonly object SyncRoot = new object();
    private static Assembly assembly = Assembly.GetExecutingAssembly();
    private static Type wglClass = Wgl.assembly.GetType("OpenTK.Platform.Windows.Wgl");
    private static Type delegatesClass = Wgl.wglClass.GetNestedType("Delegates", BindingFlags.Static | BindingFlags.NonPublic);
    private static Type importsClass = Wgl.wglClass.GetNestedType("Imports", BindingFlags.Static | BindingFlags.NonPublic);
    internal const string Library = "OPENGL32.DLL";

    static Wgl()
    {
    }

    private static Delegate LoadDelegate(string name, Type signature)
    {
      string str = name.StartsWith("wgl") ? name.Substring(3) : name;
      return Wgl.importsClass.GetMethod(str, BindingFlags.Static | BindingFlags.NonPublic) == null ? Wgl.GetExtensionDelegate(name, signature) : Wgl.GetExtensionDelegate(name, signature) ?? Delegate.CreateDelegate(signature, typeof (Wgl.Imports), str);
    }

    private static Delegate GetExtensionDelegate(string name, Type signature)
    {
      IntPtr procAddress = Wgl.Imports.GetProcAddress(name);
      if (procAddress == IntPtr.Zero || procAddress == new IntPtr(1) || procAddress == new IntPtr(2))
        return (Delegate) null;
      else
        return Marshal.GetDelegateForFunctionPointer(procAddress, signature);
    }

    public static void LoadAll()
    {
      lock (Wgl.SyncRoot)
        Utilities.LoadExtensions(typeof (Wgl));
    }

    public static bool Load(string function)
    {
      return Utilities.TryLoadExtension(typeof (Wgl), function);
    }

    public static IntPtr CreateContext(IntPtr hDc)
    {
      return Wgl.Delegates.wglCreateContext(hDc);
    }

    public static bool DeleteContext(IntPtr oldContext)
    {
      return Wgl.Delegates.wglDeleteContext(oldContext);
    }

    public static IntPtr GetCurrentContext()
    {
      return Wgl.Delegates.wglGetCurrentContext();
    }

    public static bool MakeCurrent(IntPtr hDc, IntPtr newContext)
    {
      return Wgl.Delegates.wglMakeCurrent(hDc, newContext);
    }

    [CLSCompliant(false)]
    public static bool CopyContext(IntPtr hglrcSrc, IntPtr hglrcDst, uint mask)
    {
      return Wgl.Delegates.wglCopyContext(hglrcSrc, hglrcDst, mask);
    }

    public static bool CopyContext(IntPtr hglrcSrc, IntPtr hglrcDst, int mask)
    {
      return Wgl.Delegates.wglCopyContext(hglrcSrc, hglrcDst, (uint) mask);
    }

    public static unsafe int ChoosePixelFormat(IntPtr hDc, PixelFormatDescriptor[] pPfd)
    {
      fixed (PixelFormatDescriptor* pPfd1 = pPfd)
        return Wgl.Delegates.wglChoosePixelFormat(hDc, pPfd1);
    }

    public static unsafe int ChoosePixelFormat(IntPtr hDc, ref PixelFormatDescriptor pPfd)
    {
      fixed (PixelFormatDescriptor* pPfd1 = &pPfd)
        return Wgl.Delegates.wglChoosePixelFormat(hDc, pPfd1);
    }

    [CLSCompliant(false)]
    public static unsafe int ChoosePixelFormat(IntPtr hDc, PixelFormatDescriptor* pPfd)
    {
      return Wgl.Delegates.wglChoosePixelFormat(hDc, pPfd);
    }

    [CLSCompliant(false)]
    public static unsafe int DescribePixelFormat(IntPtr hdc, int ipfd, uint cjpfd, PixelFormatDescriptor[] ppfd)
    {
      fixed (PixelFormatDescriptor* ppfd1 = ppfd)
        return Wgl.Delegates.wglDescribePixelFormat(hdc, ipfd, cjpfd, ppfd1);
    }

    public static unsafe int DescribePixelFormat(IntPtr hdc, int ipfd, int cjpfd, PixelFormatDescriptor[] ppfd)
    {
      fixed (PixelFormatDescriptor* ppfd1 = ppfd)
        return Wgl.Delegates.wglDescribePixelFormat(hdc, ipfd, (uint) cjpfd, ppfd1);
    }

    [CLSCompliant(false)]
    public static unsafe int DescribePixelFormat(IntPtr hdc, int ipfd, uint cjpfd, ref PixelFormatDescriptor ppfd)
    {
      fixed (PixelFormatDescriptor* ppfd1 = &ppfd)
        return Wgl.Delegates.wglDescribePixelFormat(hdc, ipfd, cjpfd, ppfd1);
    }

    public static unsafe int DescribePixelFormat(IntPtr hdc, int ipfd, int cjpfd, ref PixelFormatDescriptor ppfd)
    {
      fixed (PixelFormatDescriptor* ppfd1 = &ppfd)
        return Wgl.Delegates.wglDescribePixelFormat(hdc, ipfd, (uint) cjpfd, ppfd1);
    }

    [CLSCompliant(false)]
    public static unsafe int DescribePixelFormat(IntPtr hdc, int ipfd, uint cjpfd, PixelFormatDescriptor* ppfd)
    {
      return Wgl.Delegates.wglDescribePixelFormat(hdc, ipfd, cjpfd, ppfd);
    }

    [CLSCompliant(false)]
    public static unsafe int DescribePixelFormat(IntPtr hdc, int ipfd, int cjpfd, PixelFormatDescriptor* ppfd)
    {
      return Wgl.Delegates.wglDescribePixelFormat(hdc, ipfd, (uint) cjpfd, ppfd);
    }

    public static IntPtr GetCurrentDC()
    {
      return Wgl.Delegates.wglGetCurrentDC();
    }

    public static IntPtr GetDefaultProcAddres(string lpszProc)
    {
      return Wgl.Delegates.wglGetDefaultProcAddress(lpszProc);
    }

    public static IntPtr GetProcAddres(string lpszProc)
    {
      return Wgl.Delegates.wglGetProcAddress(lpszProc);
    }

    public static int GetPixelFormat(IntPtr hdc)
    {
      return Wgl.Delegates.wglGetPixelFormat(hdc);
    }

    public static unsafe bool SetPixelFormat(IntPtr hdc, int ipfd, PixelFormatDescriptor[] ppfd)
    {
      fixed (PixelFormatDescriptor* ppfd1 = ppfd)
        return Wgl.Delegates.wglSetPixelFormat(hdc, ipfd, ppfd1);
    }

    public static unsafe bool SetPixelFormat(IntPtr hdc, int ipfd, ref PixelFormatDescriptor ppfd)
    {
      fixed (PixelFormatDescriptor* ppfd1 = &ppfd)
        return Wgl.Delegates.wglSetPixelFormat(hdc, ipfd, ppfd1);
    }

    [CLSCompliant(false)]
    public static unsafe bool SetPixelFormat(IntPtr hdc, int ipfd, PixelFormatDescriptor* ppfd)
    {
      return Wgl.Delegates.wglSetPixelFormat(hdc, ipfd, ppfd);
    }

    public static bool SwapBuffers(IntPtr hdc)
    {
      return Wgl.Delegates.wglSwapBuffers(hdc);
    }

    public static bool ShareLists(IntPtr hrcSrvShare, IntPtr hrcSrvSource)
    {
      return Wgl.Delegates.wglShareLists(hrcSrvShare, hrcSrvSource);
    }

    public static IntPtr CreateLayerContext(IntPtr hDc, int level)
    {
      return Wgl.Delegates.wglCreateLayerContext(hDc, level);
    }

    [CLSCompliant(false)]
    public static unsafe bool DescribeLayerPlane(IntPtr hDc, int pixelFormat, int layerPlane, uint nBytes, LayerPlaneDescriptor[] plpd)
    {
      fixed (LayerPlaneDescriptor* plpd1 = plpd)
        return Wgl.Delegates.wglDescribeLayerPlane(hDc, pixelFormat, layerPlane, nBytes, plpd1);
    }

    public static unsafe bool DescribeLayerPlane(IntPtr hDc, int pixelFormat, int layerPlane, int nBytes, LayerPlaneDescriptor[] plpd)
    {
      fixed (LayerPlaneDescriptor* plpd1 = plpd)
        return Wgl.Delegates.wglDescribeLayerPlane(hDc, pixelFormat, layerPlane, (uint) nBytes, plpd1);
    }

    [CLSCompliant(false)]
    public static unsafe bool DescribeLayerPlane(IntPtr hDc, int pixelFormat, int layerPlane, uint nBytes, ref LayerPlaneDescriptor plpd)
    {
      fixed (LayerPlaneDescriptor* plpd1 = &plpd)
        return Wgl.Delegates.wglDescribeLayerPlane(hDc, pixelFormat, layerPlane, nBytes, plpd1);
    }

    public static unsafe bool DescribeLayerPlane(IntPtr hDc, int pixelFormat, int layerPlane, int nBytes, ref LayerPlaneDescriptor plpd)
    {
      fixed (LayerPlaneDescriptor* plpd1 = &plpd)
        return Wgl.Delegates.wglDescribeLayerPlane(hDc, pixelFormat, layerPlane, (uint) nBytes, plpd1);
    }

    [CLSCompliant(false)]
    public static unsafe bool DescribeLayerPlane(IntPtr hDc, int pixelFormat, int layerPlane, uint nBytes, LayerPlaneDescriptor* plpd)
    {
      return Wgl.Delegates.wglDescribeLayerPlane(hDc, pixelFormat, layerPlane, nBytes, plpd);
    }

    [CLSCompliant(false)]
    public static unsafe bool DescribeLayerPlane(IntPtr hDc, int pixelFormat, int layerPlane, int nBytes, LayerPlaneDescriptor* plpd)
    {
      return Wgl.Delegates.wglDescribeLayerPlane(hDc, pixelFormat, layerPlane, (uint) nBytes, plpd);
    }

    public static unsafe int SetLayerPaletteEntries(IntPtr hdc, int iLayerPlane, int iStart, int cEntries, int[] pcr)
    {
      fixed (int* pcr1 = pcr)
        return Wgl.Delegates.wglSetLayerPaletteEntries(hdc, iLayerPlane, iStart, cEntries, pcr1);
    }

    public static unsafe int SetLayerPaletteEntries(IntPtr hdc, int iLayerPlane, int iStart, int cEntries, ref int pcr)
    {
      fixed (int* pcr1 = &pcr)
        return Wgl.Delegates.wglSetLayerPaletteEntries(hdc, iLayerPlane, iStart, cEntries, pcr1);
    }

    [CLSCompliant(false)]
    public static unsafe int SetLayerPaletteEntries(IntPtr hdc, int iLayerPlane, int iStart, int cEntries, int* pcr)
    {
      return Wgl.Delegates.wglSetLayerPaletteEntries(hdc, iLayerPlane, iStart, cEntries, pcr);
    }

    public static unsafe int GetLayerPaletteEntries(IntPtr hdc, int iLayerPlane, int iStart, int cEntries, int[] pcr)
    {
      fixed (int* pcr1 = pcr)
        return Wgl.Delegates.wglGetLayerPaletteEntries(hdc, iLayerPlane, iStart, cEntries, pcr1);
    }

    public static unsafe int GetLayerPaletteEntries(IntPtr hdc, int iLayerPlane, int iStart, int cEntries, ref int pcr)
    {
      fixed (int* pcr1 = &pcr)
        return Wgl.Delegates.wglGetLayerPaletteEntries(hdc, iLayerPlane, iStart, cEntries, pcr1);
    }

    [CLSCompliant(false)]
    public static unsafe int GetLayerPaletteEntries(IntPtr hdc, int iLayerPlane, int iStart, int cEntries, int* pcr)
    {
      return Wgl.Delegates.wglGetLayerPaletteEntries(hdc, iLayerPlane, iStart, cEntries, pcr);
    }

    public static bool RealizeLayerPalette(IntPtr hdc, int iLayerPlane, bool bRealize)
    {
      return Wgl.Delegates.wglRealizeLayerPalette(hdc, iLayerPlane, bRealize);
    }

    [CLSCompliant(false)]
    public static bool SwapLayerBuffers(IntPtr hdc, uint fuFlags)
    {
      return Wgl.Delegates.wglSwapLayerBuffers(hdc, fuFlags);
    }

    public static bool SwapLayerBuffers(IntPtr hdc, int fuFlags)
    {
      return Wgl.Delegates.wglSwapLayerBuffers(hdc, (uint) fuFlags);
    }

    public static bool UseFontBitmapsA(IntPtr hDC, int first, int count, int listBase)
    {
      return Wgl.Delegates.wglUseFontBitmapsA(hDC, first, count, listBase);
    }

    public static bool UseFontBitmapsW(IntPtr hDC, int first, int count, int listBase)
    {
      return Wgl.Delegates.wglUseFontBitmapsW(hDC, first, count, listBase);
    }

    public static unsafe bool UseFontOutlinesA(IntPtr hDC, int first, int count, int listBase, float thickness, float deviation, int fontMode, GlyphMetricsFloat[] glyphMetrics)
    {
      fixed (GlyphMetricsFloat* glyphMetrics1 = glyphMetrics)
        return Wgl.Delegates.wglUseFontOutlinesA(hDC, first, count, listBase, thickness, deviation, fontMode, glyphMetrics1);
    }

    public static unsafe bool UseFontOutlinesA(IntPtr hDC, int first, int count, int listBase, float thickness, float deviation, int fontMode, ref GlyphMetricsFloat glyphMetrics)
    {
      fixed (GlyphMetricsFloat* glyphMetrics1 = &glyphMetrics)
        return Wgl.Delegates.wglUseFontOutlinesA(hDC, first, count, listBase, thickness, deviation, fontMode, glyphMetrics1);
    }

    [CLSCompliant(false)]
    public static unsafe bool UseFontOutlinesA(IntPtr hDC, int first, int count, int listBase, float thickness, float deviation, int fontMode, GlyphMetricsFloat* glyphMetrics)
    {
      return Wgl.Delegates.wglUseFontOutlinesA(hDC, first, count, listBase, thickness, deviation, fontMode, glyphMetrics);
    }

    public static unsafe bool UseFontOutlinesW(IntPtr hDC, int first, int count, int listBase, float thickness, float deviation, int fontMode, GlyphMetricsFloat[] glyphMetrics)
    {
      fixed (GlyphMetricsFloat* glyphMetrics1 = glyphMetrics)
        return Wgl.Delegates.wglUseFontOutlinesW(hDC, first, count, listBase, thickness, deviation, fontMode, glyphMetrics1);
    }

    public static unsafe bool UseFontOutlinesW(IntPtr hDC, int first, int count, int listBase, float thickness, float deviation, int fontMode, ref GlyphMetricsFloat glyphMetrics)
    {
      fixed (GlyphMetricsFloat* glyphMetrics1 = &glyphMetrics)
        return Wgl.Delegates.wglUseFontOutlinesW(hDC, first, count, listBase, thickness, deviation, fontMode, glyphMetrics1);
    }

    [CLSCompliant(false)]
    public static unsafe bool UseFontOutlinesW(IntPtr hDC, int first, int count, int listBase, float thickness, float deviation, int fontMode, GlyphMetricsFloat* glyphMetrics)
    {
      return Wgl.Delegates.wglUseFontOutlinesW(hDC, first, count, listBase, thickness, deviation, fontMode, glyphMetrics);
    }

    public static bool MakeContextCurrentEXT(IntPtr hDrawDC, IntPtr hReadDC, IntPtr hglrc)
    {
      return Wgl.Delegates.wglMakeContextCurrentEXT(hDrawDC, hReadDC, hglrc);
    }

    [CLSCompliant(false)]
    public static unsafe bool ChoosePixelFormatEXT(IntPtr hdc, int[] piAttribIList, float[] pfAttribFList, uint nMaxFormats, [Out] int[] piFormats, [Out] uint[] nNumFormats)
    {
      fixed (int* piAttribIList1 = piAttribIList)
        fixed (float* pfAttribFList1 = pfAttribFList)
          fixed (int* piFormats1 = piFormats)
            fixed (uint* nNumFormats1 = nNumFormats)
              return Wgl.Delegates.wglChoosePixelFormatEXT(hdc, piAttribIList1, pfAttribFList1, nMaxFormats, piFormats1, nNumFormats1);
    }

    public static unsafe bool ChoosePixelFormatEXT(IntPtr hdc, int[] piAttribIList, float[] pfAttribFList, int nMaxFormats, [Out] int[] piFormats, [Out] int[] nNumFormats)
    {
      fixed (int* piAttribIList1 = piAttribIList)
        fixed (float* pfAttribFList1 = pfAttribFList)
          fixed (int* piFormats1 = piFormats)
            fixed (int* numPtr = nNumFormats)
              return Wgl.Delegates.wglChoosePixelFormatEXT(hdc, piAttribIList1, pfAttribFList1, (uint) nMaxFormats, piFormats1, (uint*) numPtr);
    }

    [CLSCompliant(false)]
    public static unsafe bool ChoosePixelFormatEXT(IntPtr hdc, ref int piAttribIList, ref float pfAttribFList, uint nMaxFormats, out int piFormats, out uint nNumFormats)
    {
      fixed (int* piAttribIList1 = &piAttribIList)
        fixed (float* pfAttribFList1 = &pfAttribFList)
          fixed (int* piFormats1 = &piFormats)
            fixed (uint* nNumFormats1 = &nNumFormats)
            {
              bool flag = Wgl.Delegates.wglChoosePixelFormatEXT(hdc, piAttribIList1, pfAttribFList1, nMaxFormats, piFormats1, nNumFormats1);
              piFormats = *piFormats1;
              nNumFormats = *nNumFormats1;
              return flag;
            }
    }

    public static unsafe bool ChoosePixelFormatEXT(IntPtr hdc, ref int piAttribIList, ref float pfAttribFList, int nMaxFormats, out int piFormats, out int nNumFormats)
    {
      fixed (int* piAttribIList1 = &piAttribIList)
        fixed (float* pfAttribFList1 = &pfAttribFList)
          fixed (int* piFormats1 = &piFormats)
            fixed (int* numPtr = &nNumFormats)
            {
              bool flag = Wgl.Delegates.wglChoosePixelFormatEXT(hdc, piAttribIList1, pfAttribFList1, (uint) nMaxFormats, piFormats1, (uint*) numPtr);
              piFormats = *piFormats1;
              nNumFormats = *numPtr;
              return flag;
            }
    }

    [CLSCompliant(false)]
    public static unsafe bool ChoosePixelFormatEXT(IntPtr hdc, int* piAttribIList, float* pfAttribFList, uint nMaxFormats, [Out] int* piFormats, [Out] uint* nNumFormats)
    {
      return Wgl.Delegates.wglChoosePixelFormatEXT(hdc, piAttribIList, pfAttribFList, nMaxFormats, piFormats, nNumFormats);
    }

    [CLSCompliant(false)]
    public static unsafe bool ChoosePixelFormatEXT(IntPtr hdc, int* piAttribIList, float* pfAttribFList, int nMaxFormats, [Out] int* piFormats, [Out] int* nNumFormats)
    {
      return Wgl.Delegates.wglChoosePixelFormatEXT(hdc, piAttribIList, pfAttribFList, (uint) nMaxFormats, piFormats, (uint*) nNumFormats);
    }

    internal static class Delegates
    {
      internal static Wgl.Delegates.CreateContext wglCreateContext;
      internal static Wgl.Delegates.DeleteContext wglDeleteContext;
      internal static Wgl.Delegates.GetCurrentContext wglGetCurrentContext;
      internal static Wgl.Delegates.MakeCurrent wglMakeCurrent;
      internal static Wgl.Delegates.CopyContext wglCopyContext;
      internal static Wgl.Delegates.ChoosePixelFormat wglChoosePixelFormat;
      internal static Wgl.Delegates.DescribePixelFormat wglDescribePixelFormat;
      internal static Wgl.Delegates.GetCurrentDC wglGetCurrentDC;
      internal static Wgl.Delegates.GetDefaultProcAddress wglGetDefaultProcAddress;
      internal static Wgl.Delegates.GetProcAddress wglGetProcAddress;
      internal static Wgl.Delegates.GetPixelFormat wglGetPixelFormat;
      internal static Wgl.Delegates.SetPixelFormat wglSetPixelFormat;
      internal static Wgl.Delegates.SwapBuffers wglSwapBuffers;
      internal static Wgl.Delegates.ShareLists wglShareLists;
      internal static Wgl.Delegates.CreateLayerContext wglCreateLayerContext;
      internal static Wgl.Delegates.DescribeLayerPlane wglDescribeLayerPlane;
      internal static Wgl.Delegates.SetLayerPaletteEntries wglSetLayerPaletteEntries;
      internal static Wgl.Delegates.GetLayerPaletteEntries wglGetLayerPaletteEntries;
      internal static Wgl.Delegates.RealizeLayerPalette wglRealizeLayerPalette;
      internal static Wgl.Delegates.SwapLayerBuffers wglSwapLayerBuffers;
      internal static Wgl.Delegates.UseFontBitmapsA wglUseFontBitmapsA;
      internal static Wgl.Delegates.UseFontBitmapsW wglUseFontBitmapsW;
      internal static Wgl.Delegates.UseFontOutlinesA wglUseFontOutlinesA;
      internal static Wgl.Delegates.UseFontOutlinesW wglUseFontOutlinesW;
      internal static Wgl.Delegates.CreateContextAttribsARB wglCreateContextAttribsARB;
      internal static Wgl.Delegates.CreateBufferRegionARB wglCreateBufferRegionARB;
      internal static Wgl.Delegates.DeleteBufferRegionARB wglDeleteBufferRegionARB;
      internal static Wgl.Delegates.SaveBufferRegionARB wglSaveBufferRegionARB;
      internal static Wgl.Delegates.RestoreBufferRegionARB wglRestoreBufferRegionARB;
      internal static Wgl.Delegates.GetExtensionsStringARB wglGetExtensionsStringARB;
      internal static Wgl.Delegates.GetPixelFormatAttribivARB wglGetPixelFormatAttribivARB;
      internal static Wgl.Delegates.GetPixelFormatAttribfvARB wglGetPixelFormatAttribfvARB;
      internal static Wgl.Delegates.ChoosePixelFormatARB wglChoosePixelFormatARB;
      internal static Wgl.Delegates.MakeContextCurrentARB wglMakeContextCurrentARB;
      internal static Wgl.Delegates.GetCurrentReadDCARB wglGetCurrentReadDCARB;
      internal static Wgl.Delegates.CreatePbufferARB wglCreatePbufferARB;
      internal static Wgl.Delegates.GetPbufferDCARB wglGetPbufferDCARB;
      internal static Wgl.Delegates.ReleasePbufferDCARB wglReleasePbufferDCARB;
      internal static Wgl.Delegates.DestroyPbufferARB wglDestroyPbufferARB;
      internal static Wgl.Delegates.QueryPbufferARB wglQueryPbufferARB;
      internal static Wgl.Delegates.BindTexImageARB wglBindTexImageARB;
      internal static Wgl.Delegates.ReleaseTexImageARB wglReleaseTexImageARB;
      internal static Wgl.Delegates.SetPbufferAttribARB wglSetPbufferAttribARB;
      internal static Wgl.Delegates.CreateDisplayColorTableEXT wglCreateDisplayColorTableEXT;
      internal static Wgl.Delegates.LoadDisplayColorTableEXT wglLoadDisplayColorTableEXT;
      internal static Wgl.Delegates.BindDisplayColorTableEXT wglBindDisplayColorTableEXT;
      internal static Wgl.Delegates.DestroyDisplayColorTableEXT wglDestroyDisplayColorTableEXT;
      internal static Wgl.Delegates.GetExtensionsStringEXT wglGetExtensionsStringEXT;
      internal static Wgl.Delegates.MakeContextCurrentEXT wglMakeContextCurrentEXT;
      internal static Wgl.Delegates.GetCurrentReadDCEXT wglGetCurrentReadDCEXT;
      internal static Wgl.Delegates.CreatePbufferEXT wglCreatePbufferEXT;
      internal static Wgl.Delegates.GetPbufferDCEXT wglGetPbufferDCEXT;
      internal static Wgl.Delegates.ReleasePbufferDCEXT wglReleasePbufferDCEXT;
      internal static Wgl.Delegates.DestroyPbufferEXT wglDestroyPbufferEXT;
      internal static Wgl.Delegates.QueryPbufferEXT wglQueryPbufferEXT;
      internal static Wgl.Delegates.GetPixelFormatAttribivEXT wglGetPixelFormatAttribivEXT;
      internal static Wgl.Delegates.GetPixelFormatAttribfvEXT wglGetPixelFormatAttribfvEXT;
      internal static Wgl.Delegates.ChoosePixelFormatEXT wglChoosePixelFormatEXT;
      internal static Wgl.Delegates.SwapIntervalEXT wglSwapIntervalEXT;
      internal static Wgl.Delegates.GetSwapIntervalEXT wglGetSwapIntervalEXT;
      internal static Wgl.Delegates.AllocateMemoryNV wglAllocateMemoryNV;
      internal static Wgl.Delegates.FreeMemoryNV wglFreeMemoryNV;
      internal static Wgl.Delegates.GetSyncValuesOML wglGetSyncValuesOML;
      internal static Wgl.Delegates.GetMscRateOML wglGetMscRateOML;
      internal static Wgl.Delegates.SwapBuffersMscOML wglSwapBuffersMscOML;
      internal static Wgl.Delegates.SwapLayerBuffersMscOML wglSwapLayerBuffersMscOML;
      internal static Wgl.Delegates.WaitForMscOML wglWaitForMscOML;
      internal static Wgl.Delegates.WaitForSbcOML wglWaitForSbcOML;
      internal static Wgl.Delegates.GetDigitalVideoParametersI3D wglGetDigitalVideoParametersI3D;
      internal static Wgl.Delegates.SetDigitalVideoParametersI3D wglSetDigitalVideoParametersI3D;
      internal static Wgl.Delegates.GetGammaTableParametersI3D wglGetGammaTableParametersI3D;
      internal static Wgl.Delegates.SetGammaTableParametersI3D wglSetGammaTableParametersI3D;
      internal static Wgl.Delegates.GetGammaTableI3D wglGetGammaTableI3D;
      internal static Wgl.Delegates.SetGammaTableI3D wglSetGammaTableI3D;
      internal static Wgl.Delegates.EnableGenlockI3D wglEnableGenlockI3D;
      internal static Wgl.Delegates.DisableGenlockI3D wglDisableGenlockI3D;
      internal static Wgl.Delegates.IsEnabledGenlockI3D wglIsEnabledGenlockI3D;
      internal static Wgl.Delegates.GenlockSourceI3D wglGenlockSourceI3D;
      internal static Wgl.Delegates.GetGenlockSourceI3D wglGetGenlockSourceI3D;
      internal static Wgl.Delegates.GenlockSourceEdgeI3D wglGenlockSourceEdgeI3D;
      internal static Wgl.Delegates.GetGenlockSourceEdgeI3D wglGetGenlockSourceEdgeI3D;
      internal static Wgl.Delegates.GenlockSampleRateI3D wglGenlockSampleRateI3D;
      internal static Wgl.Delegates.GetGenlockSampleRateI3D wglGetGenlockSampleRateI3D;
      internal static Wgl.Delegates.GenlockSourceDelayI3D wglGenlockSourceDelayI3D;
      internal static Wgl.Delegates.GetGenlockSourceDelayI3D wglGetGenlockSourceDelayI3D;
      internal static Wgl.Delegates.QueryGenlockMaxSourceDelayI3D wglQueryGenlockMaxSourceDelayI3D;
      internal static Wgl.Delegates.CreateImageBufferI3D wglCreateImageBufferI3D;
      internal static Wgl.Delegates.DestroyImageBufferI3D wglDestroyImageBufferI3D;
      internal static Wgl.Delegates.AssociateImageBufferEventsI3D wglAssociateImageBufferEventsI3D;
      internal static Wgl.Delegates.ReleaseImageBufferEventsI3D wglReleaseImageBufferEventsI3D;
      internal static Wgl.Delegates.EnableFrameLockI3D wglEnableFrameLockI3D;
      internal static Wgl.Delegates.DisableFrameLockI3D wglDisableFrameLockI3D;
      internal static Wgl.Delegates.IsEnabledFrameLockI3D wglIsEnabledFrameLockI3D;
      internal static Wgl.Delegates.QueryFrameLockMasterI3D wglQueryFrameLockMasterI3D;
      internal static Wgl.Delegates.GetFrameUsageI3D wglGetFrameUsageI3D;
      internal static Wgl.Delegates.BeginFrameTrackingI3D wglBeginFrameTrackingI3D;
      internal static Wgl.Delegates.EndFrameTrackingI3D wglEndFrameTrackingI3D;
      internal static Wgl.Delegates.QueryFrameTrackingI3D wglQueryFrameTrackingI3D;

      [SuppressUnmanagedCodeSecurity]
      internal delegate IntPtr CreateContext(IntPtr hDc);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool DeleteContext(IntPtr oldContext);

      [SuppressUnmanagedCodeSecurity]
      internal delegate IntPtr GetCurrentContext();

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool MakeCurrent(IntPtr hDc, IntPtr newContext);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool CopyContext(IntPtr hglrcSrc, IntPtr hglrcDst, uint mask);

      [SuppressUnmanagedCodeSecurity]
      internal delegate int ChoosePixelFormat(IntPtr hDc, PixelFormatDescriptor* pPfd);

      [SuppressUnmanagedCodeSecurity]
      internal delegate int DescribePixelFormat(IntPtr hdc, int ipfd, uint cjpfd, PixelFormatDescriptor* ppfd);

      [SuppressUnmanagedCodeSecurity]
      internal delegate IntPtr GetCurrentDC();

      [SuppressUnmanagedCodeSecurity]
      internal delegate IntPtr GetDefaultProcAddress(string lpszProc);

      [SuppressUnmanagedCodeSecurity]
      internal delegate IntPtr GetProcAddress(string lpszProc);

      [SuppressUnmanagedCodeSecurity]
      internal delegate int GetPixelFormat(IntPtr hdc);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool SetPixelFormat(IntPtr hdc, int ipfd, PixelFormatDescriptor* ppfd);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool SwapBuffers(IntPtr hdc);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool ShareLists(IntPtr hrcSrvShare, IntPtr hrcSrvSource);

      [SuppressUnmanagedCodeSecurity]
      internal delegate IntPtr CreateLayerContext(IntPtr hDc, int level);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool DescribeLayerPlane(IntPtr hDc, int pixelFormat, int layerPlane, uint nBytes, LayerPlaneDescriptor* plpd);

      [SuppressUnmanagedCodeSecurity]
      internal delegate int SetLayerPaletteEntries(IntPtr hdc, int iLayerPlane, int iStart, int cEntries, int* pcr);

      [SuppressUnmanagedCodeSecurity]
      internal delegate int GetLayerPaletteEntries(IntPtr hdc, int iLayerPlane, int iStart, int cEntries, int* pcr);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool RealizeLayerPalette(IntPtr hdc, int iLayerPlane, bool bRealize);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool SwapLayerBuffers(IntPtr hdc, uint fuFlags);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool UseFontBitmapsA(IntPtr hDC, int first, int count, int listBase);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool UseFontBitmapsW(IntPtr hDC, int first, int count, int listBase);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool UseFontOutlinesA(IntPtr hDC, int first, int count, int listBase, float thickness, float deviation, int fontMode, GlyphMetricsFloat* glyphMetrics);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool UseFontOutlinesW(IntPtr hDC, int first, int count, int listBase, float thickness, float deviation, int fontMode, GlyphMetricsFloat* glyphMetrics);

      [SuppressUnmanagedCodeSecurity]
      internal delegate IntPtr CreateContextAttribsARB(IntPtr hDC, IntPtr hShareContext, int* attribList);

      [SuppressUnmanagedCodeSecurity]
      internal delegate IntPtr CreateBufferRegionARB(IntPtr hDC, int iLayerPlane, uint uType);

      [SuppressUnmanagedCodeSecurity]
      internal delegate void DeleteBufferRegionARB(IntPtr hRegion);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool SaveBufferRegionARB(IntPtr hRegion, int x, int y, int width, int height);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool RestoreBufferRegionARB(IntPtr hRegion, int x, int y, int width, int height, int xSrc, int ySrc);

      [SuppressUnmanagedCodeSecurity]
      internal delegate IntPtr GetExtensionsStringARB(IntPtr hdc);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool GetPixelFormatAttribivARB(IntPtr hdc, int iPixelFormat, int iLayerPlane, uint nAttributes, int* piAttributes, [Out] int* piValues);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool GetPixelFormatAttribfvARB(IntPtr hdc, int iPixelFormat, int iLayerPlane, uint nAttributes, int* piAttributes, [Out] float* pfValues);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool ChoosePixelFormatARB(IntPtr hdc, int* piAttribIList, float* pfAttribFList, uint nMaxFormats, [Out] int* piFormats, [Out] uint* nNumFormats);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool MakeContextCurrentARB(IntPtr hDrawDC, IntPtr hReadDC, IntPtr hglrc);

      [SuppressUnmanagedCodeSecurity]
      internal delegate IntPtr GetCurrentReadDCARB();

      [SuppressUnmanagedCodeSecurity]
      internal delegate IntPtr CreatePbufferARB(IntPtr hDC, int iPixelFormat, int iWidth, int iHeight, int* piAttribList);

      [SuppressUnmanagedCodeSecurity]
      internal delegate IntPtr GetPbufferDCARB(IntPtr hPbuffer);

      [SuppressUnmanagedCodeSecurity]
      internal delegate int ReleasePbufferDCARB(IntPtr hPbuffer, IntPtr hDC);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool DestroyPbufferARB(IntPtr hPbuffer);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool QueryPbufferARB(IntPtr hPbuffer, int iAttribute, [Out] int* piValue);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool BindTexImageARB(IntPtr hPbuffer, int iBuffer);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool ReleaseTexImageARB(IntPtr hPbuffer, int iBuffer);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool SetPbufferAttribARB(IntPtr hPbuffer, int* piAttribList);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool CreateDisplayColorTableEXT(ushort id);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool LoadDisplayColorTableEXT(ushort* table, uint length);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool BindDisplayColorTableEXT(ushort id);

      [SuppressUnmanagedCodeSecurity]
      internal delegate void DestroyDisplayColorTableEXT(ushort id);

      [SuppressUnmanagedCodeSecurity]
      internal delegate IntPtr GetExtensionsStringEXT();

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool MakeContextCurrentEXT(IntPtr hDrawDC, IntPtr hReadDC, IntPtr hglrc);

      [SuppressUnmanagedCodeSecurity]
      internal delegate IntPtr GetCurrentReadDCEXT();

      [SuppressUnmanagedCodeSecurity]
      internal delegate IntPtr CreatePbufferEXT(IntPtr hDC, int iPixelFormat, int iWidth, int iHeight, int* piAttribList);

      [SuppressUnmanagedCodeSecurity]
      internal delegate IntPtr GetPbufferDCEXT(IntPtr hPbuffer);

      [SuppressUnmanagedCodeSecurity]
      internal delegate int ReleasePbufferDCEXT(IntPtr hPbuffer, IntPtr hDC);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool DestroyPbufferEXT(IntPtr hPbuffer);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool QueryPbufferEXT(IntPtr hPbuffer, int iAttribute, [Out] int* piValue);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool GetPixelFormatAttribivEXT(IntPtr hdc, int iPixelFormat, int iLayerPlane, uint nAttributes, [Out] int* piAttributes, [Out] int* piValues);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool GetPixelFormatAttribfvEXT(IntPtr hdc, int iPixelFormat, int iLayerPlane, uint nAttributes, [Out] int* piAttributes, [Out] float* pfValues);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool ChoosePixelFormatEXT(IntPtr hdc, int* piAttribIList, float* pfAttribFList, uint nMaxFormats, [Out] int* piFormats, [Out] uint* nNumFormats);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool SwapIntervalEXT(int interval);

      [SuppressUnmanagedCodeSecurity]
      internal delegate int GetSwapIntervalEXT();

      [SuppressUnmanagedCodeSecurity]
      internal delegate IntPtr AllocateMemoryNV(int size, float readfreq, float writefreq, float priority);

      [SuppressUnmanagedCodeSecurity]
      internal delegate void FreeMemoryNV([Out] IntPtr pointer);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool GetSyncValuesOML(IntPtr hdc, [Out] long* ust, [Out] long* msc, [Out] long* sbc);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool GetMscRateOML(IntPtr hdc, [Out] int* numerator, [Out] int* denominator);

      [SuppressUnmanagedCodeSecurity]
      internal delegate long SwapBuffersMscOML(IntPtr hdc, long target_msc, long divisor, long remainder);

      [SuppressUnmanagedCodeSecurity]
      internal delegate long SwapLayerBuffersMscOML(IntPtr hdc, int fuPlanes, long target_msc, long divisor, long remainder);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool WaitForMscOML(IntPtr hdc, long target_msc, long divisor, long remainder, [Out] long* ust, [Out] long* msc, [Out] long* sbc);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool WaitForSbcOML(IntPtr hdc, long target_sbc, [Out] long* ust, [Out] long* msc, [Out] long* sbc);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool GetDigitalVideoParametersI3D(IntPtr hDC, int iAttribute, [Out] int* piValue);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool SetDigitalVideoParametersI3D(IntPtr hDC, int iAttribute, int* piValue);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool GetGammaTableParametersI3D(IntPtr hDC, int iAttribute, [Out] int* piValue);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool SetGammaTableParametersI3D(IntPtr hDC, int iAttribute, int* piValue);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool GetGammaTableI3D(IntPtr hDC, int iEntries, [Out] ushort* puRed, [Out] ushort* puGreen, [Out] ushort* puBlue);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool SetGammaTableI3D(IntPtr hDC, int iEntries, ushort* puRed, ushort* puGreen, ushort* puBlue);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool EnableGenlockI3D(IntPtr hDC);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool DisableGenlockI3D(IntPtr hDC);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool IsEnabledGenlockI3D(IntPtr hDC, [Out] bool* pFlag);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool GenlockSourceI3D(IntPtr hDC, uint uSource);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool GetGenlockSourceI3D(IntPtr hDC, [Out] uint* uSource);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool GenlockSourceEdgeI3D(IntPtr hDC, uint uEdge);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool GetGenlockSourceEdgeI3D(IntPtr hDC, [Out] uint* uEdge);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool GenlockSampleRateI3D(IntPtr hDC, uint uRate);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool GetGenlockSampleRateI3D(IntPtr hDC, [Out] uint* uRate);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool GenlockSourceDelayI3D(IntPtr hDC, uint uDelay);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool GetGenlockSourceDelayI3D(IntPtr hDC, [Out] uint* uDelay);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool QueryGenlockMaxSourceDelayI3D(IntPtr hDC, [Out] uint* uMaxLineDelay, [Out] uint* uMaxPixelDelay);

      [SuppressUnmanagedCodeSecurity]
      internal delegate IntPtr CreateImageBufferI3D(IntPtr hDC, int dwSize, uint uFlags);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool DestroyImageBufferI3D(IntPtr hDC, IntPtr pAddress);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool AssociateImageBufferEventsI3D(IntPtr hDC, IntPtr* pEvent, IntPtr pAddress, int* pSize, uint count);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool ReleaseImageBufferEventsI3D(IntPtr hDC, IntPtr pAddress, uint count);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool EnableFrameLockI3D();

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool DisableFrameLockI3D();

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool IsEnabledFrameLockI3D([Out] bool* pFlag);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool QueryFrameLockMasterI3D([Out] bool* pFlag);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool GetFrameUsageI3D([Out] float* pUsage);

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool BeginFrameTrackingI3D();

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool EndFrameTrackingI3D();

      [SuppressUnmanagedCodeSecurity]
      internal delegate bool QueryFrameTrackingI3D([Out] int* pFrameCount, [Out] int* pMissedFrames, [Out] float* pLastMissedUsage);
    }

    public static class Arb
    {
      public static unsafe bool SupportsExtension(WinGLContext context, string ext)
      {
        Wgl.Delegates.GetExtensionsStringARB extensionsStringArb = Wgl.Delegates.wglGetExtensionsStringARB;
        if (extensionsStringArb != null)
        {
          string[] strArray = new string((sbyte*) (void*) extensionsStringArb(context.DeviceContext)).Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
          if (strArray == null || strArray.Length == 0)
            return false;
          foreach (string str in strArray)
          {
            if (str == ext)
              return true;
          }
        }
        return false;
      }

      [CLSCompliant(false)]
      public static unsafe IntPtr CreateContextAttribs(IntPtr hDC, IntPtr hShareContext, int* attribList)
      {
        return Wgl.Delegates.wglCreateContextAttribsARB(hDC, hShareContext, attribList);
      }

      public static unsafe IntPtr CreateContextAttribs(IntPtr hDC, IntPtr hShareContext, ref int attribList)
      {
        fixed (int* attribList1 = &attribList)
          return Wgl.Delegates.wglCreateContextAttribsARB(hDC, hShareContext, attribList1);
      }

      public static unsafe IntPtr CreateContextAttribs(IntPtr hDC, IntPtr hShareContext, int[] attribList)
      {
        fixed (int* attribList1 = attribList)
          return Wgl.Delegates.wglCreateContextAttribsARB(hDC, hShareContext, attribList1);
      }

      [CLSCompliant(false)]
      public static IntPtr CreateBufferRegion(IntPtr hDC, int iLayerPlane, uint uType)
      {
        return Wgl.Delegates.wglCreateBufferRegionARB(hDC, iLayerPlane, uType);
      }

      public static IntPtr CreateBufferRegion(IntPtr hDC, int iLayerPlane, int uType)
      {
        return Wgl.Delegates.wglCreateBufferRegionARB(hDC, iLayerPlane, (uint) uType);
      }

      public static void DeleteBufferRegion(IntPtr hRegion)
      {
        Wgl.Delegates.wglDeleteBufferRegionARB(hRegion);
      }

      public static bool SaveBufferRegion(IntPtr hRegion, int x, int y, int width, int height)
      {
        return Wgl.Delegates.wglSaveBufferRegionARB(hRegion, x, y, width, height);
      }

      public static bool RestoreBufferRegion(IntPtr hRegion, int x, int y, int width, int height, int xSrc, int ySrc)
      {
        return Wgl.Delegates.wglRestoreBufferRegionARB(hRegion, x, y, width, height, xSrc, ySrc);
      }

      public static string GetExtensionsString(IntPtr hdc)
      {
        return Marshal.PtrToStringAnsi(Wgl.Delegates.wglGetExtensionsStringARB(hdc));
      }

      [CLSCompliant(false)]
      public static unsafe bool GetPixelFormatAttrib(IntPtr hdc, int iPixelFormat, int iLayerPlane, uint nAttributes, int[] piAttributes, [Out] int[] piValues)
      {
        fixed (int* piAttributes1 = piAttributes)
          fixed (int* piValues1 = piValues)
            return Wgl.Delegates.wglGetPixelFormatAttribivARB(hdc, iPixelFormat, iLayerPlane, nAttributes, piAttributes1, piValues1);
      }

      public static unsafe bool GetPixelFormatAttrib(IntPtr hdc, int iPixelFormat, int iLayerPlane, int nAttributes, int[] piAttributes, [Out] int[] piValues)
      {
        fixed (int* piAttributes1 = piAttributes)
          fixed (int* piValues1 = piValues)
            return Wgl.Delegates.wglGetPixelFormatAttribivARB(hdc, iPixelFormat, iLayerPlane, (uint) nAttributes, piAttributes1, piValues1);
      }

      [CLSCompliant(false)]
      public static unsafe bool GetPixelFormatAttrib(IntPtr hdc, int iPixelFormat, int iLayerPlane, uint nAttributes, ref int piAttributes, out int piValues)
      {
        fixed (int* piAttributes1 = &piAttributes)
          fixed (int* piValues1 = &piValues)
          {
            bool flag = Wgl.Delegates.wglGetPixelFormatAttribivARB(hdc, iPixelFormat, iLayerPlane, nAttributes, piAttributes1, piValues1);
            piValues = *piValues1;
            return flag;
          }
      }

      public static unsafe bool GetPixelFormatAttrib(IntPtr hdc, int iPixelFormat, int iLayerPlane, int nAttributes, ref int piAttributes, out int piValues)
      {
        fixed (int* piAttributes1 = &piAttributes)
          fixed (int* piValues1 = &piValues)
          {
            bool flag = Wgl.Delegates.wglGetPixelFormatAttribivARB(hdc, iPixelFormat, iLayerPlane, (uint) nAttributes, piAttributes1, piValues1);
            piValues = *piValues1;
            return flag;
          }
      }

      [CLSCompliant(false)]
      public static unsafe bool GetPixelFormatAttrib(IntPtr hdc, int iPixelFormat, int iLayerPlane, uint nAttributes, int* piAttributes, [Out] int* piValues)
      {
        return Wgl.Delegates.wglGetPixelFormatAttribivARB(hdc, iPixelFormat, iLayerPlane, nAttributes, piAttributes, piValues);
      }

      [CLSCompliant(false)]
      public static unsafe bool GetPixelFormatAttrib(IntPtr hdc, int iPixelFormat, int iLayerPlane, int nAttributes, int* piAttributes, [Out] int* piValues)
      {
        return Wgl.Delegates.wglGetPixelFormatAttribivARB(hdc, iPixelFormat, iLayerPlane, (uint) nAttributes, piAttributes, piValues);
      }

      [CLSCompliant(false)]
      public static unsafe bool GetPixelFormatAttrib(IntPtr hdc, int iPixelFormat, int iLayerPlane, uint nAttributes, int[] piAttributes, [Out] float[] pfValues)
      {
        fixed (int* piAttributes1 = piAttributes)
          fixed (float* pfValues1 = pfValues)
            return Wgl.Delegates.wglGetPixelFormatAttribfvARB(hdc, iPixelFormat, iLayerPlane, nAttributes, piAttributes1, pfValues1);
      }

      public static unsafe bool GetPixelFormatAttrib(IntPtr hdc, int iPixelFormat, int iLayerPlane, int nAttributes, int[] piAttributes, [Out] float[] pfValues)
      {
        fixed (int* piAttributes1 = piAttributes)
          fixed (float* pfValues1 = pfValues)
            return Wgl.Delegates.wglGetPixelFormatAttribfvARB(hdc, iPixelFormat, iLayerPlane, (uint) nAttributes, piAttributes1, pfValues1);
      }

      [CLSCompliant(false)]
      public static unsafe bool GetPixelFormatAttrib(IntPtr hdc, int iPixelFormat, int iLayerPlane, uint nAttributes, ref int piAttributes, out float pfValues)
      {
        fixed (int* piAttributes1 = &piAttributes)
          fixed (float* pfValues1 = &pfValues)
          {
            bool flag = Wgl.Delegates.wglGetPixelFormatAttribfvARB(hdc, iPixelFormat, iLayerPlane, nAttributes, piAttributes1, pfValues1);
            pfValues = *pfValues1;
            return flag;
          }
      }

      public static unsafe bool GetPixelFormatAttrib(IntPtr hdc, int iPixelFormat, int iLayerPlane, int nAttributes, ref int piAttributes, out float pfValues)
      {
        fixed (int* piAttributes1 = &piAttributes)
          fixed (float* pfValues1 = &pfValues)
          {
            bool flag = Wgl.Delegates.wglGetPixelFormatAttribfvARB(hdc, iPixelFormat, iLayerPlane, (uint) nAttributes, piAttributes1, pfValues1);
            pfValues = *pfValues1;
            return flag;
          }
      }

      [CLSCompliant(false)]
      public static unsafe bool GetPixelFormatAttrib(IntPtr hdc, int iPixelFormat, int iLayerPlane, uint nAttributes, int* piAttributes, [Out] float* pfValues)
      {
        return Wgl.Delegates.wglGetPixelFormatAttribfvARB(hdc, iPixelFormat, iLayerPlane, nAttributes, piAttributes, pfValues);
      }

      [CLSCompliant(false)]
      public static unsafe bool GetPixelFormatAttrib(IntPtr hdc, int iPixelFormat, int iLayerPlane, int nAttributes, int* piAttributes, [Out] float* pfValues)
      {
        return Wgl.Delegates.wglGetPixelFormatAttribfvARB(hdc, iPixelFormat, iLayerPlane, (uint) nAttributes, piAttributes, pfValues);
      }

      [CLSCompliant(false)]
      public static unsafe bool ChoosePixelFormat(IntPtr hdc, int[] piAttribIList, float[] pfAttribFList, uint nMaxFormats, [Out] int[] piFormats, [Out] uint[] nNumFormats)
      {
        fixed (int* piAttribIList1 = piAttribIList)
          fixed (float* pfAttribFList1 = pfAttribFList)
            fixed (int* piFormats1 = piFormats)
              fixed (uint* nNumFormats1 = nNumFormats)
                return Wgl.Delegates.wglChoosePixelFormatARB(hdc, piAttribIList1, pfAttribFList1, nMaxFormats, piFormats1, nNumFormats1);
      }

      public static unsafe bool ChoosePixelFormat(IntPtr hdc, int[] piAttribIList, float[] pfAttribFList, int nMaxFormats, [Out] int[] piFormats, [Out] int[] nNumFormats)
      {
        fixed (int* piAttribIList1 = piAttribIList)
          fixed (float* pfAttribFList1 = pfAttribFList)
            fixed (int* piFormats1 = piFormats)
              fixed (int* numPtr = nNumFormats)
                return Wgl.Delegates.wglChoosePixelFormatARB(hdc, piAttribIList1, pfAttribFList1, (uint) nMaxFormats, piFormats1, (uint*) numPtr);
      }

      [CLSCompliant(false)]
      public static unsafe bool ChoosePixelFormat(IntPtr hdc, ref int piAttribIList, ref float pfAttribFList, uint nMaxFormats, out int piFormats, out uint nNumFormats)
      {
        fixed (int* piAttribIList1 = &piAttribIList)
          fixed (float* pfAttribFList1 = &pfAttribFList)
            fixed (int* piFormats1 = &piFormats)
              fixed (uint* nNumFormats1 = &nNumFormats)
              {
                bool flag = Wgl.Delegates.wglChoosePixelFormatARB(hdc, piAttribIList1, pfAttribFList1, nMaxFormats, piFormats1, nNumFormats1);
                piFormats = *piFormats1;
                nNumFormats = *nNumFormats1;
                return flag;
              }
      }

      public static unsafe bool ChoosePixelFormat(IntPtr hdc, ref int piAttribIList, ref float pfAttribFList, int nMaxFormats, out int piFormats, out int nNumFormats)
      {
        fixed (int* piAttribIList1 = &piAttribIList)
          fixed (float* pfAttribFList1 = &pfAttribFList)
            fixed (int* piFormats1 = &piFormats)
              fixed (int* numPtr = &nNumFormats)
              {
                bool flag = Wgl.Delegates.wglChoosePixelFormatARB(hdc, piAttribIList1, pfAttribFList1, (uint) nMaxFormats, piFormats1, (uint*) numPtr);
                piFormats = *piFormats1;
                nNumFormats = *numPtr;
                return flag;
              }
      }

      [CLSCompliant(false)]
      public static unsafe bool ChoosePixelFormat(IntPtr hdc, int* piAttribIList, float* pfAttribFList, uint nMaxFormats, [Out] int* piFormats, [Out] uint* nNumFormats)
      {
        return Wgl.Delegates.wglChoosePixelFormatARB(hdc, piAttribIList, pfAttribFList, nMaxFormats, piFormats, nNumFormats);
      }

      [CLSCompliant(false)]
      public static unsafe bool ChoosePixelFormat(IntPtr hdc, int* piAttribIList, float* pfAttribFList, int nMaxFormats, [Out] int* piFormats, [Out] int* nNumFormats)
      {
        return Wgl.Delegates.wglChoosePixelFormatARB(hdc, piAttribIList, pfAttribFList, (uint) nMaxFormats, piFormats, (uint*) nNumFormats);
      }

      public static bool MakeContextCurrent(IntPtr hDrawDC, IntPtr hReadDC, IntPtr hglrc)
      {
        return Wgl.Delegates.wglMakeContextCurrentARB(hDrawDC, hReadDC, hglrc);
      }

      public static IntPtr GetCurrentReadDC()
      {
        return Wgl.Delegates.wglGetCurrentReadDCARB();
      }

      public static unsafe IntPtr CreatePbuffer(IntPtr hDC, int iPixelFormat, int iWidth, int iHeight, int[] piAttribList)
      {
        fixed (int* piAttribList1 = piAttribList)
          return Wgl.Delegates.wglCreatePbufferARB(hDC, iPixelFormat, iWidth, iHeight, piAttribList1);
      }

      public static unsafe IntPtr CreatePbuffer(IntPtr hDC, int iPixelFormat, int iWidth, int iHeight, ref int piAttribList)
      {
        fixed (int* piAttribList1 = &piAttribList)
          return Wgl.Delegates.wglCreatePbufferARB(hDC, iPixelFormat, iWidth, iHeight, piAttribList1);
      }

      [CLSCompliant(false)]
      public static unsafe IntPtr CreatePbuffer(IntPtr hDC, int iPixelFormat, int iWidth, int iHeight, int* piAttribList)
      {
        return Wgl.Delegates.wglCreatePbufferARB(hDC, iPixelFormat, iWidth, iHeight, piAttribList);
      }

      public static IntPtr GetPbufferDC(IntPtr hPbuffer)
      {
        return Wgl.Delegates.wglGetPbufferDCARB(hPbuffer);
      }

      public static int ReleasePbufferDC(IntPtr hPbuffer, IntPtr hDC)
      {
        return Wgl.Delegates.wglReleasePbufferDCARB(hPbuffer, hDC);
      }

      public static bool DestroyPbuffer(IntPtr hPbuffer)
      {
        return Wgl.Delegates.wglDestroyPbufferARB(hPbuffer);
      }

      public static unsafe bool QueryPbuffer(IntPtr hPbuffer, int iAttribute, [Out] int[] piValue)
      {
        fixed (int* piValue1 = piValue)
          return Wgl.Delegates.wglQueryPbufferARB(hPbuffer, iAttribute, piValue1);
      }

      public static unsafe bool QueryPbuffer(IntPtr hPbuffer, int iAttribute, out int piValue)
      {
        fixed (int* piValue1 = &piValue)
        {
          bool flag = Wgl.Delegates.wglQueryPbufferARB(hPbuffer, iAttribute, piValue1);
          piValue = *piValue1;
          return flag;
        }
      }

      [CLSCompliant(false)]
      public static unsafe bool QueryPbuffer(IntPtr hPbuffer, int iAttribute, [Out] int* piValue)
      {
        return Wgl.Delegates.wglQueryPbufferARB(hPbuffer, iAttribute, piValue);
      }

      public static bool BindTexImage(IntPtr hPbuffer, int iBuffer)
      {
        return Wgl.Delegates.wglBindTexImageARB(hPbuffer, iBuffer);
      }

      public static bool ReleaseTexImage(IntPtr hPbuffer, int iBuffer)
      {
        return Wgl.Delegates.wglReleaseTexImageARB(hPbuffer, iBuffer);
      }

      public static unsafe bool SetPbufferAttrib(IntPtr hPbuffer, int[] piAttribList)
      {
        fixed (int* piAttribList1 = piAttribList)
          return Wgl.Delegates.wglSetPbufferAttribARB(hPbuffer, piAttribList1);
      }

      public static unsafe bool SetPbufferAttrib(IntPtr hPbuffer, ref int piAttribList)
      {
        fixed (int* piAttribList1 = &piAttribList)
          return Wgl.Delegates.wglSetPbufferAttribARB(hPbuffer, piAttribList1);
      }

      [CLSCompliant(false)]
      public static unsafe bool SetPbufferAttrib(IntPtr hPbuffer, int* piAttribList)
      {
        return Wgl.Delegates.wglSetPbufferAttribARB(hPbuffer, piAttribList);
      }
    }

    public static class Ext
    {
      private static string[] extensions;

      public static bool SupportsExtension(string ext)
      {
        if (Wgl.Delegates.wglGetExtensionsStringEXT == null)
          return false;
        if (Wgl.Ext.extensions == null || Wgl.rebuildExtensionList)
        {
          Wgl.Ext.extensions = Wgl.Ext.GetExtensionsString().Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
          Array.Sort<string>(Wgl.Ext.extensions);
          Wgl.rebuildExtensionList = false;
        }
        return Array.BinarySearch<string>(Wgl.Ext.extensions, ext) != -1;
      }

      [CLSCompliant(false)]
      public static bool CreateDisplayColorTable(ushort id)
      {
        return Wgl.Delegates.wglCreateDisplayColorTableEXT(id);
      }

      public static bool CreateDisplayColorTable(short id)
      {
        return Wgl.Delegates.wglCreateDisplayColorTableEXT((ushort) id);
      }

      [CLSCompliant(false)]
      public static unsafe bool LoadDisplayColorTable(ushort[] table, uint length)
      {
        fixed (ushort* table1 = table)
          return Wgl.Delegates.wglLoadDisplayColorTableEXT(table1, length);
      }

      public static unsafe bool LoadDisplayColorTable(short[] table, int length)
      {
        fixed (short* numPtr = table)
          return Wgl.Delegates.wglLoadDisplayColorTableEXT((ushort*) numPtr, (uint) length);
      }

      [CLSCompliant(false)]
      public static unsafe bool LoadDisplayColorTable(ref ushort table, uint length)
      {
        fixed (ushort* table1 = &table)
          return Wgl.Delegates.wglLoadDisplayColorTableEXT(table1, length);
      }

      public static unsafe bool LoadDisplayColorTable(ref short table, int length)
      {
        fixed (short* numPtr = &table)
          return Wgl.Delegates.wglLoadDisplayColorTableEXT((ushort*) numPtr, (uint) length);
      }

      [CLSCompliant(false)]
      public static unsafe bool LoadDisplayColorTable(ushort* table, uint length)
      {
        return Wgl.Delegates.wglLoadDisplayColorTableEXT(table, length);
      }

      [CLSCompliant(false)]
      public static unsafe bool LoadDisplayColorTable(short* table, int length)
      {
        return Wgl.Delegates.wglLoadDisplayColorTableEXT((ushort*) table, (uint) length);
      }

      [CLSCompliant(false)]
      public static bool BindDisplayColorTable(ushort id)
      {
        return Wgl.Delegates.wglBindDisplayColorTableEXT(id);
      }

      public static bool BindDisplayColorTable(short id)
      {
        return Wgl.Delegates.wglBindDisplayColorTableEXT((ushort) id);
      }

      [CLSCompliant(false)]
      public static void DestroyDisplayColorTable(ushort id)
      {
        Wgl.Delegates.wglDestroyDisplayColorTableEXT(id);
      }

      public static void DestroyDisplayColorTable(short id)
      {
        Wgl.Delegates.wglDestroyDisplayColorTableEXT((ushort) id);
      }

      public static string GetExtensionsString()
      {
        return Marshal.PtrToStringAnsi(Wgl.Delegates.wglGetExtensionsStringEXT());
      }

      public static IntPtr GetCurrentReadDC()
      {
        return Wgl.Delegates.wglGetCurrentReadDCEXT();
      }

      public static unsafe IntPtr CreatePbuffer(IntPtr hDC, int iPixelFormat, int iWidth, int iHeight, int[] piAttribList)
      {
        fixed (int* piAttribList1 = piAttribList)
          return Wgl.Delegates.wglCreatePbufferEXT(hDC, iPixelFormat, iWidth, iHeight, piAttribList1);
      }

      public static unsafe IntPtr CreatePbuffer(IntPtr hDC, int iPixelFormat, int iWidth, int iHeight, ref int piAttribList)
      {
        fixed (int* piAttribList1 = &piAttribList)
          return Wgl.Delegates.wglCreatePbufferEXT(hDC, iPixelFormat, iWidth, iHeight, piAttribList1);
      }

      [CLSCompliant(false)]
      public static unsafe IntPtr CreatePbuffer(IntPtr hDC, int iPixelFormat, int iWidth, int iHeight, int* piAttribList)
      {
        return Wgl.Delegates.wglCreatePbufferEXT(hDC, iPixelFormat, iWidth, iHeight, piAttribList);
      }

      public static IntPtr GetPbufferDC(IntPtr hPbuffer)
      {
        return Wgl.Delegates.wglGetPbufferDCEXT(hPbuffer);
      }

      public static int ReleasePbufferDC(IntPtr hPbuffer, IntPtr hDC)
      {
        return Wgl.Delegates.wglReleasePbufferDCEXT(hPbuffer, hDC);
      }

      public static bool DestroyPbuffer(IntPtr hPbuffer)
      {
        return Wgl.Delegates.wglDestroyPbufferEXT(hPbuffer);
      }

      public static unsafe bool QueryPbuffer(IntPtr hPbuffer, int iAttribute, [Out] int[] piValue)
      {
        fixed (int* piValue1 = piValue)
          return Wgl.Delegates.wglQueryPbufferEXT(hPbuffer, iAttribute, piValue1);
      }

      public static unsafe bool QueryPbuffer(IntPtr hPbuffer, int iAttribute, out int piValue)
      {
        fixed (int* piValue1 = &piValue)
        {
          bool flag = Wgl.Delegates.wglQueryPbufferEXT(hPbuffer, iAttribute, piValue1);
          piValue = *piValue1;
          return flag;
        }
      }

      [CLSCompliant(false)]
      public static unsafe bool QueryPbuffer(IntPtr hPbuffer, int iAttribute, [Out] int* piValue)
      {
        return Wgl.Delegates.wglQueryPbufferEXT(hPbuffer, iAttribute, piValue);
      }

      [CLSCompliant(false)]
      public static unsafe bool GetPixelFormatAttrib(IntPtr hdc, int iPixelFormat, int iLayerPlane, uint nAttributes, [Out] int[] piAttributes, [Out] int[] piValues)
      {
        fixed (int* piAttributes1 = piAttributes)
          fixed (int* piValues1 = piValues)
            return Wgl.Delegates.wglGetPixelFormatAttribivEXT(hdc, iPixelFormat, iLayerPlane, nAttributes, piAttributes1, piValues1);
      }

      public static unsafe bool GetPixelFormatAttrib(IntPtr hdc, int iPixelFormat, int iLayerPlane, int nAttributes, [Out] int[] piAttributes, [Out] int[] piValues)
      {
        fixed (int* piAttributes1 = piAttributes)
          fixed (int* piValues1 = piValues)
            return Wgl.Delegates.wglGetPixelFormatAttribivEXT(hdc, iPixelFormat, iLayerPlane, (uint) nAttributes, piAttributes1, piValues1);
      }

      [CLSCompliant(false)]
      public static unsafe bool GetPixelFormatAttrib(IntPtr hdc, int iPixelFormat, int iLayerPlane, uint nAttributes, out int piAttributes, out int piValues)
      {
        fixed (int* piAttributes1 = &piAttributes)
          fixed (int* piValues1 = &piValues)
          {
            bool flag = Wgl.Delegates.wglGetPixelFormatAttribivEXT(hdc, iPixelFormat, iLayerPlane, nAttributes, piAttributes1, piValues1);
            piAttributes = *piAttributes1;
            piValues = *piValues1;
            return flag;
          }
      }

      public static unsafe bool GetPixelFormatAttrib(IntPtr hdc, int iPixelFormat, int iLayerPlane, int nAttributes, out int piAttributes, out int piValues)
      {
        fixed (int* piAttributes1 = &piAttributes)
          fixed (int* piValues1 = &piValues)
          {
            bool flag = Wgl.Delegates.wglGetPixelFormatAttribivEXT(hdc, iPixelFormat, iLayerPlane, (uint) nAttributes, piAttributes1, piValues1);
            piAttributes = *piAttributes1;
            piValues = *piValues1;
            return flag;
          }
      }

      [CLSCompliant(false)]
      public static unsafe bool GetPixelFormatAttrib(IntPtr hdc, int iPixelFormat, int iLayerPlane, uint nAttributes, [Out] int* piAttributes, [Out] int* piValues)
      {
        return Wgl.Delegates.wglGetPixelFormatAttribivEXT(hdc, iPixelFormat, iLayerPlane, nAttributes, piAttributes, piValues);
      }

      [CLSCompliant(false)]
      public static unsafe bool GetPixelFormatAttrib(IntPtr hdc, int iPixelFormat, int iLayerPlane, int nAttributes, [Out] int* piAttributes, [Out] int* piValues)
      {
        return Wgl.Delegates.wglGetPixelFormatAttribivEXT(hdc, iPixelFormat, iLayerPlane, (uint) nAttributes, piAttributes, piValues);
      }

      [CLSCompliant(false)]
      public static unsafe bool GetPixelFormatAttrib(IntPtr hdc, int iPixelFormat, int iLayerPlane, uint nAttributes, [Out] int[] piAttributes, [Out] float[] pfValues)
      {
        fixed (int* piAttributes1 = piAttributes)
          fixed (float* pfValues1 = pfValues)
            return Wgl.Delegates.wglGetPixelFormatAttribfvEXT(hdc, iPixelFormat, iLayerPlane, nAttributes, piAttributes1, pfValues1);
      }

      public static unsafe bool GetPixelFormatAttrib(IntPtr hdc, int iPixelFormat, int iLayerPlane, int nAttributes, [Out] int[] piAttributes, [Out] float[] pfValues)
      {
        fixed (int* piAttributes1 = piAttributes)
          fixed (float* pfValues1 = pfValues)
            return Wgl.Delegates.wglGetPixelFormatAttribfvEXT(hdc, iPixelFormat, iLayerPlane, (uint) nAttributes, piAttributes1, pfValues1);
      }

      [CLSCompliant(false)]
      public static unsafe bool GetPixelFormatAttrib(IntPtr hdc, int iPixelFormat, int iLayerPlane, uint nAttributes, out int piAttributes, out float pfValues)
      {
        fixed (int* piAttributes1 = &piAttributes)
          fixed (float* pfValues1 = &pfValues)
          {
            bool flag = Wgl.Delegates.wglGetPixelFormatAttribfvEXT(hdc, iPixelFormat, iLayerPlane, nAttributes, piAttributes1, pfValues1);
            piAttributes = *piAttributes1;
            pfValues = *pfValues1;
            return flag;
          }
      }

      public static unsafe bool GetPixelFormatAttrib(IntPtr hdc, int iPixelFormat, int iLayerPlane, int nAttributes, out int piAttributes, out float pfValues)
      {
        fixed (int* piAttributes1 = &piAttributes)
          fixed (float* pfValues1 = &pfValues)
          {
            bool flag = Wgl.Delegates.wglGetPixelFormatAttribfvEXT(hdc, iPixelFormat, iLayerPlane, (uint) nAttributes, piAttributes1, pfValues1);
            piAttributes = *piAttributes1;
            pfValues = *pfValues1;
            return flag;
          }
      }

      [CLSCompliant(false)]
      public static unsafe bool GetPixelFormatAttrib(IntPtr hdc, int iPixelFormat, int iLayerPlane, uint nAttributes, [Out] int* piAttributes, [Out] float* pfValues)
      {
        return Wgl.Delegates.wglGetPixelFormatAttribfvEXT(hdc, iPixelFormat, iLayerPlane, nAttributes, piAttributes, pfValues);
      }

      [CLSCompliant(false)]
      public static unsafe bool GetPixelFormatAttrib(IntPtr hdc, int iPixelFormat, int iLayerPlane, int nAttributes, [Out] int* piAttributes, [Out] float* pfValues)
      {
        return Wgl.Delegates.wglGetPixelFormatAttribfvEXT(hdc, iPixelFormat, iLayerPlane, (uint) nAttributes, piAttributes, pfValues);
      }

      public static bool SwapInterval(int interval)
      {
        return Wgl.Delegates.wglSwapIntervalEXT(interval);
      }

      public static int GetSwapInterval()
      {
        return Wgl.Delegates.wglGetSwapIntervalEXT();
      }
    }

    internal static class Imports
    {
      [SuppressUnmanagedCodeSecurity]
      [DllImport("OPENGL32.DLL", EntryPoint = "wglCreateContext", SetLastError = true)]
      internal static IntPtr CreateContext(IntPtr hDc);

      [SuppressUnmanagedCodeSecurity]
      [DllImport("OPENGL32.DLL", EntryPoint = "wglDeleteContext", SetLastError = true)]
      internal static bool DeleteContext(IntPtr oldContext);

      [SuppressUnmanagedCodeSecurity]
      [DllImport("OPENGL32.DLL", EntryPoint = "wglGetCurrentContext", SetLastError = true)]
      internal static IntPtr GetCurrentContext();

      [SuppressUnmanagedCodeSecurity]
      [DllImport("OPENGL32.DLL", EntryPoint = "wglMakeCurrent", SetLastError = true)]
      internal static bool MakeCurrent(IntPtr hDc, IntPtr newContext);

      [SuppressUnmanagedCodeSecurity]
      [DllImport("OPENGL32.DLL", EntryPoint = "wglCopyContext", SetLastError = true)]
      internal static bool CopyContext(IntPtr hglrcSrc, IntPtr hglrcDst, uint mask);

      [SuppressUnmanagedCodeSecurity]
      [DllImport("OPENGL32.DLL", EntryPoint = "wglChoosePixelFormat", SetLastError = true)]
      internal static int ChoosePixelFormat(IntPtr hDc, PixelFormatDescriptor* pPfd);

      [SuppressUnmanagedCodeSecurity]
      [DllImport("OPENGL32.DLL", EntryPoint = "wglDescribePixelFormat", SetLastError = true)]
      internal static int DescribePixelFormat(IntPtr hdc, int ipfd, uint cjpfd, PixelFormatDescriptor* ppfd);

      [SuppressUnmanagedCodeSecurity]
      [DllImport("OPENGL32.DLL", EntryPoint = "wglGetCurrentDC", SetLastError = true)]
      internal static IntPtr GetCurrentDC();

      [SuppressUnmanagedCodeSecurity]
      [DllImport("OPENGL32.DLL", EntryPoint = "wglGetDefaultProcAddress", SetLastError = true)]
      internal static IntPtr GetDefaultProcAddress(string lpszProc);

      [SuppressUnmanagedCodeSecurity]
      [DllImport("OPENGL32.DLL", EntryPoint = "wglGetProcAddress", SetLastError = true)]
      internal static IntPtr GetProcAddress(string lpszProc);

      [SuppressUnmanagedCodeSecurity]
      [DllImport("OPENGL32.DLL", EntryPoint = "wglGetPixelFormat", SetLastError = true)]
      internal static int GetPixelFormat(IntPtr hdc);

      [SuppressUnmanagedCodeSecurity]
      [DllImport("OPENGL32.DLL", EntryPoint = "wglSetPixelFormat", SetLastError = true)]
      internal static bool SetPixelFormat(IntPtr hdc, int ipfd, PixelFormatDescriptor* ppfd);

      [SuppressUnmanagedCodeSecurity]
      [DllImport("OPENGL32.DLL", EntryPoint = "wglSwapBuffers", SetLastError = true)]
      internal static bool SwapBuffers(IntPtr hdc);

      [SuppressUnmanagedCodeSecurity]
      [DllImport("OPENGL32.DLL", EntryPoint = "wglShareLists", SetLastError = true)]
      internal static bool ShareLists(IntPtr hrcSrvShare, IntPtr hrcSrvSource);

      [SuppressUnmanagedCodeSecurity]
      [DllImport("OPENGL32.DLL", EntryPoint = "wglCreateLayerContext", SetLastError = true)]
      internal static IntPtr CreateLayerContext(IntPtr hDc, int level);

      [SuppressUnmanagedCodeSecurity]
      [DllImport("OPENGL32.DLL", EntryPoint = "wglDescribeLayerPlane")]
      internal static bool DescribeLayerPlane(IntPtr hDc, int pixelFormat, int layerPlane, uint nBytes, LayerPlaneDescriptor* plpd);

      [SuppressUnmanagedCodeSecurity]
      [DllImport("OPENGL32.DLL", EntryPoint = "wglSetLayerPaletteEntries")]
      internal static int SetLayerPaletteEntries(IntPtr hdc, int iLayerPlane, int iStart, int cEntries, int* pcr);

      [SuppressUnmanagedCodeSecurity]
      [DllImport("OPENGL32.DLL", EntryPoint = "wglGetLayerPaletteEntries")]
      internal static int GetLayerPaletteEntries(IntPtr hdc, int iLayerPlane, int iStart, int cEntries, int* pcr);

      [SuppressUnmanagedCodeSecurity]
      [DllImport("OPENGL32.DLL", EntryPoint = "wglRealizeLayerPalette")]
      internal static bool RealizeLayerPalette(IntPtr hdc, int iLayerPlane, bool bRealize);

      [SuppressUnmanagedCodeSecurity]
      [DllImport("OPENGL32.DLL", EntryPoint = "wglSwapLayerBuffers")]
      internal static bool SwapLayerBuffers(IntPtr hdc, uint fuFlags);

      [SuppressUnmanagedCodeSecurity]
      [DllImport("OPENGL32.DLL", EntryPoint = "wglUseFontBitmapsA", CharSet = CharSet.Auto)]
      internal static bool UseFontBitmapsA(IntPtr hDC, int first, int count, int listBase);

      [SuppressUnmanagedCodeSecurity]
      [DllImport("OPENGL32.DLL", EntryPoint = "wglUseFontBitmapsW", CharSet = CharSet.Auto)]
      internal static bool UseFontBitmapsW(IntPtr hDC, int first, int count, int listBase);

      [SuppressUnmanagedCodeSecurity]
      [DllImport("OPENGL32.DLL", EntryPoint = "wglUseFontOutlinesA", CharSet = CharSet.Auto)]
      internal static bool UseFontOutlinesA(IntPtr hDC, int first, int count, int listBase, float thickness, float deviation, int fontMode, GlyphMetricsFloat* glyphMetrics);

      [SuppressUnmanagedCodeSecurity]
      [DllImport("OPENGL32.DLL", EntryPoint = "wglUseFontOutlinesW", CharSet = CharSet.Auto)]
      internal static bool UseFontOutlinesW(IntPtr hDC, int first, int count, int listBase, float thickness, float deviation, int fontMode, GlyphMetricsFloat* glyphMetrics);

      [SuppressUnmanagedCodeSecurity]
      [DllImport("OPENGL32.DLL", EntryPoint = "wglMakeContextCurrentEXT", SetLastError = true)]
      internal static bool MakeContextCurrentEXT(IntPtr hDrawDC, IntPtr hReadDC, IntPtr hglrc);

      [SuppressUnmanagedCodeSecurity]
      [DllImport("OPENGL32.DLL", EntryPoint = "wglChoosePixelFormatEXT", SetLastError = true)]
      internal static bool ChoosePixelFormatEXT(IntPtr hdc, int* piAttribIList, float* pfAttribFList, uint nMaxFormats, [Out] int* piFormats, [Out] uint* nNumFormats);
    }

    public static class NV
    {
      [CLSCompliant(false)]
      public static IntPtr AllocateMemory(int size, float readfreq, float writefreq, float priority)
      {
        return Wgl.Delegates.wglAllocateMemoryNV(size, readfreq, writefreq, priority);
      }

      public static void FreeMemory([Out] IntPtr pointer)
      {
        Wgl.Delegates.wglFreeMemoryNV(pointer);
      }

      public static void FreeMemory([In, Out] object pointer)
      {
        GCHandle gcHandle = GCHandle.Alloc(pointer, GCHandleType.Pinned);
        try
        {
          Wgl.Delegates.wglFreeMemoryNV(gcHandle.AddrOfPinnedObject());
        }
        finally
        {
          gcHandle.Free();
        }
      }
    }

    public static class Oml
    {
      public static unsafe bool GetSyncValues(IntPtr hdc, [Out] long[] ust, [Out] long[] msc, [Out] long[] sbc)
      {
        fixed (long* ust1 = ust)
          fixed (long* msc1 = msc)
            fixed (long* sbc1 = sbc)
              return Wgl.Delegates.wglGetSyncValuesOML(hdc, ust1, msc1, sbc1);
      }

      public static unsafe bool GetSyncValues(IntPtr hdc, out long ust, out long msc, out long sbc)
      {
        fixed (long* ust1 = &ust)
          fixed (long* msc1 = &msc)
            fixed (long* sbc1 = &sbc)
            {
              bool flag = Wgl.Delegates.wglGetSyncValuesOML(hdc, ust1, msc1, sbc1);
              ust = *ust1;
              msc = *msc1;
              sbc = *sbc1;
              return flag;
            }
      }

      [CLSCompliant(false)]
      public static unsafe bool GetSyncValues(IntPtr hdc, [Out] long* ust, [Out] long* msc, [Out] long* sbc)
      {
        return Wgl.Delegates.wglGetSyncValuesOML(hdc, ust, msc, sbc);
      }

      public static unsafe bool GetMscRate(IntPtr hdc, [Out] int[] numerator, [Out] int[] denominator)
      {
        fixed (int* numerator1 = numerator)
          fixed (int* denominator1 = denominator)
            return Wgl.Delegates.wglGetMscRateOML(hdc, numerator1, denominator1);
      }

      public static unsafe bool GetMscRate(IntPtr hdc, out int numerator, out int denominator)
      {
        fixed (int* numerator1 = &numerator)
          fixed (int* denominator1 = &denominator)
          {
            bool flag = Wgl.Delegates.wglGetMscRateOML(hdc, numerator1, denominator1);
            numerator = *numerator1;
            denominator = *denominator1;
            return flag;
          }
      }

      [CLSCompliant(false)]
      public static unsafe bool GetMscRate(IntPtr hdc, [Out] int* numerator, [Out] int* denominator)
      {
        return Wgl.Delegates.wglGetMscRateOML(hdc, numerator, denominator);
      }

      public static long SwapBuffersMsc(IntPtr hdc, long target_msc, long divisor, long remainder)
      {
        return Wgl.Delegates.wglSwapBuffersMscOML(hdc, target_msc, divisor, remainder);
      }

      public static long SwapLayerBuffersMsc(IntPtr hdc, int fuPlanes, long target_msc, long divisor, long remainder)
      {
        return Wgl.Delegates.wglSwapLayerBuffersMscOML(hdc, fuPlanes, target_msc, divisor, remainder);
      }

      public static unsafe bool WaitForMsc(IntPtr hdc, long target_msc, long divisor, long remainder, [Out] long[] ust, [Out] long[] msc, [Out] long[] sbc)
      {
        fixed (long* ust1 = ust)
          fixed (long* msc1 = msc)
            fixed (long* sbc1 = sbc)
              return Wgl.Delegates.wglWaitForMscOML(hdc, target_msc, divisor, remainder, ust1, msc1, sbc1);
      }

      public static unsafe bool WaitForMsc(IntPtr hdc, long target_msc, long divisor, long remainder, out long ust, out long msc, out long sbc)
      {
        fixed (long* ust1 = &ust)
          fixed (long* msc1 = &msc)
            fixed (long* sbc1 = &sbc)
            {
              bool flag = Wgl.Delegates.wglWaitForMscOML(hdc, target_msc, divisor, remainder, ust1, msc1, sbc1);
              ust = *ust1;
              msc = *msc1;
              sbc = *sbc1;
              return flag;
            }
      }

      [CLSCompliant(false)]
      public static unsafe bool WaitForMsc(IntPtr hdc, long target_msc, long divisor, long remainder, [Out] long* ust, [Out] long* msc, [Out] long* sbc)
      {
        return Wgl.Delegates.wglWaitForMscOML(hdc, target_msc, divisor, remainder, ust, msc, sbc);
      }

      public static unsafe bool WaitForSbc(IntPtr hdc, long target_sbc, [Out] long[] ust, [Out] long[] msc, [Out] long[] sbc)
      {
        fixed (long* ust1 = ust)
          fixed (long* msc1 = msc)
            fixed (long* sbc1 = sbc)
              return Wgl.Delegates.wglWaitForSbcOML(hdc, target_sbc, ust1, msc1, sbc1);
      }

      public static unsafe bool WaitForSbc(IntPtr hdc, long target_sbc, out long ust, out long msc, out long sbc)
      {
        fixed (long* ust1 = &ust)
          fixed (long* msc1 = &msc)
            fixed (long* sbc1 = &sbc)
            {
              bool flag = Wgl.Delegates.wglWaitForSbcOML(hdc, target_sbc, ust1, msc1, sbc1);
              ust = *ust1;
              msc = *msc1;
              sbc = *sbc1;
              return flag;
            }
      }

      [CLSCompliant(false)]
      public static unsafe bool WaitForSbc(IntPtr hdc, long target_sbc, [Out] long* ust, [Out] long* msc, [Out] long* sbc)
      {
        return Wgl.Delegates.wglWaitForSbcOML(hdc, target_sbc, ust, msc, sbc);
      }
    }

    public static class I3d
    {
      public static unsafe bool GetDigitalVideoParameters(IntPtr hDC, int iAttribute, [Out] int[] piValue)
      {
        fixed (int* piValue1 = piValue)
          return Wgl.Delegates.wglGetDigitalVideoParametersI3D(hDC, iAttribute, piValue1);
      }

      public static unsafe bool GetDigitalVideoParameters(IntPtr hDC, int iAttribute, out int piValue)
      {
        fixed (int* piValue1 = &piValue)
        {
          bool flag = Wgl.Delegates.wglGetDigitalVideoParametersI3D(hDC, iAttribute, piValue1);
          piValue = *piValue1;
          return flag;
        }
      }

      [CLSCompliant(false)]
      public static unsafe bool GetDigitalVideoParameters(IntPtr hDC, int iAttribute, [Out] int* piValue)
      {
        return Wgl.Delegates.wglGetDigitalVideoParametersI3D(hDC, iAttribute, piValue);
      }

      public static unsafe bool SetDigitalVideoParameters(IntPtr hDC, int iAttribute, int[] piValue)
      {
        fixed (int* piValue1 = piValue)
          return Wgl.Delegates.wglSetDigitalVideoParametersI3D(hDC, iAttribute, piValue1);
      }

      public static unsafe bool SetDigitalVideoParameters(IntPtr hDC, int iAttribute, ref int piValue)
      {
        fixed (int* piValue1 = &piValue)
          return Wgl.Delegates.wglSetDigitalVideoParametersI3D(hDC, iAttribute, piValue1);
      }

      [CLSCompliant(false)]
      public static unsafe bool SetDigitalVideoParameters(IntPtr hDC, int iAttribute, int* piValue)
      {
        return Wgl.Delegates.wglSetDigitalVideoParametersI3D(hDC, iAttribute, piValue);
      }

      public static unsafe bool GetGammaTableParameters(IntPtr hDC, int iAttribute, [Out] int[] piValue)
      {
        fixed (int* piValue1 = piValue)
          return Wgl.Delegates.wglGetGammaTableParametersI3D(hDC, iAttribute, piValue1);
      }

      public static unsafe bool GetGammaTableParameters(IntPtr hDC, int iAttribute, out int piValue)
      {
        fixed (int* piValue1 = &piValue)
        {
          bool flag = Wgl.Delegates.wglGetGammaTableParametersI3D(hDC, iAttribute, piValue1);
          piValue = *piValue1;
          return flag;
        }
      }

      [CLSCompliant(false)]
      public static unsafe bool GetGammaTableParameters(IntPtr hDC, int iAttribute, [Out] int* piValue)
      {
        return Wgl.Delegates.wglGetGammaTableParametersI3D(hDC, iAttribute, piValue);
      }

      public static unsafe bool SetGammaTableParameters(IntPtr hDC, int iAttribute, int[] piValue)
      {
        fixed (int* piValue1 = piValue)
          return Wgl.Delegates.wglSetGammaTableParametersI3D(hDC, iAttribute, piValue1);
      }

      public static unsafe bool SetGammaTableParameters(IntPtr hDC, int iAttribute, ref int piValue)
      {
        fixed (int* piValue1 = &piValue)
          return Wgl.Delegates.wglSetGammaTableParametersI3D(hDC, iAttribute, piValue1);
      }

      [CLSCompliant(false)]
      public static unsafe bool SetGammaTableParameters(IntPtr hDC, int iAttribute, int* piValue)
      {
        return Wgl.Delegates.wglSetGammaTableParametersI3D(hDC, iAttribute, piValue);
      }

      [CLSCompliant(false)]
      public static unsafe bool GetGammaTable(IntPtr hDC, int iEntries, [Out] ushort[] puRed, [Out] ushort[] puGreen, [Out] ushort[] puBlue)
      {
        fixed (ushort* puRed1 = puRed)
          fixed (ushort* puGreen1 = puGreen)
            fixed (ushort* puBlue1 = puBlue)
              return Wgl.Delegates.wglGetGammaTableI3D(hDC, iEntries, puRed1, puGreen1, puBlue1);
      }

      public static unsafe bool GetGammaTable(IntPtr hDC, int iEntries, [Out] short[] puRed, [Out] short[] puGreen, [Out] short[] puBlue)
      {
        fixed (short* numPtr1 = puRed)
          fixed (short* numPtr2 = puGreen)
            fixed (short* numPtr3 = puBlue)
              return Wgl.Delegates.wglGetGammaTableI3D(hDC, iEntries, (ushort*) numPtr1, (ushort*) numPtr2, (ushort*) numPtr3);
      }

      [CLSCompliant(false)]
      public static unsafe bool GetGammaTable(IntPtr hDC, int iEntries, out ushort puRed, out ushort puGreen, out ushort puBlue)
      {
        fixed (ushort* puRed1 = &puRed)
          fixed (ushort* puGreen1 = &puGreen)
            fixed (ushort* puBlue1 = &puBlue)
            {
              bool flag = Wgl.Delegates.wglGetGammaTableI3D(hDC, iEntries, puRed1, puGreen1, puBlue1);
              puRed = *puRed1;
              puGreen = *puGreen1;
              puBlue = *puBlue1;
              return flag;
            }
      }

      public static unsafe bool GetGammaTable(IntPtr hDC, int iEntries, out short puRed, out short puGreen, out short puBlue)
      {
        fixed (short* numPtr1 = &puRed)
          fixed (short* numPtr2 = &puGreen)
            fixed (short* numPtr3 = &puBlue)
            {
              bool flag = Wgl.Delegates.wglGetGammaTableI3D(hDC, iEntries, (ushort*) numPtr1, (ushort*) numPtr2, (ushort*) numPtr3);
              puRed = *numPtr1;
              puGreen = *numPtr2;
              puBlue = *numPtr3;
              return flag;
            }
      }

      [CLSCompliant(false)]
      public static unsafe bool GetGammaTable(IntPtr hDC, int iEntries, [Out] ushort* puRed, [Out] ushort* puGreen, [Out] ushort* puBlue)
      {
        return Wgl.Delegates.wglGetGammaTableI3D(hDC, iEntries, puRed, puGreen, puBlue);
      }

      [CLSCompliant(false)]
      public static unsafe bool GetGammaTable(IntPtr hDC, int iEntries, [Out] short* puRed, [Out] short* puGreen, [Out] short* puBlue)
      {
        return Wgl.Delegates.wglGetGammaTableI3D(hDC, iEntries, (ushort*) puRed, (ushort*) puGreen, (ushort*) puBlue);
      }

      [CLSCompliant(false)]
      public static unsafe bool SetGammaTable(IntPtr hDC, int iEntries, ushort[] puRed, ushort[] puGreen, ushort[] puBlue)
      {
        fixed (ushort* puRed1 = puRed)
          fixed (ushort* puGreen1 = puGreen)
            fixed (ushort* puBlue1 = puBlue)
              return Wgl.Delegates.wglSetGammaTableI3D(hDC, iEntries, puRed1, puGreen1, puBlue1);
      }

      public static unsafe bool SetGammaTable(IntPtr hDC, int iEntries, short[] puRed, short[] puGreen, short[] puBlue)
      {
        fixed (short* numPtr1 = puRed)
          fixed (short* numPtr2 = puGreen)
            fixed (short* numPtr3 = puBlue)
              return Wgl.Delegates.wglSetGammaTableI3D(hDC, iEntries, (ushort*) numPtr1, (ushort*) numPtr2, (ushort*) numPtr3);
      }

      [CLSCompliant(false)]
      public static unsafe bool SetGammaTable(IntPtr hDC, int iEntries, ref ushort puRed, ref ushort puGreen, ref ushort puBlue)
      {
        fixed (ushort* puRed1 = &puRed)
          fixed (ushort* puGreen1 = &puGreen)
            fixed (ushort* puBlue1 = &puBlue)
              return Wgl.Delegates.wglSetGammaTableI3D(hDC, iEntries, puRed1, puGreen1, puBlue1);
      }

      public static unsafe bool SetGammaTable(IntPtr hDC, int iEntries, ref short puRed, ref short puGreen, ref short puBlue)
      {
        fixed (short* numPtr1 = &puRed)
          fixed (short* numPtr2 = &puGreen)
            fixed (short* numPtr3 = &puBlue)
              return Wgl.Delegates.wglSetGammaTableI3D(hDC, iEntries, (ushort*) numPtr1, (ushort*) numPtr2, (ushort*) numPtr3);
      }

      [CLSCompliant(false)]
      public static unsafe bool SetGammaTable(IntPtr hDC, int iEntries, ushort* puRed, ushort* puGreen, ushort* puBlue)
      {
        return Wgl.Delegates.wglSetGammaTableI3D(hDC, iEntries, puRed, puGreen, puBlue);
      }

      [CLSCompliant(false)]
      public static unsafe bool SetGammaTable(IntPtr hDC, int iEntries, short* puRed, short* puGreen, short* puBlue)
      {
        return Wgl.Delegates.wglSetGammaTableI3D(hDC, iEntries, (ushort*) puRed, (ushort*) puGreen, (ushort*) puBlue);
      }

      public static bool EnableGenlock(IntPtr hDC)
      {
        return Wgl.Delegates.wglEnableGenlockI3D(hDC);
      }

      public static bool DisableGenlock(IntPtr hDC)
      {
        return Wgl.Delegates.wglDisableGenlockI3D(hDC);
      }

      public static unsafe bool IsEnabledGenlock(IntPtr hDC, [Out] bool[] pFlag)
      {
        fixed (bool* pFlag1 = pFlag)
          return Wgl.Delegates.wglIsEnabledGenlockI3D(hDC, pFlag1);
      }

      public static unsafe bool IsEnabledGenlock(IntPtr hDC, out bool pFlag)
      {
        fixed (bool* pFlag1 = &pFlag)
        {
          bool flag = Wgl.Delegates.wglIsEnabledGenlockI3D(hDC, pFlag1);
          pFlag = *pFlag1;
          return flag;
        }
      }

      [CLSCompliant(false)]
      public static unsafe bool IsEnabledGenlock(IntPtr hDC, [Out] bool* pFlag)
      {
        return Wgl.Delegates.wglIsEnabledGenlockI3D(hDC, pFlag);
      }

      [CLSCompliant(false)]
      public static bool GenlockSource(IntPtr hDC, uint uSource)
      {
        return Wgl.Delegates.wglGenlockSourceI3D(hDC, uSource);
      }

      public static bool GenlockSource(IntPtr hDC, int uSource)
      {
        return Wgl.Delegates.wglGenlockSourceI3D(hDC, (uint) uSource);
      }

      [CLSCompliant(false)]
      public static unsafe bool GetGenlockSource(IntPtr hDC, [Out] uint[] uSource)
      {
        fixed (uint* uSource1 = uSource)
          return Wgl.Delegates.wglGetGenlockSourceI3D(hDC, uSource1);
      }

      public static unsafe bool GetGenlockSource(IntPtr hDC, [Out] int[] uSource)
      {
        fixed (int* numPtr = uSource)
          return Wgl.Delegates.wglGetGenlockSourceI3D(hDC, (uint*) numPtr);
      }

      [CLSCompliant(false)]
      public static unsafe bool GetGenlockSource(IntPtr hDC, out uint uSource)
      {
        fixed (uint* uSource1 = &uSource)
        {
          bool flag = Wgl.Delegates.wglGetGenlockSourceI3D(hDC, uSource1);
          uSource = *uSource1;
          return flag;
        }
      }

      public static unsafe bool GetGenlockSource(IntPtr hDC, out int uSource)
      {
        fixed (int* numPtr = &uSource)
        {
          bool flag = Wgl.Delegates.wglGetGenlockSourceI3D(hDC, (uint*) numPtr);
          uSource = *numPtr;
          return flag;
        }
      }

      [CLSCompliant(false)]
      public static unsafe bool GetGenlockSource(IntPtr hDC, [Out] uint* uSource)
      {
        return Wgl.Delegates.wglGetGenlockSourceI3D(hDC, uSource);
      }

      [CLSCompliant(false)]
      public static unsafe bool GetGenlockSource(IntPtr hDC, [Out] int* uSource)
      {
        return Wgl.Delegates.wglGetGenlockSourceI3D(hDC, (uint*) uSource);
      }

      [CLSCompliant(false)]
      public static bool GenlockSourceEdge(IntPtr hDC, uint uEdge)
      {
        return Wgl.Delegates.wglGenlockSourceEdgeI3D(hDC, uEdge);
      }

      public static bool GenlockSourceEdge(IntPtr hDC, int uEdge)
      {
        return Wgl.Delegates.wglGenlockSourceEdgeI3D(hDC, (uint) uEdge);
      }

      [CLSCompliant(false)]
      public static unsafe bool GetGenlockSourceEdge(IntPtr hDC, [Out] uint[] uEdge)
      {
        fixed (uint* uEdge1 = uEdge)
          return Wgl.Delegates.wglGetGenlockSourceEdgeI3D(hDC, uEdge1);
      }

      public static unsafe bool GetGenlockSourceEdge(IntPtr hDC, [Out] int[] uEdge)
      {
        fixed (int* numPtr = uEdge)
          return Wgl.Delegates.wglGetGenlockSourceEdgeI3D(hDC, (uint*) numPtr);
      }

      [CLSCompliant(false)]
      public static unsafe bool GetGenlockSourceEdge(IntPtr hDC, out uint uEdge)
      {
        fixed (uint* uEdge1 = &uEdge)
        {
          bool flag = Wgl.Delegates.wglGetGenlockSourceEdgeI3D(hDC, uEdge1);
          uEdge = *uEdge1;
          return flag;
        }
      }

      public static unsafe bool GetGenlockSourceEdge(IntPtr hDC, out int uEdge)
      {
        fixed (int* numPtr = &uEdge)
        {
          bool flag = Wgl.Delegates.wglGetGenlockSourceEdgeI3D(hDC, (uint*) numPtr);
          uEdge = *numPtr;
          return flag;
        }
      }

      [CLSCompliant(false)]
      public static unsafe bool GetGenlockSourceEdge(IntPtr hDC, [Out] uint* uEdge)
      {
        return Wgl.Delegates.wglGetGenlockSourceEdgeI3D(hDC, uEdge);
      }

      [CLSCompliant(false)]
      public static unsafe bool GetGenlockSourceEdge(IntPtr hDC, [Out] int* uEdge)
      {
        return Wgl.Delegates.wglGetGenlockSourceEdgeI3D(hDC, (uint*) uEdge);
      }

      [CLSCompliant(false)]
      public static bool GenlockSampleRate(IntPtr hDC, uint uRate)
      {
        return Wgl.Delegates.wglGenlockSampleRateI3D(hDC, uRate);
      }

      public static bool GenlockSampleRate(IntPtr hDC, int uRate)
      {
        return Wgl.Delegates.wglGenlockSampleRateI3D(hDC, (uint) uRate);
      }

      [CLSCompliant(false)]
      public static unsafe bool GetGenlockSampleRate(IntPtr hDC, [Out] uint[] uRate)
      {
        fixed (uint* uRate1 = uRate)
          return Wgl.Delegates.wglGetGenlockSampleRateI3D(hDC, uRate1);
      }

      public static unsafe bool GetGenlockSampleRate(IntPtr hDC, [Out] int[] uRate)
      {
        fixed (int* numPtr = uRate)
          return Wgl.Delegates.wglGetGenlockSampleRateI3D(hDC, (uint*) numPtr);
      }

      [CLSCompliant(false)]
      public static unsafe bool GetGenlockSampleRate(IntPtr hDC, out uint uRate)
      {
        fixed (uint* uRate1 = &uRate)
        {
          bool flag = Wgl.Delegates.wglGetGenlockSampleRateI3D(hDC, uRate1);
          uRate = *uRate1;
          return flag;
        }
      }

      public static unsafe bool GetGenlockSampleRate(IntPtr hDC, out int uRate)
      {
        fixed (int* numPtr = &uRate)
        {
          bool flag = Wgl.Delegates.wglGetGenlockSampleRateI3D(hDC, (uint*) numPtr);
          uRate = *numPtr;
          return flag;
        }
      }

      [CLSCompliant(false)]
      public static unsafe bool GetGenlockSampleRate(IntPtr hDC, [Out] uint* uRate)
      {
        return Wgl.Delegates.wglGetGenlockSampleRateI3D(hDC, uRate);
      }

      [CLSCompliant(false)]
      public static unsafe bool GetGenlockSampleRate(IntPtr hDC, [Out] int* uRate)
      {
        return Wgl.Delegates.wglGetGenlockSampleRateI3D(hDC, (uint*) uRate);
      }

      [CLSCompliant(false)]
      public static bool GenlockSourceDelay(IntPtr hDC, uint uDelay)
      {
        return Wgl.Delegates.wglGenlockSourceDelayI3D(hDC, uDelay);
      }

      public static bool GenlockSourceDelay(IntPtr hDC, int uDelay)
      {
        return Wgl.Delegates.wglGenlockSourceDelayI3D(hDC, (uint) uDelay);
      }

      [CLSCompliant(false)]
      public static unsafe bool GetGenlockSourceDelay(IntPtr hDC, [Out] uint[] uDelay)
      {
        fixed (uint* uDelay1 = uDelay)
          return Wgl.Delegates.wglGetGenlockSourceDelayI3D(hDC, uDelay1);
      }

      public static unsafe bool GetGenlockSourceDelay(IntPtr hDC, [Out] int[] uDelay)
      {
        fixed (int* numPtr = uDelay)
          return Wgl.Delegates.wglGetGenlockSourceDelayI3D(hDC, (uint*) numPtr);
      }

      [CLSCompliant(false)]
      public static unsafe bool GetGenlockSourceDelay(IntPtr hDC, out uint uDelay)
      {
        fixed (uint* uDelay1 = &uDelay)
        {
          bool flag = Wgl.Delegates.wglGetGenlockSourceDelayI3D(hDC, uDelay1);
          uDelay = *uDelay1;
          return flag;
        }
      }

      public static unsafe bool GetGenlockSourceDelay(IntPtr hDC, out int uDelay)
      {
        fixed (int* numPtr = &uDelay)
        {
          bool flag = Wgl.Delegates.wglGetGenlockSourceDelayI3D(hDC, (uint*) numPtr);
          uDelay = *numPtr;
          return flag;
        }
      }

      [CLSCompliant(false)]
      public static unsafe bool GetGenlockSourceDelay(IntPtr hDC, [Out] uint* uDelay)
      {
        return Wgl.Delegates.wglGetGenlockSourceDelayI3D(hDC, uDelay);
      }

      [CLSCompliant(false)]
      public static unsafe bool GetGenlockSourceDelay(IntPtr hDC, [Out] int* uDelay)
      {
        return Wgl.Delegates.wglGetGenlockSourceDelayI3D(hDC, (uint*) uDelay);
      }

      [CLSCompliant(false)]
      public static unsafe bool QueryGenlockMaxSourceDelay(IntPtr hDC, [Out] uint[] uMaxLineDelay, [Out] uint[] uMaxPixelDelay)
      {
        fixed (uint* uMaxLineDelay1 = uMaxLineDelay)
          fixed (uint* uMaxPixelDelay1 = uMaxPixelDelay)
            return Wgl.Delegates.wglQueryGenlockMaxSourceDelayI3D(hDC, uMaxLineDelay1, uMaxPixelDelay1);
      }

      public static unsafe bool QueryGenlockMaxSourceDelay(IntPtr hDC, [Out] int[] uMaxLineDelay, [Out] int[] uMaxPixelDelay)
      {
        fixed (int* numPtr1 = uMaxLineDelay)
          fixed (int* numPtr2 = uMaxPixelDelay)
            return Wgl.Delegates.wglQueryGenlockMaxSourceDelayI3D(hDC, (uint*) numPtr1, (uint*) numPtr2);
      }

      [CLSCompliant(false)]
      public static unsafe bool QueryGenlockMaxSourceDelay(IntPtr hDC, out uint uMaxLineDelay, out uint uMaxPixelDelay)
      {
        fixed (uint* uMaxLineDelay1 = &uMaxLineDelay)
          fixed (uint* uMaxPixelDelay1 = &uMaxPixelDelay)
          {
            bool flag = Wgl.Delegates.wglQueryGenlockMaxSourceDelayI3D(hDC, uMaxLineDelay1, uMaxPixelDelay1);
            uMaxLineDelay = *uMaxLineDelay1;
            uMaxPixelDelay = *uMaxPixelDelay1;
            return flag;
          }
      }

      public static unsafe bool QueryGenlockMaxSourceDelay(IntPtr hDC, out int uMaxLineDelay, out int uMaxPixelDelay)
      {
        fixed (int* numPtr1 = &uMaxLineDelay)
          fixed (int* numPtr2 = &uMaxPixelDelay)
          {
            bool flag = Wgl.Delegates.wglQueryGenlockMaxSourceDelayI3D(hDC, (uint*) numPtr1, (uint*) numPtr2);
            uMaxLineDelay = *numPtr1;
            uMaxPixelDelay = *numPtr2;
            return flag;
          }
      }

      [CLSCompliant(false)]
      public static unsafe bool QueryGenlockMaxSourceDelay(IntPtr hDC, [Out] uint* uMaxLineDelay, [Out] uint* uMaxPixelDelay)
      {
        return Wgl.Delegates.wglQueryGenlockMaxSourceDelayI3D(hDC, uMaxLineDelay, uMaxPixelDelay);
      }

      [CLSCompliant(false)]
      public static unsafe bool QueryGenlockMaxSourceDelay(IntPtr hDC, [Out] int* uMaxLineDelay, [Out] int* uMaxPixelDelay)
      {
        return Wgl.Delegates.wglQueryGenlockMaxSourceDelayI3D(hDC, (uint*) uMaxLineDelay, (uint*) uMaxPixelDelay);
      }

      [CLSCompliant(false)]
      public static IntPtr CreateImageBuffer(IntPtr hDC, int dwSize, uint uFlags)
      {
        return Wgl.Delegates.wglCreateImageBufferI3D(hDC, dwSize, uFlags);
      }

      [CLSCompliant(false)]
      public static IntPtr CreateImageBuffer(IntPtr hDC, int dwSize, int uFlags)
      {
        return Wgl.Delegates.wglCreateImageBufferI3D(hDC, dwSize, (uint) uFlags);
      }

      public static bool DestroyImageBuffer(IntPtr hDC, IntPtr pAddress)
      {
        return Wgl.Delegates.wglDestroyImageBufferI3D(hDC, pAddress);
      }

      public static bool DestroyImageBuffer(IntPtr hDC, [In, Out] object pAddress)
      {
        GCHandle gcHandle = GCHandle.Alloc(pAddress, GCHandleType.Pinned);
        try
        {
          return Wgl.Delegates.wglDestroyImageBufferI3D(hDC, gcHandle.AddrOfPinnedObject());
        }
        finally
        {
          gcHandle.Free();
        }
      }

      [CLSCompliant(false)]
      public static unsafe bool AssociateImageBufferEvents(IntPtr hDC, IntPtr[] pEvent, IntPtr pAddress, int[] pSize, uint count)
      {
        fixed (IntPtr* pEvent1 = pEvent)
          fixed (int* pSize1 = pSize)
            return Wgl.Delegates.wglAssociateImageBufferEventsI3D(hDC, pEvent1, pAddress, pSize1, count);
      }

      public static unsafe bool AssociateImageBufferEvents(IntPtr hDC, IntPtr[] pEvent, IntPtr pAddress, int[] pSize, int count)
      {
        fixed (IntPtr* pEvent1 = pEvent)
          fixed (int* pSize1 = pSize)
            return Wgl.Delegates.wglAssociateImageBufferEventsI3D(hDC, pEvent1, pAddress, pSize1, (uint) count);
      }

      [CLSCompliant(false)]
      public static unsafe bool AssociateImageBufferEvents(IntPtr hDC, IntPtr[] pEvent, [In, Out] object pAddress, int[] pSize, uint count)
      {
        fixed (IntPtr* pEvent1 = pEvent)
          fixed (int* pSize1 = pSize)
          {
            GCHandle gcHandle = GCHandle.Alloc(pAddress, GCHandleType.Pinned);
            try
            {
              return Wgl.Delegates.wglAssociateImageBufferEventsI3D(hDC, pEvent1, gcHandle.AddrOfPinnedObject(), pSize1, count);
            }
            finally
            {
              gcHandle.Free();
            }
          }
      }

      public static unsafe bool AssociateImageBufferEvents(IntPtr hDC, IntPtr[] pEvent, [In, Out] object pAddress, int[] pSize, int count)
      {
        fixed (IntPtr* pEvent1 = pEvent)
          fixed (int* pSize1 = pSize)
          {
            GCHandle gcHandle = GCHandle.Alloc(pAddress, GCHandleType.Pinned);
            try
            {
              return Wgl.Delegates.wglAssociateImageBufferEventsI3D(hDC, pEvent1, gcHandle.AddrOfPinnedObject(), pSize1, (uint) count);
            }
            finally
            {
              gcHandle.Free();
            }
          }
      }

      [CLSCompliant(false)]
      public static unsafe bool AssociateImageBufferEvents(IntPtr hDC, IntPtr[] pEvent, [In, Out] object pAddress, ref int pSize, uint count)
      {
        fixed (IntPtr* pEvent1 = pEvent)
          fixed (int* pSize1 = &pSize)
          {
            GCHandle gcHandle = GCHandle.Alloc(pAddress, GCHandleType.Pinned);
            try
            {
              return Wgl.Delegates.wglAssociateImageBufferEventsI3D(hDC, pEvent1, gcHandle.AddrOfPinnedObject(), pSize1, count);
            }
            finally
            {
              gcHandle.Free();
            }
          }
      }

      public static unsafe bool AssociateImageBufferEvents(IntPtr hDC, IntPtr[] pEvent, [In, Out] object pAddress, ref int pSize, int count)
      {
        fixed (IntPtr* pEvent1 = pEvent)
          fixed (int* pSize1 = &pSize)
          {
            GCHandle gcHandle = GCHandle.Alloc(pAddress, GCHandleType.Pinned);
            try
            {
              return Wgl.Delegates.wglAssociateImageBufferEventsI3D(hDC, pEvent1, gcHandle.AddrOfPinnedObject(), pSize1, (uint) count);
            }
            finally
            {
              gcHandle.Free();
            }
          }
      }

      [CLSCompliant(false)]
      public static unsafe bool AssociateImageBufferEvents(IntPtr hDC, ref IntPtr pEvent, [In, Out] object pAddress, int[] pSize, uint count)
      {
        fixed (IntPtr* pEvent1 = &pEvent)
          fixed (int* pSize1 = pSize)
          {
            GCHandle gcHandle = GCHandle.Alloc(pAddress, GCHandleType.Pinned);
            try
            {
              return Wgl.Delegates.wglAssociateImageBufferEventsI3D(hDC, pEvent1, gcHandle.AddrOfPinnedObject(), pSize1, count);
            }
            finally
            {
              gcHandle.Free();
            }
          }
      }

      public static unsafe bool AssociateImageBufferEvents(IntPtr hDC, ref IntPtr pEvent, [In, Out] object pAddress, int[] pSize, int count)
      {
        fixed (IntPtr* pEvent1 = &pEvent)
          fixed (int* pSize1 = pSize)
          {
            GCHandle gcHandle = GCHandle.Alloc(pAddress, GCHandleType.Pinned);
            try
            {
              return Wgl.Delegates.wglAssociateImageBufferEventsI3D(hDC, pEvent1, gcHandle.AddrOfPinnedObject(), pSize1, (uint) count);
            }
            finally
            {
              gcHandle.Free();
            }
          }
      }

      [CLSCompliant(false)]
      public static unsafe bool AssociateImageBufferEvents(IntPtr hDC, ref IntPtr pEvent, [In, Out] object pAddress, ref int pSize, uint count)
      {
        fixed (IntPtr* pEvent1 = &pEvent)
          fixed (int* pSize1 = &pSize)
          {
            GCHandle gcHandle = GCHandle.Alloc(pAddress, GCHandleType.Pinned);
            try
            {
              return Wgl.Delegates.wglAssociateImageBufferEventsI3D(hDC, pEvent1, gcHandle.AddrOfPinnedObject(), pSize1, count);
            }
            finally
            {
              gcHandle.Free();
            }
          }
      }

      public static unsafe bool AssociateImageBufferEvents(IntPtr hDC, ref IntPtr pEvent, [In, Out] object pAddress, ref int pSize, int count)
      {
        fixed (IntPtr* pEvent1 = &pEvent)
          fixed (int* pSize1 = &pSize)
          {
            GCHandle gcHandle = GCHandle.Alloc(pAddress, GCHandleType.Pinned);
            try
            {
              return Wgl.Delegates.wglAssociateImageBufferEventsI3D(hDC, pEvent1, gcHandle.AddrOfPinnedObject(), pSize1, (uint) count);
            }
            finally
            {
              gcHandle.Free();
            }
          }
      }

      [CLSCompliant(false)]
      public static unsafe bool AssociateImageBufferEvents(IntPtr hDC, IntPtr* pEvent, [In, Out] object pAddress, int* pSize, uint count)
      {
        GCHandle gcHandle = GCHandle.Alloc(pAddress, GCHandleType.Pinned);
        try
        {
          return Wgl.Delegates.wglAssociateImageBufferEventsI3D(hDC, pEvent, gcHandle.AddrOfPinnedObject(), pSize, count);
        }
        finally
        {
          gcHandle.Free();
        }
      }

      [CLSCompliant(false)]
      public static unsafe bool AssociateImageBufferEvents(IntPtr hDC, IntPtr* pEvent, [In, Out] object pAddress, int* pSize, int count)
      {
        GCHandle gcHandle = GCHandle.Alloc(pAddress, GCHandleType.Pinned);
        try
        {
          return Wgl.Delegates.wglAssociateImageBufferEventsI3D(hDC, pEvent, gcHandle.AddrOfPinnedObject(), pSize, (uint) count);
        }
        finally
        {
          gcHandle.Free();
        }
      }

      [CLSCompliant(false)]
      public static unsafe bool AssociateImageBufferEvents(IntPtr hDC, IntPtr* pEvent, [In, Out] object pAddress, int[] pSize, uint count)
      {
        fixed (int* pSize1 = pSize)
        {
          GCHandle gcHandle = GCHandle.Alloc(pAddress, GCHandleType.Pinned);
          try
          {
            return Wgl.Delegates.wglAssociateImageBufferEventsI3D(hDC, pEvent, gcHandle.AddrOfPinnedObject(), pSize1, count);
          }
          finally
          {
            gcHandle.Free();
          }
        }
      }

      [CLSCompliant(false)]
      public static unsafe bool AssociateImageBufferEvents(IntPtr hDC, IntPtr* pEvent, [In, Out] object pAddress, int[] pSize, int count)
      {
        fixed (int* pSize1 = pSize)
        {
          GCHandle gcHandle = GCHandle.Alloc(pAddress, GCHandleType.Pinned);
          try
          {
            return Wgl.Delegates.wglAssociateImageBufferEventsI3D(hDC, pEvent, gcHandle.AddrOfPinnedObject(), pSize1, (uint) count);
          }
          finally
          {
            gcHandle.Free();
          }
        }
      }

      [CLSCompliant(false)]
      public static unsafe bool AssociateImageBufferEvents(IntPtr hDC, IntPtr* pEvent, [In, Out] object pAddress, ref int pSize, uint count)
      {
        fixed (int* pSize1 = &pSize)
        {
          GCHandle gcHandle = GCHandle.Alloc(pAddress, GCHandleType.Pinned);
          try
          {
            return Wgl.Delegates.wglAssociateImageBufferEventsI3D(hDC, pEvent, gcHandle.AddrOfPinnedObject(), pSize1, count);
          }
          finally
          {
            gcHandle.Free();
          }
        }
      }

      [CLSCompliant(false)]
      public static unsafe bool AssociateImageBufferEvents(IntPtr hDC, IntPtr* pEvent, [In, Out] object pAddress, ref int pSize, int count)
      {
        fixed (int* pSize1 = &pSize)
        {
          GCHandle gcHandle = GCHandle.Alloc(pAddress, GCHandleType.Pinned);
          try
          {
            return Wgl.Delegates.wglAssociateImageBufferEventsI3D(hDC, pEvent, gcHandle.AddrOfPinnedObject(), pSize1, (uint) count);
          }
          finally
          {
            gcHandle.Free();
          }
        }
      }

      [CLSCompliant(false)]
      public static bool ReleaseImageBufferEvents(IntPtr hDC, IntPtr pAddress, uint count)
      {
        return Wgl.Delegates.wglReleaseImageBufferEventsI3D(hDC, pAddress, count);
      }

      public static bool ReleaseImageBufferEvents(IntPtr hDC, IntPtr pAddress, int count)
      {
        return Wgl.Delegates.wglReleaseImageBufferEventsI3D(hDC, pAddress, (uint) count);
      }

      [CLSCompliant(false)]
      public static bool ReleaseImageBufferEvents(IntPtr hDC, [In, Out] object pAddress, uint count)
      {
        GCHandle gcHandle = GCHandle.Alloc(pAddress, GCHandleType.Pinned);
        try
        {
          return Wgl.Delegates.wglReleaseImageBufferEventsI3D(hDC, gcHandle.AddrOfPinnedObject(), count);
        }
        finally
        {
          gcHandle.Free();
        }
      }

      public static bool ReleaseImageBufferEvents(IntPtr hDC, [In, Out] object pAddress, int count)
      {
        GCHandle gcHandle = GCHandle.Alloc(pAddress, GCHandleType.Pinned);
        try
        {
          return Wgl.Delegates.wglReleaseImageBufferEventsI3D(hDC, gcHandle.AddrOfPinnedObject(), (uint) count);
        }
        finally
        {
          gcHandle.Free();
        }
      }

      public static bool EnableFrameLock()
      {
        return Wgl.Delegates.wglEnableFrameLockI3D();
      }

      public static bool DisableFrameLock()
      {
        return Wgl.Delegates.wglDisableFrameLockI3D();
      }

      public static unsafe bool IsEnabledFrameLock([Out] bool[] pFlag)
      {
        fixed (bool* pFlag1 = pFlag)
          return Wgl.Delegates.wglIsEnabledFrameLockI3D(pFlag1);
      }

      public static unsafe bool IsEnabledFrameLock(out bool pFlag)
      {
        fixed (bool* pFlag1 = &pFlag)
        {
          bool flag = Wgl.Delegates.wglIsEnabledFrameLockI3D(pFlag1);
          pFlag = *pFlag1;
          return flag;
        }
      }

      [CLSCompliant(false)]
      public static unsafe bool IsEnabledFrameLock([Out] bool* pFlag)
      {
        return Wgl.Delegates.wglIsEnabledFrameLockI3D(pFlag);
      }

      public static unsafe bool QueryFrameLockMaster([Out] bool[] pFlag)
      {
        fixed (bool* pFlag1 = pFlag)
          return Wgl.Delegates.wglQueryFrameLockMasterI3D(pFlag1);
      }

      public static unsafe bool QueryFrameLockMaster(out bool pFlag)
      {
        fixed (bool* pFlag1 = &pFlag)
        {
          bool flag = Wgl.Delegates.wglQueryFrameLockMasterI3D(pFlag1);
          pFlag = *pFlag1;
          return flag;
        }
      }

      [CLSCompliant(false)]
      public static unsafe bool QueryFrameLockMaster([Out] bool* pFlag)
      {
        return Wgl.Delegates.wglQueryFrameLockMasterI3D(pFlag);
      }

      public static unsafe bool GetFrameUsage([Out] float[] pUsage)
      {
        fixed (float* pUsage1 = pUsage)
          return Wgl.Delegates.wglGetFrameUsageI3D(pUsage1);
      }

      public static unsafe bool GetFrameUsage(out float pUsage)
      {
        fixed (float* pUsage1 = &pUsage)
        {
          bool flag = Wgl.Delegates.wglGetFrameUsageI3D(pUsage1);
          pUsage = *pUsage1;
          return flag;
        }
      }

      [CLSCompliant(false)]
      public static unsafe bool GetFrameUsage([Out] float* pUsage)
      {
        return Wgl.Delegates.wglGetFrameUsageI3D(pUsage);
      }

      public static bool BeginFrameTracking()
      {
        return Wgl.Delegates.wglBeginFrameTrackingI3D();
      }

      public static bool EndFrameTracking()
      {
        return Wgl.Delegates.wglEndFrameTrackingI3D();
      }

      public static unsafe bool QueryFrameTracking([Out] int[] pFrameCount, [Out] int[] pMissedFrames, [Out] float[] pLastMissedUsage)
      {
        fixed (int* pFrameCount1 = pFrameCount)
          fixed (int* pMissedFrames1 = pMissedFrames)
            fixed (float* pLastMissedUsage1 = pLastMissedUsage)
              return Wgl.Delegates.wglQueryFrameTrackingI3D(pFrameCount1, pMissedFrames1, pLastMissedUsage1);
      }

      public static unsafe bool QueryFrameTracking(out int pFrameCount, out int pMissedFrames, out float pLastMissedUsage)
      {
        fixed (int* pFrameCount1 = &pFrameCount)
          fixed (int* pMissedFrames1 = &pMissedFrames)
            fixed (float* pLastMissedUsage1 = &pLastMissedUsage)
            {
              bool flag = Wgl.Delegates.wglQueryFrameTrackingI3D(pFrameCount1, pMissedFrames1, pLastMissedUsage1);
              pFrameCount = *pFrameCount1;
              pMissedFrames = *pMissedFrames1;
              pLastMissedUsage = *pLastMissedUsage1;
              return flag;
            }
      }

      [CLSCompliant(false)]
      public static unsafe bool QueryFrameTracking([Out] int* pFrameCount, [Out] int* pMissedFrames, [Out] float* pLastMissedUsage)
      {
        return Wgl.Delegates.wglQueryFrameTrackingI3D(pFrameCount, pMissedFrames, pLastMissedUsage);
      }
    }
  }
}
