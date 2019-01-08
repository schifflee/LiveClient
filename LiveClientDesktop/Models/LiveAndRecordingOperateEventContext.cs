using LiveClientDesktop.Enums;

namespace LiveClientDesktop.Models
{
    public struct LiveAndRecordingOperateEventContext
    {
        public LiveAndRecordingOperateEventSourceType EventSource { get; }
        public LiveAndRecordingOperateEventType EventType { get; }
        public LiveAndRecordingOperateEventContext(LiveAndRecordingOperateEventSourceType eventSourceType, LiveAndRecordingOperateEventType eventType)
        {
            EventSource = eventSourceType;
            EventType = eventType;
        }

    }
}
