using LiveClientDesktop.Models;
using PowerCreatorDotCom.Sdk.Core;
using System;
using System.IO;
using System.Linq;

namespace LiveClientDesktop.Services
{
    public class VodStorageInstance : StorageInstance
    {

        private readonly IServiceClient _serviceClient;
        private readonly WebPlatformApiFactory _webPlatformApiFactory;
        private readonly string _endpoint;

        public VodStorageInstance(VodStorageInfo storageInfo, IServiceClient serviceClient, WebPlatformApiFactory webPlatformApiFactory)
            : base(storageInfo)
        {
            _serviceClient = serviceClient;
            _endpoint = storageInfo.Endpoint;
            _webPlatformApiFactory = webPlatformApiFactory;
        }

        protected override void StartUpload(RecordVideoInfo info)
        {

            var tempFileIdResult = GetTempFileId(info);
            if (!tempFileIdResult.Item1)
            {
                uploadTaskInfo.UploadStatus = tempFileIdResult.Item2;
                return;
            }
            info.TempFileID = tempFileIdResult.Item2;
            var appendFileContentResult = AppendFileContent(info);
            if (!appendFileContentResult.Item1)
            {
                uploadTaskInfo.UploadStatus = appendFileContentResult.Item2;
                return;
            }
            MarkFileUploadCompleted(info);
        }

        protected override void EndUpload()
        {
            
            var result = _serviceClient.GetResponse(_webPlatformApiFactory.CreateFromTempFileRequest(
                  _endpoint,
                  uploadTaskInfo.RecordInfo.ScheduleId,
                  uploadTaskInfo.RecordInfo.Index,
                  StorageInfo.Name, uploadTaskInfo.RecordInfo.VideoFiles.Select(item => item.TempFileID).ToArray()));

            if (!result.Success)
            {
                uploadTaskInfo.IsFail = true;
                uploadTaskInfo.FailedTime = DateTime.Now;
                uploadTaskInfo.UploadStatus = result.Message;
                return;
            }
            if (!uploadTaskInfo.Results.Any(item => item.ID == StorageInfo.ID))
            {
                uploadTaskInfo.Results.Add(new CreateFormTempFileResult
                {
                    ID = StorageInfo.ID,
                    Size = result.Value.Size,
                    DownloadFileName = result.Value.DownloadFileName,
                    FolderPath = result.Value.FolderPath,
                    PlayFileName = result.Value.PlayFileName
                });
            }
            MarkUploadCompleted();
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
    }
}
