using PowerCreatorDotCom.Sdk.Core;

namespace PowerCreator.WebPlatform.Sdk.Vod.Models
{
    public class CreateFromTempFileResponse : ServiceResponse
    {
        public string FolderPath { get; set; }
        public string PlayFileName { get; set; }
        public string DownloadFileName { get; set; }
        public long Size { get; set; }
    }
}
