using LiveClientDesktop.EventAggregations;
using LiveClientDesktop.HttpServer;
using LiveClientDesktop.ViewModels;
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
            }
            Task.Run(() =>
            {
                new HttpService(5479).listen();
            });
            LiveClientLoginRequest liveClientLoginRequest = new LiveClientLoginRequest("miyun.smartclass.cn");
            liveClientLoginRequest.AccessToken = "4d9ed79703004af4929467b68e92bf44";
            ServiceClient serviceClient = new ServiceClient();
            var s = serviceClient.GetResponse(liveClientLoginRequest);
            var sss = s.HttpResponse.Headers["Set-Cookie"];
            GetLiveInfoRequest getLiveInfoRequest = new GetLiveInfoRequest("miyun.smartclass.cn") { LiveID=609};
            getLiveInfoRequest.Headers.Add("Cookie",sss);
           var sssss= serviceClient.GetResponse(getLiveInfoRequest); ;
        }
        private void ShowPrevireWindowView(bool isOpen)
        {
            ShowDialogWindow<PreviewWindow>();
        }
        private void ShowDialogWindow<T>() where T : MetroWindow, new()
        {
            _dialogWindow = _container.Resolve<PreviewWindow>();
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
