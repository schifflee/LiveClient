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

namespace LiveClientDesktop.WindowViews
{
    /// <summary>
    /// UploadCoursewareWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UploadCoursewareWindow : MetroWindow
    {
        private readonly ViewModelContext _modelContext;
        public UploadCoursewareWindow()
        {
            InitializeComponent();
        }
        public UploadCoursewareWindow(IUnityContainer unityContainer)
            : this()
        {
            _modelContext = unityContainer.Resolve<ViewModelContext>();
            this.DataContext = _modelContext;
            var status = _modelContext.UploadCoursewareViewModel.GetUploadStatus();
            if (!status.Item1 && status.Item2) _modelContext.UploadCoursewareViewModel.ClearTaskBtnVisibility = Visibility.Visible;
        }

        private void ClearTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            _modelContext.UploadCoursewareViewModel.ClearTask();
            this.Close();
        }
    }
}
