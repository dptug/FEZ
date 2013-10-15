// Type: Microsoft.Xna.Framework.Storage.StorageDevice
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework;
using System;
using System.IO;
using System.Runtime.Remoting.Messaging;

namespace Microsoft.Xna.Framework.Storage
{
  public sealed class StorageDevice
  {
    private PlayerIndex? player;
    private int sizeInBytes;
    private int directoryCount;
    private StorageContainer storageContainer;

    public long FreeSpace
    {
      get
      {
        try
        {
          return new DriveInfo(this.GetDevicePath).AvailableFreeSpace;
        }
        catch (Exception ex)
        {
          StorageDeviceHelper.Path = StorageDevice.StorageRoot;
          return StorageDeviceHelper.FreeSpace;
        }
      }
    }

    public bool IsConnected
    {
      get
      {
        try
        {
          return new DriveInfo(this.GetDevicePath).IsReady;
        }
        catch (Exception ex)
        {
          return true;
        }
      }
    }

    public long TotalSpace
    {
      get
      {
        try
        {
          return new DriveInfo(this.GetDevicePath).TotalSize;
        }
        catch (Exception ex)
        {
          StorageDeviceHelper.Path = StorageDevice.StorageRoot;
          return StorageDeviceHelper.TotalSpace;
        }
      }
    }

    private string GetDevicePath
    {
      get
      {
        if (this.storageContainer == null)
          return StorageDevice.StorageRoot;
        else
          return this.storageContainer._storagePath;
      }
    }

    internal static string StorageRoot
    {
      get
      {
        return Environment.GetFolderPath(Environment.SpecialFolder.Personal);
      }
    }

    public static event EventHandler<EventArgs> DeviceChanged;

    internal StorageDevice(PlayerIndex? player, int sizeInBytes, int directoryCount)
    {
      this.player = player;
      this.sizeInBytes = sizeInBytes;
      this.directoryCount = directoryCount;
    }

    public IAsyncResult BeginOpenContainer(string displayName, AsyncCallback callback, object state)
    {
      return this.OpenContainer(displayName, callback, state);
    }

    private IAsyncResult OpenContainer(string displayName, AsyncCallback callback, object state)
    {
      return new OpenContainerAsynchronous(this.Open).BeginInvoke(displayName, callback, state);
    }

    private StorageContainer Open(string displayName)
    {
      this.storageContainer = new StorageContainer(this, displayName, this.player);
      return this.storageContainer;
    }

    public static IAsyncResult BeginShowSelector(AsyncCallback callback, object state)
    {
      return StorageDevice.BeginShowSelector(0, 0, callback, state);
    }

    public static IAsyncResult BeginShowSelector(PlayerIndex player, AsyncCallback callback, object state)
    {
      return StorageDevice.BeginShowSelector(player, 0, 0, callback, state);
    }

    public static IAsyncResult BeginShowSelector(int sizeInBytes, int directoryCount, AsyncCallback callback, object state)
    {
      return new ShowSelectorAsynchronousShowNoPlayer(StorageDevice.Show).BeginInvoke(sizeInBytes, directoryCount, callback, state);
    }

    public static IAsyncResult BeginShowSelector(PlayerIndex player, int sizeInBytes, int directoryCount, AsyncCallback callback, object state)
    {
      return new ShowSelectorAsynchronousShow(StorageDevice.Show).BeginInvoke(player, sizeInBytes, directoryCount, callback, state);
    }

    private static StorageDevice Show(PlayerIndex player, int sizeInBytes, int directoryCount)
    {
      return new StorageDevice(new PlayerIndex?(player), sizeInBytes, directoryCount);
    }

    private static StorageDevice Show(int sizeInBytes, int directoryCount)
    {
      return new StorageDevice(new PlayerIndex?(), sizeInBytes, directoryCount);
    }

    public void DeleteContainer(string titleName)
    {
      throw new NotImplementedException();
    }

    public StorageContainer EndOpenContainer(IAsyncResult result)
    {
      StorageContainer storageContainer = (StorageContainer) null;
      try
      {
        AsyncResult asyncResult = result as AsyncResult;
        if (asyncResult != null)
        {
          OpenContainerAsynchronous containerAsynchronous = asyncResult.AsyncDelegate as OpenContainerAsynchronous;
          result.AsyncWaitHandle.WaitOne();
          if (containerAsynchronous != null)
            storageContainer = containerAsynchronous.EndInvoke(result);
        }
      }
      finally
      {
        result.AsyncWaitHandle.Dispose();
      }
      return storageContainer;
    }

    public static StorageDevice EndShowSelector(IAsyncResult result)
    {
      if (!result.IsCompleted)
      {
        try
        {
          result.AsyncWaitHandle.WaitOne();
        }
        finally
        {
          result.AsyncWaitHandle.Close();
        }
      }
      object asyncDelegate = ((AsyncResult) result).AsyncDelegate;
      if (asyncDelegate is ShowSelectorAsynchronousShow)
        return (asyncDelegate as ShowSelectorAsynchronousShow).EndInvoke(result);
      if (asyncDelegate is ShowSelectorAsynchronousShowNoPlayer)
        return (asyncDelegate as ShowSelectorAsynchronousShowNoPlayer).EndInvoke(result);
      else
        throw new ArgumentException("result");
    }
  }
}
