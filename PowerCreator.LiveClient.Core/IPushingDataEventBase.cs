using System;

namespace PowerCreator.LiveClient.Core
{
    public interface IPushingDataPushingData<EventContext>
    {
        event Action<EventContext> PushingData;
    }
}
