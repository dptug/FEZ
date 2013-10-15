// Type: Microsoft.Xna.Framework.Content.ContentManager
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;

namespace Microsoft.Xna.Framework.Content
{
  public class ContentManager : IDisposable
  {
    private static object ContentManagerLock = new object();
    private static List<WeakReference> ContentManagers = new List<WeakReference>();
    private string _rootDirectory = string.Empty;
    private Dictionary<string, object> loadedAssets = new Dictionary<string, object>();
    private List<IDisposable> disposableAssets = new List<IDisposable>();
    private IServiceProvider serviceProvider;
    private IGraphicsDeviceService graphicsDeviceService;
    private bool disposed;

    protected virtual Dictionary<string, object> LoadedAssets
    {
      get
      {
        return this.loadedAssets;
      }
    }

    public string RootDirectory
    {
      get
      {
        return this._rootDirectory;
      }
      set
      {
        this._rootDirectory = value;
      }
    }

    internal string RootDirectoryFullPath
    {
      get
      {
        return Path.Combine(TitleContainer.Location, this.RootDirectory);
      }
    }

    public IServiceProvider ServiceProvider
    {
      get
      {
        return this.serviceProvider;
      }
    }

    static ContentManager()
    {
    }

    public ContentManager(IServiceProvider serviceProvider)
    {
      if (serviceProvider == null)
        throw new ArgumentNullException("serviceProvider");
      this.serviceProvider = serviceProvider;
      ContentManager.AddContentManager(this);
    }

    public ContentManager(IServiceProvider serviceProvider, string rootDirectory)
    {
      if (serviceProvider == null)
        throw new ArgumentNullException("serviceProvider");
      if (rootDirectory == null)
        throw new ArgumentNullException("rootDirectory");
      this.RootDirectory = rootDirectory;
      this.serviceProvider = serviceProvider;
      ContentManager.AddContentManager(this);
    }

    ~ContentManager()
    {
      this.Dispose(false);
    }

    private static void AddContentManager(ContentManager contentManager)
    {
      lock (ContentManager.ContentManagerLock)
      {
        bool local_0 = false;
        for (int local_1 = ContentManager.ContentManagers.Count - 1; local_1 >= 0; --local_1)
        {
          WeakReference local_2 = ContentManager.ContentManagers[local_1];
          if (object.ReferenceEquals(local_2.Target, (object) contentManager))
            local_0 = true;
          if (!local_2.IsAlive)
            ContentManager.ContentManagers.RemoveAt(local_1);
        }
        if (local_0)
          return;
        ContentManager.ContentManagers.Add(new WeakReference((object) contentManager));
      }
    }

    private static void RemoveContentManager(ContentManager contentManager)
    {
      lock (ContentManager.ContentManagerLock)
      {
        for (int local_0 = ContentManager.ContentManagers.Count - 1; local_0 >= 0; --local_0)
        {
          WeakReference local_1 = ContentManager.ContentManagers[local_0];
          if (!local_1.IsAlive || object.ReferenceEquals(local_1.Target, (object) contentManager))
            ContentManager.ContentManagers.RemoveAt(local_0);
        }
      }
    }

    internal static void ReloadGraphicsContent()
    {
      lock (ContentManager.ContentManagerLock)
      {
        for (int local_0 = ContentManager.ContentManagers.Count - 1; local_0 >= 0; --local_0)
        {
          WeakReference local_1 = ContentManager.ContentManagers[local_0];
          if (local_1.IsAlive)
          {
            ContentManager local_2 = (ContentManager) local_1.Target;
            if (local_2 != null)
              local_2.ReloadGraphicsAssets();
          }
          else
            ContentManager.ContentManagers.RemoveAt(local_0);
        }
      }
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
      ContentManager.RemoveContentManager(this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this.disposed)
        return;
      this.Unload();
      this.disposed = true;
    }

    public virtual T Load<T>(string assetName)
    {
      if (string.IsNullOrEmpty(assetName))
        throw new ArgumentNullException("assetName");
      if (this.disposed)
        throw new ObjectDisposedException("ContentManager");
      T obj1 = default (T);
      object obj2 = (object) null;
      if (this.loadedAssets.TryGetValue(assetName, out obj2) && obj2 is T)
        return (T) obj2;
      T obj3 = this.ReadAsset<T>(assetName, (Action<IDisposable>) null);
      this.loadedAssets[assetName] = (object) obj3;
      return obj3;
    }

    protected virtual Stream OpenStream(string assetName)
    {
      try
      {
        return TitleContainer.OpenStream(Path.Combine(this.RootDirectory, assetName) + ".xnb");
      }
      catch (FileNotFoundException ex)
      {
        throw new ContentLoadException("The content file was not found.", (Exception) ex);
      }
      catch (DirectoryNotFoundException ex)
      {
        throw new ContentLoadException("The directory was not found.", (Exception) ex);
      }
      catch (Exception ex)
      {
        throw new ContentLoadException("Opening stream error.", ex);
      }
    }

