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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LiveClientDesktop.Views
{
    /// <summary>
    /// WebPageViewer.xaml 的交互逻辑
    /// </summary>
    public partial class WebPageViewer : UserControl
    {
        WebBrowser wb = new WebBrowser();
        private readonly string _url;
        private readonly string _domain;
        private readonly string _guid;
        public WebPageViewer(string url, string domain, string guid)
        {
            InitializeComponent();
            _url = url;
            _domain = domain;
            _guid = guid;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            wb.Navigate(new Uri(_domain + "/miyun/clientlogin.html?guid=" + _guid + "&m=2333333&redirecturl=" + System.Web.HttpUtility.UrlEncode(_url)));
            MainGrid.Children.Insert(0, wb);
        }
    }
}
