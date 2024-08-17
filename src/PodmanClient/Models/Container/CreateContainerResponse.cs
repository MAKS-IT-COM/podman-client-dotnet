using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaksIT.PodmanClientDotNet.Models.Container {
  public class CreateContainerResponse {
    public string Id { get; set; }

    public string[] Warnings { get; set; }
  }
}
