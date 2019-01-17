using LiveClientDesktop.Enums;
using System.Collections.Generic;

namespace LiveClientDesktop.Models
{
    public class RecordVideoInfo
    {
        public RecordVideoInfo()
        {
            UploadedCompleted = new Dictionary<int, bool>();
        }
        private string fileName;
        public string FileName
        {
            get
            {
                return string.Format("{0}{1}", VideoType, fileName);
            }
            set
            {
                fileName = value;
            }
        }
        public string FileSavePath { get; set; }

        public VideoType VideoType { get; set; }

        public string TempFileID { get; set; }

        public Dictionary<int, bool> UploadedCompleted { get; set; }
        public override string ToString()
        {
            return $"{VideoType.ToString()},{FileSavePath}\\{FileName}";
        }
    }
}
