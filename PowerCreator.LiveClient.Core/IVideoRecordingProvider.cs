using PowerCreator.LiveClient.Core.Enums;
using System;

namespace PowerCreator.LiveClient.Core
{
    public interface IVideoRecordingProvider
    {
        RecAndLiveState RecordState { get; }
        Tuple<bool, string> StartRecording(string recFileSavePath, string fileName);
        Tuple<bool, string> PauseRecording();
        Tuple<bool, string> StopRecording();

        void Dispose();
    }
}
