using PowerCreatorDotCom.Sdk.Core;
using System;
using System.Collections.Generic;

namespace PowerCreator.WebPlatform.Sdk.WebPlatform.Moedls
{
    public class GetLiveInfoResponse : ServiceResponse
    {
        public int LiveID { get; set; }

        public int ScheduleID { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime StopTime { get; set; }
        public string Title { get; set; }

        public List<TeacherInfo> TeacherList { get; set; }
    }
}
