using PowerCreatorDotCom.Sdk.Core;
using PowerCreatorDotCom.Sdk.Core.Http;
using System;
using System.Collections.Generic;

namespace PowerCreator.WebPlatform.Sdk.WebPlatform.Moedls
{
    public class GetStoragesRequest : ServiceRequest<GetStoragesResponse>
    {
        public GetStoragesRequest(string domain,int scheduleId)
            : base(domain, null, ControllerNames.LiveClient, "GetStorages")
        {
            RecordId = scheduleId;
        }
        private int recordId;

        public int RecordId
        {
            get { return recordId; }
            set
            {
                recordId = value;
                this.AddQueryParameters("RecordId", value);
            }
        }
        public override ServiceResponseResult<GetStoragesResponse> GetResponse(string responseContext, FormatType? format)
        {
            try
            {
                var result = JsonToObject<ServiceResponseResult<List<StorageInfo>>>(responseContext);
                return new ServiceResponseResult<GetStoragesResponse>
                {
                    Message = result.Message,
                    Success = result.Success,
                    Value = new GetStoragesResponse
                    {
                        Storages = result.Value
                    }
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponseResult<GetStoragesResponse>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

    }
}
