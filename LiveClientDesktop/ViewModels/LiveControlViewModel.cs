using LiveClientDesktop.Enums;
using LiveClientDesktop.EventAggregations;
using LiveClientDesktop.HttpRequestHandler;
using LiveClientDesktop.Models;
using LiveClientDesktop.Services;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Unity;
using PowerCreator.LiveClient.Core;
using PowerCreator.LiveClient.Core.Enums;
using PowerCreatorDotCom.Sdk.Core;
using System;
using System.Collections.Generic;
using System.Windows;

namespace LiveClientDesktop.ViewModels
{
    public partial class LiveControlViewModel : NotificationObject
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IVideoLiveProvider _speechVideoLiveAndRecordProvider;
        private readonly IVideoLiveProvider _teacherVideoLiveAndRecordProvider;
        private readonly IServiceClient _serviceClient;
        private readonly WebPlatformApiFactory _webPlatformApiFactory;
        private object syncState = new object();

        public DelegateCommand StartLiveCommand { get; set; }

        public DelegateCommand StopLiveCommand { get; set; }

        public DelegateCommand PauseLiveCommand { get; set; }

        private bool startLiveBtnIsEnable;

        public bool StartLiveBtnIsEnable
        {
            get { return startLiveBtnIsEnable; }
            set
            {
                startLiveBtnIsEnable = value;
                this.RaisePropertyChanged("StartLiveBtnIsEnable");
            }
        }

        private bool stopLiveBtnIsEnable;

        public bool StopLiveBtnIsEnable
        {
            get { return stopLiveBtnIsEnable; }
            set
            {
                stopLiveBtnIsEnable = value;
                this.RaisePropertyChanged("StopLiveBtnIsEnable");
            }
        }

        private bool pauseLiveBtnIsEnable;

        public bool PauseLiveBtnIsEnable
        {
            get { return pauseLiveBtnIsEnable; }
            set
            {
                pauseLiveBtnIsEnable = value;
                this.RaisePropertyChanged("PauseLiveBtnIsEnable");
            }
        }

        public LiveControlViewModel(IEventAggregator eventAggregator, IUnityContainer container)
            : this()
        {
            _serviceClient = container.Resolve<IServiceClient>();
            _webPlatformApiFactory = container.Resolve<WebPlatformApiFactory>();
            _eventAggregator = eventAggregator ?? throw new ArgumentNullException("eventAggregator");
            _speechVideoLiveAndRecordProvider = container.Resolve<SpeechVideoLiveAndRecordProvider>();
            _teacherVideoLiveAndRecordProvider = container.Resolve<TeacherVideoLiveAndRecordProvider>();

            _speechVideoLiveAndRecordProvider.OnNetworkInterruption += NetworkStatus;
            _speechVideoLiveAndRecordProvider.OnNetworkReconnectionFailed += NetworkStatus;
            _speechVideoLiveAndRecordProvider.OnNetworkReconnectionSucceeded += NetworkStatus;

            _teacherVideoLiveAndRecordProvider.OnNetworkInterruption += NetworkStatus;
            _teacherVideoLiveAndRecordProvider.OnNetworkReconnectionFailed += NetworkStatus;
            _teacherVideoLiveAndRecordProvider.OnNetworkReconnectionSucceeded += NetworkStatus;

            StartLiveBtnIsEnable = true;
            StartLiveCommand = new DelegateCommand(new Action(StartLive));
            StopLiveCommand = new DelegateCommand(new Action(StopLive));
            PauseLiveCommand = new DelegateCommand(new Action(PauseLive));
        }

        private void NetworkStatus(string status)
        {
            _eventAggregator.GetEvent<LiveNetworkStatusEvent>().Publish(status);
        }

        private void StartLive()
        {
            MessageBoxResult confirm = MessageBox.Show("确认开始直播？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirm == MessageBoxResult.Yes)
            {
                Tuple<bool, string> result = _StartLive();
                if (!result.Item1)
                {
                    MessageBox.Show(result.Item2);
                }
            }
        }
        private Tuple<bool, string> _StartLive()
        {
            lock (syncState)
            {

                var rsp = _serviceClient.GetResponse(_webPlatformApiFactory.CreateStartLiveRequest());
                if (!rsp.Success) return new Tuple<bool, string>(rsp.Success, rsp.Message);


                //TODO 需要同步直播状态
                var state = _speechVideoLiveAndRecordProvider.LiveState;
                Tuple<bool, string> result = _speechVideoLiveAndRecordProvider.StartLiving();
                if (!result.Item1) return result;

                result = _teacherVideoLiveAndRecordProvider.StartLiving();
                if (!result.Item1)
                {
                    _speechVideoLiveAndRecordProvider.StopLiving();
                    return result;
                }

                StartLiveBtnIsEnable = false;
                StopLiveBtnIsEnable = PauseLiveBtnIsEnable = true;

                if (state == RecAndLiveState.Pause)
                    _eventAggregator.GetEvent<LiveAndRecordingOperateEvent>().Publish(new LiveAndRecordingOperateEventContext(LiveAndRecordingOperateEventSourceType.Live, LiveAndRecordingOperateEventType.Resume));
                else
                    _eventAggregator.GetEvent<LiveAndRecordingOperateEvent>().Publish(new LiveAndRecordingOperateEventContext(LiveAndRecordingOperateEventSourceType.Live, LiveAndRecordingOperateEventType.Start));

                return result;
            }
        }
        private void StopLive()
        {
            MessageBoxResult confirm = MessageBox.Show("是否关闭此直播？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirm == MessageBoxResult.Yes)
            {
                Tuple<bool, string> result = _StopLive();
                if (!result.Item1)
                {
                    MessageBox.Show(result.Item2);
                }
            }
        }
        private Tuple<bool, string> _StopLive()
        {
            lock (syncState)
            {
                Tuple<bool, string> result = _speechVideoLiveAndRecordProvider.StopLiving();
                if (!result.Item1) return result;

                result = _teacherVideoLiveAndRecordProvider.StopLiving();
                if (!result.Item1) return result;

                StartLiveBtnIsEnable = true;
                StopLiveBtnIsEnable = PauseLiveBtnIsEnable = false;

                _eventAggregator.GetEvent<LiveAndRecordingOperateEvent>().Publish(new LiveAndRecordingOperateEventContext(LiveAndRecordingOperateEventSourceType.Live, LiveAndRecordingOperateEventType.Stop));

                return result;
            }
        }

        private void PauseLive()
        {
            Tuple<bool, string> result = _PauseLive();
            if (!result.Item1) MessageBox.Show(result.Item2);
        }
        private Tuple<bool, string> _PauseLive()
        {
            lock (syncState)
            {
                Tuple<bool, string> result = _speechVideoLiveAndRecordProvider.PauseLiving();
                if (!result.Item1) return result;

                result = _teacherVideoLiveAndRecordProvider.PauseLiving();
                if (!result.Item1) return result;

                StartLiveBtnIsEnable = StopLiveBtnIsEnable = true;
                PauseLiveBtnIsEnable = false;

                _eventAggregator.GetEvent<LiveAndRecordingOperateEvent>().Publish(new LiveAndRecordingOperateEventContext(LiveAndRecordingOperateEventSourceType.Live, LiveAndRecordingOperateEventType.Pause));

                return result;
            }
        }
    }
}
