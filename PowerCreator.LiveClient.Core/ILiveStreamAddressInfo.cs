using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerCreator.LiveClient.Core
{
    public interface ILiveStreamAddressInfo
    {
        string DominName { get; }
        int Port { get; }
        string AppName { get; }
        string StreamName { get; }
        string Type { get; }
    }
}
