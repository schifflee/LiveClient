using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerCreator.LiveClient.Core.AudioDevice
{
    public interface IAudioDeviceManager : IDisposable
    {
        IEnumerable<IAudioDevice> GetAudioDevices();

        IAudioDevice GetAudioDeviceById(int id);
    }
}
