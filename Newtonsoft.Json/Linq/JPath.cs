// Type: Newtonsoft.Json.Linq.JPath
// Assembly: Newtonsoft.Json, Version=4.5.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: DB013E7B-84E7-4AE6-A132-56DC73318C48
// Assembly location: F:\Program Files (x86)\FEZ\Newtonsoft.Json.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Newtonsoft.Json.Linq
{
  internal class JPath
  {
    private readonly string _expression;
    private int _currentIndex;

    public List<object> Parts { get; private set; }

    public JPath(string expression)
    {
      ValidationUtils.ArgumentNotNull((object) expression, "expression");
      this._expression = expression;
      this.Parts = new List<object>();
      this.ParseMain();
    }

    private void ParseMain()
    {
      int startIndex = this._currentIndex;
      bool flag = false;
      for (; this._currentIndex < this._expression.Length; ++this._currentIndex)
      {
        char indexerOpenChar = this._expression[this._currentIndex];
        switch (indexerOpenChar)
        {
          case '(':
          case '[':
            if (this._currentIndex > startIndex)
              this.Parts.Add((object) this._expression.Substring(startIndex, this._currentIndex - startIndex));
            this.ParseIndexer(indexerOpenChar);
            startIndex = this._currentIndex + 1;
            flag = true;
            break;
          case ')':
          case ']':
            throw new JsonException("Unexpected character while parsing path: " + (object) indexerOpenChar);
          case '.':
            if (this._currentIndex > startIndex)
              this.Parts.Add((object) this._expression.Substring(startIndex, this._currentIndex - startIndex));
            startIndex = this._currentIndex + 1;
            flag = false;
            break;
          default:
            if (flag)
              throw new JsonException("Unexpected character following indexer: " + (object) indexerOpenChar);
            else
              break;
        }
      }
      if (this._currentIndex <= startIndex)
        return;
      this.Parts.Add((object) this._expression.Substring(startIndex, this._currentIndex - startIndex));
    }

    private void ParseIndexer(char indexerOpenChar)
    {
      ++this._currentIndex;
      char ch = (int) indexerOpenChar == 91 ? ']' : ')';
      int startIndex = this._currentIndex;
      int length = 0;
      bool flag = false;
      for (; this._currentIndex < this._expression.Length; ++this._currentIndex)
      {
        char c = this._expression[this._currentIndex];
        if (char.IsDigit(c))
        {
          ++length;
        }
        else
        {
          if ((int) c != (int) ch)
            throw new JsonException("Unexpected character while parsing path indexer: " + (object) c);
          flag = true;
          break;
        }
      }
      if (!flag)
        throw new JsonException("Path ended with open indexer. Expected " + (object) ch);
      if (length == 0)
        throw new JsonException("Empty path indexer.");
      this.Parts.Add((object) Convert.ToInt32(this._expression.Substring(startIndex, length), (IFormatProvider) CultureInfo.InvariantCulture));
    }

    internal JToken Evaluate(JToken root, bool errorWhenNoMatch)
    {
      JToken jtoken = root;
      foreach (object obj in this.Parts)
      {
        string index1 = obj as string;
        if (index1 != null)
        {
          JObject jobject = jtoken as JObject;
          if (jobject != null)
          {
            jtoken = jobject[index1];
            if (jtoken == null && errorWhenNoMatch)
              throw new JsonException(StringUtils.FormatWith("Property '{0}' does not exist on JObject.", (IFormatProvider) CultureInfo.InvariantCulture, (object) index1));
          }
          else if (errorWhenNoMatch)
            throw new JsonException(StringUtils.FormatWith("Property '{0}' not valid on {1}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) index1, (object) jtoken.GetType().Name));
          else
            return (JToken) null;
        }
        else
        {
          int index2 = (int) obj;
          JArray jarray = jtoken as JArray;
          JConstructor jconstructor = jtoken as JConstructor;
          if (jarray != null)
          {
            if (jarray.Count <= index2)
            {
              if (errorWhenNoMatch)
                throw new IndexOutOfRangeException(StringUtils.FormatWith("Index {0} outside the bounds of JArray.", (IFormatProvider) CultureInfo.InvariantCulture, (object) index2));
              else
                return (JToken) null;
            }
            else
              jtoken = jarray[index2];
          }
          else if (jconstructor != null)
          {
            if (jconstructor.Count <= index2)
            {
              if (errorWhenNoMatch)
                throw new IndexOutOfRangeException(StringUtils.FormatWith("Index {0} outside the bounds of JConstructor.", (IFormatProvider) CultureInfo.InvariantCulture, (object) index2));
              else
                return (JToken) null;
            }
            else
              jtoken = jconstructor[(object) index2];
          }
          else if (errorWhenNoMatch)
            throw new JsonException(StringUtils.FormatWith("Index {0} not valid on {1}.", (IFormatProvider) CultureInfo.InvariantCulture, (object) index2, (object) jtoken.GetType().Name));
          else
            return (JToken) null;
        }
      }
      return jtoken;
    }
  }
}
