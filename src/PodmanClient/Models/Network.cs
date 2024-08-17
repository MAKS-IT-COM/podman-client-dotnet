using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaksIT.PodmanClientDotNet.Models {
  public class Network {
    public int ClassID { get; set; }
    public List<NetworkPriority> Priorities { get; set; }
  }

}
