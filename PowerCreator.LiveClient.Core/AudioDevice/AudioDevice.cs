using PowerCreator.LiveClient.Core.Models;
using PowerCreator.LiveClient.Infrastructure.Object;
using PowerCreator.LiveClient.VsNetSdk;
using System;
using System.Runtime.ExceptionServices;
using System.Security;
using System.Threading;
using System.Threading.Tasks;

namespace PowerCreator.LiveClient.Core.AudioDevice
{
    internal sealed class AudioDevice : PushingDataEventBase<AudioDeviceDataContext>, IAudioDevice
    {
        public string Name { get; private set; }

        public int ID { get; private set; }
        public bool IsOpen { get; private set; }
        public IntPtr AudioDataFormat
        {
            get
            {
                if (_audioDataFormat == IntPtr.Zero)
                {
                    _audioDataFormat = GetAudioDataFormat();
                }
                return _audioDataFormat;
            }
        }
        private IntPtr _audioDataFormat;
        private IntPtr _handle;
        private int _bufferSize = 0;
        private bool _isRuningPullAudioData;
        internal AudioDevice(string audioDeviceName, int Id)
        {
            ID = Id;
            Name = audioDeviceName;
            _handle = VsNetSoundRecorderSdk.SoundRecorder_CreateInstance();
        }

        public bool OpenDevice()
        {
            if (IsOpen) return IsOpen;
            int i = 0;
            IsOpen = VsNetSoundRecorderSdk.SoundRecorder_OpenRecorder(_handle, ID, i) == 0;
            if (IsOpen)
            {
                StartPullAudioData();
            }
            return IsOpen;
        }

        public bool CloseDevice()
        {
            if (!IsOpen) return true;

            _isRuningPullAudioData = false;

            IsOpen = !(VsNetSoundRecorderSdk.SoundRecorder_CloseRecorder(_handle) == 0);
            return !IsOpen;
        }
        [HandleProcessCorruptedStateExceptions]
        [SecurityCritical]
        public void GetAudioLevel(int data, int size, ref int leftChannelLoudness, ref int rightChannelLoudness)
        {
            try
            {
                VsNetSoundRecorderSdk.GetAudioLevel(AudioDataFormat, data, size, ref leftChannelLoudness, ref rightChannelLoudness);
            }
            catch { }
        }
        private void StartPullAudioData()
        {
            _isRuningPullAudioData = true;
            Task.Run(() =>
            {
                PullAudioData();
            });
        }
        private void PullAudioData()
        {
            while (_isRuningPullAudioData)
            {
                _bufferSize = GetAudioDataSize();
                if (_bufferSize != 0)
                {
                    byte[] buffer = new byte[_bufferSize];
                    GetAudioData(ref buffer[0], _bufferSize);
                    Pushing(new AudioDeviceDataContext(buffer.ToIntHandle(), _bufferSize));
                }
                Thread.Sleep(40);
            }
        }
        private IntPtr GetAudioDataFormat()
        {
            return VsNetSoundRecorderSdk.SoundRecorder_GetFormat(_handle);
        }
        private int GetAudioData(ref byte buff, int size)
        {
            return VsNetSoundRecorderSdk.SoundRecorder_GetData(_handle, ref buff, size);
        }
        private int GetAudioDataSize()
        {
            return VsNetSoundRecorderSdk.SoundRecorder_GetDataSize(_handle);
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

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                VsNetSoundRecorderSdk.SoundRecorder_FreeInstance(_handle);
                disposedValue = true;
            }
        }
        ~AudioDevice()
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
