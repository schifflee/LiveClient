using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveClientDesktop.Models;

namespace LiveClientDesktop.Services
{
    public class LocalLiveStreamProvider : ILiveStreamAddressProvider
    {
        private ICollection<LiveStreamAddressInfo> _liveStreamAddressInfos;
        public LocalLiveStreamProvider()
        {
            _liveStreamAddressInfos = new List<LiveStreamAddressInfo>() {
                new LiveStreamAddressInfo { AppName = "live", DominName = "127.0.0.1", Port = 1935, StreamName = "test" },
                new LiveStreamAddressInfo { AppName = "live", DominName = "127.0.0.1", Port = 1935, StreamName = "test1" }
            };
        }

        public string ProviderName
        {
            get
            {
                return "本地直播流";
            }
        }

        public IEnumerable<LiveStreamAddressInfo> GetLiveStreamAddressList()
        {
            return _liveStreamAddressInfos;
        }
    }
}
