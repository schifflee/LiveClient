using PowerCreatorDotCom.Sdk.Core;
using PowerCreatorDotCom.Sdk.Core.Http;
using System.IO;

namespace PowerCreator.WebPlatform.Sdk.Vod.Models
{
    public class AppendFileContentRequest : ServiceRequest<ServiceResponseValue>
    {
        public AppendFileContentRequest(string domain, string tempFileId, Stream content)
            : base(domain, null, ControllerNames.TEMP_FILE_CONTROLLER, "AppendContent")
        {
            Id = tempFileId;
            Method = MethodType.POST;
            SetContent(content, FormatType.MultipartFormData);
        }
        private string id;

        public string Id
        {
            get { return id; }
            set
            {
                id = value;
                this.AddQueryParameters("Id", value);
            }
        }

    }
}
