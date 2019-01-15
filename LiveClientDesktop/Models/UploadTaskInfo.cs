using System;
using System.Collections.Generic;

namespace LiveClientDesktop.Models
{
    public class UploadTaskInfo
    {
        public List<RecordingFileInfo> UploadFileList { get; set; }
        public bool IsFail { get; set; }
        public DateTime FailedTime { get; set; }
        public bool Completed { get; set; }
        public int IsUpload { get; set; }
        public UploadTaskInfo()
        {
            UploadFileList = new List<RecordingFileInfo>();
        }
    }
}
