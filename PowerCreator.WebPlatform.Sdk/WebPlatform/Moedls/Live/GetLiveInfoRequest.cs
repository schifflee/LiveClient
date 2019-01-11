using PowerCreatorDotCom.Sdk.Core;

namespace PowerCreator.WebPlatform.Sdk.WebPlatform.Moedls
{
    public class GetLiveInfoRequest : ServiceRequest<GetLiveInfoResponse>
    {
        public GetLiveInfoRequest(string domain)
            : base(domain, null, ControllerNames.LiveController, "GetLiveInfo")
        {

        }
        private int liveId;

        public int LiveID
        {
            get { return liveId; }
            set
            {
                liveId = value;
                this.AddQueryParameters("LiveID", value);
            }
        }
    }
}
