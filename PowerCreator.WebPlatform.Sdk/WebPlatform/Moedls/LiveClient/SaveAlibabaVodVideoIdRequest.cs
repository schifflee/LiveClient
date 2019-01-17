using PowerCreatorDotCom.Sdk.Core;

namespace PowerCreator.WebPlatform.Sdk.WebPlatform.Moedls
{
    public class SaveAlibabaVodVideoIdRequest : ServiceRequest<ServiceResponseValue>
    {
        public SaveAlibabaVodVideoIdRequest(string domain, string scheduleId, string videoId, string videoType, string videoIndex)
            : base(domain, null, ControllerNames.LiveClient, "SaveAliYunVodVideoId")
        {
            ScheduleId = scheduleId;
            VideoId = videoId;
            VideoType = videoType;
            VideoIndex = videoIndex;
        }
        private string scheduleId;

        public string ScheduleId
        {
            get { return scheduleId; }
            set
            {
                scheduleId = value;
                this.AddQueryParameters("ScheduleId", value);
            }
        }
        private string videoId;

        public string VideoId
        {
            get { return videoId; }
            set
            {
                videoId = value;
                this.AddQueryParameters("VideoId", value);
            }
        }
        private string videoType;

        public string VideoType
        {
            get { return videoType; }
            set
            {
                videoType = value;
                this.AddQueryParameters("VideoType", value);
            }
        }
        private string videoIndex;

        public string VideoIndex
        {
            get { return videoIndex; }
            set
            {
                videoIndex = value;
                this.AddQueryParameters("VideoIndex", value);
            }
        }


    }
}