    protected T ReadAsset<T>(string assetName, Action<IDisposable> recordDisposableObject)
    {
      if (string.IsNullOrEmpty(assetName))
        throw new ArgumentNullException("assetName");
      if (this.disposed)
        throw new ObjectDisposedException("ContentManager");
      string originalAssetName = assetName;
      object obj = (object) null;
      if (this.graphicsDeviceService == null)
      {
        this.graphicsDeviceService = this.serviceProvider.GetService(typeof (IGraphicsDeviceService)) as IGraphicsDeviceService;
        if (this.graphicsDeviceService == null)
          throw new InvalidOperationException("No Graphics Device Service");
      }
      try
      {
        Stream stream = this.OpenStream(assetName);
        try
        {
          using (BinaryReader xnbReader = new BinaryReader(stream))
          {
            using (ContentReader contentReaderFromXnb = this.GetContentReaderFromXnb(assetName, ref stream, xnbReader, recordDisposableObject))
            {
              obj = contentReaderFromXnb.ReadAsset<T>();
              if (obj is GraphicsResource)
                ((GraphicsResource) obj).Name = originalAssetName;
            }
          }
        }
        finally
        {
          if (stream != null)
            stream.Dispose();
        }
      }
      catch (ContentLoadException ex)
      {
        assetName = TitleContainer.GetFilename(Path.Combine(this.RootDirectory, assetName));
        assetName = this.Normalize<T>(assetName);
        if (string.IsNullOrEmpty(assetName))
          throw new ContentLoadException("Could not load " + originalAssetName + " asset as a non-content file!", (Exception) ex);
        obj = this.ReadRawAsset<T>(assetName, originalAssetName);
        if (obj is IDisposable)
        {
          if (recordDisposableObject != null)
            recordDisposableObject(obj as IDisposable);
          else
            this.disposableAssets.Add(obj as IDisposable);
        }
      }
      if (obj == null)
        throw new ContentLoadException("Could not load " + originalAssetName + " asset!");
      else
        return (T) obj;
    }

    protected virtual string Normalize<T>(string assetName)
    {
      if (typeof (T) == typeof (Texture2D) || typeof (T) == typeof (Texture))
        return Texture2DReader.Normalize(assetName);
      if (typeof (T) == typeof (SpriteFont))
        return SpriteFontReader.Normalize(assetName);
      if (typeof (T) == typeof (Song))
        return SongReader.Normalize(assetName);
      if (typeof (T) == typeof (SoundEffect))
        return SoundEffectReader.Normalize(assetName);
      if (typeof (T) == typeof (Video))
        return Video.Normalize(assetName);
      if (typeof (T) == typeof (Effect))
        return EffectReader.Normalize(assetName);
      else
        return (string) null;
    }

    protected virtual object ReadRawAsset<T>(string assetName, string originalAssetName)
    {
      if (typeof (T) == typeof (Texture2D) || typeof (T) == typeof (Texture))
      {
        using (Stream stream = TitleContainer.OpenStream(assetName))
        {
          Texture2D texture2D = Texture2D.FromStream(this.graphicsDeviceService.GraphicsDevice, stream);
          texture2D.Name = originalAssetName;
          return (object) texture2D;
        }
      }
      else
      {
        if (typeof (T) == typeof (SpriteFont))
          throw new NotImplementedException();
        if (typeof (T) == typeof (Song))
          return (object) new Song(assetName);
        if (typeof (T) == typeof (SoundEffect))
          return (object) new SoundEffect(assetName);
        if (typeof (T) == typeof (Video))
          return (object) new Video(assetName);
        if (!(typeof (T) == typeof (Effect)))
          return (object) null;
        using (Stream stream = TitleContainer.OpenStream(assetName))
        {
          byte[] numArray = new byte[stream.Length];
          stream.Read(numArray, 0, (int) stream.Length);
          return (object) new Effect(this.graphicsDeviceService.GraphicsDevice, numArray);
        }
      }
    }

