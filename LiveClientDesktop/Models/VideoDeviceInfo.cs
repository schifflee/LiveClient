using PowerCreator.LiveClient.Core.VideoDevice;

namespace LiveClientDesktop.Models
{
    public class VideoDeviceInfo
    {
        public VideoDeviceInfo(IVideoDevice videoDevice)
        {
            OwnerVideoDevice = videoDevice;
        }
        public string Name => OwnerVideoDevice?.Name;
        public int ID
        {
            get
            {
                if (OwnerVideoDevice == null) return 0;
                return OwnerVideoDevice.ID;
            }
        }
        public bool IsEnable
        {
            get
            {
                if (OwnerVideoDevice == null) return false;
                return OwnerVideoDevice.IsAvailable;
            }
        }

        public IVideoDevice OwnerVideoDevice { get;private set; }
    }
}
