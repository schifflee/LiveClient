using LiveClientDesktop.EventAggregations;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.ViewModel;
using NAudio.CoreAudioApi;
using System;
using System.Windows;

namespace LiveClientDesktop.ViewModels
{
    public class MicrophoneVolumeControlViewModel : NotificationObject
    {
        private readonly SystemConfig _config;
        private readonly IEventAggregator _eventAggregator;
        private readonly EventSubscriptionManager _eventSubscriptionManager;
        private readonly MMDeviceEnumerator _mMDeviceEnumerator;
        private int _microphoneVolume;
        private SubscriptionToken microphpneVolumeChangeEventSubscriptionToken;
        public DelegateCommand MicrophoneMuteCommand { get; set; }
        public DelegateCommand PlayMicrophoneSoundCommand { get; set; }

        public MicrophoneVolumeControlViewModel(
            SystemConfig config,
            IEventAggregator eventAggregator,
            EventSubscriptionManager eventSubscriptionManager,
            MMDeviceEnumerator mMDeviceEnumerator)
        {
            _config = config ?? throw new ArgumentNullException("config");
            _eventAggregator = eventAggregator ?? throw new ArgumentNullException("_eventAggregator");
            _eventSubscriptionManager = eventSubscriptionManager ?? throw new ArgumentNullException("eventSubscriptionManager");
            _mMDeviceEnumerator = mMDeviceEnumerator ?? throw new ArgumentNullException("mMDeviceEnumerator");

            MicrophoneMuteCommand = new DelegateCommand(() =>
            {
                _microphoneVolume = MicrophoneVolume;
                MicrophoneVolume = 0;
            });
            PlayMicrophoneSoundCommand = new DelegateCommand(() =>
            {
                MicrophoneVolume = _microphoneVolume = _microphoneVolume < 5 ? 5 : _microphoneVolume;
            });

            microphpneVolumeChangeEventSubscriptionToken = eventSubscriptionManager.Subscribe<MicrophoneVolumeChangeEvent, int>(null, MicrophoneVolumeChangeEventHandler, null);

            MicrophoneVolume = _config.MicrophoneVolume;
        }
        private Visibility displayMicrophoneMuteBtn;

        public Visibility DisplayMicrophoneMuteBtn
        {
            get { return displayMicrophoneMuteBtn; }
            set
            {
                displayMicrophoneMuteBtn = value;
                this.RaisePropertyChanged("DisplayMicrophoneMuteBtn");
            }
        }
        private Visibility displayMicrophoneBtn;

        public Visibility DisplayMicrophoneBtn
        {
            get { return displayMicrophoneBtn; }
            set
            {
                displayMicrophoneBtn = value;
                this.RaisePropertyChanged("DisplayMicrophoneBtn");
            }
        }

        private int microphoneVolume;

        public int MicrophoneVolume
        {
            get { return microphoneVolume; }
            set
            {
                microphoneVolume = value < 5 ? 0 : value;
                _eventAggregator.GetEvent<MicrophoneVolumeChangeEvent>().Publish(microphoneVolume);
                this.RaisePropertyChanged("MicrophoneVolume");
            }
        }

        private void MicrophoneVolumeChangeEventHandler(int volume)
        {
            if (volume < 5)
            {
                DisplayMicrophoneBtn = Visibility.Hidden;
                DisplayMicrophoneMuteBtn = Visibility.Visible;
                volume = 0;
            }
            else
            {
                DisplayMicrophoneBtn = Visibility.Visible;
                DisplayMicrophoneMuteBtn = Visibility.Hidden;
            }
            foreach (MMDevice device in _mMDeviceEnumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active))
            {
                device.AudioEndpointVolume.MasterVolumeLevelScalar = volume / 100f;
            }
            _config.MicrophoneVolume = volume;
        }



    }
}
