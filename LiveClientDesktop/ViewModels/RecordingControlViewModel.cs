using LiveClientDesktop.Enums;
using LiveClientDesktop.EventAggregations;
using LiveClientDesktop.Models;
using LiveClientDesktop.Services;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Unity;
using PowerCreator.LiveClient.Core;
using PowerCreator.LiveClient.Core.Enums;
using System;
using System.Windows;

namespace LiveClientDesktop.ViewModels
{
    public partial class RecordingControlViewModel : NotificationObject
    {
        private readonly SystemConfig _config;
        private readonly LiveInfo _liveInfo;
        private readonly IEventAggregator _eventAggregator;
        private readonly EventSubscriptionManager _eventSubscriptionManager;
        private readonly IVideoRecordingProvider _speechVideoLiveAndRecordProvider;
        private readonly IVideoRecordingProvider _teacherVideoLiveAndRecordProvider;
        private RecordVideoInfo _speechVideoRecordingInfo;
        private RecordVideoInfo _teacherVideoRecordingInfo;
        private RecordInfo _recordInfo;
        private object syncState = new object();
        public DelegateCommand StartRecordingCommand { get; set; }

        public DelegateCommand StopRecordingCommand { get; set; }

        public DelegateCommand PauseRecordingCommand { get; set; }

        private bool startRecordingBtnIsEnable;

        public bool StartRecordingBtnIsEnable
        {
            get { return startRecordingBtnIsEnable; }
            set
            {
                startRecordingBtnIsEnable = value;
                this.RaisePropertyChanged("StartRecordingBtnIsEnable");
            }
        }
        private bool stopRecordingBtnIsEnable;

        public bool StopRecordingBtnIsEnable
        {
            get { return stopRecordingBtnIsEnable; }
            set
            {
                stopRecordingBtnIsEnable = value;
                this.RaisePropertyChanged("StopRecordingBtnIsEnable");
            }
        }

        private bool pauseRecordingBtnIsEnable;

        public bool PauseRecordingBtnIsEnable
        {
            get { return pauseRecordingBtnIsEnable; }
            set
            {
                pauseRecordingBtnIsEnable = value;
                this.RaisePropertyChanged("PauseRecordingBtnIsEnable");
            }
        }

        public RecordingControlViewModel(
            SystemConfig config,
            IEventAggregator eventAggregator,
            IUnityContainer container,
            EventSubscriptionManager eventSubscriptionManager)
            : this()
        {
            _config = config;
            _eventAggregator = eventAggregator;
            _eventSubscriptionManager = eventSubscriptionManager;
            _liveInfo = container.Resolve<LiveInfo>();
            _speechVideoLiveAndRecordProvider = container.Resolve<SpeechVideoLiveAndRecordProvider>();
            _teacherVideoLiveAndRecordProvider = container.Resolve<TeacherVideoLiveAndRecordProvider>();

            StartRecordingBtnIsEnable = true;
            StartRecordingCommand = new DelegateCommand(new Action(StartRecording));
            StopRecordingCommand = new DelegateCommand(new Action(StopRecording));
            PauseRecordingCommand = new DelegateCommand(new Action(PauseRecording));

            _eventSubscriptionManager.Subscribe<LiveAndRecordingOperateEvent, LiveAndRecordingOperateEventContext>(null, LiveOperateEventHandler, EventFilter);
            _eventSubscriptionManager.Subscribe<ShutDownEvent, bool>(null, SystemShutDownEventHandler, null);
        }
        private void LiveOperateEventHandler(LiveAndRecordingOperateEventContext context)
        {
            if (_config.RecordingStatusChangesAccordingToLiveBroadcastStatus)
            {
                switch (context.EventType)
                {
                    case LiveAndRecordingOperateEventType.Start:
                    case LiveAndRecordingOperateEventType.Resume:
                        _StartRecording();
                        break;
                    case LiveAndRecordingOperateEventType.Pause:
                        _PauseRecording();
                        break;
                    case LiveAndRecordingOperateEventType.Stop:
                        _StopRecording();
                        break;
                }
            }
        }
        private bool EventFilter(LiveAndRecordingOperateEventContext context)
        {
            return context.EventSource == LiveAndRecordingOperateEventSourceType.Live;
        }

