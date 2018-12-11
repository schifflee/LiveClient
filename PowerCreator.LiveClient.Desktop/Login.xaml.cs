using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PowerCreator.LiveClient.Desktop
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : Window
    {
        public Login(IUnityContainer container)
        {
            InitializeComponent();
            Task.Run(() =>
            {

                Thread.Sleep(3000);
                try
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        Shell mainWindow = container.Resolve<Shell>();
                        Application.Current.MainWindow = mainWindow;
                        this.Close();
                        mainWindow.Show();
                    });
                    
                }
                catch (Exception ex)
                {

                }


            });
        }
    }
}
