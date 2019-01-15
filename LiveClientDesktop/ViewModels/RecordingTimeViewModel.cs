using LiveClientDesktop.Enums;
using LiveClientDesktop.EventAggregations;
using LiveClientDesktop.Models;
using Microsoft.Practices.Prism.Events;

namespace LiveClientDesktop.ViewModels
{
    public class RecordingTimeViewModel : TimeRecorderViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        public RecordingTimeViewModel(IEventAggregator eventAggregator, EventSubscriptionManager eventSubscriptionManager)
            : base(eventSubscriptionManager)
        {
            _eventAggregator = eventAggregator;
        }
        public override bool EventFilter(LiveAndRecordingOperateEventContext context)
        {
            return context.EventSource == LiveAndRecordingOperateEventSourceType.Recording;
        }
    }
}
