using LiveClientDesktop.Enums;
using LiveClientDesktop.Models;
using LiveClientDesktop.Services;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using PowerCreator.LiveClient.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LiveClientDesktop.ViewModels
{
    public class PresentationViewModel : NotificationObject
    {
        protected readonly PresentationsRepository _presentationsRepository;

        public DelegateCommand OpenSelectPresentationFileWindow { get; set; }
        public DelegateCommand OpenSelectWarmVideoFileWindow { get; set; }

        private List<PresentationItemViewModel> presentationList;

        public List<PresentationItemViewModel> PresentationList
        {
            get { return presentationList; }
            set
            {
                presentationList = value;
                this.RaisePropertyChanged("PresentationList");
            }
        }
        private List<PresentationItemViewModel> warmVideoList;

        public List<PresentationItemViewModel> WarmVideoList
        {
            get { return warmVideoList; }
            set
            {
                warmVideoList = value;
                this.RaisePropertyChanged("WarmVideoList");
            }
        }

        private PresentationItemViewModel currentSelectedWarmVideo;

        public PresentationItemViewModel CurrentSelectedWarmVideo
        {
            get { return currentSelectedWarmVideo; }
            set
            {
                currentSelectedWarmVideo = value;
                this.RaisePropertyChanged("CurrentSelectedWarmVideo");
            }
        }

        private PresentationItemViewModel currentSelectedPresentation;

        public PresentationItemViewModel CurrentSelectedPresentation
        {
            get { return currentSelectedPresentation; }
            set
            {
                currentSelectedPresentation = value;
                this.RaisePropertyChanged("CurrentSelectedPresentation");
            }
        }

        public PresentationViewModel(PresentationsRepository presentationsRepository)
        {
            _presentationsRepository = presentationsRepository ?? throw new ArgumentNullException("presentationsRepository");
            OpenSelectPresentationFileWindow = new DelegateCommand(OpenSelectPresentationFileDialog);
            OpenSelectWarmVideoFileWindow = new DelegateCommand(OpenSelectWarmVideoFileDialog);

            LoadPresentationItem();
            LoadWarmVideoItem();

            SetSelectedPresentationItem(PresentationList, (presentation) =>
            {
                CurrentSelectedPresentation = presentation;
            });
            SetSelectedPresentationItem(WarmVideoList, (presentation) =>
            {
                CurrentSelectedWarmVideo = presentation;
            });
        }
        private void LoadPresentationItem()
        {
            PresentationList = LoadPresentationItem(DemoType.Presentation);
        }
        private void LoadWarmVideoItem()
        {
            WarmVideoList = LoadPresentationItem(DemoType.WarmVideo);
        }
        private void SetSelectedPresentationItem(List<PresentationItemViewModel> sourceList, Action<PresentationItemViewModel> callback, Func<PresentationItemViewModel, bool> where = null)
        {
            if (sourceList.Any())
            {
                PresentationItemViewModel presentation = sourceList.First();
                if (where != null)
                {
                    presentation = sourceList.First(where);
                }
                callback.Invoke(presentation);
            }

        }

        private List<PresentationItemViewModel> LoadPresentationItem(DemoType demoType)
        {
            var allPresentations = _presentationsRepository.GetAllPresentations();
            if (allPresentations.Any())
            {
                return allPresentations.Where(item => item.DemoType == demoType).Select(item => new PresentationItemViewModel { Presentation = item }).ToList();
            }
            return new List<PresentationItemViewModel>();
        }

        private void OpenSelectWarmVideoFileDialog()
        {
            OpenSelectFileDialog(DemoType.WarmVideo, "Video File(*.avi;*.mp4;*.mkv;*.wav;*.flv;*.asf;*.ppt;*.pptx;*.png;*.jpg;*.bmp;*.mp3;*.m4a;)|*.avi;*.mp4;*.mkv;*.wav;*.flv;*.asf;*.ppt;*.pptx;*.png;*.jpg;*.bmp;*.mp3;*.m4a",
                (selectedFileFullName) =>
                {
                    LoadWarmVideoItem();
                    SetSelectedPresentationItem(WarmVideoList, (presentation) =>
                    {
                        CurrentSelectedWarmVideo = presentation;

                    }, (item) => item.Presentation.FileFullPath == selectedFileFullName);
                });
        }
        private void OpenSelectPresentationFileDialog()
        {
            OpenSelectFileDialog(DemoType.Presentation, "PPT File(*.ppt;*.pptx;*.avi;*.mp4;*.mkv;*.wav;*.flv;*.asf;)|*.ppt;*.pptx;*.avi;*.mp4;*.mkv;*.wav;*.flv;*.asf;",
                (selectedFileFullName) =>
                {
                    LoadPresentationItem();
                    SetSelectedPresentationItem(PresentationList, (presentation) =>
                    {
                        CurrentSelectedPresentation = presentation;

                    }, (item) => item.Presentation.FileFullPath == selectedFileFullName);
                });
        }
        private void OpenSelectFileDialog(DemoType demoType, string fileFilter, Action<string> callback)
        {
            var selectedFileName = OpenFileDialogHelper.OpenFileDialogWindow(fileFilter);
            if (string.IsNullOrEmpty(selectedFileName)) return;
            _presentationsRepository.AddPersentation(new PresentationInfo
            {
                FileFullPath = selectedFileName,
                CreateTime = DateTime.Now,
                DemoType = demoType
            });
            callback?.Invoke(selectedFileName);
        }
    }
}
