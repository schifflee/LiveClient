using LiveClientDesktop.HttpRequestHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveClientDesktop.ViewModels
{
    public partial class RecordingControlViewModel
    {
        public RecordingControlViewModel()
        {

            HttpRequestHandlerManager.Instance.AddHandler("StartRecord", new Func<IDictionary<string, string>, string>((dic) =>
            {
                string msg = string.Empty;
                Tuple<bool, string> result = _StartRecording();
                return HttpRequestHandleResultWarpper.WriteResult(result.Item1, result.Item2);
            }));
            HttpRequestHandlerManager.Instance.AddHandler("ResumeRecord", new Func<IDictionary<string, string>, string>((dic) =>
            {
                string msg = string.Empty;
                Tuple<bool, string> result = _StartRecording();
                return HttpRequestHandleResultWarpper.WriteResult(result.Item1, result.Item2);
            }));
            HttpRequestHandlerManager.Instance.AddHandler("PauseRecord", new Func<IDictionary<string, string>, string>((dic) =>
            {
                string msg = string.Empty;
                Tuple<bool, string> result = _PauseRecording();
                return HttpRequestHandleResultWarpper.WriteResult(result.Item1, result.Item2);
            }));

            HttpRequestHandlerManager.Instance.AddHandler("StopRecord", new Func<IDictionary<string, string>, string>((dic) =>
            {
                string msg = string.Empty;
                Tuple<bool, string> result = _StopRecording();
                return HttpRequestHandleResultWarpper.WriteResult(result.Item1, result.Item2);
            }));
        }
    }
}
