using LiveClientDesktop.Enums;
using LiveClientDesktop.EventAggregations;
using LiveClientDesktop.HttpServer;
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
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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
        private MetroWindow _dialogWindow;
        private Button btnGrdSplitter;
        private GridLength m_WidthCache;
        public Shell(IUnityContainer container)
        {
            InitializeComponent();
            _eventAggregator = container.Resolve<IEventAggregator>();
            _container = container;
            shellViewModel = container.Resolve<ShellViewModel>();
            this.DataContext = shellViewModel;
            if (shellViewModel != null)
            {
                shellViewModel.EventSubscriptionManager.Subscribe<OpenPrevireWindowEvent, bool>(null, ShowPrevireWindowView, null);
                shellViewModel.EventSubscriptionManager.Subscribe<ShowClassRoomTeachingWindowEvent, ClassRoomTeachingWindowType>(null, show, null);
            }
            Task.Run(() =>
            {
                new HttpService(5479).listen();
            });
        }
        private void show(ClassRoomTeachingWindowType windowType)
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
            _eventAggregator.GetEvent<ShutDownEvent>().Publish(true);
        }

        private void MetroWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            btnGrdSplitter = gsSplitterr.Template.FindName("btnExpend", gsSplitterr) as Button;
            if (btnGrdSplitter != null)
                btnGrdSplitter.Click += new RoutedEventHandler(btnGrdSplitter_Click);
        }
        private void btnGrdSplitter_Click(object sender, RoutedEventArgs e)
        {
            GridLength temp = grdWorkbench.ColumnDefinitions[2].Width;
            GridLength def = new GridLength(0);
            if (temp.Equals(def))
            {
                grdWorkbench.ColumnDefinitions[2].Width = m_WidthCache;
                btnGrdSplitter.Content = ">";
            }
            else
            {
                btnGrdSplitter.Content = "<";
                m_WidthCache = grdWorkbench.ColumnDefinitions[2].Width;
                grdWorkbench.ColumnDefinitions[2].Width = def;
            }
        }
    }
}
