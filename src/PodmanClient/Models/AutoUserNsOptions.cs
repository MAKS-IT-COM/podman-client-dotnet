using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaksIT.PodmanClientDotNet.Models {
  public class AutoUserNsOptions {
    public List<IDMapping> AdditionalGIDMappings { get; set; }
    public List<IDMapping> AdditionalUIDMappings { get; set; }
    public string GroupFile { get; set; }
    public int InitialSize { get; set; }
    public string PasswdFile { get; set; }
    public int Size { get; set; }
  }

}
