using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaksIT.PodmanClientDotNet.Models {
  public class OverlayVolume {
    public string Destination { get; set; }
    public List<string> Options { get; set; }
    public string Source { get; set; }
  }

}
