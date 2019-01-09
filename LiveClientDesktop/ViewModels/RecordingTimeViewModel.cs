﻿using LiveClientDesktop.Enums;
using LiveClientDesktop.EventAggregations;
using LiveClientDesktop.Models;

namespace LiveClientDesktop.ViewModels
{
    public class RecordingTimeViewModel : TimeRecorderViewModel
    {
        public RecordingTimeViewModel(EventSubscriptionManager eventSubscriptionManager)
            : base(eventSubscriptionManager)
        {
        }
        public override bool EventFilter(LiveAndRecordingOperateEventContext context)
        {
            return context.EventSource == LiveAndRecordingOperateEventSourceType.Recording;
        }
    }
}
