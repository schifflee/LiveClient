using Microsoft.Practices.Prism.Events;
using PowerCreator.LiveClient.Core.VideoDevice;
using System.Collections.Generic;

namespace LiveClientDesktop.EventAggregations
{
    public class LoadCameraDeviceCompletedEvent : CompositePresentationEvent<List<IVideoDevice>>
    {
    }
}
