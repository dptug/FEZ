// Type: SDL.Tag
// Assembly: SimpleDefinitionLanguage, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8A9B110C-63DC-4C6F-B639-88CD09E9B5B5
// Assembly location: F:\Program Files (x86)\FEZ\SimpleDefinitionLanguage.dll

using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SDL
{
  public class Tag
  {
    private string sdlNamespace;
    private string name;
    private List<object> values;
    private Dictionary<string, string> attributeToNamespace;
    private Dictionary<string, object> attributes;
    private List<Tag> children;
    private List<object> valuesSnapshot;
    private bool valuesDirty;
    private Dictionary<string, object> attributesSnapshot;
    private bool attributesDirty;
    private Dictionary<string, string> attributeToNamespaceSnapshot;
    private List<Tag> childrenSnapshot;
    private bool childrenDirty;

    public string Name
    {
      get
      {
        return this.name;
      }
      set
      {
        this.name = SDLUtil.ValidateIdentifier(value);
      }
    }

    public string SDLNamespace
    {
      get
      {
        return this.sdlNamespace;
      }
      set
      {
        if (value == null)
          value = "";
        if (value.Length != 0)
          SDLUtil.ValidateIdentifier(value);
        this.sdlNamespace = value;
      }
    }

    public object Value
    {
      get
      {
        if (this.values.Count == 0)
          return (object) null;
        else
          return this.values[0];
      }
      set
      {
        value = SDLUtil.CoerceOrFail(value);
        if (this.values.Count == 0)
          this.AddValue(value);
        else
          this.values[0] = value;
        this.valuesDirty = true;
      }
    }

    public IList<Tag> Children
    {
      get
      {
        if (this.childrenDirty)
          this.childrenSnapshot = new List<Tag>((IEnumerable<Tag>) this.children);
        return (IList<Tag>) this.childrenSnapshot;
      }
      set
      {
        this.childrenDirty = true;
        this.children = new List<Tag>((IEnumerable<Tag>) value);
      }
    }

    public IList<object> Values
    {
      get
      {
        if (this.valuesDirty)
          this.valuesSnapshot = new List<object>((IEnumerable<object>) this.values);
        return (IList<object>) this.valuesSnapshot;
      }
      set
      {
        this.valuesDirty = true;
        this.values.Clear();
        foreach (object obj in (IEnumerable<object>) value)
          this.AddValue(obj);
      }
    }

    public IDictionary<string, object> Attributes
    {
      get
      {
        if (this.attributesDirty)
        {
          this.EnsureAttributesInitialized();
          this.attributesSnapshot = new Dictionary<string, object>((IDictionary<string, object>) this.attributes);
        }
        return (IDictionary<string, object>) this.attributesSnapshot;
      }
      set
      {
        this.attributesDirty = true;
        this.EnsureAttributesInitialized();
        this.attributes.Clear();
        foreach (string index in (IEnumerable<string>) value.Keys)
          this[index] = value[index];
      }
    }

    public IDictionary<string, string> AttributeToNamespace
    {
      get
      {
        if (this.attributesDirty)
        {
          this.EnsureAttributesInitialized();
          this.attributeToNamespaceSnapshot = new Dictionary<string, string>((IDictionary<string, string>) this.attributeToNamespace);
        }
        return (IDictionary<string, string>) this.attributeToNamespaceSnapshot;
      }
    }

    public object this[string key]
    {
      get
      {
        if (this.attributes == null)
          return (object) null;
        object obj;
        this.attributes.TryGetValue(key, out obj);
        return obj;
      }
      set
      {
        this["", key] = value;
      }
    }

    public object this[string sdlNamespace, string key]
    {
      set
      {
        this.attributesDirty = true;
        this.EnsureAttributesInitialized();
        this.attributes[SDLUtil.ValidateIdentifier(key)] = SDLUtil.CoerceOrFail(value);
        if (sdlNamespace == null)
          sdlNamespace = "";
        if (sdlNamespace.Length != 0)
          SDLUtil.ValidateIdentifier(sdlNamespace);
        this.attributeToNamespace[key] = sdlNamespace;
      }
    }

    public object this[int index]
    {
      get
      {
        return this.values[index];
      }
      set
      {
        this.valuesDirty = true;
        this.values[index] = SDLUtil.CoerceOrFail(value);
      }
    }

    public Tag(string name)
      : this("", name)
    {
    }

    public Tag(string sdlNamespace, string name)
    {
      this.SDLNamespace = sdlNamespace;
      this.Name = name;
      this.values = new List<object>();
      this.children = new List<Tag>();
      this.attributesDirty = this.childrenDirty = this.valuesDirty = true;
    }

    private void EnsureAttributesInitialized()
    {
      if (this.attributes != null)
        return;
      this.attributes = new Dictionary<string, object>();
      this.attributeToNamespace = new Dictionary<string, string>();
    }

    public void AddChild(Tag child)
    {
      this.childrenDirty = true;
      this.children.Add(child);
    }

    public bool RemoveChild(Tag child)
    {
      this.childrenDirty = true;
      return this.children.Remove(child);
    }

    public void AddValue(object value)
    {
      this.valuesDirty = true;
      this.values.Add(SDLUtil.CoerceOrFail(value));
    }

    public bool RemoveValue(object value)
    {
      this.valuesDirty = true;
      return this.values.Remove(value);
    }

    public IList<Tag> GetChildren(bool recursively)
    {
      if (!recursively)
        return this.Children;
      List<Tag> list = new List<Tag>();
      foreach (Tag tag in (IEnumerable<Tag>) this.Children)
      {
        list.Add(tag);
        if (recursively)
          list.AddRange((IEnumerable<Tag>) tag.GetChildren(true));
      }
      return (IList<Tag>) list;
    }

    public Tag GetChild(string childName)
    {
      return this.GetChild(childName, false);
    }

    public Tag GetChild(string childName, bool recursive)
    {
      foreach (Tag tag in this.children)
      {
        if (tag.Name.Equals(childName))
          return tag;
        if (recursive)
        {
          Tag child = tag.GetChild(childName, true);
          if (child != null)
            return child;
        }
      }
      return (Tag) null;
    }

    public IList<Tag> GetChildren(string childName)
    {
      return this.GetChildren(childName, false);
    }

    public IList<Tag> GetChildren(string childName, bool recursive)
    {
      List<Tag> list = new List<Tag>();
      foreach (Tag tag in this.children)
      {
        if (tag.Name.Equals(childName))
          list.Add(tag);
        if (recursive)
          list.AddRange((IEnumerable<Tag>) tag.GetChildren(childName, true));
      }
      return (IList<Tag>) list;
    }

    public IList<object> GetChildrenValues(string name)
    {
      List<object> list = new List<object>();
      foreach (Tag tag in (IEnumerable<Tag>) this.GetChildren(name))
      {
        IList<object> values = tag.Values;
        if (values.Count == 0)
          list.Add((object) null);
        else if (values.Count == 1)
          list.Add(values[0]);
        else
          list.Add((object) values);
      }
      return (IList<object>) list;
    }

    public IList<Tag> GetChildrenForNamespace(string sdlNamespace)
    {
      return this.GetChildrenForNamespace(sdlNamespace, false);
    }

    public IList<Tag> GetChildrenForNamespace(string sdlNamespace, bool recursive)
    {
      List<Tag> list = new List<Tag>();
      foreach (Tag tag in this.children)
      {
        if (tag.SDLNamespace.Equals(sdlNamespace))
          list.Add(tag);
        if (recursive)
          list.AddRange((IEnumerable<Tag>) tag.GetChildrenForNamespace(sdlNamespace, true));
      }
      return (IList<Tag>) list;
    }

    public IDictionary<string, object> GetAttributesForNamespace(string sdlNamespace)
    {
      Dictionary<string, object> dictionary = new Dictionary<string, object>();
      this.EnsureAttributesInitialized();
      foreach (string index in this.attributeToNamespace.Keys)
      {
        if (this.attributeToNamespace[index].Equals(sdlNamespace))
          dictionary[index] = this.attributes[index];
      }
      return (IDictionary<string, object>) dictionary;
    }

    public Tag ReadFile(string file)
    {
      return this.Read(new StreamReader(file, Encoding.UTF8));
    }

    public Tag ReadString(string text)
    {
      return this.Read(new StreamReader(text));
    }

    public Tag Read(StreamReader reader)
    {
      foreach (Tag child in (IEnumerable<Tag>) new Parser((TextReader) reader).Parse())
        this.AddChild(child);
      return this;
    }

    public void WriteFile(string file)
    {
      this.WriteFile(file, false);
    }

    public void WriteFile(string file, bool includeRoot)
    {
      using (StreamWriter streamWriter = new StreamWriter(file, false, Encoding.UTF8))
        this.Write((TextWriter) streamWriter, includeRoot);
    }

    public void Write(TextWriter writer, bool includeRoot)
    {
      string str = "\r\n";
      if (includeRoot)
      {
        writer.Write(base.ToString());
      }
      else
      {
        for (int index = 0; index < this.children.Count; ++index)
        {
          writer.Write(((object) this.children[index]).ToString());
          if (index < this.children.Count - 1)
            writer.Write(str);
        }
      }
      writer.Close();
    }

    public override string ToString()
    {
      return this.ToString("");
    }

    private string ToString(string linePrefix)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(linePrefix);
      bool flag = false;
      if (this.sdlNamespace.Length == 0 && this.name.Equals("content"))
      {
        flag = true;
      }
      else
      {
        if (this.sdlNamespace.Length != 0)
          stringBuilder.Append(this.sdlNamespace).Append(':');
        stringBuilder.Append(this.name);
      }
      if (this.values.Count != 0)
      {
        if (flag)
          flag = false;
        else
          stringBuilder.Append(" ");
        int count = this.values.Count;
        for (int index = 0; index < count; ++index)
        {
          stringBuilder.Append(SDLUtil.Format(this.values[index]));
          if (index < count - 1)
            stringBuilder.Append(" ");
        }
      }
      if (this.attributes != null && this.attributes.Count != 0)
      {
        foreach (string index in this.attributes.Keys)
        {
          stringBuilder.Append(" ");
          string str = this.AttributeToNamespace[index];
          if (!str.Equals(""))
            stringBuilder.Append(str + ":");
          stringBuilder.Append(index + "=");
          stringBuilder.Append(SDLUtil.Format(this.attributes[index]));
        }
      }
      if (this.children.Count != 0)
      {
        if (!flag)
          stringBuilder.Append(" ");
        stringBuilder.Append("{\r\n");
        foreach (Tag tag in this.children)
          stringBuilder.Append(tag.ToString(linePrefix + "\t") + "\r\n");
        stringBuilder.Append(linePrefix + "}");
      }
      return ((object) stringBuilder).ToString();
    }

    public string ToXMLString()
    {
      return this.ToXMLString("");
    }

    private string ToXMLString(string linePrefix)
    {
      string str1 = "\r\n";
      if (linePrefix == null)
        linePrefix = "";
      StringBuilder stringBuilder = new StringBuilder(linePrefix + "<");
      if (!this.sdlNamespace.Equals(""))
        stringBuilder.Append(this.sdlNamespace + ":");
      stringBuilder.Append(this.name);
      if (this.values.Count != 0)
      {
        int num = 0;
        foreach (object obj in this.values)
        {
          stringBuilder.Append(" ");
          stringBuilder.Append("_val" + (object) num + "=\"" + SDLUtil.Format(obj, false) + "\"");
          ++num;
        }
      }
      if (this.attributes != null && this.attributes.Count != 0)
      {
        foreach (string index in this.attributes.Keys)
        {
          stringBuilder.Append(" ");
          string str2 = this.attributeToNamespace[index];
          if (!str2.Equals(""))
            stringBuilder.Append(str2 + ":");
          stringBuilder.Append(index + "=");
          stringBuilder.Append("\"" + SDLUtil.Format(this.attributes[index], false) + "\"");
        }
      }
      if (this.children.Count != 0)
      {
        stringBuilder.Append(">" + str1);
        foreach (Tag tag in this.children)
          stringBuilder.Append(tag.ToXMLString(linePrefix + "    ") + str1);
        stringBuilder.Append(linePrefix + "</");
        if (!this.sdlNamespace.Equals(""))
          stringBuilder.Append(this.sdlNamespace + ":");
        stringBuilder.Append(this.name + ">");
      }
      else
        stringBuilder.Append("/>");
      return ((object) stringBuilder).ToString();
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      else
        return base.ToString().Equals(obj.ToString());
    }

    public override int GetHashCode()
    {
      return base.ToString().GetHashCode();
    }
  }
}
