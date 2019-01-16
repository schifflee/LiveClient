using System;
using System.Collections.Generic;

namespace LiveClientDesktop.Models
{
    public class UploadTaskInfo
    {
        public UploadTaskInfo(RecordInfo recordInfo)
        {
            RecordInfo = recordInfo;
            UploadedStorage = new Dictionary<int, bool>();
            Results = new List<CreateFormTempFileResult>();
        }
        public RecordInfo RecordInfo { get; set; }

        public bool IsFail { get; set; }
        public DateTime FailedTime { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsUpload { get; set; }

        public Dictionary<int, bool> UploadedStorage { get; set; }
        public List<CreateFormTempFileResult> Results { get; set; }

        private string uploadStatus;
        public string UploadStatus
        {
            get
            {
                return uploadStatus;
            }
            set {

                uploadStatus = value;
                onUploadProgressChange?.Invoke(value);
            }
        }
        private string targetSotrage;
        public string TargetStorage
        {
            get
            {
                return targetSotrage;
            }
            set
            {
                targetSotrage = value;
                onStorageChange?.Invoke(value);
            }
        }

        private event Action<string> onUploadProgressChange;
        public event Action<string> OnUploadProgressChange
        {
            add
            {
                onUploadProgressChange -= value;
                onUploadProgressChange += value;
            }
            remove
            {
                onUploadProgressChange -= value;
            }
        }
        private event Action<string> onStorageChange;
        public event Action<string> OnStorageChange
        {
            add
            {
                onStorageChange -= value;
                onStorageChange += value;
            }
            remove
            {
                onStorageChange -= value;
            }
        }
    }
}
