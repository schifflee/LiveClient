using PowerCreatorDotCom.Sdk.Core;

namespace PowerCreator.WebPlatform.Sdk.WebPlatform.Moedls
{
    public class DeleteLiveDocumentRequest : ServiceRequest<ServiceResponseValue>
    {
        public DeleteLiveDocumentRequest(string domain, string liveId,string documentId)
            : base(domain, null, ControllerNames.LiveController, "DocumentDel")
        {
            LiveId = liveId;
            DocumentID = documentId;
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
        private string documentID;

        public string DocumentID
        {
            get { return documentID; }
            set
            {
                documentID = value;
                this.AddQueryParameters("DocumentID", value);
            }
        }


    }
}
