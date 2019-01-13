using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveClientDesktop.Models;
using PowerCreator.LiveClient.Core;

namespace LiveClientDesktop.Services
{
    public class LocalLiveStreamProvider : ILiveStreamAddressProvider
    {
        private ICollection<ILiveStreamAddressInfo> _liveStreamAddressInfos;
        public LocalLiveStreamProvider()
        {
            _liveStreamAddressInfos = new List<ILiveStreamAddressInfo>() {
                new LiveStreamAddressInfo { AppName = "live", Host = "127.0.0.1", Port = 1935, StreamName = "test" },
                new LiveStreamAddressInfo { AppName = "live", Host = "127.0.0.1", Port = 1935, StreamName = "test1" }
            };
        }

        public string ProviderName
        {
            get
            {
                return "Local";
            }
        }

        public IEnumerable<ILiveStreamAddressInfo> GetLiveStreamAddressList()
        {
            return _liveStreamAddressInfos;
        }
    }
}
