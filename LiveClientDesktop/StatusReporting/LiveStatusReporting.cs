using LiveClientDesktop.Enums;
using LiveClientDesktop.EventAggregations;
using LiveClientDesktop.Models;
using LiveClientDesktop.Services;
using Microsoft.Practices.Prism.Logging;
using PowerCreator.LiveClient.Log;
using PowerCreatorDotCom.Sdk.Core;
using System.Threading.Tasks;

namespace LiveClientDesktop.StatusReporting
{
    public class LiveStatusReporting
    {
        private readonly ILoggerFacade _logger;
        private readonly IServiceClient _serviceClient;
        private readonly WebPlatformApiFactory _webPlatformApiFactory;
        public LiveStatusReporting(
            ILoggerFacade logger,
            IServiceClient serviceClient,
            WebPlatformApiFactory webPlatformApiFactory,
            EventSubscriptionManager eventSubscriptionManager)
        {
            _logger = logger;
            _serviceClient = serviceClient;
            _webPlatformApiFactory = webPlatformApiFactory;
            eventSubscriptionManager.Subscribe<LiveAndRecordingOperateEvent, LiveAndRecordingOperateEventContext>(null, Handler, EventFilter);
        }
        private void Handler(LiveAndRecordingOperateEventContext context)
        {
            Task.Run(() =>
            {
                switch (context.EventType)
                {
                    case LiveAndRecordingOperateEventType.Pause:
                        {
                            var paesuRsp = _serviceClient.GetResponse(_webPlatformApiFactory.CreatePauseLiveRequest());
                            if (!paesuRsp.Success) _logger.Error(paesuRsp.Message);
                            break;
                        }
                    case LiveAndRecordingOperateEventType.Stop:
                        {
                            var stopRsp = _serviceClient.GetResponse(_webPlatformApiFactory.CreateStopLiveRequest());
                            if (!stopRsp.Success) _logger.Error(stopRsp.Message);
                            break;
                        }
                }
            });
        }
        private bool EventFilter(LiveAndRecordingOperateEventContext context)
        {
            return context.EventSource == LiveAndRecordingOperateEventSourceType.Live;
        }
    }
}
