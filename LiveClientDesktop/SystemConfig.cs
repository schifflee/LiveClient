using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveClientDesktop
{
    public class SystemConfig
    {
        public readonly string AllDataSavePath;
        public readonly string RecFileSavePath;
        public readonly string TempImageSavePath;
        public SystemConfig()
        {
            AllDataSavePath = AppDomain.CurrentDomain.BaseDirectory + "Data";
            RecFileSavePath = AppDomain.CurrentDomain.BaseDirectory + "RecFiles";
            TempImageSavePath = AppDomain.CurrentDomain.BaseDirectory + "TempWindowImages";
            VideoVolume = 30;
            MicrophoneVolume = 50;
            UseMicrophoneID = 0;
        }

        public int UseMicrophoneID { get; set; }
        public int VideoVolume { get; set; }

        public int MicrophoneVolume { get; set; }
    }
}
