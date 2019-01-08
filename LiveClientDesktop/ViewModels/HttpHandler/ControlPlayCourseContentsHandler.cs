using LiveClientDesktop.HttpRequestHandler;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LiveClientDesktop.ViewModels
{
    public partial class CourseContentsViewModel
    {
        public CourseContentsViewModel()
        {
            HttpRequestHandlerManager.Instance.AddHandler("PlayScreenVideo", new Func<IDictionary<string, string>, string>((dic) =>
            {
                string msg = string.Empty;

                var presentation = PresentationViewModel.PresentationList.Where(item => item.Presentation.ID == dic["sourceID"]).First();
                if (presentation == null)
                    return HttpRequestHandleResultWarpper.WriteResult(false, "sourceID invalid.");

                PresentationViewModel.CurrentSelectedPresentation = presentation;
                SwitchScene("0");
                return HttpRequestHandleResultWarpper.WriteResult(true);

            }));

            HttpRequestHandlerManager.Instance.AddHandler("PlayStreamVideo", new Func<IDictionary<string, string>, string>((dic) =>
            {
                string msg = string.Empty;

                var warmVideo = PresentationViewModel.WarmVideoList.Where(item => item.Presentation.ID == dic["sourceID"]).First();
                if (warmVideo == null)
                    return HttpRequestHandleResultWarpper.WriteResult(false, "sourceID invalid.");

                PresentationViewModel.CurrentSelectedWarmVideo = warmVideo;
                SwitchScene("1");
                return HttpRequestHandleResultWarpper.WriteResult(true);

            }));

            HttpRequestHandlerManager.Instance.AddHandler("PlayCameraVideo", new Func<IDictionary<string, string>, string>((dic) =>
            {
                string msg = string.Empty;

                var videoDevice = CameraDeviceViewModel.CameraDeviceList.Where(item => item.OwnerVideoDevice.ID == int.Parse(dic["sourceID"])).First();
                if (videoDevice == null)
                    return HttpRequestHandleResultWarpper.WriteResult(false, "sourceID invalid.");

                CameraDeviceViewModel.CurrentSelectedDevice = videoDevice;
                SwitchScene("2");
                return HttpRequestHandleResultWarpper.WriteResult(true);

            }));
        }
    }
}
