using LiveClientDesktop.Enums;
using LiveClientDesktop.EventAggregations;
using LiveClientDesktop.Models;
using LiveClientDesktop.ViewModels;
using Microsoft.Practices.Prism.Events;
using PowerCreator.LiveClient.Core;
using PowerCreator.LiveClient.Core.VideoDevice;
using System.Windows;
using System.Windows.Controls;

namespace LiveClientDesktop.Views
{
    /// <summary>
    /// PowerCreatorPlayerView.xaml 的交互逻辑
    /// </summary>
    public partial class PowerCreatorPlayerView : UserControl
    {
        private EventSubscriptionManager _eventSubscriptionManager;
        private IDesktopWindowCollector _desktopWindowCollector;
        private ISetupVideoLiveAndRecordingDevices _setupVideoLiveAndRecordingDevices;
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
                _desktopWindowCollector = vm.DesktopWindowCollector;
                _setupVideoLiveAndRecordingDevices = vm.SetupVideoLiveAndRecordingDevices;

                switchingVideoDeviceEventSubscriptionToken = _eventSubscriptionManager.Subscribe<SwitchingVideoDeviceEvent, VideoDeviceEventContext>(null, SwitchingVideoDeviceEventHandler, EventFilter);

                systemCloseEventSubscriptionToken = _eventSubscriptionManager.Subscribe<ShutDownEvent, bool>(null, SystemShutdown, null);
            }
            DefaultScene.Visibility = Visibility.Visible;
            _desktopWindowCollector?.SetWindowHandle(DefaultScene.Handle);
            _setupVideoLiveAndRecordingDevices?.SetVideoDevice(_desktopWindowCollector);
        }
        private void SwitchingVideoDeviceEventHandler(VideoDeviceEventContext eventContext)
        {
            MsPlayerContainer.Visibility = Visibility.Visible;

            MsPlayer.OpenDevice(eventContext.OwnerVideoDevice);

            _setupVideoLiveAndRecordingDevices?.SetVideoDevice(eventContext.OwnerVideoDevice);
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
