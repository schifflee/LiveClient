using PowerCreatorDotCom.Sdk.Core;
using PowerCreatorDotCom.Sdk.Core.Http;
using System;
using System.Collections.Generic;

namespace PowerCreator.WebPlatform.Sdk.WebPlatform.Moedls.Storage
{
    public class GetStorageRequest : ServiceRequest<GetStoragesResponse>
    {
        public GetStorageRequest(string domain,string classroomId)
            : base(domain, null, ControllerNames.STORAGE_CONTROLLER, "getStorage.ashx")
        {
            this.classroomID = classroomId;
        }
        private string classroomID;

        public string ClassroomID
        {
            get { return classroomID; }
            set
            {
                classroomID = value;
                this.AddQueryParameters("ClassroomID", value);
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
