using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaksIT.PodmanClientDotNet.Models.Exec {
  public class InspectExecResponse {
    public bool Running { get; set; }
    public int ExitCode { get; set; }
    public string ProcessConfig { get; set; } // Additional fields can be added based on your needs
  }
}
