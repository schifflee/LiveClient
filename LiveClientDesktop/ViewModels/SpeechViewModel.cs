using LiveClientDesktop.EventAggregations;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;

namespace LiveClientDesktop.ViewModels
{
    public class SpeechViewModel
    {
        [Dependency]
        public EventSubscriptionManager EventSubscriptionManager { get; set; }

        [Dependency]
        public IEventAggregator EventAggregator { get; set; }
    }
}
