using LiveClientDesktop.Models;
using LiveClientDesktop.ServiceIntefaces;
using PowerCreator.LiveClient.Infrastructure;
using PowerCreatorDotCom.Sdk.Core.Models;
using System;
using System.IO;
using System.Linq;

namespace LiveClientDesktop.Services
{
    public abstract class StorageInstance : IStorage
    {
        public IStorageInfo StorageInfo { get; private set; }
        protected UploadTaskInfo uploadTaskInfo;
        public StorageInstance(IStorageInfo storageInfo)
        {
            StorageInfo = storageInfo;
        }
        public void UploadFile(UploadTaskInfo taskInfo)
        {
            uploadTaskInfo = taskInfo;
            uploadTaskInfo.UploadStatus = "初始化...";
            Initialize();
            uploadTaskInfo.TargetStorage = StorageInfo.Name;
            foreach (var item in uploadTaskInfo.RecordInfo.VideoFiles.Where(f => !f.UploadedCompleted[StorageInfo.ID]))
            {
                StartUpload(item);
            }
            UploadIsSuccessful();
        }
        protected void StreamTransferProgress(object sender, StreamTransferProgressArgs args)
        {
            uploadTaskInfo.UploadStatus = $"{args.PercentDone}%";
        }
        protected void StreamTransferProgress(object sender, Aliyun.OSS.StreamTransferProgressArgs args)
        {
            uploadTaskInfo.UploadStatus = $"{args.PercentDone}%";
        }
        private void Initialize()
        {
            foreach (var item in uploadTaskInfo.RecordInfo.VideoFiles)
                if (!item.UploadedCompleted.ContainsKey(StorageInfo.ID)) item.UploadedCompleted.Add(StorageInfo.ID, false);

            if (!uploadTaskInfo.UploadedStorage.ContainsKey(StorageInfo.ID)) uploadTaskInfo.UploadedStorage.Add(StorageInfo.ID, false);
        }
        private void UploadIsSuccessful()
        {
            if (uploadTaskInfo.RecordInfo.VideoFiles.Where(f => !f.UploadedCompleted[StorageInfo.ID]).Any())
            {
                uploadTaskInfo.IsFail = true;
                uploadTaskInfo.FailedTime = DateTime.Now;
                return;
            }
            EndUpload();
        }
        private long GetFilesTotalSize(RecordInfo info)
        {
            long totalSize = 0;
            if (info == null) return totalSize;
            foreach (var item in info.VideoFiles)
            {
                totalSize += FileHelper.GetFileSize(Path.Combine(item.FileSavePath, item.FileName));
            }
            return totalSize;
        }
        protected void MarkFileUploadCompleted(RecordVideoInfo info)
        {
            info.UploadedCompleted[StorageInfo.ID] = true;
        }
        protected void MarkUploadCompleted()
        {
            uploadTaskInfo.UploadedStorage[StorageInfo.ID] = true;
        }
        protected abstract void StartUpload(RecordVideoInfo info);
        protected abstract void EndUpload();
    }
}
