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
        private EventSubscriptionManager _eventSubscriptionManager;
        private SubscriptionToken switchingVideoDeviceEventSubscriptionToken;
        private SubscriptionToken systemCloseEventSubscriptionToken;
        public PowerCreatorPlayerView()
        {
            InitializeComponent();
        }
      

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as PowerCreatorPlayerViewModel;
            if (vm != null)
            {
                _eventSubscriptionManager = vm.EventSubscriptionManager;

                switchingVideoDeviceEventSubscriptionToken = _eventSubscriptionManager.Subscribe<SwitchingVideoDeviceEvent, VideoDeviceEventContext>(null, SwitchingVideoDeviceEventHandler, EventFilter);

                systemCloseEventSubscriptionToken = _eventSubscriptionManager.Subscribe<ShutDownEvent, bool>(null, SystemShutdown, null);
            }
        }
        private void SwitchingVideoDeviceEventHandler(VideoDeviceEventContext eventContext)
        {
            MsPlayerContainer.Visibility = Visibility.Visible;

            MsPlayer.OpenDevice(eventContext.OwnerVideoDevice);
        }
        private bool EventFilter(VideoDeviceEventContext eventContext)
        {
            return eventContext.EventType == SwitchingVideoDeviceSourceEventType.Video1;
        }
        private void SystemShutdown(bool isClosing)
        {
            if (isClosing)
                MsPlayer.CloseDevice();
        }
    }
}
