using LiveClientDesktop.EventAggregations;
using LiveClientDesktop.Models;
using LiveClientDesktop.Services;
using Microsoft.Practices.Prism.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveClientDesktop.ViewModels
{
    public class UploadCoursewareViewModel : NotificationObject
    {
        private readonly UploadCoursewareService _uploadCoursewareService;

        private ObservableCollection<UploadCoursewareItemViewModel> uploadFiles;

        public ObservableCollection<UploadCoursewareItemViewModel> UploadFiles
        {
            get { return uploadFiles; }
            set
            {
                uploadFiles = value;
                this.RaisePropertyChanged("UploadFiles");
            }
        }

        public UploadCoursewareViewModel(UploadCoursewareService uploadCoursewareService, EventSubscriptionManager eventSubscriptionManager)
        {
            _uploadCoursewareService = uploadCoursewareService;
            eventSubscriptionManager.Subscribe<RecordCompletedEvent, RecordCompletedEventContext>(null, Handler, null);
            UploadFiles = new ObservableCollection<UploadCoursewareItemViewModel>();
        }
        private void Handler(RecordCompletedEventContext context)
        {
            var taskInfo = new UploadTaskInfo();
            taskInfo.UploadFileList.Add(context.Vga);
            taskInfo.UploadFileList.Add(context.Video1);
            _uploadCoursewareService.AddUploadTask(taskInfo);
            UploadFiles.Add(new UploadCoursewareItemViewModel
            {
                Index = context.Vga.Index,
                PercentDone = "待上传",
                TargetVodServer = "",
                Title = context.Vga.Title
            });
        }

    }

    public class UploadCoursewareItemViewModel
    {
        public int Index { get; set; }
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
            }
        }
        public string PercentDone { get; set; }
        public string TargetVodServer { get; set; }
    }

}
