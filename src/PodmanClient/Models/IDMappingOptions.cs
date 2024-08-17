using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaksIT.PodmanClientDotNet.Models {
  public class IDMappingOptions {
    public bool AutoUserNs { get; set; }
    public AutoUserNsOptions AutoUserNsOpts { get; set; }
    public List<IDMapping> GIDMap { get; set; }
    public bool HostGIDMapping { get; set; }
    public bool HostUIDMapping { get; set; }
    public List<IDMapping> UIDMap { get; set; }
  }

}
