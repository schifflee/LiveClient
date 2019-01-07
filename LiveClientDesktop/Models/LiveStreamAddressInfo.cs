using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveClientDesktop.Models
{
    public class LiveStreamAddressInfo
    {
        public string DominName { get; set; }
        public int Port { get; set; }
        public string AppName { get; set; }
        public string StreamName { get; set; }
        public string Type { get; set; }

        public override string ToString()
        {
            return $"{DominName}:{Port}/{AppName}/{StreamName}/{Type}";
        }
    }
}
