using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaksIT.PodmanClientDotNet.Models {
  public class LinuxDeviceCgroup {
    public string Access { get; set; }
    public bool Allow { get; set; }
    public int Major { get; set; }
    public int Minor { get; set; }
    public string Type { get; set; }
  }

}
