using LiveClientDesktop.Models;
using PowerCreator.LiveClient.Core.VideoDevice;
using System;
using System.Collections.Generic;

namespace LiveClientDesktop.Services
{
    public class CameraDeviceService
    {
        private readonly IVideoDeviceManager _videoDeviceManager;
        public CameraDeviceService(IVideoDeviceManager videoDeviceManager)
        {
            _videoDeviceManager = videoDeviceManager ?? throw new ArgumentNullException("videoDeviceManager");
        }
        public IEnumerable<VideoDeviceInfo> GetVideoDevices()
        {
            ICollection<VideoDeviceInfo> videoDevices = new List<VideoDeviceInfo>();
            var devices = _videoDeviceManager.GetVideoDevices();
            foreach (var device in devices)
            {
                if (device.IsAvailable)
                    videoDevices.Add(new VideoDeviceInfo(device));
            }
            return videoDevices;
        }
    }
}
