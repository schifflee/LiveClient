using PowerCreator.LiveClient.VsNetSdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PowerCreator.LiveClient.Core.AudioDevice
{
    public class AudioDeviceManager : IAudioDeviceManager
    {
        private readonly IntPtr _handle;

        private ICollection<IAudioDevice> _audioDevices;
        public AudioDeviceManager()
        {
            _handle = VsNetSoundRecorderSdk.SoundRecorder_CreateInstance();
        }
        public IAudioDevice GetAudioDeviceById(int id)
        {
            return GetAudioDevices().FirstOrDefault(a => a.ID == id);
        }

        public IEnumerable<IAudioDevice> GetAudioDevices()
        {
            if (_audioDevices != null)
                return _audioDevices;

            _audioDevices = new List<IAudioDevice>();
            var audioDeviceTotal = GetAudioDeviceTotal();
            for (int i = 0; i < audioDeviceTotal; i++)
            {
                _audioDevices.Add(new AudioDevice(GetAudioDeviceName(i), i));
            }
            return _audioDevices;
        }
        private string GetAudioDeviceName(int index)
        {
            return Marshal.PtrToStringAnsi(VsNetSoundRecorderSdk.SoundRecorder_GetSoundRecordName(_handle, index));
        }
        private int GetAudioDeviceTotal()
        {
            return VsNetSoundRecorderSdk.SoundRecorder_GetSoundRecordCount(_handle);
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach (var audioDevice in GetAudioDevices())
                    {
                        audioDevice.Dispose();
                    }
                }
                VsNetSoundRecorderSdk.SoundRecorder_FreeInstance(_handle);
                disposedValue = true;
            }
        }
        ~AudioDeviceManager()
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
