using LiveClientDesktop.Enums;
using LiveClientDesktop.EventAggregations;
using LiveClientDesktop.Models;
using LiveClientDesktop.Services;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LiveClientDesktop.ViewModels
{
    public class CameraDeviceViewModel : NotificationObject
    {
        private readonly CameraDeviceService _cameraDeviceService;
        private readonly IEventAggregator _eventAggregator;
        private int _currentSelectDeviceID = -1;
        public DelegateCommand<int?> ChangeCameraCommand { get; set; }

        public CameraDeviceViewModel(CameraDeviceService cameraDeviceService, IEventAggregator eventAggregator)
        {
            _cameraDeviceService = cameraDeviceService ?? throw new ArgumentNullException("cameraDeviceService");
            _eventAggregator = eventAggregator;
            cameraDeviceList = new List<CameraDeviceItemViewModel>();
            ChangeCameraCommand = new DelegateCommand<int?>(new Action<int?>(SetSelectCameraDevice));
            LoadCameraDevices();
        }
        private void LoadCameraDevices()
        {
            foreach (var item in _cameraDeviceService.GetVideoDevices())
            {
                if (item.IsAvailable)
                    cameraDeviceList.Add(new CameraDeviceItemViewModel(new VideoDeviceInfo(item)));
            }
            CameraDeviceList = cameraDeviceList.OrderByDescending(item => item.ID).ToList();
        }
        public void SetSelectCameraDevice(int? deviceID)
        {
            if (!deviceID.HasValue) return;
            if (deviceID.Value == _currentSelectDeviceID) return;
            CameraDeviceList.ForEach(item =>
            {
                item.IsSelected = false;
                if (item.ID == deviceID)
                {
                    item.IsSelected = true;
                    CurrentSelectedDevice = item;
                    _eventAggregator.GetEvent<SwitchingVideoDeviceEvent>().Publish(
                        new VideoDeviceEventContext
                        {
                            OwnerVideoDevice = item.OwnerVideoDevice.OwnerVideoDevice,
                            EventType = SwitchingVideoDeviceSourceEventType.Video1
                        });
                }
            });
        }
        private List<CameraDeviceItemViewModel> cameraDeviceList;
        public List<CameraDeviceItemViewModel> CameraDeviceList
        {
            get { return cameraDeviceList; }
            set
            {
                cameraDeviceList = value;
                this.RaisePropertyChanged("CameraDeviceList");
            }
        }

        private CameraDeviceItemViewModel currentSelectedDevice;
        public CameraDeviceItemViewModel CurrentSelectedDevice
        {
            get { return currentSelectedDevice; }
            set
            {
                currentSelectedDevice = value;
                this.RaisePropertyChanged("CurrentSelectedDevice");
            }
        }
    }
}
