using System.Linq;
using LiveClientDesktop.EventAggregations;
using Microsoft.Practices.Prism.Logging;
using PowerCreator.LiveClient.Core;
using PowerCreator.LiveClient.Core.AudioDevice;

namespace LiveClientDesktop.Services
{
    public class TeacherVideoLiveAndRecordProvider : VideoLiveAndRecordBaseProvider
    {
        private readonly SystemConfig _config;
        private readonly ILiveStreamAddressProvider _liveStreamAddressProvider;
        public TeacherVideoLiveAndRecordProvider(SystemConfig config, ILoggerFacade logger, IAudioDeviceManager audioDeviceManager, ILiveStreamAddressProvider liveStreamAddressProvider, EventSubscriptionManager eventSubscriptionManager)
            : base(config, logger, audioDeviceManager, eventSubscriptionManager)
        {
            _liveStreamAddressProvider = liveStreamAddressProvider;
        }


        protected override string Name
        {
            get
            {
                return "教师视频";
            }
        }
        protected override ILiveStreamAddressInfo GetliveStreamAddressInfo()
        {
            var liveStreamAddressInfos = _liveStreamAddressProvider.GetLiveStreamAddressList();
            if (liveStreamAddressInfos.Count() == 2) return liveStreamAddressInfos.Last();
            return null;
        }
    }
}
