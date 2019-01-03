using LiveClientDesktop.EventAggregations;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LiveClientDesktop.ViewModels
{
    public class PlayVolumeControlViewModel : NotificationObject
    {
        private readonly SystemConfig _config;
        private readonly IEventAggregator _eventAggregator;
        private readonly EventSubscriptionManager _eventSubscriptionManager;
        private int _soundVolume;
        public DelegateCommand SoundMuteCommand { get; set; }
        public DelegateCommand PlaySoundCommand { get; set; }
        private SubscriptionToken playVolumeChangeEventSubscriptionToken;

        public PlayVolumeControlViewModel(SystemConfig config, IEventAggregator eventAggregator, EventSubscriptionManager eventSubscriptionManager)
        {
            _config = config ?? throw new ArgumentNullException("config");
            _eventAggregator = eventAggregator ?? throw new ArgumentNullException("eventAggregator");
            _eventSubscriptionManager = eventSubscriptionManager ?? throw new ArgumentNullException("eventSubscriptionManager");

            SoundMuteCommand = new DelegateCommand(new Action(Mute));
            PlaySoundCommand = new DelegateCommand(new Action(PlaySound));

            playVolumeChangeEventSubscriptionToken = eventSubscriptionManager.Subscribe<PlayVolumeChangeEvent, int>(null, PlayVolumeChangeHandler, null);

            SoundVolume = _config.VideoVolume;
        }


        private Visibility displayMuteBtn;

        public Visibility DisplayMuteBtn
        {
            get { return displayMuteBtn; }
            set
            {
                displayMuteBtn = value;
                this.RaisePropertyChanged("DisplayMuteBtn");
            }
        }

        private Visibility displayPlaySoundBtn;

        public Visibility DisplayPlaySoundBtn
        {
            get { return displayPlaySoundBtn; }
            set
            {
                displayPlaySoundBtn = value;
                this.RaisePropertyChanged("DisplayPlaySoundBtn");
            }
        }

        private int soundVolume;

        public int SoundVolume
        {
            get { return soundVolume; }
            set
            {
                soundVolume = value;
                _eventAggregator.GetEvent<PlayVolumeChangeEvent>().Publish(value);
                this.RaisePropertyChanged("SoundVolume");
            }
        }
        private void PlaySound()
        {
            SoundVolume = _soundVolume = _soundVolume < 5 ? 5 : _soundVolume; ;
        }

        private void Mute()
        {
            _soundVolume = SoundVolume;
            SoundVolume = 0;
        }
        private void PlayVolumeChangeHandler(int volume)
        {

            if (volume < 5)
            {
                DisplayMuteBtn = Visibility.Visible;
                DisplayPlaySoundBtn = Visibility.Hidden;
            }
            else
            {
                DisplayMuteBtn = Visibility.Hidden;
                DisplayPlaySoundBtn = Visibility.Visible;
            }
            _config.VideoVolume = volume;
        }

    }
}
