using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaksIT.PodmanClientDotNet.Models {
  public class BindOptions {
    public bool CreateMountpoint { get; set; }
    public bool NonRecursive { get; set; }
    public string Propagation { get; set; }
    public bool ReadOnlyForceRecursive { get; set; }
    public bool ReadOnlyNonRecursive { get; set; }
  }

}
