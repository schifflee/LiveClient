﻿using DirectShowLib;
using PowerCreator.LiveClient.Core.Models;
using PowerCreator.LiveClient.Core.VideoDevice;
using PowerCreator.LiveClient.Infrastructure.Object;
using PowerCreator.LiveClient.VsNetSdk;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerCreator.LiveClient.Core.VideoEncoder
{
    public class H264VideoEncoder : DevicDataRecipient<VideoDeviceDataContext>, IVideoEncoder
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
        private List<IObserver<VideoEncodedDataContext>> _observers;
        private DateTime _startEncoderTime;
        private int _bitmapInfoHeaderLength;
        private int _outputData = 0;
        private int _outputDataSize = 0;
        private int _outputTimeStamp = 0;
        private bool _outputKeyFrame = false;
        public H264VideoEncoder()
        {
            _handle = VsNetVideoEncoderSdk.VideoEncoderEx_AllocInstance();
            _bitmapInfoHeader = new BitmapInfoHeader();
            _observers = new List<IObserver<VideoEncodedDataContext>>();
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
                IsStartEncoder = _setEncoderInfo(CurrentUseVideoDevice.DeviceBitmapInfoHeaderIntPtr, 1500, 1280, 720, _bitmapInfoHeader, ref _bitmapInfoHeaderLength);
                unsubscriber = CurrentUseVideoDevice.Subscribe(this);
            }
            return IsStartEncoder;
        }

        public bool StopVideoEncoder()
        {
            if (!IsStartEncoder) return true;

            unsubscriber?.Dispose();
            IsStartEncoder = !(VsNetVideoEncoderSdk.VideoEncoderEx_StopEnc(_handle) == 0);

            return !IsStartEncoder;

        }
        private bool _setEncoderInfo(IntPtr bitmapInfoHeader, int rate, int outputWidth, int outputHeight, BitmapInfoHeader outputBitmapInfoHeader, ref int headerSize)
        {
            return VsNetVideoEncoderSdk.VideoEncoderEx_StartEnc(_handle, bitmapInfoHeader, rate, outputWidth, outputHeight, outputBitmapInfoHeader, ref headerSize) == 0;
        }
        private int _startVideoEncoder(int inputData, int inputSize, int inputTimeStamp, ref int outputData, ref int outputDataSize, ref int outputTimeStamp, ref bool frameKey)
        {
            return VsNetVideoEncoderSdk.VideoEncoderEx_EncData(_handle, inputData, inputSize, inputTimeStamp, ref outputData, ref outputDataSize, ref outputTimeStamp, ref frameKey);
        }
        private int _getTimeStamp()
        {
            TimeSpan ts = DateTime.Now - _startEncoderTime;
            return (int)ts.TotalMilliseconds;
        }

        #region IObservable Support
        public IDisposable Subscribe(IObserver<VideoEncodedDataContext> observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);
            return new Unsubscriber<VideoEncodedDataContext>(_observers, observer, (observers) =>
            {
                if (!observers.Any())
                {
                    StopVideoEncoder();
                }
            });
        }
        #endregion

        #region IObserver Support
        public override void OnCompleted() { }

        public override void OnError(Exception error) { }

        public override void OnNext(VideoDeviceDataContext value)
        {
//#if DEBUG
//            Debug.WriteLine("SourceDataLength:" + value.DataLength);
//#endif

            _startVideoEncoder(value.Data, value.DataLength, _getTimeStamp(), ref _outputData, ref _outputDataSize, ref _outputTimeStamp, ref _outputKeyFrame);
            VideoEncodedDataContext videoEncodedDataContext = new VideoEncodedDataContext(_outputData, _outputDataSize, _outputTimeStamp, _outputKeyFrame);
            foreach (var observer in _observers)
            {
//#if DEBUG
//                Debug.WriteLine("EncodedDataLength:" + videoEncodedDataContext.DataLength);
//#endif

                observer.OnNext(videoEncodedDataContext);
            }
        }
        #endregion

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _observers.Clear();
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