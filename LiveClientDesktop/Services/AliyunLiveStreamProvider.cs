using System.Collections.Generic;
using System.Linq;
using LiveClientDesktop.Models;
using Microsoft.Practices.Prism.Logging;
using PowerCreator.LiveClient.Core;
using PowerCreator.LiveClient.Log;
using PowerCreatorDotCom.Sdk.Core;

namespace LiveClientDesktop.Services
{
    public class AliyunLiveStreamProvider : ILiveStreamAddressProvider
    {
        private readonly ILoggerFacade _logger;
        private readonly IServiceClient _serviceClient;
        private readonly WebPlatformApiFactory _webPlatformApiFactory;
        private ICollection<ILiveStreamAddressInfo> _liveStreamAddressInfos;
        public AliyunLiveStreamProvider(ILoggerFacade logger, IServiceClient serviceClient, WebPlatformApiFactory webPlatformApiFactory)
        {
            _logger = logger;
            _serviceClient = serviceClient;
            _webPlatformApiFactory = webPlatformApiFactory;
            _liveStreamAddressInfos = new List<ILiveStreamAddressInfo>();
        }
        public string ProviderName
        {
            get
            {
                return "Alibaba";
            }
        }

        public IEnumerable<ILiveStreamAddressInfo> GetLiveStreamAddressList()
        {
            if (_liveStreamAddressInfos.Any())
            {
                return _liveStreamAddressInfos;
            }

            var startLiveRsp = _serviceClient.GetResponse(_webPlatformApiFactory.CreateStartLiveRequest());
            if (!startLiveRsp.Success)
            {
                _logger.Error(startLiveRsp.Message);
                return _liveStreamAddressInfos;
            }
            foreach (var streamInfo in startLiveRsp.Value.StreamInfo)
            {
                var temp = streamInfo.Replace("rtmp://", "");

                var tempArr = temp.Split('/');

                _liveStreamAddressInfos.Add(new LiveStreamAddressInfo()
                {
                    Host = tempArr[0],
                    Port = 1935,
                    AppName = tempArr[1],
                    StreamName = tempArr[2]
                });
            }
            return _liveStreamAddressInfos;
        }
    }
}
