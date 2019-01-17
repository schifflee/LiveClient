using LiveClientDesktop.EventAggregations;
using LiveClientDesktop.Models;
using LiveClientDesktop.ServiceIntefaces;
using PowerCreatorDotCom.Sdk.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace LiveClientDesktop.Services
{
    public class UploadCoursewareService
    {
        private readonly SystemConfig _config;
        private readonly StorageProvider _storageProvider;
        private readonly IServiceClient _serviceClient;
        private readonly WebPlatformApiFactory _webPlatformApiFactory;
        private readonly ICollection<UploadTaskInfo> _taskList;
        private IEnumerable<IStorage> storages;
        public bool Uploading { get; private set; }
        public bool TaskTotal
        {
            get
            {
                return _taskList.Any(item => !item.IsCompleted);
            }
        }

        public event Action<UploadTaskInfo> OnUpload;
        public UploadCoursewareService(SystemConfig config, StorageProvider storageProvider, IServiceClient serviceClient, WebPlatformApiFactory webPlatformApiFactory, EventSubscriptionManager eventSubscriptionManager)
        {
            _config = config;
            _storageProvider = storageProvider;
            _serviceClient = serviceClient;
            _webPlatformApiFactory = webPlatformApiFactory;
            eventSubscriptionManager.Subscribe<RecordCompletedEvent, RecordInfo>(null, AddUploadTask, null);
            _taskList = new List<UploadTaskInfo>();
        }


        private void AddUploadTask(RecordInfo recordInfo)
        {
            UploadTaskInfo uploadTask = new UploadTaskInfo(recordInfo);
            if (_config.IsAutoUpload)
            {
                uploadTask.IsUpload = true;
            }
            _taskList.Add(uploadTask);
            Upload();
        }

        private void Upload()
        {
            if (Uploading) return;
            Uploading = true;
            Task.Run(() =>
            {
                while (true)
                {
                    var uploadTask = _taskList.Where(item => item.IsUpload && !item.IsFail && !item.IsCompleted).FirstOrDefault();
                    if (uploadTask == null)
                    {
                        uploadTask = _taskList.Where(item => item.IsUpload && item.IsFail).OrderBy(item => item.FailedTime).FirstOrDefault();
                    }
                    if (uploadTask == null) break;

                    OnUpload?.Invoke(uploadTask);

                    if (storages == null || !storages.Any())
                    {
                        var result = _storageProvider.GetStorages();
                        if (!result.Item1)
                        {
                            uploadTask.UploadStatus = "无可用存储";
                            break;
                        }
                        storages = result.Item2.Item1;
                    }
                    foreach (var storage in storages)
                    {
                        if (uploadTask.UploadedStorage.ContainsKey(storage.StorageInfo.ID))
                        {
                            if (uploadTask.UploadedStorage[storage.StorageInfo.ID]) continue;
                        }
                        else
                        {
                            uploadTask.UploadedStorage.Add(storage.StorageInfo.ID, false);
                        }
                        storage.UploadFile(uploadTask);
                    }
                    if (!uploadTask.UploadedStorage.Any(item => !item.Value))
                    {
                        var notifyVideoUploadCompletedForm = new NotifyVideoUploadCompletedForm()
                        {
                            RecordID = uploadTask.RecordInfo.ScheduleId,
                            videoIndex = uploadTask.RecordInfo.Index,
                            Duration = (int)uploadTask.RecordInfo.Duration,
                        };
                        foreach (var item in uploadTask.Results)
                        {
                            notifyVideoUploadCompletedForm.Storages.Add(item);
                        }

                        var result = _serviceClient.GetResponse(_webPlatformApiFactory.CreateVideoUploadCompletedRequest(notifyVideoUploadCompletedForm));
                        uploadTask.IsCompleted = result.Success;
                        uploadTask.UploadStatus = "上传完成";
                    }
                    Thread.Sleep(500);
                }

                Uploading = false;
            });
        }

        public void SetTaskIsExecutable(int scheduleId, int index, bool isExecute)
        {
            _taskList.Where(item => item.RecordInfo.Index == index && item.RecordInfo.ScheduleId == scheduleId).ForEach((item) =>
            {
                item.IsUpload = isExecute;
            });
            Upload();
        }
    }
}
