using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaksIT.PodmanClientDotNet.Models {
  public class StartupHealthConfig {
    public long Interval { get; set; }
    public int Retries { get; set; }
    public long StartInterval { get; set; }
    public long StartPeriod { get; set; }
    public int Successes { get; set; }
    public List<string> Test { get; set; }
    public long Timeout { get; set; }
  }

}
