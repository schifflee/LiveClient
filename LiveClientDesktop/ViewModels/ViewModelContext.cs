﻿using Microsoft.Practices.Unity;

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
        public ClassRoomTeachingViewModel ClassRoomTeachingViewModel { get; set; }

        [Dependency]
        public MicrophoneVolumeControlViewModel MicrophoneVolumeControlViewModel { get; set; }

        [Dependency]
        public SpeechViewModel SpeechViewModel { get; set; }

        [Dependency]
        public LiveControlViewModel LiveControlViewModel { get; set; }

        [Dependency]
        public RecordingControlViewModel RecordingControlViewModel { get; set; }

        [Dependency]
        public RecordingTimeViewModel RecordingTimeViewModel { get; set; }

        [Dependency]
        public LivingTimeViewModel LivingTimeViewModel { get; set; }

        [Dependency]
        public WelcomeViewModel WelcomeViewModel  { get; set; }

        [Dependency]
        public UploadDocumentViewModel UploadDocumentViewModel { get; set; }

        [Dependency]
        public UploadCoursewareViewModel UploadCoursewareViewModel { get; set; }

        [Dependency]
        public SettingsViewModel SettingsViewModel { get; set; }

    }
}
