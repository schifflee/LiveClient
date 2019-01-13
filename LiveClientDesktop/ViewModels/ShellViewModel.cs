using LiveClientDesktop.EventAggregations;
using LiveClientDesktop.StatusReporting;
using Microsoft.Practices.Unity;

namespace LiveClientDesktop.ViewModels
{
    public class ShellViewModel
    {
        [Dependency]
        public ViewModelContext VMContext { get; set; }

        [Dependency]
        public EventSubscriptionManager EventSubscriptionManager { get; set; }

        [Dependency]
        public LiveStatusReporting LiveStatusReporting { get; set; }
    }
}
