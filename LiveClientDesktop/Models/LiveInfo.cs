using PowerCreator.WebPlatform.Sdk.WebPlatform.Moedls;
using System;
using System.Collections.Generic;

namespace LiveClientDesktop.Models
{
    public class LiveInfo
    {
        public int LiveID { get; set; }

        public int ScheduleID { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime StopTime { get; set; }

        public string Title { get; set; }

        public List<TeacherInfo> TeacherList { get; set; }
    }
}
