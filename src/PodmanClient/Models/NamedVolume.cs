using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaksIT.PodmanClientDotNet.Models {
  public class NamedVolume {
    public string Dest { get; set; }
    public bool IsAnonymous { get; set; }
    public string Name { get; set; }
    public List<string> Options { get; set; }
    public string SubPath { get; set; }
  }

}
