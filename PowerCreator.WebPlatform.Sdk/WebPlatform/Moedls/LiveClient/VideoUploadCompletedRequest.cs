using PowerCreatorDotCom.Sdk.Core;
using PowerCreatorDotCom.Sdk.Core.Http;
using System.IO;

namespace PowerCreator.WebPlatform.Sdk.WebPlatform.Moedls
{
    public class VideoUploadCompletedRequest : ServiceRequest<ServiceResponseValue>
    {
        public VideoUploadCompletedRequest(string domain, Stream postData)
            : base(domain, null, ControllerNames.LiveClient, "VideoUploadCompleted")
        {
            Method = MethodType.POST;
            SetContent(postData, FormatType.FORM);
        }
    }
}
