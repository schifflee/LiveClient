using LiveClientDesktop.Enums;
using LiveClientDesktop.EventAggregations;
using LiveClientDesktop.HttpServer;
using LiveClientDesktop.ViewModels;
using LiveClientDesktop.Views;
using LiveClientDesktop.WindowViews;
using MahApps.Metro.Controls;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using PowerCreator.WebPlatform.Sdk.WebPlatform.Moedls;
using PowerCreatorDotCom.Sdk.Core;
using System.Threading.Tasks;

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
            switch (windowType) {
                case ClassRoomTeachingWindowType.Sigin:
                    ShowDialogWindow<SignInWindow>();
                    break;
            }
           
        }
        private void ShowPrevireWindowView(bool isOpen)
        {
            ShowDialogWindow<PreviewWindow>();
        }
        private void ShowDialogWindow<T>() where T : MetroWindow
        {
            _dialogWindow = _container.Resolve<T>();
            _dialogWindow.Owner = this;
            _dialogWindow.Closed += (o, args) => _dialogWindow = null;
            _dialogWindow.ShowDialog();
        }
        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _eventAggregator.GetEvent<ShutDownEvent>().Publish(true);
        }
    }
}
