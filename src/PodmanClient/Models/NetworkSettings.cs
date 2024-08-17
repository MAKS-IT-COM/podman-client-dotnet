using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaksIT.PodmanClientDotNet.Models {
  public class NetworkSettings {
    public List<string> Aliases { get; set; }
    public string InterfaceName { get; set; }
    public List<string> StaticIps { get; set; }
    public string StaticMac { get; set; }
  }

}
