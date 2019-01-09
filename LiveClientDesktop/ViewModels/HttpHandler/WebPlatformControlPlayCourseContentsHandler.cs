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

                return Processor(PresentationViewModel.PresentationList,
                    (item) => item.Presentation.ID == dic["sourceID"],
                    (resource) =>
                    {
                        PresentationViewModel.CurrentSelectedPresentation = resource;
                        SwitchScene("0");
                        return HttpRequestHandleResultWarpper.WriteResult(true);
                    });
            }));

            HttpRequestHandlerManager.Instance.AddHandler("PlayStreamVideo", new Func<IDictionary<string, string>, string>((dic) =>
            {

                return Processor(PresentationViewModel.WarmVideoList,
                     (item) => item.Presentation.ID == dic["sourceID"],
                     (resource) =>
                     {
                         PresentationViewModel.CurrentSelectedWarmVideo = resource;
                         SwitchScene("1");
                         return HttpRequestHandleResultWarpper.WriteResult(true);
                     });
            }));

            HttpRequestHandlerManager.Instance.AddHandler("PlayCameraVideo", new Func<IDictionary<string, string>, string>((dic) =>
            {
                return Processor(CameraDeviceViewModel.CameraDeviceList,
                    (item) => item.OwnerVideoDevice.ID == int.Parse(dic["sourceID"]),
                    (resource) =>
                    {
                        CameraDeviceViewModel.CurrentSelectedDevice = resource;
                        SwitchScene("2");
                        return HttpRequestHandleResultWarpper.WriteResult(true);
                    });
            }));
        }
        private string Processor<T>(List<T> source, Func<T, bool> where, Func<T, string> callback)
        {
            T t = source.Where(where).FirstOrDefault();
            if (t == null) return HttpRequestHandleResultWarpper.WriteResult(false, "Resource not found.");
            return callback.Invoke(t);
        }
    }
}
