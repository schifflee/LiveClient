using LiveClientDesktop.HttpRequestHandler;
using System;
using System.Collections.Generic;

namespace LiveClientDesktop.ViewModels
{
    public partial class RecordingControlViewModel
    {
        public RecordingControlViewModel()
        {

            HttpRequestHandlerManager.Instance.AddHandler("StartRecord", new Func<IDictionary<string, string>, string>((dic) =>
            {
                return Processor(_StartRecording);
            }));
            HttpRequestHandlerManager.Instance.AddHandler("ResumeRecord", new Func<IDictionary<string, string>, string>((dic) =>
            {
                return Processor(_StartRecording);
            }));
            HttpRequestHandlerManager.Instance.AddHandler("PauseRecord", new Func<IDictionary<string, string>, string>((dic) =>
            {
                return Processor(_PauseRecording);
            }));

            HttpRequestHandlerManager.Instance.AddHandler("StopRecord", new Func<IDictionary<string, string>, string>((dic) =>
            {
                return Processor(_StopRecording);
            }));
        }
        private string Processor(Func<Tuple<bool, string>> handler)
        {
            Tuple<bool, string> result = handler.Invoke();
            return HttpRequestHandleResultWarpper.WriteResult(result.Item1, result.Item2);
        }
    }
}
