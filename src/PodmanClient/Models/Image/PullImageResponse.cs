using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaksIT.PodmanClientDotNet.Models.Image
{
    public class PullImageResponse
    {

        /// <summary>
        /// Error contains text of errors from c/image.
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// ID contains image ID (retained for backwards compatibility).
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Images contains the IDs of the images pulled.
        /// </summary>
        public List<string> Images { get; set; }

        /// <summary>
        /// Stream used to provide output from c/image.
        /// </summary>
        public string Stream { get; set; }
    }
}
