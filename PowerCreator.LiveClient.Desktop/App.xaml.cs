using PowerCreator.LiveClient.Core;
using Microsoft.Practices.Unity;
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
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.Run();
            var s = bootstrapper.Container.Resolve<IStartupParameters>();
        }


    }
}
