// Type: Microsoft.Xna.Framework.Storage.StorageContainer
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework;
using System;
using System.IO;

namespace Microsoft.Xna.Framework.Storage
{
  public class StorageContainer : IDisposable
  {
    internal readonly string _storagePath;
    private readonly StorageDevice _device;
    private readonly string _name;
    private readonly PlayerIndex? _playerIndex;

    public string DisplayName
    {
      get
      {
        return this._name;
      }
    }

    public bool IsDisposed { get; private set; }

    public StorageDevice StorageDevice
    {
      get
      {
        return this._device;
      }
    }

    public event EventHandler<EventArgs> Disposing;

    internal StorageContainer(StorageDevice device, string name, PlayerIndex? playerIndex)
    {
      if (string.IsNullOrEmpty(name))
        throw new ArgumentNullException("A title name has to be provided in parameter name.");
      this._device = device;
      this._name = name;
      this._playerIndex = playerIndex;
      this._storagePath = Path.Combine(Path.Combine(StorageDevice.StorageRoot, "SavedGames"), name);
      string str = string.Empty;
      if (playerIndex.HasValue)
        str = Path.Combine(this._storagePath, "Player" + (object) playerIndex.Value);
      if (!string.IsNullOrEmpty(str))
        this._storagePath = Path.Combine(this._storagePath, "Player" + (object) playerIndex.Value);
      if (Directory.Exists(this._storagePath))
        return;
      Directory.CreateDirectory(this._storagePath);
    }

    public void CreateDirectory(string directory)
    {
      if (string.IsNullOrEmpty(directory))
        throw new ArgumentNullException("Parameter directory must contain a value.");
      Directory.CreateDirectory(Path.Combine(this._storagePath, directory));
    }

    public Stream CreateFile(string file)
    {
      if (string.IsNullOrEmpty(file))
        throw new ArgumentNullException("Parameter file must contain a value.");
      else
        return (Stream) File.Create(Path.Combine(this._storagePath, file));
    }

    public void DeleteDirectory(string directory)
    {
      if (string.IsNullOrEmpty(directory))
        throw new ArgumentNullException("Parameter directory must contain a value.");
      Directory.Delete(Path.Combine(this._storagePath, directory));
    }

    public void DeleteFile(string file)
    {
      if (string.IsNullOrEmpty(file))
        throw new ArgumentNullException("Parameter file must contain a value.");
      File.Delete(Path.Combine(this._storagePath, file));
    }

    public bool DirectoryExists(string directory)
    {
      if (string.IsNullOrEmpty(directory))
        throw new ArgumentNullException("Parameter directory must contain a value.");
      else
        return Directory.Exists(Path.Combine(this._storagePath, directory));
    }

    public void Dispose()
    {
      this.IsDisposed = true;
    }

    public bool FileExists(string file)
    {
      if (string.IsNullOrEmpty(file))
        throw new ArgumentNullException("Parameter file must contain a value.");
      else
        return File.Exists(Path.Combine(this._storagePath, file));
    }

    public string[] GetDirectoryNames()
    {
      return Directory.GetDirectories(this._storagePath);
    }

    public string[] GetDirectoryNames(string searchPattern)
    {
      throw new NotImplementedException();
    }

    public string[] GetFileNames()
    {
      return Directory.GetFiles(this._storagePath);
    }

    public string[] GetFileNames(string searchPattern)
    {
      if (string.IsNullOrEmpty(searchPattern))
        throw new ArgumentNullException("Parameter searchPattern must contain a value.");
      else
        return Directory.GetFiles(this._storagePath, searchPattern);
    }

    public Stream OpenFile(string file, FileMode fileMode)
    {
      return this.OpenFile(file, fileMode, FileAccess.ReadWrite, FileShare.ReadWrite);
    }

    public Stream OpenFile(string file, FileMode fileMode, FileAccess fileAccess)
    {
      return this.OpenFile(file, fileMode, fileAccess, FileShare.ReadWrite);
    }

    public Stream OpenFile(string file, FileMode fileMode, FileAccess fileAccess, FileShare fileShare)
    {
      if (string.IsNullOrEmpty(file))
        throw new ArgumentNullException("Parameter file must contain a value.");
      else
        return (Stream) File.Open(Path.Combine(this._storagePath, file), fileMode, fileAccess, fileShare);
    }
  }
}
