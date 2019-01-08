using System;
using System.Threading;
using System.Threading.Tasks;
using DirectShowLib;
using PowerCreator.LiveClient.Core.Enums;
using PowerCreator.LiveClient.Core.Models;
using PowerCreator.LiveClient.Infrastructure.Object;
using PowerCreator.LiveClient.VsNetSdk;

namespace PowerCreator.LiveClient.Core.VideoDevice
{
    public class DesktopWindowCollector : PushingDataEventBase<VideoDeviceDataContext>, IDesktopWindowCollector
    {
        public int ID { get; private set; }

        public string Name { get; private set; }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public VideoDeviceDataFormat Format { get; private set; }

        public bool IsOpen { get; private set; }

        public bool IsAvailable { get; private set; }

        public IntPtr DeviceBitmapInfoHeaderIntPtr
        {
            get
            {
                return _bitmapInfoHeader.ToIntPtrHandle();
            }
        }

        public int DeviceBitmapInfoHeader
        {
            get
            {
                return DeviceBitmapInfoHeaderIntPtr.ToInt32();
            }
        }

        private readonly BitmapInfoHeader _bitmapInfoHeader;
        private readonly IntPtr _handel;
        private IntPtr _windowHandle;
        private BitmapInfo _bitmapInfo;
        private int _width, _height, _bufferSize;
        private byte[] _sourceImageBuffer = new byte[1920 * 1080 * 4];
        private byte[] _newImageBuffer;
        private bool _isRuningCollect;

        public DesktopWindowCollector()
        {
            Name = "桌面窗口采集器";
            ID = DateTime.Now.Millisecond;
            IsAvailable = true;
            _handel = VsNetImgScalerExSdk.ImgScalerEx_AllocInstance();
            _bitmapInfo = new BitmapInfo();
            _bitmapInfoHeader = new BitmapInfoHeader
            {
                Width = 1280,
                Height = 720,
                Compression = 842094169,
                BitCount = 32,
                Planes = 1
            };
        }

        public bool CloseDevice()
        {
            if (!IsOpen) return true;
            _isRuningCollect = false;
            IsOpen = false;
            return VsNetImgScalerExSdk.ImgScalerEx_EndConvert(_handel) == 0;
        }

        public bool OpenDevice()
        {
            if (IsOpen) return true;
            if (_windowHandle == IntPtr.Zero) return false;
            _isRuningCollect = true;
            Task.Run(() =>
            {
                StartCollectImage();
            });
            IsOpen = true;
            return true;
        }

        private void StartCollectImage()
        {
            while (_isRuningCollect)
            {
                VsNetGDI_CopyWndToBitmapSdk.GDI_GetWndWH(_windowHandle, ref _width, ref _height);
                _bufferSize = _width * _height * 4;
                if (_sourceImageBuffer.Length != _bufferSize)
                {
                    _sourceImageBuffer = new byte[_bufferSize];
                    VsNetImgScalerExSdk.ImgScalerEx_EndConvert(_handel);
                    VsNetImgScalerExSdk.ImgScalerEx_BeginConvert(_handel, _width, _height, 28, 1280, 720, 0, 0, 0, 0, 0);
                    _newImageBuffer = new byte[1280 * 720 * 3 / 2];
                }
                VsNetGDI_CopyWndToBitmapSdk.GDI_CopyWndToBitmap(_windowHandle, ref _sourceImageBuffer[0], ref _bitmapInfo);
                VsNetImgScalerExSdk.ImgScalerEx_Convert(_handel, ref _sourceImageBuffer[0], _sourceImageBuffer.Length, ref _newImageBuffer[0], _newImageBuffer.Length);
                Pushing(new VideoDeviceDataContext(_newImageBuffer.ToIntHandle(), _newImageBuffer.Length));
                Thread.Sleep(40);
            }
        }
        public bool SetWindowHandle(IntPtr intPtr)
        {
            _windowHandle = intPtr;
            return true;
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
                    _sourceImageBuffer = null;
                    _newImageBuffer = null;
                    CloseDevice();
                }
                VsNetImgScalerExSdk.ImgScalerEx_FreeInstance(_handel);
                disposedValue = true;
            }
        }
        ~DesktopWindowCollector()
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
