using LiveClientDesktop.ViewModels;
using MahApps.Metro.Controls;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LiveClientDesktop
{
    /// <summary>
    /// Shell.xaml 的交互逻辑
    /// </summary>
    public partial class Shell : MetroWindow
    {
        public Shell(IUnityContainer container)
        {
            InitializeComponent();
            this.DataContext = container.Resolve<ShellViewModel>();
        }
    }
}
