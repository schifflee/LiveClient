using PowerCreator.LiveClient.Desktop.ViewModels;
using System.Windows.Controls;

namespace PowerCreator.LiveClient.Desktop.Views
{
    /// <summary>
    /// CameraDeviceView.xaml 的交互逻辑
    /// </summary>
    public partial class CameraDeviceView : UserControl
    {
        public CameraDeviceView(CameraDeviceViewModel cameraDeviceViewModel)
        {
            InitializeComponent();
            this.DataContext = cameraDeviceViewModel;
        }
    }
}
