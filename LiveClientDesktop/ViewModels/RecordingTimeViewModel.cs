using LiveClientDesktop.Enums;
using LiveClientDesktop.EventAggregations;
using LiveClientDesktop.Models;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveClientDesktop.ViewModels
{
    public class RecordingTimeViewModel : NotificationObject
    {
        private readonly EventSubscriptionManager _eventSubscriptionManager;

        

        public RecordingTimeViewModel(EventSubscriptionManager eventSubscriptionManager)
        {
            _eventSubscriptionManager = eventSubscriptionManager;
            _eventSubscriptionManager.Subscribe<LiveAndRecordingOperateEvent, LiveAndRecordingOperateEventContext>(null, RecordingOperateEventHandler, EventFilter);
        }
        private void RecordingOperateEventHandler(LiveAndRecordingOperateEventContext context)
        {

        }
        private bool EventFilter(LiveAndRecordingOperateEventContext context)
        {
            return context.EventSource == LiveAndRecordingOperateEventSourceType.Recording;
        }

    }
}
