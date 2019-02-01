using PowerCreatorDotCom.Sdk.Core;

namespace PowerCreator.WebPlatform.Sdk.WebPlatform.Moedls
{
    public class PauseLiveRequest : ServiceRequest<PauseLiveResponse>
    {
        public PauseLiveRequest(string domain)
            : base(domain, null, ControllerNames.LIVE_CONTROLLER, "PauseLive")
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
