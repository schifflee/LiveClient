using MahApps.Metro.Controls;
using Microsoft.Practices.Unity;
using PowerCreator.LiveClient.Desktop.ViewModels;

namespace PowerCreator.LiveClient.Desktop
{
    /// <summary>
    /// Shell.xaml 的交互逻辑
    /// </summary>
    public partial class Shell : MetroWindow
    {
        public Shell(IUnityContainer container)
        {
            InitializeComponent();
            this.DataContext = new ShellViewModel();
        }
    }
}
