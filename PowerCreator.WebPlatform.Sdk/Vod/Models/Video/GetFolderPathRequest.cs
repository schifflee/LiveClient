using PowerCreatorDotCom.Sdk.Core;

namespace PowerCreator.WebPlatform.Sdk.Vod.Models.Video
{
    public class GetFolderPathRequest : ServiceRequest<ServiceResponseValue>
    {
        public GetFolderPathRequest(string domain)
            : base(domain, null, "", "")
        {
        }
    }
}
