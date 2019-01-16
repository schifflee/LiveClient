using LiveClientDesktop.Enums;
using System;
using System.Collections.Generic;

namespace LiveClientDesktop.Models
{
    public class RecordInfo
    {
        public RecordInfo()
        {
            VideoFiles = new List<RecordVideoInfo>();
        }
        public List<RecordVideoInfo> VideoFiles { get; set; }
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

        public string Title { get; set; }

        public DateTime StartRecordingTime { get; set; }
        public DateTime StopRecordingTime { get; set; }

        public override string ToString()
        {
            return $"{Index},{Title}";
        }
    }
}
