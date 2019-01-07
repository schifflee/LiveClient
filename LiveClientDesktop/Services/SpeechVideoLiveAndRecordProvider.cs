using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveClientDesktop.Models;
using Microsoft.Practices.Prism.Logging;
using PowerCreator.LiveClient.Core.AudioDevice;

namespace LiveClientDesktop.Services
{
    public class SpeechVideoLiveAndRecordProvider : VideoLiveAndRecordProvider
    {
        private readonly ILiveStreamAddressProvider _liveStreamAddressProvider;
        public SpeechVideoLiveAndRecordProvider(SystemConfig config, ILoggerFacade logger, IAudioDeviceManager audioDeviceManager, ILiveStreamAddressProvider liveStreamAddressProvider)
            : base(config, logger, audioDeviceManager)
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

        protected override LiveStreamAddressInfo GetliveStreamAddressInfo()
        {
            var liveStreamAddressInfos = _liveStreamAddressProvider.GetLiveStreamAddressList();
            if (liveStreamAddressInfos.Any()) return liveStreamAddressInfos.First();
            return null;
        }
    }
}