    private ContentReader GetContentReaderFromXnb(string originalAssetName, ref Stream stream, BinaryReader xnbReader, Action<IDisposable> recordDisposableObject)
    {
      byte num1 = xnbReader.ReadByte();
      byte num2 = xnbReader.ReadByte();
      byte num3 = xnbReader.ReadByte();
      byte num4 = xnbReader.ReadByte();
      if ((int) num1 != 88 || (int) num2 != 78 || (int) num3 != 66 || (int) num4 != 119 && (int) num4 != 120 && (int) num4 != 109)
        throw new ContentLoadException("Asset does not appear to be a valid XNB file. Did you process your content for Windows?");
      byte num5 = xnbReader.ReadByte();
      bool flag = ((int) xnbReader.ReadByte() & 128) != 0;
      if ((int) num5 != 5 && (int) num5 != 4)
        throw new ContentLoadException("Invalid XNB version");
      int num6 = xnbReader.ReadInt32();
      ContentReader contentReader;
      if (flag)
      {
        int num7 = num6 - 14;
        int capacity = xnbReader.ReadInt32();
        MemoryStream memoryStream = new MemoryStream(capacity);
        LzxDecoder lzxDecoder = new LzxDecoder(16);
        int num8 = 0;
        long position = stream.Position;
        long offset = position;
        while (offset - position < (long) num7)
        {
          int num9 = stream.ReadByte();
          int num10 = stream.ReadByte();
          int inLen = num9 << 8 | num10;
          int outLen = 32768;
          long num11;
          if (num9 == (int) byte.MaxValue)
          {
            outLen = num10 << 8 | (int) (byte) stream.ReadByte();
            inLen = (int) (byte) stream.ReadByte() << 8 | (int) (byte) stream.ReadByte();
            num11 = offset + 5L;
          }
          else
            num11 = offset + 2L;
          if (inLen != 0 && outLen != 0)
          {
            lzxDecoder.Decompress(stream, inLen, (Stream) memoryStream, outLen);
            offset = num11 + (long) inLen;
            num8 += outLen;
            stream.Seek(offset, SeekOrigin.Begin);
          }
          else
            break;
        }
        if (memoryStream.Position != (long) capacity)
          throw new ContentLoadException("Decompression of " + originalAssetName + " failed. ");
        memoryStream.Seek(0L, SeekOrigin.Begin);
        contentReader = new ContentReader(this, (Stream) memoryStream, this.graphicsDeviceService.GraphicsDevice, originalAssetName, (int) num5, recordDisposableObject);
      }
      else
        contentReader = new ContentReader(this, stream, this.graphicsDeviceService.GraphicsDevice, originalAssetName, (int) num5, recordDisposableObject);
      return contentReader;
    }

    internal void RecordDisposable(IDisposable disposable)
    {
      if (this.disposableAssets.Contains(disposable))
        return;
      this.disposableAssets.Add(disposable);
    }

    protected virtual void ReloadGraphicsAssets()
    {
      foreach (KeyValuePair<string, object> keyValuePair in this.LoadedAssets)
      {
        if (keyValuePair.Value is Texture2D)
          this.ReloadAsset<Texture2D>(keyValuePair.Key, keyValuePair.Value as Texture2D);
        else if (keyValuePair.Value is SpriteFont)
          this.ReloadAsset<SpriteFont>(keyValuePair.Key, keyValuePair.Value as SpriteFont);
        else if (keyValuePair.Value is Model)
          this.ReloadAsset<Model>(keyValuePair.Key, keyValuePair.Value as Model);
      }
    }

    protected virtual void ReloadAsset<T>(string originalAssetName, T currentAsset)
    {
      string str = originalAssetName;
      if (string.IsNullOrEmpty(str))
        throw new ArgumentNullException("assetName");
      if (this.disposed)
        throw new ObjectDisposedException("ContentManager");
      if (this.graphicsDeviceService == null)
      {
        this.graphicsDeviceService = this.serviceProvider.GetService(typeof (IGraphicsDeviceService)) as IGraphicsDeviceService;
        if (this.graphicsDeviceService == null)
          throw new InvalidOperationException("No Graphics Device Service");
      }
      try
      {
        Stream stream = this.OpenStream(str);
        try
        {
          using (BinaryReader xnbReader = new BinaryReader(stream))
          {
            using (ContentReader contentReaderFromXnb = this.GetContentReaderFromXnb(str, ref stream, xnbReader, (Action<IDisposable>) null))
            {
              contentReaderFromXnb.InitializeTypeReaders();
              contentReaderFromXnb.ReadObject<T>(currentAsset);
              contentReaderFromXnb.ReadSharedResources();
            }
          }
        }
        finally
        {
          if (stream != null)
            stream.Dispose();
        }
      }
      catch (ContentLoadException ex)
      {
        string assetName = this.Normalize<T>(TitleContainer.GetFilename(Path.Combine(this.RootDirectory, str)));
        this.ReloadRawAsset<T>(currentAsset, assetName, originalAssetName);
      }
    }

    protected virtual void ReloadRawAsset<T>(T asset, string assetName, string originalAssetName)
    {
      if (!((object) asset is Texture2D))
        return;
      using (Stream textureStream = TitleContainer.OpenStream(assetName))
        ((object) asset as Texture2D).Reload(textureStream);
    }

    public virtual void Unload()
    {
      foreach (IDisposable disposable in this.disposableAssets)
      {
        if (disposable != null)
          disposable.Dispose();
      }
      this.disposableAssets.Clear();
      this.loadedAssets.Clear();
    }
  }
}
