// Type: FezEngine.Tools.SettingsManager
// Assembly: FezEngine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11F00D13-0150-47CC-B906-98B362969219
// Assembly location: F:\Program Files (x86)\FEZ\FezEngine.dll

using Common;
using ContentSerialization;
using FezEngine.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FezEngine.Tools
{
  public static class SettingsManager
  {
    public static readonly List<DisplayMode> Resolutions = Enumerable.ToList<DisplayMode>((IEnumerable<DisplayMode>) Enumerable.ThenBy<DisplayMode, int>(Enumerable.OrderBy<DisplayMode, int>(Enumerable.Where<DisplayMode>(Enumerable.Distinct<DisplayMode>((IEnumerable<DisplayMode>) GraphicsAdapter.DefaultAdapter.SupportedDisplayModes, (IEqualityComparer<DisplayMode>) DisplayModeEqualityComparer.Default), (Func<DisplayMode, bool>) (x =>
    {
      if (x.Width < 1280)
        return x.Height >= 720;
      else
        return true;
    })), (Func<DisplayMode, int>) (x => x.Width)), (Func<DisplayMode, int>) (x => x.Height)));
    public static readonly DisplayMode NativeResolution = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
    private const float SixteenByNine = 1.777778f;
    private const string SettingsFilename = "Settings";
    public static readonly Settings Settings;
    public static GraphicsDeviceManager DeviceManager;
    public static bool FirstOpen;
    private static float viewScale;

    static SettingsManager()
    {
      if (Enumerable.Any<DisplayMode>((IEnumerable<DisplayMode>) SettingsManager.Resolutions, (Func<DisplayMode, bool>) (x =>
      {
        if (x.Width >= 1280)
          return x.Height >= 720;
        else
          return false;
      })))
        SettingsManager.Resolutions.RemoveAll((Predicate<DisplayMode>) (x =>
        {
          if (x.Width < 1280)
            return true;
          if (x.Height < 720)
            return x != SettingsManager.NativeResolution;
          else
            return false;
        }));
      string str1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FEZ");
      if (!Directory.Exists(str1))
        Directory.CreateDirectory(str1);
      string str2 = Path.Combine(str1, "Settings");
      SettingsManager.FirstOpen = !File.Exists(str2);
      if (SettingsManager.FirstOpen)
      {
        SettingsManager.Settings = new Settings();
      }
      else
      {
        try
        {
          SettingsManager.Settings = SdlSerializer.Deserialize<Settings>(str2);
        }
        catch (Exception ex)
        {
          SettingsManager.Settings = new Settings();
        }
      }
      SdlGamePad.GetState(PlayerIndex.One);
      SettingsManager.Settings.ApplyGamepadMapping();
      Culture.Language = SettingsManager.Settings.Language;
      SettingsManager.Save();
    }

    public static void Save()
    {
      SdlSerializer.Serialize<Settings>(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FEZ"), "Settings"), SettingsManager.Settings);
    }

    public static void Apply()
    {
      Game game = ServiceHelper.Game;
      int width = SettingsManager.Settings.Width;
      int height = SettingsManager.Settings.Height;
      if (SettingsManager.Settings.UseCurrentMode)
      {
        DisplayMode currentDisplayMode = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
        SettingsManager.Settings.Height = height = currentDisplayMode.Height;
        SettingsManager.Settings.Width = width = (int) Math.Round((double) height * (double) currentDisplayMode.AspectRatio);
      }
      if (SettingsManager.Settings.ScreenMode == ScreenMode.Fullscreen && !Enumerable.Any<DisplayMode>((IEnumerable<DisplayMode>) GraphicsAdapter.DefaultAdapter.SupportedDisplayModes, (Func<DisplayMode, bool>) (x =>
      {
        if (x.Width == width)
          return x.Height == height;
        else
          return false;
      })))
      {
        width = SettingsManager.Settings.Width = SettingsManager.NativeResolution.Width;
        height = SettingsManager.Settings.Height = SettingsManager.NativeResolution.Height;
      }
      SettingsManager.DeviceManager.IsFullScreen = SettingsManager.Settings.ScreenMode == ScreenMode.Fullscreen;
      SettingsManager.DeviceManager.PreferredBackBufferWidth = width;
      SettingsManager.DeviceManager.PreferredBackBufferHeight = height;
      SettingsManager.DeviceManager.PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8;
      game.IsMouseVisible = !SettingsManager.DeviceManager.IsFullScreen;
      try
      {
        SettingsManager.DeviceManager.ApplyChanges();
      }
      catch (GraphicsModeException ex)
      {
        Logger.Log("SettingsManager", string.Format("Could not set screen to desired resolution ({0}x{1}; falling back to native", (object) SettingsManager.Settings.Width, (object) SettingsManager.Settings.Height));
        DisplayMode currentDisplayMode = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
        SettingsManager.DeviceManager.PreferredBackBufferWidth = SettingsManager.Settings.Width = currentDisplayMode.Width;
        SettingsManager.DeviceManager.PreferredBackBufferHeight = SettingsManager.Settings.Height = currentDisplayMode.Height;
        SettingsManager.DeviceManager.ApplyChanges();
      }
      Logger.Log("SettingsManager", "Screen set to " + (object) GraphicsAdapter.DefaultAdapter.CurrentDisplayMode);
      Logger.Log("SettingsManager", string.Format("Backbuffer is {0}x{1}", (object) SettingsManager.DeviceManager.GraphicsDevice.PresentationParameters.BackBufferWidth, (object) SettingsManager.DeviceManager.GraphicsDevice.PresentationParameters.BackBufferHeight));
      SettingsManager.SetupViewport(SettingsManager.DeviceManager.GraphicsDevice, false);
      SettingsManager.DeviceManager.OnDeviceReset(EventArgs.Empty);
      game.IsMouseVisible = false;
    }

    public static void SetupViewport(this GraphicsDevice device, bool forceLetterbox = false)
    {
      int backBufferWidth = device.PresentationParameters.BackBufferWidth;
      int backBufferHeight = device.PresentationParameters.BackBufferHeight;
      if (!forceLetterbox)
      {
        RenderTargetBinding[] renderTargets = device.GetRenderTargets();
        if (renderTargets.Length > 0 && renderTargets[0].RenderTarget is Texture2D)
          return;
      }
      device.ScissorRectangle = new Rectangle(0, 0, backBufferWidth, backBufferHeight);
      double num1 = (double) backBufferWidth / (double) backBufferHeight;
      int num2 = Math.Max(backBufferWidth / 640 * 640, 1280);
      int num3 = FezMath.Round((double) num2 / 1.77777779102325);
      device.Viewport = new Viewport()
      {
        X = (backBufferWidth - num2) / 2,
        Y = (backBufferHeight - num3) / 2,
        Width = num2,
        Height = num3,
        MinDepth = 0.0f,
        MaxDepth = 1f
      };
      SettingsManager.viewScale = (float) device.Viewport.Width / 1280f;
    }

    public static void UnsetupViewport(this GraphicsDevice device)
    {
      int backBufferWidth = device.PresentationParameters.BackBufferWidth;
      int backBufferHeight = device.PresentationParameters.BackBufferHeight;
      device.Viewport = new Viewport()
      {
        X = 0,
        Y = 0,
        Width = backBufferWidth,
        Height = backBufferHeight,
        MinDepth = 0.0f,
        MaxDepth = 1f
      };
      SettingsManager.viewScale = (float) device.Viewport.Width / 1280f;
    }

    public static Point PositionInViewport(this IMouseStateManager mouse)
    {
      Viewport viewport = ServiceHelper.Game.GraphicsDevice.Viewport;
      Point position = mouse.Position;
      return new Point(MathHelper.Clamp(position.X - viewport.X, 0, viewport.Width), MathHelper.Clamp(position.Y - viewport.Y, 0, viewport.Height));
    }

    public static float GetViewScale(this GraphicsDevice _)
    {
      return SettingsManager.viewScale;
    }
  }
}
