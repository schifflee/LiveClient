using LiveClientDesktop.Models;
using PowerCreatorDotCom.Sdk.Core;
using PowerCreatorDotCom.Sdk.Core.Models;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using PowerCreator.WebPlatform.Sdk.WebPlatform.Moedls;

namespace LiveClientDesktop.Services
{
    public class DocumentService
    {
        private readonly IServiceClient _serviceClient;
        private readonly LiveInfo _liveInfo;
        private readonly WebPlatformApiFactory _webPlatformApiFactory;
        public DocumentService(IServiceClient serviceClient, LiveInfo liveInfo, WebPlatformApiFactory webPlatformApiFactory)
        {
            _serviceClient = serviceClient;
            _liveInfo = liveInfo;
            _webPlatformApiFactory = webPlatformApiFactory;
        }
        public Tuple<bool, string> UploadDocument(string localFileFullPath, EventHandler<StreamTransferProgressArgs> streamTransferProgress = null)
        {
            if (!File.Exists(localFileFullPath))
                return ResultWarpper(false, "File not found.");

            using (var fs = File.Open(localFileFullPath, FileMode.Open))
            {
                var request = _webPlatformApiFactory.CreateUploadDocumentRequest(
                    Path.GetFileName(localFileFullPath),
                    _liveInfo.TeacherList.Where(item => item.IsMajor).First().TeacherName,
                    Path.GetExtension(localFileFullPath),
                    fs);

                if (streamTransferProgress != null)
                    request.StreamTransferProgress += streamTransferProgress;

                var rsp = _serviceClient.GetResponse(request);

                return ResultWarpper(rsp.Success, rsp.Message);
            }
        }

        public IEnumerable<DocumentInfo> GetDocuments()
        {
            var rsp = _serviceClient.GetResponse(_webPlatformApiFactory.CreateGetLiveDocumentsRequest());
            if (!rsp.Success)
            {
                return Enumerable.Empty<DocumentInfo>();
            }
            if (rsp.Value == null)
                return Enumerable.Empty<DocumentInfo>();

            return rsp.Value.Documents;
        }

        public Tuple<bool, string> DeleteDocument(int documentId)
        {
            var rsp = _serviceClient.GetResponse(_webPlatformApiFactory.CreateDeleteLiveDocumentRequest(documentId.ToString()));
            return ResultWarpper(rsp.Success, rsp.Message);
        }

        public Tuple<bool, string> ResultWarpper(bool success, string msg)
        {
            return new Tuple<bool, string>(success, msg);
        }
    }
}
