using LiveClientDesktop.EventAggregations;
using LiveClientDesktop.Models;
using PowerCreator.LiveClient.Core.VideoDevice;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LiveClientDesktop.Services
{
    public class CameraDeviceService
    {
        private readonly IVideoDeviceManager _videoDeviceManager;
        private readonly VideoDeviceAliasService _videoDeviceAliasService;
        public CameraDeviceService(IVideoDeviceManager videoDeviceManager, VideoDeviceAliasService videoDeviceAliasService, EventSubscriptionManager eventSubscriptionManager)
        {
            _videoDeviceManager = videoDeviceManager ?? throw new ArgumentNullException("videoDeviceManager");
            _videoDeviceAliasService = videoDeviceAliasService;
            eventSubscriptionManager.Subscribe<ShutDownEvent, bool>(null, SystemShutdownEventHandler, null);
        }
        public void SystemShutdownEventHandler(bool b)
        {
            _videoDeviceManager.Dispose();
        }
        public IEnumerable<VideoDeviceInfo> GetVideoDevices()
        {
            var deviceAliasList = _videoDeviceAliasService.GetDeviceAliasList();
            if (deviceAliasList == null) deviceAliasList = new List<VideoDeviceAlias>();
            ICollection<VideoDeviceInfo> videoDevices = new List<VideoDeviceInfo>();
            var devices = _videoDeviceManager.GetVideoDevices();
            foreach (var device in devices)
            {
                var alias = deviceAliasList.FirstOrDefault(item => item.DeviceName == device.Name);
                if (alias != null)
                    device.Name = alias.DeviceNoteName;
                if (device.IsAvailable)
                    videoDevices.Add(new VideoDeviceInfo(device));
            }
            return videoDevices;
        }
        public void Dispose()
        {
            _videoDeviceManager.Dispose();
        }
    }
}
