using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveClientDesktop.ViewModels
{
    public class ViewModelContext
    {
        [Dependency]
        public CameraDeviceViewModel CameraDeviceViewModel { get; set; }

        [Dependency]
        public CourseContentsViewModel CourseContentsViewModel { get; set; }

        [Dependency]
        public PowerCreatorPlayerViewModel PowerCreatorPlayerViewModel { get; set; }

        [Dependency]
        public PlayVolumeControlViewModel PlayVolumeControlViewModel { get; set; }

        [Dependency]
        public MicrophoneVolumeControlViewModel MicrophoneVolumeControlViewModel { get; set; }

        [Dependency]
        public SpeechViewModel SpeechViewModel { get; set; }

        [Dependency]
        public LiveControlViewModel LiveControlViewModel { get; set; }

        [Dependency]
        public RecordingControlViewModel RecordingControlViewModel { get; set; }

    }
}
