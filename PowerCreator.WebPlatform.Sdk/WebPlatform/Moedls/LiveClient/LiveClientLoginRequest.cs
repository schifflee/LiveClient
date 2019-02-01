using PowerCreatorDotCom.Sdk.Core;

namespace PowerCreator.WebPlatform.Sdk.WebPlatform.Moedls
{
    public class LiveClientLoginRequest : ServiceRequest<LiveClientLoginResponse>
    {
        public LiveClientLoginRequest(string domain)
            : base(domain, null, ControllerNames.LIVE_CLIENT_LOGIN_CONTROLLER, "ClientLogin.ashx")
        {
        }
        private string accessToken;

        public string AccessToken
        {
            get { return accessToken; }
            set
            {
                accessToken = value;
                this.AddQueryParameters("AccessToken", value);
            }
        }

    }
}
