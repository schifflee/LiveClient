using PowerCreator.LiveClient.Core.AudioDevice;
using PowerCreator.LiveClient.Core.VideoDevice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerCreator.LiveClient.Core
{
    public interface ISetupVideoLiveAndRecordingDevices
    {
        bool SetVideoDevice(IVideoDevice videoDevice);
        bool SetAudioDevice(IAudioDevice audioDevice);
    }
}
