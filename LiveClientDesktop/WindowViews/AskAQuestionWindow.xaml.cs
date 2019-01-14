using LiveClientDesktop.Views;
using MahApps.Metro.Controls;

namespace LiveClientDesktop.WindowViews
{
    /// <summary>
    /// AskAQuestionWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AskAQuestionWindow : MetroWindow
    {
        public AskAQuestionWindow(StartupParameters startupParameters)
        {
            InitializeComponent();
            WebPageViewer viewer = new WebPageViewer(startupParameters.Domain + "/miyun/clientTeaAsk.aspx?liveid=" + startupParameters.LiveId, startupParameters.Domain, startupParameters.Guid);
            MainGrid.Children.Insert(0, viewer);
        }
    }
}
