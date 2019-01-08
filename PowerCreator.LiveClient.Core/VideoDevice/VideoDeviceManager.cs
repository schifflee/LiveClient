using Microsoft.Practices.Prism.Logging;
using PowerCreator.LiveClient.Infrastructure.Byte.Extensions;
using PowerCreator.LiveClient.VsNetSdk;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PowerCreator.LiveClient.Core.VideoDevice
{
    public class VideoDeviceManager : IVideoDeviceManager
    {
        //public static VideoDeviceManager Instance { get; } = new VideoDeviceManager();

        private readonly IntPtr _handle;

        private readonly ILoggerFacade _logger;

        private IEnumerable<IVideoDevice> _videoDeviceList;
        public VideoDeviceManager(ILoggerFacade logger)
        {
            _logger = logger;
            _handle = VsNetCameraSdk.Camera_AllocInstance();
            _videoDeviceList = GetVideoDeviceList();
        }
        public IEnumerable<IVideoDevice> GetVideoDevices()
        {
            return _videoDeviceList == null ? GetVideoDeviceList() : _videoDeviceList;
        }

        public IVideoDevice GetVideoDeviceById(int id)
        {
            return _videoDeviceList?.FirstOrDefault(c => c.ID == id);
        }

        private IEnumerable<IVideoDevice> GetVideoDeviceList()
        {
            ICollection<IVideoDevice> videoDeviceList = new List<IVideoDevice>();
            int videoDeviceTotal = GetVideoDeviceTotal();
            for (int index = 0; index < videoDeviceTotal; index++)
            {
                videoDeviceList.Add(new VideoDevice(GetVideoDeviceName(index), index,_logger));
            }
            return videoDeviceList;
        }
        private string GetVideoDeviceName(int index)
        {
            byte[] b = new byte[50];
            VsNetCameraSdk.Camera_GetName(_handle, index, ref b[0]);
            return b.GetString();
        }
        private int GetVideoDeviceTotal()
        {
            return VsNetCameraSdk.Camera_GetCount(_handle);
        }

        #region IDisposable Support
        private bool disposedValue = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach (var videoDevice in _videoDeviceList)
                        videoDevice.Dispose();
                }
                VsNetCameraSdk.Camera_FreeInstance(_handle);
                disposedValue = true;
            }
        }
        ~VideoDeviceManager()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion


    }
}
