using System;
using System.Collections.Generic;
using PowerCreator.LiveClient.Core.VideoDevice;


namespace PowerCreator.LiveClient.Desktop.Services
{
    public class CameraDeviceService
    {
        private readonly IVideoDeviceManager _videoDeviceManager;
        public CameraDeviceService(IVideoDeviceManager videoDeviceManager)
        {
            if (videoDeviceManager == null)
                throw new ArgumentNullException("videoDeviceManager");
            _videoDeviceManager = videoDeviceManager;
        }
        public IEnumerable<IVideoDevice> GetVideoDevices()
        {
            return _videoDeviceManager.GetVideoDevices();
        }
    }
}
