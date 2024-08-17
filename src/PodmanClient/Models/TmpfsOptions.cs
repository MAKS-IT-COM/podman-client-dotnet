using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaksIT.PodmanClientDotNet.Models {
  public class TmpfsOptions {
    public int Mode { get; set; }
    public List<string> Options { get; set; }
    public long SizeBytes { get; set; }
  }

}
