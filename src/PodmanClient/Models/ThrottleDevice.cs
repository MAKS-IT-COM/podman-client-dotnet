using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaksIT.PodmanClientDotNet.Models {
  public class ThrottleDevice {
    public int Major { get; set; }
    public int Minor { get; set; }
    public long Rate { get; set; }
  }

}
