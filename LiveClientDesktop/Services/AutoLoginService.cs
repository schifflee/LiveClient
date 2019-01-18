using PowerCreator.WebPlatform.Sdk.WebPlatform.Moedls;
using PowerCreatorDotCom.Sdk.Core;
using System;
using System.Net;
using System.Net.Sockets;

namespace LiveClientDesktop.Services
{
    public class AutoLoginService : ILoginService
    {
        private readonly IServiceClient _serviceClient;
        private readonly WebPlatformApiFactory _webPlatformApi;
        private readonly StartupParameters _startupParameters;
        public AutoLoginService(IServiceClient serviceClient, WebPlatformApiFactory webPlatformApi, StartupParameters startupParameters)
        {
            _serviceClient = serviceClient;
            _webPlatformApi = webPlatformApi;
            _startupParameters = startupParameters;
        }
        public Tuple<bool, string> Login()
        {

            var loginServiceRsp = _serviceClient.GetResponse(_webPlatformApi.CreateLiveClientLoginRequest());
            if (loginServiceRsp == null)
            {
                return ResultWarpper(false, "登录失败");
            }
            if (!loginServiceRsp.Success)
            {
                return ResultWarpper(loginServiceRsp.Success, loginServiceRsp.Message);
            }

            _startupParameters.UserIdentity = loginServiceRsp.HttpResponse.Headers["Set-Cookie"];

            var setClientInfoServiceRsp = _serviceClient.GetResponse(_webPlatformApi.CreateSetVideoClientInfoRequest(GetLocalIP()));
            if (!setClientInfoServiceRsp.Success)
            {

                return ResultWarpper(setClientInfoServiceRsp.Success, setClientInfoServiceRsp.Message);
            }

            return ResultWarpper(loginServiceRsp.Success, loginServiceRsp.Message);
        }

        public ServiceResponseResult<GetLiveInfoResponse> GetLiveInfo()
        {
            return _serviceClient.GetResponse(_webPlatformApi.CreateLiveInfoRequest());
        }
        private Tuple<bool, string> ResultWarpper(bool isSuccess, string message)
        {
            return new Tuple<bool, string>(isSuccess, message);
        }
        private string GetLocalIP()
        {
            string name = Dns.GetHostName();
            IPAddress[] ipadrlist = Dns.GetHostAddresses(name);
            string ip = string.Empty;
            foreach (IPAddress ipa in ipadrlist)
            {
                if (ipa.AddressFamily == AddressFamily.InterNetwork)
                    ip = ipa.ToString();

            }
            return ip;
        }


    }
}
