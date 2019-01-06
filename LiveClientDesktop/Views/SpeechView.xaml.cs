using LiveClientDesktop.Enums;
using LiveClientDesktop.EventAggregations;
using LiveClientDesktop.Models;
using LiveClientDesktop.ViewModels;
using Microsoft.Practices.Prism.Events;
using PowerCreator.LiveClient.Core.VideoDevice;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
        private SubscriptionToken pptClosedEventSubscriptionToken;
        private MediaPlayer player;
        private VideoDrawing videoDrawing;
        private DrawingBrush brush;
        private Timer timer;
        private string videoPath = string.Empty;
        private string imagePath = string.Empty;
        private bool isPlayVideo;
        private object syncObj = new object();
        public SpeechView()
        {
            InitializeComponent();
            player = new MediaPlayer();
            videoDrawing = new VideoDrawing();
            videoDrawing.Rect = new Rect(150, 0, 100, 100);
            videoDrawing.Player = player;
            brush = new DrawingBrush(videoDrawing);
            PlayVideoArea.Background = brush;
            player.MediaEnded += Player_MediaEnded; ;
            player.MediaOpened += Player_MediaOpened;
            timer = new Timer((state) => UpdatePlayTime(), null, Timeout.Infinite, Timeout.Infinite);
        }

        private void Player_MediaOpened(object sender, System.EventArgs e)
        {
            string duration = player.NaturalDuration.TimeSpan.ToString();
            Dispatcher.Invoke(() =>
            {

                VideoDuration.Text = duration;
                VideoPlayProgress.Maximum = player.NaturalDuration.TimeSpan.TotalSeconds;
                VideoPlayProgress.Value = 0;
                syncVideoPlayState(true);
            });
        }
        private void syncVideoPlayState(bool state)
        {
            lock (syncObj)
            {
                isPlayVideo = state;
                if (state)
                {
                    timer.Change(1000, 1000);
                }
                else
                {
                    timer.Change(Timeout.Infinite, Timeout.Infinite);
                }
            }
        }

        private void Player_MediaEnded(object sender, System.EventArgs e)
        {
            player.Stop();
            player.Play();
        }
        private void UpdatePlayTime()
        {
            timer.Change(Timeout.Infinite, Timeout.Infinite);
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    TimeSpan timeSpan = player.Position;
                    VideoPlayProgress.Value = timeSpan.TotalSeconds;
                    PlayTimeText.Text = string.Format("{0:00}:{1:00}:{2:00}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
                });
            }
            finally
            {
                if (isPlayVideo)
                {
                    timer.Change(1000, 1000);
                }
            }
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as SpeechViewModel;
            if (vm != null)
            {
                PPTViewer.SetEventAggregator(vm.EventAggregator);
                _eventSubscriptionManager = vm.EventSubscriptionManager;

                switchDemonstrationSceneEventSubscriptionToken = _eventSubscriptionManager.Subscribe<SwitchDemonstrationSceneEvent, SwitchDemonstrationSceneContext>(null, SwitchDemonstrationSceneEventHandler, null);

                systemCloseEventSubscriptionToken = _eventSubscriptionManager.Subscribe<ShutDownEvent, bool>(null, SystemShutdownHandler, null);

                pptClosedEventSubscriptionToken = _eventSubscriptionManager.Subscribe<PPTClosedEvent, bool>(null, PPTClosedEventHandler, null);
            }
            DefaultScene.Visibility = Visibility.Visible;
        }
        private void SwitchDemonstrationSceneEventHandler(SwitchDemonstrationSceneContext context)
        {
            switch (context.SceneType)
            {
                case DemonstratioType.PPT:
                    AllSceneHidden();
                    player.Pause();
                    PPTViewer.OpenPPT(context.UseDevice.ToString());
                    DemonstrationPPTScene.Visibility = Visibility;
                    break;
                case DemonstratioType.VideoDevice:
                    AllSceneHidden();
                    player.Pause();
                    DemonstrationVideoDeviceScene.Visibility = Visibility.Visible;
                    MsPlayer.OpenDevice(context.UseDevice as IVideoDevice);
                    break;
                case DemonstratioType.Image:
                    player.Pause();
                    if (imagePath != context.UseDevice.ToString())
                    {
                        AllSceneHidden();
                        imagePath = context.UseDevice.ToString();
                        BitmapImage bitmap = new BitmapImage(new Uri(imagePath));
                        ImageControl.ImageSource = bitmap;
                    }
                    DefaultScene.Visibility = Visibility.Visible;
                    break;
                case DemonstratioType.Video:
                    if (context.UseDevice.ToString() != videoPath)
                    {
                        videoPath = context.UseDevice.ToString();
                        AllSceneHidden();
                        player.Open(new Uri(videoPath));
                    }
                    player.Play();
                    DemonstrationVideoScene.Visibility = Visibility.Visible;
                    break;
                default:
                    AllSceneHidden();
                    DefaultScene.Visibility = Visibility.Visible;
                    break;
            }
        }
        private void AllSceneHidden()
        {
            DefaultScene.Visibility = Visibility.Hidden;
            DemonstrationVideoDeviceScene.Visibility = Visibility.Hidden;
            DemonstrationPPTScene.Visibility = Visibility.Hidden;
            DemonstrationVideoScene.Visibility = Visibility.Hidden;
        }
        private void SystemShutdownHandler(bool isClosed)
        {
            try
            {
                MsPlayer.CloseDevice();
            }
            catch { }
            try
            {
                syncVideoPlayState(false);
                player.Stop();
            }
            catch { }

            try
            {
                PPTViewer.Close();
            }
            catch { }
        }
        private void PPTClosedEventHandler(bool isClosed)
        {
            if (isClosed)
                SwitchDemonstrationSceneEventHandler(new SwitchDemonstrationSceneContext { SceneType = DemonstratioType.None });
        }

        private void PlayVideoArea_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            VideoBar.Height = 0;
        }

        private void PlayVideoArea_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            VideoBar.Height = 30;
        }


        private void VideoPlayProgress_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            isPlayVideo = false;
            timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        private void VideoPlayProgress_MouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            isPlayVideo = true;
            player.Position = new TimeSpan((long)(VideoPlayProgress.Value * 10000000));
            timer.Change(1000, 1000);
            player.Play();
        }

        private void VideoPlayProgress_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var timeSpan = new TimeSpan((long)(VideoPlayProgress.Value * 10000000));
            PlayTimeText.Text = string.Format("{0:00}:{1:00}:{2:00}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);

        }

        private void PlayPauseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (PlayPauseIcon.Kind == MaterialDesignThemes.Wpf.PackIconKind.PauseCircleOutline)
            {
                player.Pause();
                PlayPauseIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.PlayCircleOutline;
                PlayPauseBtn.ToolTip = "播放";
                syncVideoPlayState(false);
            }
            else
            {
                player.Play();
                PlayPauseIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.PauseCircleOutline;
                PlayPauseBtn.ToolTip = "暂停";
                syncVideoPlayState(true);
            }
        }
    }
}
