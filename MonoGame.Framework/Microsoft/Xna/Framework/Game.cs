// Type: Microsoft.Xna.Framework.Game
// Assembly: MonoGame.Framework, Version=3.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 69677294-4E99-4B9C-B72B-CC2D8AA03B63
// Assembly location: F:\Program Files (x86)\FEZ\MonoGame.Framework.dll

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace Microsoft.Xna.Framework
{
  public class Game : IDisposable
  {
    private static Game _instance = (Game) null;
    private static readonly Action<IDrawable, GameTime> DrawAction = (Action<IDrawable, GameTime>) ((drawable, gameTime) => drawable.Draw(gameTime));
    private static readonly Action<IUpdateable, GameTime> UpdateAction = (Action<IUpdateable, GameTime>) ((updateable, gameTime) => updateable.Update(gameTime));
    private Game.SortingFilteringCollection<IDrawable> _drawables = new Game.SortingFilteringCollection<IDrawable>((Predicate<IDrawable>) (d => d.Visible), (Action<IDrawable, EventHandler<EventArgs>>) ((d, handler) => d.VisibleChanged += handler), (Action<IDrawable, EventHandler<EventArgs>>) ((d, handler) => d.VisibleChanged -= handler), (Comparison<IDrawable>) ((d1, d2) => Comparer<int>.Default.Compare(d1.DrawOrder, d2.DrawOrder)), (Action<IDrawable, EventHandler<EventArgs>>) ((d, handler) => d.DrawOrderChanged += handler), (Action<IDrawable, EventHandler<EventArgs>>) ((d, handler) => d.DrawOrderChanged -= handler));
    private Game.SortingFilteringCollection<IUpdateable> _updateables = new Game.SortingFilteringCollection<IUpdateable>((Predicate<IUpdateable>) (u => u.Enabled), (Action<IUpdateable, EventHandler<EventArgs>>) ((u, handler) => u.EnabledChanged += handler), (Action<IUpdateable, EventHandler<EventArgs>>) ((u, handler) => u.EnabledChanged -= handler), (Comparison<IUpdateable>) ((u1, u2) => Comparer<int>.Default.Compare(u1.UpdateOrder, u2.UpdateOrder)), (Action<IUpdateable, EventHandler<EventArgs>>) ((u, handler) => u.UpdateOrderChanged += handler), (Action<IUpdateable, EventHandler<EventArgs>>) ((u, handler) => u.UpdateOrderChanged -= handler));
    private bool _isFixedTimeStep = true;
    private TimeSpan _targetElapsedTime = TimeSpan.FromTicks(166666L);
    private TimeSpan _inactiveSleepTime = TimeSpan.FromSeconds(1.0);
    private readonly TimeSpan _maxElapsedTime = TimeSpan.FromMilliseconds(500.0);
    private readonly GameTime _gameTime = new GameTime();
    private Stopwatch _gameTimer = Stopwatch.StartNew();
    private const float DefaultTargetFramesPerSecond = 60f;
    private GameComponentCollection _components;
    private GameServiceContainer _services;
    internal GamePlatform Platform;
    private IGraphicsDeviceManager _graphicsDeviceManager;
    private IGraphicsDeviceService _graphicsDeviceService;
    private bool _initialized;
    private bool _suppressDraw;
    private bool _isDisposed;
    private TimeSpan _accumulatedElapsedTime;
    public bool AssumeTargetElapsedTime;

    internal static Game Instance
    {
      get
      {
        return Game._instance;
      }
    }

    public LaunchParameters LaunchParameters { get; private set; }

    public GameComponentCollection Components
    {
      get
      {
        return this._components;
      }
    }

    public TimeSpan InactiveSleepTime
    {
      get
      {
        return this._inactiveSleepTime;
      }
      set
      {
        if (!(this._inactiveSleepTime != value))
          return;
        this._inactiveSleepTime = value;
      }
    }

    public bool IsActive
    {
      get
      {
        return this.Platform.IsActive;
      }
    }

    public bool IsMouseVisible
    {
      get
      {
        return this.Platform.IsMouseVisible;
      }
      set
      {
        this.Platform.IsMouseVisible = value;
      }
    }

    public TimeSpan TargetElapsedTime
    {
      get
      {
        return this._targetElapsedTime;
      }
      set
      {
        value = this.Platform.TargetElapsedTimeChanging(value);
        if (value <= TimeSpan.Zero)
          throw new ArgumentOutOfRangeException("value must be positive and non-zero.");
        if (!(value != this._targetElapsedTime))
          return;
        this._targetElapsedTime = value;
        this.Platform.TargetElapsedTimeChanged();
      }
    }

    public bool IsFixedTimeStep
    {
      get
      {
        return this._isFixedTimeStep;
      }
      set
      {
        this._isFixedTimeStep = value;
      }
    }

    public GameServiceContainer Services
    {
      get
      {
        return this._services;
      }
    }

    public ContentManager Content { get; set; }

    public GraphicsDevice GraphicsDevice
    {
      get
      {
        if (this._graphicsDeviceService == null)
        {
          this._graphicsDeviceService = (IGraphicsDeviceService) this.Services.GetService(typeof (IGraphicsDeviceService));
          if (this._graphicsDeviceService == null)
            throw new InvalidOperationException("No Graphics Device Service");
        }
        return this._graphicsDeviceService.GraphicsDevice;
      }
    }

    public GameWindow Window
    {
      get
      {
        return this.Platform.Window;
      }
    }

    internal bool Initialized
    {
      get
      {
        return this._initialized;
      }
    }

    internal GraphicsDeviceManager graphicsDeviceManager
    {
      get
      {
        if (this._graphicsDeviceManager == null)
        {
          this._graphicsDeviceManager = (IGraphicsDeviceManager) this.Services.GetService(typeof (IGraphicsDeviceManager));
          if (this._graphicsDeviceManager == null)
            throw new InvalidOperationException("No Graphics Device Manager");
        }
        return (GraphicsDeviceManager) this._graphicsDeviceManager;
      }
    }

    public event EventHandler<EventArgs> Activated;

    public event EventHandler<EventArgs> Deactivated;

    public event EventHandler<EventArgs> Disposed;

    public event EventHandler<EventArgs> Exiting;

    static Game()
    {
    }

    public Game()
    {
      Game._instance = this;
      this.LaunchParameters = new LaunchParameters();
      this._services = new GameServiceContainer();
      this._components = new GameComponentCollection();
      this.Content = new ContentManager((IServiceProvider) this._services);
      this.Platform = GamePlatform.Create(this);
      this.Platform.Activated += new EventHandler<EventArgs>(this.OnActivated);
      this.Platform.Deactivated += new EventHandler<EventArgs>(this.OnDeactivated);
      this._services.AddService(typeof (GamePlatform), (object) this.Platform);
      string str = string.Empty;
      Assembly entryAssembly = Assembly.GetEntryAssembly();
      AssemblyTitleAttribute assemblyTitleAttribute = (AssemblyTitleAttribute) System.Attribute.GetCustomAttribute(entryAssembly, typeof (AssemblyTitleAttribute));
      if (assemblyTitleAttribute != null)
        str = assemblyTitleAttribute.Title;
      if (string.IsNullOrEmpty(str))
        str = entryAssembly.GetName().Name;
      this.Window.Title = str;
    }

    ~Game()
    {
      this.Dispose(false);
    }

    [Conditional("DEBUG")]
    internal void Log(string Message)
    {
      GamePlatform gamePlatform = this.Platform;
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
      this.Raise<EventArgs>(this.Disposed, EventArgs.Empty);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (this._isDisposed)
        return;
      if (disposing)
      {
        for (int index = 0; index < this._components.Count; ++index)
        {
          IDisposable disposable = this._components[index] as IDisposable;
          if (disposable != null)
            disposable.Dispose();
        }
        if (this.Content != null)
          this.Content.Dispose();
        if (this._graphicsDeviceManager != null)
        {
          (this._graphicsDeviceManager as GraphicsDeviceManager).Dispose();
          this._graphicsDeviceManager = (IGraphicsDeviceManager) null;
        }
        this.Platform.Dispose();
      }
      this._isDisposed = true;
    }

    [DebuggerNonUserCode]
    private void AssertNotDisposed()
    {
      if (!this._isDisposed)
        return;
      string name = this.GetType().Name;
      throw new ObjectDisposedException(name, string.Format("The {0} object was used after being Disposed.", (object) name));
    }

    public void Exit()
    {
      this.Platform.Exit();
    }

    public void ResetElapsedTime()
    {
      this.Platform.ResetElapsedTime();
      this._gameTimer.Reset();
      this._gameTimer.Start();
      this._accumulatedElapsedTime = TimeSpan.Zero;
      this._gameTime.ElapsedGameTime = TimeSpan.Zero;
    }

    public void SuppressDraw()
    {
      this._suppressDraw = true;
    }

    public void RunOneFrame()
    {
      this.AssertNotDisposed();
      if (!this.Platform.BeforeRun())
        return;
      if (!this._initialized)
      {
        this.DoInitialize();
        this._initialized = true;
      }
      this.BeginRun();
      this.Tick();
      this.EndRun();
    }

    public void Run()
    {
      this.Run(this.Platform.DefaultRunBehavior);
    }

    public void Run(GameRunBehavior runBehavior)
    {
      this.AssertNotDisposed();
      if (!this.Platform.BeforeRun())
        return;
      if (!this._initialized)
      {
        this.DoInitialize();
        this._initialized = true;
      }
      this.BeginRun();
      switch (runBehavior)
      {
        case GameRunBehavior.Asynchronous:
          this.Platform.AsyncRunLoopEnded += new EventHandler<EventArgs>(this.Platform_AsyncRunLoopEnded);
          this.Platform.StartRunLoop();
          break;
        case GameRunBehavior.Synchronous:
          this.Platform.RunLoop();
          this.EndRun();
          this.DoExiting();
          break;
        default:
          throw new NotImplementedException(string.Format("Handling for the run behavior {0} is not implemented.", (object) runBehavior));
      }
    }

    public void Tick()
    {
      while (true)
      {
        if (this.AssumeTargetElapsedTime)
          this._accumulatedElapsedTime += this._targetElapsedTime;
        else
          this._accumulatedElapsedTime += this._gameTimer.Elapsed;
        this._gameTimer.Reset();
        this._gameTimer.Start();
        if (this.IsFixedTimeStep && this._accumulatedElapsedTime < this.TargetElapsedTime)
          Thread.Sleep((int) (this.TargetElapsedTime - this._accumulatedElapsedTime).TotalMilliseconds);
        else
          break;
      }
      if (this._accumulatedElapsedTime > this._maxElapsedTime)
        this._accumulatedElapsedTime = this._maxElapsedTime;
      if (this.IsFixedTimeStep)
      {
        this._gameTime.ElapsedGameTime = this.TargetElapsedTime;
        int num = 0;
        this._gameTime.IsRunningSlowly = this._accumulatedElapsedTime > this.TargetElapsedTime;
        while (this._accumulatedElapsedTime >= this.TargetElapsedTime)
        {
          this._gameTime.TotalGameTime += this.TargetElapsedTime;
          this._accumulatedElapsedTime -= this.TargetElapsedTime;
          ++num;
          this.DoUpdate(this._gameTime);
        }
        this._gameTime.ElapsedGameTime = TimeSpan.FromTicks(this.TargetElapsedTime.Ticks * (long) num);
      }
      else
      {
        this._gameTime.ElapsedGameTime = this._accumulatedElapsedTime;
        this._gameTime.TotalGameTime += this._accumulatedElapsedTime;
        this._accumulatedElapsedTime = TimeSpan.Zero;
        this._gameTime.IsRunningSlowly = false;
        this.DoUpdate(this._gameTime);
      }
      if (this._suppressDraw)
      {
        this._suppressDraw = false;
      }
      else
      {
        lock (Threading.BackgroundContext)
          this.DoDraw(this._gameTime);
      }
    }

    protected virtual bool BeginDraw()
    {
      return true;
    }

    protected virtual void EndDraw()
    {
      this.Platform.Present();
    }

    protected virtual void BeginRun()
    {
    }

    protected virtual void EndRun()
    {
    }

    protected virtual void LoadContent()
    {
    }

    protected virtual void UnloadContent()
    {
    }

    protected virtual void Initialize()
    {
      this.applyChanges(this.graphicsDeviceManager);
      this.CategorizeComponents();
      this._components.ComponentAdded += new EventHandler<GameComponentCollectionEventArgs>(this.Components_ComponentAdded);
      this._components.ComponentRemoved += new EventHandler<GameComponentCollectionEventArgs>(this.Components_ComponentRemoved);
      this.InitializeExistingComponents();
      this._graphicsDeviceService = (IGraphicsDeviceService) this.Services.GetService(typeof (IGraphicsDeviceService));
      if (this._graphicsDeviceService == null || this._graphicsDeviceService.GraphicsDevice == null)
        return;
      this.LoadContent();
    }

    protected virtual void Draw(GameTime gameTime)
    {
      this._drawables.ForEachFilteredItem<GameTime>(Game.DrawAction, gameTime);
    }

    protected virtual void Update(GameTime gameTime)
    {
      this._updateables.ForEachFilteredItem<GameTime>(Game.UpdateAction, gameTime);
    }

    protected virtual void OnExiting(object sender, EventArgs args)
    {
      this.Raise<EventArgs>(this.Exiting, args);
    }

    protected virtual void OnActivated(object sender, EventArgs args)
    {
      this.AssertNotDisposed();
      this.Raise<EventArgs>(this.Activated, args);
    }

    protected virtual void OnDeactivated(object sender, EventArgs args)
    {
      this.AssertNotDisposed();
      this.Raise<EventArgs>(this.Deactivated, args);
    }

    private void Components_ComponentAdded(object sender, GameComponentCollectionEventArgs e)
    {
      e.GameComponent.Initialize();
      this.CategorizeComponent(e.GameComponent);
    }

    private void Components_ComponentRemoved(object sender, GameComponentCollectionEventArgs e)
    {
      this.DecategorizeComponent(e.GameComponent);
    }

    private void Platform_AsyncRunLoopEnded(object sender, EventArgs e)
    {
      this.AssertNotDisposed();
      ((GamePlatform) sender).AsyncRunLoopEnded -= new EventHandler<EventArgs>(this.Platform_AsyncRunLoopEnded);
      this.EndRun();
      this.DoExiting();
    }

    internal void applyChanges(GraphicsDeviceManager manager)
    {
      this.Platform.BeginScreenDeviceChange(this.GraphicsDevice.PresentationParameters.IsFullScreen);
      if (this.GraphicsDevice.PresentationParameters.IsFullScreen)
        this.Platform.EnterFullScreen();
      else
        this.Platform.ExitFullScreen();
      Viewport viewport = new Viewport(0, 0, this.GraphicsDevice.PresentationParameters.BackBufferWidth, this.GraphicsDevice.PresentationParameters.BackBufferHeight);
      this.GraphicsDevice.Viewport = viewport;
      this.Platform.EndScreenDeviceChange(string.Empty, viewport.Width, viewport.Height);
    }

    internal void DoUpdate(GameTime gameTime)
    {
      this.AssertNotDisposed();
      if (!this.Platform.BeforeUpdate(gameTime))
        return;
      this.Update(gameTime);
    }

    internal void DoDraw(GameTime gameTime)
    {
      this.AssertNotDisposed();
      if (!this.Platform.BeforeDraw(gameTime) || !this.BeginDraw())
        return;
      this.Draw(gameTime);
      this.EndDraw();
    }

    internal void DoInitialize()
    {
      this.AssertNotDisposed();
      this.Platform.BeforeInitialize();
      this.Initialize();
    }

    internal void DoExiting()
    {
      this.OnExiting((object) this, EventArgs.Empty);
      this.UnloadContent();
    }

    internal void ResizeWindow(bool changed)
    {
      ((OpenTKGamePlatform) this.Platform).ResetWindowBounds(changed);
    }

    private void InitializeExistingComponents()
    {
      IGameComponent[] array = new IGameComponent[this.Components.Count];
      this.Components.CopyTo(array, 0);
      foreach (IGameComponent gameComponent in array)
        gameComponent.Initialize();
    }

    private void CategorizeComponents()
    {
      this.DecategorizeComponents();
      for (int index = 0; index < this.Components.Count; ++index)
        this.CategorizeComponent(this.Components[index]);
    }

    private void DecategorizeComponents()
    {
      this._updateables.Clear();
      this._drawables.Clear();
    }

    private void CategorizeComponent(IGameComponent component)
    {
      if (component is IUpdateable)
        this._updateables.Add((IUpdateable) component);
      if (!(component is IDrawable))
        return;
      this._drawables.Add((IDrawable) component);
    }

    private void DecategorizeComponent(IGameComponent component)
    {
      if (component is IUpdateable)
        this._updateables.Remove((IUpdateable) component);
      if (!(component is IDrawable))
        return;
      this._drawables.Remove((IDrawable) component);
    }

    private void Raise<TEventArgs>(EventHandler<TEventArgs> handler, TEventArgs e) where TEventArgs : EventArgs
    {
      if (handler == null)
        return;
      handler((object) this, e);
    }

    private class SortingFilteringCollection<T> : ICollection<T>, IEnumerable<T>, IEnumerable
    {
      private static readonly Comparison<int> RemoveJournalSortComparison = (Comparison<int>) ((x, y) => Comparer<int>.Default.Compare(y, x));
      private readonly List<T> _items;
      private readonly List<Game.AddJournalEntry> _addJournal;
      private readonly Comparison<Game.AddJournalEntry> _addJournalSortComparison;
      private readonly List<int> _removeJournal;
      private readonly List<T> _cachedFilteredItems;
      private bool _shouldRebuildCache;
      private readonly Predicate<T> _filter;
      private readonly Comparison<T> _sort;
      private readonly Action<T, EventHandler<EventArgs>> _filterChangedSubscriber;
      private readonly Action<T, EventHandler<EventArgs>> _filterChangedUnsubscriber;
      private readonly Action<T, EventHandler<EventArgs>> _sortChangedSubscriber;
      private readonly Action<T, EventHandler<EventArgs>> _sortChangedUnsubscriber;

      public int Count
      {
        get
        {
          return this._items.Count;
        }
      }

      public bool IsReadOnly
      {
        get
        {
          return false;
        }
      }

      static SortingFilteringCollection()
      {
      }

      public SortingFilteringCollection(Predicate<T> filter, Action<T, EventHandler<EventArgs>> filterChangedSubscriber, Action<T, EventHandler<EventArgs>> filterChangedUnsubscriber, Comparison<T> sort, Action<T, EventHandler<EventArgs>> sortChangedSubscriber, Action<T, EventHandler<EventArgs>> sortChangedUnsubscriber)
      {
        this._items = new List<T>();
        this._addJournal = new List<Game.AddJournalEntry>();
        this._removeJournal = new List<int>();
        this._cachedFilteredItems = new List<T>();
        this._shouldRebuildCache = true;
        this._filter = filter;
        this._filterChangedSubscriber = filterChangedSubscriber;
        this._filterChangedUnsubscriber = filterChangedUnsubscriber;
        this._sort = sort;
        this._sortChangedSubscriber = sortChangedSubscriber;
        this._sortChangedUnsubscriber = sortChangedUnsubscriber;
        this._addJournalSortComparison = new Comparison<Game.AddJournalEntry>(this.CompareAddJournalEntry);
      }

      private int CompareAddJournalEntry(Game.AddJournalEntry x, Game.AddJournalEntry y)
      {
        int num = this._sort((T) x.Item, (T) y.Item);
        if (num != 0)
          return num;
        else
          return x.Order - y.Order;
      }

      public void ForEachFilteredItem<TUserData>(Action<T, TUserData> action, TUserData userData)
      {
        if (this._shouldRebuildCache)
        {
          this.ProcessRemoveJournal();
          this.ProcessAddJournal();
          this._cachedFilteredItems.Clear();
          for (int index = 0; index < this._items.Count; ++index)
          {
            if (this._filter(this._items[index]))
              this._cachedFilteredItems.Add(this._items[index]);
          }
          this._shouldRebuildCache = false;
        }
        for (int index = 0; index < this._cachedFilteredItems.Count; ++index)
          action(this._cachedFilteredItems[index], userData);
        if (!this._shouldRebuildCache)
          return;
        this._cachedFilteredItems.Clear();
      }

      public void Add(T item)
      {
        this._addJournal.Add(new Game.AddJournalEntry(this._addJournal.Count, (object) item));
        this.InvalidateCache();
      }

      public bool Remove(T item)
      {
        if (this._addJournal.Remove(Game.AddJournalEntry.CreateKey((object) item)))
          return true;
        int num = this._items.IndexOf(item);
        if (num < 0)
          return false;
        this.UnsubscribeFromItemEvents(item);
        this._removeJournal.Add(num);
        this.InvalidateCache();
        return true;
      }

      public void Clear()
      {
        for (int index = 0; index < this._items.Count; ++index)
        {
          this._filterChangedUnsubscriber(this._items[index], new EventHandler<EventArgs>(this.Item_FilterPropertyChanged));
          this._sortChangedUnsubscriber(this._items[index], new EventHandler<EventArgs>(this.Item_SortPropertyChanged));
        }
        this._addJournal.Clear();
        this._removeJournal.Clear();
        this._items.Clear();
        this.InvalidateCache();
      }

      public bool Contains(T item)
      {
        return this._items.Contains(item);
      }

      public void CopyTo(T[] array, int arrayIndex)
      {
        this._items.CopyTo(array, arrayIndex);
      }

      public IEnumerator<T> GetEnumerator()
      {
        return (IEnumerator<T>) this._items.GetEnumerator();
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
        return this._items.GetEnumerator();
      }

      private void ProcessRemoveJournal()
      {
        if (this._removeJournal.Count == 0)
          return;
        this._removeJournal.Sort(Game.SortingFilteringCollection<T>.RemoveJournalSortComparison);
        for (int index1 = 0; index1 < this._removeJournal.Count; ++index1)
        {
          int index2 = this._removeJournal[index1];
          if (index2 < this._items.Count)
            this._items.RemoveAt(index2);
        }
        this._removeJournal.Clear();
      }

      private void ProcessAddJournal()
      {
        if (this._addJournal.Count == 0)
          return;
        this._addJournal.Sort(this._addJournalSortComparison);
        int index1 = 0;
        for (int index2 = 0; index2 < this._items.Count && index1 < this._addJournal.Count; ++index2)
        {
          T x = (T) this._addJournal[index1].Item;
          if (this._sort(x, this._items[index2]) < 0)
          {
            this.SubscribeToItemEvents(x);
            this._items.Insert(index2, x);
            ++index1;
          }
        }
        for (; index1 < this._addJournal.Count; ++index1)
        {
          T obj = (T) this._addJournal[index1].Item;
          this.SubscribeToItemEvents(obj);
          this._items.Add(obj);
        }
        this._addJournal.Clear();
      }

      private void SubscribeToItemEvents(T item)
      {
        this._filterChangedSubscriber(item, new EventHandler<EventArgs>(this.Item_FilterPropertyChanged));
        this._sortChangedSubscriber(item, new EventHandler<EventArgs>(this.Item_SortPropertyChanged));
      }

      private void UnsubscribeFromItemEvents(T item)
      {
        this._filterChangedUnsubscriber(item, new EventHandler<EventArgs>(this.Item_FilterPropertyChanged));
        this._sortChangedUnsubscriber(item, new EventHandler<EventArgs>(this.Item_SortPropertyChanged));
      }

      private void InvalidateCache()
      {
        this._shouldRebuildCache = true;
      }

      private void Item_FilterPropertyChanged(object sender, EventArgs e)
      {
        this.InvalidateCache();
      }

      private void Item_SortPropertyChanged(object sender, EventArgs e)
      {
        T obj = (T) sender;
        int num = this._items.IndexOf(obj);
        this._addJournal.Add(new Game.AddJournalEntry(this._addJournal.Count, (object) obj));
        this._removeJournal.Add(num);
        this.UnsubscribeFromItemEvents(obj);
        this.InvalidateCache();
      }
    }

    private struct AddJournalEntry
    {
      public readonly int Order;
      public readonly object Item;

      public AddJournalEntry(int order, object item)
      {
        this.Order = order;
        this.Item = item;
      }

      public static Game.AddJournalEntry CreateKey(object item)
      {
        return new Game.AddJournalEntry(-1, item);
      }

      public override int GetHashCode()
      {
        return this.Item.GetHashCode();
      }

      public override bool Equals(object obj)
      {
        if (!(obj is Game.AddJournalEntry))
          return false;
        else
          return object.Equals(this.Item, ((Game.AddJournalEntry) obj).Item);
      }
    }
  }
}
