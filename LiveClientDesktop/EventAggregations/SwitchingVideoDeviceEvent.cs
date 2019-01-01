using LiveClientDesktop.Models;
using Microsoft.Practices.Prism.Events;
using PowerCreator.LiveClient.Core.VideoDevice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveClientDesktop.EventAggregations
{
    public class SwitchingVideoDeviceEvent: CompositePresentationEvent<VideoDeviceEventContext>
    {

    }
}
