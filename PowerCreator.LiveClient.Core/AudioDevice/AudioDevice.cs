using PowerCreator.LiveClient.Core.Models;
using PowerCreator.LiveClient.Infrastructure.Object;
using PowerCreator.LiveClient.VsNetSdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PowerCreator.LiveClient.Core.AudioDevice
{
    public sealed class AudioDevice : PushingDataEventBase<AudioDeviceDataContext>, IAudioDevice
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
                    _audioDataFormat = _getAudioDataFormat();
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
                _startPullAudioData();
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
        private void _startPullAudioData()
        {
            _isRuningPullAudioData = true;
            Task.Run(() =>
            {
                _pullAudioData();
            });
        }
        private void _pullAudioData()
        {
            while (_isRuningPullAudioData)
            {
                _bufferSize = _getAudioDataSize();
                if (_bufferSize != 0)
                {
                    byte[] buffer = new byte[_bufferSize];
                    _getAudioData(ref buffer[0], _bufferSize);
                    AudioDeviceDataContext audioDeviceData = new AudioDeviceDataContext(buffer.ToIntHandle(), _bufferSize);
                    Pushing(audioDeviceData);
                }
                Thread.Sleep(40);
            }
        }
        private IntPtr _getAudioDataFormat()
        {
            return VsNetSoundRecorderSdk.SoundRecorder_GetFormat(_handle);
        }
        private int _getAudioData(ref byte buff, int size)
        {
            return VsNetSoundRecorderSdk.SoundRecorder_GetData(_handle, ref buff, size);
        }
        private int _getAudioDataSize()
        {
            return VsNetSoundRecorderSdk.SoundRecorder_GetDataSize(_handle);
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

        protected override void OnSubscribe()
        {
            OpenDevice();
        }

        protected override void OnAllUnSubscribe()
        {
            CloseDevice();
        }

        #endregion
    }
}
