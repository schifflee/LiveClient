using System;

namespace PowerCreator.LiveClient.Core
{
    public interface IPushingDataPushingData<EventDataContext>
    {
        event Action<EventDataContext> PushingData;
    }
}
