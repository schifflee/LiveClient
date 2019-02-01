using LiveClientDesktop.Enums;
using LiveClientDesktop.EventAggregations;
using LiveClientDesktop.Models;
using LiveClientDesktop.ViewModels;
using Microsoft.Practices.Prism.Events;
using PowerCreator.LiveClient.Core;
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
        private ISetupVideoLiveAndRecordingDevices _setupVideoLiveAndRecordingDevices;
        private SubscriptionToken _switchDemonstrationSceneEventSubscriptionToken;
        private SubscriptionToken _systemCloseEventSubscriptionToken;
        private SubscriptionToken _pptClosedEventSubscriptionToken;
        private MediaPlayer _player;
        private VideoDrawing _videoDrawing;
        private DrawingBrush _brush;
        private Timer _timer;
        private string _videoPath = string.Empty;
        private string _imagePath = string.Empty;
        private bool _isPlayVideo;
        private object _syncObj = new object();
        private D3DImageSource _d3dSource;
        public SpeechView()
        {
            InitializeComponent();
            _player = new MediaPlayer();
            _videoDrawing = new VideoDrawing();
            _videoDrawing.Rect = new Rect(150, 0, 100, 100);
            _videoDrawing.Player = _player;
            _brush = new DrawingBrush(_videoDrawing);
            PlayVideoArea.Background = _brush;
            _player.MediaEnded += Player_MediaEnded; ;
            _player.MediaOpened += Player_MediaOpened;
            _timer = new Timer((state) => UpdatePlayTime(), null, Timeout.Infinite, Timeout.Infinite);
        }

        private void Player_MediaOpened(object sender, System.EventArgs e)
        {
            string duration = _player.NaturalDuration.TimeSpan.ToString();
            Dispatcher.Invoke(() =>
            {

                VideoDuration.Text = duration;
                VideoPlayProgress.Maximum = _player.NaturalDuration.TimeSpan.TotalSeconds;
                VideoPlayProgress.Value = 0;
                syncVideoPlayState(true);
            });
        }
        private void syncVideoPlayState(bool state)
        {
            lock (_syncObj)
            {
                _isPlayVideo = state;
                if (state)
                {
                    _timer.Change(1000, 1000);
                }
                else
                {
                    _timer.Change(Timeout.Infinite, Timeout.Infinite);
                }
            }
        }

        private void Player_MediaEnded(object sender, System.EventArgs e)
        {
            _player.Stop();
            _player.Play();
        }
        private void UpdatePlayTime()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    TimeSpan timeSpan = _player.Position;
                    VideoPlayProgress.Value = timeSpan.TotalSeconds;
                    PlayTimeText.Text = string.Format("{0:00}:{1:00}:{2:00}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
                });
            }
            finally
            {
                if (_isPlayVideo)
                {
                    _timer.Change(1000, 1000);
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
                _setupVideoLiveAndRecordingDevices = vm.SetupVideoLiveAndRecordingDevices;

                _switchDemonstrationSceneEventSubscriptionToken = _eventSubscriptionManager.Subscribe<SwitchDemonstrationSceneEvent, SwitchDemonstrationSceneContext>(null, SwitchDemonstrationSceneEventHandler, null);

                _systemCloseEventSubscriptionToken = _eventSubscriptionManager.Subscribe<ShutDownEvent, bool>(null, SystemShutdownHandler, null);

                _pptClosedEventSubscriptionToken = _eventSubscriptionManager.Subscribe<PPTClosedEvent, bool>(null, PPTClosedEventHandler, null);

                _eventSubscriptionManager.Subscribe<SelectedDemonstrationWindowEvent, PreviewWindowInfo>(null, SwitchPreviewWindowSceneHandler, null);

                _eventSubscriptionManager.Subscribe<PlayVolumeChangeEvent, int>(null, (volume) =>
                {
                    _player.Volume = (double)volume / 100;
                }, null);
            }
            DefaultScene.Visibility = Visibility.Visible;
            _desktopWindowCollector.SetWindowHandle(DefaultScene.Handle);
            _setupVideoLiveAndRecordingDevices?.SetVideoDevice(_desktopWindowCollector);
        }
        private void SwitchPreviewWindowSceneHandler(PreviewWindowInfo previewWindowInfo)
        {
            AllSceneHidden();
            _player.Pause();
            if (_d3dSource == null)
            {
                try
                {
                    _d3dSource = new D3DImageSource();

                    if (_d3dSource.SetupSurface(1280, 720, FrameFormat.YV12))
                    {
                        this.imageD3D.Source = this._d3dSource.ImageSource;
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
                this._d3dSource.Render((IntPtr)value.Data);
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
                    _player.Pause();
                    PPTViewer.OpenPPT(context.UseDevice.ToString());
                    DemonstrationPPTScene.Visibility = Visibility;
                    _desktopWindowCollector.SetWindowHandle(DemonstrationPPTScene.Handle);
                    break;
                case DemonstratioType.VideoDevice:
                    AllSceneHidden();
                    _player.Pause();
                    DemonstrationVideoDeviceScene.Visibility = Visibility.Visible;
                    MsPlayer.OpenDevice(context.UseDevice as IVideoDevice);
                    _desktopWindowCollector.SetWindowHandle(DemonstrationVideoDeviceScene.Handle);
                    break;
                case DemonstratioType.Image:
                    _player.Pause();
                    AllSceneHidden();
                    if (_imagePath != context.UseDevice.ToString())
                    {
                        _imagePath = context.UseDevice.ToString();
                        BitmapImage bitmap = new BitmapImage(new Uri(_imagePath));
                        ImageControl.ImageSource = bitmap;
                    }
                    DefaultScene.Visibility = Visibility.Visible;
                    _desktopWindowCollector.SetWindowHandle(DefaultScene.Handle);
                    break;
                case DemonstratioType.Video:
                    if (context.UseDevice.ToString() != _videoPath)
                    {
                        _videoPath = context.UseDevice.ToString();
                        AllSceneHidden();
                        _player.Open(new Uri(_videoPath));
                    }
                    _player.Play();
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
                _player.Stop();
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
            try
            {
                _desktopWindowCollector.Dispose();
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
            _isPlayVideo = false;
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        private void VideoPlayProgress_MouseRightButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _isPlayVideo = true;
            _player.Position = new TimeSpan((long)(VideoPlayProgress.Value * 10000000));
            _timer.Change(1000, 1000);
            _player.Play();
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
                _player.Pause();
                PlayPauseIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.PlayCircleOutline;
                PlayPauseBtn.ToolTip = "播放";
                syncVideoPlayState(false);
            }
            else
            {
                _player.Play();
                PlayPauseIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.PauseCircleOutline;
                PlayPauseBtn.ToolTip = "暂停";
                syncVideoPlayState(true);
            }
        }
    }
}
