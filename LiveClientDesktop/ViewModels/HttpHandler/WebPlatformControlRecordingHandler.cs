using LiveClientDesktop.HttpRequestHandler;
using System;
using System.Collections.Generic;

namespace LiveClientDesktop.ViewModels
{
    public partial class RecordingControlViewModel
    {
        public RecordingControlViewModel()
        {

            HttpRequestHandleManager.Instance.AddHandler("StartRecord", new Func<IDictionary<string, string>, string>((dic) =>
            {
                return Processor(_StartRecording);
            }));
            HttpRequestHandleManager.Instance.AddHandler("ResumeRecord", new Func<IDictionary<string, string>, string>((dic) =>
            {
                return Processor(_StartRecording);
            }));
            HttpRequestHandleManager.Instance.AddHandler("PauseRecord", new Func<IDictionary<string, string>, string>((dic) =>
            {
                return Processor(_PauseRecording);
            }));

            HttpRequestHandleManager.Instance.AddHandler("StopRecord", new Func<IDictionary<string, string>, string>((dic) =>
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
