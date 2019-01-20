using LiveClientDesktop.Models;
using Newtonsoft.Json;
using PowerCreator.WebPlatform.Sdk.Vod.Models;
using PowerCreator.WebPlatform.Sdk.WebPlatform.Moedls;
using PowerCreatorDotCom.Sdk.Core.Http;
using System.IO;
using System.Text;

namespace LiveClientDesktop.Services
{
    public class WebPlatformApiFactory
    {
        private readonly StartupParameters _startupParameters;
        public WebPlatformApiFactory(StartupParameters startupParameters)
        {
            _startupParameters = startupParameters;
        }

        public LiveClientLoginRequest CreateLiveClientLoginRequest()
        {
            return new LiveClientLoginRequest(_startupParameters.Domain)
            {
                AccessToken = _startupParameters.Guid
            };
        }
        public SetVideoClientInfoRequest CreateSetVideoClientInfoRequest(string clientInfo)
        {
            return SetRequestCookie(new SetVideoClientInfoRequest(_startupParameters.Domain)
            {
                LiveID = _startupParameters.LiveId,
                ClientInfo = $"{clientInfo}:{_startupParameters.HttpServerPort}"
            });
        }

        public GetLiveInfoRequest CreateLiveInfoRequest()
        {
            return SetRequestCookie(new GetLiveInfoRequest(_startupParameters.Domain)
            {
                LiveID = _startupParameters.LiveId
            });
        }

        public StartLiveRequest CreateStartLiveRequest()
        {
            return SetRequestCookie(new StartLiveRequest(_startupParameters.Domain)
            {
                LiveID = _startupParameters.LiveId
            });
        }
        public PauseLiveRequest CreatePauseLiveRequest()
        {
            return SetRequestCookie(new PauseLiveRequest(_startupParameters.Domain)
            {
                LiveID = _startupParameters.LiveId
            });
        }

        public StopLiveRequest CreateStopLiveRequest()
        {
            return SetRequestCookie(new StopLiveRequest(_startupParameters.Domain)
            {
                LiveID = _startupParameters.LiveId
            });
        }

        public UploadDocumentRequest CreateUploadDocumentRequest(string title, string author, string ext, Stream stream)
        {
            return SetRequestCookie(
                new UploadDocumentRequest(
                    _startupParameters.Domain,
                    _startupParameters.LiveId,
                    title, author, ext, stream));
        }

        public GetLiveDocumentsRequest CreateGetLiveDocumentsRequest()
        {
            return SetRequestCookie(new GetLiveDocumentsRequest(_startupParameters.Domain, _startupParameters.LiveId));
        }
        public DeleteLiveDocumentRequest CreateDeleteLiveDocumentRequest(string documentId)
        {
            return SetRequestCookie(new DeleteLiveDocumentRequest(_startupParameters.Domain, _startupParameters.LiveId, documentId));
        }
        public GetStoragesRequest CreateGetStoragesRequest(int scheduleId)
        {
            return SetRequestCookie(new GetStoragesRequest(_startupParameters.Domain, scheduleId));
        }

        public CreateTempFileRequest CreateTempFileRequest(string domain, string fileName)
        {
            return SetRequestCookie(new CreateTempFileRequest(domain, fileName));
        }
        public AppendFileContentRequest CreateAppendFileContentRequest(string domain, string tempFileId, Stream stream)
        {
            return SetRequestCookie(new AppendFileContentRequest(domain, tempFileId, stream));
        }
        public CreateFromTempFileRequest CreateFromTempFileRequest(string domain, int scheduleId, int videoIndex, string storageName, string[] fileId)
        {
            return SetRequestCookie(new CreateFromTempFileRequest(domain, scheduleId, videoIndex, storageName, fileId));
        }
        public VideoUploadCompletedRequest CreateVideoUploadCompletedRequest(NotifyVideoUploadCompletedForm notifyVideoUploadCompletedForm)
        {
            var buffer = Encoding.GetEncoding("utf-8").GetBytes(JsonConvert.SerializeObject(notifyVideoUploadCompletedForm));
            Stream content = new MemoryStream();
            content.Write(buffer, 0, buffer.Length);
            content.Flush();
            content.Seek(0, SeekOrigin.Begin);
            return SetRequestCookie(new VideoUploadCompletedRequest(_startupParameters.Domain, content));
        }

        public SaveAlibabaVodVideoIdRequest CreateSaveAlibabaVodVideoIdRequest(int scheduleId, string videoId, string videoType, int videoIndex)
        {
            return SetRequestCookie(new SaveAlibabaVodVideoIdRequest(_startupParameters.Domain, scheduleId.ToString(), videoId, videoType, videoIndex.ToString()));
        }

        public ExtendLiveRequest CreateExtendLiveRequest(int min)
        {
            return SetRequestCookie(new ExtendLiveRequest(_startupParameters.Domain, _startupParameters.LiveId, min));
        }
        private T SetRequestCookie<T>(T request) where T : HttpRequest
        {
            request.Headers.Add("Cookie", _startupParameters.UserIdentity);
            return request;
        }
    }
}
