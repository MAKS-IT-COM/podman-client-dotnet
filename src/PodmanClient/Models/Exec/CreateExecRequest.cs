using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaksIT.PodmanClientDotNet.Models.Exec
{
    public class CreateExecRequest
    {
        public bool AttachStderr { get; set; }
        public bool AttachStdin { get; set; }
        public bool AttachStdout { get; set; }
        public string[] Cmd { get; set; }
        public string DetachKeys { get; set; }
        public string[] Env { get; set; }
        public bool Privileged { get; set; }
        public bool Tty { get; set; }
        public string User { get; set; }
        public string WorkingDir { get; set; }
    }
}
