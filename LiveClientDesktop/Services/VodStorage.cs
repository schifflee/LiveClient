using LiveClientDesktop.Models;
using LiveClientDesktop.ServiceIntefaces;
using PowerCreator.LiveClient.Infrastructure;
using PowerCreatorDotCom.Sdk.Core;
using PowerCreatorDotCom.Sdk.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveClientDesktop.Services
{
    public class VodStorage : IStorage
    {
        public IStorageInfo StorageInfo { get; private set; }

        private readonly IServiceClient _serviceClient;
        private readonly WebPlatformApiFactory _webPlatformApiFactory;
        private readonly string _endpoint;
        private UploadTaskInfo task;

        public VodStorage(VodStorageInfo storageInfo, IServiceClient serviceClient, WebPlatformApiFactory webPlatformApiFactory)
        {
            StorageInfo = storageInfo;
            _serviceClient = serviceClient;
            _endpoint = storageInfo.Endpoint;
            _webPlatformApiFactory = webPlatformApiFactory;
        }
        public void UploadFile(UploadTaskInfo taskInfo)
        {
            task = taskInfo;
            if (task == null) return;
            if (task.RecordInfo == null) return;
            task.TargetStorage = StorageInfo.Name;
            task.UploadStatus="初始化状态...";
            foreach (var item in task.RecordInfo.VideoFiles.Where(f => !f.UploadedCompleted))
            {
                var tempFileIdResult = GetTempFileId(item);
                if (!tempFileIdResult.Item1)
                {
                    task.UploadStatus = tempFileIdResult.Item2;
                    continue;
                }
                item.TempFileID = tempFileIdResult.Item2;
                var appendFileContentResult = AppendFileContent(item);
                if (!appendFileContentResult.Item1)
                {
                    task.UploadStatus = appendFileContentResult.Item2;
                    continue;
                }
                item.UploadedCompleted = true;
            }
            if (task.RecordInfo.VideoFiles.Where(f => !f.UploadedCompleted).Any())
            {
                task.IsFail = true;
                task.FailedTime = DateTime.Now;
                return;
            }
            var result = _serviceClient.GetResponse(_webPlatformApiFactory.CreateFromTempFileRequest(
                  _endpoint,
                  task.RecordInfo.ScheduleId,
                  task.RecordInfo.Index,
                  StorageInfo.Name, task.RecordInfo.VideoFiles.Select(item => item.TempFileID).ToArray()));

            if (!result.Success)
            {
                task.IsFail = true;
                task.FailedTime = DateTime.Now;
                task.UploadStatus = result.Message;
                return;
            }
            if (!task.Results.Any(item => item.ID == StorageInfo.ID))
            {
                task.Results.Add(new CreateFormTempFileResult
                {
                    ID = StorageInfo.ID,
                    Size = result.Value.Size,
                    DownloadFileName = result.Value.DownloadFileName,
                    FolderPath = result.Value.FolderPath,
                    PlayFileName = result.Value.PlayFileName
                });
            }
            if (task.UploadedStorage.ContainsKey(StorageInfo.ID))
            {
                task.UploadedStorage[StorageInfo.ID] = true;
            }
            else
            {
                task.UploadedStorage.Add(StorageInfo.ID, true);
            }
        }
        private Tuple<bool, string> AppendFileContent(RecordVideoInfo info)
        {
            using (var fs = File.Open(Path.Combine(info.FileSavePath, info.FileName), FileMode.Open))
            {
                var appendFileContentRequest = _webPlatformApiFactory.CreateAppendFileContentRequest(_endpoint, info.TempFileID, fs);
                appendFileContentRequest.StreamTransferProgress += StreamTransferProgress;
                var rsp = _serviceClient.GetResponse(appendFileContentRequest);
                return WarpperResult(rsp.Success, rsp.Message);
            }


        }
        private void StreamTransferProgress(object sender, StreamTransferProgressArgs args)
        {
            task.UploadStatus = $"{args.PercentDone}%";
        }
        private Tuple<bool, string> GetTempFileId(RecordVideoInfo info)
        {
            var rsp = _serviceClient.GetResponse(_webPlatformApiFactory.CreateTempFileRequest(_endpoint, info.FileName));
            if (!rsp.Success)
                return WarpperResult(rsp.Success, rsp.Message);

            return WarpperResult(rsp.Success, rsp.Value.TempFileId);
        }
        private Tuple<bool, string> WarpperResult(bool success, string value)
        {
            return new Tuple<bool, string>(success, value);
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
    }
}
