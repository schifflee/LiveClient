using PowerCreator.LiveClient.Core.Models;
using PowerCreator.LiveClient.Core.VideoDevice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerCreator.LiveClient.Core.VideoEncoder
{
    public interface IVideoEncoder : IPushingDataPushingData<VideoEncodedDataContext>, IDisposable
    {
        bool IsStartEncoder { get; }
        int IntBitmapInfoHeader { get; }
        IVideoDevice CurrentUseVideoDevice { get; }

        bool SetVideoSource(IVideoDevice videoDevice);

        bool StartVideoEncoder();

        bool StopVideoEncoder();
    }
}
