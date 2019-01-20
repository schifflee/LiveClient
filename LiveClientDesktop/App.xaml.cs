using Microsoft.Practices.Unity;
using PowerCreator.LiveClient.Core;
using System;
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

            IntPtr loginWnd = User32Api.FindWindow(null, "Login");
            if (loginWnd != IntPtr.Zero)
            {
                User32Api.SetForegroundWindow(loginWnd);
                Current.Shutdown();
                return;
            }

            IntPtr parenthWnd = User32Api.FindWindow(null, "密云直播云课堂");
            if (parenthWnd != IntPtr.Zero)
            {
                User32Api.SetForegroundWindow(parenthWnd);
                Current.Shutdown();
                return;
            }

            base.OnStartup(e);
            string startupPatams = string.Empty;
            if (e.Args.Length > 0) startupPatams = e.Args[0];
            Bootstrapper bootstrapper = new Bootstrapper(startupPatams);
            bootstrapper.Run();
        }
    }
}
