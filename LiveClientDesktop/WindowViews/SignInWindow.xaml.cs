using LiveClientDesktop.Views;
using MahApps.Metro.Controls;

namespace LiveClientDesktop.WindowViews
{
    /// <summary>
    /// SignInWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SignInWindow : MetroWindow
    {

        public SignInWindow(StartupParameters startupParameters)
        {
            InitializeComponent();
            WebPageViewer viewer = new WebPageViewer(startupParameters.Domain + "/miyun/clientTeaSign.aspx?liveid=" + startupParameters.LiveId, startupParameters.Domain, startupParameters.Guid);
            MainGrid.Children.Insert(0, viewer);
        }
    }
}
