using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaksIT.PodmanClientDotNet.Models {
  public class ImageVolume {
    public string Destination { get; set; }
    public bool ReadWrite { get; set; }
    public string Source { get; set; }
    public string SubPath { get; set; }
  }

}
