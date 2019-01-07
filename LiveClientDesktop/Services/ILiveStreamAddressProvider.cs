using LiveClientDesktop.Models;
using System.Collections.Generic;

namespace LiveClientDesktop.Services
{
    public interface ILiveStreamAddressProvider
    {
        string ProviderName { get; }
        IEnumerable<LiveStreamAddressInfo> GetLiveStreamAddressList();
    }
}
