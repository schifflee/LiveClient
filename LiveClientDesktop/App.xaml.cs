using PowerCreator.LiveClient.Core.VideoDevice;
using System.Windows;

namespace LiveClientDesktop
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs args)
        {
            base.OnStartup(args);
            //VideoDeviceManager.Instance.GetVideoDevices();
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.Run();
        }
    }
}
