// Type: Newtonsoft.Json.Linq.JContainer
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;
using Newtonsoft.Json.Utilities.LinqBridge;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Threading;

namespace Newtonsoft.Json.Linq
{
  public abstract class JContainer : JToken, IList<JToken>, ICollection<JToken>, IEnumerable<JToken>, ITypedList, IBindingList, IList, ICollection, IEnumerable
  {
    internal ListChangedEventHandler _listChanged;
    internal AddingNewEventHandler _addingNew;
    private object _syncRoot;
    private bool _busy;

    protected abstract IList<JToken> ChildrenTokens { get; }

    public override bool HasValues
    {
      get
      {
        return this.ChildrenTokens.Count > 0;
      }
    }

    public override JToken First
    {
      get
      {
        return Enumerable.FirstOrDefault<JToken>((IEnumerable<JToken>) this.ChildrenTokens);
      }
    }

    public override JToken Last
    {
      get
      {
        return Enumerable.LastOrDefault<JToken>((IEnumerable<JToken>) this.ChildrenTokens);
      }
    }

    bool ICollection<JToken>.IsReadOnly
    {
      get
      {
        return false;
      }
    }

    bool IList.IsFixedSize
    {
      get
      {
        return false;
      }
    }

    bool IList.IsReadOnly
    {
      get
      {
        return false;
      }
    }

    public int Count
    {
      get
      {
        return this.ChildrenTokens.Count;
      }
    }

    bool ICollection.IsSynchronized
    {
      get
      {
        return false;
      }
    }

    object ICollection.SyncRoot
    {
      get
      {
        if (this._syncRoot == null)
          Interlocked.CompareExchange(ref this._syncRoot, new object(), (object) null);
        return this._syncRoot;
      }
    }

    bool IBindingList.AllowEdit
    {
      get
      {
        return true;
      }
    }

    bool IBindingList.AllowNew
    {
      get
      {
        return true;
      }
    }

    bool IBindingList.AllowRemove
    {
      get
      {
        return true;
      }
    }

    bool IBindingList.IsSorted
    {
      get
      {
        return false;
      }
    }

    ListSortDirection IBindingList.SortDirection
    {
      get
      {
        return ListSortDirection.Ascending;
      }
    }

    PropertyDescriptor IBindingList.SortProperty
    {
      get
      {
        return (PropertyDescriptor) null;
      }
    }

    bool IBindingList.SupportsChangeNotification
    {
      get
      {
        return true;
      }
    }

    bool IBindingList.SupportsSearching
    {
      get
      {
        return false;
      }
    }

    bool IBindingList.SupportsSorting
    {
      get
      {
        return false;
      }
    }

    public event ListChangedEventHandler ListChanged
    {
      add
      {
        this._listChanged += value;
      }
      remove
      {
        this._listChanged -= value;
      }
    }

    public event AddingNewEventHandler AddingNew
    {
      add
      {
        this._addingNew += value;
      }
      remove
      {
        this._addingNew -= value;
      }
    }

    internal JContainer()
    {
    }

    internal JContainer(JContainer other)
      : this()
    {
      ValidationUtils.ArgumentNotNull((object) other, "c");
      foreach (object content in (IEnumerable<JToken>) other)
        this.Add(content);
    }

    internal void CheckReentrancy()
    {
      if (this._busy)
        throw new InvalidOperationException(StringUtils.FormatWith("Cannot change {0} during a collection change event.", (IFormatProvider) CultureInfo.InvariantCulture, (object) this.GetType()));
    }

    internal virtual IList<JToken> CreateChildrenCollection()
    {
      return (IList<JToken>) new List<JToken>();
    }

    protected virtual void OnAddingNew(AddingNewEventArgs e)
    {
      AddingNewEventHandler addingNewEventHandler = this._addingNew;
      if (addingNewEventHandler == null)
        return;
      addingNewEventHandler((object) this, e);
    }

    protected virtual void OnListChanged(ListChangedEventArgs e)
    {
      ListChangedEventHandler changedEventHandler = this._listChanged;
      if (changedEventHandler == null)
        return;
      this._busy = true;
      try
      {
        changedEventHandler((object) this, e);
      }
      finally
      {
        this._busy = false;
      }
    }

