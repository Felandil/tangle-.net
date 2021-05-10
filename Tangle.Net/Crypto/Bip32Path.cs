using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tangle.Net.Crypto
{
  public class Bip32Path
  {
    private List<string> Path { get; set; }
    public Bip32Path(string path = "")
    {
      if (!string.IsNullOrEmpty(path))
      {
        var initialPath = path.Split('/').ToList();
        if (initialPath[0] == "m")
        {
          initialPath.RemoveAt(0);
        }

        this.Path = initialPath;
      }
      else
      {
        this.Path = new List<string>();
      }
    }

    public override string ToString()
    {
      return this.Path.Count > 0 ? $"m/{string.Join("/", this.Path)}" : "m";
    }

    public void Pop()
    {
      this.Path.RemoveAt(this.Path.Count - 1);
    }

    public void PushHardened(int index)
    {
      this.Path.Add($"{index}'");
    }

    public void Push(int index)
    {
      this.Path.Add($"{index}");
    }
  }
}
