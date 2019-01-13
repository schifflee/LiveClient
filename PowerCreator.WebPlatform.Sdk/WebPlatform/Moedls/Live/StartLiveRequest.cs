using PowerCreatorDotCom.Sdk.Core;
using PowerCreatorDotCom.Sdk.Core.Http;
using System;

namespace PowerCreator.WebPlatform.Sdk.WebPlatform.Moedls
{
    public class StartLiveRequest : ServiceRequest<StartLiveResponse>
    {
        public StartLiveRequest(string domain)
            : base(domain, null, ControllerNames.LiveController, "StartLive")
        {
            Index = new string[] { "0", "1" };
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

        private string[] index;

        public string[] Index
        {
            get { return index; }
            set
            {
                index = value;
                this.AddQueryParameters("Index", value);
            }
        }
        public override ServiceResponseResult<StartLiveResponse> GetResponse(string responseContext, FormatType? format)
        {
            try
            {
                var result = JsonToObject<ServiceResponseResult<string[]>>(responseContext);
                return new ServiceResponseResult<StartLiveResponse>
                {
                    Message = result.Message,
                    Success = result.Success,
                    Value = new StartLiveResponse
                    {
                        StreamInfo = result.Value
                    }
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponseResult<StartLiveResponse>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

    }
}

