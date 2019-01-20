using LiveClientDesktop.EventAggregations;
using LiveClientDesktop.Models;
using LiveClientDesktop.Services;
using LiveClientDesktop.StatusReporting;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Unity;
using PowerCreator.WebPlatform.Sdk.WebPlatform.Moedls;
using PowerCreatorDotCom.Sdk.Core;
using System;
using System.Threading;
using System.Web;

namespace LiveClientDesktop.ViewModels
{
    public class ShellViewModel : NotificationObject
    {
        private readonly LiveInfo _liveInfo;
        private readonly IServiceClient _serviceClient;
        private readonly WebPlatformApiFactory _webPlatformApiFactory;
        private readonly SystemConfig _config;
        private readonly Timer _timer;
        private string windowTitle;

        public string WindowTitle
        {
            get { return windowTitle; }
            set
            {
                windowTitle = value;
                this.RaisePropertyChanged("WindowTitle");
            }
        }

        [Dependency]
        public ViewModelContext VMContext { get; set; }

        [Dependency]
        public EventSubscriptionManager EventSubscriptionManager { get; set; }

        [Dependency]
        public LiveStatusReporting LiveStatusReporting { get; set; }
        public ShellViewModel(string title, LiveInfo liveInfo, SystemConfig config, IServiceClient serviceClient, WebPlatformApiFactory webPlatformApiFactory)
        {
            WindowTitle = title;
            _liveInfo = liveInfo;
            _config = config;
            _serviceClient = serviceClient;
            _webPlatformApiFactory = webPlatformApiFactory;
            _timer = new Timer((state) => TimerHandler(), null, (int)(liveInfo.StopTime.AddMinutes(-5) - DateTime.Now).TotalMilliseconds, Timeout.Infinite);
        }
        private void TimerHandler()
        {
            var result = _serviceClient.GetResponse(_webPlatformApiFactory.CreateExtendLiveRequest(_config.AutoDelayDuration));
            if (result.Success)
            {
                var getLiveInfoResult = _serviceClient.GetResponse(_webPlatformApiFactory.CreateLiveInfoRequest());
                if (getLiveInfoResult.Success)
                {
                    SetLiveInfo(getLiveInfoResult.Value);
                    _timer.Change((int)(_liveInfo.StopTime.AddMinutes(-5) - DateTime.Now).TotalMilliseconds, Timeout.Infinite);
                }
            }
        }
        private void SetLiveInfo(GetLiveInfoResponse getLiveInfoResponse)
        {
            _liveInfo.LiveID = getLiveInfoResponse.LiveID;
            _liveInfo.ScheduleID = getLiveInfoResponse.ScheduleID;
            _liveInfo.StartTime = getLiveInfoResponse.StartTime;
            _liveInfo.StopTime = getLiveInfoResponse.StopTime;
            _liveInfo.TeacherList = getLiveInfoResponse.TeacherList;
            _liveInfo.Title = HttpUtility.HtmlDecode(getLiveInfoResponse.Title);
        }
    }
}
