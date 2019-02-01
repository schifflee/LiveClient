using LiveClientDesktop.Enums;
using LiveClientDesktop.EventAggregations;
using LiveClientDesktop.HttpServer;
using LiveClientDesktop.Models;
using LiveClientDesktop.Services;
using LiveClientDesktop.ViewModels;
using LiveClientDesktop.Views;
using LiveClientDesktop.WindowViews;
using MahApps.Metro.Controls;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using PowerCreator.LiveClient.Core;
using PowerCreator.WebPlatform.Sdk.WebPlatform.Moedls;
using PowerCreatorDotCom.Sdk.Core;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WinForm = System.Windows.Forms;
namespace LiveClientDesktop
{
    /// <summary>
    /// Shell.xaml 的交互逻辑
    /// </summary>
    public partial class Shell : MetroWindow
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IUnityContainer _container;
        private readonly ShellViewModel shellViewModel;
        private readonly RuntimeState _runtimeState;
        private readonly UploadCoursewareService _uploadCoursewareService;
        private readonly SystemConfig _config;
        private MetroWindow _dialogWindow;
        private Button _btnGrdSplitter;
        private GridLength _widthCache;
        private WinForm.NotifyIcon _notifyIcon;
        public Shell(IUnityContainer container)
        {
            InitializeComponent();
            _container = container;
            _config = container.Resolve<SystemConfig>();
            _runtimeState = container.Resolve<RuntimeState>();
            _eventAggregator = container.Resolve<IEventAggregator>();
            shellViewModel = container.Resolve<ShellViewModel>();
            DataContext = shellViewModel;
            _uploadCoursewareService = container.Resolve<UploadCoursewareService>();
            shellViewModel.EventSubscriptionManager.Subscribe<OpenPrevireWindowEvent, bool>(null, ShowPrevireWindowView, null);
            shellViewModel.EventSubscriptionManager.Subscribe<ShowClassRoomTeachingWindowEvent, ClassRoomTeachingWindowType>(null, ShowClassRoomTeachingWindow, null);
            shellViewModel.EventSubscriptionManager.Subscribe<LiveNetworkStatusEvent, string>(null, LiveNetworkStatusEventHandler, null);
            Task.Run(() =>
            {
                new HttpService(5479).listen();
            });
            InitializeNotifyIcon();
        }
        private void InitializeNotifyIcon()
        {
            _notifyIcon = new WinForm.NotifyIcon
            {
                Text = shellViewModel.WindowTitle,
                Icon = new System.Drawing.Icon(AppDomain.CurrentDomain.BaseDirectory + "20190119084535786.ico"),
                Visible = true
            };
        }
        private void LiveNetworkStatusEventHandler(string status)
        {
            _notifyIcon.ShowBalloonTip(2000, "系统提示", status, WinForm.ToolTipIcon.Warning);
        }
        private void ShowClassRoomTeachingWindow(ClassRoomTeachingWindowType windowType)
        {
            switch (windowType)
            {
                case ClassRoomTeachingWindowType.Sigin:
                    OpenWindow<SignInWindow>("签到");
                    break;
                case ClassRoomTeachingWindowType.AskAQuestion:
                    OpenWindow<AskAQuestionWindow>("提问");
                    break;
                case ClassRoomTeachingWindowType.Test:
                    OpenWindow<TestWindow>("测验");
                    break;
            }

        }
        private void OpenWindow<T>(string title) where T : MetroWindow
        {
            IntPtr ptr = GetWindowHandle(title);
            if (ptr == IntPtr.Zero) ShowDialogWindow<T>();
            else User32Api.SetForegroundWindow(ptr);
        }
        private IntPtr GetWindowHandle(string windowTitle)
        {
            return User32Api.FindWindow(null, windowTitle);
        }
        private void ShowPrevireWindowView(bool isOpen)
        {
            ShowDialogWindow<PreviewWindow>(true);
        }
        private void ShowDialogWindow<T>(bool ownerMainWindow = false) where T : MetroWindow
        {
            _dialogWindow = _container.Resolve<T>();
            if (ownerMainWindow)
                _dialogWindow.Owner = this;
            _dialogWindow.Closed += (o, args) => _dialogWindow = null;
            _dialogWindow.Show();
        }
        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_uploadCoursewareService.Uploading)
            {
                e.Cancel = true;
                ShowDialogWindow<UploadCoursewareWindow>(true);
                return;
            }

            if (_uploadCoursewareService.IsThereTask)
            {
                e.Cancel = true;
                ShowDialogWindow<UploadCoursewareWindow>(true);
                return;
            }

            if (_runtimeState.IsLiveRunning)
            {
                e.Cancel = true;
                MessageBox.Show("当前系统正在直播,请先关闭直播", "系统提示");
                return;
            }
            if (_runtimeState.IsRecording)
            {
                e.Cancel = true;
                MessageBox.Show("当前系统正在录制,请先关闭录制", "系统提示");
                return;
            }
            _config.Save();
            _eventAggregator.GetEvent<ShutDownEvent>().Publish(true);
        }

        private void MetroWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _btnGrdSplitter = gsSplitterr.Template.FindName("btnExpend", gsSplitterr) as Button;
            if (_btnGrdSplitter != null)
                _btnGrdSplitter.Click += new RoutedEventHandler(BtnGrdSplitter_Click);
        }
        private void BtnGrdSplitter_Click(object sender, RoutedEventArgs e)
        {
            GridLength temp = grdWorkbench.ColumnDefinitions[2].Width;
            GridLength def = new GridLength(0);
            if (temp.Equals(def))
            {
                grdWorkbench.ColumnDefinitions[2].Width = _widthCache;
                _btnGrdSplitter.Content = ">";
            }
            else
            {
                _btnGrdSplitter.Content = "<";
                _widthCache = grdWorkbench.ColumnDefinitions[2].Width;
                grdWorkbench.ColumnDefinitions[2].Width = def;
            }
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            ShowDialogWindow<SettingsWindow>(true);
        }
    }
}
