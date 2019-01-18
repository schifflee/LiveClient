using DirectShowLib;
using PowerCreator.LiveClient.Core.Models;
using PowerCreator.LiveClient.Core.VideoDevice;
using PowerCreator.LiveClient.Infrastructure.Object;
using PowerCreator.LiveClient.VsNetSdk;
using System;

namespace PowerCreator.LiveClient.Core.VideoEncoder
{
    internal class H264VideoEncoder : PushingDataEventBase<VideoEncodedDataContext>, IVideoEncoder
    {
        public bool IsStartEncoder { get; private set; }

        public IVideoDevice CurrentUseVideoDevice { get; private set; }

        public int IntBitmapInfoHeader
        {
            get
            {
                return _bitmapInfoHeader.ToIntHandle();
            }
        }

        private readonly IntPtr _handle;
        private readonly BitmapInfoHeader _bitmapInfoHeader;
        private DateTime _startEncoderTime;
        private int _bitmapInfoHeaderLength;
        private int _outputData = 0;
        private int _outputDataSize = 0;
        private int _outputTimeStamp = 0;
        private bool _outputKeyFrame = false;
        private int _resolutionWidth = 1280;
        private int _resolutionHeight = 720;
        private int _rate = 1500;
        public H264VideoEncoder()
        {
            _handle = VsNetVideoEncoderSdk.VideoEncoderEx_AllocInstance();
            _bitmapInfoHeader = new BitmapInfoHeader();
        }
        public void SetVideoResolution(int width, int height)
        {
            _resolutionWidth = width;
            _resolutionHeight = height;
        }
        public void SetVideoRate(int rate)
        {
            _rate = rate;
        }
        public bool SetVideoSource(IVideoDevice videoDevice)
        {
            if (!IsStartEncoder)
            {
                CurrentUseVideoDevice = videoDevice;
                return true;
            }
            StopVideoEncoder();
            CurrentUseVideoDevice = videoDevice;
            StartVideoEncoder();
            return true;
        }
        public bool StartVideoEncoder()
        {
            if (IsStartEncoder) return true;
            if (CurrentUseVideoDevice == null)
            {
                throw new NullReferenceException("CurrentUseVideoDevice Can't be null");
            }
            if (!IsStartEncoder && CurrentUseVideoDevice != null)
            {
                _startEncoderTime = DateTime.Now;
                CurrentUseVideoDevice.OpenDevice();
                IsStartEncoder = SetEncoderInfo(CurrentUseVideoDevice.DeviceBitmapInfoHeaderIntPtr, _rate, _resolutionWidth, _resolutionHeight, _bitmapInfoHeader, ref _bitmapInfoHeaderLength);
                CurrentUseVideoDevice.PushingData += CurrentUseVideoDevice_PushingData;
            }
            return IsStartEncoder;
        }

        private void CurrentUseVideoDevice_PushingData(VideoDeviceDataContext value)
        {
            StartVideoEncoder(value.Data, value.DataLength, GetTimeStamp(), ref _outputData, ref _outputDataSize, ref _outputTimeStamp, ref _outputKeyFrame);
            VideoEncodedDataContext videoEncodedDataContext = new VideoEncodedDataContext(_outputData, _outputDataSize, _outputTimeStamp, _outputKeyFrame);
            Pushing(videoEncodedDataContext);
        }

        public bool StopVideoEncoder()
        {
            if (!IsStartEncoder) return true;

            CurrentUseVideoDevice.PushingData -= CurrentUseVideoDevice_PushingData;
            IsStartEncoder = !(VsNetVideoEncoderSdk.VideoEncoderEx_StopEnc(_handle) == 0);

            return !IsStartEncoder;

        }
        private bool SetEncoderInfo(IntPtr bitmapInfoHeader, int rate, int outputWidth, int outputHeight, BitmapInfoHeader outputBitmapInfoHeader, ref int headerSize)
        {
            return VsNetVideoEncoderSdk.VideoEncoderEx_StartEnc(_handle, bitmapInfoHeader, rate, outputWidth, outputHeight, outputBitmapInfoHeader, ref headerSize) == 0;
        }
        private int StartVideoEncoder(int inputData, int inputSize, int inputTimeStamp, ref int outputData, ref int outputDataSize, ref int outputTimeStamp, ref bool frameKey)
        {
            return VsNetVideoEncoderSdk.VideoEncoderEx_EncData(_handle, inputData, inputSize, inputTimeStamp, ref outputData, ref outputDataSize, ref outputTimeStamp, ref frameKey);
        }
        private int GetTimeStamp()
        {
            TimeSpan ts = DateTime.Now - _startEncoderTime;
            return (int)ts.TotalMilliseconds;
        }

        protected override void OnSubscribe()
        {

        }

        protected override void OnAllUnSubscribe()
        {
            StopVideoEncoder();
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    StopVideoEncoder();
                }
                VsNetVideoEncoderSdk.VideoEncoderEx_FreeInstance(_handle);
                disposedValue = true;
            }
        }

        ~H264VideoEncoder()
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
