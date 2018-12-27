using System;
using System.Collections.Generic;
using PowerCreator.LiveClient.Core.VideoDevice;


namespace PowerCreator.LiveClient.Desktop.Services
{
    public class CameraDeviceService
    {
        public CameraDeviceService()
        {
        }
        public IEnumerable<IVideoDevice> GetVideoDevices()
        {
            return VideoDeviceManager.Instance.GetVideoDevices();
        }
    }
}
