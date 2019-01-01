using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Unity;

namespace LiveClientDesktop.ViewModels
{
    public class CourseContentsViewModel : NotificationObject
    {
        [Dependency]
        public PresentationViewModel PresentationViewModel { get; set; }

        public CameraDeviceViewModel CameraDeviceViewModel { get; private set; }

        public CourseContentsViewModel(CameraDeviceViewModel cameraDeviceViewModel)
        {
            CameraDeviceViewModel = cameraDeviceViewModel;
            CameraDeviceViewModel.SetSelectCameraDevice(0);
        }
    }
}
