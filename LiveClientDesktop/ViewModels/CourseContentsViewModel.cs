using LiveClientDesktop.Enums;
using LiveClientDesktop.EventAggregations;
using LiveClientDesktop.Models;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Unity;
using System;
using System.Windows;

namespace LiveClientDesktop.ViewModels
{
    public class CourseContentsViewModel : NotificationObject
    {

        private readonly IEventAggregator _eventAggregator;

        public CourseContentsViewModel(CameraDeviceViewModel cameraDeviceViewModel, IEventAggregator eventAggregator)
        {
            CameraDeviceViewModel = cameraDeviceViewModel;
            CameraDeviceViewModel.SetSelectCameraDevice(0);
            _eventAggregator = eventAggregator;
            SwitchDemonstrationSceneCommand = new DelegateCommand<string>(new Action<string>(SwitchScene));
        }

        [Dependency]
        public PresentationViewModel PresentationViewModel { get; set; }

        public CameraDeviceViewModel CameraDeviceViewModel { get; private set; }

        public DelegateCommand<string> SwitchDemonstrationSceneCommand { get; set; }

        private void SwitchScene(string sceneType)
        {

            SwitchDemonstrationSceneContext context = new SwitchDemonstrationSceneContext() { SceneType = DemonstratioType.None };
            PresentationInfo presentationInfo;
            switch ((SceneType)int.Parse(sceneType))
            {
                case SceneType.PPT:
                    if (PresentationViewModel.CurrentSelectedPresentation == null) {
                        MessageBox.Show("请选择需要演示的文件","系统提示");
                        return;
                    }
                    presentationInfo = PresentationViewModel.CurrentSelectedPresentation.Presentation;
                    context.SceneType = presentationInfo.DemoFileType;
                    context.UseDevice = presentationInfo.FileFullPath;
                    break;
                case SceneType.WarmVideo:
                    if (PresentationViewModel.CurrentSelectedWarmVideo == null) {
                        MessageBox.Show("请选择需要演示的文件", "系统提示");
                        return;
                    }
                    presentationInfo = PresentationViewModel.CurrentSelectedWarmVideo.Presentation;
                    context.SceneType = presentationInfo.DemoFileType;
                    context.UseDevice = presentationInfo.FileFullPath;
                    break;
                case SceneType.VideoDevice:
                    if (CameraDeviceViewModel.CurrentSelectedDevice == null) {
                        MessageBox.Show("请选择需要播放的视频设备", "系统提示");
                        return;
                    }
                    context.SceneType = DemonstratioType.VideoDevice;
                    context.UseDevice = CameraDeviceViewModel.CurrentSelectedDevice.OwnerVideoDevice.OwnerVideoDevice;
                    break;
            }
            _eventAggregator.GetEvent<SwitchDemonstrationSceneEvent>().Publish(context);
        }
    }
}
