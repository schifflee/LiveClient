using System;
using System.Runtime.ExceptionServices;
using System.Security;
using DirectShowLib;
using PowerCreator.LiveClient.Core.AudioDevice;
using PowerCreator.LiveClient.Core.Models;
using PowerCreator.LiveClient.Infrastructure.Object;
using PowerCreator.LiveClient.VsNetSdk;
using static PowerCreator.LiveClient.VsNetSdk.VsNetAACEncoder;

namespace PowerCreator.LiveClient.Core.AudioEncoder
{
    internal class AacEncoder : PushingDataEventBase<AudioEncodedDataContext>, IAacEncoder
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
        public AacEncoder()
        {
            _handle = AACEncoder_AllocInstance();
            _audioDeviceDataCallBack = AudioDeviceData;
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
                CurrentUseAudioDevice.PushingData += CurrentUseAudioDevice_PushingData;

                IsStartEncoder = StartEncoder();
            }
            return IsStartEncoder;
        }

        [HandleProcessCorruptedStateExceptions]
        [SecurityCritical]
        private void CurrentUseAudioDevice_PushingData(AudioDeviceDataContext value)
        {
            try
            {
                AACEncoder_EncData(_handle, value.Data, value.DataLength, (int)DateTime.Now.Ticks);
            }
            catch { }
        }

        public bool StopAudioEncoder()
        {
            if (!IsStartEncoder) return true;

            CurrentUseAudioDevice.PushingData -= CurrentUseAudioDevice_PushingData;
            IsStartEncoder = !(AACEncoder_StopEnc(_handle) == 0);

            return !IsStartEncoder;
        }
        private bool StartEncoder()
        {
            AACEncoder_GenExtraData(_handle, CurrentUseAudioDevice.AudioDataFormat, _waveFormatEx);
            AACEncoder_SetDataCallFunc(_handle, _audioDeviceDataCallBack, 2018);
            return AACEncoder_StartEnc(_handle, CurrentUseAudioDevice.AudioDataFormat) == 0;
        }
        private void AudioDeviceData(ref DataHeader dataHeader, IntPtr pData, int pContext)
        {
            AudioEncodedDataContext audioEncodedDataContext = new AudioEncodedDataContext(pData, dataHeader.DataSize, (int)dataHeader.TimeStamp, Convert.ToBoolean(dataHeader.KeyFrame));
            Pushing(audioEncodedDataContext);
        }

        protected override void OnSubscribe()
        {

        }

        protected override void OnAllUnSubscribe()
        {
            StopAudioEncoder();
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
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
