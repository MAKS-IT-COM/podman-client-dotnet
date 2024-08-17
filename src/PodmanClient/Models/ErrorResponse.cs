using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaksIT.PodmanClientDotNet.Models {
  public class ErrorResponse {
    public string Cause { get; set; }
    public string Message { get; set; }
    public int Response { get; set; }
  }
}
