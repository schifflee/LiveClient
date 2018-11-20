using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerCreator.LiveClient.Core.Enums;
using PowerCreator.LiveClient.Core.Models;
using PowerCreator.LiveClient.VsNetSdk;

namespace PowerCreator.LiveClient.Core.VideoDevice
{
    public class VideoDevice : IVideoDevice
    {
        public int ID { get; private set; }

        public string Name { get; private set; }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public VideoDeviceDataFormat Format { get; private set; }

        public bool IsOpen { get; private set; }

        public bool IsAvailable { get; private set; }

        private List<IObserver<VideoDeviceData>> _observers;
        private readonly IntPtr _handle;
        private byte[] _buffer;

        private Task _task;
        internal VideoDevice(string videoDeviceName, int id)
        {
            ID = id;
            Name = videoDeviceName;

            _handle = VsNetCameraSdk.Camera_AllocInstance();
            _observers = new List<IObserver<VideoDeviceData>>();

            _setDeviceAvailableStatus();
        }
        public bool CloseDevice()
        {
            if (!_observers.Any())
            {
                VsNetCameraSdk.Camera_CloseCamera(_handle);
                IsOpen = false;
                return true;
            }
            return false;
        }

        public bool OpenDevice()
        {
            IsOpen = _openDevice();
            if (IsOpen)
            {
                int width = 0, height = 0, format = 0;
                VsNetCameraSdk.Camera_GetInfo(_handle, ref width, ref height, ref format);
                Width = width;
                Height = height;
                Format = (VideoDeviceDataFormat)format;
                if (_buffer == null)
                    _buffer = new byte[_getBufferSize()];

                _task = new Task();
            }
            return IsOpen;
        }


        public IDisposable Subscribe(IObserver<VideoDeviceData> observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);
            return new Unsubscriber(_observers, observer);
        }
        public override string ToString()
        {
            return string.Format("Device:{0},DeviceID:{1}", Name, ID);
        }

        private int _getBufferSize()
        {
            return Width * Height * 4;
        }
        private void _setDeviceAvailableStatus()
        {
            IsAvailable = _openDevice();
            CloseDevice();
        }
        private bool _openDevice()
        {
            return VsNetCameraSdk.Camera_OpenCamera(_handle, ID, false, 0, 0);
        }
        private class Unsubscriber : IDisposable
        {
            private List<IObserver<VideoDeviceData>> _observers;
            private IObserver<VideoDeviceData> _observer;

            public Unsubscriber(List<IObserver<VideoDeviceData>> observers, IObserver<VideoDeviceData> observer)
            {
                this._observers = observers;
                this._observer = observer;
            }
            public void Dispose()
            {
                if (_observer != null && _observers.Contains(_observer))
                    _observers.Remove(_observer);
            }
        }
    }
}
