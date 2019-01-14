using LiveClientDesktop.Views;
using MahApps.Metro.Controls;
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

namespace LiveClientDesktop.WindowViews
{
    /// <summary>
    /// TestWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TestWindow : MetroWindow
    {
        public TestWindow(StartupParameters startupParameters)
        {
            InitializeComponent();
            WebPageViewer viewer = new WebPageViewer(startupParameters.Domain + "/miyun/clientTeaTest.aspx?liveid=" + startupParameters.LiveId, startupParameters.Domain, startupParameters.Guid);
            MainGrid.Children.Insert(0, viewer);
        }
    }
}
