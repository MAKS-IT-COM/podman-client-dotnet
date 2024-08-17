using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaksIT.PodmanClientDotNet.Models {
  public class LinuxDevice {
    public int FileMode { get; set; }
    public int Gid { get; set; }
    public int Major { get; set; }
    public int Minor { get; set; }
    public string Path { get; set; }
    public string Type { get; set; }
    public int Uid { get; set; }
  }

}
