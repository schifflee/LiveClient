using LiveClientDesktop.Enums;
using LiveClientDesktop.EventAggregations;
using LiveClientDesktop.Models;
using LiveClientDesktop.ViewModels;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using PowerCreator.LiveClient.Core.VideoDevice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LiveClientDesktop.Views
{
    /// <summary>
    /// PowerCreatorPlayerView.xaml 的交互逻辑
    /// </summary>
    public partial class PowerCreatorPlayerView : UserControl
    {
        private IEventAggregator _eventAggregator { get; set; }
        private SubscriptionToken subscriptionToken;
        private SubscriptionToken subscriptionToken1;
        public PowerCreatorPlayerView()
        {
            InitializeComponent();
        }
        public void SwitchingVideoDeviceEventHandler(VideoDeviceEventContext eventContext)
        {
            MsPlayer.OpenDevice(eventContext.OwnerVideoDevice);
        }
        public bool EventFilter(VideoDeviceEventContext eventContext)
        {
            return eventContext.EventType == SwitchingVideoDeviceSourceEventType.Video1;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _eventAggregator = (this.DataContext as PowerCreatorPlayerViewModel).EventAggregator;

            SwitchingVideoDeviceEvent fundAddedEvent = _eventAggregator.GetEvent<SwitchingVideoDeviceEvent>();

            if (subscriptionToken != null)
            {
                fundAddedEvent.Unsubscribe(subscriptionToken);
            }

            subscriptionToken = fundAddedEvent.Subscribe(SwitchingVideoDeviceEventHandler, ThreadOption.UIThread, false, EventFilter);

            SystemClosingEvent fundAddedEvent1 = _eventAggregator.GetEvent<SystemClosingEvent>();

            if (subscriptionToken1 != null)
            {
                fundAddedEvent1.Unsubscribe(subscriptionToken1);
            }

            subscriptionToken1 = fundAddedEvent1.Subscribe(WinClose, ThreadOption.UIThread, false);
        }
        public void WinClose(bool b)
        {
            MsPlayer.CloseDevice();
        }
    }
}
