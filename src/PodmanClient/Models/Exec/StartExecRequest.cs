using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaksIT.PodmanClientDotNet.Models.Exec {
  public class StartExecRequest {
    public bool Detach { get; set; }
    public bool Tty { get; set; }
    public int? Height { get; set; } // Optional, nullable if not provided
    public int? Width { get; set; }  // Optional, nullable if not provided
  }
}
