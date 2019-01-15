using PowerCreator.WebPlatform.Sdk.WebPlatform.Moedls;
using PowerCreatorDotCom.Sdk.Core.Http;
using System.IO;

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

        private T SetRequestCookie<T>(T request) where T : HttpRequest
        {
            request.Headers.Add("Cookie", _startupParameters.UserIdentity);
            return request;
        }
    }
}
