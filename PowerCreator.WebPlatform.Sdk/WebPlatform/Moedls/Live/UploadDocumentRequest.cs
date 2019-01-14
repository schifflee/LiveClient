using PowerCreatorDotCom.Sdk.Core;
using PowerCreatorDotCom.Sdk.Core.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerCreator.WebPlatform.Sdk.WebPlatform.Moedls
{
    public class UploadDocumentRequest : ServiceRequest<ServiceResponseValue>
    {
        public UploadDocumentRequest(string domain, string liveId, string title, string author, string ext, Stream content)
            : base(domain, null, ControllerNames.LiveController, "DocumentAdd")
        {
            LiveID = liveId;
            Title = title;
            AuthorName = author;
            Extension = ext;
            Method = MethodType.POST;
            SetContent(content, FormatType.MultipartFormData);
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
        private string title;

        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                this.AddQueryParameters("Title", value);
            }
        }
        private string authorName;

        public string AuthorName
        {
            get { return authorName; }
            set
            {
                authorName = value;
                this.AddQueryParameters("AuthorName", value);
            }
        }
        private string extension;

        public string Extension
        {
            get { return extension; }
            set
            {
                extension = value;
                this.AddQueryParameters("Extension", value);
            }
        }
    }
}
