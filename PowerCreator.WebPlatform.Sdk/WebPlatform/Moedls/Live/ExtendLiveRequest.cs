using PowerCreatorDotCom.Sdk.Core;

namespace PowerCreator.WebPlatform.Sdk.WebPlatform.Moedls
{
    public class ExtendLiveRequest : ServiceRequest<ServiceResponseValue>
    {
        public ExtendLiveRequest(string domain,string liveId,int minutes)
            : base(domain, null, ControllerNames.LIVE_CONTROLLER, "ExtendLive")
        {
            LiveId = liveId;
            Minutes = minutes;
        }
        private int minutes;

        public int Minutes
        {
            get { return minutes; }
            set
            {
                minutes = value;
                this.AddQueryParameters("Minutes", value);
            }
        }

        private string liveId;

        public string LiveId
        {
            get { return liveId; }
            set
            {
                liveId = value;
                this.AddQueryParameters("LiveId", value);
            }
        }

    }
}
