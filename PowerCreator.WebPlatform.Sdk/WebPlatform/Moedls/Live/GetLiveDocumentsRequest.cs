using PowerCreatorDotCom.Sdk.Core;
using PowerCreatorDotCom.Sdk.Core.Http;
using System;
using System.Collections.Generic;

namespace PowerCreator.WebPlatform.Sdk.WebPlatform.Moedls
{
    public class GetLiveDocumentsRequest : ServiceRequest<GetLiveDocumentsResponse>
    {
        public GetLiveDocumentsRequest(string domain, string liveId)
            : base(domain, null, ControllerNames.LIVE_CONTROLLER, "DocumentList")
        {
            LiveId = liveId;
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
        public override ServiceResponseResult<GetLiveDocumentsResponse> GetResponse(string responseContext, FormatType? format)
        {
            try
            {
                var result = JsonToObject<ServiceResponseResult<List<DocumentInfo>>>(responseContext);
                return new ServiceResponseResult<GetLiveDocumentsResponse>
                {
                    Message = result.Message,
                    Success = result.Success,
                    Value = new GetLiveDocumentsResponse
                    {
                        Documents = result.Value
                    }
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponseResult<GetLiveDocumentsResponse>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}
