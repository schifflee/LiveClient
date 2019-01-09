using LiveClientDesktop.Enums;
using LiveClientDesktop.EventAggregations;
using LiveClientDesktop.Models;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LiveClientDesktop.ViewModels
{
    public abstract class TimeRecorderViewModel : NotificationObject
    {
        private readonly Timer _timer;
        private readonly EventSubscriptionManager _eventSubscriptionManager;
        private int _duration = 0;
        public TimeRecorderViewModel(EventSubscriptionManager eventSubscriptionManager)
        {
            _eventSubscriptionManager = eventSubscriptionManager;
            _timer = new Timer((state) => TimerHandler(), null, Timeout.Infinite, Timeout.Infinite);
            DurationText = "00:00:00";

            _eventSubscriptionManager.Subscribe<LiveAndRecordingOperateEvent, LiveAndRecordingOperateEventContext>(null, LiveOrRecordingOperateEventHandler, EventFilter);
        }
        private string duration;

        public string DurationText
        {
            get { return duration; }
            set
            {
                duration = value;
                this.RaisePropertyChanged("DurationText");
            }
        }

        protected virtual void LiveOrRecordingOperateEventHandler(LiveAndRecordingOperateEventContext context)
        {
            switch (context.EventType)
            {
                case LiveAndRecordingOperateEventType.Start:
                    Start();
                    break;
                case LiveAndRecordingOperateEventType.Pause:
                    Pause();
                    break;
                case LiveAndRecordingOperateEventType.Resume:
                    Resume();
                    break;
                case LiveAndRecordingOperateEventType.Stop:
                    Stop();
                    break;
            }
        }
        public abstract bool EventFilter(LiveAndRecordingOperateEventContext context);

        protected virtual void TimerHandler()
        {
            _duration += 1;
            DurationText = TimeSpan.FromSeconds(_duration).ToString(@"hh\:mm\:ss");
        }
        protected virtual void Start()
        {
            _duration = 0;
            Resume();
        }
        protected virtual void Stop()
        {
            Pause();
            DurationText = "00:00:00";
        }

        protected virtual void Pause()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        protected virtual void Resume()
        {
            _timer.Change(1000, 1000);
        }
    }
}
