using LiveClientDesktop.EventAggregations;
using Microsoft.Practices.Prism.Logging;
using PowerCreator.LiveClient.Core;
using PowerCreator.LiveClient.Core.AudioDevice;

namespace LiveClientDesktop.Services
{
    public abstract class VideoLiveAndRecordBaseProvider : VideoLiveAndRecordProvider
    {
        private readonly SystemConfig _config;
        public VideoLiveAndRecordBaseProvider(SystemConfig config, ILoggerFacade logger, IAudioDeviceManager audioDeviceManager, EventSubscriptionManager eventSubscriptionManager)
            : base(logger, audioDeviceManager, config.UseMicrophoneID)
        {
            _config = config;
            SetViedoEncoderParams(true);
            eventSubscriptionManager.Subscribe<ConfigSaveEvent, bool>(null, SetViedoEncoderParams, null);
        }
        protected void SetViedoEncoderParams(bool b)
        {
            SetVideoResolution(_config.UseResolutionInfo.Width, _config.UseResolutionInfo.Height);
            SetVideoRate(_config.UseRateInfo.Value);
            SetAudioDeviceById(_config.UseMicrophoneID);
        }
    }
}
