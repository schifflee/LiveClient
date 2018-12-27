using Microsoft.Practices.Unity;
using PowerCreator.LiveClient.Core.VideoDevice;
using System.Windows;

namespace PowerCreator.LiveClient.Desktop
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs args)
        {
            base.OnStartup(args);
            VideoDeviceManager.Instance.GetVideoDevices();
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.Run();
            var s = bootstrapper.Container.Resolve<IStartupParameters>();
        }


    }
}
