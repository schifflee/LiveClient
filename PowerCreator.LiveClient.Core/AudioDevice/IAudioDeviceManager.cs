using System;
using System.Collections.Generic;

namespace PowerCreator.LiveClient.Core.AudioDevice
{
    public interface IAudioDeviceManager : IDisposable
    {
        IEnumerable<IAudioDevice> GetAudioDevices();

        IAudioDevice GetAudioDeviceById(int id);
    }
}
