using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using DirectShowLib;
using Microsoft.Practices.Prism.Logging;
using PowerCreator.LiveClient.Core.AudioDevice;
using PowerCreator.LiveClient.Core.Models;
using PowerCreator.LiveClient.Infrastructure.Object;
using PowerCreator.LiveClient.VsNetSdk;
using static PowerCreator.LiveClient.VsNetSdk.VsNetAACEncoder;

namespace PowerCreator.LiveClient.Core.AudioEncoder
{
    public class AacEncoder : DevicDataRecipient<AudioDeviceDataContext>, IAacEncoder
    {
        public bool IsStartEncoder { get; private set; }

        public IAudioDevice CurrentUseAudioDevice { get; private set; }
        public IntPtr IntPtrWaveFormatEx
        {
            get
            {
                return _waveFormatEx.ToIntPtrHandle();
            }
        }

        public int IntWaveFormatEx
        {
            get
            {
                return IntPtrWaveFormatEx.ToInt32();
            }
        }

        private readonly IntPtr _handle;
        private readonly WaveFormatEx _waveFormatEx;
        private readonly AudioDeviceDataCallBack _audioDeviceDataCallBack;
        private List<IObserver<AudioEncodedDataContext>> _observers;
        public AacEncoder()
        {
            _handle = AACEncoder_AllocInstance();
            _audioDeviceDataCallBack = _audioDeviceData;
            _observers = new List<IObserver<AudioEncodedDataContext>>();
            _waveFormatEx = new WaveFormatEx();
        }

        public bool SetAudioDataSource(IAudioDevice audioDevice)
        {
            if (!IsStartEncoder)
            {
                CurrentUseAudioDevice = audioDevice;
                return true;
            }
            StopAudioEncoder();
            CurrentUseAudioDevice = audioDevice;
            StartAudioEncoder();
            return true;
        }

        public bool StartAudioEncoder()
        {
            if (IsStartEncoder) return true;
            if (CurrentUseAudioDevice == null)
            {
                throw new NullReferenceException("CurrentUseAudioDevice Can't be null");
            }
            if (!IsStartEncoder && CurrentUseAudioDevice != null)
            {
                CurrentUseAudioDevice.OpenDevice();
                unsubscriber = CurrentUseAudioDevice.Subscribe(this);

                IsStartEncoder = _startEncoder();
            }
            return IsStartEncoder;
        }

        public bool StopAudioEncoder()
        {
            if (!IsStartEncoder) return true;

            unsubscriber?.Dispose();
            IsStartEncoder = !(AACEncoder_StopEnc(_handle) == 0);

            return !IsStartEncoder;
        }
        private bool _startEncoder()
        {
            AACEncoder_GenExtraData(_handle, CurrentUseAudioDevice.AudioDataFormat, _waveFormatEx);
            AACEncoder_SetDataCallFunc(_handle, _audioDeviceDataCallBack, 2018);
            return AACEncoder_StartEnc(_handle, CurrentUseAudioDevice.AudioDataFormat) == 0;
        }
        private void _audioDeviceData(ref DataHeader dataHeader, IntPtr pData, int pContext)
        {
//#if DEBUG
//            Debug.WriteLine("_audioDeviceData:" + dataHeader.DataSize);
//#endif
            AudioEncodedDataContext audioEncodedDataContext = new AudioEncodedDataContext(pData, dataHeader.DataSize, (int)dataHeader.TimeStamp, Convert.ToBoolean(dataHeader.KeyFrame));
            foreach (var observer in _observers)
            {
                observer.OnNext(audioEncodedDataContext);
            }
        }

        #region DevicDataRecipient Support
        public override void OnCompleted() { }

        public override void OnError(Exception error) { }

        [HandleProcessCorruptedStateExceptions]
        [SecurityCritical]
        public override void OnNext(AudioDeviceDataContext value)
        {
//#if DEBUG
//            Debug.WriteLine("SourceAudioDataLength:" + value.DataLength);
//#endif
            try
            {
                AACEncoder_EncData(_handle, value.Data, value.DataLength, (int)DateTime.Now.Ticks);
            }
            catch { }
        }
        #endregion

        #region IObservable Support
        public IDisposable Subscribe(IObserver<AudioEncodedDataContext> observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);
            return new Unsubscriber<AudioEncodedDataContext>(_observers, observer, (observers) =>
            {
                if (!observers.Any())
                {
                    StopAudioEncoder();
                }
            });
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
                    StopAudioEncoder();
                }
                AACEncoder_FreeInstance(_handle);
                disposedValue = true;
            }
        }
        ~AacEncoder()
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
