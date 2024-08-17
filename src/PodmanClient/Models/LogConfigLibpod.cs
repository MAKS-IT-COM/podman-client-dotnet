using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaksIT.PodmanClientDotNet.Models {
  public class LogConfigLibpod {
    public string Driver { get; set; }
    public Dictionary<string, string> Options { get; set; }
    public string Path { get; set; }
    public long Size { get; set; }
  }

}
