using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaksIT.PodmanClientDotNet.Models {
  public class LinuxIntelRdt {
    public string ClosID { get; set; }
    public bool EnableCMT { get; set; }
    public bool EnableMBM { get; set; }
    public string L3CacheSchema { get; set; }
    public string MemBwSchema { get; set; }
  }

}
