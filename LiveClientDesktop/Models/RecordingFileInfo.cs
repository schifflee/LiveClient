using LiveClientDesktop.Enums;
using System;

namespace LiveClientDesktop.Models
{
    public class RecordingFileInfo
    {
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

        public int ScheduleId { get; set; }

        public double Duration
        {
            get
            {
                if (StartRecordingTime == null) return 0;
                if (StopRecordingTime == null) return 0;
                return (StopRecordingTime - StartRecordingTime).TotalSeconds;
            }
        }

        public int Index { get; set; }
        public VideoType VideoType { get; set; }

        public string Title { get; set; }

        public DateTime StartRecordingTime { get; set; }
        public DateTime StopRecordingTime { get; set; }

        public override string ToString()
        {
            return $"{Title},{VideoType.ToString()},{FileSavePath}/{FileName}";
        }

    }
}
