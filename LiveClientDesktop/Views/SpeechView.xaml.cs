using LiveClientDesktop.Enums;
using LiveClientDesktop.EventAggregations;
using LiveClientDesktop.Models;
using LiveClientDesktop.ViewModels;
using Microsoft.Practices.Prism.Events;
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
    /// SpeechView.xaml 的交互逻辑
    /// </summary>
    public partial class SpeechView : UserControl
    {
        private EventSubscriptionManager _eventSubscriptionManager;
        private SubscriptionToken switchDemonstrationSceneEventSubscriptionToken;
        private SubscriptionToken systemCloseEventSubscriptionToken;
        public SpeechView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as SpeechViewModel;
            if (vm != null)
            {
                _eventSubscriptionManager = vm.EventSubscriptionManager;

                switchDemonstrationSceneEventSubscriptionToken = _eventSubscriptionManager.Subscribe<SwitchDemonstrationSceneEvent, SwitchDemonstrationSceneContext>(null, SwitchDemonstrationSceneEventHandler, null);

                systemCloseEventSubscriptionToken = _eventSubscriptionManager.Subscribe<ShutDownEvent, bool>(null, SystemShutdown, null);
            }
        }
        private void SwitchDemonstrationSceneEventHandler(SwitchDemonstrationSceneContext context)
        {
            switch (context.SceneType)
            {
                case DemonstratioType.PPT:
                    PPTViewer.OpenPPT(context.UseDevice.ToString());
                    DemonstrationPPTScene.Visibility = Visibility.Visible;
                    break;
                case DemonstratioType.VideoDevice:
                    DemonstrationPPTScene.Visibility = Visibility.Hidden;
                    DemonstrationVideoDeviceScene.Visibility = Visibility.Visible;
                    MsPlayer.OpenDevice(context.UseDevice as IVideoDevice);
                    break;
            }
        }
        private void SystemShutdown(bool isClose)
        {

        }
    }
}
