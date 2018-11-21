using DirectShowLib;
using PowerCreator.LiveClient.Core.AudioDevice;
using PowerCreator.LiveClient.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerCreator.LiveClient.Core.AudioEncoder
{
    public interface IAacEncoder : IObservable<AudioEncodedDataContext>, IDisposable
    {
        bool IsStartEncoder { get; }

        IntPtr IntPtrWaveFormatEx { get; }

        IAudioDevice CurrentUseAudioDevice { get; }

        int IntWaveFormatEx { get; }
        bool SetAudioDataSource(IAudioDevice audioDevice);

        bool StartAudioEncoder();

        bool StopAudioEncoder();
    }
}
