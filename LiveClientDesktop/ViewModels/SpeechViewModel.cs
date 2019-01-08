using LiveClientDesktop.EventAggregations;
using LiveClientDesktop.Services;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using PowerCreator.LiveClient.Core;
using PowerCreator.LiveClient.Core.VideoDevice;

namespace LiveClientDesktop.ViewModels
{
    public class SpeechViewModel
    {
        [Dependency]
        public EventSubscriptionManager EventSubscriptionManager { get; set; }

        [Dependency]
        public IEventAggregator EventAggregator { get; set; }

        [Dependency]
        public IDesktopWindowCollector DesktopWindowCollector { get; set; }

        public ISetupVideoLiveAndRecordingDevices SetupVideoLiveAndRecordingDevices { get; set; }
        public SpeechViewModel(IUnityContainer container)
        {
            SetupVideoLiveAndRecordingDevices = container.Resolve<SpeechVideoLiveAndRecordProvider>();
        }
    }
}
