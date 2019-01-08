using PowerCreator.LiveClient.Core.Enums;
using System;

namespace PowerCreator.LiveClient.Core
{
    public interface IVideoLiveProvider
    {
        RecAndLiveState LiveState { get; }
        Tuple<bool, string> StartLiving();
        Tuple<bool, string> PauseLiving();
        Tuple<bool, string> StopLiving();
    }
}
