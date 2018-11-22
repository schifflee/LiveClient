using PowerCreator.LiveClient.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerCreator.LiveClient.Core.LiveBroadcast
{
    public interface ILiveBroadcast : IDisposable
    {
        bool IsLive { get; }
        RecAndLiveState State { get; }

        bool StartLive(string liveServer, int liveServerPort, string liveChannel, string liveStreamName);

        bool StopLive();

        bool PauseLive();

        bool ResumeLive();

    }
}
