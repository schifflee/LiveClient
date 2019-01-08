using LiveClientDesktop.Models;
using PowerCreator.LiveClient.Core;
using System.Collections.Generic;

namespace LiveClientDesktop.Services
{
    public interface ILiveStreamAddressProvider
    {
        string ProviderName { get; }
        IEnumerable<ILiveStreamAddressInfo> GetLiveStreamAddressList();
    }
}
