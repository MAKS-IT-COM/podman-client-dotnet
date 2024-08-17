using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaksIT.PodmanClientDotNet.Models {
  public class PortMapping {
    public int ContainerPort { get; set; }
    public string HostIp { get; set; }
    public int HostPort { get; set; }
    public string Protocol { get; set; }
    public int Range { get; set; }
  }

}
