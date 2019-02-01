using PowerCreatorDotCom.Sdk.Core;

namespace PowerCreator.WebPlatform.Sdk.Vod.Models
{
    public class CreateFromTempFileRequest : ServiceRequest<CreateFromTempFileResponse>
    {
        public CreateFromTempFileRequest(string domain, int scheduleId, int videoIndex, string storageName, string[] fileId)
            : base(domain, null, ControllerNames.VOD_VIDEO_CONTROLLER, "CreateFromTempFile")
        {
            RecordId = scheduleId;
            VideoIndex = videoIndex;
            StorageName = storageName;
            FileID = fileId;
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
        private int videoIndex;

        public int VideoIndex
        {
            get { return videoIndex; }
            set
            {
                videoIndex = value;
                this.AddQueryParameters("VideoIndex", value);
            }
        }
        private string storageName;

        public string StorageName
        {
            get { return storageName; }
            set
            {
                storageName = value;
                this.AddQueryParameters("StorageName", value);
            }
        }

        private string[] fileID;

        public string[] FileID
        {
            get { return fileID; }
            set
            {
                fileID = value;
                this.AddQueryParameters("FileID", value);
            }
        }



    }
}
