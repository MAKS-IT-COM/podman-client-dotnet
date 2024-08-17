using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaksIT.PodmanClientDotNet.Models {
  public class POSIXRlimit {
    public long Hard { get; set; }
    public long Soft { get; set; }
    public string Type { get; set; }
  }

}
