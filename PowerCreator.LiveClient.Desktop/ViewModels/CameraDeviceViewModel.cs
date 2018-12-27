using Microsoft.Practices.Prism.ViewModel;
using PowerCreator.LiveClient.Desktop.Services;

namespace PowerCreator.LiveClient.Desktop.ViewModels
{
    public class CameraDeviceViewModel : NotificationObject
    {
        public CameraDeviceViewModel(CameraDeviceService cameraDeviceService)
        {
            cameraDeviceService.GetVideoDevices();

        }
    }
}
