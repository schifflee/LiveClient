using Aliyun.Acs.vod.Model.V20170321;

namespace LiveClientDesktop.Models
{
    public class CreateUploadVideoResponseResult
    {
        public CreateUploadVideoResponse CreateUploadVideoResponse { get; set; }
        public UploadAddressInfo UploadAddressInfo { get; set; }

        public UploadAuth UploadAuth { get; set; }

    }
    public class UploadAddressInfo
    {
        public string Endpoint { get; set; }

        public string Bucket { get; set; }

        public string FileName { get; set; }
    }

    public class UploadAuth
    {
        public string AccessKeyId { get; set; }

        public string AccessKeySecret { get; set; }

        public string SecurityToken { get; set; }
    }

}
