using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Logging;
using PowerCreator.LiveClient.Core.Enums;
using PowerCreator.LiveClient.Core.Models;
using PowerCreator.LiveClient.Infrastructure.Object;
using PowerCreator.LiveClient.VsNetSdk;

namespace PowerCreator.LiveClient.Core.VideoDevice
{
    internal class VideoDevice : PushingDataEventBase<VideoDeviceDataContext>, IVideoDevice
    {
        public int ID { get; private set; }

        public string Name { get;  set; }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public VideoDeviceDataFormat Format { get; private set; }

        public bool IsOpen { get; private set; }

        public bool IsAvailable { get; private set; }

        private IntPtr _deviceBitmapInfoHeader;
        public IntPtr DeviceBitmapInfoHeaderIntPtr
        {
            get
            {
                if (_deviceBitmapInfoHeader == IntPtr.Zero && IsOpen)
                {
                    _deviceBitmapInfoHeader = GetDeviceBitmapInfoHeader();
                }
                return _deviceBitmapInfoHeader;
            }
        }
        public int DeviceBitmapInfoHeader
        {
            get
            {
                return DeviceBitmapInfoHeaderIntPtr.ToInt32();
            }
        }
        private readonly ILoggerFacade _logger;
        private readonly IntPtr _handle;
        private byte[] _buffer;
        private bool _isRuningPullDeviceData;
        private int _bufferHandle;
        private int _frameRate = 30;
        internal VideoDevice(string videoDeviceName, int id, ILoggerFacade logger)
        {
            ID = id;
            Name = videoDeviceName;

            _logger = logger;
            _handle = VsNetCameraSdk.Camera_AllocInstance();
            SetDeviceAvailableStatus();
        }
        public bool CloseDevice()
        {
            if (!IsOpen) return true;

            StopPullDeviceData();
            IsOpen = false;
            VsNetCameraSdk.Camera_CloseCamera(_handle);

            return true;
        }

        public bool OpenDevice()
        {
            if (IsOpen) return IsOpen;

            IsOpen = OpenVideoDevice();
            if (IsOpen)
            {
                int width = 0, height = 0, format = 0;
                VsNetCameraSdk.Camera_GetInfo(_handle, ref width, ref height, ref format);
                Width = width;
                Height = height;
                Format = (VideoDeviceDataFormat)format;
                if (_buffer == null)
                    _buffer = new byte[GetBufferSize()];

                //TODO 应使用配置
                _frameRate = FrameRateSetting.GetCameraFrameRate(width + height);

                StartPullDeviceData();
            }
            return IsOpen;
        }



        public override string ToString()
        {
            return string.Format("Device:{0},DeviceID:{1}", Name, ID);
        }
        private void StopPullDeviceData()
        {
            _isRuningPullDeviceData = false;
        }
        private void StartPullDeviceData()
        {
            _isRuningPullDeviceData = true;
            Task.Run(() =>
            {
                PullDeviceData();
            });
        }
        private void PullDeviceData()
        {
            while (_isRuningPullDeviceData)
            {
                if (IsOpen)
                {
                    GetDeviceData(ref _buffer);
                    if (_bufferHandle <= 0)
                    {
                        _bufferHandle = _buffer.ToIntHandle();
                    }
                    Pushing(new VideoDeviceDataContext(_bufferHandle, _buffer.Length));
                }
                Thread.Sleep(_frameRate);
            }
        }

        private void GetDeviceData(ref byte[] b)
        {
            VsNetCameraSdk.Camera_QueryFrame(_handle, ref b[0], b.Length);
        }
        private IntPtr GetDeviceBitmapInfoHeader()
        {
            return VsNetCameraSdk.Camera_GetInfoEx(_handle);
        }
        private int GetBufferSize()
        {
            return Width * Height * 4;
        }
        private void SetDeviceAvailableStatus()
        {
            IsAvailable = OpenVideoDevice();
            if (IsAvailable)
            {
                CloseVideoDevice();
            }
        }
        private void CloseVideoDevice()
        {
            VsNetCameraSdk.Camera_CloseCamera(_handle);
        }
        private bool OpenVideoDevice()
        {
            return VsNetCameraSdk.Camera_OpenCamera(_handle, ID, false, 0, 0);

        }
        protected override void OnSubscribe()
        {
            OpenDevice();
        }
        protected override void OnAllUnSubscribe()
        {
            CloseDevice();
        }

        #region IDisposable Support
        private bool disposedValue = false;


        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    CloseDevice();
                    _buffer = null;
                }
                VsNetCameraSdk.Camera_FreeInstance(_handle);
                disposedValue = true;
            }
        }

        ~VideoDevice()
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
