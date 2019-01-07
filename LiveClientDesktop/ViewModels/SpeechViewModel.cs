using LiveClientDesktop.EventAggregations;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
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
    }
}
