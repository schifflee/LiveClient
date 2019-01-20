using LiveClientDesktop.EventAggregations;
using LiveClientDesktop.Models;
using LiveClientDesktop.Services;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace LiveClientDesktop.ViewModels
{
    public class UploadCoursewareViewModel : NotificationObject
    {
        private readonly SystemConfig _config;
        private readonly UploadCoursewareService _uploadCoursewareService;
        private UploadTaskInfo _currentUploadTask;
        private UploadCoursewareItemViewModel _currentUploadItem;

        private ObservableCollection<UploadCoursewareItemViewModel> uploadFiles;

        public DelegateCommand<string> SetUploadCommand { get; set; }
        public ObservableCollection<UploadCoursewareItemViewModel> UploadFiles
        {
            get { return uploadFiles; }
            set
            {
                uploadFiles = value;
                this.RaisePropertyChanged("UploadFiles");
            }
        }
        private Visibility clearTaskBtnVisibility;

        public Visibility ClearTaskBtnVisibility
        {
            get { return clearTaskBtnVisibility; }
            set
            {
                clearTaskBtnVisibility = value;
                this.RaisePropertyChanged("ClearTaskBtnVisibility");
            }
        }

        public void ClearTask()
        {
            _uploadCoursewareService.ClearTask();
            UploadFiles.Clear();
        }

        public UploadCoursewareViewModel(SystemConfig config, UploadCoursewareService uploadCoursewareService, EventSubscriptionManager eventSubscriptionManager)
        {
            _config = config;
            _uploadCoursewareService = uploadCoursewareService;
            eventSubscriptionManager.Subscribe<RecordCompletedEvent, RecordInfo>(null, Handler, null);
            UploadFiles = new ObservableCollection<UploadCoursewareItemViewModel>();
            uploadCoursewareService.OnUpload += UploadCoursewareService_OnUpload;
            SetUploadCommand = new DelegateCommand<string>(new Action<string>(SetUploadByIndex));
            ClearTaskBtnVisibility = Visibility.Hidden;
        }
        public Tuple<bool, bool> GetUploadStatus()
        {
            return new Tuple<bool, bool>(_uploadCoursewareService.Uploading, _uploadCoursewareService.IsThereTask);
        }
        private void SetUploadByIndex(string id)
        {
            if (string.IsNullOrEmpty(id)) return;
            UploadFiles.Where(item => item.Id == id).ForEach((item) =>
            {
                item.IsUpload = !item.IsUpload;
                _uploadCoursewareService.SetTaskIsExecutable(item.ScheduleId, item.Index, item.IsUpload);
            });
        }

        private void UploadCoursewareService_OnUpload(UploadTaskInfo task)
        {
            if (_currentUploadTask != null)
            {
                _currentUploadTask.OnStorageChange -= _currentUploadTask_OnStorageChange;
                _currentUploadTask.OnUploadProgressChange -= _currentUploadTask_OnUploadProgressChange;
            }
            _currentUploadTask = task;
            _currentUploadItem = UploadFiles.FirstOrDefault(item => item.Index == task.RecordInfo.Index && item.ScheduleId == task.RecordInfo.ScheduleId);
            _currentUploadItem.BtnIsEnable = false;
            _currentUploadTask.OnStorageChange += _currentUploadTask_OnStorageChange;
            _currentUploadTask.OnUploadProgressChange += _currentUploadTask_OnUploadProgressChange;
        }

        private void _currentUploadTask_OnUploadProgressChange(string value)
        {
            if (_currentUploadItem != null) _currentUploadItem.PercentDone = value;
        }

        private void _currentUploadTask_OnStorageChange(string storage)
        {
            if (_currentUploadItem != null) _currentUploadItem.TargetVodServer = storage;
        }

        private void Handler(RecordInfo context)
        {
            var uploadTaskView = new UploadCoursewareItemViewModel()
            {
                Index = context.Index,
                ScheduleId = context.ScheduleId,
                PercentDone = "待上传",
                Title = context.Title,
                BtnIsEnable = true,
            };
            uploadTaskView.IsUpload = _config.IsAutoUpload;

            UploadFiles.Add(uploadTaskView);
        }

    }

    public class UploadCoursewareItemViewModel : NotificationObject
    {
        public UploadCoursewareItemViewModel()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public int Index { get; set; }

        public int ScheduleId { get; set; }
        private string title;
        public string Title
        {
            get
            {
                return $"{title}-{Index}";
            }
            set
            {
                title = value;
                this.RaisePropertyChanged("Title");
            }
        }

        private string percentDone;

        public string PercentDone
        {
            get { return percentDone; }
            set
            {
                percentDone = value;
                this.RaisePropertyChanged("PercentDone");
            }
        }

        private string targetVodServer;

        public string TargetVodServer
        {
            get { return targetVodServer; }
            set
            {
                targetVodServer = value;
                this.RaisePropertyChanged("TargetVodServer");
            }
        }

        private bool isUpload;

        public bool IsUpload
        {
            get { return isUpload; }
            set
            {
                isUpload = value;
                BtnContent = isUpload ? "取消" : "上传";
                this.RaisePropertyChanged("IsUpload");
            }
        }
        private string btnContent;

        public string BtnContent
        {
            get { return btnContent; }
            set
            {
                btnContent = value;
                this.RaisePropertyChanged("BtnContent");
            }
        }

        private bool btnIsEnable;

        public bool BtnIsEnable
        {
            get { return btnIsEnable; }
            set
            {
                btnIsEnable = value;
                if (!btnIsEnable) BtnContent = string.Empty;
                this.RaisePropertyChanged("BtnIsEnable");
            }
        }


    }

}
