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
    public partial class CourseContentsViewModel : NotificationObject
    {

        private readonly IEventAggregator _eventAggregator;
        private readonly EventSubscriptionManager _eventSubscriptionManager;

        public CourseContentsViewModel(CameraDeviceViewModel cameraDeviceViewModel, IEventAggregator eventAggregator, EventSubscriptionManager eventSubscriptionManager) : this()
        {
            CameraDeviceViewModel = cameraDeviceViewModel;
            CameraDeviceViewModel.SetSelectCameraDevice(0);
            _eventAggregator = eventAggregator;
            _eventSubscriptionManager = eventSubscriptionManager;
            SwitchDemonstrationSceneCommand = new DelegateCommand<string>(new Action<string>(SwitchScene));
            OpenPreviewWindow = new DelegateCommand(() =>
            {
                _eventAggregator.GetEvent<OpenPrevireWindowEvent>().Publish(true);
            });

            _eventSubscriptionManager.Subscribe<SelectedDemonstrationWindowEvent, PreviewWindowInfo>(null, SelectedDemonstrationWindowEventHandler, null);

        }

        [Dependency]
        public PresentationViewModel PresentationViewModel { get; set; }

        public CameraDeviceViewModel CameraDeviceViewModel { get; private set; }

        public DelegateCommand<string> SwitchDemonstrationSceneCommand { get; set; }

        public DelegateCommand OpenPreviewWindow { get; set; }

        private PreviewWindowInfo selectedPreviewWindowInfo;

        public PreviewWindowInfo SelectedPreviewWindowInfo
        {
            get { return selectedPreviewWindowInfo; }
            set
            {
                selectedPreviewWindowInfo = value;
                this.RaisePropertyChanged("SelectedPreviewWindowInfo");
            }
        }

        private void SelectedDemonstrationWindowEventHandler(PreviewWindowInfo previewWindowInfo)
        {
            SelectedPreviewWindowInfo = previewWindowInfo;
        }
        private void SwitchScene(string sceneType)
        {
            SwitchDemonstrationSceneContext context = new SwitchDemonstrationSceneContext() { SceneType = DemonstratioType.None };
            PresentationInfo presentationInfo;
            switch ((SceneType)int.Parse(sceneType))
            {
                case SceneType.PPT:
                    if (PresentationViewModel.CurrentSelectedPresentation == null)
                    {
                        MessageBox.Show("请选择需要演示的文件", "系统提示");
                        return;
                    }
                    presentationInfo = PresentationViewModel.CurrentSelectedPresentation.Presentation;
                    context.SceneType = presentationInfo.DemoFileType;
                    context.UseDevice = presentationInfo.FileFullPath;
                    break;
                case SceneType.WarmVideo:
                    if (PresentationViewModel.CurrentSelectedWarmVideo == null)
                    {
                        MessageBox.Show("请选择需要演示的文件", "系统提示");
                        return;
                    }
                    presentationInfo = PresentationViewModel.CurrentSelectedWarmVideo.Presentation;
                    context.SceneType = presentationInfo.DemoFileType;
                    context.UseDevice = presentationInfo.FileFullPath;
                    break;
                case SceneType.VideoDevice:
                    if (CameraDeviceViewModel.CurrentSelectedDevice == null)
                    {
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
