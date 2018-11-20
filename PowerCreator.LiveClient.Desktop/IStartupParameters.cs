using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerCreator.LiveClient.Desktop
{
    public interface IStartupParameters
    {
        string LiveId { get; }
        string Guid { get; }
        string Domain { get; }
        string Cookie { get; }
    }
}
