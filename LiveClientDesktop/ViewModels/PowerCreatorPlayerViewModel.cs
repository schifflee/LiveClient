using LiveClientDesktop.EventAggregations;
using LiveClientDesktop.Services;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using PowerCreator.LiveClient.Core;
using PowerCreator.LiveClient.Core.VideoDevice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveClientDesktop.ViewModels
{
    public class PowerCreatorPlayerViewModel
    {
        [Dependency]
        public EventSubscriptionManager EventSubscriptionManager { get; set; }

        [Dependency]
        public IDesktopWindowCollector DesktopWindowCollector { get; set; }
        public ISetupVideoLiveAndRecordingDevices SetupVideoLiveAndRecordingDevices { get; set; }
        public PowerCreatorPlayerViewModel(IUnityContainer container)
        {
            SetupVideoLiveAndRecordingDevices = container.Resolve<TeacherVideoLiveAndRecordProvider>();
        }
    }
}
