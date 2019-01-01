using LiveClientDesktop.Enums;
using PowerCreator.LiveClient.Core.VideoDevice;

namespace LiveClientDesktop.Models
{
    public class VideoDeviceEventContext
    {
        public IVideoDevice OwnerVideoDevice { get; set; }
        public SwitchingVideoDeviceSourceEventType EventType { get; set; }
    }
}
