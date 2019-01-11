using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveClientDesktop
{
    public class StartupParameters
    {
        public string LiveId { get; set; }

        public string Guid { get; set; }
        public string Domain { get; set; }
        public string Cookie { get; set; }

        public StartupParameters(string liveId, string guid, string domain)
        {
            LiveId = liveId;
            Guid = guid;
            Domain = domain;
        }
    }
}