    internal bool ContentsEqual(JContainer container)
    {
      if (container == this)
        return true;
      IList<JToken> childrenTokens1 = this.ChildrenTokens;
      IList<JToken> childrenTokens2 = container.ChildrenTokens;
      if (childrenTokens1.Count != childrenTokens2.Count)
        return false;
      for (int index = 0; index < childrenTokens1.Count; ++index)
      {
        if (!childrenTokens1[index].DeepEquals(childrenTokens2[index]))
          return false;
      }
      return true;
    }

    public override JEnumerable<JToken> Children()
    {
      return new JEnumerable<JToken>((IEnumerable<JToken>) this.ChildrenTokens);
    }

    public override IEnumerable<T> Values<T>()
    {
      return Extensions.Convert<JToken, T>((IEnumerable<JToken>) this.ChildrenTokens);
    }

    public IEnumerable<JToken> Descendants()
    {
      foreach (JToken jtoken1 in (IEnumerable<JToken>) this.ChildrenTokens)
      {
        yield return jtoken1;
        JContainer c = jtoken1 as JContainer;
        if (c != null)
        {
          foreach (JToken jtoken2 in c.Descendants())
            yield return jtoken2;
        }
      }
    }

    internal bool IsMultiContent(object content)
    {
      if (content is IEnumerable && !(content is string) && !(content is JToken))
        return !(content is byte[]);
      else
        return false;
    }

    internal JToken EnsureParentToken(JToken item, bool skipParentCheck)
    {
      if (item == null)
        return (JToken) new JValue((object) null);
      if (skipParentCheck || item.Parent == null && item != this && (!item.HasValues || this.Root != item))
        return item;
      item = item.CloneToken();
      return item;
    }

    internal int IndexOfItem(JToken item)
    {
      return CollectionUtils.IndexOf<JToken>((IEnumerable<JToken>) this.ChildrenTokens, item, (IEqualityComparer<JToken>) JContainer.JTokenReferenceEqualityComparer.Instance);
    }

    internal virtual void InsertItem(int index, JToken item, bool skipParentCheck)
    {
      if (index > this.ChildrenTokens.Count)
        throw new ArgumentOutOfRangeException("index", "Index must be within the bounds of the List.");
      this.CheckReentrancy();
      item = this.EnsureParentToken(item, skipParentCheck);
      JToken jtoken1 = index == 0 ? (JToken) null : this.ChildrenTokens[index - 1];
      JToken jtoken2 = index == this.ChildrenTokens.Count ? (JToken) null : this.ChildrenTokens[index];
      this.ValidateToken(item, (JToken) null);
      item.Parent = this;
      item.Previous = jtoken1;
      if (jtoken1 != null)
        jtoken1.Next = item;
      item.Next = jtoken2;
      if (jtoken2 != null)
        jtoken2.Previous = item;
      this.ChildrenTokens.Insert(index, item);
      if (this._listChanged == null)
        return;
      this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
    }

    internal virtual void RemoveItemAt(int index)
    {
      if (index < 0)
        throw new ArgumentOutOfRangeException("index", "Index is less than 0.");
      if (index >= this.ChildrenTokens.Count)
        throw new ArgumentOutOfRangeException("index", "Index is equal to or greater than Count.");
      this.CheckReentrancy();
      JToken jtoken1 = this.ChildrenTokens[index];
      JToken jtoken2 = index == 0 ? (JToken) null : this.ChildrenTokens[index - 1];
      JToken jtoken3 = index == this.ChildrenTokens.Count - 1 ? (JToken) null : this.ChildrenTokens[index + 1];
      if (jtoken2 != null)
        jtoken2.Next = jtoken3;
      if (jtoken3 != null)
        jtoken3.Previous = jtoken2;
      jtoken1.Parent = (JContainer) null;
      jtoken1.Previous = (JToken) null;
      jtoken1.Next = (JToken) null;
      this.ChildrenTokens.RemoveAt(index);
      if (this._listChanged == null)
        return;
      this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
    }

    internal virtual bool RemoveItem(JToken item)
    {
      int index = this.IndexOfItem(item);
      if (index < 0)
        return false;
      this.RemoveItemAt(index);
      return true;
    }

    internal virtual JToken GetItem(int index)
    {
      return this.ChildrenTokens[index];
    }

