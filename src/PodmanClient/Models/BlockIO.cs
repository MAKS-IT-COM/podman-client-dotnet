using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaksIT.PodmanClientDotNet.Models {
  public class BlockIO {
    public int LeafWeight { get; set; }
    public List<ThrottleDevice> ThrottleReadBpsDevice { get; set; }
    public List<ThrottleDevice> ThrottleReadIopsDevice { get; set; }
    public List<ThrottleDevice> ThrottleWriteBpsDevice { get; set; }
    public List<ThrottleDevice> ThrottleWriteIopsDevice { get; set; }
    public int Weight { get; set; }
    public List<WeightDevice> WeightDevice { get; set; }
  }

}
