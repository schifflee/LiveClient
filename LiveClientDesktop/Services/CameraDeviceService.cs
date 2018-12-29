using PowerCreator.LiveClient.Core.VideoDevice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveClientDesktop.Services
{
    public class CameraDeviceService
    {
        private readonly IVideoDeviceManager _videoDeviceManager;
        public CameraDeviceService(IVideoDeviceManager videoDeviceManager)
        {
            _videoDeviceManager = videoDeviceManager ?? throw new ArgumentNullException("videoDeviceManager");
        }
        public IEnumerable<IVideoDevice> GetVideoDevices()
        {
            return _videoDeviceManager.GetVideoDevices();
        }
    }
}
