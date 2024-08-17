using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaksIT.PodmanClientDotNet.Models {
  public class CPU {
    public int Burst { get; set; }
    public string Cpus { get; set; }
    public int Idle { get; set; }
    public string Mems { get; set; }
    public int Period { get; set; }
    public int Quota { get; set; }
    public int RealtimePeriod { get; set; }
    public int RealtimeRuntime { get; set; }
    public int Shares { get; set; }
  }

}
