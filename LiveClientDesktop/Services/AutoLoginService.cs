using PowerCreator.WebPlatform.Sdk.WebPlatform.Moedls;
using PowerCreatorDotCom.Sdk.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveClientDesktop.Services
{
    public class AutoLoginService
    {
        private readonly IServiceClient _serviceClient;
        private readonly StartupParameters _startupParameters;
        public AutoLoginService(IServiceClient serviceClient, StartupParameters startupParameters)
        {
            _serviceClient = serviceClient;
            _startupParameters = startupParameters;
        }
        public Tuple<bool, string> Login()
        {
            LiveClientLoginRequest liveClientLoginRequest = new LiveClientLoginRequest("miyun.smartclass.cn");
            liveClientLoginRequest.AccessToken = "4d9ed79703004af4929467b68e92bf44";
            var response = _serviceClient.GetResponse(liveClientLoginRequest);
            if (response == null)
            {
                return new Tuple<bool, string>(false, "登录失败.");
            }
            if (response.Success)
            {
                _startupParameters.Cookie = response.HttpResponse.Headers["Set-Cookie"];
            }
            return new Tuple<bool, string>(response.Success, response.Message);
        }
    }
}
