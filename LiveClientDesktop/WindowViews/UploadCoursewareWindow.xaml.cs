using LiveClientDesktop.ViewModels;
using MahApps.Metro.Controls;
using Microsoft.Practices.Unity;
using System.Windows;

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
