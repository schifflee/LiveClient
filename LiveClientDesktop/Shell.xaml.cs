using LiveClientDesktop.EventAggregations;
using LiveClientDesktop.ViewModels;
using MahApps.Metro.Controls;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;

namespace LiveClientDesktop
{
    /// <summary>
    /// Shell.xaml 的交互逻辑
    /// </summary>
    public partial class Shell : MetroWindow
    {
        private IEventAggregator _eventAggregator;
        public Shell(IUnityContainer container)
        {
            InitializeComponent();
            _eventAggregator = container.Resolve<IEventAggregator>();
            this.DataContext = container.Resolve<ShellViewModel>();
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _eventAggregator.GetEvent<ShutDownEvent>().Publish(true);
        }
    }
}