        private void StartRecording()
        {
            Tuple<bool, string> result = _StartRecording();
            if (!result.Item1)
            {
                MessageBox.Show(result.Item2);
            }
        }
        private Tuple<bool, string> _StartRecording()
        {
            lock (syncState)
            {

                int index = _config.VideoIndex;
                var state = _speechVideoLiveAndRecordProvider.RecordState;
                if (state == RecAndLiveState.NotStart)
                {
                    _speechVideoRecordingInfo = CreateRecordingFileInfo(VideoType.VGA);
                    _teacherVideoRecordingInfo = CreateRecordingFileInfo(VideoType.Video1);
                    _recordInfo = new RecordInfo { Index = index, ScheduleId = _liveInfo.ScheduleID, StartRecordingTime = DateTime.Now, Title = _liveInfo.Title };
                    _recordInfo.VideoFiles.Add(_speechVideoRecordingInfo);
                    _recordInfo.VideoFiles.Add(_teacherVideoRecordingInfo);
                }
                Tuple<bool, string> result = _speechVideoLiveAndRecordProvider.StartRecording(_config.RecFileSavePath, _speechVideoRecordingInfo.FileName);
                if (!result.Item1) return result;

                result = _teacherVideoLiveAndRecordProvider.StartRecording(_config.RecFileSavePath, _teacherVideoRecordingInfo.FileName);

                if (!result.Item1)
                {
                    _speechVideoLiveAndRecordProvider.StopRecording();
                    return result;
                }

                StartRecordingBtnIsEnable = false;
                StopRecordingBtnIsEnable = PauseRecordingBtnIsEnable = true;

                if (state == RecAndLiveState.Pause)
                    _eventAggregator.GetEvent<LiveAndRecordingOperateEvent>().Publish(new LiveAndRecordingOperateEventContext(LiveAndRecordingOperateEventSourceType.Recording, LiveAndRecordingOperateEventType.Resume));
                else
                    _eventAggregator.GetEvent<LiveAndRecordingOperateEvent>().Publish(new LiveAndRecordingOperateEventContext(LiveAndRecordingOperateEventSourceType.Recording, LiveAndRecordingOperateEventType.Start));


                return result;
            }
        }

        private RecordVideoInfo CreateRecordingFileInfo(VideoType videoType)
        {
            return new RecordVideoInfo()
            {
                FileSavePath = _config.RecFileSavePath,
                FileName = string.Format("{0}{1:yyMMddHHmmss}-{2}.mp4", _liveInfo.Title, DateTime.Now, _config.VideoIndex),
                VideoType = videoType
            };
        }

        private void StopRecording()
        {
            Tuple<bool, string> result = _StopRecording();
            if (!result.Item1)
            {
                MessageBox.Show(result.Item2);
            }
        }

        private Tuple<bool, string> _StopRecording()
        {
            lock (syncState)
            {
                Tuple<bool, string> result = _speechVideoLiveAndRecordProvider.StopRecording();
                if (!result.Item1) return result;

                result = _teacherVideoLiveAndRecordProvider.StopRecording();
                if (!result.Item1) return result;

                StartRecordingBtnIsEnable = true;
                StopRecordingBtnIsEnable = PauseRecordingBtnIsEnable = false;

                _eventAggregator.GetEvent<LiveAndRecordingOperateEvent>().Publish(new LiveAndRecordingOperateEventContext(LiveAndRecordingOperateEventSourceType.Recording, LiveAndRecordingOperateEventType.Stop));

                _recordInfo.StopRecordingTime = DateTime.Now;
                _eventAggregator.GetEvent<RecordCompletedEvent>().Publish(_recordInfo);
                _config.VideoIndex += 1;
                return result;
            }
        }

        private void PauseRecording()
        {
            Tuple<bool, string> result = _PauseRecording();
            if (!result.Item1)
            {
                MessageBox.Show(result.Item2);
            }
        }

        private Tuple<bool, string> _PauseRecording()
        {
            lock (syncState)
            {
                Tuple<bool, string> result = _speechVideoLiveAndRecordProvider.PauseRecording();
                if (!result.Item1) return result;

                result = _teacherVideoLiveAndRecordProvider.PauseRecording();
                if (!result.Item1) return result;

                PauseRecordingBtnIsEnable = false;
                StopRecordingBtnIsEnable = StartRecordingBtnIsEnable = true;

                _eventAggregator.GetEvent<LiveAndRecordingOperateEvent>().Publish(new LiveAndRecordingOperateEventContext(LiveAndRecordingOperateEventSourceType.Recording, LiveAndRecordingOperateEventType.Pause));

                return result;
            }
        }
        private void SystemShutDownEventHandler(bool b)
        {
            _speechVideoLiveAndRecordProvider.Dispose();
            _teacherVideoLiveAndRecordProvider.Dispose();
        }
    }
}
