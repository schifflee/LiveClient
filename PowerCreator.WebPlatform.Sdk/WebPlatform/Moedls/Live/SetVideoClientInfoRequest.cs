using PowerCreatorDotCom.Sdk.Core;

namespace PowerCreator.WebPlatform.Sdk.WebPlatform.Moedls
{
    public class SetVideoClientInfoRequest : ServiceRequest<SetVideoClientInfoResponse>
    {
        public SetVideoClientInfoRequest(string domain)
            : base(domain, null, ControllerNames.LiveController, "SetVideoClientInfo")
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

        private string clientInfo;

        public string ClientInfo
        {
            get { return clientInfo; }
            set
            {
                clientInfo = value;
                this.AddQueryParameters("ClientInfo", value);
            }
        }
    }
}
