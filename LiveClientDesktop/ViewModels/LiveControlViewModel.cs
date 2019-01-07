using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveClientDesktop.ViewModels
{
    public class LiveControlViewModel : NotificationObject
    {
        private readonly IEventAggregator _eventAggregator;

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



        public LiveControlViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator ?? throw new ArgumentNullException("eventAggregator");

            StartLiveBtnIsEnable = true;
            StartLiveCommand = new DelegateCommand(new Action(_StartLive));
            StopLiveCommand = new DelegateCommand(new Action(_StopLive));
            PauseLiveCommand = new DelegateCommand(new Action(_PauseLive));
        }

        private void _StartLive()
        {
            StartLiveBtnIsEnable = false;
            StopLiveBtnIsEnable = PauseLiveBtnIsEnable = true;
        }
        private void _StopLive()
        {
            StartLiveBtnIsEnable = true;
            StopLiveBtnIsEnable = PauseLiveBtnIsEnable = false;
        }

        private void _PauseLive()
        {
            StartLiveBtnIsEnable = StopLiveBtnIsEnable = true;
            PauseLiveBtnIsEnable = false;
        }
    }
}
