using System.Linq;
using LiveClientDesktop.EventAggregations;
using Microsoft.Practices.Prism.Logging;
using PowerCreator.LiveClient.Core;
using PowerCreator.LiveClient.Core.AudioDevice;

namespace LiveClientDesktop.Services
{
    public class SpeechVideoLiveAndRecordProvider : VideoLiveAndRecordBaseProvider
    {
        private readonly ILiveStreamAddressProvider _liveStreamAddressProvider;
        public SpeechVideoLiveAndRecordProvider(SystemConfig config, ILoggerFacade logger, IAudioDeviceManager audioDeviceManager, ILiveStreamAddressProvider liveStreamAddressProvider, EventSubscriptionManager eventSubscriptionManager)
            : base(config, logger, audioDeviceManager, eventSubscriptionManager)
        {
            _liveStreamAddressProvider = liveStreamAddressProvider;
        }

        protected override string Name
        {
            get
            {
                return "演讲视频";
            }
        }

        protected override ILiveStreamAddressInfo GetliveStreamAddressInfo()
        {
            var liveStreamAddressInfos = _liveStreamAddressProvider.GetLiveStreamAddressList();
            if (liveStreamAddressInfos.Any()) return liveStreamAddressInfos.First();
            return null;
        }
    }
}