    internal virtual void SetItem(int index, JToken item)
    {
      if (index < 0)
        throw new ArgumentOutOfRangeException("index", "Index is less than 0.");
      if (index >= this.ChildrenTokens.Count)
        throw new ArgumentOutOfRangeException("index", "Index is equal to or greater than Count.");
      JToken jtoken1 = this.ChildrenTokens[index];
      if (JContainer.IsTokenUnchanged(jtoken1, item))
        return;
      this.CheckReentrancy();
      item = this.EnsureParentToken(item, false);
      this.ValidateToken(item, jtoken1);
      JToken jtoken2 = index == 0 ? (JToken) null : this.ChildrenTokens[index - 1];
      JToken jtoken3 = index == this.ChildrenTokens.Count - 1 ? (JToken) null : this.ChildrenTokens[index + 1];
      item.Parent = this;
      item.Previous = jtoken2;
      if (jtoken2 != null)
        jtoken2.Next = item;
      item.Next = jtoken3;
      if (jtoken3 != null)
        jtoken3.Previous = item;
      this.ChildrenTokens[index] = item;
      jtoken1.Parent = (JContainer) null;
      jtoken1.Previous = (JToken) null;
      jtoken1.Next = (JToken) null;
      if (this._listChanged == null)
        return;
      this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, index));
    }

    internal virtual void ClearItems()
    {
      this.CheckReentrancy();
      foreach (JToken jtoken in (IEnumerable<JToken>) this.ChildrenTokens)
      {
        jtoken.Parent = (JContainer) null;
        jtoken.Previous = (JToken) null;
        jtoken.Next = (JToken) null;
      }
      this.ChildrenTokens.Clear();
      if (this._listChanged == null)
        return;
      this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
    }

    internal virtual void ReplaceItem(JToken existing, JToken replacement)
    {
      if (existing == null || existing.Parent != this)
        return;
      this.SetItem(this.IndexOfItem(existing), replacement);
    }

    internal virtual bool ContainsItem(JToken item)
    {
      return this.IndexOfItem(item) != -1;
    }

    internal virtual void CopyItemsTo(Array array, int arrayIndex)
    {
      if (array == null)
        throw new ArgumentNullException("array");
      if (arrayIndex < 0)
        throw new ArgumentOutOfRangeException("arrayIndex", "arrayIndex is less than 0.");
      if (arrayIndex >= array.Length && arrayIndex != 0)
        throw new ArgumentException("arrayIndex is equal to or greater than the length of array.");
      if (this.Count > array.Length - arrayIndex)
        throw new ArgumentException("The number of elements in the source JObject is greater than the available space from arrayIndex to the end of the destination array.");
      int num = 0;
      foreach (JToken jtoken in (IEnumerable<JToken>) this.ChildrenTokens)
      {
        array.SetValue((object) jtoken, arrayIndex + num);
        ++num;
      }
    }

    internal static bool IsTokenUnchanged(JToken currentValue, JToken newValue)
    {
      JValue jvalue = currentValue as JValue;
      if (jvalue == null)
        return false;
      if (jvalue.Type == JTokenType.Null && newValue == null)
        return true;
      else
        return ((object) jvalue).Equals((object) newValue);
    }

    internal virtual void ValidateToken(JToken o, JToken existing)
    {
      ValidationUtils.ArgumentNotNull((object) o, "o");
      if (o.Type == JTokenType.Property)
        throw new ArgumentException(StringUtils.FormatWith("Can not add {0} to {1}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) o.GetType(), (object) this.GetType()));
    }

    public virtual void Add(object content)
    {
      this.AddInternal(this.ChildrenTokens.Count, content, false);
    }

    internal void AddAndSkipParentCheck(JToken token)
    {
      this.AddInternal(this.ChildrenTokens.Count, (object) token, true);
    }

    public void AddFirst(object content)
    {
      this.AddInternal(0, content, false);
    }

    internal void AddInternal(int index, object content, bool skipParentCheck)
    {
      if (this.IsMultiContent(content))
      {
        IEnumerable enumerable = (IEnumerable) content;
        int index1 = index;
        foreach (object content1 in enumerable)
        {
          this.AddInternal(index1, content1, skipParentCheck);
          ++index1;
        }
      }
      else
      {
        JToken fromContent = this.CreateFromContent(content);
        this.InsertItem(index, fromContent, skipParentCheck);
      }
    }

    internal JToken CreateFromContent(object content)
    {
      if (content is JToken)
        return (JToken) content;
      else
        return (JToken) new JValue(content);
    }

    public JsonWriter CreateWriter()
    {
      return (JsonWriter) new JTokenWriter(this);
    }

    public void ReplaceAll(object content)
    {
      this.ClearItems();
      this.Add(content);
    }

    public void RemoveAll()
    {
      this.ClearItems();
    }

    internal void ReadTokenFrom(JsonReader reader)
    {
      int depth = reader.Depth;
      if (!reader.Read())
        throw JsonReaderException.Create(reader, StringUtils.FormatWith("Error reading {0} from JsonReader.", (IFormatProvider) CultureInfo.InvariantCulture, (object) this.GetType().Name));
      this.ReadContentFrom(reader);
      if (reader.Depth > depth)
        throw JsonReaderException.Create(reader, StringUtils.FormatWith("Unexpected end of content while loading {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) this.GetType().Name));
    }

    internal void ReadContentFrom(JsonReader r)
    {
      ValidationUtils.ArgumentNotNull((object) r, "r");
      IJsonLineInfo lineInfo = r as IJsonLineInfo;
      JContainer jcontainer = this;
      do
      {
        if (jcontainer is JProperty && ((JProperty) jcontainer).Value != null)
        {
          if (jcontainer == this)
            break;
          jcontainer = jcontainer.Parent;
        }
        switch (r.TokenType)
        {
          case JsonToken.None:
            continue;
          case JsonToken.StartObject:
            JObject jobject = new JObject();
            jobject.SetLineInfo(lineInfo);
            jcontainer.Add((object) jobject);
            jcontainer = (JContainer) jobject;
            goto case 0;
          case JsonToken.StartArray:
            JArray jarray = new JArray();
            jarray.SetLineInfo(lineInfo);
            jcontainer.Add((object) jarray);
            jcontainer = (JContainer) jarray;
            goto case 0;
          case JsonToken.StartConstructor:
            JConstructor jconstructor = new JConstructor(r.Value.ToString());
            jconstructor.SetLineInfo((IJsonLineInfo) jconstructor);
            jcontainer.Add((object) jconstructor);
            jcontainer = (JContainer) jconstructor;
            goto case 0;
          case JsonToken.PropertyName:
            string name = r.Value.ToString();
            JProperty jproperty1 = new JProperty(name);
            jproperty1.SetLineInfo(lineInfo);
            JProperty jproperty2 = ((JObject) jcontainer).Property(name);
            if (jproperty2 == null)
              jcontainer.Add((object) jproperty1);
            else
              jproperty2.Replace((JToken) jproperty1);
            jcontainer = (JContainer) jproperty1;
            goto case 0;
          case JsonToken.Comment:
            JValue comment = JValue.CreateComment(r.Value.ToString());
            comment.SetLineInfo(lineInfo);
            jcontainer.Add((object) comment);
            goto case 0;
          case JsonToken.Integer:
          case JsonToken.Float:
          case JsonToken.String:
          case JsonToken.Boolean:
          case JsonToken.Date:
          case JsonToken.Bytes:
            JValue jvalue1 = new JValue(r.Value);
            jvalue1.SetLineInfo(lineInfo);
            jcontainer.Add((object) jvalue1);
            goto case 0;
          case JsonToken.Null:
            JValue jvalue2 = new JValue((object) null, JTokenType.Null);
            jvalue2.SetLineInfo(lineInfo);
            jcontainer.Add((object) jvalue2);
            goto case 0;
          case JsonToken.Undefined:
            JValue jvalue3 = new JValue((object) null, JTokenType.Undefined);
            jvalue3.SetLineInfo(lineInfo);
            jcontainer.Add((object) jvalue3);
            goto case 0;
          case JsonToken.EndObject:
            if (jcontainer == this)
              return;
            jcontainer = jcontainer.Parent;
            goto case 0;
          case JsonToken.EndArray:
            if (jcontainer == this)
              return;
            jcontainer = jcontainer.Parent;
            goto case 0;
          case JsonToken.EndConstructor:
            if (jcontainer == this)
              return;
            jcontainer = jcontainer.Parent;
            goto case 0;
          default:
            throw new InvalidOperationException(StringUtils.FormatWith("The JsonReader should not be on a token of type {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) r.TokenType));
        }
      }
      while (r.Read());
    }

    internal int ContentsHashCode()
    {
      int num = 0;
      foreach (JToken jtoken in (IEnumerable<JToken>) this.ChildrenTokens)
        num ^= jtoken.GetDeepHashCode();
      return num;
    }

    string ITypedList.GetListName(PropertyDescriptor[] listAccessors)
    {
      return string.Empty;
    }

    PropertyDescriptorCollection ITypedList.GetItemProperties(PropertyDescriptor[] listAccessors)
    {
      ICustomTypeDescriptor customTypeDescriptor = this.First as ICustomTypeDescriptor;
      if (customTypeDescriptor != null)
        return customTypeDescriptor.GetProperties();
      else
        return (PropertyDescriptorCollection) null;
    }

    int IList<JToken>.IndexOf(JToken item)
    {
      return this.IndexOfItem(item);
    }

    void IList<JToken>.Insert(int index, JToken item)
    {
      this.InsertItem(index, item, false);
    }

    void IList<JToken>.RemoveAt(int index)
    {
      this.RemoveItemAt(index);
    }

    JToken IList<JToken>.get_Item(int index)
    {
      return this.GetItem(index);
    }

    void IList<JToken>.set_Item(int index, JToken value)
    {
      this.SetItem(index, value);
    }

    void ICollection<JToken>.Add(JToken item)
    {
      this.Add((object) item);
    }

    void ICollection<JToken>.Clear()
    {
      this.ClearItems();
    }

    bool ICollection<JToken>.Contains(JToken item)
    {
      return this.ContainsItem(item);
    }

    void ICollection<JToken>.CopyTo(JToken[] array, int arrayIndex)
    {
      this.CopyItemsTo((Array) array, arrayIndex);
    }

    bool ICollection<JToken>.Remove(JToken item)
    {
      return this.RemoveItem(item);
    }

    private JToken EnsureValue(object value)
    {
      if (value == null)
        return (JToken) null;
      if (value is JToken)
        return (JToken) value;
      else
        throw new ArgumentException("Argument is not a JToken.");
    }

    int IList.Add(object value)
    {
      this.Add((object) this.EnsureValue(value));
      return this.Count - 1;
    }

    void IList.Clear()
    {
      this.ClearItems();
    }

    bool IList.Contains(object value)
    {
      return this.ContainsItem(this.EnsureValue(value));
    }

    int IList.IndexOf(object value)
    {
      return this.IndexOfItem(this.EnsureValue(value));
    }

    void IList.Insert(int index, object value)
    {
      this.InsertItem(index, this.EnsureValue(value), false);
    }

    void IList.Remove(object value)
    {
      this.RemoveItem(this.EnsureValue(value));
    }

    void IList.RemoveAt(int index)
    {
      this.RemoveItemAt(index);
    }

    object IList.get_Item(int index)
    {
      return (object) this.GetItem(index);
    }

    void IList.set_Item(int index, object value)
    {
      this.SetItem(index, this.EnsureValue(value));
    }

    void ICollection.CopyTo(Array array, int index)
    {
      this.CopyItemsTo(array, index);
    }

    void IBindingList.AddIndex(PropertyDescriptor property)
    {
    }

    object IBindingList.AddNew()
    {
      AddingNewEventArgs e = new AddingNewEventArgs();
      this.OnAddingNew(e);
      if (e.NewObject == null)
        throw new JsonException(StringUtils.FormatWith("Could not determine new value to add to '{0}'.", (IFormatProvider) CultureInfo.InvariantCulture, (object) this.GetType()));
      if (!(e.NewObject is JToken))
        throw new JsonException(StringUtils.FormatWith("New item to be added to collection must be compatible with {0}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) typeof (JToken)));
      JToken jtoken = (JToken) e.NewObject;
      this.Add((object) jtoken);
      return (object) jtoken;
    }

    void IBindingList.ApplySort(PropertyDescriptor property, ListSortDirection direction)
    {
      throw new NotSupportedException();
    }

    int IBindingList.Find(PropertyDescriptor property, object key)
    {
      throw new NotSupportedException();
    }

    void IBindingList.RemoveIndex(PropertyDescriptor property)
    {
    }

    void IBindingList.RemoveSort()
    {
      throw new NotSupportedException();
    }

    private class JTokenReferenceEqualityComparer : IEqualityComparer<JToken>
    {
      public static readonly JContainer.JTokenReferenceEqualityComparer Instance = new JContainer.JTokenReferenceEqualityComparer();

      static JTokenReferenceEqualityComparer()
      {
      }

      public bool Equals(JToken x, JToken y)
      {
        return object.ReferenceEquals((object) x, (object) y);
      }

      public int GetHashCode(JToken obj)
      {
        if (obj == null)
          return 0;
        else
          return obj.GetHashCode();
      }
    }
  }
}
