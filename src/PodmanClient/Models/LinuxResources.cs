using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MaksIT.PodmanClientDotNet.Models {
  public class LinuxResources {
    public BlockIO BlockIO { get; set; }
    public CPU CPU { get; set; }
    public List<LinuxDeviceCgroup> Devices { get; set; }
    public List<HugepageLimit> HugepageLimits { get; set; }
    public Memory Memory { get; set; }
    public Network Network { get; set; }
    public Pids Pids { get; set; }
    public Dictionary<string, RdmaResource> Rdma { get; set; }
    public Dictionary<string, string> Unified { get; set; }
  }

}
