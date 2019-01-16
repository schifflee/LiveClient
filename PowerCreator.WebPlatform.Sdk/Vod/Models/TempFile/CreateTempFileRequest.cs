using PowerCreatorDotCom.Sdk.Core;
using PowerCreatorDotCom.Sdk.Core.Http;
using System;

namespace PowerCreator.WebPlatform.Sdk.Vod.Models
{
    public class CreateTempFileRequest : ServiceRequest<CreateTempFileResponse>
    {
        public CreateTempFileRequest(string domain, string fileName)
            : base(domain, null, ControllerNames.TempFile, "Create")
        {
            FileName = fileName;
        }
        private string fileName;

        public string FileName
        {
            get { return fileName; }
            set
            {
                fileName = value;
                this.AddQueryParameters("FileName", value);
            }
        }
        public override ServiceResponseResult<CreateTempFileResponse> GetResponse(string responseContext, FormatType? format)
        {

            try
            {
                var result = JsonToObject<ServiceResponseResult<string>>(responseContext);
                return new ServiceResponseResult<CreateTempFileResponse>
                {
                    Message = result.Message,
                    Success = result.Success,
                    Value = new CreateTempFileResponse
                    {
                        TempFileId = result.Value
                    }
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponseResult<CreateTempFileResponse>
                {
                    Success = false,
                    Message = ex.Message
                };

            }
        }

    }
}
