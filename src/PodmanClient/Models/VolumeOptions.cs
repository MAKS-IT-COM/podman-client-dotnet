using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaksIT.PodmanClientDotNet.Models {
  public class VolumeOptions {
    public DriverConfig DriverConfig { get; set; }
    public Dictionary<string, string> Labels { get; set; }
    public bool NoCopy { get; set; }
    public string Subpath { get; set; }
  }

}
