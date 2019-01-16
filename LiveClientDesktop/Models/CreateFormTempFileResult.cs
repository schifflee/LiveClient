namespace LiveClientDesktop.Models
{
    public class CreateFormTempFileResult
    {
        public int ID { get; set; }
        public string FolderPath { get; set; }

        public string PlayFileName { get; set; }

        public string DownloadFileName { get; set; }
        public long Size { get; set; }
    }
}
