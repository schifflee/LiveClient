using LiveClientDesktop.Models;
using System.Collections.Generic;

namespace LiveClientDesktop.ViewModels
{
    public class SettingsViewModel
    {
        private readonly SystemConfig _config;

        public List<ResolutionInfo> ResolutionInfoList { get; set; }
        public List<RateInfo> RateInfoList { get; set; }
        public List<FrameRateInfo> FrameRateInfoList { get; set; }

        public SettingsViewModel(SystemConfig config)
        {
            _config = config;
            //ResolutionInfoList = resolutionInfoList;
        }
        public void Initialize()
        {
            ResolutionInfoList = new List<ResolutionInfo> {
                new ResolutionInfo{ID=1,DisplayName="1280*720",Width=1280,Height=720 },
                new ResolutionInfo{ ID=2,DisplayName="960*540",Width=960,Height=540},
                new ResolutionInfo{ ID=2,DisplayName="640*360",Width=640,Height=360},
                new ResolutionInfo{ ID=2,DisplayName="480*270",Width=480,Height=270},
            };
        }
    }
}
