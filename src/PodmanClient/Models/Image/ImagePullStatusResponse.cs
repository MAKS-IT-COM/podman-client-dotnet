using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaksIT.PodmanClientDotNet.Models.Image {
  public class ImagePullStatusResponse {
    public string Status { get; set; }
    public string Id { get; set; }
    public string Progress { get; set; }
    public ProgressDetail ProgressDetail { get; set; }
  }
}
