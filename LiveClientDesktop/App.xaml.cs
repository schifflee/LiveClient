using Microsoft.Practices.Unity;
using System.Windows;

namespace LiveClientDesktop
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Bootstrapper bootstrapper = new Bootstrapper("sssssss");
            bootstrapper.Run();
        }
    }
}
