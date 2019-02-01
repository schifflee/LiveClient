using PowerCreatorDotCom.Sdk.Core;

namespace PowerCreator.WebPlatform.Sdk.WebPlatform.Moedls
{
    public class StopLiveRequest : ServiceRequest<StopLiveResponse>
    {
        public StopLiveRequest(string domain) 
            : base(domain, null,ControllerNames.LIVE_CONTROLLER, "StopLive")
        {
        }
        private string liveId;

        public string LiveID
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
