using LiveClientDesktop.Enums;
using LiveClientDesktop.EventAggregations;
using LiveClientDesktop.Models;
using LiveClientDesktop.ViewModels;
using Microsoft.Practices.Prism.Events;
using PowerCreator.LiveClient.Core.VideoDevice;
using Renderer.Core;
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
        private IDesktopWindowCollector _desktopWindowCollector;
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
        private D3DImageSource d3dSource;
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
                _desktopWindowCollector = vm.DesktopWindowCollector;

                switchDemonstrationSceneEventSubscriptionToken = _eventSubscriptionManager.Subscribe<SwitchDemonstrationSceneEvent, SwitchDemonstrationSceneContext>(null, SwitchDemonstrationSceneEventHandler, null);

                systemCloseEventSubscriptionToken = _eventSubscriptionManager.Subscribe<ShutDownEvent, bool>(null, SystemShutdownHandler, null);

                pptClosedEventSubscriptionToken = _eventSubscriptionManager.Subscribe<PPTClosedEvent, bool>(null, PPTClosedEventHandler, null);

                _eventSubscriptionManager.Subscribe<SelectedDemonstrationWindowEvent, PreviewWindowInfo>(null, SwitchPreviewWindowSceneHandler, null);

                _eventSubscriptionManager.Subscribe<PlayVolumeChangeEvent, int>(null, (volume) => {
                    player.Volume = (double)volume / 100;
                }, null);
            }
            DefaultScene.Visibility = Visibility.Visible;
            _desktopWindowCollector.SetWindowHandle(DefaultScene.Handle);
        }
        private void SwitchPreviewWindowSceneHandler(PreviewWindowInfo previewWindowInfo)
        {
            AllSceneHidden();
            player.Pause();
            if (d3dSource == null)
            {
                try
                {
                    d3dSource = new D3DImageSource();

                    if (d3dSource.SetupSurface(1280, 720, FrameFormat.YV12))
                    {
                        this.imageD3D.Source = this.d3dSource.ImageSource;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            _desktopWindowCollector.SetWindowHandle(previewWindowInfo.HWD);
            _desktopWindowCollector.PushingData += _desktopWindowCollector_PushingData;
            LiveWindowPreviewScene.Visibility = Visibility.Visible;
        }

        private void _desktopWindowCollector_PushingData(PowerCreator.LiveClient.Core.Models.VideoDeviceDataContext value)
        {
            try
            {
                this.d3dSource.Render((IntPtr)value.Data);
            }
            catch
            {
            }
        }

        private void SwitchDemonstrationSceneEventHandler(SwitchDemonstrationSceneContext context)
        {
            _desktopWindowCollector.PushingData -= _desktopWindowCollector_PushingData;

            switch (context.SceneType)
            {
                case DemonstratioType.PPT:
                    AllSceneHidden();
                    player.Pause();
                    PPTViewer.OpenPPT(context.UseDevice.ToString());
                    DemonstrationPPTScene.Visibility = Visibility;
                    _desktopWindowCollector.SetWindowHandle(DemonstrationPPTScene.Handle);
                    break;
                case DemonstratioType.VideoDevice:
                    AllSceneHidden();
                    player.Pause();
                    DemonstrationVideoDeviceScene.Visibility = Visibility.Visible;
                    MsPlayer.OpenDevice(context.UseDevice as IVideoDevice);
                    _desktopWindowCollector.SetWindowHandle(DemonstrationVideoDeviceScene.Handle);
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
                    _desktopWindowCollector.SetWindowHandle(DefaultScene.Handle);
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
                    _desktopWindowCollector.SetWindowHandle(DemonstrationVideoScene.Handle);
                    break;
                default:
                    AllSceneHidden();
                    DefaultScene.Visibility = Visibility.Visible;
                    _desktopWindowCollector.SetWindowHandle(DefaultScene.Handle);
                    break;
            }
        }
        private void AllSceneHidden()
        {
            DefaultScene.Visibility = Visibility.Hidden;
            DemonstrationVideoDeviceScene.Visibility = Visibility.Hidden;
            DemonstrationPPTScene.Visibility = Visibility.Hidden;
            DemonstrationVideoScene.Visibility = Visibility.Hidden;
            LiveWindowPreviewScene.Visibility = Visibility.Hidden;
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
            try
            {
                _desktopWindowCollector.PushingData -= _desktopWindowCollector_PushingData;
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
