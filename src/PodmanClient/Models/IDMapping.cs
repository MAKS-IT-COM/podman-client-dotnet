using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaksIT.PodmanClientDotNet.Models {
  public class IDMapping {
    public int ContainerId { get; set; }
    public int HostId { get; set; }
    public int Size { get; set; }
  }

}
