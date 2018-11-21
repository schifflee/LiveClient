using PowerCreator.LiveClient.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerCreator.LiveClient.Core.AudioDevice
{
    public interface IAudioDevice : IObservable<AudioDeviceDataContext>, IDisposable
    {
        int ID { get; }
        string Name { get; }
        bool IsOpen { get; }
        IntPtr AudioDataFormat { get; }
        bool OpenDevice();

        bool CloseDevice();
    }
}
